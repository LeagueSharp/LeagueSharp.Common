// <copyright file="DamageViR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Vi R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Vi", SpellSlot.R)]
    public class DamageViR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageViR" /> class.
        /// </summary>
        public DamageViR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 150, 300, 450 }[level] + (1.4 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}