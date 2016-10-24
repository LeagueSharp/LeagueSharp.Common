// <copyright file="IDamageSpellMetadata.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    /// <summary>
    ///     The damage spell metadata interface.
    /// </summary>
    public interface IDamageSpellMetadata
    {
        #region Public Properties

        /// <summary>
        ///     Gets the champion name.
        /// </summary>
        string ChampionName { get; }

        /// <summary>
        ///     Gets the spell slot.
        /// </summary>
        SpellSlot SpellSlot { get; }

        /// <summary>
        ///     Gets the spell stage.
        /// </summary>
        int Stage { get; }

        #endregion
    }
}