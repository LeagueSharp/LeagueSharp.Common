// <copyright file="DamageThreshR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Thresh R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Thresh", SpellSlot.R)]
    public class DamageThreshR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageThreshR" /> class.
        /// </summary>
        public DamageThreshR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 250, 400, 550 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}