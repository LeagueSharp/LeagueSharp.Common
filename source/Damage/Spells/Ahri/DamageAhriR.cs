// <copyright file="DamageAhriR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Ahri R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Ahri", SpellSlot.R)]
    public class DamageAhriR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageAhriR" /> class.
        /// </summary>
        public DamageAhriR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 110, 150 }[level] + (0.3 * source.TotalMagicalDamage);
        }

        #endregion
    }
}