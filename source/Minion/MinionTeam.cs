// <copyright file="MinionTeam.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;

    /// <summary>
    ///     Minion team.
    /// </summary>
    public enum MinionTeam
    {
        /// <summary>
        ///     Neutral.
        /// </summary>
        Neutral,

        /// <summary>
        ///     Ally.
        /// </summary>
        Ally,

        /// <summary>
        ///     Enemy.
        /// </summary>
        Enemy,

        /// <summary>
        ///     Not Ally.
        /// </summary>
        [Obsolete]
        NotAlly,

        /// <summary>
        ///     Not Ally for Enemy.
        /// </summary>
        [Obsolete]
        NotAllyForEnemy,

        /// <summary>
        ///     All.
        /// </summary>
        All
    }
}