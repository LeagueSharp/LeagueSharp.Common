#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Render.cs is part of LeagueSharp.Common.
 
 LeagueSharp.Common is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 LeagueSharp.Common is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with LeagueSharp.Common. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using SharpDX;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;
using Font = SharpDX.Direct3D9.Font;
using Rectangle = SharpDX.Rectangle;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    /// The render class allows you to draw stuff using SharpDX easier.
    /// </summary>
    public static class Render
    {
        /// <summary>
        /// The render objects
        /// </summary>
        private static readonly List<RenderObject> RenderObjects = new List<RenderObject>();

        /// <summary>
        /// The visible render objects.
        /// </summary>
        private static List<RenderObject> _renderVisibleObjects = new List<RenderObject>();

        /// <summary>
        /// <c>true</c> if the thread should be canceled.
        /// </summary>
        private static bool _cancelThread;

        /// <summary>
        /// The render objects lock
        /// </summary>
        private static readonly object RenderObjectsLock = new object();

        /// <summary>
        /// Initializes static members of the <see cref="Render"/> class.
        /// </summary>
        static Render()
        {
            Drawing.OnEndScene += Drawing_OnEndScene;
            Drawing.OnPreReset += DrawingOnOnPreReset;
            Drawing.OnPostReset += DrawingOnOnPostReset;
            Drawing.OnDraw += Drawing_OnDraw;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainOnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnDomainUnload;
            var thread = new Thread(PrepareObjects);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        /// <summary>
        /// Gets the device.
        /// </summary>
        /// <value>The device.</value>
        public static Device Device
        {
            get { return Drawing.Direct3DDevice; }
        }

        /// <summary>
        /// Determines if the point is on the screen.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns><c>true</c> if the point is on the screen, <c>false</c> otherwise.</returns>
        public static bool OnScreen(Vector2 point)
        {
            return point.X > 0 && point.Y > 0 && point.X < Drawing.Width && point.Y < Drawing.Height;
        }

        /// <summary>
        /// Fired when the current domain is unloaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void CurrentDomainOnDomainUnload(object sender, EventArgs eventArgs)
        {
            _cancelThread = true;
            foreach (var renderObject in RenderObjects)
            {
                renderObject.Dispose();
            }
        }

        /// <summary>
        /// Fired after the DirectX device is reset.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void DrawingOnOnPostReset(EventArgs args)
        {
            foreach (var renderObject in RenderObjects)
            {
                renderObject.OnPostReset();
            }
        }

        /// <summary>
        /// Fired before the DirectX device is reset.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void DrawingOnOnPreReset(EventArgs args)
        {
            foreach (var renderObject in RenderObjects)
            {
                renderObject.OnPreReset();
            }
        }

        /// <summary>
        /// Fired when the game is drawn.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Device == null || Device.IsDisposed)
            {
                return;
            }

            foreach (var renderObject in _renderVisibleObjects)
            {
                renderObject.OnDraw();
            }
        }

        /// <summary>
        /// Fired when the scene ends, and everything has been rendered.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (Device == null || Device.IsDisposed)
            {
                return;
            }

            Device.SetRenderState(RenderState.AlphaBlendEnable, true);

            foreach (var renderObject in _renderVisibleObjects)
            {
                renderObject.OnEndScene();
            }
        }

        /// <summary>
        /// Adds the specified layer.
        /// </summary>
        /// <param name="renderObject">The render object.</param>
        /// <param name="layer">The layer.</param>
        /// <returns>RenderObject.</returns>
        public static RenderObject Add(this RenderObject renderObject, float layer = float.MaxValue)
        {
            renderObject.Layer = !layer.Equals(float.MaxValue) ? layer : renderObject.Layer;
            lock (RenderObjectsLock)
            {
                RenderObjects.Add(renderObject);
            }
            return renderObject;
        }

        /// <summary>
        /// Removes the specified render object.
        /// </summary>
        /// <param name="renderObject">The render object.</param>
        public static void Remove(this RenderObject renderObject)
        {
            lock (RenderObjectsLock)
            {
                RenderObjects.Remove(renderObject);
            }
        }

        /// <summary>
        /// Prepares the objects.
        /// </summary>
        private static void PrepareObjects()
        {
            while (!_cancelThread)
            {
                try
                {
                    Thread.Sleep(1);
                    lock (RenderObjectsLock)
                    {
                        _renderVisibleObjects =
                            RenderObjects.Where(obj => obj.Visible && obj.HasValidLayer())
                                .OrderBy(obj => obj.Layer)
                                .ToList();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(@"Cannot prepare RenderObjects for drawing. Ex:" + e);
                }
            }
        }

        /// <summary>
        /// Draws circles.
        /// </summary>
        public class Circle : RenderObject
        {
            /// <summary>
            /// The vertices
            /// </summary>
            private static VertexBuffer _vertices;

            /// <summary>
            /// The vertex elements
            /// </summary>
            private static VertexElement[] _vertexElements;

            /// <summary>
            /// The vertex declaration
            /// </summary>
            private static VertexDeclaration _vertexDeclaration;

            /// <summary>
            /// The sprite effect
            /// </summary>
            private static Effect _effect;

            /// <summary>
            /// The technique
            /// </summary>
            private static EffectHandle _technique;

            /// <summary>
            /// <c>true</c> if this instanced initialized.
            /// </summary>
            private static bool _initialized;

            /// <summary>
            /// The offset
            /// </summary>
            private static Vector3 _offset = new Vector3(0, 0, 0);

            /// <summary>
            /// Initializes a new instance of the <see cref="Circle"/> class.
            /// </summary>
            /// <param name="unit">The unit.</param>
            /// <param name="radius">The radius.</param>
            /// <param name="color">The color.</param>
            /// <param name="width">The width.</param>
            /// <param name="zDeep">if set to <c>true</c> [z deep].</param>
            public Circle(GameObject unit, float radius, Color color, int width = 1, bool zDeep = false)
            {
                Color = color;
                Unit = unit;
                Radius = radius;
                Width = width;
                ZDeep = zDeep;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Circle"/> class.
            /// </summary>
            /// <param name="unit">The unit.</param>
            /// <param name="offset">The offset.</param>
            /// <param name="radius">The radius.</param>
            /// <param name="color">The color.</param>
            /// <param name="width">The width.</param>
            /// <param name="zDeep">if set to <c>true</c> [z deep].</param>
            public Circle(GameObject unit, Vector3 offset, float radius, Color color, int width = 1, bool zDeep = false)
            {
                Color = color;
                Unit = unit;
                Radius = radius;
                Width = width;
                ZDeep = zDeep;
                Offset = offset;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Circle"/> class.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <param name="offset">The offset.</param>
            /// <param name="radius">The radius.</param>
            /// <param name="color">The color.</param>
            /// <param name="width">The width.</param>
            /// <param name="zDeep">if set to <c>true</c> [z deep].</param>
            public Circle(Vector3 position, Vector3 offset, float radius, Color color, int width = 1, bool zDeep = false)
            {
                Color = color;
                Position = position;
                Radius = radius;
                Width = width;
                ZDeep = zDeep;
                Offset = offset;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Circle"/> class.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <param name="radius">The radius.</param>
            /// <param name="color">The color.</param>
            /// <param name="width">The width.</param>
            /// <param name="zDeep">if set to <c>true</c> [z deep].</param>
            public Circle(Vector3 position, float radius, Color color, int width = 1, bool zDeep = false)
            {
                Color = color;
                Position = position;
                Radius = radius;
                Width = width;
                ZDeep = zDeep;
            }

            /// <summary>
            /// Gets or sets the position.
            /// </summary>
            /// <value>The position.</value>
            public Vector3 Position { get; set; }

            /// <summary>
            /// Gets or sets the unit.
            /// </summary>
            /// <value>The unit.</value>
            public GameObject Unit { get; set; }

            /// <summary>
            /// Gets or sets the radius.
            /// </summary>
            /// <value>The radius.</value>
            public float Radius { get; set; }

            /// <summary>
            /// Gets or sets the color.
            /// </summary>
            /// <value>The color.</value>
            public Color Color { get; set; }

            /// <summary>
            /// Gets or sets the width.
            /// </summary>
            /// <value>The width.</value>
            public int Width { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether to enable depth buffering.
            /// </summary>
            /// <value><c>true</c> if depth buffering enabled; otherwise, <c>false</c>.</value>
            public bool ZDeep { get; set; }

            /// <summary>
            /// Gets or sets the offset.
            /// </summary>
            /// <value>The offset.</value>
            public Vector3 Offset
            {
                get { return _offset; }
                set { _offset = value; }
            }

            /// <summary>
            /// Called when the circle is drawn.
            /// </summary>
            public override void OnDraw()
            {
                try
                {
                    if (Unit != null && Unit.IsValid)
                    {
                        DrawCircle(Unit.Position + _offset, Radius, Color, Width, ZDeep);
                    }
                    else if ((Position + _offset).To2D().IsValid())
                    {
                        DrawCircle(Position + _offset, Radius, Color, Width, ZDeep);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(@"Common.Render.Circle.OnEndScene: " + e);
                }
            }

            /// <summary>
            /// Creates the vertexes.
            /// </summary>
            public static void CreateVertexes()
            {
                const float x = 6000f;
                _vertices = new VertexBuffer(
                    Device, Utilities.SizeOf<Vector4>() * 2 * 6, Usage.WriteOnly, VertexFormat.None, Pool.Managed);

                _vertices.Lock(0, 0, LockFlags.None).WriteRange(
                    new[]
                    {
                        //T1
                        new Vector4(-x, 0f, -x, 1.0f), new Vector4(), 
                        new Vector4(-x, 0f, x, 1.0f), new Vector4(),
                        new Vector4(x, 0f, -x, 1.0f), new Vector4(),

                        //T2
                        new Vector4(-x, 0f, x, 1.0f), new Vector4(),
                        new Vector4(x, 0f, x, 1.0f), new Vector4(),
                        new Vector4(x, 0f, -x, 1.0f), new Vector4()
                    });
                _vertices.Unlock();

                _vertexElements = new[]
                {
                    new VertexElement(
                        0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                    new VertexElement(
                        0, 16, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                    VertexElement.VertexDeclarationEnd
                };

                _vertexDeclaration = new VertexDeclaration(Device, _vertexElements);

                #region Effect

                try
                {
                    /*   
                    _effect = Effect.FromString(Device, @"
                    struct VS_S
                     {
                         float4 Position : POSITION;
                         float4 Color : COLOR0;
                         float4 Position3D : TEXCOORD0;
                     };

                     float4x4 ProjectionMatrix;
                     float4 CircleColor;
                     float Radius;
                     float Border;
                     bool zEnabled;
                     VS_S VS( VS_S input )
                     {
                         VS_S output = (VS_S)0;
	
                         output.Position = mul(input.Position, ProjectionMatrix);
                         output.Color = input.Color;
                         output.Position3D = input.Position;
                         return output;
                     }

                     float4 PS( VS_S input ) : COLOR
                     {
                         VS_S output = (VS_S)0;
                         output = input;

                         float4 v = output.Position3D; 
                         float distance = Radius - sqrt(v.x * v.x + v.z*v.z); // Distance to the circle arc.
    
                         output.Color.x = CircleColor.x;
                         output.Color.y = CircleColor.y;
                         output.Color.z = CircleColor.z;
                            
                         if(distance < Border && distance > -Border)
                         {
                             output.Color.w = (CircleColor.w - CircleColor.w * abs(distance * 1.75 / Border));
                         }
                         else
                         {
                             output.Color.w = 0;
                         }
                            
                         if(Border < 1 && distance >= 0)
                         {
                             output.Color.w = CircleColor.w;
                         }

                         return output.Color;
                     }

                     technique Main {
                         pass P0 {
                             ZEnable = zEnabled;
                             AlphaBlendEnable = TRUE;
                             DestBlend = INVSRCALPHA;
                             SrcBlend = SRCALPHA;
                             VertexShader = compile vs_2_0 VS();
                             PixelShader  = compile ps_2_0 PS();
                         }
                     }", ShaderFlags.None);
                    */
                    var compiledEffect = new byte[]
                    {
                        0x01, 0x09, 0xFF, 0xFE, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00,
                        0x02, 0x00, 0x00, 0x00, 0x60, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x04, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x50, 0x72, 0x6F, 0x6A,
                        0x65, 0x63, 0x74, 0x69, 0x6F, 0x6E, 0x4D, 0x61, 0x74, 0x72, 0x69, 0x78, 0x00, 0x00, 0x00, 0x00,
                        0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0xA4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00,
                        0x43, 0x69, 0x72, 0x63, 0x6C, 0x65, 0x43, 0x6F, 0x6C, 0x6F, 0x72, 0x00, 0x03, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0xD4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00,
                        0x52, 0x61, 0x64, 0x69, 0x75, 0x73, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                        0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x42, 0x6F, 0x72, 0x64,
                        0x65, 0x72, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2C, 0x01, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x7A, 0x45, 0x6E, 0x61, 0x62, 0x6C, 0x65, 0x64,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                        0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                        0x01, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                        0x01, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                        0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                        0x0F, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x50, 0x30, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00,
                        0x4D, 0x61, 0x69, 0x6E, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                        0x03, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x78, 0x00, 0x00, 0x00, 0x94, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xB4, 0x00, 0x00, 0x00, 0xD0, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xE0, 0x00, 0x00, 0x00, 0xFC, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x01, 0x00, 0x00, 0x28, 0x01, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF4, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x01, 0x00, 0x00, 0x00, 0xEC, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x01, 0x00, 0x00, 0x3C, 0x01, 0x00, 0x00,
                        0x0D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x60, 0x01, 0x00, 0x00, 0x5C, 0x01, 0x00, 0x00,
                        0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x01, 0x00, 0x00, 0x7C, 0x01, 0x00, 0x00,
                        0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xA0, 0x01, 0x00, 0x00, 0x9C, 0x01, 0x00, 0x00,
                        0x92, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC0, 0x01, 0x00, 0x00, 0xBC, 0x01, 0x00, 0x00,
                        0x93, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xD8, 0x01, 0x00, 0x00, 0xD4, 0x01, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0xFF, 0xFF, 0xFF, 0xFF, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4C, 0x04, 0x00, 0x00,
                        0x00, 0x02, 0xFF, 0xFF, 0xFE, 0xFF, 0x38, 0x00, 0x43, 0x54, 0x41, 0x42, 0x1C, 0x00, 0x00, 0x00,
                        0xAA, 0x00, 0x00, 0x00, 0x00, 0x02, 0xFF, 0xFF, 0x03, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x20, 0xA3, 0x00, 0x00, 0x00, 0x58, 0x00, 0x00, 0x00, 0x02, 0x00, 0x05, 0x00,
                        0x01, 0x00, 0x00, 0x00, 0x60, 0x00, 0x00, 0x00, 0x70, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00,
                        0x02, 0x00, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x8C, 0x00, 0x00, 0x00, 0x70, 0x00, 0x00, 0x00,
                        0x9C, 0x00, 0x00, 0x00, 0x02, 0x00, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00, 0x60, 0x00, 0x00, 0x00,
                        0x70, 0x00, 0x00, 0x00, 0x42, 0x6F, 0x72, 0x64, 0x65, 0x72, 0x00, 0xAB, 0x00, 0x00, 0x03, 0x00,
                        0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x43, 0x69, 0x72, 0x63,
                        0x6C, 0x65, 0x43, 0x6F, 0x6C, 0x6F, 0x72, 0x00, 0x01, 0x00, 0x03, 0x00, 0x01, 0x00, 0x04, 0x00,
                        0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x52, 0x61, 0x64, 0x69, 0x75, 0x73, 0x00, 0x70,
                        0x73, 0x5F, 0x32, 0x5F, 0x30, 0x00, 0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66, 0x74, 0x20,
                        0x28, 0x52, 0x29, 0x20, 0x48, 0x4C, 0x53, 0x4C, 0x20, 0x53, 0x68, 0x61, 0x64, 0x65, 0x72, 0x20,
                        0x43, 0x6F, 0x6D, 0x70, 0x69, 0x6C, 0x65, 0x72, 0x20, 0x39, 0x2E, 0x32, 0x39, 0x2E, 0x39, 0x35,
                        0x32, 0x2E, 0x33, 0x31, 0x31, 0x31, 0x00, 0xAB, 0xFE, 0xFF, 0x7C, 0x00, 0x50, 0x52, 0x45, 0x53,
                        0x01, 0x02, 0x58, 0x46, 0xFE, 0xFF, 0x30, 0x00, 0x43, 0x54, 0x41, 0x42, 0x1C, 0x00, 0x00, 0x00,
                        0x8B, 0x00, 0x00, 0x00, 0x01, 0x02, 0x58, 0x46, 0x02, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00,
                        0x00, 0x01, 0x00, 0x20, 0x88, 0x00, 0x00, 0x00, 0x44, 0x00, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00,
                        0x01, 0x00, 0x00, 0x00, 0x4C, 0x00, 0x00, 0x00, 0x5C, 0x00, 0x00, 0x00, 0x6C, 0x00, 0x00, 0x00,
                        0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x78, 0x00, 0x00, 0x00, 0x5C, 0x00, 0x00, 0x00,
                        0x42, 0x6F, 0x72, 0x64, 0x65, 0x72, 0x00, 0xAB, 0x00, 0x00, 0x03, 0x00, 0x01, 0x00, 0x01, 0x00,
                        0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x43, 0x69, 0x72, 0x63, 0x6C, 0x65, 0x43, 0x6F,
                        0x6C, 0x6F, 0x72, 0x00, 0x01, 0x00, 0x03, 0x00, 0x01, 0x00, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x74, 0x78, 0x00, 0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66, 0x74,
                        0x20, 0x28, 0x52, 0x29, 0x20, 0x48, 0x4C, 0x53, 0x4C, 0x20, 0x53, 0x68, 0x61, 0x64, 0x65, 0x72,
                        0x20, 0x43, 0x6F, 0x6D, 0x70, 0x69, 0x6C, 0x65, 0x72, 0x20, 0x39, 0x2E, 0x32, 0x39, 0x2E, 0x39,
                        0x35, 0x32, 0x2E, 0x33, 0x31, 0x31, 0x31, 0x00, 0xFE, 0xFF, 0x0C, 0x00, 0x50, 0x52, 0x53, 0x49,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFE, 0xFF, 0x1A, 0x00,
                        0x43, 0x4C, 0x49, 0x54, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0xBF,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFE, 0xFF, 0x1F, 0x00, 0x46, 0x58, 0x4C, 0x43,
                        0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x30, 0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x02, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x40, 0xA0, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x02, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                        0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
                        0x03, 0x00, 0x00, 0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00,
                        0xF0, 0xF0, 0xF0, 0xF0, 0x0F, 0x0F, 0x0F, 0x0F, 0xFF, 0xFF, 0x00, 0x00, 0x51, 0x00, 0x00, 0x05,
                        0x06, 0x00, 0x0F, 0xA0, 0x00, 0x00, 0xE0, 0x3F, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x80, 0xBF,
                        0x00, 0x00, 0x00, 0x00, 0x1F, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x07, 0xB0,
                        0x05, 0x00, 0x00, 0x03, 0x00, 0x00, 0x08, 0x80, 0x00, 0x00, 0xAA, 0xB0, 0x00, 0x00, 0xAA, 0xB0,
                        0x04, 0x00, 0x00, 0x04, 0x00, 0x00, 0x01, 0x80, 0x00, 0x00, 0x00, 0xB0, 0x00, 0x00, 0x00, 0xB0,
                        0x00, 0x00, 0xFF, 0x80, 0x07, 0x00, 0x00, 0x02, 0x00, 0x00, 0x01, 0x80, 0x00, 0x00, 0x00, 0x80,
                        0x06, 0x00, 0x00, 0x02, 0x00, 0x00, 0x01, 0x80, 0x00, 0x00, 0x00, 0x80, 0x02, 0x00, 0x00, 0x03,
                        0x00, 0x00, 0x01, 0x80, 0x00, 0x00, 0x00, 0x81, 0x04, 0x00, 0x00, 0xA0, 0x02, 0x00, 0x00, 0x03,
                        0x00, 0x00, 0x02, 0x80, 0x00, 0x00, 0x00, 0x81, 0x05, 0x00, 0x00, 0xA1, 0x58, 0x00, 0x00, 0x04,
                        0x00, 0x00, 0x02, 0x80, 0x00, 0x00, 0x55, 0x80, 0x06, 0x00, 0x55, 0xA0, 0x06, 0x00, 0xAA, 0xA0,
                        0x02, 0x00, 0x00, 0x03, 0x00, 0x00, 0x04, 0x80, 0x00, 0x00, 0x00, 0x80, 0x05, 0x00, 0x00, 0xA1,
                        0x58, 0x00, 0x00, 0x04, 0x00, 0x00, 0x02, 0x80, 0x00, 0x00, 0xAA, 0x80, 0x06, 0x00, 0x55, 0xA0,
                        0x00, 0x00, 0x55, 0x80, 0x05, 0x00, 0x00, 0x03, 0x00, 0x00, 0x04, 0x80, 0x00, 0x00, 0x00, 0x80,
                        0x06, 0x00, 0x00, 0xA0, 0x58, 0x00, 0x00, 0x04, 0x00, 0x00, 0x01, 0x80, 0x00, 0x00, 0x00, 0x80,
                        0x06, 0x00, 0xAA, 0xA0, 0x06, 0x00, 0x55, 0xA0, 0x01, 0x00, 0x00, 0x02, 0x00, 0x00, 0x08, 0x80,
                        0x06, 0x00, 0x55, 0xA0, 0x58, 0x00, 0x00, 0x04, 0x00, 0x00, 0x01, 0x80, 0x01, 0x00, 0x00, 0xA0,
                        0x00, 0x00, 0xFF, 0x80, 0x00, 0x00, 0x00, 0x80, 0x05, 0x00, 0x00, 0x03, 0x00, 0x00, 0x04, 0x80,
                        0x00, 0x00, 0xAA, 0x80, 0x00, 0x00, 0x00, 0xA0, 0x23, 0x00, 0x00, 0x02, 0x00, 0x00, 0x04, 0x80,
                        0x00, 0x00, 0xAA, 0x80, 0x04, 0x00, 0x00, 0x04, 0x00, 0x00, 0x04, 0x80, 0x03, 0x00, 0xFF, 0xA0,
                        0x00, 0x00, 0xAA, 0x81, 0x03, 0x00, 0xFF, 0xA0, 0x58, 0x00, 0x00, 0x04, 0x00, 0x00, 0x02, 0x80,
                        0x00, 0x00, 0x55, 0x80, 0x06, 0x00, 0xFF, 0xA0, 0x00, 0x00, 0xAA, 0x80, 0x58, 0x00, 0x00, 0x04,
                        0x00, 0x00, 0x08, 0x80, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x55, 0x80, 0x03, 0x00, 0xFF, 0xA0,
                        0x01, 0x00, 0x00, 0x02, 0x00, 0x00, 0x07, 0x80, 0x02, 0x00, 0xE4, 0xA0, 0x01, 0x00, 0x00, 0x02,
                        0x00, 0x08, 0x0F, 0x80, 0x00, 0x00, 0xE4, 0x80, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x4C, 0x01, 0x00, 0x00, 0x00, 0x02, 0xFE, 0xFF, 0xFE, 0xFF, 0x34, 0x00, 0x43, 0x54, 0x41, 0x42,
                        0x1C, 0x00, 0x00, 0x00, 0x9B, 0x00, 0x00, 0x00, 0x00, 0x02, 0xFE, 0xFF, 0x01, 0x00, 0x00, 0x00,
                        0x1C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x94, 0x00, 0x00, 0x00, 0x30, 0x00, 0x00, 0x00,
                        0x02, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x44, 0x00, 0x00, 0x00, 0x54, 0x00, 0x00, 0x00,
                        0x50, 0x72, 0x6F, 0x6A, 0x65, 0x63, 0x74, 0x69, 0x6F, 0x6E, 0x4D, 0x61, 0x74, 0x72, 0x69, 0x78,
                        0x00, 0xAB, 0xAB, 0xAB, 0x03, 0x00, 0x03, 0x00, 0x04, 0x00, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x76, 0x73, 0x5F, 0x32, 0x5F, 0x30, 0x00, 0x4D, 0x69, 0x63, 0x72, 0x6F,
                        0x73, 0x6F, 0x66, 0x74, 0x20, 0x28, 0x52, 0x29, 0x20, 0x48, 0x4C, 0x53, 0x4C, 0x20, 0x53, 0x68,
                        0x61, 0x64, 0x65, 0x72, 0x20, 0x43, 0x6F, 0x6D, 0x70, 0x69, 0x6C, 0x65, 0x72, 0x20, 0x39, 0x2E,
                        0x32, 0x39, 0x2E, 0x39, 0x35, 0x32, 0x2E, 0x33, 0x31, 0x31, 0x31, 0x00, 0x1F, 0x00, 0x00, 0x02,
                        0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x0F, 0x90, 0x1F, 0x00, 0x00, 0x02, 0x0A, 0x00, 0x00, 0x80,
                        0x01, 0x00, 0x0F, 0x90, 0x09, 0x00, 0x00, 0x03, 0x00, 0x00, 0x01, 0xC0, 0x00, 0x00, 0xE4, 0x90,
                        0x00, 0x00, 0xE4, 0xA0, 0x09, 0x00, 0x00, 0x03, 0x00, 0x00, 0x02, 0xC0, 0x00, 0x00, 0xE4, 0x90,
                        0x01, 0x00, 0xE4, 0xA0, 0x09, 0x00, 0x00, 0x03, 0x00, 0x00, 0x04, 0xC0, 0x00, 0x00, 0xE4, 0x90,
                        0x02, 0x00, 0xE4, 0xA0, 0x09, 0x00, 0x00, 0x03, 0x00, 0x00, 0x08, 0xC0, 0x00, 0x00, 0xE4, 0x90,
                        0x03, 0x00, 0xE4, 0xA0, 0x01, 0x00, 0x00, 0x02, 0x00, 0x00, 0x0F, 0xD0, 0x01, 0x00, 0xE4, 0x90,
                        0x01, 0x00, 0x00, 0x02, 0x00, 0x00, 0x0F, 0xE0, 0x00, 0x00, 0xE4, 0x90, 0xFF, 0xFF, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0xE0, 0x00, 0x00, 0x00, 0x00, 0x02, 0x58, 0x46, 0xFE, 0xFF, 0x25, 0x00,
                        0x43, 0x54, 0x41, 0x42, 0x1C, 0x00, 0x00, 0x00, 0x5F, 0x00, 0x00, 0x00, 0x00, 0x02, 0x58, 0x46,
                        0x01, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x20, 0x5C, 0x00, 0x00, 0x00,
                        0x30, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x3C, 0x00, 0x00, 0x00,
                        0x4C, 0x00, 0x00, 0x00, 0x7A, 0x45, 0x6E, 0x61, 0x62, 0x6C, 0x65, 0x64, 0x00, 0xAB, 0xAB, 0xAB,
                        0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x74, 0x78, 0x00, 0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66, 0x74, 0x20, 0x28, 0x52, 0x29,
                        0x20, 0x48, 0x4C, 0x53, 0x4C, 0x20, 0x53, 0x68, 0x61, 0x64, 0x65, 0x72, 0x20, 0x43, 0x6F, 0x6D,
                        0x70, 0x69, 0x6C, 0x65, 0x72, 0x20, 0x39, 0x2E, 0x32, 0x39, 0x2E, 0x39, 0x35, 0x32, 0x2E, 0x33,
                        0x31, 0x31, 0x31, 0x00, 0xFE, 0xFF, 0x02, 0x00, 0x43, 0x4C, 0x49, 0x54, 0x00, 0x00, 0x00, 0x00,
                        0xFE, 0xFF, 0x0C, 0x00, 0x46, 0x58, 0x4C, 0x43, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x10,
                        0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0xF0, 0xF0, 0xF0,
                        0x0F, 0x0F, 0x0F, 0x0F, 0xFF, 0xFF, 0x00, 0x00
                    };
                    _effect = Effect.FromMemory(Device, compiledEffect, ShaderFlags.None);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }

                #endregion

                _technique = _effect.GetTechnique(0);

                if (!_initialized)
                {
                    _initialized = true;
                    Drawing.OnPreReset += OnPreReset;
                    Drawing.OnPreReset += OnPostReset;
                    AppDomain.CurrentDomain.DomainUnload += Dispose;
                }
            }

            /// <summary>
            /// Handles the <see cref="E:PreReset" /> event.
            /// </summary>
            /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
            private static void OnPreReset(EventArgs args)
            {
                if (_effect != null && !_effect.IsDisposed)
                {
                    _effect.OnLostDevice();
                }
            }

            /// <summary>
            /// Handles the <see cref="E:PostReset" /> event.
            /// </summary>
            /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
            private static void OnPostReset(EventArgs args)
            {
                if (_effect != null && !_effect.IsDisposed)
                {
                    _effect.OnResetDevice();
                }
            }

            /// <summary>
            /// Disposes the circle.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
            private static void Dispose(object sender, EventArgs e)
            {
                if (_effect != null && !_effect.IsDisposed)
                {
                    _effect.Dispose();
                }

                if (_vertices != null && !_vertices.IsDisposed)
                {
                    _vertices.Dispose();
                }

                if (_vertexDeclaration != null && !_vertexDeclaration.IsDisposed)
                {
                    _vertexDeclaration.Dispose();
                }
            }

            /// <summary>
            /// Draws the circle.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <param name="radius">The radius.</param>
            /// <param name="color">The color.</param>
            /// <param name="width">The width.</param>
            /// <param name="zDeep">if set to <c>true</c> the circle will be drawn with depth buffering.</param>
            public static void DrawCircle(Vector3 position, float radius, Color color, int width = 5, bool zDeep = false)
            {
                try
                {
                    if (Device == null || Device.IsDisposed)
                    {
                        return;
                    }

                    if (_vertices == null)
                    {
                        CreateVertexes();
                    }

                    if (_vertices == null || _vertices.IsDisposed || _vertexDeclaration.IsDisposed || _effect.IsDisposed ||
                        _technique.IsDisposed)
                    {
                        return;
                    }

                    var olddec = Device.VertexDeclaration;

                    _effect.Technique = _technique;

                    _effect.Begin();
                    _effect.BeginPass(0);
                    _effect.SetValue(
                        "ProjectionMatrix", Matrix.Translation(position.SwitchYZ()) * Drawing.View * Drawing.Projection);
                    _effect.SetValue(
                        "CircleColor", new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f));
                    _effect.SetValue("Radius", radius);
                    _effect.SetValue("Border", 2f + width);
                    _effect.SetValue("zEnabled", zDeep);

                    Device.SetStreamSource(0, _vertices, 0, Utilities.SizeOf<Vector4>() * 2);
                    Device.VertexDeclaration = _vertexDeclaration;

                    Device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

                    _effect.EndPass();
                    _effect.End();

                    Device.VertexDeclaration = olddec;
                }
                catch (Exception e)
                {
                    _vertices = null;
                    Console.WriteLine(@"DrawCircle: " + e);
                }
            }
        }

        /// <summary>
        /// Draws lines.
        /// </summary>
        public class Line : RenderObject
        {
            /// <summary>
            /// Delegate to get the position of the line.
            /// </summary>
            /// <returns>Vector2.</returns>
            public delegate Vector2 PositionDelegate();

            /// <summary>
            /// The DirectX line
            /// </summary>
            private readonly SharpDX.Direct3D9.Line _line;

            /// <summary>
            /// The width
            /// </summary>
            private int _width;

            /// <summary>
            /// The color
            /// </summary>
            public ColorBGRA Color;

            /// <summary>
            /// Initializes a new instance of the <see cref="Line"/> class.
            /// </summary>
            /// <param name="start">The start.</param>
            /// <param name="end">The end.</param>
            /// <param name="width">The width.</param>
            /// <param name="color">The color.</param>
            public Line(Vector2 start, Vector2 end, int width, ColorBGRA color)
            {
                _line = new SharpDX.Direct3D9.Line(Device);
                Width = width;
                Color = color;
                Start = start;
                End = end;
                Game.OnUpdate += GameOnOnUpdate;
            }

            /// <summary>
            /// Games the on on update.
            /// </summary>
            /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
            private void GameOnOnUpdate(EventArgs args)
            {
                if (StartPositionUpdate != null)
                {
                    Start = StartPositionUpdate();
                }

                if (EndPositionUpdate != null)
                {
                    End = EndPositionUpdate();
                }
            }

            /// <summary>
            /// Gets or sets the start.
            /// </summary>
            /// <value>The start.</value>
            public Vector2 Start { get; set; }

            /// <summary>
            /// Gets or sets the end.
            /// </summary>
            /// <value>The end.</value>
            public Vector2 End { get; set; }

            /// <summary>
            /// Gets or sets the delegate that sets the start position.
            /// </summary>
            /// <value>The start position update.</value>
            public PositionDelegate StartPositionUpdate { get; set; }

            /// <summary>
            /// Gets or sets the delegate that gets the end position.
            /// </summary>
            /// <value>The end position update.</value>
            public PositionDelegate EndPositionUpdate { get; set; }

            /// <summary>
            /// Gets or sets the width.
            /// </summary>
            /// <value>The width.</value>
            public int Width
            {
                get { return _width; }
                set
                {
                    _line.Width = value;
                    _width = value;
                }
            }

            /// <summary>
            /// Called when the scene has ended.
            /// </summary>
            public override void OnEndScene()
            {
                try
                {
                    if (_line.IsDisposed)
                    {
                        return;
                    }

                    _line.Begin();
                    _line.Draw(new[] { Start, End }, Color);
                    _line.End();
                }
                catch (Exception e)
                {
                    Console.WriteLine(@"Common.Render.Line.OnEndScene: " + e);
                }
            }

            /// <summary>
            /// Called before the DirectX device is reset.
            /// </summary>
            public override void OnPreReset()
            {
                _line.OnLostDevice();
            }

            /// <summary>
            /// Called after the DirectX is reset.
            /// </summary>
            public override void OnPostReset()
            {
                _line.OnResetDevice();
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public override void Dispose()
            {
                if (!_line.IsDisposed)
                {
                    _line.Dispose();
                }
                Game.OnUpdate -= GameOnOnUpdate;
            }
        }

        /// <summary>
        /// Draws a Rectangle.
        /// </summary>
        public class Rectangle : RenderObject
        {
            /// <summary>
            /// Delegate to get the position of the rectangle.
            /// </summary>
            /// <returns>Vector2.</returns>
            public delegate Vector2 PositionDelegate();

            /// <summary>
            /// The DirectX line
            /// </summary>
            private readonly SharpDX.Direct3D9.Line _line;

            /// <summary>
            /// The color of the rectangle
            /// </summary>
            public ColorBGRA Color;

            /// <summary>
            /// Initializes a new instance of the <see cref="Rectangle"/> class.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <param name="width">The width.</param>
            /// <param name="height">The height.</param>
            /// <param name="color">The color.</param>
            public Rectangle(int x, int y, int width, int height, ColorBGRA color)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                Color = color;
                _line = new SharpDX.Direct3D9.Line(Device) { Width = height };
                Game.OnUpdate += Game_OnUpdate;
            }

            /// <summary>
            /// Fired when the game is updated.
            /// </summary>
            /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
            private void Game_OnUpdate(EventArgs args)
            {
                if (PositionUpdate != null)
                {
                    Vector2 pos = PositionUpdate();
                    X = (int) pos.X;
                    Y = (int) pos.Y;
                }
            }

            /// <summary>
            /// Gets or sets the x.
            /// </summary>
            /// <value>The x.</value>
            public int X { get; set; }

            /// <summary>
            /// Gets or sets the y.
            /// </summary>
            /// <value>The y.</value>
            public int Y { get; set; }

            /// <summary>
            /// Gets or sets the width.
            /// </summary>
            /// <value>The width.</value>
            public int Width { get; set; }

            /// <summary>
            /// Gets or sets the height.
            /// </summary>
            /// <value>The height.</value>
            public int Height { get; set; }

            /// <summary>
            /// Gets or sets the delegate that gets the position.
            /// </summary>
            /// <value>The position update.</value>
            public PositionDelegate PositionUpdate { get; set; }

            /// <summary>
            /// Called when [end scene].
            /// </summary>
            public override void OnEndScene()
            {
                try
                {
                    if (_line.IsDisposed)
                    {
                        return;
                    }

                    _line.Begin();
                    _line.Draw(new[] { new Vector2(X, Y + Height / 2), new Vector2(X + Width, Y + Height / 2) }, Color);
                    _line.End();
                }
                catch (Exception e)
                {
                    Console.WriteLine(@"Common.Render.Rectangle.OnEndScene: " + e);
                }
            }

            /// <summary>
            /// Called before the DirectX device is reset.
            /// </summary>
            public override void OnPreReset()
            {
                _line.OnLostDevice();
            }

            /// <summary>
            /// Called after the DirectX device is reset.
            /// </summary>
            public override void OnPostReset()
            {
                _line.OnResetDevice();
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public override void Dispose()
            {
                if (!_line.IsDisposed)
                {
                    _line.Dispose();
                }
                Game.OnUpdate -= Game_OnUpdate;
            }
        }

        /// <summary>
        /// A base class that renders objects.
        /// </summary>
        public class RenderObject : IDisposable
        {
            /// <summary>
            /// Delegate that gets if the object is visible.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <returns><c>true</c> if the object is visible, <c>false</c> otherwise.</returns>
            public delegate bool VisibleConditionDelegate(RenderObject sender);

            /// <summary>
            /// <c>true</c> if the render object is visible
            /// </summary>
            private bool _visible = true;

            /// <summary>
            /// The layer
            /// </summary>
            public float Layer = 0.0f;

            /// <summary>
            /// The visible condition delegate.
            /// </summary>
            public VisibleConditionDelegate VisibleCondition;

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="RenderObject"/> is visible.
            /// </summary>
            /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
            public bool Visible
            {
                get { return VisibleCondition != null ? VisibleCondition(this) : _visible; }
                set { _visible = value; }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public virtual void Dispose() {}

            /// <summary>
            /// Called when the render object is drawn.
            /// </summary>
            public virtual void OnDraw() {}

            /// <summary>
            /// Called when the scene has ended..
            /// </summary>
            public virtual void OnEndScene() {}
            /// <summary>

            /// Called before the DirectX device is reset.
            /// </summary>
            public virtual void OnPreReset() {}

            /// <summary>
            /// Called after the DirectX device is reset.
            /// </summary>
            public virtual void OnPostReset() {}

            /// <summary>
            /// Determines whether this instace has a valid layer.
            /// </summary>
            /// <returns><c>true</c> if has a valid layer; otherwise, <c>false</c>.</returns>
            public bool HasValidLayer()
            {
                return Layer >= -5 && Layer <= 5;
            }
        }

        /// <summary>
        /// Draws a sprite image.
        /// </summary>
        public class Sprite : RenderObject
        {
            /// <summary>
            /// Delegate for when the sprite is reset.
            /// </summary>
            /// <param name="sprite">The sprite.</param>
            public delegate void OnResetting(Sprite sprite);

            /// <summary>
            /// Delegate that gets the position of the sprite.
            /// </summary>
            /// <returns>Vector2.</returns>
            public delegate Vector2 PositionDelegate();

            /// <summary>
            /// The DirectX sprite
            /// </summary>
            private readonly SharpDX.Direct3D9.Sprite _sprite = new SharpDX.Direct3D9.Sprite(Device);

            /// <summary>
            /// The color of the sprite.
            /// </summary>
            private ColorBGRA _color = SharpDX.Color.White;

            /// <summary>
            /// The crop of the sprite.
            /// </summary>
            private SharpDX.Rectangle? _crop;

            /// <summary>
            /// <c>true</c> if the sprite is hidden.
            /// </summary>
            private bool _hide;

            /// <summary>
            /// The original texture
            /// </summary>
            private Texture _originalTexture;

            /// <summary>
            /// The scale
            /// </summary>
            private Vector2 _scale = new Vector2(1, 1);

            /// <summary>
            /// The texture
            /// </summary>
            private Texture _texture;

            /// <summary>
            /// Prevents a default instance of the <see cref="Sprite"/> class from being created.
            /// </summary>
            private Sprite()
            {
                Game.OnUpdate += Game_OnUpdate;
            }

            /// <summary>
            /// Fired when the game is updated.
            /// </summary>
            /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
            private void Game_OnUpdate(EventArgs args)
            {
                if (PositionUpdate != null)
                {
                    Vector2 pos = PositionUpdate();
                    X = (int) pos.X;
                    Y = (int) pos.Y;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Sprite"/> class.
            /// </summary>
            /// <param name="bitmap">The bitmap.</param>
            /// <param name="position">The position.</param>
            public Sprite(Bitmap bitmap, Vector2 position) : this()
            {
                UpdateTextureBitmap(bitmap, position);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Sprite"/> class.
            /// </summary>
            /// <param name="texture">The texture.</param>
            /// <param name="position">The position.</param>
            public Sprite(BaseTexture texture, Vector2 position) : this()
            {
                UpdateTextureBitmap(
                    (Bitmap) Image.FromStream(BaseTexture.ToStream(texture, ImageFileFormat.Bmp)), position);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Sprite"/> class.
            /// </summary>
            /// <param name="stream">The stream.</param>
            /// <param name="position">The position.</param>
            public Sprite(Stream stream, Vector2 position) : this()
            {
                UpdateTextureBitmap(new Bitmap(stream), position);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Sprite"/> class.
            /// </summary>
            /// <param name="bytesArray">The bytes array.</param>
            /// <param name="position">The position.</param>
            public Sprite(byte[] bytesArray, Vector2 position) : this()
            {
                UpdateTextureBitmap((Bitmap) Image.FromStream(new MemoryStream(bytesArray)), position);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Sprite"/> class.
            /// </summary>
            /// <param name="fileLocation">The file location.</param>
            /// <param name="position">The position.</param>
            public Sprite(string fileLocation, Vector2 position) : this()
            {
                if (!File.Exists((fileLocation)))
                {
                    return;
                }

                UpdateTextureBitmap(new Bitmap(fileLocation), position);
            }

            /// <summary>
            /// Gets or sets the x.
            /// </summary>
            /// <value>The x.</value>
            public int X { get; set; }

            /// <summary>
            /// Gets or sets the y.
            /// </summary>
            /// <value>The y.</value>
            public int Y { get; set; }

            /// <summary>
            /// Gets or sets the bitmap.
            /// </summary>
            /// <value>The bitmap.</value>
            public Bitmap Bitmap { get; set; }

            /// <summary>
            /// Gets the width.
            /// </summary>
            /// <value>The width.</value>
            public int Width
            {
                get { return (int) (Bitmap.Width * _scale.X); }
            }

            /// <summary>
            /// Gets the height.
            /// </summary>
            /// <value>The height.</value>
            public int Height
            {
                get { return (int) (Bitmap.Height * _scale.Y); }
            }

            /// <summary>
            /// Gets the size.
            /// </summary>
            /// <value>The size.</value>
            public Vector2 Size
            {
                get { return new Vector2(Bitmap.Width, Bitmap.Height); }
            }

            /// <summary>
            /// Gets or sets the position.
            /// </summary>
            /// <value>The position.</value>
            public Vector2 Position
            {
                set
                {
                    X = (int) value.X;
                    Y = (int) value.Y;
                }

                get { return new Vector2(X, Y); }
            }

            /// <summary>
            /// Gets or sets the delegate that gets the position.
            /// </summary>
            /// <value>The position update.</value>
            public PositionDelegate PositionUpdate { get; set; }

            /// <summary>
            /// Gets or sets the scale.
            /// </summary>
            /// <value>The scale.</value>
            public Vector2 Scale
            {
                set { _scale = value; }
                get { return _scale; }
            }

            /// <summary>
            /// Gets or sets the rotation.
            /// </summary>
            /// <value>The rotation.</value>
            public float Rotation { set; get; }

            /// <summary>
            /// Gets or sets the color.
            /// </summary>
            /// <value>The color.</value>
            public ColorBGRA Color
            {
                set { _color = value; }
                get { return _color; }
            }

            /// <summary>
            /// Occurs when the sprite is reset.
            /// </summary>
            public event OnResetting OnReset;

            /// <summary>
            /// Crops the sprite.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <param name="w">The width.</param>
            /// <param name="h">The height.</param>
            /// <param name="scale">if set to <c>true</c>, crops with the scale.</param>
            public void Crop(int x, int y, int w, int h, bool scale = false)
            {
                _crop = new SharpDX.Rectangle(x, y, w, h);

                if (scale)
                {
                    _crop = new SharpDX.Rectangle(
                        (int) (_scale.X * x), (int) (_scale.Y * y), (int) (_scale.X * w), (int) (_scale.Y * h));
                }
            }

            /// <summary>
            /// Crops the sprite.
            /// </summary>
            /// <param name="rect">The rectangle.</param>
            /// <param name="scale">if set to <c>true</c>, crops with the scale.</param>
            public void Crop(SharpDX.Rectangle rect, bool scale = false)
            {
                _crop = rect;

                if (scale)
                {
                    _crop = new SharpDX.Rectangle(
                        (int) (_scale.X * rect.X), (int) (_scale.Y * rect.Y), (int) (_scale.X * rect.Width),
                        (int) (_scale.Y * rect.Height));
                }
            }

            /// <summary>
            /// Shows this instance.
            /// </summary>
            public void Show()
            {
                _hide = false;
            }

            /// <summary>
            /// Hides this instance.
            /// </summary>
            public void Hide()
            {
                _hide = true;
            }

            /// <summary>
            /// Resets this instance.
            /// </summary>
            public void Reset()
            {
                UpdateTextureBitmap(
                    (Bitmap) Image.FromStream(BaseTexture.ToStream(_originalTexture, ImageFileFormat.Bmp)));

                if (OnReset != null)
                {
                    OnReset(this);
                }
            }

            /// <summary>
            /// Makes the sprite black and white.
            /// </summary>
            public void GrayScale()
            {
                SetSaturation(0.0f);
            }

            /// <summary>
            /// Fades this instance. (Saturation is 1/2)
            /// </summary>
            public void Fade()
            {
                SetSaturation(0.5f);
            }

            /// <summary>
            /// Complements this instance.
            /// </summary>
            public void Complement()
            {
                SetSaturation(-1.0f);
            }

            /// <summary>
            /// Sets the saturation.
            /// </summary>
            /// <param name="saturiation">The saturiation.</param>
            public void SetSaturation(float saturiation)
            {
                UpdateTextureBitmap(SaturateBitmap(Bitmap, saturiation));
            }

            /// <summary>
            /// Saturates the bitmap.
            /// </summary>
            /// <param name="original">The original image.</param>
            /// <param name="saturation">The saturation.</param>
            /// <returns>Bitmap.</returns>
            private static Bitmap SaturateBitmap(Image original, float saturation)
            {
                const float rWeight = 0.3086f;
                const float gWeight = 0.6094f;
                const float bWeight = 0.0820f;

                var a = (1.0f - saturation) * rWeight + saturation;
                var b = (1.0f - saturation) * rWeight;
                var c = (1.0f - saturation) * rWeight;
                var d = (1.0f - saturation) * gWeight;
                var e = (1.0f - saturation) * gWeight + saturation;
                var f = (1.0f - saturation) * gWeight;
                var g = (1.0f - saturation) * bWeight;
                var h = (1.0f - saturation) * bWeight;
                var i = (1.0f - saturation) * bWeight + saturation;

                var newBitmap = new Bitmap(original.Width, original.Height);
                var gr = Graphics.FromImage(newBitmap);

                // ColorMatrix elements
                float[][] ptsArray =
                {
                    new[] { a, b, c, 0, 0 }, new[] { d, e, f, 0, 0 }, new[] { g, h, i, 0, 0 },
                    new float[] { 0, 0, 0, 1, 0 }, new float[] { 0, 0, 0, 0, 1 }
                };
                // Create ColorMatrix
                var clrMatrix = new ColorMatrix(ptsArray);
                // Create ImageAttributes
                var imgAttribs = new ImageAttributes();
                // Set color matrix
                imgAttribs.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Default);
                // Draw Image with no effects
                gr.DrawImage(original, 0, 0, original.Width, original.Height);
                // Draw Image with image attributes
                gr.DrawImage(
                    original, new System.Drawing.Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width,
                    original.Height, GraphicsUnit.Pixel, imgAttribs);
                gr.Dispose();

                return newBitmap;
            }

            /// <summary>
            /// Updates the texture bitmap.
            /// </summary>
            /// <param name="newBitmap">The new bitmap.</param>
            /// <param name="position">The position.</param>
            public void UpdateTextureBitmap(Bitmap newBitmap, Vector2 position = new Vector2())
            {
                if (position.IsValid())
                {
                    Position = position;
                }

                if (Bitmap != null)
                {
                    Bitmap.Dispose();
                }
                Bitmap = newBitmap;

                _texture = Texture.FromMemory(
                    Device, (byte[]) new ImageConverter().ConvertTo(newBitmap, typeof(byte[])), Width, Height, 0,
                    Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);

                if (_originalTexture == null)
                {
                    _originalTexture = _texture;
                }
            }


            /// <summary>
            /// Called when the scene has ended.
            /// </summary>
            public override void OnEndScene()
            {
                try
                {
                    if (_sprite.IsDisposed || _texture.IsDisposed || !Position.IsValid() || _hide)
                    {
                        return;
                    }

                    _sprite.Begin();
                    var matrix = _sprite.Transform;
                    var nMatrix = (Matrix.Scaling(Scale.X, Scale.Y, 0)) * Matrix.RotationZ(Rotation) *
                                  Matrix.Translation(Position.X, Position.Y, 0);
                    _sprite.Transform = nMatrix;
                    _sprite.Draw(_texture, _color, _crop);
                    _sprite.Transform = matrix;
                    _sprite.End();
                }
                catch (Exception e)
                {
                    Reset();
                    Console.WriteLine(@"Common.Render.Sprite.OnEndScene: " + e);
                }
            }


            /// <summary>
            /// Called before the DirectX device is reset..
            /// </summary>
            public override void OnPreReset()
            {
                _sprite.OnLostDevice();
            }

            /// <summary>
            /// Called after the DirectX device is reset.
            /// </summary>
            public override void OnPostReset()
            {
                _sprite.OnResetDevice();
            }

            /// <summary>
            /// Disposes this instance.
            /// </summary>
            public override void Dispose()
            {
                Game.OnUpdate -= Game_OnUpdate;
                if (!_sprite.IsDisposed)
                {
                    _sprite.Dispose();
                }

                if (!_texture.IsDisposed)
                {
                    _texture.Dispose();
                }

                if (!_originalTexture.IsDisposed)
                {
                    _originalTexture.Dispose();
                }
            }
        }

        /// <summary>
        /// Object used to draw text on the screen.
        /// </summary>
        public class Text : RenderObject
        {
            /// <summary>
            /// Delegate that gets the position of the text.
            /// </summary>
            /// <returns>Vector2.</returns>
            public delegate Vector2 PositionDelegate();

            /// <summary>
            /// Delegate that gets the text.
            /// </summary>
            /// <returns>System.String.</returns>
            public delegate string TextDelegate();

            /// <summary>
            /// The DirectX text font
            /// </summary>
            private Font _textFont;

            /// <summary>
            /// The x
            /// </summary>
            private int _x;

            /// <summary>
            /// The y
            /// </summary>
            private int _y;

            /// <summary>
            /// The calculated x
            /// </summary>
            private int _xCalculated;

            /// <summary>
            /// The calculated y
            /// </summary>
            private int _yCalculated;

            /// <summary>
            /// <c>true</c> if the text should be centered at the position.
            /// </summary>
            public bool Centered = false;

            /// <summary>
            /// The offset
            /// </summary>
            public Vector2 Offset;

            /// <summary>
            /// <c>true</c> if the text should have an outline.
            /// </summary>
            public bool OutLined = false;

            /// <summary>
            /// The delegate that updates the position of the text.
            /// </summary>
            public PositionDelegate PositionUpdate;

            /// <summary>
            /// The delegate that updates the text.
            /// </summary>
            public TextDelegate TextUpdate;

            /// <summary>
            /// The unit
            /// </summary>
            public Obj_AI_Base Unit;

            /// <summary>
            /// Initializes a new instance of the <see cref="Text"/> class.
            /// </summary>
            /// <param name="text">The text.</param>
            /// <param name="fontName">Name of the font.</param>
            /// <param name="size">The size.</param>
            /// <param name="color">The color.</param>
            private Text(string text, string fontName, int size, ColorBGRA color)
            {
                _textFont = new Font(
                    Device,
                    new FontDescription
                    {
                        FaceName = fontName,
                        Height = size,
                        OutputPrecision = FontPrecision.Default,
                        Quality = FontQuality.Default
                    });
                Color = color;
                this.text = text;
                Game.OnUpdate += Game_OnUpdate;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Text"/> class.
            /// </summary>
            /// <param name="text">The text.</param>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <param name="size">The size.</param>
            /// <param name="color">The color.</param>
            /// <param name="fontName">Name of the font.</param>
            public Text(string text, int x, int y, int size, ColorBGRA color, string fontName = "Calibri") : this(text, fontName, size, color)
            {
                _x = x;
                _y = y;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Text"/> class.
            /// </summary>
            /// <param name="text">The text.</param>
            /// <param name="position">The position.</param>
            /// <param name="size">The size.</param>
            /// <param name="color">The color.</param>
            /// <param name="fontName">Name of the font.</param>
            public Text(string text, Vector2 position, int size, ColorBGRA color, string fontName = "Calibri")
                : this(text, fontName, size, color)
            {
                _x = (int) position.X;
                _y = (int) position.Y;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Text"/> class.
            /// </summary>
            /// <param name="text">The text.</param>
            /// <param name="unit">The unit.</param>
            /// <param name="offset">The offset.</param>
            /// <param name="size">The size.</param>
            /// <param name="color">The color.</param>
            /// <param name="fontName">Name of the font.</param>
            public Text(string text,
                Obj_AI_Base unit,
                Vector2 offset,
                int size,
                ColorBGRA color,
                string fontName = "Calibri")
                : this(text, fontName, size, color)
            {
                Unit = unit;
                Offset = offset;

                var pos = unit.HPBarPosition + offset;

                _x = (int) pos.X;
                _y = (int) pos.Y;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Text"/> class.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <param name="text">The text.</param>
            /// <param name="size">The size.</param>
            /// <param name="color">The color.</param>
            /// <param name="fontName">Name of the font.</param>
            public Text(int x, int y, string text, int size, ColorBGRA color, string fontName = "Calibri")
                : this(text, fontName, size, color)
            {
                _x = x;
                _y = y;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Text"/> class.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <param name="text">The text.</param>
            /// <param name="size">The size.</param>
            /// <param name="color">The color.</param>
            /// <param name="fontName">Name of the font.</param>
            public Text(Vector2 position, string text, int size, ColorBGRA color, string fontName = "Calibri")
                : this(text, fontName, size, color)
            {
                _x = (int) position.X;
                _y = (int) position.Y;
            }

            /// <summary>
            /// Gets or sets the text font description.
            /// </summary>
            /// <value>The text font description.</value>
            public FontDescription TextFontDescription
            {
                get { return _textFont.Description; }

                set
                {
                    _textFont.Dispose();
                    _textFont = new Font(Device, value);
                }
            }

            /// <summary>
            /// Game_s the on update.
            /// </summary>
            /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
            private void Game_OnUpdate(EventArgs args)
            {
                if (Visible)
                {
                    if (TextUpdate != null)
                    {
                        text = TextUpdate();
                    }

                    if (PositionUpdate != null && !string.IsNullOrEmpty(text))
                    {
                        Vector2 pos = PositionUpdate();
                        _xCalculated = (int) pos.X + XOffset;
                        _yCalculated = (int) pos.Y + YOffset;
                    }
                }
            }

            /// <summary>
            /// Gets or sets the x.
            /// </summary>
            /// <value>The x.</value>
            public int X
            {
                get
                {
                    if (PositionUpdate != null)
                    {
                        return _xCalculated;
                    }
                    return _x + XOffset;
                }
                set { _x = value; }
            }

            /// <summary>
            /// Gets or sets the y.
            /// </summary>
            /// <value>The y.</value>
            public int Y
            {
                get
                {
                    if (PositionUpdate != null)
                    {
                        return _yCalculated;
                    }
                    return _y + YOffset;
                }
                set { _y = value; }
            }

            /// <summary>
            /// Gets the x offset.
            /// </summary>
            /// <value>The x offset.</value>
            private int XOffset
            {
                get { return Centered ? -Width / 2 : 0; }
            }

            /// <summary>
            /// Gets the y offset.
            /// </summary>
            /// <value>The y offset.</value>
            private int YOffset
            {
                get { return Centered ? -Height / 2 : 0; }
            }

            /// <summary>
            /// Gets the width.
            /// </summary>
            /// <value>The width.</value>
            public int Width { get; private set; }

            /// <summary>
            /// Gets the height.
            /// </summary>
            /// <value>The height.</value>
            public int Height { get; private set; }

            /// <summary>
            /// Gets or sets the color.
            /// </summary>
            /// <value>The color.</value>
            public ColorBGRA Color { get; set; }

            /// <summary>
            /// The text
            /// </summary>
            private string _text;

            /// <summary>
            /// Gets or sets the text.
            /// </summary>
            /// <value>The text.</value>
            public string text
            {
                get { return _text; }
                set
                {
                    if (value != _text && _textFont != null && !_textFont.IsDisposed && !string.IsNullOrEmpty(value))
                    {
                        SharpDX.Rectangle size = _textFont.MeasureText(null, value, 0);
                        Width = size.Width;
                        Height = size.Height;
                        _textFont.PreloadText(value);
                    }
                    _text = value;
                }
            }

            /// <summary>
            /// Called when the scene has ended.
            /// </summary>
            public override void OnEndScene()
            {
                try
                {
                    if (_textFont.IsDisposed || text == "")
                    {
                        return;
                    }

                    if (Unit != null && Unit.IsValid)
                    {
                        var pos = Unit.HPBarPosition + Offset;
                        X = (int) pos.X;
                        Y = (int) pos.Y;
                    }

                    var xP = X;
                    var yP = Y;
                    if (OutLined)
                    {
                        var outlineColor = new ColorBGRA(0, 0, 0, 255);
                        _textFont.DrawText(null, text, xP - 1, yP - 1, outlineColor);
                        _textFont.DrawText(null, text, xP + 1, yP + 1, outlineColor);
                        _textFont.DrawText(null, text, xP - 1, yP, outlineColor);
                        _textFont.DrawText(null, text, xP + 1, yP, outlineColor);
                    }
                    _textFont.DrawText(null, text, xP, yP, Color);
                }
                catch (Exception e)
                {
                    Console.WriteLine(@"Common.Render.Text.OnEndScene: " + e);
                }
            }

            /// <summary>
            /// Called before the DirectX device is reset.
            /// </summary>
            public override void OnPreReset()
            {
                _textFont.OnLostDevice();
            }

            /// <summary>
            /// Called after the DirectX device has been reset.
            /// </summary>
            public override void OnPostReset()
            {
                _textFont.OnResetDevice();
            }

            /// <summary>
            /// Disposes this instance.
            /// </summary>
            public override void Dispose()
            {
                Game.OnUpdate -= Game_OnUpdate;
                if (!_textFont.IsDisposed)
                {
                    _textFont.Dispose();
                }
            }
        }
    }

    /// <summary>
    /// Provides extensions for fonts.
    /// </summary>
    public static class FontExtension
    {
        /// <summary>
        /// The widths
        /// </summary>
        private static readonly Dictionary<Font, Dictionary<string, Rectangle>> Widths = new Dictionary<Font, Dictionary<string, Rectangle>>();

        /// <summary>
        /// Measures the text.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="sprite">The sprite.</param>
        /// <param name="text">The text.</param>
        /// <returns>Rectangle.</returns>
        public static Rectangle MeasureText(this Font font, Sprite sprite, string text)
        {
            Dictionary<string, Rectangle> rectangles;
            if (!Widths.TryGetValue(font, out rectangles))
            {
                rectangles = new Dictionary<string, Rectangle>();
                Widths[font] = rectangles;
            }

            Rectangle rectangle;
            if (rectangles.TryGetValue(text, out rectangle))
            {
                return rectangle;
            }
            rectangle = font.MeasureText(sprite, text, 0);
            rectangles[text] = rectangle;
            return rectangle;
        }

        /// <summary>
        /// Measures the text.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="text">The text.</param>
        /// <returns>Rectangle.</returns>
        public static Rectangle MeasureText(this Font font, string text)
        {
            return font.MeasureText(null, text);
        }
    }
}