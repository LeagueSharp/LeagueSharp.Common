// <copyright file="DamageXerathW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Xerath W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Xerath", SpellSlot.W)]
    public class DamageXerathW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageXerathW" /> class.
        /// </summary>
        public DamageXerathW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 90, 120, 150, 180 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}