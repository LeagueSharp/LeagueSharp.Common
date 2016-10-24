// <copyright file="DamageTalonW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Talon W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Talon", SpellSlot.W)]
    public class DamageTalonW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTalonW" /> class.
        /// </summary>
        public DamageTalonW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 30, 55, 80, 105, 130 }[level] + (0.6 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}