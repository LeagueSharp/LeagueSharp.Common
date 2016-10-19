// <copyright file="IPassiveDamage.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    /// <summary>
    ///     The passive damage interface.
    /// </summary>
    public interface IPassiveDamage
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <returns>
        ///     Thee <see cref="double" />.
        /// </returns>
        double GetDamage(Obj_AI_Hero source, Obj_AI_Base target);

        /// <summary>
        ///     Determines if the passive damage is active.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool IsActive(Obj_AI_Hero source, Obj_AI_Base target);

        #endregion
    }
}