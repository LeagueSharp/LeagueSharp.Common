// <copyright file="DamageBrandW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Brand W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Brand", SpellSlot.W)]
    public class DamageBrandW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageBrandW" /> class.
        /// </summary>
        public DamageBrandW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 75, 120, 165, 210, 255 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}