// <copyright file="Library.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Globalization;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading;

    using PlaySharp.Toolkit.AppDomain.Loader;
    using PlaySharp.Toolkit.EventAggregator;

    /// <summary>
    ///     Library entry point for external service loading.
    /// </summary>
    public class Library : ILibrary
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Library" /> class.
        /// </summary>
        [PermissionSet(SecurityAction.Assert, Unrestricted = true)]
        public Library()
        {
            Instance = this;

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            Console.OutputEncoding = Encoding.Default;
            Console.WindowWidth = (int)(Console.LargestWindowWidth / 1.5);
            Console.BufferWidth = (int)(Console.LargestWindowWidth / 1.5);
            Console.WindowHeight = Console.LargestWindowHeight / 2;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the library instance.
        /// </summary>
        public static Library Instance { get; private set; }

        /// <summary>
        ///     Gets the anti gapcloser.
        /// </summary>
        [Import(typeof(AntiGapcloser))]
        public AntiGapcloser AntiGapcloser { get; private set; }

        /// <summary>
        ///     Gets the damage calculation system.
        /// </summary>
        [Import(typeof(Damage))]
        public Damage Damage { get; private set; }

        /// <summary>
        ///     Gets the dash system.
        /// </summary>
        [Import(typeof(Dash))]
        public Dash Dash { get; private set; }

        /// <summary>
        ///     Gets the event aggregator.
        /// </summary>
        [Import(typeof(IEventAggregator))]
        public IEventAggregator EventAggregator { get; private set; }

        /// <summary>
        ///     Gets the game custom events.
        /// </summary>
        [Import(typeof(CustomEvents.Game))]
        public CustomEvents.Game GameEvents { get; private set; }

        /// <summary>
        ///     Gets the map.
        /// </summary>
        [Import(typeof(Utility.Map))]
        public Utility.Map Map { get; private set; }

        /// <summary>
        ///     Gets the minion manager.
        /// </summary>
        [Import(typeof(MinionManager))]
        public MinionManager MinionManager { get; private set; }

        /// <summary>
        ///     Gets the unit custom events.
        /// </summary>
        [Import(typeof(CustomEvents.Unit))]
        public CustomEvents.Unit UnitEvents { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public void Configure(CompositionContainer container)
        {
            this.EventAggregator.Subscribe(this.GameEvents);
            this.EventAggregator.Subscribe(this.UnitEvents);
            this.Damage.SortSpells();
        }

        /// <inheritdoc />
        public void Unload()
        {
            Render.Terminate();
            Render.Circle.Dispose(this, EventArgs.Empty);
        }

        #endregion
    }
}