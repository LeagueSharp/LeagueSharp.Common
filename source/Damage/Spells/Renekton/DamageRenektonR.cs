// <copyright file="DamageRenektonR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Renekton R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Renekton", SpellSlot.R)]
    public class DamageRenektonR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRenektonR" /> class.
        /// </summary>
        public DamageRenektonR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 30, 60, 120 }[level] + (0.1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}