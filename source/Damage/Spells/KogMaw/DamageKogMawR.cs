// <copyright file="DamageKogMawR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, KogMaw R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("KogMaw", SpellSlot.R)]
    public class DamageKogMawR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKogMawR" /> class.
        /// </summary>
        public DamageKogMawR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return (new double[] { 70, 110, 150 }[level] + (0.65 * source.FlatPhysicalDamageMod) + (0.25 * source.TotalMagicalDamage)) * (target.HealthPercent < 25 ? 3 : (target.HealthPercent < 50 ? 2 : 1));
        }

        #endregion
    }
}