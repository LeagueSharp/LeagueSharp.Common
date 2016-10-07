// <copyright file="DamageMissFortuneR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, MissFortune R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("MissFortune", SpellSlot.R)]
    public class DamageMissFortuneR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMissFortuneR" /> class.
        /// </summary>
        public DamageMissFortuneR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return (0.75 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)) + (0.2 * source.TotalMagicalDamage);
        }

        #endregion
    }
}