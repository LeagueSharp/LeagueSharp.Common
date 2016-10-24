// <copyright file="DamageWarwickQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Warwick Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Warwick", SpellSlot.Q)]
    public class DamageWarwickQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageWarwickQ" /> class.
        /// </summary>
        public DamageWarwickQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return Math.Max(
                       new double[] { 75, 125, 175, 225, 275 }[level],
                       new double[] { 8, 10, 12, 14, 16 }[level] / 100 * target.MaxHealth)
                   + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}