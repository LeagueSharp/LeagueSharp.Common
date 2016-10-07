// <copyright file="DamageVladimirE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Vladimir E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Vladimir", SpellSlot.E)]
    public class DamageVladimirE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageVladimirE" /> class.
        /// </summary>
        public DamageVladimirE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 80, 100, 120, 140 }[level] + (0.45 * source.TotalMagicalDamage);
        }

        #endregion
    }
}