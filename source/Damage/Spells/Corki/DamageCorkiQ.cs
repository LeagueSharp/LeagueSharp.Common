// <copyright file="DamageCorkiQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Corki Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Corki", SpellSlot.Q)]
    public class DamageCorkiQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageCorkiQ" /> class.
        /// </summary>
        public DamageCorkiQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 115, 150, 205, 250 }[level] + (0.5 * source.TotalMagicalDamage) + (0.5 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}