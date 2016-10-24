// <copyright file="DamageXerathE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Xerath E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Xerath", SpellSlot.E)]
    public class DamageXerathE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageXerathE" /> class.
        /// </summary>
        public DamageXerathE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 110, 140, 170, 200 }[level] + (0.45 * source.TotalMagicalDamage);
        }

        #endregion
    }
}