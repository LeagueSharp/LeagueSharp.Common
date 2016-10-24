// <copyright file="IDamageSpell.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    /// <summary>
    ///     The damage spell interface.
    /// </summary>
    public interface IDamageSpell
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the calculated damage.
        /// </summary>
        double CalculatedDamage { get; set; }

        /// <summary>
        ///     Gets or sets the damage delegate.
        /// </summary>
        SpellDamageDelegate Damage { get; set; }

        /// <summary>
        ///     Gets or sets the damage type.
        /// </summary>
        Damage.DamageType DamageType { get; set; }

        /// <summary>
        ///     Gets or sets the spell slot.
        /// </summary>
        SpellSlot Slot { get; set; }

        /// <summary>
        ///     Gets or sets the stage.
        /// </summary>
        int Stage { get; set; }

        #endregion
    }
}