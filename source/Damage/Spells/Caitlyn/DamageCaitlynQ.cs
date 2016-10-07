// <copyright file="DamageCaitlynQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Caitlyn Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Caitlyn", SpellSlot.Q)]
    public class DamageCaitlynQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageCaitlynQ" /> class.
        /// </summary>
        public DamageCaitlynQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 30, 70, 110, 150, 190 }[level] + (new[] { 1.3, 1.4, 1.5, 1.6, 1.7 }[level] * source.TotalAttackDamage);
        }

        #endregion
    }
}