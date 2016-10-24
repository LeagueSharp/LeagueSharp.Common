// <copyright file="DamageAsheW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Ashe W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Ashe", SpellSlot.W)]
    public class DamageAsheW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageAsheW" /> class.
        /// </summary>
        public DamageAsheW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 20, 35, 50, 65, 80 }[level] + (1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod));
        }

        #endregion
    }
}