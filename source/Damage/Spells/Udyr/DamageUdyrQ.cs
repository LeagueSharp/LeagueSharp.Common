// <copyright file="DamageUdyrQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Udyr Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Udyr", SpellSlot.Q)]
    public class DamageUdyrQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageUdyrQ" /> class.
        /// </summary>
        public DamageUdyrQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 30, 80, 130, 180, 230 }[level] + ((new double[] { 120, 130, 140, 150, 160 }[level] / 100) * (source.BaseAttackDamage + source.FlatPhysicalDamageMod));
        }

        #endregion
    }
}