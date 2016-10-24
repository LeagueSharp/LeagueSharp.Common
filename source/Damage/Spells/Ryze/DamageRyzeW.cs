// <copyright file="DamageRyzeW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Ryze W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Ryze", SpellSlot.W)]
    public class DamageRyzeW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRyzeW" /> class.
        /// </summary>
        public DamageRyzeW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 100, 120, 140, 160 }[level] + (0.2 * source.TotalMagicalDamage) + (0.01 * (source.MaxMana - 392.4 - (52 * (source as Obj_AI_Hero).Level)));
        }

        #endregion
    }
}