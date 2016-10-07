// <copyright file="GameEvents.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Provides custom defined events.
    /// </summary>
    public static partial class CustomEvents
    {
        /// <summary>
        ///     The game events.
        /// </summary>
        [Export(typeof(Game))]
        public partial class Game
        {
        }
    }
}