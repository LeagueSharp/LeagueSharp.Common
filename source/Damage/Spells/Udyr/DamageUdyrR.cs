// <copyright file="DamageUdyrR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Udyr R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Udyr", SpellSlot.R)]
    public class DamageUdyrR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageUdyrR" /> class.
        /// </summary>
        public DamageUdyrR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 10, 20, 30, 40, 50 }[level] + (0.25 * source.TotalMagicalDamage);
        }

        #endregion
    }
}