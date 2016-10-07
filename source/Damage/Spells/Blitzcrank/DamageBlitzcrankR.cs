// <copyright file="DamageBlitzcrankR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Blitzcrank R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Blitzcrank", SpellSlot.R)]
    public class DamageBlitzcrankR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageBlitzcrankR" /> class.
        /// </summary>
        public DamageBlitzcrankR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 250, 375, 500 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}