// <copyright file="DamageTristanaE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Tristana E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Tristana", SpellSlot.E)]
    public class DamageTristanaE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTristanaE" /> class.
        /// </summary>
        public DamageTristanaE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 70, 80, 90, 100 }[level] + (new[] { 0.5, 0.65, 0.8, 0.95, 1.10 }[level] * source.FlatPhysicalDamageMod) + (0.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}