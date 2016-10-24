// <copyright file="DamageNamiMarkPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Aatrox's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "")]
    public class DamageNamiMarkPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.CalcDamage(
                target,
                Damage.DamageType.Magical,
                new[] { 25, 40, 55, 70, 85 }[
                    ((Obj_AI_Hero)source.GetBuff("NamiE").Caster).Spellbook.GetSpell(SpellSlot.E).Level - 1]
                + (0.2 * ((Obj_AI_Hero)source.GetBuff("NamiE").Caster).TotalMagicalDamage));
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("NamiE");
        }

        #endregion
    }
}