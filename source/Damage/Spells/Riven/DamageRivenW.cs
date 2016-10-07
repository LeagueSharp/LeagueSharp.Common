// <copyright file="DamageRivenW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Riven W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Riven", SpellSlot.W)]
    public class DamageRivenW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRivenW" /> class.
        /// </summary>
        public DamageRivenW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 80, 110, 140, 170 }[level] + (1 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}