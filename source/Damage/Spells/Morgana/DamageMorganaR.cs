// <copyright file="DamageMorganaR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Morgana R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Morgana", SpellSlot.R)]
    public class DamageMorganaR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMorganaR" /> class.
        /// </summary>
        public DamageMorganaR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 150, 225, 300 }[level] + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}