// <copyright file="DamageKhaZixQ3.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, KhaZix Q (Stage 3).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("KhaZix", SpellSlot.Q, 3)]
    public class DamageKhaZixQ3 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKhaZixQ3" /> class.
        /// </summary>
        public DamageKhaZixQ3()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
            this.Stage = 3;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new[] { 91, 123.5, 156, 188.5, 221 }[level] + (2.6 * source.FlatPhysicalDamageMod) + (10 * ((Obj_AI_Hero)source).Level);
        }

        #endregion
    }
}