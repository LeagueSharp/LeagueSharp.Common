// <copyright file="IDamageItemMetadata.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    /// <summary>
    ///     The damage item metadata.
    /// </summary>
    public interface IDamageItemMetadata
    {
        #region Public Properties

        /// <summary>
        ///     Gets the item.
        /// </summary>
        Damage.DamageItems Item { get; }

        #endregion
    }
}