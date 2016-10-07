// <copyright file="DamageHecarimE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Hecarim E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Hecarim", SpellSlot.E)]
    public class DamageHecarimE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageHecarimE" /> class.
        /// </summary>
        public DamageHecarimE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 40, 75, 110, 145, 180 }[level] + (0.5 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}