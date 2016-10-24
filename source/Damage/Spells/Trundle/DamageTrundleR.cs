// <copyright file="DamageTrundleR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Trundle R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Trundle", SpellSlot.R)]
    public class DamageTrundleR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTrundleR" /> class.
        /// </summary>
        public DamageTrundleR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return ((new double[] { 20, 24, 28 }[level] / 100) + (0.02 * source.TotalMagicalDamage / 100)) * target.MaxHealth;
        }

        #endregion
    }
}