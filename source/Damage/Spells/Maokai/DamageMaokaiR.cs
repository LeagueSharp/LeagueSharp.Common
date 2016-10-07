// <copyright file="DamageMaokaiR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Maokai R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Maokai", SpellSlot.R)]
    public class DamageMaokaiR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMaokaiR" /> class.
        /// </summary>
        public DamageMaokaiR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 100, 150, 200 }[level] + (0.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}