// <copyright file="DamageRammusR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Rammus R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Rammus", SpellSlot.R)]
    public class DamageRammusR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRammusR" /> class.
        /// </summary>
        public DamageRammusR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 65, 130, 195 }[level] + (0.3 * source.TotalMagicalDamage);
        }

        #endregion
    }
}