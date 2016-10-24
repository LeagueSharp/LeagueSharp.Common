// <copyright file="DamageQuinnE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Quinn E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Quinn", SpellSlot.E)]
    public class DamageQuinnE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageQuinnE" /> class.
        /// </summary>
        public DamageQuinnE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 40, 70, 100, 130, 160 }[level] + (0.2 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}