// <copyright file="DamageIreliaR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Irelia R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Irelia", SpellSlot.R)]
    public class DamageIreliaR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageIreliaR" /> class.
        /// </summary>
        public DamageIreliaR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 120, 160 }[level] + (0.5 * source.TotalMagicalDamage) + (0.6 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}