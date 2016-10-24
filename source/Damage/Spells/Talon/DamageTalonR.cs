// <copyright file="DamageTalonR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Talon R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Talon", SpellSlot.R)]
    public class DamageTalonR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTalonR" /> class.
        /// </summary>
        public DamageTalonR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 120, 170, 220 }[level] + (0.75 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}