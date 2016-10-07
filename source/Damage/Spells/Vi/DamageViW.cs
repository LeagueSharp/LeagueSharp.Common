// <copyright file="DamageViW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Vi W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Vi", SpellSlot.W)]
    public class DamageViW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageViW" /> class.
        /// </summary>
        public DamageViW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return ((new[] { 4, 5.5, 7, 8.5, 10 }[level] / 100) + (0.01 * source.FlatPhysicalDamageMod / 35)) * target.MaxHealth;
        }

        #endregion
    }
}