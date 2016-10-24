// <copyright file="CollisionableObjects.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    /// <summary>
    ///     Objects that can cause collision with a spell.
    /// </summary>
    public enum CollisionableObjects
    {
        /// <summary>
        ///     Minion.
        /// </summary>
        Minions,

        /// <summary>
        ///     Heroes.
        /// </summary>
        Heroes,

        /// <summary>
        ///     Yasuo's Wind Wall.
        /// </summary>
        YasuoWall,

        /// <summary>
        ///     Walls.
        /// </summary>
        Walls,

        /// <summary>
        ///     Allies.
        /// </summary>
        Allies
    }
}