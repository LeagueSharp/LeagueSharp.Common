// <copyright file="DamageJhinW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Jhin W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Jhin", SpellSlot.W)]
    public class DamageJhinW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageJhinW" /> class.
        /// </summary>
        public DamageJhinW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 85, 120, 155, 190 }[level] + (0.7 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}