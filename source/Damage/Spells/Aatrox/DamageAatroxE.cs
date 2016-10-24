// <copyright file="DamageAatroxE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Aatrox E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Aatrox", SpellSlot.E)]
    public class DamageAatroxE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageAatroxE" /> class.
        /// </summary>
        public DamageAatroxE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 75, 110, 145, 180, 215 }[level] + (0.6 * source.TotalMagicalDamage) + (0.6 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}