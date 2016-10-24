// <copyright file="IPassiveDamageMetadata.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    /// <summary>
    ///     The passive damage metadata.
    /// </summary>
    public interface IPassiveDamageMetadata
    {
        #region Public Properties

        /// <summary>
        ///     Gets the champion name.
        /// </summary>
        string ChampionName { get; }

        #endregion
    }
}