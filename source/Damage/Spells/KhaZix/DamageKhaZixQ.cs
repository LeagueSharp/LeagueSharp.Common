// <copyright file="DamageKhaZixQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, KhaZix Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("KhaZix", SpellSlot.Q)]
    public class DamageKhaZixQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKhaZixQ" /> class.
        /// </summary>
        public DamageKhaZixQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 95, 120, 145, 170 }[level] + (1.2 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}