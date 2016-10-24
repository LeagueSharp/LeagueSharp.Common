// <copyright file="DamageVayneW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Vayne W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Vayne", SpellSlot.W)]
    public class DamageVayneW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageVayneW" /> class.
        /// </summary>
        public DamageVayneW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.True;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return Math.Max(
                new double[] { 40, 60, 80, 100, 120 }[level],
                (new[] { 6, 7.5, 9, 10.5, 12 }[level] / 100) * target.MaxHealth);
        }

        #endregion
    }
}