// <copyright file="DamageDianaR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Diana R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Diana", SpellSlot.R)]
    public class DamageDianaR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageDianaR" /> class.
        /// </summary>
        public DamageDianaR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 100, 160, 220 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}