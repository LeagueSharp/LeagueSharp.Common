// <copyright file="DamageVarusR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Varus R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Varus", SpellSlot.R)]
    public class DamageVarusR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageVarusR" /> class.
        /// </summary>
        public DamageVarusR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 100, 175, 250 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}