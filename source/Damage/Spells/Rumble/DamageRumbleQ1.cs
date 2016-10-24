// <copyright file="DamageRumbleQ1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Rumble Q (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Rumble", SpellSlot.Q, 1)]
    public class DamageRumbleQ1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRumbleQ1" /> class.
        /// </summary>
        public DamageRumbleQ1()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Magical;
            this.Stage = 1;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new[] { 112.5, 202.5, 292.5, 382.5, 472.5 }[level] + (1.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}