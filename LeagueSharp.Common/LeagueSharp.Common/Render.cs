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

        private static void DrawingOnOnPreReset(EventArgs args)
        {
            foreach (var renderObject in RenderObjects)
            {
                renderObject.OnPreReset();
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

            public override void OnUnload()
            {
                _line.Dispose();
            }
        }

        public class RenderObject
        {
            public int Layer = 0;
            public bool Visible = true;

            public virtual void OnEndScene() { }
            public virtual void OnPreReset() { }
            public virtual void OnUnload() { }
        }

        public class Sprite : RenderObject
        {
            private readonly SharpDX.Direct3D9.Sprite _sprite;
            public int X = 0;
            public int Y = 0;

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
                Size = new Vector2(_texture.GetLevelDescription(0).Width, _texture.GetLevelDescription(0).Height);
            }

            public Sprite(Texture texture, Vector2 position)
            {
                _sprite = new SharpDX.Direct3D9.Sprite(Drawing.Direct3DDevice);
                _texture = texture;
                Position = position;
                Size = new Vector2(_texture.GetLevelDescription(0).Width, _texture.GetLevelDescription(0).Height);
            }

            public Sprite(Stream stream, Vector2 position)
            {
                Position = position;
                _sprite = new SharpDX.Direct3D9.Sprite(Drawing.Direct3DDevice);
                _texture = Texture.FromStream(Drawing.Direct3DDevice, stream);
                Size = new Vector2(_texture.GetLevelDescription(0).Width, _texture.GetLevelDescription(0).Height);
            }

            public Sprite(byte[] bytesArray, Vector2 position)
            {
                Position = position;
                _sprite = new SharpDX.Direct3D9.Sprite(Drawing.Direct3DDevice);
                _texture = Texture.FromStream(Drawing.Direct3DDevice, new MemoryStream(bytesArray));
                Size = new Vector2(_texture.GetLevelDescription(0).Width, _texture.GetLevelDescription(0).Height);
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
                Size = new Vector2(_texture.GetLevelDescription(0).Width, _texture.GetLevelDescription(0).Height);
            }

            public int Width
            {
                set { _width = value; }
                get { return _width > 0 ? _width : Bitmap.Width; }
            }

            public int Height
            {
                set { _height = value; }
                get { return _height > 0 ? _height : Bitmap.Height; }
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

            public Vector2 Size
            {
                set
                {
                    _width = (int) value.X;
                    _height = (int) value.Y;
                }

                get
                {
                    return new Vector2(_texture.GetLevelDescription(0).Width, _texture.GetLevelDescription(0).Height);
                }
            }


            public Vector2 Scale
            {
                set
                {
                    _scale = value;
                    _width = (int) (_width * _scale.X);
                    _height = (int) (_width * _scale.Y);
                    //Transform _texture
                }

                get { return _scale; }
            }

            public Bitmap Bitmap { get; set; }

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
                    _sprite.Draw(_texture, new ColorBGRA(255, 255, 255, 255), null, new Vector3(-X, -Y, 0));
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

            public override void OnUnload()
            {
                _textFont.Dispose();
            }
        }
    }
}