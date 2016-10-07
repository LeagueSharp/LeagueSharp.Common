// <copyright file="DamageJaxW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Jax W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Jax", SpellSlot.W)]
    public class DamageJaxW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageJaxW" /> class.
        /// </summary>
        public DamageJaxW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 40, 75, 110, 145, 180 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}