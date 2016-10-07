// <copyright file="DamageZacR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Zac R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Zac", SpellSlot.R)]
    public class DamageZacR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageZacR" /> class.
        /// </summary>
        public DamageZacR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 140, 210, 280 }[level] + (0.4 * source.TotalMagicalDamage);
        }

        #endregion
    }
}