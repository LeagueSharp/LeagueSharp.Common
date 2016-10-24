// <copyright file="DamageNocturneR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Nocturne R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Nocturne", SpellSlot.R)]
    public class DamageNocturneR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageNocturneR" /> class.
        /// </summary>
        public DamageNocturneR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 150, 250, 350 }[level] + (1.2 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}