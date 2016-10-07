// <copyright file="DamageRumbleE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Rumble E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Rumble", SpellSlot.E)]
    public class DamageRumbleE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRumbleE" /> class.
        /// </summary>
        public DamageRumbleE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 45, 70, 95, 120, 145 }[level] + (0.4 * source.TotalMagicalDamage);
        }

        #endregion
    }
}