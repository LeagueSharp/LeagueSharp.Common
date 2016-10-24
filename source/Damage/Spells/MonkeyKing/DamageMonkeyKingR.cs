// <copyright file="DamageMonkeyKingR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, MonkeyKing R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("MonkeyKing", SpellSlot.R)]
    public class DamageMonkeyKingR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMonkeyKingR" /> class.
        /// </summary>
        public DamageMonkeyKingR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 20, 110, 200 }[level] + (1.1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod));
        }

        #endregion
    }
}