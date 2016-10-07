// <copyright file="DamageThreshE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Thresh E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Thresh", SpellSlot.E)]
    public class DamageThreshE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageThreshE" /> class.
        /// </summary>
        public DamageThreshE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 65, 95, 125, 155, 185 }[level] + (0.4 * source.TotalMagicalDamage);
        }

        #endregion
    }
}