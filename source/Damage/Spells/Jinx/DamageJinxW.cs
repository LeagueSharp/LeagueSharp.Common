// <copyright file="DamageJinxW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Jinx W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Jinx", SpellSlot.W)]
    public class DamageJinxW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageJinxW" /> class.
        /// </summary>
        public DamageJinxW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 10, 60, 110, 160, 210 }[level] + (1.4 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}