namespace LeagueSharp.Common
{
    using System.Drawing;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     A menu wrapper that adds the typical menus, such has orbwalker, and target selector.
    /// </summary>
    public class MenuWrapper
    {
        #region Fields

        /// <summary>
        ///     The main menu
        /// </summary>
        private readonly SubMenu _mainMenu;

        /// <summary>
        ///     The menu
        /// </summary>
        private readonly Menu _menu;

        /// <summary>
        ///     The orbwalker
        /// </summary>
        private readonly Orbwalking.Orbwalker _orbwalker;

        /// <summary>
        ///     The_orbwalker menu
        /// </summary>
        private readonly Menu _orbwalkerMenu;

        /// <summary>
        ///     The target selector menu
        /// </summary>
        private readonly Menu _targetSelectorMenu;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MenuWrapper" /> class.
        /// </summary>
        /// <param name="menuName">Name of the menu.</param>
        /// <param name="addTargetSelector">if set to <c>true</c> [add target selector].</param>
        /// <param name="addOrbwalker">if set to <c>true</c> [add orbwalker].</param>
        public MenuWrapper(string menuName, bool addTargetSelector = true, bool addOrbwalker = true)
        {
            // Create menu
            this._menu = new Menu(menuName, Regex.Replace(menuName.ToLower(), @"\s+", ""), true);

            if (addTargetSelector)
            {
                // Target selector
                this._targetSelectorMenu = new Menu("Target Selector", "ts");
                TargetSelector.AddToMenu(this._targetSelectorMenu);
                this._menu.AddSubMenu(this._targetSelectorMenu);
            }

            if (addOrbwalker)
            {
                // Orbwalker
                this._orbwalkerMenu = new Menu("Orbwalker", "orbwalker");
                this._orbwalker = new Orbwalking.Orbwalker(this._orbwalkerMenu);
                this._menu.AddSubMenu(this._orbwalkerMenu);
            }

            // Create main menu wrapper
            this._mainMenu = new SubMenu(this._menu);

            // Finalize menu
            this._menu.AddToMainMenu();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the main menu.
        /// </summary>
        /// <value>The main menu.</value>
        public SubMenu MainMenu
        {
            get
            {
                return this._mainMenu;
            }
        }

        /// <summary>
        ///     Gets the menu handle.
        /// </summary>
        /// <value>The menu handle.</value>
        public Menu MenuHandle
        {
            get
            {
                return this._menu;
            }
        }

        /// <summary>
        ///     Gets the orbwalker.
        /// </summary>
        /// <value>The orbwalker.</value>
        public Orbwalking.Orbwalker Orbwalker
        {
            get
            {
                return this._orbwalker;
            }
        }

        /// <summary>
        ///     Gets the orbwalker menu.
        /// </summary>
        /// <value>The orbwalker menu.</value>
        public Menu OrbwalkerMenu
        {
            get
            {
                return this._orbwalkerMenu;
            }
        }

        /// <summary>
        ///     Gets the target selector menu.
        /// </summary>
        /// <value>The target selector menu.</value>
        public Menu TargetSelectorMenu
        {
            get
            {
                return this._targetSelectorMenu;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="name">The name.</param>
        /// <param name="fullName">if set to <c>true</c> <paramref name="name" /> gets the full name.</param>
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

        #endregion

        /// <summary>
        ///     A boolean link.
        /// </summary>
        public class BoolLink
        {
            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="BoolLink" /> class.
            /// </summary>
            /// <param name="menu">The menu.</param>
            /// <param name="displayName">The display name.</param>
            public BoolLink(SubMenu menu, string displayName)
            {
                this.SubMenu = menu;
                this.DisplayName = displayName;
                this.Name = GetName(menu, displayName);
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the display name.
            /// </summary>
            /// <value>The display name.</value>
            public string DisplayName { get; private set; }

            /// <summary>
            ///     Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; private set; }

            /// <summary>
            ///     Gets the sub menu.
            /// </summary>
            /// <value>The sub menu.</value>
            public SubMenu SubMenu { get; private set; }

            /// <summary>
            ///     Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public bool Value
            {
                get
                {
                    return this.SubMenu.MenuHandle.Item(this.Name).GetValue<bool>();
                }
                set
                {
                    this.SubMenu.MenuHandle.Item(this.Name).SetValue(value);
                }
            }

            #endregion
        }

        /// <summary>
        ///     A Circle link.
        /// </summary>
        public class CircleLink
        {
            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="CircleLink" /> class.
            /// </summary>
            /// <param name="menu">The menu.</param>
            /// <param name="displayName">The display name.</param>
            public CircleLink(SubMenu menu, string displayName)
            {
                this.SubMenu = menu;
                this.DisplayName = displayName;
                this.Name = GetName(menu, displayName);
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the display name.
            /// </summary>
            /// <value>The display name.</value>
            public string DisplayName { get; private set; }

            /// <summary>
            ///     Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; private set; }

            /// <summary>
            ///     Gets the sub menu.
            /// </summary>
            /// <value>The sub menu.</value>
            public SubMenu SubMenu { get; private set; }

            /// <summary>
            ///     Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public Circle Value
            {
                get
                {
                    return this.SubMenu.MenuHandle.Item(this.Name).GetValue<Circle>();
                }
                set
                {
                    this.SubMenu.MenuHandle.Item(this.Name).SetValue(value);
                }
            }

            #endregion
        }

        /// <summary>
        ///     A keybind link.
        /// </summary>
        public class KeyBindLink
        {
            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="KeyBindLink" /> class.
            /// </summary>
            /// <param name="menu">The menu.</param>
            /// <param name="displayName">The display name.</param>
            public KeyBindLink(SubMenu menu, string displayName)
            {
                this.SubMenu = menu;
                this.DisplayName = displayName;
                this.Name = GetName(menu, displayName);
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the display name.
            /// </summary>
            /// <value>The display name.</value>
            public string DisplayName { get; private set; }

            /// <summary>
            ///     Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; private set; }

            /// <summary>
            ///     Gets the sub menu.
            /// </summary>
            /// <value>The sub menu.</value>
            public SubMenu SubMenu { get; private set; }

            /// <summary>
            ///     Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public KeyBind Value
            {
                get
                {
                    return this.SubMenu.MenuHandle.Item(this.Name).GetValue<KeyBind>();
                }
                set
                {
                    this.SubMenu.MenuHandle.Item(this.Name).SetValue(value);
                }
            }

            #endregion
        }

        /// <summary>
        ///     A slider link.
        /// </summary>
        public class SliderLink
        {
            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="SliderLink" /> class.
            /// </summary>
            /// <param name="menu">The menu.</param>
            /// <param name="displayName">The display name.</param>
            public SliderLink(SubMenu menu, string displayName)
            {
                this.SubMenu = menu;
                this.DisplayName = displayName;
                this.Name = GetName(menu, displayName);
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the display name.
            /// </summary>
            /// <value>The display name.</value>
            public string DisplayName { get; private set; }

            /// <summary>
            ///     Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; private set; }

            /// <summary>
            ///     Gets the sub menu.
            /// </summary>
            /// <value>The sub menu.</value>
            public SubMenu SubMenu { get; private set; }

            /// <summary>
            ///     Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public Slider Value
            {
                get
                {
                    return this.SubMenu.MenuHandle.Item(this.Name).GetValue<Slider>();
                }
                set
                {
                    this.SubMenu.MenuHandle.Item(this.Name).SetValue(value);
                }
            }

            #endregion
        }

        /// <summary>
        ///     A string list link.
        /// </summary>
        public class StringListLink
        {
            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="StringListLink" /> class.
            /// </summary>
            /// <param name="menu">The menu.</param>
            /// <param name="displayName">The display name.</param>
            public StringListLink(SubMenu menu, string displayName)
            {
                this.SubMenu = menu;
                this.DisplayName = displayName;
                this.Name = GetName(menu, displayName);
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the display name.
            /// </summary>
            /// <value>The display name.</value>
            public string DisplayName { get; private set; }

            /// <summary>
            ///     Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; private set; }

            /// <summary>
            ///     Gets the sub menu.
            /// </summary>
            /// <value>The sub menu.</value>
            public SubMenu SubMenu { get; private set; }

            /// <summary>
            ///     Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public StringList Value
            {
                get
                {
                    return this.SubMenu.MenuHandle.Item(this.Name).GetValue<StringList>();
                }
                set
                {
                    this.SubMenu.MenuHandle.Item(this.Name).SetValue(value);
                }
            }

            #endregion
        }

        /// <summary>
        ///     A wrapper around a menu.
        /// </summary>
        public class SubMenu
        {
            #region Fields

            /// <summary>
            ///     The sub menu
            /// </summary>
            private readonly Menu _subMenu;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="SubMenu" /> class.
            /// </summary>
            /// <param name="current">The current.</param>
            public SubMenu(Menu current)
            {
                // This initializer is only for existing menus
                this._subMenu = current;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="SubMenu" /> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="name">The name.</param>
            public SubMenu(SubMenu parent, string name)
            {
                // Initialize this submenu
                this.Parent = parent;
                this._subMenu = new Menu(name, GetName(this, name));

                // Add submenu to the parent menu
                parent.MenuHandle.AddSubMenu(this._subMenu);
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the menu handle.
            /// </summary>
            /// <value>The menu handle.</value>
            public Menu MenuHandle
            {
                get
                {
                    return this._subMenu;
                }
            }

            /// <summary>
            ///     Gets the parent.
            /// </summary>
            /// <value>The parent.</value>
            public SubMenu Parent { get; private set; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Adds a bool.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
            /// <returns>SubMenu.</returns>
            public SubMenu AddBool(string name, bool defaultValue = true)
            {
                this._subMenu.AddItem(new MenuItem(GetName(this, name), name).SetValue(defaultValue));
                return this;
            }

            /// <summary>
            ///     Adds a circle.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="enabled">if set to <c>true</c> [enabled].</param>
            /// <param name="color">The color.</param>
            /// <param name="radius">The radius.</param>
            /// <returns>SubMenu.</returns>
            public SubMenu AddCircle(string name, bool enabled, Color color, float radius = 100)
            {
                this._subMenu.AddItem(
                    new MenuItem(GetName(this, name), name).SetValue(new Circle(enabled, color, radius)));
                return this;
            }

            /// <summary>
            ///     Adds a key bind.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="key">The key.</param>
            /// <param name="type">The type.</param>
            /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
            /// <returns>SubMenu.</returns>
            public SubMenu AddKeyBind(string name, uint key, KeyBindType type, bool defaultValue = false)
            {
                this._subMenu.AddItem(
                    new MenuItem(GetName(this, name), name).SetValue(new KeyBind(key, type, defaultValue)));
                return this;
            }

            /// <summary>
            ///     Adds a linked bool.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
            /// <returns>BoolLink.</returns>
            public BoolLink AddLinkedBool(string name, bool defaultValue = true)
            {
                this.AddBool(name, defaultValue);
                return this.CreateBoolLink(name);
            }

            /// <summary>
            ///     Adds a linked circle.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="enabled">if set to <c>true</c> [enabled].</param>
            /// <param name="color">The color.</param>
            /// <param name="radius">The radius.</param>
            /// <returns>CircleLink.</returns>
            public CircleLink AddLinkedCircle(string name, bool enabled, Color color, float radius = 100)
            {
                this.AddCircle(name, enabled, color, radius);
                return this.CreateCircleLink(name);
            }

            /// <summary>
            ///     Adds a linked key bind.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="key">The key.</param>
            /// <param name="type">The type.</param>
            /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
            /// <returns>KeyBindLink.</returns>
            public KeyBindLink AddLinkedKeyBind(string name, uint key, KeyBindType type, bool defaultValue = false)
            {
                this.AddKeyBind(name, key, type, defaultValue);
                return this.CreateKeyBindLink(name);
            }

            /// <summary>
            ///     Adds a linked slider.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="value">The value.</param>
            /// <param name="minValue">The minimum value.</param>
            /// <param name="maxValue">The maximum value.</param>
            /// <returns>SliderLink.</returns>
            public SliderLink AddLinkedSlider(string name, int value, int minValue = 0, int maxValue = 100)
            {
                this.AddSlider(name, value, minValue, maxValue);
                return this.CreateSliderLink(name);
            }

            /// <summary>
            ///     Adds the linked string list.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="sList">The s list.</param>
            /// <param name="defaultSelectedIndex">Default index of the selected.</param>
            /// <returns>StringListLink.</returns>
            public StringListLink AddLinkedStringList(string name, string[] sList, int defaultSelectedIndex = 0)
            {
                this.AddStringList(name, sList, defaultSelectedIndex);
                return this.CreateStringListLink(name);
            }

            /// <summary>
            ///     Adds a slider.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="value">The value.</param>
            /// <param name="minValue">The minimum value.</param>
            /// <param name="maxValue">The maximum value.</param>
            /// <returns>SubMenu.</returns>
            public SubMenu AddSlider(string name, int value, int minValue = 0, int maxValue = 100)
            {
                this._subMenu.AddItem(
                    new MenuItem(GetName(this, name), name).SetValue(new Slider(value, minValue, maxValue)));
                return this;
            }

            /// <summary>
            ///     Adds the string list.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="sList">The s list.</param>
            /// <param name="defaultSelectedIndex">Default index of the selected.</param>
            /// <returns>SubMenu.</returns>
            public SubMenu AddStringList(string name, string[] sList, int defaultSelectedIndex = 0)
            {
                this._subMenu.AddItem(
                    new MenuItem(GetName(this, name), name).SetValue(new StringList(sList, defaultSelectedIndex)));
                return this;
            }

            /// <summary>
            ///     Adds the sub menu.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>SubMenu.</returns>
            public SubMenu AddSubMenu(string name)
            {
                return new SubMenu(this, name);
            }

            /// <summary>
            ///     Creates a bool link.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>BoolLink.</returns>
            public BoolLink CreateBoolLink(string name)
            {
                return new BoolLink(this, name);
            }

            /// <summary>
            ///     Creates the circle link.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>CircleLink.</returns>
            public CircleLink CreateCircleLink(string name)
            {
                return new CircleLink(this, name);
            }

            /// <summary>
            ///     Creates the key bind link.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>KeyBindLink.</returns>
            public KeyBindLink CreateKeyBindLink(string name)
            {
                return new KeyBindLink(this, name);
            }

            /// <summary>
            ///     Creates the slider link.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>SliderLink.</returns>
            public SliderLink CreateSliderLink(string name)
            {
                return new SliderLink(this, name);
            }

            /// <summary>
            ///     Creates the string list link.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>StringListLink.</returns>
            public StringListLink CreateStringListLink(string name)
            {
                return new StringListLink(this, name);
            }

            /// <summary>
            ///     Gets the bool.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
            public bool GetBool(string name)
            {
                return this._subMenu.Item(GetName(this, name)).GetValue<bool>();
            }

            /// <summary>
            ///     Gets the circle.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>Circle.</returns>
            public Circle GetCircle(string name)
            {
                return this._subMenu.Item(GetName(this, name)).GetValue<Circle>();
            }

            /// <summary>
            ///     Gets the key bind.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>KeyBind.</returns>
            public KeyBind GetKeyBind(string name)
            {
                return this._subMenu.Item(GetName(this, name)).GetValue<KeyBind>();
            }

            /// <summary>
            ///     Gets the slider.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>Slider.</returns>
            public Slider GetSlider(string name)
            {
                return this._subMenu.Item(GetName(this, name)).GetValue<Slider>();
            }

            /// <summary>
            ///     Gets the string list.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>StringList.</returns>
            public StringList GetStringList(string name)
            {
                return this._subMenu.Item(GetName(this, name)).GetValue<StringList>();
            }

            /// <summary>
            ///     Gets the sub menu.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>SubMenu.</returns>
            public SubMenu GetSubMenu(string name)
            {
                return new SubMenu(this._subMenu.SubMenu(GetName(this, name))) { Parent = this };
            }

            #endregion
        }
    }
}