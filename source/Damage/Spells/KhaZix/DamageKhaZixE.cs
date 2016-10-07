// <copyright file="DamageKhaZixE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, KhaZix E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("KhaZix", SpellSlot.E)]
    public class DamageKhaZixE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKhaZixE" /> class.
        /// </summary>
        public DamageKhaZixE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 65, 100, 135, 170, 205 }[level] + (0.2 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}