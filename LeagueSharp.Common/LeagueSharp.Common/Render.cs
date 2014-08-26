#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Orbwalking.cs is part of LeagueSharp.Common.
 
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
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using SharpDX;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;
using Font = SharpDX.Direct3D9.Font;
using Matrix = SharpDX.Matrix;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    ///     The render class allows you to draw stuff using SharpDX easier.
    /// </summary>
    public static class Render
    {
        private static readonly List<RenderObject> RenderObjects = new List<RenderObject>();

        static Render()
        {
            Drawing.OnEndScene += Drawing_OnEndScene;
            Drawing.OnPreReset += DrawingOnOnPreReset;
            Drawing.OnPostReset += DrawingOnOnPostReset;
            Drawing.OnDraw += Drawing_OnDraw;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainOnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnDomainUnload;
        }


        private static void CurrentDomainOnDomainUnload(object sender, EventArgs eventArgs)
        {
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
            if (Drawing.Direct3DDevice == null || Drawing.Direct3DDevice.IsDisposed)
            {
                return;
            }

            for (var i = -5; i < 5; i++)
            {
                foreach (var renderObject in
                    RenderObjects.Where(renderObject => renderObject.Layer == i && renderObject.Visible))
                {
                    renderObject.OnDraw();
                }
            }
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (Drawing.Direct3DDevice == null || Drawing.Direct3DDevice.IsDisposed)
            {
                return;
            }

            for (var i = -5; i < 5; i++)
            {
                foreach (var renderObject in
                    RenderObjects.Where(renderObject => renderObject.Layer == i && renderObject.Visible))
                {
                    renderObject.OnEndScene();
                }
            }
        }

        public static RenderObject Add(this RenderObject renderObject, int layer = int.MaxValue)
        {
            renderObject.Layer = layer != int.MaxValue ? layer : renderObject.Layer;
            RenderObjects.Add(renderObject);
            return renderObject;
        }

        public class Circle : RenderObject
        {
            private static VertexBuffer _vertices;
            private static VertexElement[] _vertexElements;
            private static VertexDeclaration _vertexDeclaration;
            private static Effect _effect;
            private static EffectHandle _technique;

            public Circle(Obj_AI_Base unit, float radius, Color color, int width = 1, bool zDeep = false)
            {
                Color = color;
                Unit = unit;
                Radius = radius;
                Width = width;
                ZDeep = zDeep;
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
            public Obj_AI_Base Unit { get; set; }

            public float Radius { get; set; }
            public Color Color { get; set; }
            public int Width { get; set; }
            public bool ZDeep { get; set; }

            public override void OnDraw()
            {
                try
                {
                    if (Unit != null && Unit.IsValid)
                    {
                        DrawCircle(Unit.Position, Radius, Color, Width, ZDeep);
                    }
                    else if (Position.To2D().IsValid())
                    {
                        DrawCircle(Position, Radius, Color, Width, ZDeep);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(@"Common.Render.Circle.OnEndScene: " + e);
                }
            }

            public static void DrawCircle(Vector3 position, float radius, Color color, int width = 1, bool zDeep = false)
            {
                if (_vertices == null)
                {
                    const float x = 6000f;
                    _vertices = new VertexBuffer(
                        Drawing.Direct3DDevice, Utilities.SizeOf<Vector4>() * 2 * 6, Usage.WriteOnly, VertexFormat.None,
                        Pool.Managed);

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

                    _vertexDeclaration = new VertexDeclaration(Drawing.Direct3DDevice, _vertexElements);

                    #region Effect

                    _effect = Effect.FromString(Drawing.Direct3DDevice, @"
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
                                AlphaBlendEnable = TRUE;
                                DestBlend = INVSRCALPHA;
                                SrcBlend = SRCALPHA;
		                        VertexShader = compile vs_2_0 VS();
                                PixelShader  = compile ps_2_0 PS();
	                        }
                        }", ShaderFlags.None);

                    #endregion

                    _technique = _effect.GetTechnique(0);

                    Drawing.OnPreReset += delegate { _effect.OnLostDevice(); };
                    Drawing.OnPostReset += delegate { _effect.OnResetDevice(); };
                    AppDomain.CurrentDomain.DomainUnload += delegate
                    {
                        _effect.Dispose();
                        _vertices.Dispose();
                        _vertexDeclaration.Dispose();
                    };
                }


                if (_vertices.IsDisposed || _vertexDeclaration.IsDisposed || _effect.IsDisposed)
                {
                    return;
                }

                if (zDeep)
                {
                    Drawing.Direct3DDevice.SetRenderState(RenderState.ZEnable, true);
                }

                var olddec = Drawing.Direct3DDevice.VertexDeclaration;

                _effect.Technique = _technique;
                _effect.Begin();
                _effect.BeginPass(0);
                _effect.SetValue(
                    "ProjectionMatrix", Matrix.Translation(position.SwitchYZ()) * Drawing.View * Drawing.Projection);
                _effect.SetValue(
                    "CircleColor", new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f));
                _effect.SetValue("Radius", radius);
                _effect.SetValue("Border", 2f + width);

                Drawing.Direct3DDevice.SetStreamSource(0, _vertices, 0, Utilities.SizeOf<Vector4>() * 2);
                Drawing.Direct3DDevice.VertexDeclaration = _vertexDeclaration;

                Drawing.Direct3DDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

                _effect.EndPass();
                _effect.End();

                if (zDeep)
                {
                    Drawing.Direct3DDevice.SetRenderState(RenderState.ZEnable, false);
                }

                Drawing.Direct3DDevice.VertexDeclaration = olddec;
            }
        }

        public class Rectangle : RenderObject
        {
            private readonly Line _line;
            public ColorBGRA Color;

            public Rectangle(int x, int y, int width, int height, ColorBGRA color)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                Color = color;
                _line = new Line(Drawing.Direct3DDevice) { Width = height };
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }

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

        public class RenderObject
        {
            public int Layer = 0;
            public bool Visible = true;

            public virtual void OnDraw() { }
            public virtual void OnEndScene() { }
            public virtual void OnPreReset() { }
            public virtual void OnPostReset() { }
            public virtual void Dispose() { }
        }

        public class Sprite : RenderObject
        {
            public delegate Vector2 PositionDelegate();

            private readonly SharpDX.Direct3D9.Sprite _sprite = new SharpDX.Direct3D9.Sprite(Drawing.Direct3DDevice);
            private ColorBGRA _color = SharpDX.Color.White;
            private bool _hide;
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
                UpdateTextureBitmap((Bitmap) Image.FromStream(stream), position);
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
                get { return Bitmap.Width; }
            }

            public int Height
            {
                get { return Bitmap.Height; }
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
                set
                {
                    _scale = value;

                    var stream = BaseTexture.ToStream(_texture, ImageFileFormat.Bmp);
                    var original = new Bitmap(stream);
                    var target = new Bitmap((int) (_scale.X * original.Width), (int) (_scale.Y * original.Height));

                    using (var g = Graphics.FromImage(target))
                    {
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.DrawImage(original, 0, 0, target.Width, target.Height);
                    }
                    UpdateTextureBitmap(target);
                }

                get { return _scale; }
            }

            public ColorBGRA Color
            {
                set { _color = value; }
                get { return _color; }
            }

            public void Show()
            {
                _hide = false;
            }

            public void Hide()
            {
                _hide = true;
            }

            public void UpdateTextureBitmap(Bitmap newBitmap, Vector2 position = new Vector2())
            {
                if (position.IsValid())
                {
                    Position = position;
                }

                Bitmap = newBitmap;
                _texture = Texture.FromMemory(
                    Drawing.Direct3DDevice, (byte[]) new ImageConverter().ConvertTo(newBitmap, typeof(byte[])), Width,
                    Height, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
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
                    _sprite.Draw(_texture, _color, null, new Vector3(-X, -Y, 0));
                    _sprite.End();
                }
                catch (Exception e)
                {
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
            }
        }

        /// <summary>
        /// Object used to draw text on the screen.
        /// </summary>
        public class Text : RenderObject
        {
            private readonly Font _textFont;
            public FontDescription TextFontDescription;

            public Text(int x, int y, string text, int size, ColorBGRA color, string faceName = "Calibri")
            {
                Color = color;
                this.text = text;

                _textFont = new Font(
                    Drawing.Direct3DDevice,
                    new FontDescription
                    {
                        FaceName = faceName,
                        Height = size,
                        OutputPrecision = FontPrecision.Default,
                        Quality = FontQuality.Default,
                    });
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public ColorBGRA Color { get; set; }
            public string text { get; set; }

            public override void OnEndScene()
            {
                try
                {
                    if (_textFont.IsDisposed)
                    {
                        return;
                    }

                    _textFont.DrawText(null, text, X, Y, Color);
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