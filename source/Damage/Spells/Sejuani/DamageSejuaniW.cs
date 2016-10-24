// <copyright file="DamageSejuaniW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Sejuani W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Sejuani", SpellSlot.W)]
    public class DamageSejuaniW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSejuaniW" /> class.
        /// </summary>
        public DamageSejuaniW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new[] { 4, 4.5, 5, 5.5, 6 }[level] / 100 * target.MaxHealth;
        }

        #endregion
    }
}