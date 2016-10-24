// <copyright file="DamageCorkiE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Corki E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Corki", SpellSlot.E)]
    public class DamageCorkiE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageCorkiE" /> class.
        /// </summary>
        public DamageCorkiE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 20, 32, 44, 56, 68 }[level] + (0.4 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}