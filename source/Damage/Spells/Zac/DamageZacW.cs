// <copyright file="DamageZacW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Zac W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Zac", SpellSlot.W)]
    public class DamageZacW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageZacW" /> class.
        /// </summary>
        public DamageZacW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 40, 55, 70, 85, 100 }[level] + (((new double[] { 4, 5, 6, 7, 8 }[level] / 100) + (0.02 * source.TotalMagicalDamage / 100)) * target.MaxHealth);
        }

        #endregion
    }
}