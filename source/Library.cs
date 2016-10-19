// <copyright file="Library.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Diagnostics;
    using System.Globalization;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading;

    using LeagueSharp.Common.Configuration;

    using PlaySharp.Toolkit.AppDomain.Loader;

    /// <summary>
    ///     The library manager.
    /// </summary>
    public class Library : ILibrary
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Library" /> class.
        /// </summary>
        public Library()
        {
            Instances.Library = this;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public void Configure(CompositionContainer container)
        {
            ExpandConsole();
            CreateInstances(container);
        }

        /// <inheritdoc />
        public void Unload()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates static instances to support old API.
        /// </summary>
        /// <param name="container">
        ///     The composition container.
        /// </param>
        private static void CreateInstances(CompositionContainer container)
        {
            Instances.AntiGapcloser = Export(container, new AntiGapcloser());
            Instances.Damage = Export(container, new Damage());
            Instances.Dash = Export(container, new Dash());
            Instances.GameEvents = Export(container, new CustomEvents.Game());
            Instances.HeroManager = Export(container, new HeroManager());
            Instances.Map = Export(container, new Utility.Map());
            Instances.MinionManager = Export(container, new MinionManager());
            Instances.UnitEvents = Export(container, new CustomEvents.Unit());
            Instances.MenuManager = Export(container, new MenuManager());
        }

        /// <summary>
        ///     Expands the console on debug, requires permissions.
        /// </summary>
        [PermissionSet(SecurityAction.Assert, Unrestricted = true)]
        private static void ExpandConsole()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            Console.OutputEncoding = Encoding.Default;
            Console.WindowWidth = (int)(Console.LargestWindowWidth / 1.5);
            Console.BufferWidth = (int)(Console.LargestWindowWidth / 1.5);
            Console.WindowHeight = Console.LargestWindowHeight / 2;
        }

        /// <summary>
        ///     Composes exported value macro.
        /// </summary>
        /// <typeparam name="T">
        ///     The value type.
        /// </typeparam>
        /// <param name="container">
        ///     The container.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <returns>
        ///     The value composed.
        /// </returns>
        private static T Export<T>(CompositionContainer container, T value)
        {
            container.SatisfyImportsOnce(value);
            container.ComposeExportedValue(value);
            return value;
        }

        #endregion
    }
}