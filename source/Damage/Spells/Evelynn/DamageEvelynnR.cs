// <copyright file="DamageEvelynnR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Evelynn R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Evelynn", SpellSlot.R)]
    public class DamageEvelynnR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageEvelynnR" /> class.
        /// </summary>
        public DamageEvelynnR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return (new[] { 0.15, 0.20, 0.25 }[level] + (0.01 / 100 * source.TotalMagicalDamage)) * target.Health;
        }

        #endregion
    }
}