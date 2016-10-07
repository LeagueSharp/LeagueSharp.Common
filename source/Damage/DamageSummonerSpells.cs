// <copyright file="DamageSummonerSpells.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    using JetBrains.Annotations;

    using LeagueSharp.Common.SummonerSpells;

    /// <summary>
    ///     Damage calculations and data.
    /// </summary>
    public partial class Damage
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the summoner spells.
        /// </summary>
        [ImportMany]
        public IEnumerable<Lazy<ISummonerSpell, ISummonerSpellMetadata>> SummonerSpellLazies { get; protected set; }

        #endregion

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
        /// <param name="summonerSpell">
        ///     The summoner spell.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public double GetSummonerSpellDamage(
            [NotNull] Obj_AI_Hero source,
            [NotNull] Obj_AI_Base target,
            SummonerSpell summonerSpell)
        {
            return (from lazy in this.SummonerSpellLazies
                    where lazy.Metadata.SummonerSpell == summonerSpell
                    select lazy.Value.GetDamage(source, target)).FirstOrDefault();
        }

        #endregion
    }
}