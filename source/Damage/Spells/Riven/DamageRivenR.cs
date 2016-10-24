// <copyright file="DamageRivenR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Riven R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Riven", SpellSlot.R)]
    public class DamageRivenR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRivenR" /> class.
        /// </summary>
        public DamageRivenR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return (new double[] { 80, 120, 160 }[level] + (0.6 * source.FlatPhysicalDamageMod)) * (((target.MaxHealth - target.Health) / target.MaxHealth * 2.67) + 1);
        }

        #endregion
    }
}