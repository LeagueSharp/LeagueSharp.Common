#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Orbwalking.cs is part of LeagueSharp.Common.
 
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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace LeagueSharp.Common
{
    [Serializable]
    public struct Circle
    {
        public bool Active;
        public Color Color;
        public float Radius;

        public Circle(bool enabled, Color color, float radius = 100)
        {
            Active = enabled;
            Color = color;
            Radius = radius;
        }
    }

    [Serializable]
    public struct Slider
    {
        public int MaxValue;
        public int MinValue;
        private int _value;

        public Slider(int value = 0, int minValue = 0, int maxValue = 100)
        {
            MaxValue = Math.Max(maxValue, minValue);
            MinValue = Math.Min(maxValue, minValue);
            _value = value;
        }

        public int Value
        {
            get { return _value; }
            set { _value = Math.Min(Math.Max(value, MinValue), MaxValue); }
        }
    }

    [Serializable]
    public struct StringList
    {
        public string[] SList;
        public int SelectedIndex;

        public StringList(string[] sList, int defaultSelectedIndex = 0)
        {
            SList = sList;
            SelectedIndex = defaultSelectedIndex;
        }
    }

    public enum KeyBindType
    {
        Toggle,
        Press,
    }

    [Serializable]
    public struct KeyBind
    {
        public bool Active;
        public uint Key;
        public KeyBindType Type;

        public KeyBind(uint key, KeyBindType type, bool defaultValue = false)
        {
            Key = key;
            Active = defaultValue;
            Type = type;
        }
    }

    internal static class MenuSettings
    {
        public static Vector2 BasePosition = new Vector2(10, 10);
        public static List<Color> ColorList = new List<Color>();
        public static Color BooleanOnColor = Color.FromArgb(150, Color.Green);
        public static Color BooleanOffColor = Color.FromArgb(150, Color.Red);
        public static Color StringListColor = Color.FromArgb(150, Color.Blue);

        public static XmlDocument Xml = new XmlDocument();
        /* Slider */
        public static Color SliderIndicator = Color.FromArgb(150, Color.Yellow);

        /*String Lists*/
        public static Color NextBColor = Color.FromArgb(100, Color.Blue);

        private static bool _drawTheMenu;
        private static int _cachedWidth = -1;
        private static int _cachedHeight = -1;
        private static Color _chachedBgColor = Color.Transparent;
        private static Color _chachedActiveColor = Color.Transparent;

        static MenuSettings()
        {
            /* Add the default colors to the list */
            ColorList.Add(Color.DarkSlateGray);
            ColorList.Add(Color.Black);
            ColorList.Add(Color.Gray);
            ColorList.Add(Color.White);
            ColorList.Add(Color.Red);
            ColorList.Add(Color.Fuchsia);
            ColorList.Add(Color.Lime);
            ColorList.Add(Color.Yellow);
            ColorList.Add(Color.Turquoise);

            if (File.Exists(MenuSettingsPath))
            {
                Xml.Load(MenuSettingsPath);
            }
            else
            {
                var rootNode = Xml.CreateElement("MenuSettings");
                Xml.AppendChild(rootNode);

                var varNode = Xml.CreateElement("Width");
                varNode.InnerText = "2";
                rootNode.AppendChild(varNode);

                varNode = Xml.CreateElement("Height");
                varNode.InnerText = "25";
                rootNode.AppendChild(varNode);

                varNode = Xml.CreateElement("BackgroundColor");
                varNode.InnerText = ColorTranslator.ToHtml(Color.DarkSlateGray);
                rootNode.AppendChild(varNode);

                varNode = Xml.CreateElement("BackgroundColorAlpha");
                varNode.InnerText = "100";
                rootNode.AppendChild(varNode);

                varNode = Xml.CreateElement("ActiveColor");
                varNode.InnerText = ColorTranslator.ToHtml(Color.Red);
                rootNode.AppendChild(varNode);

                varNode = Xml.CreateElement("ActiveColorAlpha");
                varNode.InnerText = "150";
                rootNode.AppendChild(varNode);

                varNode = Xml.CreateElement("ShowMenuPress");
                varNode.InnerText = "16";
                rootNode.AppendChild(varNode);

                varNode = Xml.CreateElement("ShowMenuToggle");
                varNode.InnerText = "120";
                rootNode.AppendChild(varNode);
            }

            Directory.CreateDirectory(MenuConfigPath);

            Xml.Save(MenuSettingsPath);

            Game.OnWndProc += Game_OnWndProc;
            _drawTheMenu = Global.Read<bool>("DrawMenu", true);
        }

        internal static bool DrawMenu
        {
            get { return _drawTheMenu; }
            set
            {
                _drawTheMenu = value;
                Global.Write("DrawMenu", value);
            }
        }


        public static string MenuSettingsPath
        {
            get { return MenuConfigPath + "MenuSettings.xml"; }
        }

        public static string MenuConfigPath
        {
            get
            {
                return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) +
                       "\\CommonLibMenuConfig\\";
            }
        }

        public static int MenuItemWidth
        {
            get
            {
                var m = _cachedWidth != -1 ? _cachedWidth : Convert.ToInt32(GetXmlValue("Width"));
                _cachedWidth = m;
                return Drawing.Width / (10 - m);
            }
        }

        public static int MenuItemHeight
        {
            get
            {
                _cachedHeight = _cachedHeight != -1 ? _cachedHeight : Convert.ToInt32(GetXmlValue("Height"));
                return _cachedHeight;
            }
        }

        public static Color BackgroundColor
        {
            get
            {
                if (_chachedBgColor != Color.Transparent)
                {
                    return _chachedBgColor;
                }

                var color = ColorTranslator.FromHtml(GetXmlValue("BackgroundColor"));
                var alpha = GetXmlValue("BackgroundColorAlpha");
                _chachedBgColor = Color.FromArgb(Convert.ToInt32(alpha), color);
                return _chachedBgColor;
            }
        }

        public static Color ActiveBackgroundColor
        {
            get
            {
                if (_chachedActiveColor != Color.Transparent)
                {
                    return _chachedActiveColor;
                }

                var color = ColorTranslator.FromHtml(GetXmlValue("ActiveColor"));
                var alpha = GetXmlValue("ActiveColorAlpha");
                _chachedActiveColor = Color.FromArgb(Convert.ToInt32(alpha), color);
                return _chachedActiveColor;
            }
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if ((args.Msg == (uint) WindowsMessages.WM_KEYUP || args.Msg == (uint) WindowsMessages.WM_KEYDOWN) &&
                args.WParam == Convert.ToInt32(GetXmlValue("ShowMenuPress")))
            {
                DrawMenu = args.Msg == (uint) WindowsMessages.WM_KEYDOWN;
            }

            if (args.Msg == (uint) WindowsMessages.WM_KEYUP &&
                args.WParam == Convert.ToInt32(GetXmlValue("ShowMenuToggle")))
            {
                DrawMenu = !DrawMenu;
            }
        }

        private static string GetXmlValue(string tagName)
        {
            return Xml.GetElementsByTagName(tagName)[0].InnerText;
        }
    }

    internal static class MenuDrawHelper
    {
        internal static void DrawBox(Vector2 position,
            int width,
            int height,
            Color color,
            int borderwidth,
            Color borderColor)
        {
            Drawing.DrawLine(position.X, position.Y, position.X + width, position.Y, height, color);

            if (borderwidth > 0)
            {
                Drawing.DrawLine(position.X, position.Y, position.X + width, position.Y, borderwidth, borderColor);
                Drawing.DrawLine(
                    position.X, position.Y + height, position.X + width, position.Y + height, borderwidth, borderColor);
                Drawing.DrawLine(position.X, position.Y, position.X, position.Y + height, borderwidth, borderColor);
                Drawing.DrawLine(
                    position.X + width, position.Y, position.X + width, position.Y + height, borderwidth, borderColor);
            }
        }

        internal static void DrawOnOff(bool on, Vector2 position, MenuItem item)
        {
            DrawBox(
                position, item.Height, item.Height, on ? MenuSettings.BooleanOnColor : MenuSettings.BooleanOffColor, 1,
                Color.Black);
            var s = on ? "On" : "Off";
            Drawing.DrawText(
                item.Position.X + item.Width - item.Height + (item.Height - Drawing.GetTextExtent(s).Width) / 2 - 2,
                item.Position.Y + (item.Height - Drawing.GetTextExtent(s).Height) / 2, Color.White, s);
        }

        internal static void DrawArrow(string s, Vector2 position, MenuItem item, Color color)
        {
            DrawBox(position, item.Height, item.Height, color, 1, Color.Black);
            Drawing.DrawText(
                position.X + (item.Height - Drawing.GetTextExtent(s).Width) / 2 - 2,
                item.Position.Y + (item.Height - Drawing.GetTextExtent(s).Height) / 2, Color.White, s);
        }

        internal static void DrawSlider(Vector2 position, MenuItem item, int width = -1, bool drawText = true)
        {
            var val = item.GetValue<Slider>();
            DrawSlider(position, item, val.MinValue, val.MaxValue, val.Value, width, drawText);
        }

        internal static void DrawSlider(Vector2 position,
            MenuItem item,
            int min,
            int max,
            int value,
            int width,
            bool drawText)
        {
            width = (width > 0 ? width : item.Width);
            var percentage = 100 * (value - min) / (max - min);
            var x = position.X + (percentage * width) / 100;
            Drawing.DrawLine(x, position.Y + 2, x, position.Y + item.Height, 2, MenuSettings.SliderIndicator);

            if (drawText)
            {
                Drawing.DrawText(
                    position.X - 7 + width - Drawing.GetTextExtent(value.ToString()).Width,
                    position.Y + (item.Height - Drawing.GetTextExtent(value.ToString()).Height) / 2, Color.White,
                    value.ToString());
            }
        }
    }

    public class Menu
    {
        public List<Menu> Children = new List<Menu>();

        public string DisplayName;
        public bool IsRootMenu;
        public List<MenuItem> Items = new List<MenuItem>();
        public string Name;
        public Menu Parent;
        internal int _cachedMenuCount = -1;
        internal int _cachedMenuCountT = 0;

        private bool _visible;

        public Menu(string displayName, string name, bool isRootMenu = false)
        {
            DisplayName = displayName;
            Name = name;
            IsRootMenu = isRootMenu;

            if (isRootMenu)
            {
                CustomEvents.Game.OnGameEnd += delegate { SaveAll(); };
                Game.OnGameEnd += delegate { SaveAll(); };
                AppDomain.CurrentDomain.DomainUnload += delegate
                {
                    var m = Global.Read<List<string>>("CommonMenuList", true);
                    if (m == default(List<string>))
                    {
                        m = new List<string>();
                    }
                    m.Remove(DisplayName + Name);
                    Global.Write("CommonMenuList", m);

                    SaveAll();
                };
                AppDomain.CurrentDomain.ProcessExit += delegate { SaveAll(); };
            }
        }

        internal int XLevel
        {
            get
            {
                var result = 0;
                var m = this;
                while (m.Parent != null)
                {
                    m = m.Parent;
                    result++;
                }

                return result;
            }
        }

        internal int YLevel
        {
            get
            {
                if (IsRootMenu || Parent == null)
                {
                    return 0;
                }
                var result = Parent.YLevel;

                foreach (var test in Parent.Children)
                {
                    if (test.Name == Name)
                    {
                        break;
                    }
                    result++;
                }

                return result;
            }
        }

        internal int MenuCount
        {
            get
            {
                if (Environment.TickCount - _cachedMenuCountT < 500)
                {
                    return _cachedMenuCount;
                }
                var l = Global.Read<List<string>>("CommonMenuList");
                var result = 0;
                foreach (var s in l)
                {
                    if (s == DisplayName + Name)
                    {
                        break;
                    }
                    result++;
                }

                _cachedMenuCount = result;
                _cachedMenuCountT = Environment.TickCount;
                return result;
            }
        }

        internal Vector2 MyBasePosition
        {
            get
            {
                if (IsRootMenu || Parent == null)
                {
                    return MenuSettings.BasePosition + MenuCount * new Vector2(0, MenuSettings.MenuItemHeight);
                }

                return Parent.MyBasePosition;
            }
        }

        internal Vector2 Position
        {
            get
            {
                return MyBasePosition + XLevel * new Vector2(MenuSettings.MenuItemWidth, 0) +
                       YLevel * new Vector2(0, MenuSettings.MenuItemHeight);
            }
        }

        internal int Width
        {
            get { return MenuSettings.MenuItemWidth; }
        }

        internal int Height
        {
            get { return MenuSettings.MenuItemHeight; }
        }

        internal bool Visible
        {
            get
            {
                if (!MenuSettings.DrawMenu)
                {
                    return false;
                }
                return IsRootMenu ? true : _visible;
            }
            set { _visible = value; }
        }

        internal bool IsInside(Vector2 position)
        {
            return Utils.IsUnderRectangle(position, Position.X, Position.Y, Width, Height);
        }

        internal void Game_OnWndProc(WndEventArgs args)
        {
            OnReceiveMessage((WindowsMessages) args.Msg, Utils.GetCursorPos(), args.WParam);
        }

        internal void OnReceiveMessage(WindowsMessages message, Vector2 cursorPos, uint key)
        {
            //Spread the message to the menu's children recursively
            foreach (var child in Children)
            {
                child.OnReceiveMessage(message, cursorPos, key);
            }


            foreach (var item in Items)
            {
                item.OnReceiveMessage(message, cursorPos, key);
            }

            //Handle the left clicks on the menus to hide or show the submenus.
            if (message != WindowsMessages.WM_LBUTTONDOWN)
            {
                return;
            }

            if (IsRootMenu && Visible)
            {
                if (cursorPos.X - MenuSettings.BasePosition.X < MenuSettings.MenuItemWidth)
                {
                    var n = (int) (cursorPos.Y - MenuSettings.BasePosition.Y) / MenuSettings.MenuItemHeight;
                    if (MenuCount != n && n < Global.Read<List<string>>("CommonMenuList").Count)
                    {
                        foreach (var schild in Children)
                        {
                            schild.Visible = false;
                        }

                        foreach (var sitem in Items)
                        {
                            sitem.Visible = false;
                        }
                    }
                }
            }

            if (!IsInside(cursorPos))
            {
                return;
            }
            if (!Visible)
            {
                return;
            }

            if (!IsRootMenu && Parent != null)
            {
                //Close all the submenus in the level 
                foreach (var child in Parent.Children)
                {
                    if (child.Name != Name)
                    {
                        foreach (var schild in child.Children)
                        {
                            schild.Visible = false;
                        }

                        foreach (var sitem in child.Items)
                        {
                            sitem.Visible = false;
                        }
                    }
                }
            }

            //Hide or Show the submenus.
            foreach (var child in Children)
            {
                child.Visible = !child.Visible;
            }

            //Hide or Show the items.
            foreach (var item in Items)
            {
                item.Visible = !item.Visible;
            }
        }

        internal void Drawing_OnDraw(EventArgs args)
        {
            if (!Visible)
            {
                return;
            }

            MenuDrawHelper.DrawBox(
                Position, Width, Height,
                (Children.Count > 0 && Children[0].Visible || Items.Count > 0 && Items[0].Visible)
                    ? MenuSettings.ActiveBackgroundColor
                    : MenuSettings.BackgroundColor, 1, Color.Black);
            Drawing.DrawText(
                Position.X + 5, Position.Y + (Height - Drawing.GetTextExtent(DisplayName).Height) / 2, Color.White,
                DisplayName);

            Drawing.DrawText(
                Position.X + Width - 15, Position.Y + (Height - Drawing.GetTextExtent(DisplayName).Height) / 2,
                Color.White, ">");

            //Draw the menu submenus
            foreach (var child in Children)
            {
                if (child.Visible)
                {
                    child.Drawing_OnDraw(args);
                }
            }

            //Draw the items
            for (var i = Items.Count - 1; i >= 0; i--)
            {
                var item = Items[i];
                if (item.Visible)
                {
                    item.Drawing_OnDraw();
                }
            }
        }

        internal void SaveAll()
        {
            foreach (var child in Children)
            {
                child.SaveAll();
            }

            foreach (var item in Items)
            {
                item.SaveToFile();
            }
        }

        public void AddToMainMenu()
        {
            Drawing.OnDraw += Drawing_OnDraw;


            Game.OnWndProc += Game_OnWndProc;
            var m = Global.Read<List<string>>("CommonMenuList", true);
            if (m == default(List<string>))
            {
                m = new List<string>();
            }
            m.Add(DisplayName + Name);
            Global.Write("CommonMenuList", m);
        }

        public MenuItem AddItem(MenuItem item)
        {
            item.Parent = this;
            Items.Add(item);
            return item;
        }

        public Menu AddSubMenu(Menu subMenu)
        {
            subMenu.Parent = this;
            Children.Add(subMenu);

            return subMenu;
        }

        public MenuItem Item(string name)
        {
            //Search in our own items
            foreach (var item in Items)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }

            //Search in submenus
            foreach (var subMenu in Children)
            {
                if (subMenu.Item(name) != null)
                {
                    return subMenu.Item(name);
                }
            }

            return null;
        }

        public Menu SubMenu(string name)
        {
            //Search in submenus
            foreach (var subMenu in Children)
            {
                if (subMenu.Name == name)
                {
                    return subMenu;
                }
            }
            return null;
        }
    }

    internal enum MenuValueType
    {
        None,
        Boolean,
        Slider,
        KeyBind,
        Integer,
        Color,
        Circle,
        StringList,
    }

    public class OnValueChangeEventArgs
    {
        private readonly object _newValue;
        private readonly object _oldValue;

        public OnValueChangeEventArgs(object oldValue, object newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public T GetOldValue<T>()
        {
            return (T) _oldValue;
        }

        public T GetNewValue<T>()
        {
            return (T) _newValue;
        }
    }

    public class MenuItem
    {
        private readonly string _assemblyPath;
        public string DisplayName;
        internal bool Interacting;
        public string Name;

        public Menu Parent;
        internal MenuValueType ValueType;

        private bool _dontSave;
        private bool _isShared;

        private string _saveFilePath;
        private bool _saved;
        private byte[] _serialized;
        private object _value;
        private bool _valueSet;
        private bool _visible;

        public MenuItem(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
            _assemblyPath = System.Reflection.Assembly.GetCallingAssembly().Location;
        }

        internal bool Visible
        {
            get
            {
                if (!MenuSettings.DrawMenu)
                {
                    return false;
                }
                return _visible;
            }
            set { _visible = value; }
        }

        internal int XLevel
        {
            get
            {
                if (Parent == null)
                {
                    return 1;
                }
                var result = 1;
                var m = Parent;
                while (m.Parent != null)
                {
                    m = m.Parent;
                    result++;
                }

                return result;
            }
        }

        internal int YLevel
        {
            get
            {
                if (Parent == null)
                {
                    return 0;
                }
                var result = Parent.YLevel + Parent.Children.Count;

                foreach (var test in Parent.Items)
                {
                    if (test.Name == Name)
                    {
                        break;
                    }
                    result++;
                }
                return result;
            }
        }

        internal Vector2 MyBasePosition
        {
            get
            {
                if (Parent == null)
                {
                    return MenuSettings.BasePosition;
                }

                return Parent.MyBasePosition;
            }
        }


        internal Vector2 Position
        {
            get
            {
                return MyBasePosition + XLevel * new Vector2(MenuSettings.MenuItemWidth, 0) +
                       YLevel * new Vector2(0, MenuSettings.MenuItemHeight);
            }
        }

        internal int Width
        {
            get { return MenuSettings.MenuItemWidth; }
        }

        internal int Height
        {
            get { return MenuSettings.MenuItemHeight; }
        }

        public event EventHandler<OnValueChangeEventArgs> ValueChanged;

        public MenuItem SetShared()
        {
            _isShared = true;
            return this;
        }

        public MenuItem DontSave()
        {
            _dontSave = true;
            return this;
        }

        public T GetValue<T>()
        {
            return (T) _value;
        }

        public MenuItem SetValue<T>(T newValue)
        {
            ValueType = MenuValueType.None;
            if (newValue.GetType().ToString().Contains("Boolean"))
            {
                ValueType = MenuValueType.Boolean;
            }
            else if (newValue.GetType().ToString().Contains("Slider"))
            {
                ValueType = MenuValueType.Slider;
            }
            else if (newValue.GetType().ToString().Contains("KeyBind"))
            {
                ValueType = MenuValueType.KeyBind;
            }
            else if (newValue.GetType().ToString().Contains("Int"))
            {
                ValueType = MenuValueType.Integer;
            }
            else if (newValue.GetType().ToString().Contains("Circle"))
            {
                ValueType = MenuValueType.Circle;
            }
            else if (newValue.GetType().ToString().Contains("StringList"))
            {
                ValueType = MenuValueType.StringList;
            }
            else if (newValue.GetType().ToString().Contains("Color"))
            {
                ValueType = MenuValueType.Color;
            }
            else
            {
                Game.PrintChat("CommonLibMenu: Data type not supported");
            }

            var dname = (_isShared ? "SharedConfig" : Path.GetFileName(_assemblyPath));
            Directory.CreateDirectory(MenuSettings.MenuConfigPath);
            Directory.CreateDirectory(MenuSettings.MenuConfigPath + dname);

            _saveFilePath = MenuSettings.MenuConfigPath + dname + "\\" +
                            Utils.Md5Hash("v2" + DisplayName + Name + newValue.GetType());

            if (!_valueSet && File.Exists(_saveFilePath))
            {
                if (ValueType == MenuValueType.KeyBind)
                {
                    var SavedVal = (KeyBind) (object) Global.Deserialize<T>(File.ReadAllBytes(_saveFilePath));
                    if (SavedVal.Type == KeyBindType.Press)
                    {
                        SavedVal.Active = false;
                    }
                    newValue = (T) (object) SavedVal;
                }
                else
                {
                    newValue = Global.Deserialize<T>(File.ReadAllBytes(_saveFilePath));
                }
            }

            if (_valueSet)
            {
                var handler = ValueChanged;
                if (handler != null)
                {
                    handler(this, new OnValueChangeEventArgs(_value, newValue));
                }
            }

            _valueSet = true;

            _value = newValue;
            _serialized = Global.Serialize(_value);
            return this;
        }

        internal void SaveToFile()
        {
            if (!_saved && _saveFilePath != null && !_dontSave)
            {
                File.WriteAllBytes(_saveFilePath, _serialized);
                _saved = true;
            }
        }

        internal bool IsInside(Vector2 position)
        {
            return Utils.IsUnderRectangle(position, Position.X, Position.Y, Width, Height);
        }

        internal void OnReceiveMessage(WindowsMessages message, Vector2 cursorPos, uint key)
        {
            switch (ValueType)
            {
                case MenuValueType.Boolean:

                    if (message != WindowsMessages.WM_LBUTTONDOWN)
                    {
                        return;
                    }
                    if (!IsInside(cursorPos))
                    {
                        return;
                    }
                    if (!Visible)
                    {
                        return;
                    }

                    if (cursorPos.X > Position.X + Width - Height)
                    {
                        SetValue(!GetValue<bool>());
                    }

                    break;

                case MenuValueType.Slider:
                    if (!Visible)
                    {
                        Interacting = false;
                        return;
                    }

                    if (message == WindowsMessages.WM_MOUSEMOVE && Interacting ||
                        message == WindowsMessages.WM_LBUTTONDOWN && !Interacting && IsInside(cursorPos))
                    {
                        var val = GetValue<Slider>();
                        var t = val.MinValue + ((cursorPos.X - Position.X) * (val.MaxValue - val.MinValue)) / Width;
                        val.Value = (int) t;
                        SetValue(val);
                    }

                    if (message != WindowsMessages.WM_LBUTTONDOWN && message != WindowsMessages.WM_LBUTTONUP)
                    {
                        return;
                    }
                    if (!IsInside(cursorPos) && message == WindowsMessages.WM_LBUTTONDOWN)
                    {
                        return;
                    }

                    Interacting = message == WindowsMessages.WM_LBUTTONDOWN;
                    break;

                case MenuValueType.Color:

                    if (message != WindowsMessages.WM_LBUTTONDOWN)
                    {
                        return;
                    }
                    if (!IsInside(cursorPos))
                    {
                        return;
                    }
                    if (!Visible)
                    {
                        return;
                    }

                    if (cursorPos.X > Position.X + Width - Height)
                    {
                        var c = GetValue<Color>();
                        ColorPicker.Load(delegate(Color args) { SetValue(args); }, c);
                    }

                    break;
                case MenuValueType.Circle:

                    if (message != WindowsMessages.WM_LBUTTONDOWN)
                    {
                        return;
                    }
                    if (!Visible)
                    {
                        return;
                    }
                    if (!IsInside(cursorPos))
                    {
                        return;
                    }

                    if (cursorPos.X - Position.X > Width - Height)
                    {
                        var val = GetValue<Circle>();
                        val.Active = !val.Active;
                        SetValue(val);
                    }
                    else
                    {
                        var c = GetValue<Circle>();
                        ColorPicker.Load(
                            delegate(Color args)
                            {
                                var val = GetValue<Circle>();
                                val.Color = args;
                                SetValue(val);
                            }, c.Color);
                    }

                    break;
                case MenuValueType.KeyBind:

                    if (!MenuGUI.IsChatOpen)
                    {
                        switch (message)
                        {
                            case WindowsMessages.WM_KEYDOWN:
                                var val = GetValue<KeyBind>();
                                if (key == val.Key)
                                {
                                    if (val.Type == KeyBindType.Press)
                                    {
                                        if (!val.Active)
                                        {
                                            val.Active = true;
                                            SetValue(val);
                                        }
                                    }
                                }
                                break;
                            case WindowsMessages.WM_KEYUP:

                                var val2 = GetValue<KeyBind>();
                                if (key == val2.Key)
                                {
                                    if (val2.Type == KeyBindType.Press)
                                    {
                                        val2.Active = false;
                                        SetValue(val2);
                                    }
                                    else
                                    {
                                        val2.Active = !val2.Active;
                                        SetValue(val2);
                                    }
                                }
                                break;
                        }
                    }

                    if (message == WindowsMessages.WM_KEYUP && Interacting)
                    {
                        var val = GetValue<KeyBind>();
                        val.Key = key;
                        SetValue(val);
                        Interacting = false;
                    }

                    if (!Visible)
                    {
                        return;
                    }

                    if (message != WindowsMessages.WM_LBUTTONDOWN)
                    {
                        return;
                    }

                    if (!IsInside(cursorPos))
                    {
                        return;
                    }
                    if (cursorPos.X > Position.X + Width - Height)
                    {
                        var val = GetValue<KeyBind>();
                        val.Active = !val.Active;
                        SetValue(val);
                    }
                    else
                    {
                        Interacting = !Interacting;
                    }
                    break;
                case MenuValueType.StringList:
                    if (!Visible)
                    {
                        return;
                    }
                    if (message != WindowsMessages.WM_LBUTTONDOWN)
                    {
                        return;
                    }
                    if (!IsInside(cursorPos))
                    {
                        return;
                    }

                    var slVal = GetValue<StringList>();
                    if (cursorPos.X > Position.X + Width - Height)
                    {
                        slVal.SelectedIndex = slVal.SelectedIndex == slVal.SList.Length - 1
                            ? 0
                            : (slVal.SelectedIndex + 1);
                        SetValue(slVal);
                    }
                    else if (cursorPos.X > Position.X + Width - 2 * Height)
                    {
                        slVal.SelectedIndex = slVal.SelectedIndex == 0
                            ? slVal.SList.Length - 1
                            : (slVal.SelectedIndex - 1);
                        SetValue(slVal);
                    }

                    break;
            }
        }

        internal void Drawing_OnDraw()
        {
            MenuDrawHelper.DrawBox(Position, Width, Height, MenuSettings.BackgroundColor, 1, Color.Black);
            var s = DisplayName;

            switch (ValueType)
            {
                case MenuValueType.Boolean:
                    MenuDrawHelper.DrawOnOff(
                        GetValue<bool>(), new Vector2(Position.X + Width - Height, Position.Y), this);
                    break;

                case MenuValueType.Slider:
                    MenuDrawHelper.DrawSlider(Position, this);
                    break;

                case MenuValueType.KeyBind:
                    var val = GetValue<KeyBind>();
                    s += " (" + Utils.KeyToText(val.Key) + ")";
                    if (Interacting)
                    {
                        s = "Press new key";
                    }
                    MenuDrawHelper.DrawOnOff(val.Active, new Vector2(Position.X + Width - Height, Position.Y), this);

                    break;

                case MenuValueType.Integer:
                    var intVal = GetValue<int>();
                    Drawing.DrawText(
                        Position.X + Width - Drawing.GetTextExtent(intVal.ToString()).Width - 7,
                        Position.Y + (Height - Drawing.GetTextExtent(intVal.ToString()).Height) / 2, Color.White,
                        intVal.ToString());
                    break;

                case MenuValueType.Color:
                    var colorVal = GetValue<Color>();
                    MenuDrawHelper.DrawBox(
                        Position + new Vector2(Width - Height, 0), Height, Height, colorVal, 1, Color.Black);
                    break;

                case MenuValueType.Circle:
                    var circleVal = GetValue<Circle>();
                    MenuDrawHelper.DrawBox(
                        Position + new Vector2(Width - Height * 2, 0), Height, Height, circleVal.Color, 1, Color.Black);
                    MenuDrawHelper.DrawOnOff(
                        circleVal.Active, new Vector2(Position.X + Width - Height, Position.Y), this);
                    break;

                case MenuValueType.StringList:
                    var slVal = GetValue<StringList>();

                    var t = slVal.SList[slVal.SelectedIndex];

                    MenuDrawHelper.DrawArrow(
                        "<", Position + new Vector2(Width - Height * 2, 0), this, MenuSettings.StringListColor);
                    MenuDrawHelper.DrawArrow(
                        ">", Position + new Vector2(Width - Height, 0), this, MenuSettings.StringListColor);

                    Drawing.DrawText(
                        Position.X + Width - Drawing.GetTextExtent(t).Width - 2 * Height - 20,
                        Position.Y + (Height - Drawing.GetTextExtent(t).Height) / 2, Color.White, t);

                    break;
            }

            Drawing.DrawText(
                Position.X + 5, Position.Y + (Height - Drawing.GetTextExtent(s).Height) / 2, Color.White, s);
        }
    }

    public static class ColorPicker
    {
        public delegate void OnSelectColor(Color color);

        public static OnSelectColor OnChangeColor;
        private static int _x = 100;
        private static int _y = 100;

        private static bool _moving;
        private static bool _selecting;

        public static Render.Sprite BackgroundSprite;
        public static Render.Sprite LuminitySprite;
        public static Render.Sprite OpacitySprite;
        public static Render.Rectangle PreviewRectangle;

        public static Bitmap LuminityBitmap;
        public static Bitmap OpacityBitmap;

        public static CPSlider LuminositySlider;
        public static CPSlider AlphaSlider;

        private static Vector2 _prevPos;
        private static bool _visible;

        private static HSLColor SColor = new HSLColor(255, 255, 255);
        private static Color InitialColor;
        private static double SHue;
        private static double SSaturation;

        private static int LastBitmapUpdate;
        private static int LastBitmapUpdate2;

        static ColorPicker()
        {
            LuminityBitmap = new Bitmap(9, 238);
            OpacityBitmap = new Bitmap(9, 238);

            UpdateLuminosityBitmap(Color.White, true);
            UpdateOpacityBitmap(Color.White, true);

            BackgroundSprite = (Render.Sprite) new Render.Sprite(Properties.Resources.CPForm, new Vector2(X, Y)).Add(1);

            LuminitySprite = (Render.Sprite) new Render.Sprite(LuminityBitmap, new Vector2(X + 285, Y + 40)).Add(0);
            OpacitySprite = (Render.Sprite) new Render.Sprite(OpacityBitmap, new Vector2(X + 349, Y + 40)).Add(0);

            PreviewRectangle =
                (Render.Rectangle)
                    new Render.Rectangle(X + 375, Y + 44, 54, 80, new ColorBGRA(255, 255, 255, 255)).Add(0);

            LuminositySlider = new CPSlider(285 - Properties.Resources.CPActiveSlider.Width / 3, 35, 248);
            AlphaSlider = new CPSlider(350 - Properties.Resources.CPActiveSlider.Width / 3, 35, 248);

            Game.OnWndProc += Game_OnWndProc;
        }

        public static int X
        {
            get { return _x; }
            set
            {
                var oldX = _x;
                _x = value;
                BackgroundSprite.X += value - oldX;
                LuminitySprite.X += value - oldX;
                OpacitySprite.X += value - oldX;
                PreviewRectangle.X += value - oldX;
                LuminositySlider.sX += value - oldX;
                AlphaSlider.sX += value - oldX;
            }
        }

        public static int Y
        {
            get { return _y; }
            set
            {
                var oldY = _y;
                _y = value;
                BackgroundSprite.Y += value - oldY;
                LuminitySprite.Y += value - oldY;
                OpacitySprite.Y += value - oldY;
                PreviewRectangle.Y += value - oldY;
                LuminositySlider.sY += value - oldY;
                AlphaSlider.sY += value - oldY;
            }
        }

        public static bool Visible
        {
            get { return _visible; }
            set
            {
                LuminitySprite.Visible = value;
                OpacitySprite.Visible = value;
                BackgroundSprite.Visible = value;
                LuminositySlider.Visible = value;
                AlphaSlider.Visible = value;
                PreviewRectangle.Visible = value;
                _visible = value;
            }
        }

        public static int ColorPickerX
        {
            get { return X + 18; }
        }

        public static int ColorPickerY
        {
            get { return Y + 61; }
        }

        public static int ColorPickerW
        {
            get { return 252 - 18; }
        }

        public static int ColorPickerH
        {
            get { return 282 - 61; }
        }

        public static void Load(OnSelectColor onSelectcolor, Color color)
        {
            OnChangeColor = onSelectcolor;
            SColor = color;
            SHue = ((HSLColor) color).Hue;
            SSaturation = ((HSLColor) color).Saturation;

            LuminositySlider.Percent = (float) SColor.Luminosity / 100f;
            AlphaSlider.Percent = color.A / 255f;
            X = (Drawing.Width - BackgroundSprite.Width) / 2;
            Y = (Drawing.Height - BackgroundSprite.Height) / 2;

            Visible = true;
            UpdateLuminosityBitmap(color);
            UpdateOpacityBitmap(color);
            InitialColor = color;
        }

        private static void Close()
        {
            _selecting = false;
            _moving = false;
            AlphaSlider.Moving = false;
            LuminositySlider.Moving = false;
            AlphaSlider.Visible = false;
            LuminositySlider.Visible = false;
            Visible = false;
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (!_visible)
            {
                return;
            }

            LuminositySlider.OnWndProc(args);
            AlphaSlider.OnWndProc(args);

            if (args.Msg == (uint) WindowsMessages.WM_LBUTTONDOWN)
            {
                var pos = Utils.GetCursorPos();

                if (Utils.IsUnderRectangle(pos, X, Y, BackgroundSprite.Width, 25))
                {
                    _moving = true;
                }

                //Apply button:
                if (Utils.IsUnderRectangle(pos, X + 290, Y + 297, 74, 12))
                {
                    Close();
                    return;
                }

                //Cancel button:
                if (Utils.IsUnderRectangle(pos, X + 370, Y + 296, 73, 14))
                {
                    FireOnChangeColor(InitialColor);
                    Close();
                    return;
                }

                if (Utils.IsUnderRectangle(pos, ColorPickerX, ColorPickerY, ColorPickerW, ColorPickerH))
                {
                    UpdateColor();
                    _selecting = true;
                }
            }
            else if (args.Msg == (uint) WindowsMessages.WM_LBUTTONUP)
            {
                _moving = false;
                _selecting = false;
            }
            else if (args.Msg == (uint) WindowsMessages.WM_MOUSEMOVE)
            {
                if (_selecting)
                {
                    var pos = Utils.GetCursorPos();
                    if (Utils.IsUnderRectangle(pos, ColorPickerX, ColorPickerY, ColorPickerW, ColorPickerH))
                    {
                        UpdateColor();
                    }
                }

                if (_moving)
                {
                    var pos = Utils.GetCursorPos();
                    X += (int) (pos.X - _prevPos.X);
                    Y += (int) (pos.Y - _prevPos.Y);
                }
                _prevPos = Utils.GetCursorPos();
            }
        }

        private static void UpdateLuminosityBitmap(HSLColor color, bool force = false)
        {
            if (Environment.TickCount - LastBitmapUpdate < 100 && !force)
            {
                return;
            }
            LastBitmapUpdate = Environment.TickCount;

            color.Luminosity = 0d;
            for (var y = 0; y < LuminityBitmap.Height; y++)
            {
                for (var x = 0; x < LuminityBitmap.Width; x++)
                {
                    LuminityBitmap.SetPixel(x, y, color);
                }

                color.Luminosity += 100d / LuminityBitmap.Height;
            }

            if (LuminitySprite != null)
            {
                LuminitySprite.UpdateTextureBitmap(LuminityBitmap);
            }
        }

        private static void UpdateOpacityBitmap(HSLColor color, bool force = false)
        {
            if (Environment.TickCount - LastBitmapUpdate2 < 100 && !force)
            {
                return;
            }
            LastBitmapUpdate2 = Environment.TickCount;

            color.Luminosity = 0d;
            for (var y = 0; y < OpacityBitmap.Height; y++)
            {
                for (var x = 0; x < OpacityBitmap.Width; x++)
                {
                    OpacityBitmap.SetPixel(x, y, color);
                }

                color.Luminosity += 40d / LuminityBitmap.Height;
            }

            if (OpacitySprite != null)
            {
                OpacitySprite.UpdateTextureBitmap(OpacityBitmap);
            }
        }

        private static void UpdateColor()
        {
            if (_selecting)
            {
                var pos = Utils.GetCursorPos();
                var color = BackgroundSprite.Bitmap.GetPixel((int) pos.X - X, (int) pos.Y - Y);
                SHue = ((HSLColor) color).Hue;
                SSaturation = ((HSLColor) color).Saturation;
                UpdateLuminosityBitmap(color);
            }

            SColor.Hue = SHue;
            SColor.Saturation = SSaturation;
            SColor.Luminosity = (LuminositySlider.Percent * 100d);
            var r = Color.FromArgb(((int) (AlphaSlider.Percent * 255)), SColor);
            PreviewRectangle.Color = new ColorBGRA(r.R, r.G, r.B, r.A);
            UpdateOpacityBitmap(r);
            FireOnChangeColor(r);
        }

        public static void FireOnChangeColor(Color color)
        {
            if (OnChangeColor != null)
            {
                OnChangeColor(color);
            }
        }

        //From: https://richnewman.wordpress.com/about/code-listings-and-diagrams/hslcolor-class/

        public class CPSlider
        {
            private readonly int _x;
            private readonly int _y;
            internal Render.Sprite ActiveSprite;
            public int Height;
            internal Render.Sprite InactiveSprite;
            public bool Moving = false;
            private float _percent;

            private bool _visible = true;

            public CPSlider(int x, int y, int height, float percent = 1)
            {
                _x = x;
                _y = y;
                Height = height - Properties.Resources.CPActiveSlider.Height;
                Percent = percent;
                ActiveSprite = new Render.Sprite(Properties.Resources.CPActiveSlider, new Vector2(sX, sY));
                InactiveSprite = new Render.Sprite(Properties.Resources.CPInactiveSlider, new Vector2(sX, sY));

                ActiveSprite.Add(2);
                InactiveSprite.Add(2);
            }

            public bool Visible
            {
                get { return _visible; }
                set
                {
                    ActiveSprite.Visible = value;
                    InactiveSprite.Visible = value;
                    _visible = value;
                }
            }

            public int sX
            {
                set
                {
                    ActiveSprite.X = sX;
                    InactiveSprite.X = sX;
                }
                get { return _x + X; }
            }

            public int sY
            {
                set
                {
                    ActiveSprite.Y = sY;
                    InactiveSprite.Y = sY;
                    ActiveSprite.Y = sY + (int) (Percent * Height);
                    InactiveSprite.Y = sY + (int) (Percent * Height);
                }
                get { return _y + Y; }
            }

            public int Width
            {
                get { return Properties.Resources.CPActiveSlider.Width; }
            }

            public float Percent
            {
                get { return _percent; }
                set
                {
                    var newValue = Math.Max(0f, Math.Min(1f, value));
                    _percent = newValue;
                }
            }

            private void UpdatePercent()
            {
                var pos = Utils.GetCursorPos();
                Percent = (pos.Y - Properties.Resources.CPActiveSlider.Height / 2 - sY) / Height;
                UpdateColor();
                ActiveSprite.Y = sY + (int) (Percent * Height);
                InactiveSprite.Y = sY + (int) (Percent * Height);
            }

            public void OnWndProc(WndEventArgs args)
            {
                switch (args.Msg)
                {
                    case (uint) WindowsMessages.WM_LBUTTONDOWN:
                        var pos = Utils.GetCursorPos();
                        if (Utils.IsUnderRectangle(
                            pos, sX, sY, Width, Height + Properties.Resources.CPActiveSlider.Height))
                        {
                            Moving = true;
                            ActiveSprite.Visible = Moving;
                            InactiveSprite.Visible = !Moving;
                            UpdatePercent();
                        }
                        break;
                    case (uint) WindowsMessages.WM_MOUSEMOVE:
                        if (Moving)
                        {
                            UpdatePercent();
                        }
                        break;
                    case (uint) WindowsMessages.WM_LBUTTONUP:
                        Moving = false;
                        ActiveSprite.Visible = Moving;
                        InactiveSprite.Visible = !Moving;
                        break;
                }
            }
        }

        public class HSLColor
        {
            //from: https://richnewman.wordpress.com/about/code-listings-and-diagrams/hslcolor-class/
            // Private data members below are on scale 0-1
            // They are scaled for use externally based on scale
            private const double scale = 100.0;
            private double hue = 1.0;
            private double luminosity = 1.0;
            private double saturation = 1.0;

            public HSLColor() { }

            public HSLColor(Color color)
            {
                SetRGB(color.R, color.G, color.B);
            }

            public HSLColor(int red, int green, int blue)
            {
                SetRGB(red, green, blue);
            }

            public HSLColor(double hue, double saturation, double luminosity)
            {
                Hue = hue;
                Saturation = saturation;
                Luminosity = luminosity;
            }

            public double Hue
            {
                get { return hue * scale; }
                set { hue = CheckRange(value / scale); }
            }

            public double Saturation
            {
                get { return saturation * scale; }
                set { saturation = CheckRange(value / scale); }
            }

            public double Luminosity
            {
                get { return luminosity * scale; }
                set { luminosity = CheckRange(value / scale); }
            }

            private double CheckRange(double value)
            {
                if (value < 0.0)
                {
                    value = 0.0;
                }
                else if (value > 1.0)
                {
                    value = 1.0;
                }
                return value;
            }

            public override string ToString()
            {
                return String.Format("H: {0:#0.##} S: {1:#0.##} L: {2:#0.##}", Hue, Saturation, Luminosity);
            }

            public string ToRGBString()
            {
                Color color = this;
                return String.Format("R: {0:#0.##} G: {1:#0.##} B: {2:#0.##}", color.R, color.G, color.B);
            }

            public void SetRGB(int red, int green, int blue)
            {
                HSLColor hslColor = Color.FromArgb(red, green, blue);
                hue = hslColor.hue;
                saturation = hslColor.saturation;
                luminosity = hslColor.luminosity;
            }

            #region Casts to/from System.Drawing.Color

            public static implicit operator Color(HSLColor hslColor)
            {
                double r = 0, g = 0, b = 0;
                if (hslColor.luminosity != 0)
                {
                    if (hslColor.saturation == 0)
                    {
                        r = g = b = hslColor.luminosity;
                    }
                    else
                    {
                        var temp2 = GetTemp2(hslColor);
                        var temp1 = 2.0 * hslColor.luminosity - temp2;

                        r = GetColorComponent(temp1, temp2, hslColor.hue + 1.0 / 3.0);
                        g = GetColorComponent(temp1, temp2, hslColor.hue);
                        b = GetColorComponent(temp1, temp2, hslColor.hue - 1.0 / 3.0);
                    }
                }
                return Color.FromArgb((int) (255 * r), (int) (255 * g), (int) (255 * b));
            }

            private static double GetColorComponent(double temp1, double temp2, double temp3)
            {
                temp3 = MoveIntoRange(temp3);
                if (temp3 < 1.0 / 6.0)
                {
                    return temp1 + (temp2 - temp1) * 6.0 * temp3;
                }
                if (temp3 < 0.5)
                {
                    return temp2;
                }
                if (temp3 < 2.0 / 3.0)
                {
                    return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
                }
                return temp1;
            }

            private static double MoveIntoRange(double temp3)
            {
                if (temp3 < 0.0)
                {
                    temp3 += 1.0;
                }
                else if (temp3 > 1.0)
                {
                    temp3 -= 1.0;
                }
                return temp3;
            }

            private static double GetTemp2(HSLColor hslColor)
            {
                double temp2;
                if (hslColor.luminosity < 0.5) //<=??
                {
                    temp2 = hslColor.luminosity * (1.0 + hslColor.saturation);
                }
                else
                {
                    temp2 = hslColor.luminosity + hslColor.saturation - (hslColor.luminosity * hslColor.saturation);
                }
                return temp2;
            }

            public static implicit operator HSLColor(Color color)
            {
                var hslColor = new HSLColor();
                hslColor.hue = color.GetHue() / 360.0; // we store hue as 0-1 as opposed to 0-360 
                hslColor.luminosity = color.GetBrightness();
                hslColor.saturation = color.GetSaturation();
                return hslColor;
            }

            #endregion
        }
    }
}