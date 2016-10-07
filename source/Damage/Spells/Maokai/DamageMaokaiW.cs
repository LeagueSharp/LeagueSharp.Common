// <copyright file="DamageMaokaiW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Maokai W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Maokai", SpellSlot.W)]
    public class DamageMaokaiW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMaokaiW" /> class.
        /// </summary>
        public DamageMaokaiW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return ((new double[] { 9, 10, 11, 12, 13 }[level] / 100) + (0.03 / 100 * source.TotalMagicalDamage)) * target.MaxHealth;
        }

        #endregion
    }
}