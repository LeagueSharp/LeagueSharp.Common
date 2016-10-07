// <copyright file="ItemDamageType.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System;

    /// <summary>
    ///     The item damage type.
    /// </summary>
    [Flags]
    public enum ItemDamageType
    {
        /// <summary>
        ///     The default type.
        /// </summary>
        Default = 0 << 1,

        /// <summary>
        ///     The damage over time type.
        /// </summary>
        Dot = 1 << 1,

        /// <summary>
        ///     The passive type.
        /// </summary>
        Passive = 2 << 1
    }
}