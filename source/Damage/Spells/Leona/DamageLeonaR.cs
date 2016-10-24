// <copyright file="DamageLeonaR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Leona R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Leona", SpellSlot.R)]
    public class DamageLeonaR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLeonaR" /> class.
        /// </summary>
        public DamageLeonaR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 100, 175, 250 }[level] + (0.8 * source.TotalMagicalDamage);
        }

        #endregion
    }
}