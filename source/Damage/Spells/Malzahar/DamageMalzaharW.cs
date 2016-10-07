// <copyright file="DamageMalzaharW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Malzahar W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Malzahar", SpellSlot.W)]
    public class DamageMalzaharW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMalzaharW" /> class.
        /// </summary>
        public DamageMalzaharW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return ((new[] { 4, 4.5, 5, 5.5, 6 }[level] / 100) + (0.01 / 100 * source.TotalMagicalDamage)) * target.MaxHealth;
        }

        #endregion
    }
}