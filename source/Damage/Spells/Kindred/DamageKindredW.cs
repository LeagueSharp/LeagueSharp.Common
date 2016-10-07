// <copyright file="DamageKindredW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Kindred W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Kindred", SpellSlot.W)]
    public class DamageKindredW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKindredW" /> class.
        /// </summary>
        public DamageKindredW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 25, 30, 35, 40, 45 }[level] + ((source.BaseAttackDamage + source.FlatPhysicalDamageMod) * 0.4f);
        }

        #endregion
    }
}