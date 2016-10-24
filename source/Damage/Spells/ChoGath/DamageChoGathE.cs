// <copyright file="DamageChoGathE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, ChoGath E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("ChoGath", SpellSlot.E)]
    public class DamageChoGathE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageChoGathE" /> class.
        /// </summary>
        public DamageChoGathE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 20, 35, 50, 65, 80 }[level] + (0.3 * source.TotalMagicalDamage);
        }

        #endregion
    }
}