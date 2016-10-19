// <copyright file="DXColorView.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.View
{
    using System;
    using System.ComponentModel.Composition;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    using LeagueSharp.Common.Properties;

    using SharpDX;
    using SharpDX.Menu;

    using Color = SharpDX.Color;
    using Rectangle = System.Drawing.Rectangle;

    /// <summary>
    ///     The component view.
    /// </summary>
    [Export(typeof(IView))]
    [ExportMetadata("Service", typeof(MenuItem<Color>))]
    public class DxColorView : View
    {
        #region Fields

        private Render.Sprite sprite;

        private bool updateContext;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DxColorView" /> class.
        /// </summary>
        /// <param name="component">
        ///     The component.
        /// </param>
        public DxColorView(IMenuComponent component)
        {
            this.Component = component as MenuItem<Color>;
            this.CreateContext();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DxColorView" /> class.
        /// </summary>
        [ImportingConstructor]
        internal DxColorView()
        {
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="DxColorView" /> class.
        /// </summary>
        ~DxColorView()
        {
            this.sprite?.Bitmap?.Dispose();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the color picker instance.
        /// </summary>
        public static DxColorView ColorPickerInstance { get; set; }

        /// <summary>
        ///     Gets the component.
        /// </summary>
        public MenuItem<Color> Component { get; }

        #endregion

        #region Properties

        private static Render.Sprite Spectrum { get; set; }

        private bool AlphaPickClick { get; set; }

        private bool ColorPickClick { get; set; }

        private bool IsPickingColor { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Closes the color picker.
        /// </summary>
        public void Close()
        {
            this.IsPickingColor = false;
            this.updateContext = true;
            this.ColorPickClick = this.AlphaPickClick = false;
        }

        /// <inheritdoc />
        public override IView CreateView(IMenuComponent component)
        {
            return new DxColorView(component);
        }

        /// <inheritdoc />
        public override void OnAttributeChange()
        {
            this.CreateContext();
        }

        /// <inheritdoc />
        public override void OnDraw()
        {
            if (this.Component == null)
            {
                return;
            }

            if (this.updateContext)
            {
                this.CreateContext();
                this.updateContext = false;
            }

            this.sprite.Position = this.Component.Position;
            this.sprite.OnEndScene();

            if (this.IsPickingColor)
            {
                var width = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentWidth ?? 180f;
                Spectrum.Position = new Vector2(this.Component.Position.X + width + 5, this.Component.Position.Y + 5);
                Spectrum.OnEndScene();
            }
        }

        /// <inheritdoc />
        public override void OnUpdate()
        {
        }

        /// <inheritdoc />
        public override void OnWindowProc(uint message, uint wParam, long lParam)
        {
            if (this.Component == null)
            {
                return;
            }

            var cursor = Cursor.GetCursorPos();
            switch (message)
            {
                case (uint)WindowsMessages.WM_LBUTTONDOWN:
                    var width = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentWidth ?? 180f;
                    var height = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentHeight ?? 32f;
                    if (Utils.IsUnderRectangle(
                        cursor,
                        this.Component.Position.X + width - height,
                        this.Component.Position.Y,
                        height,
                        height))
                    {
                        if (!this.IsPickingColor)
                        {
                            SystemColorView.ColorPickerInstance?.Close();
                            CircleView.ColorPickerInstance?.Close();
                            ColorPickerInstance?.Close();
                            ColorPickerInstance = this;
                        }

                        this.IsPickingColor = !this.IsPickingColor;
                        this.updateContext = true;
                    }

                    if (this.IsPickingColor)
                    {
                        if (this.ColorPickClick || this.AlphaPickClick)
                        {
                            break;
                        }

                        if (Utils.IsUnderRectangle(
                            cursor,
                            Spectrum.Position.X,
                            Spectrum.Position.Y,
                            Spectrum.Width,
                            Spectrum.Height))
                        {
                            this.ColorPickClick = true;
                            this.UpdateColorPicker(cursor);
                        }
                        else if (Utils.IsUnderRectangle(
                            cursor,
                            Spectrum.Position.X,
                            Spectrum.Position.Y + Spectrum.Height,
                            Spectrum.Width,
                            15))
                        {
                            this.AlphaPickClick = true;
                            this.UpdateColorPicker(cursor);
                        }
                    }

                    break;

                case (uint)WindowsMessages.WM_LBUTTONUP:
                    if (this.IsPickingColor)
                    {
                        this.ColorPickClick = this.AlphaPickClick = false;
                    }

                    break;

                case (uint)WindowsMessages.WM_MOUSEMOVE:
                    if (this.IsPickingColor)
                    {
                        this.UpdateColorPicker(cursor);
                    }

                    break;
            }
        }

        #endregion

        #region Methods

        private static void CreateGlobalContext()
        {
            if (Spectrum == null)
            {
                Spectrum = new Render.Sprite(Resources.spectrum_chart, default(Vector2))
                                   { Scale = new Vector2(0.5f, 0.5f) };
            }
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var diameter = radius * 2;
            var size = new Size(diameter, diameter);
            var arc = new Rectangle(bounds.Location, size);
            var path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void CreateContext()
        {
            CreateGlobalContext();

            var height = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentHeight ?? 32f;
            var width = Instances.MenuManager?.MenuFactory?.MenuController?.ComponentWidth ?? 180f;
            var title = this.Component.DisplayName;
            var viewAttributes = new ViewAttributes(this);

            var bitmap = new Bitmap(
                             (int)Math.Round(width) + 1 + Spectrum.Width + 25,
                             (int)Math.Round(height) + 1 + Spectrum.Height + 25);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                SharedView.CreateBackgroundView(graphics, 0, 0, width, height);
                SharedView.CreateTitle(graphics, 0, 0, width - height - 5, height, title, viewAttributes);

                var c = this.Component.Value;
                var color = System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);
                SharedView.CreateBox(graphics, width - height, 0, height, height, color);

                if (this.IsPickingColor)
                {
                    SharedView.CreateBackgroundView(graphics, width, 0, Spectrum.Width + 8, Spectrum.Height + 25);
                    using (var pen = new Pen(System.Drawing.Color.FromArgb(51, 200, 203, 203)))
                    {
                        var startX = (int)width + 5;
                        var startY = Spectrum.Height + 10;
                        var alphaWidth = Spectrum.Width;
                        var alphaHeight = 10;
                        var alphaValue = this.Component.Value.A;

                        var valuePercentage = 100 * alphaValue / 255;
                        var alphaX = (int)Math.Round(startX + 3 + ((valuePercentage * (alphaWidth - 3)) / 100f));

                        using (var path = RoundedRect(new Rectangle(startX, startY, alphaWidth, alphaHeight), 5))
                        {
                            graphics.FillPath(new SolidBrush(System.Drawing.Color.FromArgb(150, 200, 203, 203)), path);
                            graphics.DrawPath(pen, path);
                        }

                        using (var path = RoundedRect(new Rectangle(startX, startY, alphaX - startX, alphaHeight), 5))
                        {
                            graphics.FillPath(new SolidBrush(System.Drawing.Color.FromArgb(150, 12, 125, 141)), path);
                            graphics.DrawPath(pen, path);
                        }

                        var rect = new Rectangle(alphaX - (34 / 2), startY - 3, 34, alphaHeight + 5);
                        using (var path = RoundedRect(rect, 5))
                        {
                            graphics.FillPath(new SolidBrush(System.Drawing.Color.FromArgb(255, 0, 0, 0)), path);
                            graphics.DrawPath(pen, path);
                        }

                        SharedView.CreateText(
                            graphics,
                            rect.X,
                            rect.Y,
                            rect.Width,
                            rect.Height,
                            alphaValue.ToString(),
                            new ViewAttributes(null));
                    }
                }
            }

            if (this.sprite == null)
            {
                this.sprite = new Render.Sprite(bitmap, default(Vector2));
                return;
            }

            this.sprite.UpdateTextureBitmap(bitmap);
        }

        private void UpdateColorPicker(Vector2 cursor)
        {
            if (this.ColorPickClick)
            {
                var cursorX = (int)Math.Round(((float)Spectrum.Bitmap.Width / Spectrum.Width) * cursor.X);
                var cursorY = (int)Math.Round(((float)Spectrum.Bitmap.Height / Spectrum.Height) * cursor.Y);
                var bitmapX = (int)Math.Round(((float)Spectrum.Bitmap.Width / Spectrum.Width) * Spectrum.X);
                var bitmapY = (int)Math.Round(((float)Spectrum.Bitmap.Width / Spectrum.Width) * Spectrum.Y);

                var x = Math.Max(0, Math.Min(Spectrum.Bitmap.Width - 1, cursorX - bitmapX));
                var y = Math.Max(0, Math.Min(Spectrum.Bitmap.Height - 1, cursorY - bitmapY));

                var c = Spectrum.Bitmap.GetPixel(x, y);
                this.Component.Value = new Color(c.R, c.G, c.B, this.Component.Value.A);
                this.updateContext = true;
            }
            else if (this.AlphaPickClick)
            {
                var cursorValue = 0 + ((Cursor.GetCursorPos().X - Spectrum.Position.X) * (255 - 0) / Spectrum.Width);
                var value = Math.Max(0, Math.Min(255, (int)cursorValue));
                var c = this.Component.Value;
                this.Component.Value = new Color(c.R, c.G, c.B, value);
                this.updateContext = true;
            }
        }

        #endregion
    }
}