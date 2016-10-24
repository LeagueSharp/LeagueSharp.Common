// <copyright file="Instances.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using LeagueSharp.Common.Configuration;

    /// <summary>
    ///     Provides system instances in a static manner to support older API usage.
    /// </summary>
    public static class Instances
    {
        #region Public Properties

        /// <summary>
        ///     Gets the anti gapcloser instance.
        /// </summary>
        public static AntiGapcloser AntiGapcloser { get; internal set; }

        /// <summary>
        ///     Gets the damage instance.
        /// </summary>
        public static Damage Damage { get; internal set; }

        /// <summary>
        ///     Gets the dash instance.
        /// </summary>
        public static Dash Dash { get; internal set; }

        /// <summary>
        ///     Gets the game events instance.
        /// </summary>
        public static CustomEvents.Game GameEvents { get; internal set; }

        /// <summary>
        ///     Gets the hero manager instance.
        /// </summary>
        public static HeroManager HeroManager { get; internal set; }

        /// <summary>
        ///     Gets the library manager.
        /// </summary>
        public static Library Library { get; internal set; }

        /// <summary>
        ///     Gets the map instance.
        /// </summary>
        public static Utility.Map Map { get; internal set; }

        /// <summary>
        ///     Gets the menu manager instance.
        /// </summary>
        public static MenuManager MenuManager { get; internal set; }

        /// <summary>
        ///     Gets the minion manager instance.
        /// </summary>
        public static MinionManager MinionManager { get; internal set; }

        /// <summary>
        ///     Gets the unit events.
        /// </summary>
        public static CustomEvents.Unit UnitEvents { get; internal set; }

        #endregion
    }
}