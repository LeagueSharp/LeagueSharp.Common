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

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    ///     The render class allows you to draw stuff using SharpDX easier.
    /// </summary>
    public static class Render
    {
        private static readonly List<RenderObject> RenderObjects = new List<RenderObject>();
        private static List<RenderObject> _renderVisibleObjects = new List<RenderObject>();
        private static bool _cancelThread;
        private static readonly object RenderObjectsLock = new object();

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

        public static Device Device
        {
            get { return Drawing.Direct3DDevice; }
        }

        public static bool OnScreen(Vector2 point)
        {
            return point.X > 0 && point.Y > 0 && point.X < Drawing.Width && point.Y < Drawing.Height;
        }

        private static void CurrentDomainOnDomainUnload(object sender, EventArgs eventArgs)
        {
            _cancelThread = true;
            foreach (var renderObject in RenderObjects)
            {
                renderObject.Dispose();
            }
        }

        private static void DrawingOnOnPostReset(EventArgs args)
        {
            foreach (var renderObject in RenderObjects)
            {
                renderObject.OnPostReset();
            }
        }

        private static void DrawingOnOnPreReset(EventArgs args)
        {
            foreach (var renderObject in RenderObjects)
            {
                renderObject.OnPreReset();
            }
        }

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

        public static RenderObject Add(this RenderObject renderObject, int layer = int.MaxValue)
        {
            renderObject.Layer = layer != int.MaxValue ? layer : renderObject.Layer;
            lock (RenderObjectsLock)
            {
                RenderObjects.Add(renderObject);
            }
            return renderObject;
        }

        public static void Remove(this RenderObject renderObject)
        {
            lock (RenderObjectsLock)
            {
                RenderObjects.Remove(renderObject);
            }
        }

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

        public class Circle : RenderObject
        {
            private static VertexBuffer _vertices;
            private static VertexElement[] _vertexElements;
            private static VertexDeclaration _vertexDeclaration;
            private static Effect _effect;
            private static EffectHandle _technique;
            private static bool _initialized;
            private static Vector3 _offset = new Vector3(0, 0, 0);

            public Circle(GameObject unit, float radius, Color color, int width = 1, bool zDeep = false)
            {
                Color = color;
                Unit = unit;
                Radius = radius;
                Width = width;
                ZDeep = zDeep;
            }

            public Circle(GameObject unit, Vector3 offset, float radius, Color color, int width = 1, bool zDeep = false)
            {
                Color = color;
                Unit = unit;
                Radius = radius;
                Width = width;
                ZDeep = zDeep;
                Offset = offset;
            }

            public Circle(Vector3 position, Vector3 offset, float radius, Color color, int width = 1, bool zDeep = false)
            {
                Color = color;
                Position = position;
                Radius = radius;
                Width = width;
                ZDeep = zDeep;
                Offset = offset;
            }

            public Circle(Vector3 position, float radius, Color color, int width = 1, bool zDeep = false)
            {
                Color = color;
                Position = position;
                Radius = radius;
                Width = width;
                ZDeep = zDeep;
            }

            public Vector3 Position { get; set; }
            public GameObject Unit { get; set; }
            public float Radius { get; set; }
            public Color Color { get; set; }
            public int Width { get; set; }
            public bool ZDeep { get; set; }

            public Vector3 Offset
            {
                get { return _offset; }
                set { _offset = value; }
            }

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

            public static void CreateVertexes()
            {
                const float x = 6000f;
                _vertices = new VertexBuffer(
                    Device, Utilities.SizeOf<Vector4>() * 2 * 6, Usage.WriteOnly, VertexFormat.None, Pool.Managed);

                _vertices.Lock(0, 0, LockFlags.None).WriteRange(
                    new[]
                    {
                        //T1
                        new Vector4(-x, 0f, -x, 1.0f), new Vector4(), new Vector4(-x, 0f, x, 1.0f), new Vector4(),
                        new Vector4(x, 0f, -x, 1.0f), new Vector4(),

                        //T2
                        new Vector4(x, 0f, x, 1.0f), new Vector4(), new Vector4(-x, 0f, x, 1.0f), new Vector4(),
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

            private static void OnPreReset(EventArgs args)
            {
                if (_effect != null && !_effect.IsDisposed)
                {
                    _effect.OnLostDevice();
                }
            }

            private static void OnPostReset(EventArgs args)
            {
                if (_effect != null && !_effect.IsDisposed)
                {
                    _effect.OnResetDevice();
                }
            }

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

        public class Line : RenderObject
        {
            public delegate Vector2 PositionDelegate();

            private readonly SharpDX.Direct3D9.Line _line;
            private Vector2 _end;
            private Vector2 _start;
            private int _width;
            public ColorBGRA Color;

            public Line(Vector2 start, Vector2 end, int width, ColorBGRA color)
            {
                _line = new SharpDX.Direct3D9.Line(Device);
                Width = width;
                Color = color;
                _start = start;
                _end = end;
            }

            public Vector2 Start
            {
                get { return StartPositionUpdate != null ? StartPositionUpdate() : _start; }
                set { _start = value; }
            }

            public Vector2 End
            {
                get { return EndPositionUpdate != null ? EndPositionUpdate() : _end; }
                set { _end = value; }
            }

            public PositionDelegate StartPositionUpdate { get; set; }
            public PositionDelegate EndPositionUpdate { get; set; }

            public int Width
            {
                get { return _width; }
                set
                {
                    _line.Width = value;
                    _width = value;
                }
            }

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

            public override void OnPreReset()
            {
                _line.OnLostDevice();
            }

            public override void OnPostReset()
            {
                _line.OnResetDevice();
            }

            public override void Dispose()
            {
                if (!_line.IsDisposed)
                {
                    _line.Dispose();
                }
            }
        }

        public class Rectangle : RenderObject
        {
            public delegate Vector2 PositionDelegate();

            private readonly SharpDX.Direct3D9.Line _line;
            private int _x;
            private int _y;
            public ColorBGRA Color;

            public Rectangle(int x, int y, int width, int height, ColorBGRA color)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                Color = color;
                _line = new SharpDX.Direct3D9.Line(Device) { Width = height };
            }

            public int X
            {
                get
                {
                    if (PositionUpdate != null)
                    {
                        return (int) PositionUpdate().X;
                    }
                    return _x;
                }
                set { _x = value; }
            }

            public int Y
            {
                get
                {
                    if (PositionUpdate != null)
                    {
                        return (int) PositionUpdate().Y;
                    }
                    return _y;
                }
                set { _y = value; }
            }

            public int Width { get; set; }
            public int Height { get; set; }
            public PositionDelegate PositionUpdate { get; set; }

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

            public override void OnPreReset()
            {
                _line.OnLostDevice();
            }

            public override void OnPostReset()
            {
                _line.OnResetDevice();
            }

            public override void Dispose()
            {
                if (!_line.IsDisposed)
                {
                    _line.Dispose();
                }
            }
        }

        public class RenderObject : IDisposable
        {
            public delegate bool VisibleConditionDelegate(RenderObject sender);

            private bool _visible = true;
            public int Layer;
            public VisibleConditionDelegate VisibleCondition;

            public bool Visible
            {
                get { return VisibleCondition != null ? VisibleCondition(this) : _visible; }
                set { _visible = value; }
            }

            public virtual void Dispose() {}
            public virtual void OnDraw() {}
            public virtual void OnEndScene() {}
            public virtual void OnPreReset() {}
            public virtual void OnPostReset() {}

            public bool HasValidLayer()
            {
                return Layer >= -5 && Layer <= 5;
            }
        }

        public class Sprite : RenderObject
        {
            public delegate void OnResetting(Sprite sprite);

            public delegate Vector2 PositionDelegate();

            private readonly SharpDX.Direct3D9.Sprite _sprite = new SharpDX.Direct3D9.Sprite(Device);
            private ColorBGRA _color = SharpDX.Color.White;
            private SharpDX.Rectangle? _crop;
            private bool _hide;
            private Texture _originalTexture;
            private Vector2 _scale = new Vector2(1, 1);
            private Texture _texture;
            private int _x;
            private int _y;

            public Sprite(Bitmap bitmap, Vector2 position)
            {
                UpdateTextureBitmap(bitmap, position);
            }

            public Sprite(BaseTexture texture, Vector2 position)
            {
                UpdateTextureBitmap(
                    (Bitmap) Image.FromStream(BaseTexture.ToStream(texture, ImageFileFormat.Bmp)), position);
            }

            public Sprite(Stream stream, Vector2 position)
            {
                UpdateTextureBitmap(new Bitmap(stream), position);
            }

            public Sprite(byte[] bytesArray, Vector2 position)
            {
                UpdateTextureBitmap((Bitmap) Image.FromStream(new MemoryStream(bytesArray)), position);
            }

            public Sprite(string fileLocation, Vector2 position)
            {
                if (!File.Exists((fileLocation)))
                {
                    return;
                }

                UpdateTextureBitmap(new Bitmap(fileLocation), position);
            }

            public int X
            {
                get
                {
                    if (PositionUpdate != null)
                    {
                        return (int) PositionUpdate().X;
                    }
                    return _x;
                }
                set { _x = value; }
            }

            public int Y
            {
                get
                {
                    if (PositionUpdate != null)
                    {
                        return (int) PositionUpdate().Y;
                    }
                    return _y;
                }
                set { _y = value; }
            }

            public Bitmap Bitmap { get; set; }

            public int Width
            {
                get { return (int) (Bitmap.Width * _scale.X); }
            }

            public int Height
            {
                get { return (int) (Bitmap.Height * _scale.Y); }
            }

            public Vector2 Size
            {
                get { return new Vector2(Bitmap.Width, Bitmap.Height); }
            }

            public Vector2 Position
            {
                set
                {
                    X = (int) value.X;
                    Y = (int) value.Y;
                }

                get { return new Vector2(X, Y); }
            }

            public PositionDelegate PositionUpdate { get; set; }

            public Vector2 Scale
            {
                set { _scale = value; }
                get { return _scale; }
            }

            public float Rotation { set; get; }

            public ColorBGRA Color
            {
                set { _color = value; }
                get { return _color; }
            }

            public event OnResetting OnReset;

            public void Crop(int x, int y, int w, int h, bool scale = false)
            {
                _crop = new SharpDX.Rectangle(x, y, w, h);

                if (scale)
                {
                    _crop = new SharpDX.Rectangle(
                        (int) (_scale.X * x), (int) (_scale.Y * y), (int) (_scale.X * w), (int) (_scale.Y * h));
                }
            }

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

            public void Show()
            {
                _hide = false;
            }

            public void Hide()
            {
                _hide = true;
            }

            public void Reset()
            {
                UpdateTextureBitmap(
                    (Bitmap) Image.FromStream(BaseTexture.ToStream(_originalTexture, ImageFileFormat.Bmp)));

                if (OnReset != null)
                {
                    OnReset(this);
                }
            }

            public void GrayScale()
            {
                SetSaturation(0.0f);
            }

            public void Fade()
            {
                SetSaturation(0.5f);
            }

            public void Complement()
            {
                SetSaturation(-1.0f);
            }

            public void SetSaturation(float saturiation)
            {
                UpdateTextureBitmap(SaturateBitmap(Bitmap, saturiation));
            }

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

            public override void OnPreReset()
            {
                _sprite.OnLostDevice();
            }

            public override void OnPostReset()
            {
                _sprite.OnResetDevice();
            }

            public override void Dispose()
            {
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
        ///     Object used to draw text on the screen.
        /// </summary>
        public class Text : RenderObject
        {
            public delegate Vector2 PositionDelegate();

            public delegate string TextDelegate();

            private string _text;
            private Font _textFont;
            private int _x;
            private int _y;
            public bool Centered = false;
            public Vector2 Offset;
            public bool OutLined = false;
            public PositionDelegate PositionUpdate;
            public TextDelegate TextUpdate;
            public Obj_AI_Base Unit;

            public Text(string text, int x, int y, int size, ColorBGRA color, string fontName = "Calibri")
            {
                Color = color;
                this.text = text;

                _x = x;
                _y = y;

                _textFont = new Font(
                    Device,
                    new FontDescription
                    {
                        FaceName = fontName,
                        Height = size,
                        OutputPrecision = FontPrecision.Default,
                        Quality = FontQuality.Default
                    });
            }

            public Text(string text, Vector2 position, int size, ColorBGRA color, string fontName = "Calibri")
            {
                Color = color;
                this.text = text;

                _x = (int) position.X;
                _y = (int) position.Y;

                _textFont = new Font(
                    Device,
                    new FontDescription
                    {
                        FaceName = fontName,
                        Height = size,
                        OutputPrecision = FontPrecision.Default,
                        Quality = FontQuality.Default
                    });
            }

            public Text(string text,
                Obj_AI_Base unit,
                Vector2 offset,
                int size,
                ColorBGRA color,
                string fontName = "Calibri")
            {
                Unit = unit;
                Color = color;
                this.text = text;
                Offset = offset;

                var pos = unit.HPBarPosition + offset;

                _x = (int) pos.X;
                _y = (int) pos.Y;

                _textFont = new Font(
                    Device,
                    new FontDescription
                    {
                        FaceName = fontName,
                        Height = size,
                        OutputPrecision = FontPrecision.Default,
                        Quality = FontQuality.Default
                    });
            }

            public Text(int x, int y, string text, int size, ColorBGRA color, string fontName = "Calibri")
            {
                Color = color;
                this.text = text;

                _x = x;
                _y = y;

                _textFont = new Font(
                    Device,
                    new FontDescription
                    {
                        FaceName = fontName,
                        Height = size,
                        OutputPrecision = FontPrecision.Default,
                        Quality = FontQuality.Default
                    });
            }

            public Text(Vector2 position, string text, int size, ColorBGRA color, string fontName = "Calibri")
            {
                Color = color;
                this.text = text;
                _x = (int) position.X;
                _y = (int) position.Y;
                _textFont = new Font(
                    Device,
                    new FontDescription
                    {
                        FaceName = fontName,
                        Height = size,
                        OutputPrecision = FontPrecision.Default,
                        Quality = FontQuality.Default
                    });
            }

            public FontDescription TextFontDescription
            {
                get { return _textFont.Description; }

                set
                {
                    _textFont.Dispose();
                    _textFont = new Font(Device, value);
                }
            }

            public int X
            {
                get
                {
                    var dx = Centered ? -_textFont.MeasureText(null, text, FontDrawFlags.Center).Width / 2 : 0;

                    if (PositionUpdate != null)
                    {
                        return (int) PositionUpdate().X + dx;
                    }

                    return _x + dx;
                }
                set { _x = value; }
            }

            public int Y
            {
                get
                {
                    var dy = Centered ? -_textFont.MeasureText(null, text, FontDrawFlags.Center).Height / 2 : 0;

                    if (PositionUpdate != null)
                    {
                        return (int) PositionUpdate().Y + dy;
                    }
                    return _y + dy;
                }
                set { _y = value; }
            }

            public int Width { get; set; }
            public ColorBGRA Color { get; set; }

            public string text
            {
                get
                {
                    if (TextUpdate != null)
                    {
                        return TextUpdate();
                    }
                    return _text;
                }
                set { _text = value; }
            }

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

            public override void OnPreReset()
            {
                _textFont.OnLostDevice();
            }

            public override void OnPostReset()
            {
                _textFont.OnResetDevice();
            }

            public override void Dispose()
            {
                if (!_textFont.IsDisposed)
                {
                    _textFont.Dispose();
                }
            }
        }
    }
}