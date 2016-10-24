// <copyright file="DamageAmumuW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Amumu W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Amumu", SpellSlot.W)]
    public class DamageAmumuW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageAmumuW" /> class.
        /// </summary>
        public DamageAmumuW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 8, 12, 16, 20, 24 }[level] + ((new[] { 0.01, 0.015, 0.02, 0.025, 0.03 }[level] + (0.01 * source.TotalMagicalDamage / 100)) * target.MaxHealth);
        }

        #endregion
    }
}