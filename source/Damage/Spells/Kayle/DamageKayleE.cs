// <copyright file="DamageKayleE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Kayle E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Kayle", SpellSlot.E)]
    public class DamageKayleE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKayleE" /> class.
        /// </summary>
        public DamageKayleE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return source.HasBuff("judicatorrighteousfury") ? new double[] { 20, 30, 40, 50, 60 }[level] + (0.30 * source.TotalMagicalDamage) : new double[] { 10, 15, 20, 25, 30 }[level] + (0.15 * source.TotalMagicalDamage);
        }

        #endregion
    }
}