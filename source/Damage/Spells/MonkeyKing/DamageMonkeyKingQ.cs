// <copyright file="DamageMonkeyKingQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, MonkeyKing Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("MonkeyKing", SpellSlot.Q)]
    public class DamageMonkeyKingQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMonkeyKingQ" /> class.
        /// </summary>
        public DamageMonkeyKingQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 30, 60, 90, 120, 150 }[level] + (0.1 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod));
        }

        #endregion
    }
}