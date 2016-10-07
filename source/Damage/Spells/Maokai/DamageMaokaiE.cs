// <copyright file="DamageMaokaiE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Maokai E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Maokai", SpellSlot.E)]
    public class DamageMaokaiE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMaokaiE" /> class.
        /// </summary>
        public DamageMaokaiE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 40, 60, 80, 100, 120 }[level] + (0.4 * source.TotalMagicalDamage);
        }

        #endregion
    }
}