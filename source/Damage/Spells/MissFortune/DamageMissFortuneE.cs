// <copyright file="DamageMissFortuneE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, MissFortune E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("MissFortune", SpellSlot.E)]
    public class DamageMissFortuneE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMissFortuneE" /> class.
        /// </summary>
        public DamageMissFortuneE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 115, 150, 185, 220 }[level] + (0.8 * source.TotalMagicalDamage);
        }

        #endregion
    }
}