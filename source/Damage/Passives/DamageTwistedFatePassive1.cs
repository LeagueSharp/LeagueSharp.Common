// <copyright file="DamageTwistedFatePassive1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     TwistedFate's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "TwistedFate")]
    public class DamageTwistedFatePassive1 : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return (float)source.GetSpellDamage(target, SpellSlot.W, 2)
                   - (float)
                   source.CalcDamage(
                       target,
                       Damage.DamageType.Physical,
                       source.BaseAttackDamage + source.FlatPhysicalDamageMod) - 10f;
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("redcardpreattack");
        }

        #endregion
    }
}