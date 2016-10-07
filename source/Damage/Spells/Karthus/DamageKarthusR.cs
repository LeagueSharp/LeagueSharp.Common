// <copyright file="DamageKarthusR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Karthus R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Karthus", SpellSlot.R)]
    public class DamageKarthusR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKarthusR" /> class.
        /// </summary>
        public DamageKarthusR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 250, 400, 550 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}