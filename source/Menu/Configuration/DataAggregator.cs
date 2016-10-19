// <copyright file="DataAggregator.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Configuration
{
    using System;
    using System.ComponentModel.Composition;

    using log4net;

    using PlaySharp.Toolkit.Logging;

    using SharpDX;
    using SharpDX.Direct3D9;
    using SharpDX.Menu;

    /// <summary>
    ///     The data aggregator for the menu factory.
    /// </summary>
    [Export(typeof(SharpDX.Menu.DataAggregator))]
    public class DataAggregator : SharpDX.Menu.DataAggregator
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataAggregator" /> class.
        /// </summary>
        public DataAggregator()
        {
            Game.OnUpdate += args => this.OnExternalEvent(ExternalEventType.Update, null);
            Game.OnWndProc +=
                args =>
                    this.OnExternalEvent(
                        ExternalEventType.WindowProc,
                        new object[] { args.Msg, args.WParam, args.LParam });
            Drawing.OnDraw += args => this.OnExternalEvent(ExternalEventType.Draw, null);
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override Vector2 GetCursorPosition()
        {
            return Cursor.GetCursorPos();
        }

        /// <inheritdoc />
        public override Device GetDevice()
        {
            return Drawing.Direct3DDevice;
        }

        /// <inheritdoc />
        public override int GetEnvironmentTick()
        {
            return Utils.GameTimeTickCount;
        }

        /// <inheritdoc />
        public override ILog GetLogger(Type service)
        {
            return AssemblyLogs.GetLogger(service);
        }

        #endregion
    }
}