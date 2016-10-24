// <copyright file="DamageKhaZixQ2.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, KhaZix Q (Stage 2).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("KhaZix", SpellSlot.Q, 2)]
    public class DamageKhaZixQ2 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKhaZixQ2" /> class.
        /// </summary>
        public DamageKhaZixQ2()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
            this.Stage = 2;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 95, 120, 145, 170 }[level] + (2.24 * source.FlatPhysicalDamageMod) + (10 * ((Obj_AI_Hero)source).Level);
        }

        #endregion
    }
}