// <copyright file="DamageExtensions.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.Collections.Generic;

    using BaseAI = Obj_AI_Base;
    using HeroAI = Obj_AI_Hero;
    using SpellCombo = System.Collections.Generic.IEnumerable<System.Tuple<SpellSlot, int>>;

    /// <summary>
    ///     The damage extensions.
    /// </summary>
    public static class DamageExtensions
    {
        #region Properties

        private static Damage Instance => Instances.Damage;

        #endregion

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
        public static double CalcDamage(this BaseAI source, BaseAI target, Damage.DamageType damageType, double amount)
            => Instance?.CalculateDamage(source, target, damageType, amount) ?? 0d;

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
        public static double GetAutoAttackDamage(this BaseAI source, BaseAI target, bool includePassive = false)
            => Instance?.GetAutoAttackDamage(source, target, includePassive) ?? 0d;

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
        public static double GetComboDamage(this HeroAI source, BaseAI target, IEnumerable<SpellSlot> spellCombo)
            => Instance?.GetComboDamage(source, target, spellCombo) ?? 0d;

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
        public static double GetComboDamage(this HeroAI source, BaseAI target, SpellCombo spellCombo)
            => Instance?.GetComboDamage(source, target, spellCombo) ?? 0d;

        /// <summary>
        ///     Gets the damage spell.
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
        ///     The <see cref="DamageSpell" />
        /// </returns>
        public static DamageSpell GetDamageSpell(this BaseAI source, BaseAI target, string spellName)
            => Instance?.GetDamageSpell(source, target, spellName);

        /// <summary>
        ///     Gets the damage spell.
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
        ///     The <see cref="DamageSpell" />
        /// </returns>
        public static DamageSpell GetDamageSpell(this HeroAI source, BaseAI target, SpellSlot slot, int stage = 0)
            => Instance?.GetDamageSpell(source, target, slot, stage);

        /// <summary>
        ///     Calculates the item damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="item">
        ///     The item.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public static double GetItemDamage(this HeroAI source, BaseAI target, Damage.DamageItems item)
            => Instance?.GetItemDamage(source, target, item) ?? 0d;

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
        public static double GetSpellDamage(this BaseAI source, BaseAI target, string spellName)
            => Instance?.GetSpellDamage(source, target, spellName) ?? 0d;

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
        public static double GetSpellDamage(this HeroAI source, BaseAI target, SpellSlot slot, int stage = 0)
            => Instance?.GetSpellDamage(source, target, slot, stage) ?? 0d;

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
        public static double GetSummonerSpellDamage(
            this HeroAI source,
            BaseAI target,
            Damage.SummonerSpell summonerSpell) => Instance?.GetSummonerSpellDamage(source, target, summonerSpell) ?? 0d;

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
        public static bool IsKillable(this HeroAI source, BaseAI target, SpellCombo spellCombo)
            => Instance?.IsKillable(source, target, spellCombo) ?? false;

        #endregion
    }
}