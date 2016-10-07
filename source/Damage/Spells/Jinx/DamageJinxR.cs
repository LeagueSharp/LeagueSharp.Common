// <copyright file="DamageJinxR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Jinx R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Jinx", SpellSlot.R)]
    public class DamageJinxR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageJinxR" /> class.
        /// </summary>
        public DamageJinxR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 25, 35, 45 }[level] + (new double[] { 25, 30, 35 }[level] / 100 * (target.MaxHealth - target.Health)) + (0.1 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}