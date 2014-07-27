#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#endregion

namespace LeagueSharp.Common
{
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
        public int Value;

        public Slider(int value = 0, int maxValue = 100, int minValue = 0)
        {
            MaxValue = maxValue;
            Value = value;
            MinValue = minValue;
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

    [ Serializable ]
    internal static class MenuSettings
    {
        public static Menu Config;
        public static List<Color> ColorList = new List<Color>();
        public static Color BooleanOnColor = Color.FromArgb(100, Color.Green);
        public static Color BooleanOffColor = Color.FromArgb(100, Color.Red);

        /* Slider */
        public static Color SliderIndicator = Color.FromArgb(150, Color.Yellow);

        /*String Lists*/
        public static Color NextBColor = Color.FromArgb(100, Color.Blue);

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
                var m = 6;
                if (Config.Item("Width") != null)
                {
                    switch (Config.Item("Width").GetValue<StringList>().SelectedIndex)
                    {
                        case 0:
                            m = 9;
                            break;
                        case 1:
                            m = 7;
                            break;
                        case 2:
                            m = 6;
                            break;
                        case 3:
                            m = 5;
                            break;
                        case 4:
                            m = 4;
                            break;
                    }
                }

                return Drawing.Width / m;
            }
        }

        public static int MenuItemHeight
        {
            get
            {
                if (Config.Item("Height") != null)
                    return Config.Item("Height").GetValue<Slider>().Value;
                return 25;
            }
        }


        public static Color BackgroundColor
        {
            get
            {
                if (Config.Item("BackgroundColor") != null)
                    return Config.Item("BackgroundColor").GetValue<Color>();
                return Color.FromArgb(150, Color.DarkSlateGray);
            }
        }

        public static Color ActiveBackgroundColor
        {
            get
            {
                if (Config.Item("SelectedColor") != null)
                    return Config.Item("SelectedColor").GetValue<Color>();
                return Color.FromArgb(150, Color.Red);
            }
        }


