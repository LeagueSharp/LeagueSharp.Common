// <copyright file="ISummonerSpellMetadata.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.SummonerSpells
{
    /// <summary>
    ///     The summoner spell metadata interface.
    /// </summary>
    public interface ISummonerSpellMetadata
    {
        #region Public Properties

        /// <summary>
        ///     Gets the summoner spell type.
        /// </summary>
        Damage.SummonerSpell SummonerSpell { get; }

        #endregion
    }
}