// <copyright file="DamageNasusE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Nasus E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Nasus", SpellSlot.E)]
    public class DamageNasusE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageNasusE" /> class.
        /// </summary>
        public DamageNasusE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 55, 95, 135, 175, 215 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}