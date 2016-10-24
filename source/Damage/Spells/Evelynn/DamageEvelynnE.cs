// <copyright file="DamageEvelynnE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Evelynn E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Evelynn", SpellSlot.E)]
    public class DamageEvelynnE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageEvelynnE" /> class.
        /// </summary>
        public DamageEvelynnE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 110, 150, 190, 230 }[level] + (1 * source.TotalMagicalDamage) + (1 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}