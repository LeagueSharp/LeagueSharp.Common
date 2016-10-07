// <copyright file="OnDash.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    /// <summary>
    ///     Provides custom defined events.
    /// </summary>
    public static partial class CustomEvents
    {
        /// <summary>
        ///     The unit events.
        /// </summary>
        public partial class Unit
        {
            #region Delegates

            /// <summary>
            ///     The dashed delegate.
            /// </summary>
            /// <param name="sender">
            ///     The sender.
            /// </param>
            /// <param name="args">
            ///     The args.
            /// </param>
            public delegate void OnDashed(Obj_AI_Base sender, Dash.DashItem args);

            #endregion

            #region Public Events

            /// <summary>
            ///     The dash event.
            /// </summary>
            public static event OnDashed OnDash;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Triggers the dash event.
            /// </summary>
            /// <param name="sender">
            ///     The sender.
            /// </param>
            /// <param name="args">
            ///     The args.
            /// </param>
            public static void TriggerOnDash(Obj_AI_Base sender, Dash.DashItem args) => OnDash?.Invoke(sender, args);

            #endregion
        }
    }
}