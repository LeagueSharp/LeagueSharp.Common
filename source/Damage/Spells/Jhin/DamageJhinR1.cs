// <copyright file="DamageJhinR1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Jhin R (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Jhin", SpellSlot.R, 1)]
    public class DamageJhinR1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageJhinR1" /> class.
        /// </summary>
        public DamageJhinR1()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
            this.Stage = 1;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 125, 200 }[level] + (0.25 * source.FlatPhysicalDamageMod * (1 + ((100 - target.HealthPercent) * 1.02)) * 2) + (0.01 * source.FlatCritDamageMod);
        }

        #endregion
    }
}