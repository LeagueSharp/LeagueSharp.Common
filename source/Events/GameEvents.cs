// <copyright file="GameEvents.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;
    using System.Reflection;

    using log4net;

    using PlaySharp.Toolkit.AppDomain.Messages;
    using PlaySharp.Toolkit.EventAggregator;
    using PlaySharp.Toolkit.Logging;

    /// <summary>
    ///     Provides custom defined events.
    /// </summary>
    public static partial class CustomEvents
    {
        /// <summary>
        ///     The game events.
        /// </summary>
        [Export(typeof(Game))]
        public partial class Game : IPartImportsSatisfiedNotification
        {
            #region Public Properties

            /// <summary>
            ///     Gets the event aggregator.
            /// </summary>
            [Import(typeof(IEventAggregator), AllowDefault = true)]
            public IEventAggregator EventAggregator { get; private set; }

            #endregion

            #region Properties

            private ILog Log { get; } = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion

            #region Public Methods and Operators

            /// <inheritdoc />
            public void OnImportsSatisfied()
            {
                if (this.EventAggregator == null)
                {
                    this.Log.Fatal("EventAggregator was not imported, using legacy events.");
                    LeagueSharp.Game.OnUpdate += args => this.Handle(new OnGameUpdate());
                    LeagueSharp.Game.OnEnd += args => this.Handle(new OnGameEnd());
                    return;
                }

                this.EventAggregator.Subscribe(this);
            }

            #endregion
        }
    }
}