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
using System.IO;
using System.Linq;

using SharpDX;
using SharpDX.Direct3D9;
using Font = SharpDX.Direct3D9.Font;
using Color = System.Drawing.Color;

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
            Drawing.OnDraw +=Drawing_OnDraw;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainOnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnDomainUnload;
        }


        private static void CurrentDomainOnDomainUnload(object sender, EventArgs eventArgs)
        {
            foreach (var renderObject in RenderObjects)
            {
                renderObject.OnUnload();
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

        public class RenderObject
        {
            public int Layer = 0;
            public bool Visible = true;

            public virtual void OnDraw() { }
            public virtual void OnEndScene() { }
            public virtual void OnPreReset() { }

            public virtual void OnPostReset() { }
            public virtual void OnUnload() { }
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

            public override void OnUnload()
            {
                _line.Dispose();
            }
        }

        public class Sprite : RenderObject
        {
            private readonly SharpDX.Direct3D9.Sprite _sprite;
            public int X = 0;
            public int Y = 0;
            private ColorBGRA _color = SharpDX.Color.White;

            private int _height;
            private bool _hide;
            private Vector2 _scale = new Vector2(1, 1);
            private Texture _texture;
            private int _width;

            public Sprite(Bitmap bitmap, Vector2 position)
            {
                Position = position;
                Bitmap = bitmap;
                _sprite = new SharpDX.Direct3D9.Sprite(Drawing.Direct3DDevice);
                _texture = Texture.FromMemory(
                    Drawing.Direct3DDevice, (byte[]) new ImageConverter().ConvertTo(bitmap, typeof(byte[])),
                    bitmap.Width, bitmap.Height, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default,
                    0);
                _width = _texture.GetLevelDescription(0).Width;
                _height = _texture.GetLevelDescription(0).Height;
            }

            public Sprite(Texture texture, Vector2 position)
            {
                _sprite = new SharpDX.Direct3D9.Sprite(Drawing.Direct3DDevice);
                _texture = texture;
                Position = position;
                _width = _texture.GetLevelDescription(0).Width;
                _height = _texture.GetLevelDescription(0).Height;
            }

            public Sprite(Stream stream, Vector2 position)
            {
                Position = position;
                _sprite = new SharpDX.Direct3D9.Sprite(Drawing.Direct3DDevice);
                _texture = Texture.FromStream(Drawing.Direct3DDevice, stream);
                _width = _texture.GetLevelDescription(0).Width;
                _height = _texture.GetLevelDescription(0).Height;
            }

            public Sprite(byte[] bytesArray, Vector2 position)
            {
                Position = position;
                _sprite = new SharpDX.Direct3D9.Sprite(Drawing.Direct3DDevice);
                _texture = Texture.FromStream(Drawing.Direct3DDevice, new MemoryStream(bytesArray));
                _width = _texture.GetLevelDescription(0).Width;
                _height = _texture.GetLevelDescription(0).Height;
            }

            public Sprite(string fileLocation, Vector2 position)
            {
                if (!File.Exists((fileLocation)))
                {
                    return;
                }

                Position = position;
                _sprite = new SharpDX.Direct3D9.Sprite(Drawing.Direct3DDevice);
                _texture = Texture.FromFile(Drawing.Direct3DDevice, fileLocation);
                _width = _texture.GetLevelDescription(0).Width;
                _height = _texture.GetLevelDescription(0).Height;
            }

            public Bitmap Bitmap { get; set; }

            public int Width
            {
                get { return (int) Size.X; }
            }

            public int Height
            {
                get { return (int) Size.Y; }
            }

            public Vector2 Size
            {
                get
                {
                    return new Vector2(_texture.GetLevelDescription(0).Width, _texture.GetLevelDescription(0).Height);
                }
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

            public Vector2 Scale
            {
                set
                {
                    _scale = value;
                    //Transform _texture
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

            public void UpdateTextureBitmap(Bitmap newBitmap)
            {
                Bitmap = newBitmap;
                _texture = Texture.FromMemory(
                    Drawing.Direct3DDevice, (byte[]) new ImageConverter().ConvertTo(newBitmap, typeof(byte[])), Width,
                    Height, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 0);
            }

            public override void OnEndScene()
            {
                try
                {
                    if (_sprite.IsDisposed || _texture.IsDisposed || _hide)
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

            public override void OnUnload()
            {
                _sprite.Dispose();
                _texture.Dispose();
            }
        }

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

            public override void OnUnload()
            {
                _textFont.Dispose();
            }
        }

        public class Circle : RenderObject
        {
            private static Line _line;

            public Vector3 Position { get; set; }
            public Obj_AI_Base Unit { get; set; }

            public float Radius { get; set; }
            public Color Color { get; set; }
            public bool Antialias { get; set; }
            public int Width { get; set; }
            public int Quality { get; set; }
            public bool ZDeep { get; set; }

            public Circle(Obj_AI_Base unit, float radius, Color color, bool antialias = true, int width = 1, bool zDeep = true, int quality = 24)
            {
                Color = color;
                Unit = unit;
                Radius = radius;
                Antialias = antialias;
                Width = width;
                Quality = quality;
                ZDeep = zDeep;
            }

            public Circle(Vector3 position, float radius, Color color, bool antialias = true, int width = 1, bool zDeep = true, int quality = 24)
            {
                Color = color;
                Position = position;
                Radius = radius;
                Antialias = antialias;
                Width = width;
                Quality = quality;
                ZDeep = zDeep;
            }

            public override void OnDraw()
            {
                try
                {
                    if (Unit != null && Unit.IsValid)
                    {
                        DrawCircle(Unit.Position, Radius, Color, Antialias, Width, ZDeep, Quality);
                    }
                    else if (Position.To2D().IsValid())
                    {
                        DrawCircle(Position, Radius, Color, Antialias, Width, ZDeep, Quality);
                    }
                   
                }
                catch (Exception e)
                {
                    Console.WriteLine(@"Common.Render.Circle.OnEndScene: " + e);
                }
            }

            public static void DrawCircle(Vector3 position, float radius, Color color, bool antialias = true, int width = 1, bool zDeep = true, int quality = 24)
            {
                if (_line == null)
                {
                    _line = new Line(Drawing.Direct3DDevice);
                    Drawing.OnPreReset += delegate { _line.OnLostDevice(); };
                    Drawing.OnPostReset += delegate { _line.OnResetDevice(); };
                    AppDomain.CurrentDomain.DomainUnload += delegate { _line.Dispose(); };
                }
                    

                if(_line.IsDisposed) return;

                _line.Width = width;
                _line.Antialias = antialias;

                if(zDeep)
                    Drawing.Direct3DDevice.SetRenderState(RenderState.ZEnable, true);
                
                _line.Begin();
                var v3 = new []{new Vector3(),new Vector3(), };
                for (var i = 0; i < quality; i++)
                {
                    var aAngle = i * Math.PI * 2 / quality;
                    var bAngle = (i + 1) * Math.PI * 2 / quality;

                    v3[0].X = position.X + radius * (float)Math.Cos(aAngle);
                    v3[0].Y = position.Z + 30;
                    v3[0].Z = position.Y + radius * (float)Math.Sin(aAngle);

                    v3[1].X = position.X + radius * (float)Math.Cos(bAngle);
                    v3[1].Y = position.Z + 30;
                    v3[1].Z = position.Y + radius * (float)Math.Sin(bAngle);

                    var aOnScreen = Drawing.WorldToScreen(v3[0].SwitchYZ());
                    if (aOnScreen.X > -Drawing.Width * 0.5f && aOnScreen.X < Drawing.Width * 1.5f && aOnScreen.Y > -Drawing.Height * 0.5f && aOnScreen.Y < Drawing.Height * 1.5f)
                        _line.DrawTransform(v3, Drawing.View * Drawing.Projection, new ColorBGRA(color.R, color.G, color.B, color.A));
                }
                _line.End();

                if (zDeep)
                    Drawing.Direct3DDevice.SetRenderState(RenderState.ZEnable, false);
            }

        }
    }
}