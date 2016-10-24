// <copyright file="DamageMordekaiserR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Mordekaiser R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Mordekaiser", SpellSlot.R)]
    public class DamageMordekaiserR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMordekaiserR" /> class.
        /// </summary>
        public DamageMordekaiserR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return ((new double[] { 24, 29, 34 }[level] / 100) + (0.04 / 100 * source.TotalMagicalDamage)) * target.MaxHealth;
        }

        #endregion
    }
}