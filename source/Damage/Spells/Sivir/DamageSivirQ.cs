// <copyright file="DamageSivirQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Sivir Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Sivir", SpellSlot.Q)]
    public class DamageSivirQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSivirQ" /> class.
        /// </summary>
        public DamageSivirQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 25, 45, 65, 85, 105 }[level] + (new double[] { 70, 80, 90, 100, 110 }[level] / 100 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)) + (0.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}