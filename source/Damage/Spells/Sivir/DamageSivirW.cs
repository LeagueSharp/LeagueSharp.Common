// <copyright file="DamageSivirW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Sivir W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Sivir", SpellSlot.W)]
    public class DamageSivirW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSivirW" /> class.
        /// </summary>
        public DamageSivirW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 65, 70, 75, 80 }[level] / 100 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}