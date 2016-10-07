// <copyright file="DamageTrundleQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Trundle Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Trundle", SpellSlot.Q)]
    public class DamageTrundleQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTrundleQ" /> class.
        /// </summary>
        public DamageTrundleQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 20, 40, 60, 80, 100 }[level] + (new[] { 0, 0.5, 0.1, 0.15, 0.2 }[level] * (source.BaseAttackDamage + source.FlatPhysicalDamageMod));
        }

        #endregion
    }
}