        /* Booleans */
    }

    [ Serializable ]
    internal static class MenuHandler
    {
        private static bool _enabled;
        public static List<Menu> Menus = new List<Menu>();
        public static int LastTick = 0;
        public static int LastMMoveTick = 0;

        static MenuHandler()
        {
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnWndProc += GameOnOnWndProc;
            Game.OnGameUpdate += GameOnOnGameUpdate;

            CustomEvents.Game.OnGameEnd += Game_OnGameEnd;
            Game.OnGameEnd += GameOnOnGameEnd;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainOnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }


        public static bool Visible
        {
            get
            {
                if (MenuSettings.Config.Item("showMenu") != null &&
                    MenuSettings.Config.Item("showMenu").GetValue<KeyBind>().Active)
                    return true;

                if (MenuSettings.Config.Item("showMenu2") != null &&
                    MenuSettings.Config.Item("showMenu2").GetValue<KeyBind>().Active)
                    return true;

                return false;
            }
        }

        public static uint MenuKey
        {
            get
            {
                if (MenuSettings.Config.Item("showMenu") != null)
                    return MenuSettings.Config.Item("showMenu").GetValue<KeyBind>().Key;
                return 16;
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            OnExit();
        }


        private static void OnExit()
        {
            SaveItems();
            Game.PrintChat("Saving");
        }

        private static void Game_OnGameEnd(EventArgs args)
        {
            if (!_enabled) return;
            OnExit();
        }

        private static void GameOnOnGameEnd(GameEndEventArgs args)
        {
            if (!_enabled) return;
            OnExit();
        }

        private static void CurrentDomainOnDomainUnload(object sender, EventArgs eventArgs)
        {
            if (!_enabled) return;
            OnExit();
        }

        private static void GameOnOnGameUpdate(EventArgs args)
        {
            if ((Environment.TickCount - LastTick < 500))
                return;

            Menus = GetMenus();

            LastTick = Environment.TickCount;
        }

        public static List<Menu> GetMenus()
        {
            var result = new List<Menu>();

            for (var i = 0; i < 30; i++)
            {
                var menu = Global.Read<Menu>("MENU" + i, true);
                if (default(Menu) != menu)
                {
                    result.Add(menu);
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        private static void GameOnOnWndProc(WndEventArgs args)
        {
            if (!_enabled)
                return;

            //Cap the mouse move event since it causes lag.
            if (args.Msg == 0x200)
            {
                if (Environment.TickCount - LastMMoveTick < 100) return;
                LastMMoveTick = Environment.TickCount;
            }

            if (args.Msg == 0x0100 || args.Msg == 0x0101)
            {
                SendKeys(args.WParam, args.Msg == 0x0101);
            }

            /* WM_LBUTTONDOWN, WM_LBUTTONUP and MouseMove */
            if (args.Msg != 0x201 && args.Msg != 0x202 && args.Msg != 0x200)
                return;

            if (args.Msg != 0x202 && !Visible)
                return;

            int X = 10, Y = 10;
            SendClick(X, Y, args.Msg);
        }

        public static void SaveItems(List<Menu> menuList = null)
        {
            if (menuList == null)
                menuList = Menus;

            foreach (var menu in menuList)
            {
                foreach (var item in menu.Items)
                {
                    item.SaveToConfigFile();
                }
                SaveItems(menu.Children);
            }
        }

        public static void SendKeys(uint vKeyCode, bool up, List<Menu> menuList = null)
        {
            if (menuList == null)
                menuList = Menus;

            foreach (var menu in menuList)
            {
                foreach (var item in menu.Items)
                {
                    item.OnKeyMessage(vKeyCode, up);
                }

                SendKeys(vKeyCode, up, menu.Children);
            }
        }

        public static void SendClick(int X, int Y, uint message, List<Menu> menuList = null)
        {
            if (menuList == null)
                menuList = Menus;

            var cursorPosition = Utility.GetCursorPos();

            /*Draw all the root menus*/
            for (var i = 0; i < menuList.Count; i++)
            {
                var menu = menuList[i];

                if (Utility.IsUnderRectangle(cursorPosition, X, Y, MenuSettings.MenuItemWidth,
                    MenuSettings.MenuItemHeight))
                {
                    if (message == 0x201)
                    {
                        menu.Open = !menu.Open;

                        LastTick = Environment.TickCount + 10000;

                        if (menu.Open)
                        {
                            /*Close the rest of the in the same level*/
                            foreach (var test in menuList)
                            {
                                if (test.DisplayName != menu.DisplayName && test.Name != menu.Name)
                                {
                                    if (test.Open)
                                        test.Open = false;
                                }
                            }
                        }
                    }
                }

                if (menu.Open)
                {
                    /*Send the click messages to the submenus */
                    SendClick(X + MenuSettings.MenuItemWidth, Y, message, menu.Children);

                    /*Send the click messages to the items */

                    /* Draw the items */
                    for (var j = 0; j < menu.Items.Count; j++)
                    {
                        var item = menu.Items[j];


                        var itemX = X + MenuSettings.MenuItemWidth;
                        var itemY = Y + (j + menu.Children.Count) * MenuSettings.MenuItemHeight;
                        if (
                            Utility.IsUnderRectangle(cursorPosition, itemX, itemY, MenuSettings.MenuItemWidth,
                                MenuSettings.MenuItemHeight) || message != 0x201)
                        {
                            if (message == 0x201 || message == 0x202)
                            {
                                LastTick = Environment.TickCount + 10000;
                                item.OnClick(cursorPosition.X - itemX, cursorPosition.Y - itemY, message == 0x202);
                            }


                            if (message == 0x200)
                                item.OnMoveMouse(cursorPosition.X - itemX, cursorPosition.Y - itemY);
                        }
                    }
                }

                Y += MenuSettings.MenuItemHeight;
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (!_enabled) return;
            if (!Visible) return;

            int X = 10, Y = 10;

            DrawMenus(X, Y);
        }

        private static void DrawMenus(int X, int Y, List<Menu> menuList = null)
        {
            if (menuList == null)
                menuList = Menus;

            /*Draw all the root menus*/
            for (var i = 0; i < menuList.Count; i++)
            {
                var menu = menuList[i];
                DrawRectangle(X, Y, MenuSettings.MenuItemWidth, MenuSettings.MenuItemHeight,
                    !menu.Open ? MenuSettings.BackgroundColor : MenuSettings.ActiveBackgroundColor);
                menu.Draw(X, Y);

                /*Draw the opened menus*/
                if (menu.Open)
                {
                    DrawMenus(X + MenuSettings.MenuItemWidth, Y, menu.Children);

                    /* Draw the items */
                    for (var j = 0; j < menu.Items.Count; j++)
                    {
                        var item = menu.Items[j];
                        DrawRectangle(X + MenuSettings.MenuItemWidth,
                            Y + (j + menu.Children.Count) * MenuSettings.MenuItemHeight, MenuSettings.MenuItemWidth,
                            MenuSettings.MenuItemHeight, MenuSettings.BackgroundColor);

                        item.Draw(X + MenuSettings.MenuItemWidth,
                            Y + (j + menu.Children.Count) * MenuSettings.MenuItemHeight);
                    }
                }

                Y += MenuSettings.MenuItemHeight;
            }
        }

        public static void DrawRectangle(float X, float Y, float width, float height, Color color, float thickness = 1)
        {
            /* Background */
            Drawing.DrawLine(X, Y, X + width, Y, height, color);

            /* Draw the borders */
            Drawing.DrawLine(X, Y, X + width, Y, thickness, Color.Black);
            Drawing.DrawLine(X, Y + height, X + width, Y + height, thickness, Color.Black);

            Drawing.DrawLine(X, Y, X, Y + height, thickness, Color.Black);
            Drawing.DrawLine(X + width, Y, X + width, Y + height, thickness, Color.Black);
        }

        public static void Enable()
        {
            _enabled = true;
        }
    }

    [ Serializable ]
    public class Menu
    {
        public List<Menu> Children = new List<Menu>();
        public string DisplayName;

        internal bool IsRootMenu;

        public List<MenuItem> Items = new List<MenuItem>();
        public string Name;
        public Menu Parent;

        private int _id = -1;

        public Menu(string displayName, string name, bool isRootMenu = false)
        {
            Name = name;
            DisplayName = displayName;
            IsRootMenu = isRootMenu;

            if (MenuSettings.Config == null && name != "Menu" && Global.Read<Menu>("MENU0", true) == default(Menu))
            {
                MenuSettings.Config = new Menu("Menu", "Menu", true);

                var colors = new Menu("Colors", "Colors");

                colors.AddItem(new MenuItem("BackgroundColor", "Background Color").SetValue(MenuSettings.BackgroundColor));
                colors.AddItem(
                    new MenuItem("SelectedColor", "Selected Color").SetValue(MenuSettings.ActiveBackgroundColor));
                MenuSettings.Config.AddSubMenu(colors);

                var sizes = new Menu("Sizes", "Sizes");
                sizes.AddItem(
                    new MenuItem("Width", "Width").SetValue(new StringList(new[] { "1", "2", "3", "4", "5" }, 2)));
                sizes.AddItem(new MenuItem("Height", "Height").SetValue(new Slider(25, 100, 10)));
                sizes.AddItem(new MenuItem("TextSize", "Text Size").SetValue(new Slider(50, 100, 0)));
                MenuSettings.Config.AddSubMenu(sizes);

                var hotkeys = new Menu("Hotkeys", "Hotkeys");
                hotkeys.AddItem(
                    new MenuItem("showMenu2", "Show the menu (Toggle)").SetValue(new KeyBind(27, KeyBindType.Toggle)));
                hotkeys.AddItem(
                    new MenuItem("showMenu", "Show the menu (Hold)").SetValue(new KeyBind(16, KeyBindType.Press)));

                MenuSettings.Config.AddSubMenu(hotkeys);

                MenuSettings.Config.AddToMainMenu();
            }
        }

        internal bool Open { get; set; }

        internal int Id
        {
            get
            {
                if (_id != -1) return _id;

                for (var i = 0; i < 30; i++)
                {
                    var test = Global.Read<Menu>("MENU" + i, true);
                    if (default(Menu) == test)
                    {
                        _id = i;

                        if (i == 0)
                            MenuHandler.Enable();

                        return i;
                    }
                }

                return -1;
            }
        }

        internal void Draw(int X, int Y)
        {
            var v = (MenuSettings.MenuItemHeight - Drawing.GetTextExtent(DisplayName).Height) / 2;
            Drawing.DrawText(X + 5, Y + v, Color.White, DisplayName);
        }

        public void AddToMainMenu()
        {
            if (IsRootMenu)
            {
                Global.Write("MENU" + Id, this);
            }

            else if (Parent != null)
            {
                Parent.AddToMainMenu();
            }
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

    [ Serializable ]
    public class MenuItem
    {
        private readonly string _assemblyPath;
        public string DisplayName;
        public string Name;

        public Menu Parent;

        internal string ValueType = "null";
        private bool _appendAssemblyPathToId = true;

        private int _colorId;
        private string _configFilePath;
        internal string _id;
        private bool _interacting;
        private byte[] _serialized;

        private bool _valueSet;

        private object lastReadValue;
        private int lastReadValueT;

        public MenuItem(string name, string displayName)
        {
            DisplayName = displayName;
            Name = name;
            _assemblyPath = System.Reflection.Assembly.GetCallingAssembly().Location;
        }

        public string Id
        {
            get
            {
                if (_id != null) return Id;

                return (_appendAssemblyPathToId ? _assemblyPath : "") + DisplayName + Name;
            }
            set { _id = value; }
        }

        internal MenuItem DontAppendAP()
        {
            _appendAssemblyPathToId = false;
            return this;
        }

        internal void OnClick(float X, float Y, bool up)
        {
            if (ValueType == "Boolean" && up == false && X > MenuSettings.MenuItemWidth - MenuSettings.MenuItemHeight)
            {
                SetValue(!GetValue<bool>());
            }

            if (ValueType == "Slider")
            {
                _interacting = !up;
                OnMoveMouse(X, Y);
            }

            if (ValueType == "Color")
            {
                _interacting = false;
                if (X < MenuSettings.MenuItemWidth - MenuSettings.MenuItemHeight)
                {
                    _interacting = !up;
                }
                else if (!up)
                {
                    var val = GetValue<Color>();
                    _colorId = (_colorId != MenuSettings.ColorList.Count - 1) ? _colorId + 1 : 0;
                    SetValue(Color.FromArgb(val.A, MenuSettings.ColorList[_colorId]));
                }

                OnMoveMouse(X, Y);
            }

            if (ValueType == "Circle")
            {
                _interacting = false;
                if (X < MenuSettings.MenuItemWidth - 2 * MenuSettings.MenuItemHeight)
                {
                    _interacting = !up;
                }
                else if (X < MenuSettings.MenuItemWidth - MenuSettings.MenuItemHeight && !up)
                {
                    var val = GetValue<Circle>();
                    var newVal = new Circle(!val.Active, val.Color);
                    SetValue(newVal);
                }
                else if (!up)
                {
                    var val = GetValue<Circle>();
                    _colorId = (_colorId != MenuSettings.ColorList.Count - 1) ? _colorId + 1 : 0;
                    SetValue(new Circle(val.Active, Color.FromArgb(val.Color.A, MenuSettings.ColorList[_colorId])));
                }

                OnMoveMouse(X, Y);
            }

            if (ValueType == "KeyBind" && !up)
            {
                if (X > MenuSettings.MenuItemWidth - MenuSettings.MenuItemHeight)
                {
                    var val = GetValue<KeyBind>();
                    val.Active = !val.Active;
                    SetValue(val);
                }
                else
                {
                    _interacting = !_interacting;
                }
            }

            if (ValueType == "StringList" && up == false)
            {
                if (X > MenuSettings.MenuItemWidth - MenuSettings.MenuItemHeight)
                {
                    var val = GetValue<StringList>();
                    val.SelectedIndex = val.SelectedIndex == val.SList.Length - 1 ? 0 : (val.SelectedIndex + 1);
                    SetValue(val);
                }
                else if (X > MenuSettings.MenuItemWidth - 2 * MenuSettings.MenuItemHeight)
                {
                    var val = GetValue<StringList>();
                    val.SelectedIndex = val.SelectedIndex == 0 ? val.SList.Length - 1 : (val.SelectedIndex - 1);
                    SetValue(val);
                }
            }
        }

        internal void OnKeyMessage(uint vKeyCode, bool up)
        {
            if (ValueType == "KeyBind")
            {
                var val = GetValue<KeyBind>();

                if (MenuGUI.IsChatOpen)
                {
                    if (val.Type == KeyBindType.Press)
                    {
                        val.Active = false;
                        SetValue(val);
                    }

                    return;
                }

                if (val.Key == vKeyCode)
                {
                    switch (val.Type)
                    {
                        case KeyBindType.Press:
                            val.Active = !up;
                            break;
                        case KeyBindType.Toggle:
                            if (up) val.Active = !val.Active;
                            break;
                    }

                    SetValue(val);
                }

                if (_interacting && up)
                {
                    val.Key = vKeyCode;
                    SetValue(val);

                    _interacting = false;
                }
            }
        }

        internal void OnMoveMouse(float X, float Y)
        {
            if (ValueType == "Slider" && _interacting)
            {
                var val = GetValue<Slider>();
                var t = val.MinValue + (X * (val.MaxValue - val.MinValue)) / MenuSettings.MenuItemWidth;
                val.Value = (int)Math.Min(Math.Max(t, val.MinValue), val.MaxValue);
                SetValue(val);
            }

            if (ValueType == "Color")
            {
                if (_interacting)
                {
                    var val = GetValue<Color>();
                    var alpha = X * 255 / (MenuSettings.MenuItemWidth - MenuSettings.MenuItemHeight);

                    alpha = Math.Max(Math.Min(alpha, 255), 0);
                    var c = Color.FromArgb((int)alpha, val);

                    SetValue(c);
                }
            }

            if (ValueType == "Circle")
            {
                if (_interacting)
                {
                    var val = GetValue<Circle>();
                    var alpha = X * 255 / (MenuSettings.MenuItemWidth - MenuSettings.MenuItemHeight * 2);

                    alpha = Math.Max(Math.Min(alpha, 255), 0);
                    var c = Color.FromArgb((int)alpha, val.Color);

                    SetValue(new Circle(val.Active, c));
                }
            }
        }

        internal void Draw(int X, int Y)
        {
            var text = (_interacting && ValueType == "KeyBind") ? ("Press new key") : DisplayName;

            if (ValueType == "KeyBind")
            {
                var valu = GetValue<KeyBind>();

                var sKey = Utility.KeyToText(valu.Key);

                text += " (" + sKey + ")";
            }

            var v = (MenuSettings.MenuItemHeight - Drawing.GetTextExtent(DisplayName).Height) / 2;
            Drawing.DrawText(X + 5, Y + v, Color.White, text);


            if (ValueType == "Boolean")
            {
                var val = GetValue<bool>();
                MenuHandler.DrawRectangle(X + MenuSettings.MenuItemWidth - MenuSettings.MenuItemHeight, Y,
                    MenuSettings.MenuItemHeight, MenuSettings.MenuItemHeight,
                    val ? MenuSettings.BooleanOnColor : MenuSettings.BooleanOffColor);
                Drawing.DrawText(X + MenuSettings.MenuItemWidth - 7 - Drawing.GetTextExtent(val ? "On" : "Off").Width,
                    Y + v, Color.White, val ? "On" : "Off");
            }

            if (ValueType == "Integer")
            {
                var val = GetValue<Int32>();
                Drawing.DrawText(X + MenuSettings.MenuItemWidth - 7 - Drawing.GetTextExtent(val.ToString()).Width, Y + v,
                    Color.White, val.ToString());
            }

            if (ValueType == "Slider")
            {
                var val = GetValue<Slider>();
                Drawing.DrawText(
                    X + MenuSettings.MenuItemWidth - 7 - Drawing.GetTextExtent(val.Value.ToString()).Width, Y + v,
                    Color.White, val.Value.ToString());

                var t = (MenuSettings.MenuItemWidth - 7) * (val.Value - val.MinValue) / (val.MaxValue - val.MinValue);

                Drawing.DrawLine(X + 5 + t, Y + 2, X + 5 + t, Y + MenuSettings.MenuItemHeight - 1, 3,
                    MenuSettings.SliderIndicator);
            }


            if (ValueType == "KeyBind")
            {
                var val = GetValue<KeyBind>();
                MenuHandler.DrawRectangle(X + MenuSettings.MenuItemWidth - MenuSettings.MenuItemHeight, Y,
                    MenuSettings.MenuItemHeight, MenuSettings.MenuItemHeight,
                    val.Active ? MenuSettings.BooleanOnColor : MenuSettings.BooleanOffColor);
                Drawing.DrawText(
                    X + MenuSettings.MenuItemWidth - 7 - Drawing.GetTextExtent(val.Active ? "On" : "Off").Width, Y + v,
                    Color.White, val.Active ? "On" : "Off");
            }

            if (ValueType == "Color")
            {
                var val = GetValue<Color>();

                var t = (MenuSettings.MenuItemWidth - MenuSettings.MenuItemHeight - 7) * (val.A) / (255);
                Drawing.DrawLine(X + 5 + t, Y + 2, X + 5 + t, Y + MenuSettings.MenuItemHeight - 1, 3,
                    MenuSettings.SliderIndicator);


                MenuHandler.DrawRectangle(X + MenuSettings.MenuItemWidth - MenuSettings.MenuItemHeight, Y,
                    MenuSettings.MenuItemHeight, MenuSettings.MenuItemHeight, val);
            }

            if (ValueType == "Circle")
            {
                var val = GetValue<Circle>();

                var t = (MenuSettings.MenuItemWidth - 2 * MenuSettings.MenuItemHeight - 7) * (val.Color.A) / (255);
                Drawing.DrawLine(X + 5 + t, Y + 2, X + 5 + t, Y + MenuSettings.MenuItemHeight - 1, 3,
                    MenuSettings.SliderIndicator);

                MenuHandler.DrawRectangle(X + MenuSettings.MenuItemWidth - 2 * MenuSettings.MenuItemHeight, Y,
                    MenuSettings.MenuItemHeight, MenuSettings.MenuItemHeight,
                    val.Active ? MenuSettings.BooleanOnColor : MenuSettings.BooleanOffColor);
                Drawing.DrawText(
                    X + MenuSettings.MenuItemWidth - MenuSettings.MenuItemHeight - 7 -
                    Drawing.GetTextExtent(val.Active ? "On" : "Off").Width,
                    Y + v, Color.White, val.Active ? "On" : "Off");

                MenuHandler.DrawRectangle(X + MenuSettings.MenuItemWidth - MenuSettings.MenuItemHeight, Y,
                    MenuSettings.MenuItemHeight, MenuSettings.MenuItemHeight, val.Color);
            }


            if (ValueType == "StringList")
            {
                var val = GetValue<StringList>();
                var s = val.SList[val.SelectedIndex];
                Drawing.DrawText(
                    X + MenuSettings.MenuItemWidth - Drawing.GetTextExtent(s).Width - 7 -
                    2 * MenuSettings.MenuItemHeight,
                    Y + v, Color.White, s);

                MenuHandler.DrawRectangle(X + MenuSettings.MenuItemWidth - MenuSettings.MenuItemHeight * 2, Y,
                    MenuSettings.MenuItemHeight, MenuSettings.MenuItemHeight, MenuSettings.NextBColor);
                Drawing.DrawText(
                    X + MenuSettings.MenuItemWidth - 9 - Drawing.GetTextExtent("<").Width - MenuSettings.MenuItemHeight,
                    Y + v, Color.White, "<");

                MenuHandler.DrawRectangle(X + MenuSettings.MenuItemWidth - MenuSettings.MenuItemHeight, Y,
                    MenuSettings.MenuItemHeight, MenuSettings.MenuItemHeight, MenuSettings.NextBColor);
                Drawing.DrawText(X + MenuSettings.MenuItemWidth - 9 - Drawing.GetTextExtent(">").Width, Y + v,
                    Color.White, ">");
            }
        }

        public T GetValue<T>()
        {
            if (Environment.TickCount - lastReadValueT < 75) return (T)lastReadValue;

            var val = Global.Read<T>(Id, true);
            lastReadValue = val;
            lastReadValueT = Environment.TickCount;

            return val;
        }

        public MenuItem SetValue<T>(T newValue)
        {
            if (newValue.GetType().ToString().Contains("Boolean"))
            {
                ValueType = "Boolean";
            }

            if (newValue.GetType().ToString().Contains("Int32"))
            {
                ValueType = "Integer";
            }

            if (newValue.GetType().ToString().Contains("Slider"))
            {
                ValueType = "Slider";
            }

            if (newValue.GetType().ToString().Contains("KeyBind"))
            {
                ValueType = "KeyBind";
            }

            if (newValue.GetType().ToString().Contains("Color"))
            {
                ValueType = "Color";
            }

            if (newValue.GetType().ToString().Contains("StringList"))
            {
                ValueType = "StringList";
            }

            if (newValue.GetType().ToString().Contains("Circle"))
            {
                ValueType = "Circle";
            }

            /* Read the value from the saved configuration file.*/
            var dName = Path.GetFileName(_assemblyPath);

            Directory.CreateDirectory(MenuSettings.MenuConfigPath + dName);

            _configFilePath = MenuSettings.MenuConfigPath + dName + "\\" +
                              Utility.Md5Hash(DisplayName + Name + newValue.GetType());

            if (!_valueSet)
            {
                if (File.Exists(_configFilePath))
                {
                    var savedObject = File.ReadAllBytes(_configFilePath);

                    //Prevent saving the Pressed keys.
                    if (ValueType == "KeyBind")
                    {
                        var cValue = (KeyBind)(object)newValue;

                        if (cValue.Type != KeyBindType.Press)
                            newValue = Global.Deserialize<T>(savedObject);
                        else
                        {
                            var savedValue = (KeyBind)(object)Global.Deserialize<T>(savedObject);
                            cValue.Key = savedValue.Key;
                            newValue = (T)(object)cValue;
                        }
                    }
                    else
                    {
                        newValue = Global.Deserialize<T>(savedObject);
                    }
                }
            }

            if (typeof (T).IsSerializable)
                _serialized = Global.Serialize(newValue);

            _valueSet = true;

            Global.Write(Id, newValue);
            return this;
        }

        internal void SaveToConfigFile()
        {
            var savedBytes = new byte[1];

            if (File.Exists(_configFilePath))
                savedBytes = File.ReadAllBytes(_configFilePath);


            if (savedBytes != _serialized)
            {
                File.WriteAllBytes(_configFilePath, _serialized);
            }
        }
    }
}