// <copyright file="DashAdapter.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Game dash information parser.
    /// </summary>
    [Export(typeof(Dash))]
    public partial class Dash
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initalizes the system.
        /// </summary>
        public static void Initalize() => Library.Instance?.Dash?.Activate();

        /// <summary>
        ///     Shuts the system down.
        /// </summary>
        public static void Shutdown() => Library.Instance?.Dash?.Deactivate();

        #endregion
    }
}