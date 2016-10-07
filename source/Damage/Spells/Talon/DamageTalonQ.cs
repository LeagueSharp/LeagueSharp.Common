// <copyright file="DamageTalonQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Talon Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Talon", SpellSlot.Q)]
    public class DamageTalonQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTalonQ" /> class.
        /// </summary>
        public DamageTalonQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 30, 60, 90, 120, 150 }[level] + (0.3 * source.FlatPhysicalDamageMod) + ((target is Obj_AI_Hero) ? (new double[] { 10, 20, 30, 40, 50 }[level] + (1 * source.FlatPhysicalDamageMod)) : 0);
        }

        #endregion
    }
}