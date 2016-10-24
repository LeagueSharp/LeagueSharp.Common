// <copyright file="DamageSwainR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Swain R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Swain", SpellSlot.R)]
    public class DamageSwainR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSwainR" /> class.
        /// </summary>
        public DamageSwainR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 70, 90 }[level] + (0.2 * source.TotalMagicalDamage);
        }

        #endregion
    }
}