// <copyright file="DamageAniviaR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Anivia R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Anivia", SpellSlot.R)]
    public class DamageAniviaR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageAniviaR" /> class.
        /// </summary>
        public DamageAniviaR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 120, 160 }[level] + (0.25 * source.TotalMagicalDamage);
        }

        #endregion
    }
}