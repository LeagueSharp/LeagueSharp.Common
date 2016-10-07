// <copyright file="DamageXerathR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Xerath R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Xerath", SpellSlot.R)]
    public class DamageXerathR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageXerathR" /> class.
        /// </summary>
        public DamageXerathR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 200, 230, 260 }[level] + (0.43 * source.TotalMagicalDamage);
        }

        #endregion
    }
}