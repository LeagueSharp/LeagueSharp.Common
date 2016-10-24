// <copyright file="DamageLuluE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Lulu E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Lulu", SpellSlot.E)]
    public class DamageLuluE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLuluE" /> class.
        /// </summary>
        public DamageLuluE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 110, 140, 170, 200 }[level] + (0.4 * source.TotalMagicalDamage);
        }

        #endregion
    }
}