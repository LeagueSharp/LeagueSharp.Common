// <copyright file="DamageShacoPassive2.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Shaco's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Shaco")]
    public class DamageShacoPassive2 : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.CalcDamage(
                target,
                Damage.DamageType.Physical,
                (Damage.GetCritMultiplier(source)
                 + new[] { -0.6, -0.4, -0.2, 0, 0.2 }[source.Spellbook.GetSpell(SpellSlot.Q).Level - 1])
                * source.TotalAttackDamage);
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("Deceive");
        }

        #endregion
    }
}