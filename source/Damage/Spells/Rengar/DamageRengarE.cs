// <copyright file="DamageRengarE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Rengar E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Rengar", SpellSlot.E)]
    public class DamageRengarE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRengarE" /> class.
        /// </summary>
        public DamageRengarE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 100, 150, 200, 250 }[level] + (0.7 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}