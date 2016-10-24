// <copyright file="HitChance.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    /// <summary>
    ///     Hit chance enum.
    /// </summary>
    public enum HitChance
    {
        /// <summary>
        ///     Collision of other units.
        /// </summary>
        Collision,

        /// <summary>
        ///     Unit is out of range.
        /// </summary>
        OutOfRange,

        /// <summary>
        ///     Impossible to hit the unit.
        /// </summary>
        Impossible,

        /// <summary>
        ///     Low probability of hitting the unit.
        /// </summary>
        Low,

        /// <summary>
        ///     Medium probability of hitting the unit.
        /// </summary>
        Medium,

        /// <summary>
        ///     High probability of hitting the unit.
        /// </summary>
        High,

        /// <summary>
        ///     Very high probability of hitting the unit.
        /// </summary>
        VeryHigh,

        /// <summary>
        ///     Unit is dashing.
        /// </summary>
        Dashing,

        /// <summary>
        ///     Unit is immobile.
        /// </summary>
        Immobile
    }
}