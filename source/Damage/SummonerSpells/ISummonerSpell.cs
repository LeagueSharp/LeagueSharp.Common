// <copyright file="ISummonerSpell.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.SummonerSpells
{
    using JetBrains.Annotations;

    /// <summary>
    ///     The summoner spell interface.
    /// </summary>
    public interface ISummonerSpell
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Calculates the summoner spell damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        double GetDamage([NotNull] Obj_AI_Hero source, [NotNull] Obj_AI_Base target);

        #endregion
    }
}