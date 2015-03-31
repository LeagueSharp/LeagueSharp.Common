#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Menu.cs is part of LeagueSharp.Common.
 
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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using LeagueSharp.Common.Properties;
using SharpDX;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;
using Font = SharpDX.Direct3D9.Font;
using Rectangle = SharpDX.Rectangle;

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
        private int _value;
        public int MaxValue;
        public int MinValue;

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
        public int SelectedIndex;
        public string[] SList;

        public StringList(string[] sList, int defaultSelectedIndex = 0)
        {
            SList = sList;
            SelectedIndex = defaultSelectedIndex;
        }

        public string SelectedValue
        {
            get { return SList[SelectedIndex]; }
        }
    }

    public enum KeyBindType
    {
        Toggle,
        Press
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

    [Serializable]
    internal static class SavedSettings
    {
        public static Dictionary<string, Dictionary<string, byte[]>> LoadedFiles =
            new Dictionary<string, Dictionary<string, byte[]>>();

        public static byte[] GetSavedData(string name, string key)
        {
            Dictionary<string, byte[]> dic = null;

            dic = LoadedFiles.ContainsKey(name) ? LoadedFiles[name] : Load(name);

            if (dic == null)
            {
                return null;
            }
            return dic.ContainsKey(key) ? dic[key] : null;
        }

        public static Dictionary<string, byte[]> Load(string name)
        {
            try
            {
                var fileName = Path.Combine(MenuSettings.MenuConfigPath, name + ".bin");
                if (File.Exists(fileName))
                {
                    return Utils.Deserialize<Dictionary<string, byte[]>>(File.ReadAllBytes(fileName));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public static void Save(string name, Dictionary<string, byte[]> entries)
        {
            try
            {
                Directory.CreateDirectory(MenuSettings.MenuConfigPath);
                var fileName = Path.Combine(MenuSettings.MenuConfigPath, name + ".bin");
                File.WriteAllBytes(fileName, Utils.Serialize(entries));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    public static class MenuGlobals
    {
        public static bool DrawMenu = false;
        public static List<string> MenuState = new List<string>();
    }

    internal static class MenuSettings
    {
        public static Vector2 BasePosition = new Vector2(10, 10);
        private static bool _drawTheMenu;

        static MenuSettings()
        {
            Game.OnWndProc += Game_OnWndProc;
            _drawTheMenu = MenuGlobals.DrawMenu;
        }

        internal static bool DrawMenu
        {
            get { return _drawTheMenu; }
            set
            {
                _drawTheMenu = value;
                MenuGlobals.DrawMenu = value;
            }
        }

        public static string MenuConfigPath
        {
            get
            {
                return Path.Combine(Config.AppDataDirectory, "MenuConfig");
            }
        }

        public static int MenuItemWidth
        {
            get { return 160; }
        }

        public static int MenuItemHeight
        {
            get { return 30; }
        }

        public static Color BackgroundColor
        {
            get { return Color.FromArgb(200, Color.Black); }
        }

        public static Color ActiveBackgroundColor
        {
            get { return Color.DimGray; }
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if ((args.Msg == (uint) WindowsMessages.WM_KEYUP || args.Msg == (uint) WindowsMessages.WM_KEYDOWN) &&
                args.WParam == Config.ShowMenuPressKey)
            {
                DrawMenu = args.Msg == (uint) WindowsMessages.WM_KEYDOWN;
            }

            if (args.Msg == (uint) WindowsMessages.WM_KEYUP && args.WParam == Config.ShowMenuToggleKey)
            {
                DrawMenu = !DrawMenu;
            }
        }
    }

    internal static class MenuDrawHelper
    {
        internal static Font Font;

        static MenuDrawHelper()
        {
            Font = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Tahoma",
                    Height = 14,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Antialiased
                });

            Drawing.OnPreReset += Drawing_OnPreReset;
            Drawing.OnPostReset += DrawingOnOnPostReset;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainOnDomainUnload;
        }

        private static void CurrentDomainOnDomainUnload(object sender, EventArgs eventArgs)
        {
            if (Font != null)
            {
                Font.Dispose();
                Font = null;
            }
        }

        private static void DrawingOnOnPostReset(EventArgs args)
        {
            Font.OnResetDevice();
        }

        private static void Drawing_OnPreReset(EventArgs args)
        {
            Font.OnLostDevice();
        }

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
            DrawBox(position, item.Height, item.Height, on ? Color.Green : Color.Red, 1, Color.Black);
            var s = on ? "On" : "Off";
            Font.DrawText(
                null, s,
                new Rectangle(
                    (int) (item.Position.X + item.Width - item.Height), (int) item.Position.Y, item.Height, item.Height),
                FontDrawFlags.VerticalCenter | FontDrawFlags.Center, new ColorBGRA(255, 255, 255, 255));
        }

        internal static void DrawArrow(string s, Vector2 position, MenuItem item, Color color)
        {
            DrawBox(position, item.Height, item.Height, Color.Blue, 1, color);
            Font.DrawText(
                null, s, new Rectangle((int) (position.X), (int) item.Position.Y, item.Height, item.Height),
                FontDrawFlags.VerticalCenter | FontDrawFlags.Center, new ColorBGRA(255, 255, 255, 255));
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
            var x = position.X + 3 + (percentage * (width - 3)) / 100;
            Drawing.DrawLine(x, position.Y + 2, x, position.Y + item.Height, 2, Color.Yellow);

            if (drawText)
            {
                Font.DrawText(
                    null, value.ToString(),
                    new Rectangle((int) position.X - 5, (int) position.Y, item.Width, item.Height),
                    FontDrawFlags.VerticalCenter | FontDrawFlags.Right, new ColorBGRA(255, 255, 255, 255));
            }
        }
    }

    public class Menu
    {
        private int _cachedMenuCount = 2;
        private int _cachedMenuCountT;
        private bool _visible;
        public List<Menu> Children = new List<Menu>();
        public string DisplayName;
        public bool IsRootMenu;
        public List<MenuItem> Items = new List<MenuItem>();
        public string Name;
        public Menu Parent;
        private string uniqueId;

        public Menu(string displayName, string name, bool isRootMenu = false)
        {
            DisplayName = displayName;
            Name = name;
            IsRootMenu = isRootMenu;

            if (isRootMenu)
            {
                CustomEvents.Game.OnGameEnd += delegate { SaveAll(); };
                Game.OnEnd += delegate { SaveAll(); };
                AppDomain.CurrentDomain.DomainUnload += delegate { SaveAll(); };
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

                return Parent.YLevel + Parent.Children.TakeWhile(test => test.Name != Name).Count();
            }
        }

        internal int MenuCount
        {
            get
            {
                if (Utils.TickCount - _cachedMenuCountT < 500)
                {
                    return _cachedMenuCount;
                }

                var globalMenuList = MenuGlobals.MenuState;
                var i = 0;
                var result = 0;

                foreach (var item in globalMenuList)
                {
                    if(item == uniqueId)
                    {
                        result = i;
                        break;
                    }
                    i++;
                }
                
                _cachedMenuCount = result;
                _cachedMenuCountT = Utils.TickCount;
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
                var xOffset = 0;

                if (Parent != null)
                {
                    xOffset = (int) (Parent.Position.X + Parent.Width);
                }
                else
                {
                    xOffset = (int) MyBasePosition.X;
                }

                return new Vector2(0, MyBasePosition.Y) + new Vector2(xOffset, 0) +
                       YLevel * new Vector2(0, MenuSettings.MenuItemHeight);
            }
        }

        internal int ChildrenMenuWidth
        {
            get
            {
                var result = Children.Select(item => item.NeededWidth).Concat(new[] { 0 }).Max();

                return Items.Select(item => item.NeededWidth).Concat(new[] { result }).Max();
            }
        }

        internal int Width
        {
            get { return Parent != null ? Parent.ChildrenMenuWidth : MenuSettings.MenuItemWidth; }
        }

        internal int NeededWidth
        {
            get
            {
                return MenuDrawHelper.Font.MeasureText(null, MultiLanguage._(DisplayName), FontDrawFlags.Left).Width + 25;
            }
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
                return IsRootMenu || _visible;
            }
            set
            {
                _visible = value;
                //Hide all the children
                if (!_visible)
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
                    if (MenuCount != n)
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

            if (!Visible)
            {
                return;
            }

            if (!IsInside(cursorPos))
            {
                return;
            }

            if (!IsRootMenu && Parent != null)
            {
                //Close all the submenus in the level 
                foreach (var child in Parent.Children.Where(child => child.Name != Name))
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

            Drawing.Direct3DDevice.SetRenderState(RenderState.AlphaBlendEnable, true);
            MenuDrawHelper.DrawBox(
                Position, Width, Height,
                (Children.Count > 0 && Children[0].Visible || Items.Count > 0 && Items[0].Visible)
                    ? MenuSettings.ActiveBackgroundColor
                    : MenuSettings.BackgroundColor, 1, Color.Black);

            MenuDrawHelper.Font.DrawText(
                null, MultiLanguage._(DisplayName), new Rectangle((int) Position.X + 5, (int) Position.Y, Width, Height),
                FontDrawFlags.VerticalCenter, new ColorBGRA(255, 255, 255, 255));
            MenuDrawHelper.Font.DrawText(
                null, ">", new Rectangle((int) Position.X - 5, (int) Position.Y, Width, Height),
                FontDrawFlags.Right | FontDrawFlags.VerticalCenter, new ColorBGRA(255, 255, 255, 255));

            //Draw the menu submenus
            foreach (var child in Children.Where(child => child.Visible))
            {
                child.Drawing_OnDraw(args);
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

        internal void RecursiveSaveAll(ref Dictionary<string, Dictionary<string, byte[]>> dics)
        {
            foreach (var child in Children)
            {
                child.RecursiveSaveAll(ref dics);
            }

            foreach (var item in Items)
            {
                item.SaveToFile(ref dics);
            }
        }

        internal void SaveAll()
        {
            var dic = new Dictionary<string, Dictionary<string, byte[]>>();
            RecursiveSaveAll(ref dic);


            foreach (var dictionary in dic)
            {
                var dicToSave = SavedSettings.Load(dictionary.Key) ?? new Dictionary<string, byte[]>();

                foreach (var entry in dictionary.Value)
                {
                    dicToSave[entry.Key] = entry.Value;
                }

                SavedSettings.Save(dictionary.Key, dicToSave);
            }
        }

        public void AddToMainMenu()
        {
            InitMenuState(Assembly.GetCallingAssembly().GetName().Name);

            AppDomain.CurrentDomain.DomainUnload += (sender, args) => UnloadMenuState();

            Drawing.OnEndScene += Drawing_OnDraw;
            Game.OnWndProc += Game_OnWndProc;
        }

        private void InitMenuState(string assemblyName)
        {
            List<string> globalMenuList;
            uniqueId = assemblyName + "." + Name;

            globalMenuList = MenuGlobals.MenuState;

            if (globalMenuList == null)
            {
                globalMenuList = new List<string>();
            }
            while (globalMenuList.Contains(uniqueId))
            {
                uniqueId += ".";
            }

            globalMenuList.Add(uniqueId);

            MenuGlobals.MenuState = globalMenuList;
        }

        private void UnloadMenuState()
        {
            var globalMenuList = MenuGlobals.MenuState;
            globalMenuList.Remove(uniqueId);
            MenuGlobals.MenuState = globalMenuList;
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

        public MenuItem Item(string name, bool makeChampionUniq = false)
        {
            if (makeChampionUniq)
            {
                name = ObjectManager.Player.ChampionName + name;
            }

            //Search in our own items
            foreach (var item in Items.Where(item => item.Name == name))
            {
                return item;
            }

            //Search in submenus
            return
                (from subMenu in Children where subMenu.Item(name) != null select subMenu.Item(name)).FirstOrDefault();
        }

        public Menu SubMenu(string name)
        {
            //Search in submenus and if it doesn't exist add it.
            var subMenu = Children.FirstOrDefault(sm => sm.Name == name);
            return subMenu ?? AddSubMenu(new Menu(name, name));
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
        StringList
    }

    public class OnValueChangeEventArgs
    {
        private readonly object _newValue;
        private readonly object _oldValue;

        public OnValueChangeEventArgs(object oldValue, object newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
            Process = true;
        }

        public bool Process { get; set; }

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
        private bool _dontSave;
        private bool _isShared;
        private byte[] _serialized;
        private object _value;
        private bool _valueSet;
        private bool _visible;
        private string _configName;
        public string DisplayName;
        internal bool Interacting;
        public string Name;
        public Menu Parent;
        internal MenuValueType ValueType;

        public MenuItem(string name, string displayName, bool makeChampionUniq = false)
        {
            if (makeChampionUniq)
            {
                name = ObjectManager.Player.ChampionName + name;
            }

            Name = name;
            DisplayName = displayName;
            _configName = Assembly.GetCallingAssembly().GetName().Name +
                          Assembly.GetCallingAssembly().GetType().GUID;
        }

        internal string SaveFileName
        {
            get { return (_isShared ? "SharedConfig" : _configName); }
        }

        internal string SaveKey
        {
            get { return Utils.Md5Hash("v3" + DisplayName + Name); }
        }

        internal bool Visible
        {
            get { return MenuSettings.DrawMenu && _visible; }
            set { _visible = value; }
        }

        internal int YLevel
        {
            get
            {
                if (Parent == null)
                {
                    return 0;
                }

                return Parent.YLevel + Parent.Children.Count + Parent.Items.TakeWhile(test => test.Name != Name).Count();
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
                var xOffset = 0;

                if (Parent != null)
                {
                    xOffset = (int) (Parent.Position.X + Parent.Width);
                }

                return new Vector2(0, MyBasePosition.Y) + new Vector2(xOffset, 0) +
                       YLevel * new Vector2(0, MenuSettings.MenuItemHeight);
            }
        }

        internal int Width
        {
            get { return Parent != null ? Parent.ChildrenMenuWidth : MenuSettings.MenuItemWidth; }
        }

        internal int NeededWidth
        {
            get
            {
                var extra = 0;

                if (ValueType == MenuValueType.StringList)
                {
                    var slVal = GetValue<StringList>();
                    var max =
                        slVal.SList.Select(v => MenuDrawHelper.Font.MeasureText(null, v, FontDrawFlags.Left).Width + 25)
                            .Concat(new[] { 0 })
                            .Max();

                    extra += max;
                }

                if (ValueType == MenuValueType.KeyBind)
                {
                    var val = GetValue<KeyBind>();
                    extra +=
                        MenuDrawHelper.Font.MeasureText(null, " (" + Utils.KeyToText(val.Key) + ")", FontDrawFlags.Left)
                            .Width;
                }

                return MenuDrawHelper.Font.MeasureText(null, MultiLanguage._(DisplayName), FontDrawFlags.Left).Width +
                       Height * 2 + 10 + extra;
            }
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

        public bool IsActive()
        {
            switch (ValueType)
            {
                case MenuValueType.Boolean:
                    return GetValue<bool>();
                case MenuValueType.Circle:
                    return GetValue<Circle>().Active;
                case MenuValueType.KeyBind:
                    return GetValue<KeyBind>().Active;
            }
            return false;
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

            var readBytes = SavedSettings.GetSavedData(SaveFileName, SaveKey);

            var v = newValue;
            try
            {
                if (!_valueSet && readBytes != null)
                {
                    switch (ValueType)
                    {
                        case MenuValueType.KeyBind:               
                            var savedKeyValue = (KeyBind)(object) Utils.Deserialize<T>(readBytes);
                            if (savedKeyValue.Type == KeyBindType.Press)
                            {
                                savedKeyValue.Active = false;
                            }
                            newValue = (T) (object) savedKeyValue;
                            break;

                        case MenuValueType.Circle:
                            var savedCircleValue = (Circle)(object) Utils.Deserialize<T>(readBytes);
                            var newCircleValue = (Circle) (object) newValue;
                            savedCircleValue.Radius = newCircleValue.Radius;
                            newValue = (T) (object) savedCircleValue;
                            break;

                        case MenuValueType.Slider:
                            var savedSliderValue = (Slider)(object) Utils.Deserialize<T>(readBytes);
                            var newSliderValue = (Slider) (object) newValue;
                            if (savedSliderValue.MinValue == newSliderValue.MinValue &&
                                savedSliderValue.MaxValue == newSliderValue.MaxValue)
                            {
                                newValue = (T) (object) savedSliderValue;
                            }
                            break;

                        case MenuValueType.StringList:
                            var savedListValue = (StringList)(object) Utils.Deserialize<T>(readBytes);
                            var newListValue = (StringList) (object) newValue;
                            if (savedListValue.SList.SequenceEqual(newListValue.SList))
                            {
                                newValue = (T) (object) savedListValue;
                            }
                            break;

                        default:
                            newValue = Utils.Deserialize<T>(readBytes);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                newValue = v;
                Console.WriteLine(e);
            }

            OnValueChangeEventArgs valueChangedEvent = null;

            if (_valueSet)
            {
                var handler = ValueChanged;
                if (handler != null)
                {
                    valueChangedEvent = new OnValueChangeEventArgs(_value, newValue);
                    handler(this, valueChangedEvent);
                }
            }

            if (valueChangedEvent != null)
            {
                if (valueChangedEvent.Process)
                {
                    _value = newValue;
                }
            }
            else
            {
                _value = newValue;
            }
            _valueSet = true;
            _serialized = Utils.Serialize(_value);
            return this;
        }

        internal void SaveToFile(ref Dictionary<string, Dictionary<string, byte[]>> dics)
        {
            if (!_dontSave)
            {
                if (!dics.ContainsKey(SaveFileName))
                {
                    dics[SaveFileName] = new Dictionary<string, byte[]>();
                }

                dics[SaveFileName][SaveKey] = _serialized;
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

                    if (!Visible)
                    {
                        return;
                    }

                    if (!IsInside(cursorPos))
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

                    if (!Visible)
                    {
                        return;
                    }

                    if (!IsInside(cursorPos))
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
                    else if (cursorPos.X - Position.X > Width - 2 * Height)
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
            var s = MultiLanguage._(DisplayName);

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
                        s = MultiLanguage._("Press new key");
                    }

                    MenuDrawHelper.DrawOnOff(val.Active, new Vector2(Position.X + Width - Height, Position.Y), this);

                    break;

                case MenuValueType.Integer:
                    var intVal = GetValue<int>();
                    MenuDrawHelper.Font.DrawText(
                        null, intVal.ToString(), new Rectangle((int) Position.X + 5, (int) Position.Y, Width, Height),
                        FontDrawFlags.VerticalCenter | FontDrawFlags.Right, new ColorBGRA(255, 255, 255, 255));
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

                    MenuDrawHelper.DrawArrow("<", Position + new Vector2(Width - Height * 2, 0), this, Color.Black);
                    MenuDrawHelper.DrawArrow(">", Position + new Vector2(Width - Height, 0), this, Color.Black);

                    MenuDrawHelper.Font.DrawText(
                        null, MultiLanguage._(t),
                        new Rectangle((int) Position.X - 5 - 2 * Height, (int) Position.Y, Width, Height),
                        FontDrawFlags.VerticalCenter | FontDrawFlags.Right, new ColorBGRA(255, 255, 255, 255));
                    break;
            }

            MenuDrawHelper.Font.DrawText(
                null, s, new Rectangle((int) Position.X + 5, (int) Position.Y, Width, Height),
                FontDrawFlags.VerticalCenter, new ColorBGRA(255, 255, 255, 255));
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

            BackgroundSprite = (Render.Sprite) new Render.Sprite(Resources.CPForm, new Vector2(X, Y)).Add(1);

            LuminitySprite = (Render.Sprite) new Render.Sprite(LuminityBitmap, new Vector2(X + 285, Y + 40)).Add(0);
            OpacitySprite = (Render.Sprite) new Render.Sprite(OpacityBitmap, new Vector2(X + 349, Y + 40)).Add(0);

            PreviewRectangle =
                (Render.Rectangle)
                    new Render.Rectangle(X + 375, Y + 44, 54, 80, new ColorBGRA(255, 255, 255, 255)).Add(0);

            LuminositySlider = new CPSlider(285 - Resources.CPActiveSlider.Width / 3, 35, 248);
            AlphaSlider = new CPSlider(350 - Resources.CPActiveSlider.Width / 3, 35, 248);

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
                    _selecting = true;
                    UpdateColor();
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
            if (Utils.TickCount - LastBitmapUpdate < 100 && !force)
            {
                return;
            }
            LastBitmapUpdate = Utils.TickCount;

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
            if (Utils.TickCount - LastBitmapUpdate2 < 100 && !force)
            {
                return;
            }
            LastBitmapUpdate2 = Utils.TickCount;

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
            private float _percent;
            private bool _visible = true;
            internal Render.Sprite ActiveSprite;
            public int Height;
            internal Render.Sprite InactiveSprite;
            public bool Moving;

            public CPSlider(int x, int y, int height, float percent = 1)
            {
                _x = x;
                _y = y;
                Height = height - Resources.CPActiveSlider.Height;
                Percent = percent;
                ActiveSprite = new Render.Sprite(Resources.CPActiveSlider, new Vector2(sX, sY));
                InactiveSprite = new Render.Sprite(Resources.CPInactiveSlider, new Vector2(sX, sY));

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
                get { return Resources.CPActiveSlider.Width; }
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
                Percent = (pos.Y - Resources.CPActiveSlider.Height / 2 - sY) / Height;
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
                        if (Utils.IsUnderRectangle(pos, sX, sY, Width, Height + Resources.CPActiveSlider.Height))
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
            public HSLColor() {}

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
                var hslColor = new HSLColor
                {
                    hue = color.GetHue() / 360.0,
                    luminosity = color.GetBrightness(),
                    saturation = color.GetSaturation()
                };
                // we store hue as 0-1 as opposed to 0-360 
                return hslColor;
            }

            #endregion
        }
    }
}
