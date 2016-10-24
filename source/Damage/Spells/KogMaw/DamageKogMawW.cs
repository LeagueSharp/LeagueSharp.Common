// <copyright file="DamageKogMawW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, KogMaw W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("KogMaw", SpellSlot.W)]
    public class DamageKogMawW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKogMawW" /> class.
        /// </summary>
        public DamageKogMawW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            var dmg = (0.02 + (Math.Truncate(source.TotalMagicalDamage / 100) * 0.75)) * target.MaxHealth;
            if (target is Obj_AI_Minion && dmg > 100)
            {
                dmg = 100;
            }

            return dmg;
        }

        #endregion
    }
}