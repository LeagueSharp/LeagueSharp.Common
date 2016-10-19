namespace LeagueSharp.Common
{
    using System;
    using System.Drawing;

    /// <summary>
    ///     The color picker.
    /// </summary>
    [Obsolete]
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
        public static int ColorPickerX => 0;

        /// <summary>
        ///     The color picker Y-axis.
        /// </summary>
        public static int ColorPickerY => 0;

        /// <summary>
        ///     Gets or sets a value indicating whether the color picker is visible.
        /// </summary>
        public static bool Visible { get; set; }

        /// <summary>
        ///     Gets or sets the X-axis position.
        /// </summary>
        public static int X { get; set; }

        /// <summary>
        ///     Gets or sets the Y-axis position.
        /// </summary>
        public static int Y { get; set; }

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
        }

        #endregion
    }
}