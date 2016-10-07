// <copyright file="DamageKarthusE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Karthus E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Karthus", SpellSlot.E)]
    public class DamageKarthusE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKarthusE" /> class.
        /// </summary>
        public DamageKarthusE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 30, 50, 70, 90, 110 }[level] + (0.2 * source.TotalMagicalDamage);
        }

        #endregion
    }
}