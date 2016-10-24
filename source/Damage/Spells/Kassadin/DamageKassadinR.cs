// <copyright file="DamageKassadinR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Kassadin R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Kassadin", SpellSlot.R)]
    public class DamageKassadinR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKassadinR" /> class.
        /// </summary>
        public DamageKassadinR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 100, 120 }[level] + (0.02 * source.MaxMana);
        }

        #endregion
    }
}