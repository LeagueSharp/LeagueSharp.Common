// <copyright file="DamageYorickE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Yorick E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Yorick", SpellSlot.E)]
    public class DamageYorickE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageYorickE" /> class.
        /// </summary>
        public DamageYorickE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 55, 85, 115, 145, 175 }[level] + (1 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}