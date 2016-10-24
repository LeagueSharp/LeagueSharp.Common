// <copyright file="MenuManager.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Configuration
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    using SharpDX.Menu;

    using Menu = LeagueSharp.Common.Menu;

    /// <summary>
    ///     The menu manager, provides menu configuration for the new menu.
    /// </summary>
    public class MenuManager : IPartImportsSatisfiedNotification
    {
        #region Public Properties

        /// <summary>
        ///     Gets the menu instance.
        /// </summary>
        public static Menu InstanceMenu => Instances.MenuManager.Menu;

        /// <summary>
        ///     Gets the root menu.
        /// </summary>
        public Menu Menu { get; private set; }

        /// <summary>
        ///     Gets the factory.
        /// </summary>
        [Import(typeof(MenuFactory))]
        public MenuFactory MenuFactory { get; private set; }

        #endregion

        #region Properties

        private static IList<Menu> SatisfyMenuCollection { get; } = new List<Menu>();

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public void OnImportsSatisfied()
        {
            Instances.MenuManager = this;

            this.Menu = new Menu("LeagueSharp.Common", "LeagueSharp.Common", true);

            TargetSelector.Initialize(this.Menu);
            Prediction.Initialize(this.Menu);
            Hacks.Initialize(this.Menu);

            foreach (var menu in SatisfyMenuCollection)
            {
                this.MenuFactory.Add(menu.MenuReference, true);
            }

            SatisfyMenuCollection.Clear();
        }

        /// <summary>
        ///     Saves all existing menus.
        /// </summary>
        public void SaveAll()
        {
            foreach (var menu in this.MenuFactory.MenuCollection.Values)
            {
                menu.SaveComponents();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the menu for satisfaction once the manager is ready.
        /// </summary>
        /// <param name="menu">
        ///     The menu.
        /// </param>
        internal static void SatisfyMenu(Menu menu)
        {
            if (!SatisfyMenuCollection.Contains(menu))
            {
                SatisfyMenuCollection.Add(menu);
            }
        }

        #endregion
    }
}