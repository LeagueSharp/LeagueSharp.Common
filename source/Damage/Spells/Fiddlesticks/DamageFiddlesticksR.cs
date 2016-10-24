// <copyright file="DamageFiddlesticksR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Fiddlesticks R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Fiddlesticks", SpellSlot.R)]
    public class DamageFiddlesticksR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageFiddlesticksR" /> class.
        /// </summary>
        public DamageFiddlesticksR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 125, 225, 325 }[level] + (0.45 * source.TotalMagicalDamage);
        }

        #endregion
    }
}