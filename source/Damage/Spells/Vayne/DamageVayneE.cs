// <copyright file="DamageVayneE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Vayne E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Vayne", SpellSlot.E)]
    public class DamageVayneE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageVayneE" /> class.
        /// </summary>
        public DamageVayneE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 45, 80, 115, 150, 185 }[level] + (0.5 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}