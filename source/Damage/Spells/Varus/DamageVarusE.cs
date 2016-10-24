// <copyright file="DamageVarusE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Varus E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Varus", SpellSlot.E)]
    public class DamageVarusE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageVarusE" /> class.
        /// </summary>
        public DamageVarusE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 65, 100, 135, 170, 205 }[level] + (0.6 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}