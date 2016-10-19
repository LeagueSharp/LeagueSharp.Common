// <copyright file="DamageSonaPassive1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Sona's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Sona")]
    public class DamageSonaPassive1 : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.CalcDamage(
                target,
                Damage.DamageType.Physical,
                new[] { 20, 30, 40, 50, 60 }[
                    ((Obj_AI_Hero)source.GetBuff("SonaQProcAttacker").Caster).Spellbook.GetSpell(SpellSlot.Q).Level - 1]
                + (0.2 * ((Obj_AI_Hero)source.GetBuff("SonaQProcAttacker").Caster).TotalMagicalDamage)
                + new[] { 0, 10, 20, 30 }[
                    ((Obj_AI_Hero)source.GetBuff("SonaQProcAttacker").Caster).Spellbook.GetSpell(SpellSlot.R).Level]);
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("SonaQProcAttacker");
        }

        #endregion
    }
}