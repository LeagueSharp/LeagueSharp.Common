// <copyright file="OnGameEnd.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;

    using PlaySharp.Toolkit.AppDomain.Messages;
    using PlaySharp.Toolkit.EventAggregator;

    /// <summary>
    ///     Provides custom defined events.
    /// </summary>
    public static partial class CustomEvents
    {
        /// <summary>
        ///     The game events.
        /// </summary>
        public partial class Game : IHandle<OnGameEnd>
        {
            #region Delegates

            /// <summary>
            ///     Game end delegate.
            /// </summary>
            /// <param name="e">
            ///     The event args.
            /// </param>
            public delegate void OnGameEnded(EventArgs e);

            #endregion

            #region Public Events

            /// <summary>
            ///     Game end event.
            /// </summary>
            public static event OnGameEnded OnGameEnd;

            #endregion

            #region Public Methods and Operators

            /// <inheritdoc />
            public void Handle(OnGameEnd message)
            {
                OnGameEnd?.Invoke(EventArgs.Empty);
            }

            #endregion
        }
    }
}