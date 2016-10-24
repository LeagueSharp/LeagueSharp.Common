// <copyright file="DamagePoppyE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Poppy E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Poppy", SpellSlot.E)]
    public class DamagePoppyE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamagePoppyE" /> class.
        /// </summary>
        public DamagePoppyE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 70, 90, 110, 130 }[level] + (0.5 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}