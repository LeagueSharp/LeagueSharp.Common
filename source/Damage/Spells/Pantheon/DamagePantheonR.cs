// <copyright file="DamagePantheonR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Pantheon R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Pantheon", SpellSlot.R)]
    public class DamagePantheonR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamagePantheonR" /> class.
        /// </summary>
        public DamagePantheonR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 400, 700, 1000 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}