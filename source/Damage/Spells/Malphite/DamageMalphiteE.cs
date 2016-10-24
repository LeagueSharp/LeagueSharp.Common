// <copyright file="DamageMalphiteE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Malphite E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Malphite", SpellSlot.E)]
    public class DamageMalphiteE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMalphiteE" /> class.
        /// </summary>
        public DamageMalphiteE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 100, 140, 180, 220 }[level] + (0.3 * source.Armor) + (0.2 * source.TotalMagicalDamage);
        }

        #endregion
    }
}