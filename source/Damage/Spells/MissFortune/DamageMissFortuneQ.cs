// <copyright file="DamageMissFortuneQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, MissFortune Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("MissFortune", SpellSlot.Q)]
    public class DamageMissFortuneQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageMissFortuneQ" /> class.
        /// </summary>
        public DamageMissFortuneQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 20, 35, 50, 65, 80 }[level] + (0.35 * source.TotalMagicalDamage) + (0.85 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod));
        }

        #endregion
    }
}