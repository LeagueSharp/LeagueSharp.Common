// <copyright file="DamageXinZhaoR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, XinZhao R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("XinZhao", SpellSlot.R)]
    public class DamageXinZhaoR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageXinZhaoR" /> class.
        /// </summary>
        public DamageXinZhaoR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 75, 175, 275 }[level] + (1 * source.FlatPhysicalDamageMod) + (0.15 * target.Health);
        }

        #endregion
    }
}