// <copyright file="DamageBrandE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Brand E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Brand", SpellSlot.E)]
    public class DamageBrandE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageBrandE" /> class.
        /// </summary>
        public DamageBrandE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 90, 110, 130, 150 }[level] + (0.35 * source.TotalMagicalDamage);
        }

        #endregion
    }
}