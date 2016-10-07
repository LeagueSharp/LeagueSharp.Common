// <copyright file="DamageRivenQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Riven Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Riven", SpellSlot.Q)]
    public class DamageRivenQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRivenQ" /> class.
        /// </summary>
        public DamageRivenQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 10, 30, 50, 70, 90 }[level] + (((source.BaseAttackDamage + source.FlatPhysicalDamageMod) / 100) * new double[] { 40, 45, 50, 55, 60 }[level]);
        }

        #endregion
    }
}