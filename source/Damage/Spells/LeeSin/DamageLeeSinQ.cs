// <copyright file="DamageLeeSinQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, LeeSin Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("LeeSin", SpellSlot.Q)]
    public class DamageLeeSinQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLeeSinQ" /> class.
        /// </summary>
        public DamageLeeSinQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 80, 110, 140, 170 }[level] + (0.9 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}