// <copyright file="DamageGravesQ1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Graves Q (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Graves", SpellSlot.Q, 1)]
    public class DamageGravesQ1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageGravesQ1" /> class.
        /// </summary>
        public DamageGravesQ1()
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
            return new double[] { 80, 125, 170, 215, 260 }[level] + (new[] { 0.4, 0.6, 0.8, 1, 1.2 }[level] * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}