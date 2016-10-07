// <copyright file="DamageFiddlesticksE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Fiddlesticks E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Fiddlesticks", SpellSlot.E)]
    public class DamageFiddlesticksE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageFiddlesticksE" /> class.
        /// </summary>
        public DamageFiddlesticksE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 65, 85, 105, 125, 145 }[level] + (0.45 * source.TotalMagicalDamage);
        }

        #endregion
    }
}