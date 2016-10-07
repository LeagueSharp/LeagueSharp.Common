// <copyright file="AntiGapcloserAdapter.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    /// <summary>
    ///     Gapcloser delegate.
    /// </summary>
    /// <param name="gapcloser">
    ///     The active gapcloser.
    /// </param>
    public delegate void OnGapcloseH(ActiveGapcloser gapcloser);

    /// <summary>
    ///     The anti-gapcloser, provides an event about game gapclose.
    /// </summary>
    public partial class AntiGapcloser
    {
        #region Public Events

        /// <summary>
        ///     Enemy gapcloser event.
        /// </summary>
        public static event OnGapcloseH OnEnemyGapcloser;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the active gapclosers.
        /// </summary>
        public static List<ActiveGapcloser> ActiveGapclosers => Instance?.ActiveGapclosersList;

        /// <summary>
        ///     Gets a value indicating whether the system is initialized.
        /// </summary>
        public static bool IsInitialized => Instance?.IsActive ?? false;

        /// <summary>
        ///     Gets the registered spells.
        /// </summary>
        public static List<Gapcloser> Spells => Instance?.LazySpells.Select(s => s.Value as Gapcloser).ToList();

        #endregion

        #region Properties

        private static AntiGapcloser Instance => Library.Instance?.AntiGapcloser;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Activates the system.
        /// </summary>
        public static void Initialize() => Instance?.Activate();

        /// <summary>
        ///     Shuts down the system.
        /// </summary>
        public static void Shutdown() => Instance?.Deactivate();

        #endregion

        #region Methods

        private static void OnGapcloser(object sender, ActiveGapcloser activeGapcloser)
            => OnEnemyGapcloser?.Invoke(activeGapcloser);

        #endregion
    }
}