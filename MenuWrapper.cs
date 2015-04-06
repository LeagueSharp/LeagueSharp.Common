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
    public class MenuWrapper
    {
        private readonly SubMenu _mainMenu;

        private readonly Menu _menu;
        private readonly Orbwalking.Orbwalker _orbwalker;
        private readonly Menu _orbwalkerMenu;
        private readonly Menu _targetSelectorMenu;

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

        public SubMenu MainMenu
        {
            get { return _mainMenu; }
        }

        public Menu MenuHandle
        {
            get { return _menu; }
        }

        public Menu TargetSelectorMenu
        {
            get { return _targetSelectorMenu; }
        }

        public Menu OrbwalkerMenu
        {
            get { return _orbwalkerMenu; }
        }

        public Orbwalking.Orbwalker Orbwalker
        {
            get { return _orbwalker; }
        }

        public class SubMenu
        {
            public SubMenu Parent { get; private set; }
            private readonly Menu _subMenu;

            public SubMenu(Menu current)
            {
                // This initializer is only for existing menus
                _subMenu = current;
            }

            public SubMenu(SubMenu parent, string name)
            {
                // Initialize this submenu
                Parent = parent;
                _subMenu = new Menu(name, GetName(this, name));

                // Add submenu to the parent menu
                parent.MenuHandle.AddSubMenu(_subMenu);
            }

            #region SubMenu

            public SubMenu AddSubMenu(string name)
            {
                return new SubMenu(this, name);
            }

            public SubMenu GetSubMenu(string name)
            {
                return new SubMenu(_subMenu.SubMenu(GetName(this, name))) { Parent = this };
            }

            #endregion

            #region Boolean

            public SubMenu AddBool(string name, bool defaultValue = true)
            {
                _subMenu.AddItem(new MenuItem(GetName(this, name), name).SetValue(defaultValue));
                return this;
            }

            public BoolLink AddLinkedBool(string name, bool defaultValue = true)
            {
                AddBool(name, defaultValue);
                return CreateBoolLink(name);
            }

            public bool GetBool(string name)
            {
                return _subMenu.Item(GetName(this, name)).GetValue<bool>();
            }

            public BoolLink CreateBoolLink(string name)
            {
                return new BoolLink(this, name);
            }

            #endregion

            #region Circle

            public SubMenu AddCircle(string name, bool enabled, Color color, float radius = 100)
            {
                _subMenu.AddItem(new MenuItem(GetName(this, name), name).SetValue(new Circle(enabled, color, radius)));
                return this;
            }

            public CircleLink AddLinkedCircle(string name, bool enabled, Color color, float radius = 100)
            {
                AddCircle(name, enabled, color, radius);
                return CreateCircleLink(name);
            }

            public Circle GetCircle(string name)
            {
                return _subMenu.Item(GetName(this, name)).GetValue<Circle>();
            }

            public CircleLink CreateCircleLink(string name)
            {
                return new CircleLink(this, name);
            }

            #endregion

            #region KeyBind

            public SubMenu AddKeyBind(string name, uint key, KeyBindType type, bool defaultValue = false)
            {
                _subMenu.AddItem(new MenuItem(GetName(this, name), name).SetValue(new KeyBind(key, type, defaultValue)));
                return this;
            }

            public KeyBindLink AddLinkedKeyBind(string name, uint key, KeyBindType type, bool defaultValue = false)
            {
                AddKeyBind(name, key, type, defaultValue);
                return CreateKeyBindLink(name);
            }

            public KeyBind GetKeyBind(string name)
            {
                return _subMenu.Item(GetName(this, name)).GetValue<KeyBind>();
            }

            public KeyBindLink CreateKeyBindLink(string name)
            {
                return new KeyBindLink(this, name);
            }

            #endregion

            #region Slider

            public SubMenu AddSlider(string name, int value, int minValue = 0, int maxValue = 100)
            {
                _subMenu.AddItem(new MenuItem(GetName(this, name), name).SetValue(new Slider(value, minValue, maxValue)));
                return this;
            }

            public SliderLink AddLinkedSlider(string name, int value, int minValue = 0, int maxValue = 100)
            {
                AddSlider(name, value, minValue, maxValue);
                return CreateSliderLink(name);
            }

            public Slider GetSlider(string name)
            {
                return _subMenu.Item(GetName(this, name)).GetValue<Slider>();
            }

            public SliderLink CreateSliderLink(string name)
            {
                return new SliderLink(this, name);
            }

            #endregion

            #region StringList

            public SubMenu AddStringList(string name, string[] sList, int defaultSelectedIndex = 0)
            {
                _subMenu.AddItem(new MenuItem(GetName(this, name), name).SetValue(new StringList(sList, defaultSelectedIndex)));
                return this;
            }

            public StringListLink AddLinkedStringList(string name, string[] sList, int defaultSelectedIndex = 0)
            {
                AddStringList(name, sList, defaultSelectedIndex);
                return CreateStringListLink(name);
            }

            public StringList GetStringList(string name)
            {
                return _subMenu.Item(GetName(this, name)).GetValue<StringList>();
            }

            public StringListLink CreateStringListLink(string name)
            {
                return new StringListLink(this, name);
            }

            #endregion

            public Menu MenuHandle
            {
                get { return _subMenu; }
            }
        }

        #region MenuItemLinks

        public class BoolLink
        {
            public SubMenu SubMenu { get; private set; }
            public string DisplayName { get; private set; }
            public string Name { get; private set; }

            public bool Value
            {
                get { return SubMenu.MenuHandle.Item(Name).GetValue<bool>(); }
                set { SubMenu.MenuHandle.Item(Name).SetValue(value); }
            }

            public BoolLink(SubMenu menu, string displayName)
            {
                SubMenu = menu;
                DisplayName = displayName;
                Name = GetName(menu, displayName);
            }
        }

        public class CircleLink
        {
            public SubMenu SubMenu { get; private set; }
            public string DisplayName { get; private set; }
            public string Name { get; private set; }

            public Circle Value
            {
                get { return SubMenu.MenuHandle.Item(Name).GetValue<Circle>(); }
                set { SubMenu.MenuHandle.Item(Name).SetValue(value); }
            }

            public CircleLink(SubMenu menu, string displayName)
            {
                SubMenu = menu;
                DisplayName = displayName;
                Name = GetName(menu, displayName);
            }
        }

        public class KeyBindLink
        {
            public SubMenu SubMenu { get; private set; }
            public string DisplayName { get; private set; }
            public string Name { get; private set; }

            public KeyBind Value
            {
                get { return SubMenu.MenuHandle.Item(Name).GetValue<KeyBind>(); }
                set { SubMenu.MenuHandle.Item(Name).SetValue(value); }
            }

            public KeyBindLink(SubMenu menu, string displayName)
            {
                SubMenu = menu;
                DisplayName = displayName;
                Name = GetName(menu, displayName);
            }
        }

        public class SliderLink
        {
            public SubMenu SubMenu { get; private set; }
            public string DisplayName { get; private set; }
            public string Name { get; private set; }

            public Slider Value
            {
                get { return SubMenu.MenuHandle.Item(Name).GetValue<Slider>(); }
                set { SubMenu.MenuHandle.Item(Name).SetValue(value); }
            }

            public SliderLink(SubMenu menu, string displayName)
            {
                SubMenu = menu;
                DisplayName = displayName;
                Name = GetName(menu, displayName);
            }
        }

        public class StringListLink
        {
            public SubMenu SubMenu { get; private set; }
            public string DisplayName { get; private set; }
            public string Name { get; private set; }

            public StringList Value
            {
                get { return SubMenu.MenuHandle.Item(Name).GetValue<StringList>(); }
                set { SubMenu.MenuHandle.Item(Name).SetValue(value); }
            }

            public StringListLink(SubMenu menu, string displayName)
            {
                SubMenu = menu;
                DisplayName = displayName;
                Name = GetName(menu, displayName);
            }
        }

        #endregion

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