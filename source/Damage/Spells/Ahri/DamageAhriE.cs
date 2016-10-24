// <copyright file="DamageAhriE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Ahri E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Ahri", SpellSlot.E)]
    public class DamageAhriE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageAhriE" /> class.
        /// </summary>
        public DamageAhriE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 95, 130, 165, 200 }[level] + (0.50 * source.TotalMagicalDamage);
        }

        #endregion
    }
}