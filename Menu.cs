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
using System.Drawing;
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
    /// <summary>
    /// The LeagueSharp.Common Menu
    /// </summary>
    internal class CommonMenu
    {
        /// <summary>
        /// The configuration
        /// </summary>
        internal static Menu Config = new Menu("LeagueSharp.Common", "LeagueSharp.Common", true);

        /// <summary>
        /// Initializes static members of the <see cref="CommonMenu"/> class.
        /// </summary>
        static CommonMenu()
        {
            TargetSelector.Initialize();
            Prediction.Initialize();
            Hacks.Initialize();
            FakeClicks.Initialize();
            Spell.Initialize();

            Config.AddToMainMenu();
        }
    }

    /// <summary>
    /// A color picker with an on/off switch.
    /// </summary>
    [Serializable]
    public struct Circle
    {
        /// <summary>
        /// Whether the circle is enabled or not.
        /// </summary>
        public bool Active;

        /// <summary>
        /// The color
        /// </summary>
        public Color Color;

        /// <summary>
        /// The radius
        /// </summary>
        public float Radius;

        /// <summary>
        /// Initializes a new instance of the <see cref="Circle"/> struct.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c>, the circle is enabled.</param>
        /// <param name="color">The color.</param>
        /// <param name="radius">The radius.</param>
        public Circle(bool enabled, Color color, float radius = 100)
        {
            Active = enabled;
            Color = color;
            Radius = radius;
        }
    }

    /// <summary>
    /// A slider.
    /// </summary>
    [Serializable]
    public struct Slider
    {
        /// <summary>
        /// The value
        /// </summary>
        private int _value;

        /// <summary>
        /// The maximum value
        /// </summary>
        public int MaxValue;

        /// <summary>
        /// The minimum value
        /// </summary>
        public int MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Slider"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public Slider(int value = 0, int minValue = 0, int maxValue = 100)
        {
            MaxValue = Math.Max(maxValue, minValue);
            MinValue = Math.Min(maxValue, minValue);
            _value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value
        {
            get { return _value; }
            set { _value = Math.Min(Math.Max(value, MinValue), MaxValue); }
        }
    }

    /// <summary>
    /// A list of strings.
    /// </summary>
    [Serializable]
    public struct StringList
    {
        /// <summary>
        /// The selected index
        /// </summary>
        public int SelectedIndex;

        /// <summary>
        /// The string list.
        /// </summary>
        public string[] SList;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringList"/> struct.
        /// </summary>
        /// <param name="sList">The string list.</param>
        /// <param name="defaultSelectedIndex">The index of the default option.</param>
        public StringList(string[] sList, int defaultSelectedIndex = 0)
        {
            SList = sList;
            SelectedIndex = defaultSelectedIndex;
        }

        /// <summary>
        /// Gets the selected value.
        /// </summary>
        /// <value>The selected value.</value>
        public string SelectedValue
        {
            get { return SList[SelectedIndex]; }
        }
    }

    /// <summary>
    /// Represents the type of keybind.
    /// </summary>
    public enum KeyBindType
    {
        /// <summary>
        /// The keybind toggles itself on and off.
        /// </summary>
        Toggle,

        /// <summary>
        /// The keybind must be pressed to be enabled.
        /// </summary>
        Press
    }

    /// <summary>
    /// Struct KeyBind
    /// </summary>
    [Serializable]
    public struct KeyBind
    {
        /// <summary>
        /// <c>true</c> if the user is holding the current key down.
        /// </summary>
        public bool Active;

        /// <summary>
        /// The key
        /// </summary>
        public uint Key;

        /// <summary>
        /// The key bind type
        /// </summary>
        public KeyBindType Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBind"/> struct.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="type">The type.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        public KeyBind(uint key, KeyBindType type, bool defaultValue = false)
        {
            Key = key;
            Active = defaultValue;
            Type = type;
        }
    }

    /// <summary>
    /// Serializes menu settings.
    /// </summary>
    [Serializable]
    internal static class SavedSettings
    {
        /// <summary>
        /// The loaded files
        /// </summary>
        public static Dictionary<string, Dictionary<string, byte[]>> LoadedFiles =
            new Dictionary<string, Dictionary<string, byte[]>>();

        /// <summary>
        /// Gets the saved data.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="key">The key.</param>
        /// <returns>System.Byte[].</returns>
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

        /// <summary>
        /// Loads the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Dictionary&lt;System.String, System.Byte[]&gt;.</returns>
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

        /// <summary>
        /// Saves the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="entries">The entries.</param>
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

    /// <summary>
    /// Global values regarding the menu.
    /// </summary>
    public static class MenuGlobals
    {
        /// <summary>
        /// <c>true</c> to draw the menu.
        /// </summary>
        public static bool DrawMenu;

        /// <summary>
        /// The menu state
        /// </summary>
        public static List<string> MenuState = new List<string>();
    }

    /// <summary>
    /// The menu settings.
    /// </summary>
    internal static class MenuSettings
    {
        /// <summary>
        /// The base position
        /// </summary>
        public static Vector2 BasePosition = new Vector2(10, 10);

        /// <summary>
        /// If <c>true</c>, draws the menu.
        /// </summary>
        private static bool _drawTheMenu;

        /// <summary>
        /// Initializes static members of the <see cref="MenuSettings"/> class.
        /// </summary>
        static MenuSettings()
        {
            Game.OnWndProc += Game_OnWndProc;
            _drawTheMenu = MenuGlobals.DrawMenu;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw the menu.
        /// </summary>
        /// <value><c>true</c> if we should draw the menu; otherwise, <c>false</c>.</value>
        internal static bool DrawMenu
        {
            get { return _drawTheMenu; }
            set
            {
                _drawTheMenu = value;
                MenuGlobals.DrawMenu = value;
            }
        }

        /// <summary>
        /// Gets the menu configuration path.
        /// </summary>
        /// <value>The menu configuration path.</value>
        public static string MenuConfigPath
        {
            get { return Path.Combine(Config.AppDataDirectory, "MenuConfig"); }
        }

        /// <summary>
        /// Gets the width of the menu item.
        /// </summary>
        /// <value>The width of the menu item.</value>
        public static int MenuItemWidth
        {
            get { return 160; }
        }

        /// <summary>
        /// Gets or sets the size of the menu font.
        /// </summary>
        /// <value>The size of the menu font.</value>
        public static int MenuFontSize { get; set; }

        /// <summary>
        /// Gets the height of the menu item.
        /// </summary>
        /// <value>The height of the menu item.</value>
        public static int MenuItemHeight
        {
            get { return 32; }
        }

        /// <summary>
        /// Gets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        public static Color BackgroundColor
        {
            get { return Color.FromArgb(Menu.root.Item("BackgroundAlpha").GetValue<Slider>().Value, Color.Black); }
        }

        /// <summary>
        /// Gets the color of the active background.
        /// </summary>
        /// <value>The color of the active background.</value>
        public static Color ActiveBackgroundColor
        {
            get { return Color.FromArgb(0, 37, 53); }
        }

        /// <summary>
        /// Fired when the game receives a window event.
        /// </summary>
        /// <param name="args">The <see cref="WndEventArgs"/> instance containing the event data.</param>
        private static void Game_OnWndProc(WndEventArgs args)
        {
            if ((args.Msg == (uint)WindowsMessages.WM_KEYUP || args.Msg == (uint)WindowsMessages.WM_KEYDOWN) &&
                args.WParam == Config.ShowMenuPressKey)
            {
                DrawMenu = args.Msg == (uint)WindowsMessages.WM_KEYDOWN;
            }

            if (args.Msg == (uint)WindowsMessages.WM_KEYUP && args.WParam == Config.ShowMenuToggleKey)
            {
                DrawMenu = !DrawMenu;
            }
        }
    }

    /// <summary>
    /// Contains methods to help draw the menu.
    /// </summary>
    internal static class MenuDrawHelper
    {
        /// <summary>
        /// The fonts.
        /// </summary>
        internal static Font Font, FontBold;

        /// <summary>
        /// Initializes static members of the <see cref="MenuDrawHelper"/> class.
        /// </summary>
        static MenuDrawHelper()
        {
            Font = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = Menu.root.Item("FontName").GetValue<StringList>().SelectedValue,
                    Height = Menu.root.Item("FontSize").GetValue<Slider>().Value,
                    OutputPrecision = FontPrecision.Default,
                    Quality =
                        (FontQuality)
                            Enum.Parse(
                                typeof(FontQuality), Menu.root.Item("FontQuality").GetValue<StringList>().SelectedValue,
                                true)
                });

            FontBold = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = Menu.root.Item("FontName").GetValue<StringList>().SelectedValue,
                    Height = Menu.root.Item("FontSize").GetValue<Slider>().Value,
                    OutputPrecision = FontPrecision.Default,
                    Weight = FontWeight.Bold,
                    Quality =
                        (FontQuality)
                            Enum.Parse(
                                typeof(FontQuality), Menu.root.Item("FontQuality").GetValue<StringList>().SelectedValue,
                                true)
                });

            Drawing.OnPreReset += Drawing_OnPreReset;
            Drawing.OnPostReset += DrawingOnOnPostReset;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainOnDomainUnload;
        }

        /// <summary>
        /// Fired when the AppDomain is unloaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void CurrentDomainOnDomainUnload(object sender, EventArgs eventArgs)
        {
            if (Font != null)
            {
                Font.Dispose();
                Font = null;
            }
            if (FontBold != null)
            {
                FontBold.Dispose();
                FontBold = null;
            }
        }

        /// <summary>
        /// Fired when the DirectX device is reset.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void DrawingOnOnPostReset(EventArgs args)
        {
            Font.OnResetDevice();
            FontBold.OnResetDevice();
        }

        /// <summary>
        /// Fired before the DirectX device is reset.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void Drawing_OnPreReset(EventArgs args)
        {
            Font.OnLostDevice();
            FontBold.OnLostDevice();
        }

        /// <summary>
        /// Draws a box.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="color">The color.</param>
        /// <param name="borderwidth">The border width.</param>
        /// <param name="borderColor">Color of the border.</param>
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

        /// <summary>
        /// Draws the on off toggle switch.
        /// </summary>
        /// <param name="on">if set to <c>true</c> draws the switch as on.</param>
        /// <param name="position">The position.</param>
        /// <param name="item">The item.</param>
        internal static void DrawOnOff(bool on, Vector2 position, MenuItem item)
        {
            DrawBox(position, item.Height, item.Height, on ? Color.FromArgb(1, 169, 234) : Color.FromArgb(37, 37, 37), 1, Color.Black);
            var s = on ? "ON" : "OFF";
            Font.DrawText(
                null, s,
                new Rectangle(
                    (int)(item.Position.X + item.Width - item.Height), (int)item.Position.Y, item.Height, item.Height),
                FontDrawFlags.VerticalCenter | FontDrawFlags.Center, new ColorBGRA(255, 255, 255, 255));
        }

        /// <summary>
        /// Draws an arrow.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="position">The position.</param>
        /// <param name="item">The item.</param>
        /// <param name="color">The color.</param>
        internal static void DrawArrow(string s, Vector2 position, MenuItem item, Color color)
        {
            DrawBox(position, item.Height, item.Height, Color.FromArgb(0, 37, 53), 1, color);
            Font.DrawText(
                null, s, new Rectangle((int)(position.X), (int)item.Position.Y, item.Height, item.Height),
                FontDrawFlags.VerticalCenter | FontDrawFlags.Center, new ColorBGRA(255, 255, 255, 255));
        }

        /// <summary>
        /// Draws a slider.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="item">The item.</param>
        /// <param name="width">The width.</param>
        /// <param name="drawText">if set to <c>true</c> draws the text.</param>
        internal static void DrawSlider(Vector2 position, MenuItem item, int width = -1, bool drawText = true)
        {
            var val = item.GetValue<Slider>();
            DrawSlider(position, item, val.MinValue, val.MaxValue, val.Value, width, drawText);
        }

        /// <summary>
        /// Draws a slider.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="item">The item.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="value">The value.</param>
        /// <param name="width">The width.</param>
        /// <param name="drawText">if set to <c>true</c> draws the text.</param>
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
            var x2D = 3 + (percentage * (width - 3)) / 100;
            Drawing.DrawLine(x, position.Y + 2, x, position.Y + item.Height, 2, Color.FromArgb(0, 74, 103));
            DrawBox(new Vector2(position.X, position.Y), x2D - 2, item.Height, Color.FromArgb(0, 37, 53), 0, Color.Black);

            if (drawText)
            {
                Font.DrawText(
                    null, value.ToString(),
                    new Rectangle((int)position.X - 5, (int)position.Y, item.Width, item.Height),
                    FontDrawFlags.VerticalCenter | FontDrawFlags.Right, new ColorBGRA(255, 255, 255, 255));
            }
        }

        /// <summary>
        /// Draws a tool tip button.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="item">The item.</param>
        internal static void DrawToolTip_Button(Vector2 position, MenuItem item)
        {
            if (item.ValueType == MenuValueType.StringList)
            {
                return;
            }

            var s = "[?]";
            int x = (int) item.Position.X + item.Width - item.Height - Font.MeasureText(s).Width - 7;

            Font.DrawText(null, s,
                new Rectangle(x, (int)item.Position.Y, item.Width, item.Height),
                    FontDrawFlags.VerticalCenter,
                        new ColorBGRA(255, 255, 255, 255));
        }

        /// <summary>
        /// Draws the tool tip text.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="item">The item.</param>
        /// <param name="TextColor">Color of the text.</param>
        internal static void DrawToolTip_Text(Vector2 position, MenuItem item, SharpDX.Color? TextColor = null)
        {
            if (item.ValueType == MenuValueType.StringList)
            {
                return;
            }

            DrawBox(new Vector2(position.X + item.Height - 28, position.Y + 1),
                (int) Font.MeasureText(item.Tooltip).Width + 8, item.Height, MenuSettings.BackgroundColor, 1, Color.Black);

            var s = item.Tooltip;
            Font.DrawText(
                null, s,
                new Rectangle(
                    (int) (item.Position.X + item.Width - 33 + item.Height + 8), (int) item.Position.Y - 3,
                    Font.MeasureText(item.Tooltip).Width + 8, item.Height + 8),
                FontDrawFlags.VerticalCenter, TextColor ?? SharpDX.Color.White);
        }
    }

    /// <summary>
    /// A menu.
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// The root menus
        /// </summary>
        public static Dictionary<string, Menu> RootMenus = new Dictionary<string, Menu>();

        /// <summary>
        /// The root menu
        /// </summary>
        public static readonly Menu root = new Menu("Menu Settings", "Menu Settings");

        /// <summary>
        /// The cached menu count
        /// </summary>
        private int _cachedMenuCount = 2;

        /// <summary>
        /// The cached menu count tick
        /// </summary>
        private int _cachedMenuCountT;

        /// <summary>
        /// If the menu is visible
        /// </summary>
        private bool _visible;

        /// <summary>
        /// The children
        /// </summary>
        public List<Menu> Children = new List<Menu>();

        /// <summary>
        /// The color
        /// </summary>
        public SharpDX.Color Color;

        /// <summary>
        /// The display name
        /// </summary>
        public string DisplayName;

        /// <summary>
        /// Whether this instance is a root menu.
        /// </summary>
        public bool IsRootMenu;

        /// <summary>
        /// The menu items
        /// </summary>
        public List<MenuItem> Items = new List<MenuItem>();

        /// <summary>
        /// The name
        /// </summary>
        public string Name;

        /// <summary>
        /// The parent menu
        /// </summary>
        public Menu Parent;

        /// <summary>
        /// The font style
        /// </summary>
        public FontStyle Style;

        /// <summary>
        /// The unique identifier
        /// </summary>
        private string uniqueId;

        /// <summary>
        /// Initializes static members of the <see cref="Menu"/> class.
        /// </summary>
        static Menu()
        {
            root.AddItem(new MenuItem("BackgroundAlpha", "Background Opacity")).SetValue(new Slider(165, 55, 255));
            root.AddItem(
                new MenuItem("FontName", "Font Name:").SetValue(
                    new StringList(new[] { "Tahoma", "Calibri", "Segoe UI" }, 0)));
            root.AddItem(new MenuItem("FontSize", "Font Size:").SetValue(new Slider(13, 12, 20)));
            var qualities = Enum.GetValues(typeof(FontQuality)).Cast<FontQuality>().Select(v => v.ToString()).ToArray();
            root.AddItem(new MenuItem("FontQuality", "Font Quality").SetValue(new StringList(qualities, 4)));
            root.AddItem(
                new MenuItem("LeagueSharp.Common.TooltipDuration", "Tooltip Notification Duration").SetValue(
                    new Slider(1500, 0, 5000)));
            root.AddItem(
                new MenuItem("FontInfo", "Press F5 after your change").SetFontStyle(FontStyle.Bold, SharpDX.Color.Yellow));
            CommonMenu.Config.AddSubMenu(root);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="name">The name.</param>
        /// <param name="isRootMenu">if set to <c>true</c> [is root menu].</param>
        public Menu(string displayName,
            string name,
            bool isRootMenu = false)
        {
            DisplayName = displayName;
            Name = name;
            IsRootMenu = isRootMenu;
            Style = FontStyle.Regular;
            Color = SharpDX.Color.White;

            if (isRootMenu)
            {
                CustomEvents.Game.OnGameEnd += delegate { SaveAll(); };
                Game.OnEnd += delegate { SaveAll(); };
                AppDomain.CurrentDomain.DomainUnload += delegate { SaveAll(); };
                AppDomain.CurrentDomain.ProcessExit += delegate { SaveAll(); };

                var rootName = Assembly.GetCallingAssembly().GetName().Name + "." + name;

                if (RootMenus.ContainsKey(rootName))
                {
                    throw new ArgumentException("Root Menu [" + rootName + "] with the same name exists", "name");
                }

                RootMenus.Add(rootName, this);
            }
        }

        /// <summary>
        /// Sets the font style.
        /// </summary>
        /// <param name="fontStyle">The font style.</param>
        /// <param name="fontColor">Color of the font.</param>
        /// <returns>Menu.</returns>
        public Menu SetFontStyle(FontStyle fontStyle = FontStyle.Regular, SharpDX.Color? fontColor = null)
        {
            Style = fontStyle;
            Color = fontColor ?? SharpDX.Color.White;

            return this;
        }

        /// <summary>
        /// Gets the x level.
        /// </summary>
        /// <value>The x level.</value>
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

        /// <summary>
        /// Gets the y level.
        /// </summary>
        /// <value>The y level.</value>
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

        /// <summary>
        /// Gets the menu count.
        /// </summary>
        /// <value>The menu count.</value>
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
                    if (item == uniqueId)
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

        /// <summary>
        /// Gets the base position.
        /// </summary>
        /// <value>The base position.</value>
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

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        internal Vector2 Position
        {
            get
            {
                var xOffset = 0;

                if (Parent != null)
                {
                    xOffset = (int)(Parent.Position.X + Parent.Width);
                }
                else
                {
                    xOffset = (int)MyBasePosition.X;
                }

                return new Vector2(0, MyBasePosition.Y) + new Vector2(xOffset, 0) +
                       YLevel * new Vector2(0, MenuSettings.MenuItemHeight);
            }
        }

        /// <summary>
        /// Gets the width of the children menu.
        /// </summary>
        /// <value>The width of the children menu.</value>
        internal int ChildrenMenuWidth
        {
            get
            {
                var result = Children.Select(item => item.NeededWidth).Concat(new[] { 0 }).Max();

                return Items.Select(item => item.NeededWidth).Concat(new[] { result }).Max();
            }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        internal int Width
        {
            get { return Parent != null ? Parent.ChildrenMenuWidth : MenuSettings.MenuItemWidth; }
        }

        /// <summary>
        /// Gets the needed width
        /// </summary>
        /// <value>The needed width</value>
        internal int NeededWidth
        {
            get { return MenuDrawHelper.Font.MeasureText(MultiLanguage._(DisplayName)).Width + 25; }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        internal int Height
        {
            get { return MenuSettings.MenuItemHeight; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Menu"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// Gets the value globally.
        /// </summary>
        /// <param name="Assemblyname">The assembly name.</param>
        /// <param name="menuname">The menu name.</param>
        /// <param name="itemname">The item name.</param>
        /// <param name="submenu">The submenu.</param>
        /// <returns>MenuItem.</returns>
        public static MenuItem GetValueGlobally(string Assemblyname,
            string menuname,
            string itemname,
            string submenu = null)
        {
            var menu = RootMenus.FirstOrDefault(x => x.Key == Assemblyname + "." + menuname).Value;

            if (submenu != null)
            {
                menu = menu.SubMenu(submenu);
            }

            var menuitem = menu.Item(itemname);

            return menuitem;
        }

        /// <summary>
        /// Gets the menu.
        /// </summary>
        /// <param name="Assemblyname">The assembly name.</param>
        /// <param name="menuname">The menu name.</param>
        /// <returns>Menu.</returns>
        public static Menu GetMenu(string Assemblyname, string menuname)
        {
            var menu = RootMenus.FirstOrDefault(x => x.Key == Assemblyname + "." + menuname).Value;
            return menu;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="message">The message.</param>
        public static void SendMessage(uint key, WindowsMessages message)
        {
            foreach (var menu in RootMenus)
            {
                menu.Value.OnReceiveMessage(message, Utils.GetCursorPos(), key);
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Menu"/> class.
        /// </summary>
        ~Menu()
        {
            if (RootMenus.ContainsKey(Name))
            {
                RootMenus.Remove(Name);
            }
        }

        /// <summary>
        /// Determines whether the specified position is inside the menu.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns><c>true</c> if the specified position is inside the menu; otherwise, <c>false</c>.</returns>
        internal bool IsInside(Vector2 position)
        {
            return Utils.IsUnderRectangle(position, Position.X, Position.Y, Width, Height);
        }

        /// <summary>
        /// Fired when the game receives a window event.
        /// </summary>
        /// <param name="args">The <see cref="WndEventArgs"/> instance containing the event data.</param>
        internal void Game_OnWndProc(WndEventArgs args)
        {
            OnReceiveMessage((WindowsMessages)args.Msg, Utils.GetCursorPos(), args.WParam);
        }

        /// <summary>
        /// Called when the game receives a window message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="cursorPos">The cursor position.</param>
        /// <param name="key">The key.</param>
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
                    var n = (int)(cursorPos.Y - MenuSettings.BasePosition.Y) / MenuSettings.MenuItemHeight;
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

        /// <summary>
        /// Fired when the game is drawn.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
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
                    : MenuSettings.BackgroundColor, 1, System.Drawing.Color.Black);

            MenuDrawHelper.Font.DrawText(
                null, MultiLanguage._(DisplayName), new Rectangle((int)Position.X + 5, (int)Position.Y, Width, Height),
                FontDrawFlags.VerticalCenter, Color);
            MenuDrawHelper.Font.DrawText(
                null, ">", new Rectangle((int)Position.X - 5, (int)Position.Y, Width, Height),
                FontDrawFlags.Right | FontDrawFlags.VerticalCenter, Color);

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

        /// <summary>
        /// Saves everything recursively.
        /// </summary>
        /// <param name="dics">The dics.</param>
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

        /// <summary>
        /// Saves all.
        /// </summary>
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

        /// <summary>
        /// Adds the menu to the main menu.
        /// </summary>
        public void AddToMainMenu()
        {
            InitMenuState(Assembly.GetCallingAssembly().GetName().Name);
            AppDomain.CurrentDomain.DomainUnload += (sender, args) => UnloadMenuState();
            Drawing.OnEndScene += Drawing_OnDraw;
            Game.OnWndProc += Game_OnWndProc;
        }

        /// <summary>
        /// Initializes the state of the menu.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
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

        /// <summary>
        /// Unloads the state of the menu.
        /// </summary>
        private void UnloadMenuState()
        {
            var globalMenuList = MenuGlobals.MenuState;
            globalMenuList.Remove(uniqueId);
            MenuGlobals.MenuState = globalMenuList;
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>MenuItem.</returns>
        public MenuItem AddItem(MenuItem item)
        {
            item.Parent = this;
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Adds the sub menu.
        /// </summary>
        /// <param name="subMenu">The sub menu.</param>
        /// <returns>Menu.</returns>
        public Menu AddSubMenu(Menu subMenu)
        {
            subMenu.Parent = this;
            Children.Add(subMenu);

            return subMenu;
        }

        /// <summary>
        /// Gets the menu item of the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="makeChampionUniq">if set to <c>true</c> [make champion uniq].</param>
        /// <returns>MenuItem.</returns>
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

        /// <summary>
        /// Gets the submenu with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Menu.</returns>
        public Menu SubMenu(string name)
        {
            //Search in submenus and if it doesn't exist add it.
            var subMenu = Children.FirstOrDefault(sm => sm.Name == name);
            return subMenu ?? AddSubMenu(new Menu(name, name));
        }
    }

    /// <summary>
    /// A type of menu item.
    /// </summary>
    internal enum MenuValueType
    {
        /// <summary>
        /// None.
        /// </summary>
        None,

        /// <summary>
        /// A boolean
        /// </summary>
        Boolean,

        /// <summary>
        /// A slider
        /// </summary>
        Slider,

        /// <summary>
        /// A key bind
        /// </summary>
        KeyBind,

        /// <summary>
        /// An integer
        /// </summary>
        Integer,

        /// <summary>
        /// A color
        /// </summary>
        Color,

        /// <summary>
        /// A circle
        /// </summary>
        Circle,

        /// <summary>
        /// A string list
        /// </summary>
        StringList
    }

    /// <summary>
    /// The event arguments for the <see cref="MenuItem.ValueChanged"/> event.
    /// </summary>
    public class OnValueChangeEventArgs : EventArgs
    {
        /// <summary>
        /// The new value
        /// </summary>
        private readonly object _newValue;

        /// <summary>
        /// The old value
        /// </summary>
        private readonly object _oldValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnValueChangeEventArgs"/> class.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public OnValueChangeEventArgs(object oldValue, object newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
            Process = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="OnValueChangeEventArgs"/> should process the change.
        /// </summary>
        /// <value><c>true</c> if the menu should process the change; otherwise, <c>false</c>.</value>
        public bool Process { get; set; }

        /// <summary>
        /// Gets the old value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        public T GetOldValue<T>()
        {
            return (T)_oldValue;
        }

        /// <summary>
        /// Gets the new value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        public T GetNewValue<T>()
        {
            return (T)_newValue;
        }
    }

    /// <summary>
    /// A Menu Item.
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// The configuration name
        /// </summary>
        private readonly string _configName;

        /// <summary>
        /// Whether to save the menu value or not.
        /// </summary>
        private bool _dontSave;

        /// <summary>
        /// If this menu item is shared across menus.
        /// </summary>
        private bool _isShared;

        /// <summary>
        /// The serialized data
        /// </summary>
        private byte[] _serialized;

        /// <summary>
        /// The value
        /// </summary>
        private object _value;

        /// <summary>
        /// If the value was set
        /// </summary>
        internal bool _valueSet;

        /// <summary>
        /// If the menu item is visible
        /// </summary>
        private bool _visible;

        /// <summary>
        /// The display name
        /// </summary>
        public string DisplayName;

        /// <summary>
        /// The font color
        /// </summary>
        public ColorBGRA FontColor;

        /// <summary>
        /// The font style
        /// </summary>
        public FontStyle FontStyle;

        /// <summary>
        /// If the menu item is interacting
        /// </summary>
        internal bool Interacting;

        /// <summary>
        /// The menu font size
        /// </summary>
        public int MenuFontSize;

        /// <summary>
        /// The name
        /// </summary>
        public string Name;

        /// <summary>
        /// The parent
        /// </summary>
        public Menu Parent;

        /// <summary>
        /// Whether to show the item or not.
        /// </summary>
        public bool ShowItem;

        /// <summary>
        /// The tag
        /// </summary>
        public int Tag;

        /// <summary>
        /// The value type
        /// </summary>
        internal MenuValueType ValueType;

        /// <summary>
        /// The tooltip
        /// </summary>
        public string Tooltip;

        /// <summary>
        /// Gets the duration of the tooltip.
        /// </summary>
        /// <value>The duration of the tooltip.</value>
        public int TooltipDuration
        {
            get
            {
                return CommonMenu.Config.Item("LeagueSharp.Common.TooltipDuration").GetValue<Slider>().Value;
            }
        }

        /// <summary>
        /// The tooltip color
        /// </summary>
        public SharpDX.Color TooltipColor;

        /// <summary>
        /// The drawing tooltip
        /// </summary>
        internal bool DrawingTooltip;


        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="makeChampionUniq">if set to <c>true</c> [make champion uniq].</param>
        public MenuItem(string name,
            string displayName,
            bool makeChampionUniq = false)
        {
            if (makeChampionUniq)
            {
                name = ObjectManager.Player.ChampionName + name;
            }

            Name = name;
            DisplayName = displayName;
            FontStyle = FontStyle.Regular;
            FontColor = SharpDX.Color.White;
            ShowItem = true;
            Tag = 0;
            _configName = Assembly.GetCallingAssembly().GetName().Name + Assembly.GetCallingAssembly().GetType().GUID;
        }

        /// <summary>
        /// Sets the font style.
        /// </summary>
        /// <param name="fontStyle">The font style.</param>
        /// <param name="fontColor">Color of the font.</param>
        /// <returns>MenuItem.</returns>
        public MenuItem SetFontStyle(FontStyle fontStyle = FontStyle.Regular, SharpDX.Color? fontColor = null)
        {
            FontStyle = fontStyle;
            FontColor = fontColor ?? SharpDX.Color.White;

            return this;
        }

        /// <summary>
        /// Sets the tooltip.
        /// </summary>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="tooltipColor">Color of the tooltip.</param>
        /// <returns>MenuItem.</returns>
        public MenuItem SetTooltip(string tooltip, SharpDX.Color? tooltipColor = null)
        {
            Tooltip = tooltip;
            TooltipColor = tooltipColor ?? SharpDX.Color.White;
            return this;
        }

        /// <summary>
        /// Shows the specified show item.
        /// </summary>
        /// <param name="showItem">if set to <c>true</c> [show item].</param>
        /// <returns>MenuItem.</returns>
        public MenuItem Show(bool showItem = true)
        {
            this.ShowItem = showItem;

            return this;
        }

        /// <summary>
        /// Sets the tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>MenuItem.</returns>
        public MenuItem SetTag(int tag = 0)
        {
            this.Tag = tag;

            return this;
        }

        /// <summary>
        /// Gets the name of the save file.
        /// </summary>
        /// <value>The name of the save file.</value>
        internal string SaveFileName
        {
            get { return (_isShared ? "SharedConfig" : _configName); }
        }

        /// <summary>
        /// Gets the save key.
        /// </summary>
        /// <value>The save key.</value>
        internal string SaveKey
        {
            get { return Utils.Md5Hash("v3" + DisplayName + Name); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MenuItem"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        internal bool Visible
        {
            get { return MenuSettings.DrawMenu && _visible && ShowItem; }
            set { _visible = value; }
        }

        /// <summary>
        /// Gets the y level.
        /// </summary>
        /// <value>The y level.</value>
        internal int YLevel
        {
            get
            {
                if (Parent == null)
                {
                    return 0;
                }

                return Parent.YLevel + Parent.Children.Count +
                       Parent.Items.TakeWhile(test => test.Name != Name).Count(c => c.ShowItem);
            }
        }

        /// <summary>
        /// Gets the base position.
        /// </summary>
        /// <value>The base position.</value>
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

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        internal Vector2 Position
        {
            get
            {
                var xOffset = 0;

                if (Parent != null)
                {
                    xOffset = (int)(Parent.Position.X + Parent.Width);
                }

                return new Vector2(0, MyBasePosition.Y) + new Vector2(xOffset, 0) +
                       YLevel * new Vector2(0, MenuSettings.MenuItemHeight);
            }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        internal int Width
        {
            get { return Parent != null ? Parent.ChildrenMenuWidth : MenuSettings.MenuItemWidth; }
        }

        /// <summary>
        /// Gets the needed width.
        /// </summary>
        /// <value>The needed width.</value>
        internal int NeededWidth
        {
            get
            {
                var extra = 0;

                if (ValueType == MenuValueType.StringList)
                {
                    var slVal = GetValue<StringList>();
                    var max =
                        slVal.SList.Select(v => MenuDrawHelper.Font.MeasureText(v).Width + 25).Concat(new[] { 0 }).Max();

                    extra += max;
                }

                if (ValueType == MenuValueType.KeyBind)
                {
                    var val = GetValue<KeyBind>();
                    extra += MenuDrawHelper.Font.MeasureText(" [" + Utils.KeyToText(val.Key) + "]").Width;
                }

                return MenuDrawHelper.Font.MeasureText(MultiLanguage._(DisplayName)).Width + Height * 2 + 10 + extra;
            }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        internal int Height
        {
            get { return MenuSettings.MenuItemHeight; }
        }

        /// <summary>
        /// Occurs when the menu item's value is changed.
        /// </summary>
        public event EventHandler<OnValueChangeEventArgs> ValueChanged;

        /// <summary>
        /// Makes this instance shared among menus.
        /// </summary>
        /// <returns>MenuItem.</returns>
        public MenuItem SetShared()
        {
            _isShared = true;
            return this;
        }

        /// <summary>
        /// Makes this instance not save its value.
        /// </summary>
        /// <returns>MenuItem.</returns>
        public MenuItem DontSave()
        {
            _dontSave = true;
            return this;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        public T GetValue<T>()
        {
            return (T)_value;
        }

        /// <summary>
        /// Determines whether this instance is active.
        /// </summary>
        /// <returns><c>true</c> if this instance is active; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newValue">The new value.</param>
        /// <returns>MenuItem.</returns>
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
                            var savedKeyValue = (KeyBind)(object)Utils.Deserialize<T>(readBytes);
                            if (savedKeyValue.Type == KeyBindType.Press)
                            {
                                savedKeyValue.Active = false;
                            }
                            newValue = (T)(object)savedKeyValue;
                            break;

                        case MenuValueType.Circle:
                            var savedCircleValue = (Circle)(object)Utils.Deserialize<T>(readBytes);
                            var newCircleValue = (Circle)(object)newValue;
                            savedCircleValue.Radius = newCircleValue.Radius;
                            newValue = (T)(object)savedCircleValue;
                            break;

                        case MenuValueType.Slider:
                            var savedSliderValue = (Slider)(object)Utils.Deserialize<T>(readBytes);
                            var newSliderValue = (Slider)(object)newValue;
                            if (savedSliderValue.MinValue == newSliderValue.MinValue &&
                                savedSliderValue.MaxValue == newSliderValue.MaxValue)
                            {
                                newValue = (T)(object)savedSliderValue;
                            }
                            break;

                        case MenuValueType.StringList:
                            var savedListValue = (StringList)(object)Utils.Deserialize<T>(readBytes);
                            var newListValue = (StringList)(object)newValue;
                            if (savedListValue.SList.SequenceEqual(newListValue.SList))
                            {
                                newValue = (T)(object)savedListValue;
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

        /// <summary>
        /// Saves to file.
        /// </summary>
        /// <param name="dics">The dics.</param>
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

        /// <summary>
        /// Determines whether the specified position is inside.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns><c>true</c> if the specified position is inside; otherwise, <c>false</c>.</returns>
        internal bool IsInside(Vector2 position)
        {
            return Utils.IsUnderRectangle(position, Position.X, Position.Y, !String.IsNullOrEmpty(this.Tooltip) ? Width + Height : Width, Height);
        }

        /// <summary>
        /// Called when a windows message is received.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="cursorPos">The cursor position.</param>
        /// <param name="key">The key.</param>
        internal void OnReceiveMessage(WindowsMessages message, Vector2 cursorPos, uint key)
        {
            if (message == WindowsMessages.WM_MOUSEMOVE)
            {
                if (Visible && IsInside(cursorPos))
                {
                    if (cursorPos.X > Position.X + Width - 67 && cursorPos.X < Position.X + Width - 67 + Height + 8)
                    {
                        ShowTooltip();
                    }
                }
                else
                {
                    ShowTooltip(true);
                }
            }

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

                    if (cursorPos.X > Position.X + Width)
                    {
                        break;
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
                        val.Value = (int)t;
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

                    if (cursorPos.X > Position.X + Width)
                    {
                        break;
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

                    if (cursorPos.X > Position.X + Width)
                    {
                        break;
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

                    if (!MenuGUI.IsChatOpen && !MenuGUI.IsShopOpen)
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

                    if (cursorPos.X > Position.X + Width)
                    {
                        break;
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

                    if (cursorPos.X > Position.X + Width)
                    {
                        break;
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

        /// <summary>
        /// Shows the tooltip notification.
        /// </summary>
        public void ShowTooltip_Notification()
        {
            if (!string.IsNullOrEmpty(this.Tooltip))
            {
                var notif = new Notification(this.Tooltip).SetTextColor(Color.White);
                Notifications.AddNotification(notif);
                Utility.DelayAction.Add(this.TooltipDuration, () => notif.Dispose());
            }
        }

        /// <summary>
        /// Shows the tooltip.
        /// </summary>
        /// <param name="hide">if set to <c>true</c> [hide].</param>
        public void ShowTooltip(bool hide = false)
        {
            if (!string.IsNullOrEmpty(this.Tooltip))
            {
                DrawingTooltip = !hide;
            }
        }

        /// <summary>
        /// Fired when the game is drawn.
        /// </summary>
        internal void Drawing_OnDraw()
        {
            var s = MultiLanguage._(DisplayName);

            MenuDrawHelper.DrawBox(Position, Width, Height, MenuSettings.BackgroundColor, 1, Color.Black);

            if (DrawingTooltip)
            {
                MenuDrawHelper.DrawToolTip_Text(new Vector2(Position.X + Width, Position.Y), this, TooltipColor);
            }

            Font font;
            switch (FontStyle)
            {
                case FontStyle.Bold:
                    font = MenuDrawHelper.FontBold;
                    break;
                default:
                    font = MenuDrawHelper.Font;
                    break;
            }

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

                    if (Interacting)
                    {
                        s = MultiLanguage._("Press new key");
                    }

                    var x = !string.IsNullOrEmpty(this.Tooltip)
                        ? (int) Position.X + Width - Height -
                          font.MeasureText("[" + Utils.KeyToText(val.Key) + "]").Width - 35
                        : (int) Position.X + Width - Height -
                          font.MeasureText("[" + Utils.KeyToText(val.Key) + "]").Width - 10;

                    font.DrawText(null, "[" + Utils.KeyToText(val.Key) + "]", new Rectangle(x, (int)Position.Y, Width, Height), FontDrawFlags.VerticalCenter, new ColorBGRA(1, 169, 234, 255));
                    MenuDrawHelper.DrawOnOff(val.Active, new Vector2(Position.X + Width - Height, Position.Y), this);

                    break;

                case MenuValueType.Integer:
                    var intVal = GetValue<int>();
                    MenuDrawHelper.Font.DrawText(
                        null, intVal.ToString(), new Rectangle((int)Position.X + 5, (int)Position.Y, Width, Height),
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

                    MenuDrawHelper.DrawArrow("<<", Position + new Vector2(Width - Height * 2, 0), this, Color.Black);
                    MenuDrawHelper.DrawArrow(">>", Position + new Vector2(Width - Height, 0), this, Color.Black);

                    MenuDrawHelper.Font.DrawText(
                        null, MultiLanguage._(t),
                        new Rectangle((int)Position.X - 5 - 2 * Height, (int)Position.Y, Width, Height),
                        FontDrawFlags.VerticalCenter | FontDrawFlags.Right, new ColorBGRA(255, 255, 255, 255));
                    break;
            }

            if (!String.IsNullOrEmpty(this.Tooltip))
            {
                MenuDrawHelper.DrawToolTip_Button(new Vector2(Position.X + Width, Position.Y), this);
            }

            font.DrawText(
                null, s, new Rectangle((int)Position.X + 5, (int)Position.Y, Width, Height),
                FontDrawFlags.VerticalCenter, FontColor);
        }
    }



    /// <summary>
    /// A color picker.
    /// </summary>
    public static class ColorPicker
    {
        /// <summary>
        /// Delegate OnSelectColor
        /// </summary>
        /// <param name="color">The color.</param>
        public delegate void OnSelectColor(Color color);

        /// <summary>
        /// Occurs when the color is changed.
        /// </summary>
        public static OnSelectColor OnChangeColor;

        /// <summary>
        /// The x
        /// </summary>
        private static int _x = 100;

        /// <summary>
        /// The y
        /// </summary>
        private static int _y = 100;

        /// <summary>
        /// If the dialog is moving
        /// </summary>
        private static bool _moving;

        /// <summary>
        /// If the user is selecting a color.
        /// </summary>
        private static bool _selecting;

        /// <summary>
        /// The background sprite
        /// </summary>
        public static Render.Sprite BackgroundSprite;

        /// <summary>
        /// The luminity sprite
        /// </summary>
        public static Render.Sprite LuminitySprite;

        /// <summary>
        /// The opacity sprite
        /// </summary>
        public static Render.Sprite OpacitySprite;

        /// <summary>
        /// The preview rectangle
        /// </summary>
        public static Render.Rectangle PreviewRectangle;

        /// <summary>
        /// The luminity bitmap
        /// </summary>
        public static Bitmap LuminityBitmap;

        /// <summary>
        /// The opacity bitmap
        /// </summary>
        public static Bitmap OpacityBitmap;

        /// <summary>
        /// The luminosity slider
        /// </summary>
        public static CPSlider LuminositySlider;

        /// <summary>
        /// The alpha slider
        /// </summary>
        public static CPSlider AlphaSlider;

        /// <summary>
        /// The previous position
        /// </summary>
        private static Vector2 _prevPos;

        /// <summary>
        /// If the color picker is visible
        /// </summary>
        private static bool _visible;

        /// <summary>
        /// The string color
        /// </summary>
        private static HSLColor SColor = new HSLColor(255, 255, 255);

        /// <summary>
        /// The initial color
        /// </summary>
        private static Color InitialColor;

        /// <summary>
        /// The string hue
        /// </summary>
        private static double SHue;

        /// <summary>
        /// The string saturation
        /// </summary>
        private static double SSaturation;

        /// <summary>
        /// The last bitmap update
        /// </summary>
        private static int LastBitmapUpdate;

        /// <summary>
        /// The last bitmap update 2
        /// </summary>
        private static int LastBitmapUpdate2;

        /// <summary>
        /// Initializes static members of the <see cref="ColorPicker"/> class.
        /// </summary>
        static ColorPicker()
        {
            LuminityBitmap = new Bitmap(9, 238);
            OpacityBitmap = new Bitmap(9, 238);

            UpdateLuminosityBitmap(Color.White, true);
            UpdateOpacityBitmap(Color.White, true);

            BackgroundSprite = (Render.Sprite)new Render.Sprite(Resources.CPForm, new Vector2(X, Y)).Add(1);

            LuminitySprite = (Render.Sprite)new Render.Sprite(LuminityBitmap, new Vector2(X + 285, Y + 40)).Add(0);
            OpacitySprite = (Render.Sprite)new Render.Sprite(OpacityBitmap, new Vector2(X + 349, Y + 40)).Add(0);

            PreviewRectangle =
                (Render.Rectangle)
                    new Render.Rectangle(X + 375, Y + 44, 54, 80, new ColorBGRA(255, 255, 255, 255)).Add(0);

            LuminositySlider = new CPSlider(285 - Resources.CPActiveSlider.Width / 3, 35, 248);
            AlphaSlider = new CPSlider(350 - Resources.CPActiveSlider.Width / 3, 35, 248);

            Game.OnWndProc += Game_OnWndProc;
        }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>The x.</value>
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

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>The y.</value>
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

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ColorPicker"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// Gets the color picker x.
        /// </summary>
        /// <value>The color picker x.</value>
        public static int ColorPickerX
        {
            get { return X + 18; }
        }

        /// <summary>
        /// Gets the color picker y.
        /// </summary>
        /// <value>The color picker y.</value>
        public static int ColorPickerY
        {
            get { return Y + 61; }
        }

        /// <summary>
        /// Gets the color picker w.
        /// </summary>
        /// <value>The color picker w.</value>
        public static int ColorPickerW
        {
            get { return 252 - 18; }
        }

        /// <summary>
        /// Gets the color picker h.
        /// </summary>
        /// <value>The color picker h.</value>
        public static int ColorPickerH
        {
            get { return 282 - 61; }
        }

        /// <summary>
        /// Loads the color picker.
        /// </summary>
        /// <param name="onSelectcolor">The on selectcolor.</param>
        /// <param name="color">The color.</param>
        public static void Load(OnSelectColor onSelectcolor, Color color)
        {
            OnChangeColor = onSelectcolor;
            SColor = color;
            SHue = ((HSLColor)color).Hue;
            SSaturation = ((HSLColor)color).Saturation;

            LuminositySlider.Percent = (float)SColor.Luminosity / 100f;
            AlphaSlider.Percent = color.A / 255f;
            X = (Drawing.Width - BackgroundSprite.Width) / 2;
            Y = (Drawing.Height - BackgroundSprite.Height) / 2;

            Visible = true;
            UpdateLuminosityBitmap(color);
            UpdateOpacityBitmap(color);
            InitialColor = color;
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
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

        /// <summary>
        /// Fired when the game receives a window event.
        /// </summary>
        /// <param name="args">The <see cref="WndEventArgs"/> instance containing the event data.</param>
        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (!_visible)
            {
                return;
            }

            LuminositySlider.OnWndProc(args);
            AlphaSlider.OnWndProc(args);

            if (args.Msg == (uint)WindowsMessages.WM_LBUTTONDOWN)
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
            else if (args.Msg == (uint)WindowsMessages.WM_LBUTTONUP)
            {
                _moving = false;
                _selecting = false;
            }
            else if (args.Msg == (uint)WindowsMessages.WM_MOUSEMOVE)
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
                    X += (int)(pos.X - _prevPos.X);
                    Y += (int)(pos.Y - _prevPos.Y);
                }
                _prevPos = Utils.GetCursorPos();
            }
        }

        /// <summary>
        /// Updates the luminosity bitmap.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
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

        /// <summary>
        /// Updates the opacity bitmap.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
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

        /// <summary>
        /// Updates the color.
        /// </summary>
        private static void UpdateColor()
        {
            if (_selecting)
            {
                var pos = Utils.GetCursorPos();
                var color = BackgroundSprite.Bitmap.GetPixel((int)pos.X - X, (int)pos.Y - Y);
                SHue = ((HSLColor)color).Hue;
                SSaturation = ((HSLColor)color).Saturation;
                UpdateLuminosityBitmap(color);
            }

            SColor.Hue = SHue;
            SColor.Saturation = SSaturation;
            SColor.Luminosity = (LuminositySlider.Percent * 100d);
            var r = Color.FromArgb(((int)(AlphaSlider.Percent * 255)), SColor);
            PreviewRectangle.Color = new ColorBGRA(r.R, r.G, r.B, r.A);
            UpdateOpacityBitmap(r);
            FireOnChangeColor(r);
        }

        /// <summary>
        /// Fires the on change color event.
        /// </summary>
        /// <param name="color">The color.</param>
        public static void FireOnChangeColor(Color color)
        {
            if (OnChangeColor != null)
            {
                OnChangeColor(color);
            }
        }

        //From: https://richnewman.wordpress.com/about/code-listings-and-diagrams/hslcolor-class/

        /// <summary>
        /// A slider.
        /// </summary>
        public class CPSlider
        {
            /// <summary>
            /// The x
            /// </summary>
            private readonly int _x;

            /// <summary>
            /// The y
            /// </summary>
            private readonly int _y;

            /// <summary>
            /// The percent
            /// </summary>
            private float _percent;

            /// <summary>
            /// If this instance is visible or not
            /// </summary>
            private bool _visible = true;

            /// <summary>
            /// The active sprite
            /// </summary>
            internal Render.Sprite ActiveSprite;

            /// <summary>
            /// The height
            /// </summary>
            public int Height;

            /// <summary>
            /// The inactive sprite
            /// </summary>
            internal Render.Sprite InactiveSprite;

            /// <summary>
            /// If the slider is moving.
            /// </summary>
            public bool Moving;

            /// <summary>
            /// Initializes a new instance of the <see cref="CPSlider"/> class.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <param name="height">The height.</param>
            /// <param name="percent">The percent.</param>
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

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="CPSlider"/> is visible.
            /// </summary>
            /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
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

            /// <summary>
            /// Gets or sets the s x.
            /// </summary>
            /// <value>The s x.</value>
            public int sX
            {
                set
                {
                    ActiveSprite.X = sX;
                    InactiveSprite.X = sX;
                }
                get { return _x + X; }
            }

            /// <summary>
            /// Gets or sets the s y.
            /// </summary>
            /// <value>The s y.</value>
            public int sY
            {
                set
                {
                    ActiveSprite.Y = sY;
                    InactiveSprite.Y = sY;
                    ActiveSprite.Y = sY + (int)(Percent * Height);
                    InactiveSprite.Y = sY + (int)(Percent * Height);
                }
                get { return _y + Y; }
            }

            /// <summary>
            /// Gets the width.
            /// </summary>
            /// <value>The width.</value>
            public int Width
            {
                get { return Resources.CPActiveSlider.Width; }
            }

            /// <summary>
            /// Gets or sets the percent.
            /// </summary>
            /// <value>The percent.</value>
            public float Percent
            {
                get { return _percent; }
                set
                {
                    var newValue = Math.Max(0f, Math.Min(1f, value));
                    _percent = newValue;
                }
            }

            /// <summary>
            /// Updates the percent.
            /// </summary>
            private void UpdatePercent()
            {
                var pos = Utils.GetCursorPos();
                Percent = (pos.Y - Resources.CPActiveSlider.Height / 2 - sY) / Height;
                UpdateColor();
                ActiveSprite.Y = sY + (int)(Percent * Height);
                InactiveSprite.Y = sY + (int)(Percent * Height);
            }

            /// <summary>
            /// Handles the <see cref="E:WndProc" /> event.
            /// </summary>
            /// <param name="args">The <see cref="WndEventArgs"/> instance containing the event data.</param>
            public void OnWndProc(WndEventArgs args)
            {
                switch (args.Msg)
                {
                    case (uint)WindowsMessages.WM_LBUTTONDOWN:
                        var pos = Utils.GetCursorPos();
                        if (Utils.IsUnderRectangle(pos, sX, sY, Width, Height + Resources.CPActiveSlider.Height))
                        {
                            Moving = true;
                            ActiveSprite.Visible = Moving;
                            InactiveSprite.Visible = !Moving;
                            UpdatePercent();
                        }
                        break;
                    case (uint)WindowsMessages.WM_MOUSEMOVE:
                        if (Moving)
                        {
                            UpdatePercent();
                        }
                        break;
                    case (uint)WindowsMessages.WM_LBUTTONUP:
                        Moving = false;
                        ActiveSprite.Visible = Moving;
                        InactiveSprite.Visible = !Moving;
                        break;
                }
            }
        }

        /// <summary>
        /// A HSL color.
        /// </summary>
        public class HSLColor
        {
            //from: https://richnewman.wordpress.com/about/code-listings-and-diagrams/hslcolor-class/
            // Private data members below are on scale 0-1
            // They are scaled for use externally based on scale

            /// <summary>
            /// The scale
            /// </summary>
            private const double scale = 100.0;

            /// <summary>
            /// The hue
            /// </summary>
            private double hue = 1.0;

            /// <summary>
            /// The luminosity
            /// </summary>
            private double luminosity = 1.0;

            /// <summary>
            /// The saturation
            /// </summary>
            private double saturation = 1.0;

            /// <summary>
            /// Initializes a new instance of the <see cref="HSLColor"/> class.
            /// </summary>
            public HSLColor() { }

            /// <summary>
            /// Initializes a new instance of the <see cref="HSLColor"/> class.
            /// </summary>
            /// <param name="color">The color.</param>
            public HSLColor(Color color)
            {
                SetRGB(color.R, color.G, color.B);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="HSLColor"/> class.
            /// </summary>
            /// <param name="red">The red.</param>
            /// <param name="green">The green.</param>
            /// <param name="blue">The blue.</param>
            public HSLColor(int red, int green, int blue)
            {
                SetRGB(red, green, blue);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="HSLColor"/> class.
            /// </summary>
            /// <param name="hue">The hue.</param>
            /// <param name="saturation">The saturation.</param>
            /// <param name="luminosity">The luminosity.</param>
            public HSLColor(double hue, double saturation, double luminosity)
            {
                Hue = hue;
                Saturation = saturation;
                Luminosity = luminosity;
            }

            /// <summary>
            /// Gets or sets the hue.
            /// </summary>
            /// <value>The hue.</value>
            public double Hue
            {
                get { return hue * scale; }
                set { hue = CheckRange(value / scale); }
            }

            /// <summary>
            /// Gets or sets the saturation.
            /// </summary>
            /// <value>The saturation.</value>
            public double Saturation
            {
                get { return saturation * scale; }
                set { saturation = CheckRange(value / scale); }
            }

            /// <summary>
            /// Gets or sets the luminosity.
            /// </summary>
            /// <value>The luminosity.</value>
            public double Luminosity
            {
                get { return luminosity * scale; }
                set { luminosity = CheckRange(value / scale); }
            }

            /// <summary>
            /// Checks the range.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>System.Double.</returns>
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

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return String.Format("H: {0:#0.##} S: {1:#0.##} L: {2:#0.##}", Hue, Saturation, Luminosity);
            }

            /// <summary>
            /// To the RGB string.
            /// </summary>
            /// <returns>System.String.</returns>
            public string ToRGBString()
            {
                Color color = this;
                return String.Format("R: {0:#0.##} G: {1:#0.##} B: {2:#0.##}", color.R, color.G, color.B);
            }

            /// <summary>
            /// Sets the RGB.
            /// </summary>
            /// <param name="red">The red.</param>
            /// <param name="green">The green.</param>
            /// <param name="blue">The blue.</param>
            public void SetRGB(int red, int green, int blue)
            {
                HSLColor hslColor = Color.FromArgb(red, green, blue);
                hue = hslColor.hue;
                saturation = hslColor.saturation;
                luminosity = hslColor.luminosity;
            }

            #region Casts to/from System.Drawing.Color

            /// <summary>
            /// Performs an implicit conversion from <see cref="HSLColor"/> to <see cref="Color"/>.
            /// </summary>
            /// <param name="hslColor">Color of the HSL.</param>
            /// <returns>The result of the conversion.</returns>
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
                return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
            }

            /// <summary>
            /// Gets the color component.
            /// </summary>
            /// <param name="temp1">The temp1.</param>
            /// <param name="temp2">The temp2.</param>
            /// <param name="temp3">The temp3.</param>
            /// <returns>System.Double.</returns>
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

            /// <summary>
            /// Moves the into range.
            /// </summary>
            /// <param name="temp3">The temp3.</param>
            /// <returns>System.Double.</returns>
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

            /// <summary>
            /// Gets the temp2.
            /// </summary>
            /// <param name="hslColor">Color of the HSL.</param>
            /// <returns>System.Double.</returns>
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

            /// <summary>
            /// Performs an implicit conversion from <see cref="Color"/> to <see cref="HSLColor"/>.
            /// </summary>
            /// <param name="color">The color.</param>
            /// <returns>The result of the conversion.</returns>
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