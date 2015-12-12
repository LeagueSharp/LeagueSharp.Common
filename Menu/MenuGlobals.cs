namespace LeagueSharp.Common
{
    using System.Collections.Generic;

    /// <summary>
    ///     Menu global values.
    /// </summary>
    public static class MenuGlobals
    {
        #region Static Fields

        /// <summary>
        ///     Indicates whether to draw the menu.
        /// </summary>
        public static bool DrawMenu;

        /// <summary>
        ///     A collection containing the menu state.
        /// </summary>
        public static List<string> MenuState = new List<string>();

        #endregion
    }
}