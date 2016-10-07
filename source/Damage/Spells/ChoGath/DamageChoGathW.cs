// <copyright file="DamageChoGathW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, ChoGath W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("ChoGath", SpellSlot.W)]
    public class DamageChoGathW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageChoGathW" /> class.
        /// </summary>
        public DamageChoGathW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 75, 125, 175, 225, 275 }[level] + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}