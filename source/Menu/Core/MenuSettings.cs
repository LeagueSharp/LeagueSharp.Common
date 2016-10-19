namespace LeagueSharp.Common
{
    using System.IO;

    using SharpDX;

    using Color = System.Drawing.Color;

    /// <summary>
    ///     The menu settings.
    /// </summary>
    internal static class MenuSettings
    {
        #region Static Fields

        public static readonly Color ActiveBackgroundColor = Color.FromArgb(0, 37, 53);

        /// <summary>
        ///     The menu starting position.
        /// </summary>
        public static Vector2 BasePosition = new Vector2(10, 10);

        #endregion

        #region Public Properties

        public static Color BackgroundColor
        {
            get
            {
                return Color.FromArgb(Menu.Root.Item("BackgroundAlpha").GetValue<Slider>().Value, Color.Black);
            }
        }

        /// <summary>
        ///     Gets the menu configuration path.
        /// </summary>
        public static string MenuConfigPath
        {
            get
            {
                return Path.Combine(Config.AppDataDirectory, "MenuConfigCommon");
            }
        }

        /// <summary>
        ///     Gets or sets the size of the menu font.
        /// </summary>
        public static int MenuFontSize { get; set; }

        /// <summary>
        ///     Gets the menu item height.
        /// </summary>
        public static int MenuItemHeight
        {
            get
            {
                return 32;
            }
        }

        /// <summary>
        ///     Gets the menu item width.
        /// </summary>
        public static int MenuItemWidth
        {
            get
            {
                return 160;
            }
        }

        #endregion
    }
}