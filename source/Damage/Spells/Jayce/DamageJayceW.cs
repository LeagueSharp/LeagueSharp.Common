// <copyright file="DamageJayceW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Jayce W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Jayce", SpellSlot.W)]
    public class DamageJayceW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageJayceW" /> class.
        /// </summary>
        public DamageJayceW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new[] { 25, 40, 55, 70, 85, 100 }[level] + (0.25 * source.TotalMagicalDamage);
        }

        #endregion
    }
}