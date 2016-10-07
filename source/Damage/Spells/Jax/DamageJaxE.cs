// <copyright file="DamageJaxE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Jax E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Jax", SpellSlot.E)]
    public class DamageJaxE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageJaxE" /> class.
        /// </summary>
        public DamageJaxE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 75, 100, 125, 150 }[level] + (0.5 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}