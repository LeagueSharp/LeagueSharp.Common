// <copyright file="Damage.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    /// <summary>
    ///     Damage calculations and data.
    /// </summary>
    [Export(typeof(Damage))]
    public partial class Damage
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Calculates the damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="damageType">
        ///     The damage type.
        /// </param>
        /// <param name="amount">
        ///     The amount.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public double CalculateDamage(Obj_AI_Base source, Obj_AI_Base target, DamageType damageType, double amount)
        {
            var damage = amount;

            switch (damageType)
            {
                case DamageType.Magical:
                    damage = this.CalculateMagicDamage(source, target, amount);
                    break;
                case DamageType.Physical:
                    damage = this.CalculatePhysicalDamage(source, target, amount);
                    break;
            }

            return Math.Max(damage, 0d);
        }

        /// <summary>
        ///     Gets the auto attack damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="includePassive">
        ///     A value indicating whether to include the passive.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public double GetAutoAttackDamage(Obj_AI_Base source, Obj_AI_Base target, bool includePassive = false)
        {
            return 0d;
        }

        /// <summary>
        ///     Calculates the combo damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="spellCombo">
        ///     The spell combo.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public double GetComboDamage(Obj_AI_Hero source, Obj_AI_Base target, IEnumerable<SpellSlot> spellCombo)
            => this.GetComboDamage(source, target, spellCombo.Select(s => Tuple.Create(s, 0)).ToArray());

        /// <summary>
        ///     Calculates the combo damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="spellCombo">
        ///     The spell combo.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public double GetComboDamage(
                Obj_AI_Hero source,
                Obj_AI_Base target,
                IEnumerable<Tuple<SpellSlot, int>> spellCombo)
            => spellCombo.Sum(s => this.GetSpellDamage(source, target, s.Item1, s.Item2));

        /// <summary>
        ///     Calculates the spell damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="spellName">
        ///     The spell name.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public double GetSpellDamage(Obj_AI_Base source, Obj_AI_Base target, string spellName)
            => this.GetDamageSpell(source, target, spellName)?.CalculatedDamage ?? 0d;

        /// <summary>
        ///     Calculates the spell damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="slot">
        ///     The slot.
        /// </param>
        /// <param name="stage">
        ///     The stage.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public double GetSpellDamage(Obj_AI_Hero source, Obj_AI_Base target, SpellSlot slot, int stage = 0)
            => this.GetDamageSpell(source, target, slot, stage)?.CalculatedDamage ?? 0d;

        /// <summary>
        ///     Determines if the target is killable by source.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="spellCombo">
        ///     The spell combo.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool IsKillable(Obj_AI_Hero source, Obj_AI_Base target, IEnumerable<Tuple<SpellSlot, int>> spellCombo)
            => this.GetComboDamage(source, target, spellCombo) >= target.Health;

        #endregion

        #region Methods

        private double CalculateMagicDamage(Obj_AI_Base source, Obj_AI_Base target, double amount)
        {
            return 0d;
        }

        private double CalculatePhysicalDamage(Obj_AI_Base source, Obj_AI_Base target, double amount)
        {
            return 0;
        }

        #endregion
    }
}