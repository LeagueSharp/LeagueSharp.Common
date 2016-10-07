// <copyright file="OnGameLoad.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        public partial class Game : IHandle<OnGameUpdate>
        {
            #region Fields

            private int lastHandleTick;

            #endregion

            #region Delegates

            /// <summary>
            ///     Game loaded delegate.
            /// </summary>
            /// <param name="e">
            ///     The event args.
            /// </param>
            public delegate void OnGameLoaded(EventArgs e);

            #endregion

            #region Public Events

            /// <summary>
            ///     Game load event.
            /// </summary>
            public static event OnGameLoaded OnGameLoad;

            #endregion

            #region Properties

            private List<Delegate> InvocationList { get; } = new List<Delegate>();

            private ILog Log { get; } = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion

            #region Public Methods and Operators

            /// <inheritdoc />
            public void Handle(OnGameUpdate message)
            {
                if (Utils.GameTimeTickCount - this.lastHandleTick <= 500)
                {
                    return;
                }

                this.lastHandleTick = Utils.GameTimeTickCount;

                if (LeagueSharp.Game.Mode == GameMode.Running && OnGameLoad != null)
                {
                    foreach (var s in OnGameLoad.GetInvocationList().Where(s => !this.InvocationList.Contains(s)))
                    {
                        this.InvocationList.Add(s);
                        try
                        {
                            s.DynamicInvoke(EventArgs.Empty);
                        }
                        catch (Exception e)
                        {
                            this.Log.Fatal("Failed to dynamiclly invoke a loading request.", e);
                        }
                    }
                }
            }

            #endregion
        }
    }
}