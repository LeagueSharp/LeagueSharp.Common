// <copyright file="SummonerSpellIgnite.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.SummonerSpells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The summoner spell: Ignite.
    /// </summary>
    [Export(typeof(ISummonerSpell))]
    [ExportMetadata("SummonerSpell", Damage.SummonerSpell.Ignite)]
    public class SummonerSpellIgnite : ISummonerSpell
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return 50 + (20 * source.Level) - (target.HPRegenRate / 5 * 3);
        }

        #endregion
    }
}