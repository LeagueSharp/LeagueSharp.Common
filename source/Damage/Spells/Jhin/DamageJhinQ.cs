// <copyright file="DamageJhinQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Jhin Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Jhin", SpellSlot.Q)]
    public class DamageJhinQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageJhinQ" /> class.
        /// </summary>
        public DamageJhinQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 75, 100, 125, 150 }[level] + (new[] { 0.3, 0.35, 0.4, 0.45, 0.5 }[level] * source.FlatPhysicalDamageMod) + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}