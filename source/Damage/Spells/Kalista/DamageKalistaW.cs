// <copyright file="DamageKalistaW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Kalista W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Kalista", SpellSlot.W)]
    public class DamageKalistaW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKalistaW" /> class.
        /// </summary>
        public DamageKalistaW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return (new double[] { 12, 14, 16, 18, 20 }[level] / 100) * target.MaxHealth;
        }

        #endregion
    }
}