// <copyright file="DashExtensions.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    /// <summary>
    ///     Dash extensions.
    /// </summary>
    public static class DashExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the dash info.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="Dash.DashItem" />.
        /// </returns>
        public static Dash.DashItem GetDashInfo(this Obj_AI_Base unit) => Library.Instance?.Dash?.GetDashInfo(unit);

        /// <summary>
        ///     Determines if the unit is dashing.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsDashing(this Obj_AI_Base unit) => Library.Instance?.Dash?.IsDashing(unit) ?? false;

        #endregion
    }
}