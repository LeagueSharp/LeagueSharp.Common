// <copyright file="DamageNasusR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Nasus R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Nasus", SpellSlot.R)]
    public class DamageNasusR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageNasusR" /> class.
        /// </summary>
        public DamageNasusR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return ((new double[] { 3, 4, 5 }[level] / 100) + (0.01 / 100 * source.TotalMagicalDamage)) * target.MaxHealth;
        }

        #endregion
    }
}