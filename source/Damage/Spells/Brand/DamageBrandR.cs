// <copyright file="DamageBrandR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Brand R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Brand", SpellSlot.R)]
    public class DamageBrandR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageBrandR" /> class.
        /// </summary>
        public DamageBrandR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 100, 200, 300 }[level] + (0.25 * source.TotalMagicalDamage);
        }

        #endregion
    }
}