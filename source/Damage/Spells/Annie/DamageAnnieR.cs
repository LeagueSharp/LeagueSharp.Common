// <copyright file="DamageAnnieR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Annie R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Annie", SpellSlot.R)]
    public class DamageAnnieR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageAnnieR" /> class.
        /// </summary>
        public DamageAnnieR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 150, 275, 400 }[level] + new double[] { 10, 15, 20 }[level] + new double[] { 50, 75, 100 }[level] + ((0.65 + 0.1 + 0.15) * source.TotalMagicalDamage);
        }

        #endregion
    }
}