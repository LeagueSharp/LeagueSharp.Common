// <copyright file="DamageSwainW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Swain W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Swain", SpellSlot.W)]
    public class DamageSwainW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSwainW" /> class.
        /// </summary>
        public DamageSwainW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 120, 160, 200, 240 }[level] + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}