// <copyright file="DamageShenQ1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Shen Q (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Shen", SpellSlot.Q, 1)]
    public class DamageShenQ1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageShenQ1" /> class.
        /// </summary>
        public DamageShenQ1()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Magical;
            this.Stage = 1;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            var dmg = (new[] { 5, 5.5, 6, 6.6, 7 }[level] + (0.02 * source.TotalMagicalDamage))
                      * target.MaxHealth / 100;
            if (target is Obj_AI_Hero)
            {
                return dmg;
            }

            return Math.Min(
                new double[] { 30, 50, 70, 90, 110 }[level] + dmg,
                new double[] { 75, 100, 125, 150, 175 }[level]);
        }

        #endregion
    }
}