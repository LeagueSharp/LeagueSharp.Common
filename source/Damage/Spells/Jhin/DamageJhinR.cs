// <copyright file="DamageJhinR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Jhin R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Jhin", SpellSlot.R)]
    public class DamageJhinR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageJhinR" /> class.
        /// </summary>
        public DamageJhinR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 125, 200 }[level] + (0.25 * source.FlatPhysicalDamageMod * (1 + ((100 - target.HealthPercent) * 1.02)));
        }

        #endregion
    }
}