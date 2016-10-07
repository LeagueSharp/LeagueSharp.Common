// <copyright file="DamageSingedE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Singed E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Singed", SpellSlot.E)]
    public class DamageSingedE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSingedE" /> class.
        /// </summary>
        public DamageSingedE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 65, 80, 95, 110 }[level] + (0.75 * source.TotalMagicalDamage) + (new[] { 4, 5.5, 7, 8.5, 10 }[level] / 100 * target.MaxHealth);
        }

        #endregion
    }
}