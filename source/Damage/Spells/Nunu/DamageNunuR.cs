// <copyright file="DamageNunuR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Nunu R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Nunu", SpellSlot.R)]
    public class DamageNunuR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageNunuR" /> class.
        /// </summary>
        public DamageNunuR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 625, 875, 1125 }[level] + (2.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}