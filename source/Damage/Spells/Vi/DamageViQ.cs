// <copyright file="DamageViQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Vi Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Vi", SpellSlot.Q)]
    public class DamageViQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageViQ" /> class.
        /// </summary>
        public DamageViQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 75, 100, 125, 150 }[level] + (0.8 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}