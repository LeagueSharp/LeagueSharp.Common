// <copyright file="DamageEzrealE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Ezreal E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Ezreal", SpellSlot.E)]
    public class DamageEzrealE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageEzrealE" /> class.
        /// </summary>
        public DamageEzrealE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 75, 125, 175, 225, 275 }[level] + (0.75 * source.TotalMagicalDamage) + (0.5 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}