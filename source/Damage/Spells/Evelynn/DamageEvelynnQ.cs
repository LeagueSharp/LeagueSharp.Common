// <copyright file="DamageEvelynnQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Evelynn Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Evelynn", SpellSlot.Q)]
    public class DamageEvelynnQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageEvelynnQ" /> class.
        /// </summary>
        public DamageEvelynnQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 40, 50, 60, 70, 80 }[level] + (new double[] { 35, 40, 45, 50, 55 }[level] / 100 * source.TotalMagicalDamage) + (new double[] { 50, 55, 60, 65, 70 }[level] / 100 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}