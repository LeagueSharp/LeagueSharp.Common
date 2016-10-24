// <copyright file="DamageGarenE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Garen E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Garen", SpellSlot.E)]
    public class DamageGarenE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageGarenE" /> class.
        /// </summary>
        public DamageGarenE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 20, 45, 70, 95, 120 }[level] + (new double[] { 70, 80, 90, 100, 110 }[level] / 100 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod));
        }

        #endregion
    }
}