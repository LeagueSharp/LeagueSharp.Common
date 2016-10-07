// <copyright file="DamageYasuoE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Yasuo E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Yasuo", SpellSlot.E)]
    public class DamageYasuoE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageYasuoE" /> class.
        /// </summary>
        public DamageYasuoE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 90, 110, 130, 150 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}