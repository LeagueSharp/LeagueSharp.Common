// <copyright file="SummonerSpellSmite.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.SummonerSpells
{
    using System.ComponentModel.Composition;
    using System.Linq;

    /// <summary>
    ///     The summoner spell: Smite.
    /// </summary>
    [Export(typeof(ISummonerSpell))]
    [ExportMetadata("SummonerSpell", Damage.SummonerSpell.Smite)]
    public class SummonerSpellSmite : ISummonerSpell
    {
        #region Static Fields

        private static readonly double[] DamageArray =
            {
                390, 410, 430, 450, 480, 510, 540, 570, 600, 640, 680, 720, 760,
                800, 850, 900, 950, 1000
            };

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            if (target is Obj_AI_Hero)
            {
                var spells = source.Spellbook.Spells;
                if (spells.FirstOrDefault(h => h.Name.Equals("s5_summonersmiteplayerganker")) != null)
                {
                    return 20 + (8 * source.Level);
                }

                if (spells.FirstOrDefault(h => h.Name.Equals("s5_summonersmiteduel")) != null)
                {
                    return 54 + (6 * source.Level);
                }
            }

            return DamageArray[source.Level - 1];
        }

        #endregion
    }
}