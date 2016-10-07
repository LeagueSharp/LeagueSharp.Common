// <copyright file="DamageViE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Vi E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Vi", SpellSlot.E)]
    public class DamageViE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageViE" /> class.
        /// </summary>
        public DamageViE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 5, 20, 35, 50, 65 }[level] + (1.15 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)) + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}