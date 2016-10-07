// <copyright file="DamageAniviaE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Anivia E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Anivia", SpellSlot.E)]
    public class DamageAniviaE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageAniviaE" /> class.
        /// </summary>
        public DamageAniviaE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return (new double[] { 55, 85, 115, 145, 175 }[level] + (0.5 * source.TotalMagicalDamage)) * (target.HasBuff("chilled") ? 2 : 1);
        }

        #endregion
    }
}