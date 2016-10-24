// <copyright file="DamageTaricR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Taric R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Taric", SpellSlot.R)]
    public class DamageTaricR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTaricR" /> class.
        /// </summary>
        public DamageTaricR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 150, 250, 350 }[level] + (0.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}