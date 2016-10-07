namespace LeagueSharp.Common
{
    using System.Drawing;

    using LeagueSharp.Common.Properties;

    using SharpDX;

    using Color = System.Drawing.Color;

    /// <summary>
    ///     The color picker.
    /// </summary>
    public static class ColorPicker
    {
        #region Static Fields

        /// <summary>
        ///     The color picker height.
        /// </summary>
        public static readonly int ColorPickerH = 221;

        /// <summary>
        ///     The color picker width.
        /// </summary>
        public static readonly int ColorPickerW = 234;

        /// <summary>
        ///     The alpha slider.
        /// </summary>
        public static CPSlider AlphaSlider;

        /// <summary>
        ///     The background sprite.
        /// </summary>
        public static Render.Sprite BackgroundSprite;

        /// <summary>
        ///     The luminity bitmap.
        /// </summary>
        public static Bitmap LuminityBitmap;

        /// <summary>
        ///     The luminity sprite.
        /// </summary>
        public static Render.Sprite LuminitySprite;

        /// <summary>
        ///     The luminosity slider.
        /// </summary>
        public static CPSlider LuminositySlider;

        /// <summary>
        ///     The change color event.
        /// </summary>
        public static OnSelectColor OnChangeColor;

        /// <summary>
        ///     The opacity bitmap.
        /// </summary>
        public static Bitmap OpacityBitmap;

        /// <summary>
        ///     The opacity sprite.
        /// </summary>
        public static Render.Sprite OpacitySprite;

        /// <summary>
        ///     The preview rectangle.
        /// </summary>
        public static Render.Rectangle PreviewRectangle;

        /// <summary>
        ///     The last bitmap update(s).
        /// </summary>
        private static readonly int[] LastBitmapUpdate = new int[2];

        /// <summary>
        ///     The initial color.
        /// </summary>
        private static Color initialColor;

        /// <summary>
        ///     Indicates whether the color picker is moving.
        /// </summary>
        private static bool isMoving;

        /// <summary>
        ///     Indicates whether the color is being selected.
        /// </summary>
        private static bool isSelecting;

        /// <summary>
        ///     Indicates whether the color picker is visible.
        /// </summary>
        private static bool isVisible;

        /// <summary>
        ///     The previous position.
        /// </summary>
        private static Vector2 previousPos;

        /// <summary>
        ///     The HSL color.
        /// </summary>
        private static HSLColor sColor = new HSLColor(255, 255, 255);

        /// <summary>
        ///     The Hue value.
        /// </summary>
        private static double sHue;

        /// <summary>
        ///     The saturation value.
        /// </summary>
        private static double sSaturation;

        /// <summary>
        ///     The X-axis position.
        /// </summary>
        private static int xPos = 100;

        /// <summary>
        ///     The Y-axis position.
        /// </summary>
        private static int yPos = 100;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a static instance of the <see cref="ColorPicker" /> class.
        /// </summary>
        static ColorPicker()
        {
            LuminityBitmap = new Bitmap(9, 238);
            OpacityBitmap = new Bitmap(9, 238);

            UpdateLuminosityBitmap(Color.White, true);
            UpdateOpacityBitmap(Color.White, true);

            BackgroundSprite = (Render.Sprite)new Render.Sprite(Resources.CPForm, new Vector2(X, Y)).Add(1);

            LuminitySprite = (Render.Sprite)new Render.Sprite(LuminityBitmap, new Vector2(X + 285, Y + 40)).Add(0);
            OpacitySprite = (Render.Sprite)new Render.Sprite(OpacityBitmap, new Vector2(X + 349, Y + 40)).Add(0);

            PreviewRectangle =
                (Render.Rectangle)
                new Render.Rectangle(X + 375, Y + 44, 54, 80, new ColorBGRA(255, 255, 255, 255)).Add(0);

            LuminositySlider = new CPSlider(285 - Resources.CPActiveSlider.Width / 3, 35, 248);
            AlphaSlider = new CPSlider(350 - Resources.CPActiveSlider.Width / 3, 35, 248);

            Game.OnWndProc += args => OnWndProc(new WndEventComposition(args));
        }

        #endregion

        #region Delegates

        /// <summary>
        ///     The on select color delegate.
        /// </summary>
        /// <param name="color">
        ///     The color.
        /// </param>
        public delegate void OnSelectColor(Color color);

        #endregion

        #region Public Properties

        /// <summary>
        ///     The color picker X-axis.
        /// </summary>
        public static int ColorPickerX
        {
            get
            {
                return X + 18;
            }
        }

        /// <summary>
        ///     The color picker Y-axis.
        /// </summary>
        public static int ColorPickerY
        {
            get
            {
                return Y + 61;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the color picker is visible.
        /// </summary>
        public static bool Visible
        {
            get
            {
                return isVisible;
            }

            set
            {
                LuminitySprite.Visible =
                    OpacitySprite.Visible =
                    BackgroundSprite.Visible =
                    LuminositySlider.Visible = AlphaSlider.Visible = PreviewRectangle.Visible = isVisible = value;
            }
        }

        /// <summary>
        ///     Gets or sets the X-axis position.
        /// </summary>
        public static int X
        {
            get
            {
                return xPos;
            }

            set
            {
                var oldX = xPos;

                xPos = value;
                BackgroundSprite.X += value - oldX;
                LuminitySprite.X += value - oldX;
                OpacitySprite.X += value - oldX;
                PreviewRectangle.X += value - oldX;
                LuminositySlider.X += value - oldX;
                AlphaSlider.X += value - oldX;
            }
        }

        /// <summary>
        ///     Gets or sets the Y-axis position.
        /// </summary>
        public static int Y
        {
            get
            {
                return yPos;
            }

            set
            {
                var oldY = yPos;

                yPos = value;
                BackgroundSprite.Y += value - oldY;
                LuminitySprite.Y += value - oldY;
                OpacitySprite.Y += value - oldY;
                PreviewRectangle.Y += value - oldY;
                LuminositySlider.Y += value - oldY;
                AlphaSlider.Y += value - oldY;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Fires the change color event.
        /// </summary>
        /// <param name="color">
        ///     The color.
        /// </param>
        public static void FireEvent(Color color)
        {
            if (OnChangeColor != null)
            {
                OnChangeColor(color);
            }
        }

        /// <summary>
        ///     Loads the color picker.
        /// </summary>
        /// <param name="onSelectColor">
        ///     The select color delegate.
        /// </param>
        /// <param name="color">
        ///     The color.
        /// </param>
        public static void Load(OnSelectColor onSelectColor, Color color)
        {
            OnChangeColor = onSelectColor;
            sColor = color;
            sHue = ((HSLColor)color).Hue;
            sSaturation = ((HSLColor)color).Saturation;

            LuminositySlider.Percent = (float)sColor.Luminosity / 100f;
            AlphaSlider.Percent = color.A / 255f;
            X = (Drawing.Width - BackgroundSprite.Width) / 2;
            Y = (Drawing.Height - BackgroundSprite.Height) / 2;

            Visible = true;
            UpdateLuminosityBitmap(color);
            UpdateOpacityBitmap(color);
            initialColor = color;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Closes the color picker.
        /// </summary>
        private static void Close()
        {
            isSelecting =
                isMoving =
                AlphaSlider.Moving =
                LuminositySlider.Moving = AlphaSlider.Visible = LuminositySlider.Visible = Visible = false;
        }

        /// <summary>
        ///     The windows process event messages.
        /// </summary>
        /// <param name="args">
        ///     The event args.
        /// </param>
        private static void OnWndProc(WndEventComposition args)
        {
            if (!isVisible)
            {
                return;
            }

            LuminositySlider.OnWndProc(args);
            AlphaSlider.OnWndProc(args);

            var pos = Utils.GetCursorPos();

            if (args.Msg == WindowsMessages.WM_LBUTTONDOWN)
            {
                isMoving = Utils.IsUnderRectangle(pos, X, Y, BackgroundSprite.Width, 25);

                // Apply Button
                if (Utils.IsUnderRectangle(pos, X + 290, Y + 297, 74, 12))
                {
                    Close();
                    return;
                }

                // Cancel Button
                if (Utils.IsUnderRectangle(pos, X + 370, Y + 296, 73, 14))
                {
                    FireEvent(initialColor);
                    Close();
                    return;
                }

                if (Utils.IsUnderRectangle(pos, ColorPickerX, ColorPickerY, ColorPickerW, ColorPickerH))
                {
                    isSelecting = true;
                    UpdateColor();
                }
            }
            else if (args.Msg == WindowsMessages.WM_LBUTTONUP)
            {
                isMoving = isSelecting = false;
            }
            else if (args.Msg == WindowsMessages.WM_MOUSEMOVE)
            {
                if (isSelecting)
                {
                    if (Utils.IsUnderRectangle(pos, ColorPickerX, ColorPickerY, ColorPickerW, ColorPickerH))
                    {
                        UpdateColor();
                    }
                }

                if (isMoving)
                {
                    X += (int)(pos.X - previousPos.X);
                    Y += (int)(pos.Y - previousPos.Y);
                }

                previousPos = pos;
            }
        }

        /// <summary>
        ///     Updates the color picker color.
        /// </summary>
        internal static void UpdateColor()
        {
            if (isSelecting)
            {
                var pos = Utils.GetCursorPos();
                var color = BackgroundSprite.Bitmap.GetPixel((int)pos.X - X, (int)pos.Y - Y);
                sHue = ((HSLColor)color).Hue;
                sSaturation = ((HSLColor)color).Saturation;
                UpdateLuminosityBitmap(color);
            }

            sColor.Hue = sHue;
            sColor.Saturation = sSaturation;
            sColor.Luminosity = (LuminositySlider.Percent * 100d);
            var r = Color.FromArgb(((int)(AlphaSlider.Percent * 255)), sColor);
            PreviewRectangle.Color = new ColorBGRA(r.R, r.G, r.B, r.A);
            UpdateOpacityBitmap(r);
            FireEvent(r);
        }

        /// <summary>
        ///     Updates the luminosity bitmap.
        /// </summary>
        /// <param name="color">
        ///     The color.
        /// </param>
        /// <param name="force">
        ///     Indicates whether to force update.
        /// </param>
        private static void UpdateLuminosityBitmap(HSLColor color, bool force = false)
        {
            if (Utils.TickCount - LastBitmapUpdate[0] < 100 && !force)
            {
                return;
            }

            LastBitmapUpdate[0] = Utils.TickCount;
            color.Luminosity = 0d;

            for (var y = 0; y < LuminityBitmap.Height; y++)
            {
                for (var x = 0; x < LuminityBitmap.Width; x++)
                {
                    LuminityBitmap.SetPixel(x, y, color);
                }

                color.Luminosity += 100d / LuminityBitmap.Height;
            }

            if (LuminitySprite != null)
            {
                LuminitySprite.UpdateTextureBitmap(LuminityBitmap);
            }
        }

        /// <summary>
        ///     Updates the opacity bitmap.
        /// </summary>
        /// <param name="color">
        ///     The color.
        /// </param>
        /// <param name="force">
        ///     Indicates whether to force update.
        /// </param>
        private static void UpdateOpacityBitmap(HSLColor color, bool force = false)
        {
            if (Utils.TickCount - LastBitmapUpdate[1] < 100 && !force)
            {
                return;
            }

            LastBitmapUpdate[1] = Utils.TickCount;
            color.Luminosity = 0d;

            for (var y = 0; y < OpacityBitmap.Height; y++)
            {
                for (var x = 0; x < OpacityBitmap.Width; x++)
                {
                    OpacityBitmap.SetPixel(x, y, color);
                }

                color.Luminosity += 40d / LuminityBitmap.Height;
            }

            if (OpacitySprite != null)
            {
                OpacitySprite.UpdateTextureBitmap(OpacityBitmap);
            }
        }

        #endregion
    }
}