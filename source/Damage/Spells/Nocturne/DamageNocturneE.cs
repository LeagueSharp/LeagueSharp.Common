// <copyright file="DamageNocturneE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Nocturne E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Nocturne", SpellSlot.E)]
    public class DamageNocturneE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageNocturneE" /> class.
        /// </summary>
        public DamageNocturneE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 120, 160, 200, 260 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}