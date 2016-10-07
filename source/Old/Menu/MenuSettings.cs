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

        /// <summary>
        ///     Indicates whether to draw the menu.
        /// </summary>
        private static bool drawMenu;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a static instance of the <see cref="MenuSettings" /> class.
        /// </summary>
        static MenuSettings()
        {
            drawMenu = MenuGlobals.DrawMenu;
            Game.OnWndProc += args => OnWndProc(new WndEventComposition(args));
        }

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

        #region Properties

        /// <summary>
        ///     Gets or sets the value indicating whether to draw the menu.
        /// </summary>
        internal static bool DrawMenu
        {
            get
            {
                return drawMenu;
            }

            set
            {
                MenuGlobals.DrawMenu = drawMenu = value;
            }
        }

        #endregion

        #region Methods

        private static void OnWndProc(WndEventComposition args)
        {
            if ((args.Msg == WindowsMessages.WM_KEYUP || args.Msg == WindowsMessages.WM_KEYDOWN)
                && args.WParam == Config.ShowMenuPressKey)
            {
                DrawMenu = args.Msg == WindowsMessages.WM_KEYDOWN;
            }

            if (args.Msg == WindowsMessages.WM_KEYUP && args.WParam == Config.ShowMenuToggleKey)
            {
                DrawMenu = !DrawMenu;
            }
        }

        #endregion
    }
}