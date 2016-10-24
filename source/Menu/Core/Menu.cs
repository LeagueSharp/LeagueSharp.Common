// <copyright file="Menu.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using LeagueSharp.Common.Configuration;

    using SharpDX.Menu;

    using Color = SharpDX.Color;

    /// <summary>
    ///     The menu.
    /// </summary>
#pragma warning disable 612
    public partial class Menu
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Menu" /> class.
        /// </summary>
        /// <param name="displayName">
        ///     The display name.
        /// </param>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="isRootMenu">
        ///     Indicates whether the menu has root attribute.
        /// </param>
        public Menu(string displayName, string name, bool isRootMenu = false)
        {
            this.MenuReference = ComponentFactory.CreateMenu(name, displayName);
            if (isRootMenu)
            {
                if (Instances.MenuManager == null)
                {
                    MenuManager.SatisfyMenu(this);
                }
                else
                {
                    Instances.MenuManager.MenuFactory?.Add(this.MenuReference);
                }
            }

            this.IsRootMenu = isRootMenu;
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="Menu" /> class.
        /// </summary>
        ~Menu()
        {
            Instances.MenuManager?.MenuFactory?.Remove(this.MenuReference);
            this.IsRootMenu = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the menu children.
        /// </summary>
        public List<Menu> Children { get; } = new List<Menu>();

        /// <summary>
        ///     Gets or sets the color.
        /// </summary>
        [Obsolete]
        public Color Color { get; set; }

        /// <summary>
        ///     Gets or sets the menu display name.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this.MenuReference.DisplayName;
            }

            set
            {
                this.MenuReference.DisplayName = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the menu has a root attribute.
        /// </summary>
        [Obsolete]
        public bool IsRootMenu { get; set; }

        /// <summary>
        ///     Gets the menu items.
        /// </summary>
        public List<MenuItem> Items { get; } = new List<MenuItem>();

        /// <summary>
        ///     Gets or sets the menu name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.MenuReference.Name;
            }

            set
            {
                // TODO: Log?
            }
        }

        /// <summary>
        ///     Gets or sets the menu parent.
        /// </summary>
        public Menu Parent { get; set; }

        /// <summary>
        ///     Gets or sets the menu font style.
        /// </summary>
        [Obsolete]
        public FontStyle Style { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the menu reference.
        /// </summary>
        internal SharpDX.Menu.Menu MenuReference { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds a menu item component to the menu.
        /// </summary>
        /// <param name="item">
        ///     The menu item component.
        /// </param>
        /// <returns>
        ///     The <see cref="MenuItem" />.
        /// </returns>
        public MenuItem AddItem(MenuItem item)
        {
            var itemRef = item.MenuItemReference as MenuComponent;
            if (itemRef != null)
            {
                this.MenuReference.Add(itemRef);
            }

            this.Items.Add(item);
            item.Parent = this;
            return item;
        }

        /// <summary>
        ///     Adds a subdirectory menu to the menu.
        /// </summary>
        /// <param name="subMenu">
        ///     The subdirectory menu.
        /// </param>
        /// <returns>
        ///     The <see cref="Menu" />.
        /// </returns>
        public Menu AddSubMenu(Menu subMenu)
        {
            var itemRef = subMenu.MenuReference as MenuComponent;
            if (itemRef != null)
            {
                this.MenuReference.Add(itemRef);
            }

            this.Children.Add(subMenu);
            return subMenu;
        }

        /// <summary>
        ///     Adds the menu to the main menu.
        /// </summary>
        [Obsolete]
        public void AddToMainMenu()
        {
        }

        /// <summary>
        ///     Gets the menu's item by name.
        /// </summary>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="championUnique">
        ///     Indicates whether the item is champion unique.
        /// </param>
        /// <returns>
        ///     The <see cref="MenuItem" />.
        /// </returns>
        public MenuItem Item(string name, bool championUnique = false)
        {
            if (championUnique)
            {
                name = ObjectManager.Player.ChampionName + name;
            }

            foreach (var item in this.Items.ToArray().Where(item => item.Name == name))
            {
                return item;
            }

            return
                (from subMenu in this.Children.ToArray() where subMenu.Item(name) != null select subMenu.Item(name))
                    .FirstOrDefault();
        }

        /// <summary>
        ///     Removes the subdirectory menu from the menu.
        /// </summary>
        /// <param name="menu">
        ///     The subdirectory menu.
        /// </param>
        public void RemoveMenu(Menu menu)
        {
            var itemRef = menu.MenuReference as MenuComponent;
            if (itemRef != null)
            {
                this.MenuReference.Remove(itemRef);
            }

            this.Children.Remove(menu);
        }

        /// <summary>
        ///     Sets the font style.
        /// </summary>
        /// <param name="fontStyle">
        ///     The font style.
        /// </param>
        /// <param name="fontColor">
        ///     Color of the font.
        /// </param>
        /// <returns>
        ///     The <see cref="Menu" />.
        /// </returns>
        public Menu SetFontStyle(FontStyle fontStyle = FontStyle.Regular, Color? fontColor = null)
        {
            this.Style = fontStyle;

            var view = this.MenuReference.View;
            if (view != null)
            {
                view.SetAttribute(typeof(FontStyle).ToString(), fontStyle);
                if (fontColor.HasValue)
                {
                    var color = fontColor.Value;
                    this.Color = color;

                    var brush = new SolidBrush(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
                    view.SetAttribute(typeof(Brush).ToString(), brush);
                }
            }

            return this;
        }

        /// <summary>
        ///     Gets the menu's submenu by name.
        /// </summary>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <returns>
        ///     The <see cref="Menu" />.
        /// </returns>
        public Menu SubMenu(string name)
        {
            return this.Children.ToArray().FirstOrDefault(sm => sm.Name == name)
                   ?? this.AddSubMenu(new Menu(name, name));
        }

        #endregion
    }
}