namespace LeagueSharp.Common
{
    /// <summary>
    ///     Adds hacks to the menu.
    /// </summary>
    internal class Hacks
    {
        #region Constants

        private const int WM_KEYDOWN = 0x100;

        private const int WM_KEYUP = 0x101;

        #endregion

        #region Static Fields

        private static Menu menu;

        private static MenuItem MenuAntiAfk;

        private static MenuItem MenuDisableDrawings;

        private static MenuItem MenuDisableSay;

        private static MenuItem MenuTowerRange;

        #endregion

        #region Public Methods and Operators

        public static void Shutdown()
        {
            Menu.Remove(menu);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            CustomEvents.Game.OnGameLoad += eventArgs =>
                {
                    menu = new Menu("Hacks", "Hacks");

                    MenuAntiAfk = menu.AddItem(new MenuItem("AfkHack", "Anti-AFK").SetValue(false));
                    MenuAntiAfk.ValueChanged += (sender, args) => LeagueSharp.Hacks.AntiAFK = args.GetNewValue<bool>();

                    MenuDisableDrawings = menu.AddItem(new MenuItem("DrawingHack", "Disable Drawing").SetValue(false));
                    MenuDisableDrawings.ValueChanged +=
                        (sender, args) => LeagueSharp.Hacks.DisableDrawings = args.GetNewValue<bool>();
                    MenuDisableDrawings.SetValue(LeagueSharp.Hacks.DisableDrawings);

                    MenuDisableSay =
                        menu.AddItem(
                            new MenuItem("SayHack", "Disable L# Send Chat").SetValue(false)
                                .SetTooltip("Block Game.Say from Assemblies"));
                    MenuDisableSay.ValueChanged +=
                        (sender, args) => LeagueSharp.Hacks.DisableSay = args.GetNewValue<bool>();

                    MenuTowerRange = menu.AddItem(new MenuItem("TowerHack", "Show Tower Ranges").SetValue(false));
                    MenuTowerRange.ValueChanged +=
                        (sender, args) => LeagueSharp.Hacks.TowerRanges = args.GetNewValue<bool>();

                    LeagueSharp.Hacks.AntiAFK = MenuAntiAfk.GetValue<bool>();
                    LeagueSharp.Hacks.DisableDrawings = MenuDisableDrawings.GetValue<bool>();
                    LeagueSharp.Hacks.DisableSay = MenuDisableSay.GetValue<bool>();
                    LeagueSharp.Hacks.TowerRanges = MenuTowerRange.GetValue<bool>();

                    CommonMenu.Instance.AddSubMenu(menu);

                    Game.OnWndProc += args =>
                        {
                            if (!MenuDisableDrawings.GetValue<bool>())
                            {
                                return;
                            }

                            if ((int)args.WParam != Config.ShowMenuPressKey)
                            {
                                return;
                            }

                            if (args.Msg == WM_KEYDOWN)
                            {
                                LeagueSharp.Hacks.DisableDrawings = false;
                            }

                            if (args.Msg == WM_KEYUP)
                            {
                                LeagueSharp.Hacks.DisableDrawings = true;
                            }
                        };
                };
        }

        #endregion
    }
}