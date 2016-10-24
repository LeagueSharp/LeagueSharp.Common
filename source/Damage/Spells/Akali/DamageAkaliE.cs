// <copyright file="DamageAkaliE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Akali E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Akali", SpellSlot.E)]
    public class DamageAkaliE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageAkaliE" /> class.
        /// </summary>
        public DamageAkaliE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 30, 55, 80, 105, 130 }[level] + (0.4 * source.TotalMagicalDamage) + (0.6 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod));
        }

        #endregion
    }
}