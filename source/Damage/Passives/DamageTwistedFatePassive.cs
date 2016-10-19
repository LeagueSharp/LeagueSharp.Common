// <copyright file="DamageTwistedFatePassive.cs" company="LeagueSharp">
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
    public class DamageTwistedFatePassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return (float)source.GetSpellDamage(target, SpellSlot.W)
                   - (float)
                   source.CalcDamage(
                       target,
                       Damage.DamageType.Physical,
                       source.BaseAttackDamage + source.FlatPhysicalDamageMod) - 10f;
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("bluecardpreattack");
        }

        #endregion
    }
}