namespace LeagueSharp.Common
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

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

        #region Public Methods and Operators

        public static string Function001(string arg0)
        {
            const string Var0 = "\x70\x61\x63\x6b\x65\x74\x73?";
            const string Var1 = "\x4d\x4f\x41\x52\x20\x44\x4d\x47";

            return Regex.Replace(arg0, Var0, Var1, RegexOptions.IgnoreCase);
        }

        #endregion
    }
}