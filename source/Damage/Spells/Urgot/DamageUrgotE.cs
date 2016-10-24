// <copyright file="DamageUrgotE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Urgot E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Urgot", SpellSlot.E)]
    public class DamageUrgotE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageUrgotE" /> class.
        /// </summary>
        public DamageUrgotE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 75, 130, 185, 240, 295 }[level] + (0.6 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}