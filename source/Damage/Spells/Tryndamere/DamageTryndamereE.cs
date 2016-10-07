// <copyright file="DamageTryndamereE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Tryndamere E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Tryndamere", SpellSlot.E)]
    public class DamageTryndamereE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTryndamereE" /> class.
        /// </summary>
        public DamageTryndamereE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 100, 130, 160, 190 }[level] + (1.2 * source.FlatPhysicalDamageMod) + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}