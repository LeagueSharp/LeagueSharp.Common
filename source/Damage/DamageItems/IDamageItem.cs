// <copyright file="IDamageItem.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    /// <summary>
    ///     The damage item interface.
    /// </summary>
    public interface IDamageItem
    {
        #region Public Properties

        /// <summary>
        ///     Gets the damage type.
        /// </summary>
        Damage.DamageType DamageType { get; }

        /// <summary>
        ///     Gets a value indicating whether the damage is over time.
        /// </summary>
        bool IsDot { get; }

        /// <summary>
        ///     Gets the item id.
        /// </summary>
        int ItemId { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Calculates the raw damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        double GetDamage(Obj_AI_Hero source, Obj_AI_Base target);

        /// <summary>
        ///     Calculates the single damage over time raw damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        double GetDotDamage(Obj_AI_Hero source, Obj_AI_Base target);

        /// <summary>
        ///     Calculates the passive raw damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        double GetPassiveDamage(Obj_AI_Hero source, Obj_AI_Base target);

        #endregion
    }
}