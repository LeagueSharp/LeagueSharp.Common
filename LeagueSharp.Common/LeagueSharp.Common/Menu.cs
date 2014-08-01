#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace LeagueSharp.Common
{
    internal enum WindowsMessages
    {
        WM_MOUSEMOVE = 0x200,
        WM_LBUTTONDOWN = 0x201,
        WM_LBUTTONUP = 0x202,
        WM_RBUTTONDOWN = 0x203,
        WM_RBUTTONUP = 0x202,
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x101,
    }

    [ Serializable ]
    public struct Circle
    {
        public bool Active;
        public Color Color;

        public Circle(bool enabled, Color color)
        {
            Active = enabled;
            Color = color;
        }
    }

    [ Serializable ]
    public struct Slider
    {
        public int MaxValue;
        public int MinValue;
        private int _value;

        public Slider(int value = 0, int maxValue = 100, int minValue = 0)
        {
            MaxValue = maxValue;
            _value = value;
            MinValue = minValue;
        }

        public int Value
        {
            get { return _value; }
            set { _value = Math.Min(Math.Max(value, MinValue), MaxValue); }
        }
    }

    [ Serializable ]
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

    [ Serializable ]
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

        public static XmlDocument xml = new XmlDocument();
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
                xml.Load(MenuSettingsPath);
            }
            else
            {
                var rootNode = xml.CreateElement("MenuSettings");
                xml.AppendChild(rootNode);

                var varNode = xml.CreateElement("Width");
                varNode.InnerText = "2";
                rootNode.AppendChild(varNode);

                varNode = xml.CreateElement("Height");
                varNode.InnerText = "25";
                rootNode.AppendChild(varNode);

                varNode = xml.CreateElement("BackgroundColor");
                varNode.InnerText = System.Drawing.ColorTranslator.ToHtml(Color.DarkSlateGray);
                rootNode.AppendChild(varNode);

                varNode = xml.CreateElement("BackgroundColorAlpha");
                varNode.InnerText = "100";
                rootNode.AppendChild(varNode);

                varNode = xml.CreateElement("ActiveColor");
                varNode.InnerText = System.Drawing.ColorTranslator.ToHtml(Color.Red);
                rootNode.AppendChild(varNode);

                varNode = xml.CreateElement("ActiveColorAlpha");
                varNode.InnerText = "150";
                rootNode.AppendChild(varNode);

                varNode = xml.CreateElement("ShowMenuPress");
                varNode.InnerText = "16";
                rootNode.AppendChild(varNode);

                varNode = xml.CreateElement("ShowMenuToggle");
                varNode.InnerText = "27";
                rootNode.AppendChild(varNode);
            }

            Directory.CreateDirectory(MenuConfigPath);

            xml.Save(MenuSettingsPath);

            Game.OnWndProc += Game_OnWndProc;
            _drawTheMenu = Global.Read<bool>("DrawMenu", true);
        }

        internal static bool DrawMenu
        {
            get { return _drawTheMenu; }
            set
            {
                Global.Write("DrawMenu", value);
                _drawTheMenu = value;
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
                if (_chachedBgColor != Color.Transparent) return _chachedBgColor;

                var color = System.Drawing.ColorTranslator.FromHtml(GetXmlValue("BackgroundColor"));
                var alpha = GetXmlValue("BackgroundColorAlpha");
                _chachedBgColor = Color.FromArgb(Convert.ToInt32(alpha), color);
                return _chachedBgColor;
            }
        }

        public static Color ActiveBackgroundColor
        {
            get
            {
                if (_chachedActiveColor != Color.Transparent) return _chachedActiveColor;

                var color = System.Drawing.ColorTranslator.FromHtml(GetXmlValue("ActiveColor"));
                var alpha = GetXmlValue("ActiveColorAlpha");
                _chachedActiveColor = Color.FromArgb(Convert.ToInt32(alpha), color);
                return _chachedActiveColor;
            }
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if ((args.Msg == (uint)WindowsMessages.WM_KEYUP || args.Msg == (uint)WindowsMessages.WM_KEYDOWN) &&
                args.WParam == Convert.ToInt32(GetXmlValue("ShowMenuPress")))
            {
                DrawMenu = args.Msg == (uint)WindowsMessages.WM_KEYDOWN;
            }

            if (args.Msg == (uint)WindowsMessages.WM_KEYUP &&
                args.WParam == Convert.ToInt32(GetXmlValue("ShowMenuToggle")))
                DrawMenu = !DrawMenu;
        }

        private static string GetXmlValue(string tagName)
        {
            return xml.GetElementsByTagName(tagName)[0].InnerText;
        }
    }

    internal static class MenuDrawHelper
    {
        internal static void DrawBox(Vector2 position, int width, int height, Color color, int borderwidth,
            Color borderColor)
        {
            Drawing.DrawLine(position.X, position.Y, position.X + width, position.Y, height, color);

            if (borderwidth > 0)
            {
                Drawing.DrawLine(position.X, position.Y, position.X + width, position.Y, borderwidth, borderColor);
                Drawing.DrawLine(position.X, position.Y + height, position.X + width, position.Y + height, borderwidth,
                    borderColor);
                Drawing.DrawLine(position.X, position.Y, position.X, position.Y + height, borderwidth, borderColor);
                Drawing.DrawLine(position.X + width, position.Y, position.X + width, position.Y + height, borderwidth,
                    borderColor);
            }
        }

        internal static void DrawOnOff(bool on, Vector2 position, MenuItem item)
        {
            DrawBox(position, item.Height, item.Height, on ? MenuSettings.BooleanOnColor : MenuSettings.BooleanOffColor,
                1, Color.Black);
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

        internal static void DrawSlider(Vector2 position, MenuItem item, int min, int max, int value, int width,
            bool drawText)
        {
            width = (width > 0 ? width : item.Width);
            var percentage = 100 * (value - min) / (max - min);
            var x = position.X + (percentage * width) / 100;
            Drawing.DrawLine(x, position.Y + 2, x, position.Y + item.Height, 2, MenuSettings.SliderIndicator);

            if (drawText)
                Drawing.DrawText(position.X - 7 + width - Drawing.GetTextExtent(value.ToString()).Width,
                    position.Y + (item.Height - Drawing.GetTextExtent(value.ToString()).Height) / 2, Color.White,
                    value.ToString());
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
                        m = new List<string>();
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
                if (IsRootMenu || Parent == null) return 0;
                var result = 0;
                foreach (var test in Parent.Children)
                {
                    if (test.Name == Name)
                        break;
                    result++;
                }
                return result;
            }
        }

        internal int MenuCount
        {
            get
            {
                if (Environment.TickCount - _cachedMenuCountT < 500) return _cachedMenuCount;
                var l = Global.Read<List<string>>("CommonMenuList");
                var result = 0;
                foreach (var s in l)
                {
                    if (s == DisplayName + Name)
                        break;
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
                    return MenuSettings.BasePosition + MenuCount * new Vector2(0, MenuSettings.MenuItemHeight);

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
                if (!MenuSettings.DrawMenu) return false;
                return IsRootMenu ? true : _visible;
            }
            set { _visible = value; }
        }

        internal bool IsInside(Vector2 position)
        {
            return Utility.IsUnderRectangle(position, Position.X, Position.Y, Width, Height);
        }

        internal void Game_OnWndProc(WndEventArgs args)
        {
            OnReceiveMessage((WindowsMessages)args.Msg, Utility.GetCursorPos(), args.WParam);
        }

        internal void OnReceiveMessage(WindowsMessages message, Vector2 cursorPos, uint key)
        {
            //Spread the message to the menu's children recursively
            foreach (var child in Children)
                child.OnReceiveMessage(message, cursorPos, key);


            foreach (var item in Items)
                item.OnReceiveMessage(message, cursorPos, key);

            //Handle the left clicks on the menus to hide or show the submenus.
            if (message != WindowsMessages.WM_LBUTTONDOWN) return;

            if (IsRootMenu && Visible)
            {
                if (cursorPos.X - MenuSettings.BasePosition.X < MenuSettings.MenuItemWidth)
                {
                    var n = (int)(cursorPos.Y - MenuSettings.BasePosition.Y) / MenuSettings.MenuItemHeight;
                    if (MenuCount != n && n < Global.Read<List<string>>("CommonMenuList").Count)
                    {
                        foreach (var schild in Children)
                            schild.Visible = false;

                        foreach (var sitem in Items)
                            sitem.Visible = false;
                    }
                }
            }

            if (!IsInside(cursorPos)) return;
            if (!Visible) return;

            if (!IsRootMenu && Parent != null)
                //Close all the submenus in the level 
                foreach (var child in Parent.Children)
                {
                    if (child.Name != Name)
                    {
                        foreach (var schild in child.Children)
                            schild.Visible = false;

                        foreach (var sitem in child.Items)
                            sitem.Visible = false;
                    }
                }

            //Hide or Show the submenus.
            foreach (var child in Children)
                child.Visible = !child.Visible;

            //Hide or Show the items.
            foreach (var item in Items)
                item.Visible = !item.Visible;
        }

        internal void Drawing_OnDraw(System.EventArgs args)
        {
            if (!Visible) return;

            MenuDrawHelper.DrawBox(Position, Width, Height,
                (Children.Count > 0 && Children[0].Visible || Items.Count > 0 && Items[0].Visible)
                    ? MenuSettings.ActiveBackgroundColor
                    : MenuSettings.BackgroundColor, 1, Color.Black);
            Drawing.DrawText(Position.X + 5, Position.Y + (Height - Drawing.GetTextExtent(DisplayName).Height) / 2,
                Color.White, DisplayName);

            Drawing.DrawText(Position.X + Width - 15,
                Position.Y + (Height - Drawing.GetTextExtent(DisplayName).Height) / 2,
                Color.White, ">");

            //Draw the menu submenus
            foreach (var child in Children)
                if (child.Visible)
                    child.Drawing_OnDraw(args);

            //Draw the items
            for (var i = Items.Count - 1; i >= 0; i--)
            {
                var item = Items[i];
                if (item.Visible)
                    item.Drawing_OnDraw();
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
                m = new List<string>();
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
                    return item;
            }

            //Search in submenus
            foreach (var subMenu in Children)
            {
                if (subMenu.Item(name) != null)
                    return subMenu.Item(name);
            }

            return null;
        }

        public Menu SubMenu(string name)
        {
            //Search in submenus
            foreach (var subMenu in Children)
            {
                if (subMenu.Name == name)
                    return subMenu;
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

    public class MenuItem
    {
        private readonly string _assemblyPath;
        internal int ColorId;
        public string DisplayName;
        internal bool Interacting;
        public string Name;

        public Menu Parent;
        internal MenuValueType ValueType;

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
                if (!MenuSettings.DrawMenu) return false;
                return _visible;
            }
            set { _visible = value; }
        }

        internal int XLevel
        {
            get
            {
                if (Parent == null) return 1;
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
                if (Parent == null) return 0;
                var result = Parent.YLevel + Parent.Children.Count;
                foreach (var test in Parent.Items)
                {
                    if (test.Name == Name)
                        break;
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
                    return MenuSettings.BasePosition;

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

        internal MenuItem SetShared()
        {
            _isShared = true;
            return this;
        }

        public T GetValue<T>()
        {
            return (T)_value;
        }

        public MenuItem SetValue<T>(T newValue)
        {
            ValueType = MenuValueType.None;
            if (newValue.GetType().ToString().Contains("Boolean"))
                ValueType = MenuValueType.Boolean;
            else if (newValue.GetType().ToString().Contains("Slider"))
                ValueType = MenuValueType.Slider;
            else if (newValue.GetType().ToString().Contains("KeyBind"))
                ValueType = MenuValueType.KeyBind;
            else if (newValue.GetType().ToString().Contains("Integer"))
                ValueType = MenuValueType.Integer;
            else if (newValue.GetType().ToString().Contains("Circle"))
                ValueType = MenuValueType.Circle;
            else if (newValue.GetType().ToString().Contains("StringList"))
                ValueType = MenuValueType.StringList;
            else if (newValue.GetType().ToString().Contains("Color"))
                ValueType = MenuValueType.Color;
            else
                Game.PrintChat("CommonLibMenu: Data type not supported");

            var dname = (_isShared ? "SharedConfig" : Path.GetFileName(_assemblyPath));
            Directory.CreateDirectory(MenuSettings.MenuConfigPath);
            Directory.CreateDirectory(MenuSettings.MenuConfigPath + dname);

            _saveFilePath = MenuSettings.MenuConfigPath + dname + "\\" +
                            Utility.Md5Hash("v2" + DisplayName + Name + newValue.GetType());

            if (!_valueSet && File.Exists(_saveFilePath))
            {
                if (ValueType == MenuValueType.KeyBind)
                {
                    var SavedVal = (KeyBind)(object)Global.Deserialize<T>(File.ReadAllBytes(_saveFilePath));
                    if (SavedVal.Type == KeyBindType.Press)
                        SavedVal.Active = false;
                    newValue = (T)(object)SavedVal;
                }
                else
                    newValue = Global.Deserialize<T>(File.ReadAllBytes(_saveFilePath));
            }

            _valueSet = true;

            _value = newValue;
            _serialized = Global.Serialize(_value);
            return this;
        }

        internal void SaveToFile()
        {
            if (!_saved && _saveFilePath != null)
            {
                File.WriteAllBytes(_saveFilePath, _serialized);
                _saved = true;
            }
        }

        internal bool IsInside(Vector2 position)
        {
            return Utility.IsUnderRectangle(position, Position.X, Position.Y, Width, Height);
        }

        internal void OnReceiveMessage(WindowsMessages message, Vector2 cursorPos, uint key)
        {
            switch (ValueType)
            {
                case MenuValueType.Boolean:

                    if (message != WindowsMessages.WM_LBUTTONDOWN) return;
                    if (!IsInside(cursorPos)) return;
                    if (!Visible) return;

                    if (cursorPos.X > Position.X + Width - Height)
                        SetValue(!GetValue<bool>());

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
                        val.Value = (int)t;
                        SetValue(val);
                    }

                    if (message != WindowsMessages.WM_LBUTTONDOWN && message != WindowsMessages.WM_LBUTTONUP) return;
                    if (!Visible) return;
                    if (!IsInside(cursorPos) && message == WindowsMessages.WM_LBUTTONDOWN) return;

                    Interacting = message == WindowsMessages.WM_LBUTTONDOWN;
                    break;

                case MenuValueType.Color:
                    if (!Visible)
                    {
                        Interacting = false;
                        return;
                    }

                    if (message == WindowsMessages.WM_MOUSEMOVE && Interacting ||
                        message == WindowsMessages.WM_LBUTTONDOWN && !Interacting && IsInside(cursorPos) &&
                        cursorPos.X - Position.X < Width - Height)
                    {
                        var val = GetValue<Color>();
                        var t = 1 + ((cursorPos.X - Position.X) * (254 - 1)) / (Width - Height);
                        t = Math.Max(Math.Min(t, 255), 0);
                        SetValue(Color.FromArgb((int)t, val));
                    }

                    if (message != WindowsMessages.WM_LBUTTONDOWN && message != WindowsMessages.WM_LBUTTONUP) return;
                    if (!Visible) return;
                    if (!IsInside(cursorPos) && message == WindowsMessages.WM_LBUTTONDOWN) return;
                    if (cursorPos.X - Position.X > Width - Height)
                    {
                        if (message == WindowsMessages.WM_LBUTTONDOWN)
                        {
                            var val = GetValue<Color>();
                            ColorId = (ColorId != MenuSettings.ColorList.Count - 1) ? ColorId + 1 : 0;
                            SetValue(Color.FromArgb(val.A, MenuSettings.ColorList[ColorId]));
                        }
                        Interacting = false;
                        return;
                    }

                    Interacting = message == WindowsMessages.WM_LBUTTONDOWN;
                    break;
                case MenuValueType.Circle:
                    if (!Visible)
                    {
                        Interacting = false;
                        return;
                    }

                    if (message == WindowsMessages.WM_MOUSEMOVE && Interacting ||
                        message == WindowsMessages.WM_LBUTTONDOWN && !Interacting && IsInside(cursorPos) &&
                        cursorPos.X - Position.X < Width - 2 * Height)
                    {
                        var val = GetValue<Circle>();
                        var t = 1 + ((cursorPos.X - Position.X) * (254 - 1)) / (Width - Height * 2);
                        t = Math.Max(Math.Min(t, 255), 0);
                        val.Color = Color.FromArgb((int)t, val.Color);
                        SetValue(val);
                    }

                    if (message != WindowsMessages.WM_LBUTTONDOWN && message != WindowsMessages.WM_LBUTTONUP) return;
                    if (!Visible) return;
                    if (!IsInside(cursorPos) && message == WindowsMessages.WM_LBUTTONDOWN) return;
                    if (cursorPos.X - Position.X > Width - Height * 2)
                    {
                        if (message == WindowsMessages.WM_LBUTTONDOWN)
                            if (cursorPos.X - Position.X > Width - Height)
                            {
                                var val = GetValue<Circle>();
                                val.Active = !val.Active;
                                SetValue(val);
                            }
                            else
                            {
                                var val = GetValue<Circle>();
                                ColorId = (ColorId != MenuSettings.ColorList.Count - 1) ? ColorId + 1 : 0;
                                val.Color = Color.FromArgb(val.Color.A, MenuSettings.ColorList[ColorId]);
                                SetValue(val);
                            }
                        Interacting = false;
                        return;
                    }

                    Interacting = message == WindowsMessages.WM_LBUTTONDOWN;
                    break;
                case MenuValueType.KeyBind:

                    if (!MenuGUI.IsChatOpen)
                        switch (message)
                        {
                            case WindowsMessages.WM_KEYDOWN:
                                var val = GetValue<KeyBind>();
                                if (key == val.Key)
                                    if (val.Type == KeyBindType.Press)
                                    {
                                        if (!val.Active)
                                        {
                                            val.Active = true;
                                            SetValue(val);
                                        }
                                    }
                                break;
                            case WindowsMessages.WM_KEYUP:

                                var val2 = GetValue<KeyBind>();
                                if (key == val2.Key)
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
                                break;
                        }

                    if (!Visible) return;

                    if (message == WindowsMessages.WM_KEYUP && Interacting)
                    {
                        var val = GetValue<KeyBind>();
                        val.Key = key;
                        SetValue(val);
                        Interacting = false;
                    }

                    if (message != WindowsMessages.WM_LBUTTONDOWN) return;
                    if (!IsInside(cursorPos)) return;
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
                    if (!Visible) return;
                    if (message != WindowsMessages.WM_LBUTTONDOWN) return;
                    if (!IsInside(cursorPos)) return;

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
                    MenuDrawHelper.DrawOnOff(GetValue<bool>(), new Vector2(Position.X + Width - Height, Position.Y),
                        this);
                    break;

                case MenuValueType.Slider:
                    MenuDrawHelper.DrawSlider(Position, this);
                    break;

                case MenuValueType.KeyBind:
                    var val = GetValue<KeyBind>();
                    s += " (" + Utility.KeyToText(val.Key) + ")";
                    if (Interacting)
                        s = "Press new key";
                    MenuDrawHelper.DrawOnOff(val.Active, new Vector2(Position.X + Width - Height, Position.Y), this);

                    break;

                case MenuValueType.Integer:
                    var intVal = GetValue<int>();
                    Drawing.DrawText(Position.X + Width - Drawing.GetTextExtent(intVal.ToString()).Width - 7,
                        Position.Y + (Height - Drawing.GetTextExtent(intVal.ToString()).Height) / 2, Color.White,
                        intVal.ToString());
                    break;

                case MenuValueType.Color:
                    var colorVal = GetValue<Color>();
                    MenuDrawHelper.DrawSlider(Position, this, 1, 254, colorVal.A, Width - Height, false);
                    MenuDrawHelper.DrawBox(Position + new Vector2(Width - Height, 0), Height, Height, colorVal, 1,
                        Color.Black);
                    break;

                case MenuValueType.Circle:
                    var circleVal = GetValue<Circle>();
                    MenuDrawHelper.DrawSlider(Position, this, 1, 254, circleVal.Color.A, Width - Height * 2, false);
                    MenuDrawHelper.DrawBox(Position + new Vector2(Width - Height * 2, 0), Height, Height,
                        circleVal.Color, 1,
                        Color.Black);
                    MenuDrawHelper.DrawOnOff(circleVal.Active, new Vector2(Position.X + Width - Height, Position.Y),
                        this);
                    break;

                case MenuValueType.StringList:
                    var slVal = GetValue<StringList>();

                    var t = slVal.SList[slVal.SelectedIndex];

                    MenuDrawHelper.DrawArrow("<", Position + new Vector2(Width - Height * 2, 0), this,
                        MenuSettings.StringListColor);
                    MenuDrawHelper.DrawArrow(">", Position + new Vector2(Width - Height, 0), this,
                        MenuSettings.StringListColor);

                    Drawing.DrawText(Position.X + Width - Drawing.GetTextExtent(t).Width - 2 * Height - 20,
                        Position.Y + (Height - Drawing.GetTextExtent(t).Height) / 2, Color.White, t);

                    break;
            }

            Drawing.DrawText(Position.X + 5, Position.Y + (Height - Drawing.GetTextExtent(s).Height) / 2, Color.White, s);
        }
    }
}