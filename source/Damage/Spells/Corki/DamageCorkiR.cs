// <copyright file="DamageCorkiR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Corki R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Corki", SpellSlot.R)]
    public class DamageCorkiR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageCorkiR" /> class.
        /// </summary>
        public DamageCorkiR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 100, 130, 160 }[level] + (0.3 * source.TotalMagicalDamage) + (new double[] { 20, 50, 80 }[level] / 100 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod));
        }

        #endregion
    }
}