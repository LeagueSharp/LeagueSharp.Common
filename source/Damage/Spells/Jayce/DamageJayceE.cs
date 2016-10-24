// <copyright file="DamageJayceE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Jayce E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Jayce", SpellSlot.E)]
    public class DamageJayceE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageJayceE" /> class.
        /// </summary>
        public DamageJayceE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return ((new[] { 8, 10.4, 12.8, 15.2, 17.6, 20 }[level] / 100) * target.MaxHealth) + (1 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}