// <copyright file="DamageShenPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Shen's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Shen")]
    public class DamageShenPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            double dmg = 0;
            if (source.HasBuff("shenqbuffweak"))
            {
                dmg = source.GetSpellDamage(target, SpellSlot.Q);
            }

            if (source.HasBuff("shenqbuffstrong"))
            {
                dmg = source.GetSpellDamage(target, SpellSlot.Q, 1);
            }

            return dmg;
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("shenqbuff");
        }

        #endregion
    }
}