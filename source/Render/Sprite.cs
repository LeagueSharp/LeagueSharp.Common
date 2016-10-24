// <copyright file="Sprite.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    using SharpDX;
    using SharpDX.Direct3D9;

    /// <summary>
    ///     The render class.
    /// </summary>
    public static partial class Render
    {
        /// <summary>
        ///     Draws a sprite.
        /// </summary>
        public class Sprite : RenderObject
        {
            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sprite" /> class.
            /// </summary>
            /// <param name="bitmap">
            ///     The bitmap.
            /// </param>
            /// <param name="position">
            ///     The position.
            /// </param>
            public Sprite(Bitmap bitmap, Vector2 position)
                : this()
            {
                this.UpdateTextureBitmap(bitmap, position);
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sprite" /> class.
            /// </summary>
            /// <param name="texture">
            ///     The texture.
            /// </param>
            /// <param name="position">
            ///     The position.
            /// </param>
            public Sprite(BaseTexture texture, Vector2 position)
                : this((Bitmap)Image.FromStream(BaseTexture.ToStream(texture, ImageFileFormat.Bmp)), position)
            {
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sprite" /> class.
            /// </summary>
            /// <param name="stream">
            ///     The stream.
            /// </param>
            /// <param name="position">
            ///     The position.
            /// </param>
            public Sprite(Stream stream, Vector2 position)
                : this(new Bitmap(stream), position)
            {
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sprite" /> class.
            /// </summary>
            /// <param name="bytesArray">
            ///     The bytes array.
            /// </param>
            /// <param name="position">
            ///     The position.
            /// </param>
            public Sprite(byte[] bytesArray, Vector2 position)
                : this((Bitmap)Image.FromStream(new MemoryStream(bytesArray)), position)
            {
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sprite" /> class.
            /// </summary>
            /// <param name="fileLocation">
            ///     The file location.
            /// </param>
            /// <param name="position">
            ///     The position.
            /// </param>
            public Sprite(string fileLocation, Vector2 position)
                : this()
            {
                if (!File.Exists(fileLocation))
                {
                    return;
                }

                this.UpdateTextureBitmap(new Bitmap(fileLocation), position);
            }

            private Sprite()
            {
                Game.OnUpdate += this.OnUpdate;
                this.SubscribeToResetEvents();
            }

            #endregion

            #region Delegates

            /// <summary>
            ///     The reset delegate.
            /// </summary>
            /// <param name="sprite">
            ///     The sprite.
            /// </param>
            public delegate void OnResetting(Sprite sprite);

            /// <summary>
            ///     The position delegate.
            /// </summary>
            /// <returns>
            ///     The <see cref="Vector2" />.
            /// </returns>
            public delegate Vector2 PositionDelegate();

            #endregion

            #region Public Events

            /// <summary>
            ///     The reset event.
            /// </summary>
            public event OnResetting OnReset;

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets or sets the bitmap.
            /// </summary>
            public Bitmap Bitmap { get; set; }

            /// <summary>
            ///     Gets or sets the color.
            /// </summary>
            public ColorBGRA Color { get; set; } = SharpDX.Color.White;

            /// <summary>
            ///     Gets the height.
            /// </summary>
            public int Height => (int)(this.Bitmap.Height * this.Scale.Y);

            /// <summary>
            ///     Gets or sets a value indicating whether the sprite is visible.
            /// </summary>
            public bool IsVisible { get; set; } = true;

            /// <summary>
            ///     Gets or sets the position.
            /// </summary>
            public Vector2 Position
            {
                get
                {
                    return new Vector2(this.X, this.Y);
                }

                set
                {
                    this.X = (int)value.X;
                    this.Y = (int)value.Y;
                }
            }

            /// <summary>
            ///     Gets or sets the position update.
            /// </summary>
            public PositionDelegate PositionUpdate { get; set; }

            /// <summary>
            ///     Gets or sets the rotation.
            /// </summary>
            public float Rotation { get; set; }

            /// <summary>
            ///     Gets or sets the scale.
            /// </summary>
            public Vector2 Scale { get; set; } = Vector2.One;

            /// <summary>
            ///     Gets the size.
            /// </summary>
            public Vector2 Size => new Vector2(this.Bitmap.Width, this.Bitmap.Height);

            /// <summary>
            ///     Gets or sets the sprite crop.
            /// </summary>
            public SharpDX.Rectangle? SpriteCrop { get; set; }

            /// <summary>
            ///     Gets or sets the texture.
            /// </summary>
            public Texture Texture { get; set; }

            /// <summary>
            ///     Gets the width.
            /// </summary>
            public int Width => (int)(this.Bitmap.Width * this.Scale.X);

            /// <summary>
            ///     Gets or sets the X-Axis of the position.
            /// </summary>
            public int X { get; set; }

            /// <summary>
            ///     Gets or sets the Y-Axis of the position.
            /// </summary>
            public int Y { get; set; }

            #endregion

            #region Properties

            private SharpDX.Direct3D9.Sprite DeviceSprite { get; } = new SharpDX.Direct3D9.Sprite(Device);

            private Texture OriginalTexture { get; set; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Complements the sprite.
            /// </summary>
            public void Complement() => this.SetSaturation(-1.0f);

            /// <summary>
            ///     Crops the sprite.
            /// </summary>
            /// <param name="x">
            ///     The X-axis of the position.
            /// </param>
            /// <param name="y">
            ///     The Y-axis of the position.
            /// </param>
            /// <param name="w">
            ///     The width.
            /// </param>
            /// <param name="h">
            ///     The height.
            /// </param>
            /// <param name="scale">
            ///     The scale.
            /// </param>
            public void Crop(int x, int y, int w, int h, bool scale = false)
                => this.Crop(new SharpDX.Rectangle(x, y, w, h), scale);

            /// <summary>
            ///     Crops the sprite.
            /// </summary>
            /// <param name="rect">
            ///     The rectangle.
            /// </param>
            /// <param name="scale">
            ///     The scale.
            /// </param>
            public void Crop(SharpDX.Rectangle rect, bool scale = false)
            {
                this.SpriteCrop = rect;

                if (scale)
                {
                    this.SpriteCrop = new SharpDX.Rectangle(
                                          (int)(this.Scale.X * rect.X),
                                          (int)(this.Scale.Y * rect.Y),
                                          (int)(this.Scale.X * rect.Width),
                                          (int)(this.Scale.Y * rect.Height));
                }
            }

            /// <summary>
            ///     Fades the sprite.
            /// </summary>
            public void Fade() => this.SetSaturation(.5f);

            /// <summary>
            ///     Grey scales the sprite.
            /// </summary>
            public void GreyScale() => this.SetSaturation(.0f);

            /// <summary>
            ///     Hides the sprite.
            /// </summary>
            public void Hide() => this.IsVisible = false;

            /// <inheritdoc />
            public override void OnEndScene()
            {
                if (this.DeviceSprite.IsDisposed || this.Texture.IsDisposed || this.Position.IsZero || !this.IsVisible)
                {
                    return;
                }

                try
                {
                    this.DeviceSprite.Begin();

                    var matrix = this.DeviceSprite.Transform;
                    var nMatrix = Matrix.Scaling(this.Scale.X, this.Scale.Y, 0) * Matrix.RotationZ(this.Rotation)
                                  * Matrix.Translation(this.Position.X, this.Position.Y, 0);
                    var rotation = Math.Abs(this.Rotation) > float.Epsilon
                                       ? new Vector3(this.Width / 2f, this.Height / 2f, 0)
                                       : (Vector3?)null;

                    this.DeviceSprite.Transform = nMatrix;
                    this.DeviceSprite.Draw(this.Texture, this.Color, this.SpriteCrop, rotation);
                    this.DeviceSprite.Transform = matrix;

                    this.DeviceSprite.End();
                }
                catch (Exception e)
                {
                    this.Reset();
                    this.Log.Error("Unable to draw sprite.", e);
                }
            }

            /// <inheritdoc />
            public override void OnPostReset()
            {
                try
                {
                    this.DeviceSprite?.OnResetDevice();
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            /// <inheritdoc />
            public override void OnPreReset()
            {
                try
                {
                    this.DeviceSprite?.OnLostDevice();
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            /// <summary>
            ///     Resets the sprite.
            /// </summary>
            public void Reset()
            {
                this.UpdateTextureBitmap(
                    (Bitmap)Image.FromStream(BaseTexture.ToStream(this.OriginalTexture, ImageFileFormat.Bmp)));

                this.OnReset?.Invoke(this);
            }

            /// <summary>
            ///     Sets the sprite saturation.
            /// </summary>
            /// <param name="saturation">
            ///     The saturation level.
            /// </param>
            public void SetSaturation(float saturation)
                => this.UpdateTextureBitmap(SaturateBitmap(this.Bitmap, saturation));

            /// <summary>
            ///     Shows the sprite.
            /// </summary>
            public void Show() => this.IsVisible = true;

            /// <summary>
            ///     Updates the texture.
            /// </summary>
            /// <param name="bitmap">
            ///     The bitmap.
            /// </param>
            /// <param name="position">
            ///     The position.
            /// </param>
            public void UpdateTextureBitmap(Bitmap bitmap, Vector2 position = default(Vector2))
            {
                if (!position.IsZero)
                {
                    this.Position = position;
                }

                this.Bitmap?.Dispose();
                this.Bitmap = bitmap;

                this.Texture = Texture.FromMemory(
                    Device,
                    (byte[])new ImageConverter().ConvertTo(bitmap, typeof(byte[])),
                    this.Width,
                    this.Height,
                    0,
                    Usage.None,
                    Format.A1,
                    Pool.Managed,
                    Filter.Default,
                    Filter.Default,
                    0);
                if (this.OriginalTexture == null)
                {
                    this.OriginalTexture = this.Texture;
                }
            }

            #endregion

            #region Methods

            /// <inheritdoc />
            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (!this.DeviceSprite.IsDisposed)
                {
                    this.DeviceSprite.Dispose();
                }

                if (!this.Texture.IsDisposed)
                {
                    this.Texture.Dispose();
                }

                if (!this.OriginalTexture.IsDisposed)
                {
                    this.OriginalTexture.Dispose();
                }

                this.Bitmap = null;
            }

            private static Bitmap SaturateBitmap(Image original, float saturation)
            {
                const float RWeight = 0.3086f;
                const float GWeight = 0.6094f;
                const float BWeight = 0.0820f;

                var a = ((1.0f - saturation) * RWeight) + saturation;
                var b = (1.0f - saturation) * RWeight;
                var c = (1.0f - saturation) * RWeight;
                var d = (1.0f - saturation) * GWeight;
                var e = ((1.0f - saturation) * GWeight) + saturation;
                var f = (1.0f - saturation) * GWeight;
                var g = (1.0f - saturation) * BWeight;
                var h = (1.0f - saturation) * BWeight;
                var i = ((1.0f - saturation) * BWeight) + saturation;

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
                    original,
                    new System.Drawing.Rectangle(0, 0, original.Width, original.Height),
                    0,
                    0,
                    original.Width,
                    original.Height,
                    GraphicsUnit.Pixel,
                    imgAttribs);
                gr.Dispose();

                return newBitmap;
            }

            private void OnUpdate(EventArgs args)
            {
                if (this.PositionUpdate != null)
                {
                    var pos = this.PositionUpdate();
                    this.X = (int)pos.X;
                    this.Y = (int)pos.Y;
                }
            }

            #endregion
        }
    }
}