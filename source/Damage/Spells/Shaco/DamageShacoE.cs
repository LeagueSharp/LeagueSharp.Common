// <copyright file="DamageShacoE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Shaco E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Shaco", SpellSlot.E)]
    public class DamageShacoE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageShacoE" /> class.
        /// </summary>
        public DamageShacoE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 90, 130, 170, 210 }[level] + (1 * source.FlatPhysicalDamageMod) + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}