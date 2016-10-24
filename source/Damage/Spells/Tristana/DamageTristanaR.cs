// <copyright file="DamageTristanaR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Tristana R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Tristana", SpellSlot.R)]
    public class DamageTristanaR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTristanaR" /> class.
        /// </summary>
        public DamageTristanaR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 300, 400, 500 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}