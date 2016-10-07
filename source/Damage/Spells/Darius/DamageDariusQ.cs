// <copyright file="DamageDariusQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Darius Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Darius", SpellSlot.Q)]
    public class DamageDariusQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageDariusQ" /> class.
        /// </summary>
        public DamageDariusQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new[] { 40, 70, 100, 130, 160 }[level] + (new[] { 0.5, 1.1, 1.2, 1.3, 1.4 }[level] * source.TotalAttackDamage);
        }

        #endregion
    }
}