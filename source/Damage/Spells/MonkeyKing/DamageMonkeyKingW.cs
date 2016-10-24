// <copyright file="DamageMonkeyKingW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, MonkeyKing W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("MonkeyKing", SpellSlot.W)]
    public class DamageMonkeyKingW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMonkeyKingW" /> class.
        /// </summary>
        public DamageMonkeyKingW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 115, 160, 205, 250 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}