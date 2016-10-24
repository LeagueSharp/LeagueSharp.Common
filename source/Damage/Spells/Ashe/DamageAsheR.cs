// <copyright file="DamageAsheR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Ashe R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Ashe", SpellSlot.R)]
    public class DamageAsheR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageAsheR" /> class.
        /// </summary>
        public DamageAsheR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 250, 425, 600 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}