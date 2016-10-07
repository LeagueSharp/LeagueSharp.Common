// <copyright file="DamageRammusW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Rammus W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Rammus", SpellSlot.W)]
    public class DamageRammusW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRammusW" /> class.
        /// </summary>
        public DamageRammusW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 15, 25, 35, 45, 55 }[level] + (0.1 * source.Armor);
        }

        #endregion
    }
}