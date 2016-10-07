// <copyright file="DamageXinZhaoQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, XinZhao Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("XinZhao", SpellSlot.Q)]
    public class DamageXinZhaoQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageXinZhaoQ" /> class.
        /// </summary>
        public DamageXinZhaoQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 15, 30, 45, 60, 75 }[level] + (0.2 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod));
        }

        #endregion
    }
}