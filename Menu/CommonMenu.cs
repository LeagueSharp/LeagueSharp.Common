namespace LeagueSharp.Common
{
    /// <summary>
    ///     The LeagueSharp.Common official menu.
    /// </summary>
    internal class CommonMenu
    {
        #region Static Fields

        /// <summary>
        ///     The menu instance.
        /// </summary>
        internal static Menu Instance = new Menu("LeagueSharp.Common", "LeagueSharp.Common", true);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a static instance of the <see cref="CommonMenu" /> class.
        /// </summary>
        static CommonMenu()
        {
            TargetSelector.Initialize();
            Prediction.Initialize();
            Hacks.Initialize();
            FakeClicks.Initialize();

            Instance.AddToMainMenu();
        }

        #endregion
    }
}