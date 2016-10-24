// <copyright file="DamageZedE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Zed E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Zed", SpellSlot.E)]
    public class DamageZedE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageZedE" /> class.
        /// </summary>
        public DamageZedE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 90, 120, 150, 180 }[level] + (0.8 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}