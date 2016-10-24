// <copyright file="DamageKindredE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Kindred E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Kindred", SpellSlot.E)]
    public class DamageKindredE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKindredE" /> class.
        /// </summary>
        public DamageKindredE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 110, 140, 170, 200 }[level] + ((source.BaseAttackDamage + source.FlatPhysicalDamageMod) * 0.2f) + (target.MaxHealth * 0.05f);
        }

        #endregion
    }
}