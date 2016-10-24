namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;

    public partial class Menu
    {
        #region Static Fields

        public static readonly Menu Root = new Menu("Menu Settings", "Menu Settings");

        public static Dictionary<string, Menu> RootMenus = new Dictionary<string, Menu>();

        #endregion

        #region Public Methods and Operators

        public static Menu GetMenu(string assemblyname, string menuname)
        {
            return null;
        }

        public static MenuItem GetValueGlobally(
            string assemblyname,
            string menuname,
            string itemname,
            string submenu = null)
        {
            return null;
        }

        public static void Remove(Menu menu)
        {
        }

        [Obsolete]
        public static void SendMessage(uint key, WindowsMessages message, WndEventComposition args)
        {
        }

        #endregion
    }
}