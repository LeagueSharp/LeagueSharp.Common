// <copyright file="DamageGravesR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Graves R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Graves", SpellSlot.R)]
    public class DamageGravesR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageGravesR" /> class.
        /// </summary>
        public DamageGravesR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 250, 400, 550 }[level] + (1.5 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}