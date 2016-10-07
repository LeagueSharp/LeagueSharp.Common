// <copyright file="DamageRenektonE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Renekton E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Renekton", SpellSlot.E)]
    public class DamageRenektonE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRenektonE" /> class.
        /// </summary>
        public DamageRenektonE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return (new double[] { 30, 60, 90, 120, 150 }[level] + (0.9 * source.FlatPhysicalDamageMod)) * 1.5;
        }

        #endregion
    }
}