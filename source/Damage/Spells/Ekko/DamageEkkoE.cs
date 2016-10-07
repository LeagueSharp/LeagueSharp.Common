// <copyright file="DamageEkkoE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Ekko E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Ekko", SpellSlot.E)]
    public class DamageEkkoE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageEkkoE" /> class.
        /// </summary>
        public DamageEkkoE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 80, 110, 140, 170 }[level] + (0.2 * source.TotalMagicalDamage);
        }

        #endregion
    }
}