using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D9;

namespace LeagueSharp.Common
{
    /// <summary>
    /// The render class allows you to draw stuff using SharpDX easier.
    /// </summary>
    public static class Render
    {
        private static readonly List<RenderObject> RenderObjects = new List<RenderObject>();

        static Render()
        {
            Drawing.OnEndScene += Drawing_OnEndScene;
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (Drawing.Direct3DDevice == null) return;
            if (Drawing.Direct3DDevice.IsDisposed) return;

            for (var i = -5; i < 5; i++)
                foreach (var renderObject in RenderObjects)
                    if(renderObject.Layer == i && renderObject.Visible)
                        renderObject.OnEndScene();
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

            public virtual void OnEndScene()
            {
            }
        }

        public class Text : RenderObject
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public ColorBGRA Color { get; set; }
            public string text { get; set; }
            public FontDescription TextFontDescription;
            private SharpDX.Direct3D9.Font _textFont;
            
            public Text(int x, int y, string text ,int size, ColorBGRA color, string faceName = "Calibri")
            {
                Color = color;
                this.text = text;
                
                _textFont = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new FontDescription
                {
                    FaceName = faceName,
                    Height = size,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Default,
                });

            }
            public override void OnEndScene()
            {
                try
                {
                    if (_textFont.IsDisposed) return;
                    _textFont.DrawText(null, "text", X, Y, Color);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Common.Render.Text.OnEndScene: " + e);
                }
            }
        }

        public class Rectangle : RenderObject
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public ColorBGRA Color;

            private readonly Line _line;
            public Rectangle(int x, int y, int width, int height, ColorBGRA color)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                Color = color;
                _line = new Line(Drawing.Direct3DDevice);
                _line.Width = height;
            }

            public override void OnEndScene()
            {
                try
                {
                    if(_line.IsDisposed) return;
                    _line.Begin();
                    _line.Draw(new[] { new Vector2(X, Y + Height / 2), new Vector2(X + Width, Y + Height / 2), }, Color);
                    _line.End();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Common.Render.Rectangle.OnEndScene: " + e);
                }
            }
        }
        
        public class Sprite : RenderObject
        {
            public int X = 0;
            public int Y = 0;
            private int _width = 0;
            private int _height = 0;

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

            public Bitmap Bitmap { get; set; }

            private Texture _texture;
            private readonly SharpDX.Direct3D9.Sprite _sprite;

            public Sprite(int x, int y, Bitmap bitmap, int width = -1, int height = -1)
            {
                X = x;
                Y = y;
                _width = width;
                _height = height;
                Bitmap = bitmap;
                _sprite = new SharpDX.Direct3D9.Sprite(Drawing.Direct3DDevice);
                _texture = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(bitmap, typeof(byte[])), width != -1 ? width : bitmap.Width, height != -1 ? height : bitmap.Height, 0, Usage.None, Format.A1, Pool.Default, Filter.Default, Filter.Default, 0);
            }

            public void UpdateTextureBitmap(Bitmap newBitmap)
            {
                Bitmap = newBitmap;
                _texture = Texture.FromMemory(Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(newBitmap, typeof(byte[])), Width, Height, 0, Usage.None, Format.A1, Pool.Default, Filter.Default, Filter.Default, 0);
            }

            public override void OnEndScene()
            {
                try
                {
                    if (_sprite.IsDisposed) return;
                    _sprite.Begin();
                    _sprite.Draw(_texture, new ColorBGRA(255, 255, 255, 255), null, new Vector3(-X, -Y, 0));
                    _sprite.End();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Common.Render.Sprite.OnEndScene: " + e);
                }
            }
        }
    }

   
}
