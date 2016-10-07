// <copyright file="DamageVolibearR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Volibear R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Volibear", SpellSlot.R)]
    public class DamageVolibearR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageVolibearR" /> class.
        /// </summary>
        public DamageVolibearR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 75, 115, 155 }[level] + (0.3 * source.TotalMagicalDamage);
        }

        #endregion
    }
}