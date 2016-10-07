// <copyright file="DamageSwainE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Swain E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Swain", SpellSlot.E)]
    public class DamageSwainE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSwainE" /> class.
        /// </summary>
        public DamageSwainE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 80, 110, 140, 170 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}