#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 MenuWrapper.cs is part of LeagueSharp.Common.
 
 LeagueSharp.Common is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 LeagueSharp.Common is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with LeagueSharp.Common. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

#region

using System.Drawing;
using System.Text.RegularExpressions;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    /// A menu wrapper that adds the typical menus, such has orbwalker, and target selector.
    /// </summary>
    public class MenuWrapper
    {
        /// <summary>
        /// The main menu
        /// </summary>
        private readonly SubMenu _mainMenu;

        /// <summary>
        /// The menu
        /// </summary>
        private readonly Menu _menu;

        /// <summary>
        /// The orbwalker
        /// </summary>
        private readonly Orbwalking.Orbwalker _orbwalker;

        /// <summary>
        /// The_orbwalker menu
        /// </summary>
        private readonly Menu _orbwalkerMenu;

        /// <summary>
        /// The target selector menu
        /// </summary>
        private readonly Menu _targetSelectorMenu;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuWrapper"/> class.
        /// </summary>
        /// <param name="menuName">Name of the menu.</param>
        /// <param name="addTargetSelector">if set to <c>true</c> [add target selector].</param>
        /// <param name="addOrbwalker">if set to <c>true</c> [add orbwalker].</param>
        public MenuWrapper(string menuName, bool addTargetSelector = true, bool addOrbwalker = true)
        {
            // Create menu
            _menu = new Menu(menuName, Regex.Replace(menuName.ToLower(), @"\s+", ""), true);

            if (addTargetSelector)
            {
                // Target selector
                _targetSelectorMenu = new Menu("Target Selector", "ts");
                TargetSelector.AddToMenu(_targetSelectorMenu);
                _menu.AddSubMenu(_targetSelectorMenu);
            }

            if (addOrbwalker)
            {
                // Orbwalker
                _orbwalkerMenu = new Menu("Orbwalker", "orbwalker");
                _orbwalker = new Orbwalking.Orbwalker(_orbwalkerMenu);
                _menu.AddSubMenu(_orbwalkerMenu);
            }

            // Create main menu wrapper
            _mainMenu = new SubMenu(_menu);

            // Finalize menu
            _menu.AddToMainMenu();
        }

        /// <summary>
        /// Gets the main menu.
        /// </summary>
        /// <value>The main menu.</value>
        public SubMenu MainMenu
        {
            get { return _mainMenu; }
        }

        /// <summary>
        /// Gets the menu handle.
        /// </summary>
        /// <value>The menu handle.</value>
        public Menu MenuHandle
        {
            get { return _menu; }
        }

        /// <summary>
        /// Gets the target selector menu.
        /// </summary>
        /// <value>The target selector menu.</value>
        public Menu TargetSelectorMenu
        {
            get { return _targetSelectorMenu; }
        }

        /// <summary>
        /// Gets the orbwalker menu.
        /// </summary>
        /// <value>The orbwalker menu.</value>
        public Menu OrbwalkerMenu
        {
            get { return _orbwalkerMenu; }
        }

        /// <summary>
        /// Gets the orbwalker.
        /// </summary>
        /// <value>The orbwalker.</value>
        public Orbwalking.Orbwalker Orbwalker
        {
            get { return _orbwalker; }
        }

        /// <summary>
        /// A wrapper around a menu.
        /// </summary>
        public class SubMenu
        {
            /// <summary>
            /// Gets the parent.
            /// </summary>
            /// <value>The parent.</value>
            public SubMenu Parent { get; private set; }

            /// <summary>
            /// The sub menu
            /// </summary>
            private readonly Menu _subMenu;

            /// <summary>
            /// Initializes a new instance of the <see cref="SubMenu"/> class.
            /// </summary>
            /// <param name="current">The current.</param>
            public SubMenu(Menu current)
            {
                // This initializer is only for existing menus
                _subMenu = current;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SubMenu"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="name">The name.</param>
            public SubMenu(SubMenu parent, string name)
            {
                // Initialize this submenu
                Parent = parent;
                _subMenu = new Menu(name, GetName(this, name));

                // Add submenu to the parent menu
                parent.MenuHandle.AddSubMenu(_subMenu);
            }

            #region SubMenu

            /// <summary>
            /// Adds the sub menu.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>SubMenu.</returns>
            public SubMenu AddSubMenu(string name)
            {
                return new SubMenu(this, name);
            }

            /// <summary>
            /// Gets the sub menu.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>SubMenu.</returns>
            public SubMenu GetSubMenu(string name)
            {
                return new SubMenu(_subMenu.SubMenu(GetName(this, name))) { Parent = this };
            }

            #endregion

            #region Boolean

            /// <summary>
            /// Adds a bool.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
            /// <returns>SubMenu.</returns>
            public SubMenu AddBool(string name, bool defaultValue = true)
            {
                _subMenu.AddItem(new MenuItem(GetName(this, name), name).SetValue(defaultValue));
                return this;
            }

            /// <summary>
            /// Adds a linked bool.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
            /// <returns>BoolLink.</returns>
            public BoolLink AddLinkedBool(string name, bool defaultValue = true)
            {
                AddBool(name, defaultValue);
                return CreateBoolLink(name);
            }

            /// <summary>
            /// Gets the bool.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
            public bool GetBool(string name)
            {
                return _subMenu.Item(GetName(this, name)).GetValue<bool>();
            }

            /// <summary>
            /// Creates a bool link.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>BoolLink.</returns>
            public BoolLink CreateBoolLink(string name)
            {
                return new BoolLink(this, name);
            }

            #endregion

            #region Circle

            /// <summary>
            /// Adds a circle.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="enabled">if set to <c>true</c> [enabled].</param>
            /// <param name="color">The color.</param>
            /// <param name="radius">The radius.</param>
            /// <returns>SubMenu.</returns>
            public SubMenu AddCircle(string name, bool enabled, Color color, float radius = 100)
            {
                _subMenu.AddItem(new MenuItem(GetName(this, name), name).SetValue(new Circle(enabled, color, radius)));
                return this;
            }

            /// <summary>
            /// Adds a linked circle.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="enabled">if set to <c>true</c> [enabled].</param>
            /// <param name="color">The color.</param>
            /// <param name="radius">The radius.</param>
            /// <returns>CircleLink.</returns>
            public CircleLink AddLinkedCircle(string name, bool enabled, Color color, float radius = 100)
            {
                AddCircle(name, enabled, color, radius);
                return CreateCircleLink(name);
            }

            /// <summary>
            /// Gets the circle.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>Circle.</returns>
            public Circle GetCircle(string name)
            {
                return _subMenu.Item(GetName(this, name)).GetValue<Circle>();
            }

            /// <summary>
            /// Creates the circle link.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>CircleLink.</returns>
            public CircleLink CreateCircleLink(string name)
            {
                return new CircleLink(this, name);
            }

            #endregion

            #region KeyBind

            /// <summary>
            /// Adds a key bind.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="key">The key.</param>
            /// <param name="type">The type.</param>
            /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
            /// <returns>SubMenu.</returns>
            public SubMenu AddKeyBind(string name, uint key, KeyBindType type, bool defaultValue = false)
            {
                _subMenu.AddItem(new MenuItem(GetName(this, name), name).SetValue(new KeyBind(key, type, defaultValue)));
                return this;
            }

            /// <summary>
            /// Adds a linked key bind.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="key">The key.</param>
            /// <param name="type">The type.</param>
            /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
            /// <returns>KeyBindLink.</returns>
            public KeyBindLink AddLinkedKeyBind(string name, uint key, KeyBindType type, bool defaultValue = false)
            {
                AddKeyBind(name, key, type, defaultValue);
                return CreateKeyBindLink(name);
            }

            /// <summary>
            /// Gets the key bind.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>KeyBind.</returns>
            public KeyBind GetKeyBind(string name)
            {
                return _subMenu.Item(GetName(this, name)).GetValue<KeyBind>();
            }

            /// <summary>
            /// Creates the key bind link.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>KeyBindLink.</returns>
            public KeyBindLink CreateKeyBindLink(string name)
            {
                return new KeyBindLink(this, name);
            }

            #endregion

            #region Slider

            /// <summary>
            /// Adds a slider.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="value">The value.</param>
            /// <param name="minValue">The minimum value.</param>
            /// <param name="maxValue">The maximum value.</param>
            /// <returns>SubMenu.</returns>
            public SubMenu AddSlider(string name, int value, int minValue = 0, int maxValue = 100)
            {
                _subMenu.AddItem(new MenuItem(GetName(this, name), name).SetValue(new Slider(value, minValue, maxValue)));
                return this;
            }

            /// <summary>
            /// Adds a linked slider.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="value">The value.</param>
            /// <param name="minValue">The minimum value.</param>
            /// <param name="maxValue">The maximum value.</param>
            /// <returns>SliderLink.</returns>
            public SliderLink AddLinkedSlider(string name, int value, int minValue = 0, int maxValue = 100)
            {
                AddSlider(name, value, minValue, maxValue);
                return CreateSliderLink(name);
            }

            /// <summary>
            /// Gets the slider.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>Slider.</returns>
            public Slider GetSlider(string name)
            {
                return _subMenu.Item(GetName(this, name)).GetValue<Slider>();
            }

            /// <summary>
            /// Creates the slider link.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>SliderLink.</returns>
            public SliderLink CreateSliderLink(string name)
            {
                return new SliderLink(this, name);
            }

            #endregion

            #region StringList

            /// <summary>
            /// Adds the string list.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="sList">The s list.</param>
            /// <param name="defaultSelectedIndex">Default index of the selected.</param>
            /// <returns>SubMenu.</returns>
            public SubMenu AddStringList(string name, string[] sList, int defaultSelectedIndex = 0)
            {
                _subMenu.AddItem(new MenuItem(GetName(this, name), name).SetValue(new StringList(sList, defaultSelectedIndex)));
                return this;
            }

            /// <summary>
            /// Adds the linked string list.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="sList">The s list.</param>
            /// <param name="defaultSelectedIndex">Default index of the selected.</param>
            /// <returns>StringListLink.</returns>
            public StringListLink AddLinkedStringList(string name, string[] sList, int defaultSelectedIndex = 0)
            {
                AddStringList(name, sList, defaultSelectedIndex);
                return CreateStringListLink(name);
            }

            /// <summary>
            /// Gets the string list.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>StringList.</returns>
            public StringList GetStringList(string name)
            {
                return _subMenu.Item(GetName(this, name)).GetValue<StringList>();
            }

            /// <summary>
            /// Creates the string list link.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>StringListLink.</returns>
            public StringListLink CreateStringListLink(string name)
            {
                return new StringListLink(this, name);
            }

            #endregion

            /// <summary>
            /// Gets the menu handle.
            /// </summary>
            /// <value>The menu handle.</value>
            public Menu MenuHandle
            {
                get { return _subMenu; }
            }
        }

        #region MenuItemLinks

        /// <summary>
        /// A boolean link.
        /// </summary>
        public class BoolLink
        {
            /// <summary>
            /// Gets the sub menu.
            /// </summary>
            /// <value>The sub menu.</value>
            public SubMenu SubMenu { get; private set; }

            /// <summary>
            /// Gets the display name.
            /// </summary>
            /// <value>The display name.</value>
            public string DisplayName { get; private set; }

            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; private set; }


            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public bool Value
            {
                get { return SubMenu.MenuHandle.Item(Name).GetValue<bool>(); }
                set { SubMenu.MenuHandle.Item(Name).SetValue(value); }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="BoolLink"/> class.
            /// </summary>
            /// <param name="menu">The menu.</param>
            /// <param name="displayName">The display name.</param>
            public BoolLink(SubMenu menu, string displayName)
            {
                SubMenu = menu;
                DisplayName = displayName;
                Name = GetName(menu, displayName);
            }
        }

        /// <summary>
        /// A Circle link.
        /// </summary>
        public class CircleLink
        {
            /// <summary>
            /// Gets the sub menu.
            /// </summary>
            /// <value>The sub menu.</value>
            public SubMenu SubMenu { get; private set; }

            /// <summary>
            /// Gets the display name.
            /// </summary>
            /// <value>The display name.</value>
            public string DisplayName { get; private set; }

            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; private set; }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public Circle Value
            {
                get { return SubMenu.MenuHandle.Item(Name).GetValue<Circle>(); }
                set { SubMenu.MenuHandle.Item(Name).SetValue(value); }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="CircleLink"/> class.
            /// </summary>
            /// <param name="menu">The menu.</param>
            /// <param name="displayName">The display name.</param>
            public CircleLink(SubMenu menu, string displayName)
            {
                SubMenu = menu;
                DisplayName = displayName;
                Name = GetName(menu, displayName);
            }
        }

        /// <summary>
        /// A keybind link.
        /// </summary>
        public class KeyBindLink
        {
            /// <summary>
            /// Gets the sub menu.
            /// </summary>
            /// <value>The sub menu.</value>
            public SubMenu SubMenu { get; private set; }

            /// <summary>
            /// Gets the display name.
            /// </summary>
            /// <value>The display name.</value>
            public string DisplayName { get; private set; }

            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; private set; }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public KeyBind Value
            {
                get { return SubMenu.MenuHandle.Item(Name).GetValue<KeyBind>(); }
                set { SubMenu.MenuHandle.Item(Name).SetValue(value); }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="KeyBindLink"/> class.
            /// </summary>
            /// <param name="menu">The menu.</param>
            /// <param name="displayName">The display name.</param>
            public KeyBindLink(SubMenu menu, string displayName)
            {
                SubMenu = menu;
                DisplayName = displayName;
                Name = GetName(menu, displayName);
            }
        }

        /// <summary>
        /// A slider link.
        /// </summary>
        public class SliderLink
        {
            /// <summary>
            /// Gets the sub menu.
            /// </summary>
            /// <value>The sub menu.</value>
            public SubMenu SubMenu { get; private set; }

            /// <summary>
            /// Gets the display name.
            /// </summary>
            /// <value>The display name.</value>
            public string DisplayName { get; private set; }

            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; private set; }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public Slider Value
            {
                get { return SubMenu.MenuHandle.Item(Name).GetValue<Slider>(); }
                set { SubMenu.MenuHandle.Item(Name).SetValue(value); }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SliderLink"/> class.
            /// </summary>
            /// <param name="menu">The menu.</param>
            /// <param name="displayName">The display name.</param>
            public SliderLink(SubMenu menu, string displayName)
            {
                SubMenu = menu;
                DisplayName = displayName;
                Name = GetName(menu, displayName);
            }
        }

        /// <summary>
        /// A string list link.
        /// </summary>
        public class StringListLink
        {
            /// <summary>
            /// Gets the sub menu.
            /// </summary>
            /// <value>The sub menu.</value>
            public SubMenu SubMenu { get; private set; }

            /// <summary>
            /// Gets the display name.
            /// </summary>
            /// <value>The display name.</value>
            public string DisplayName { get; private set; }

            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; private set; }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public StringList Value
            {
                get { return SubMenu.MenuHandle.Item(Name).GetValue<StringList>(); }
                set { SubMenu.MenuHandle.Item(Name).SetValue(value); }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="StringListLink"/> class.
            /// </summary>
            /// <param name="menu">The menu.</param>
            /// <param name="displayName">The display name.</param>
            public StringListLink(SubMenu menu, string displayName)
            {
                SubMenu = menu;
                DisplayName = displayName;
                Name = GetName(menu, displayName);
            }
        }

        #endregion

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="name">The name.</param>
        /// <param name="fullName">if set to <c>true</c> <paramref name="name"/> gets the full name.</param>
        /// <returns>System.String.</returns>
        private static string GetName(SubMenu menu, string name, bool fullName = true)
        {
            var prefix = "";
            if (fullName)
            {
                var currentPartent = menu;
                while (currentPartent != null && currentPartent.MenuHandle != null)
                {
                    prefix = string.Format("{0}.{1}", currentPartent.MenuHandle.DisplayName, prefix);
                    currentPartent = currentPartent.Parent;
                }
            }
            return Regex.Replace(prefix + name.ToLower(), @"\s+", "_");
        }
    }
}