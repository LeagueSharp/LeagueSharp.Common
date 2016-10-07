// <copyright file="DamageEzrealR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Ezreal R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Ezreal", SpellSlot.R)]
    public class DamageEzrealR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageEzrealR" /> class.
        /// </summary>
        public DamageEzrealR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 350, 500, 650 }[level] + (0.9 * source.TotalMagicalDamage) + (1 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}