// <copyright file="DamageShenQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Shen Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Shen", SpellSlot.Q)]
    public class DamageShenQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageShenQ" /> class.
        /// </summary>
        public DamageShenQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            var dmg = (new[] { 3, 3.5, 4, 4.5, 5 }[level] + (0.015 * source.TotalMagicalDamage)) * target.MaxHealth
                      / 100;
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