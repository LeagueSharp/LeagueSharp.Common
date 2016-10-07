// <copyright file="DamagePoppyQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Poppy Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Poppy", SpellSlot.Q)]
    public class DamagePoppyQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamagePoppyQ" /> class.
        /// </summary>
        public DamagePoppyQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 35, 55, 75, 95, 115 }[level] + (0.80 * source.FlatPhysicalDamageMod) + (0.07 * target.MaxHealth);
        }

        #endregion
    }
}