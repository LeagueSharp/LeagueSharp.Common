// <copyright file="DamageKennenR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Kennen R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Kennen", SpellSlot.R)]
    public class DamageKennenR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKennenR" /> class.
        /// </summary>
        public DamageKennenR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 145, 210 }[level] + (0.4 * source.TotalMagicalDamage);
        }

        #endregion
    }
}