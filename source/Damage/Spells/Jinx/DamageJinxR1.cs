// <copyright file="DamageJinxR1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Jinx R (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Jinx", SpellSlot.R, 1)]
    public class DamageJinxR1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageJinxR1" /> class.
        /// </summary>
        public DamageJinxR1()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
            this.Stage = 1;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 250, 350, 450 }[level] + (new double[] { 25, 30, 35 }[level] / 100 * (target.MaxHealth - target.Health)) + (1 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}