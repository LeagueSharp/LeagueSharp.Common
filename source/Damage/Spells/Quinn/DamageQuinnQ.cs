// <copyright file="DamageQuinnQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Quinn Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Quinn", SpellSlot.Q)]
    public class DamageQuinnQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageQuinnQ" /> class.
        /// </summary>
        public DamageQuinnQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            var damage = new double[] { 20, 45, 70, 95, 120 }[level]
                         + (new[] { 0.8, 0.9, 1.0, 1.1, 1.2 }[level] * source.TotalAttackDamage)
                         + (0.35 * source.TotalMagicalDamage);
            damage += damage * ((100 - target.HealthPercent) / 100);
            return damage;
        }

        #endregion
    }
}