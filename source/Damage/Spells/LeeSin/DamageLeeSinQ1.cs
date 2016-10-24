// <copyright file="DamageLeeSinQ1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, LeeSin Q (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("LeeSin", SpellSlot.Q, 1)]
    public class DamageLeeSinQ1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLeeSinQ1" /> class.
        /// </summary>
        public DamageLeeSinQ1()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
            this.Stage = 1;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 80, 110, 140, 170 }[level] + (0.9 * source.FlatPhysicalDamageMod) + (0.08 * (target.MaxHealth - target.Health));
        }

        #endregion
    }
}