namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;

    using SharpDX;
    using SharpDX.Direct3D9;

    using Color = SharpDX.Color;
    using Rectangle = SharpDX.Rectangle;

    /// <summary>
    ///     The menu item.
    /// </summary>
    public class MenuItem
    {
        #region Fields

        /// <summary>
        ///     The display name.
        /// </summary>
        public string DisplayName;

        /// <summary>
        ///     The font color.
        /// </summary>
        public ColorBGRA FontColor;

        /// <summary>
        ///     The font style.
        /// </summary>
        public FontStyle FontStyle;

        /// <summary>
        ///     The menu font size.
        /// </summary>
        public int MenuFontSize;

        /// <summary>
        ///     The name.
        /// </summary>
        public string Name;

        /// <summary>
        ///     The parent.
        /// </summary>
        public Menu Parent;

        /// <summary>
        ///     Indicates whether to show the item/
        /// </summary>
        public bool ShowItem;

        /// <summary>
        ///     The tag.
        /// </summary>
        public int Tag;

        /// <summary>
        ///     The tooltip.
        /// </summary>
        public string Tooltip;

        /// <summary>
        ///     The tooltip color.
        /// </summary>
        public Color TooltipColor;

        /// <summary>
        ///     Indicates whether the value was set.
        /// </summary>
        public bool ValueSet;

        /// <summary>
        ///     Indicates whether the menu item is drawing the tooltip.
        /// </summary>
        internal bool DrawingTooltip;

        /// <summary>
        ///     Indicates whether the menu item is being interacted with.
        /// </summary>
        internal bool Interacting;

        /// <summary>
        ///     The stage of the KeybindSetting
        /// </summary>
        internal KeybindSetStage KeybindSettingStage = KeybindSetStage.NotSetting;

        /// <summary>
        ///     The value type.
        /// </summary>
        internal MenuValueType ValueType;

        /// <summary>
        ///     The configuration name.
        /// </summary>
        private readonly string configName;

        /// <summary>
        ///     Indicates whether the menu item won't save.
        /// </summary>
        private bool dontSave;

        /// <summary>
        ///     Indicates whether the menu item is shared.
        /// </summary>
        private bool isShared;

        /// <summary>
        ///     Indicates whether the menu item is visible.
        /// </summary>
        private bool isVisible;

        /// <summary>
        ///     The serialized data.
        /// </summary>
        private byte[] serialized;

        /// <summary>
        ///     The value.
        /// </summary>
        private object value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Menu" /> class.
        /// </summary>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="displayName">
        ///     The display name.
        /// </param>
        /// <param name="championUnique">
        ///     Indicates whether the menu item is champion unique.
        /// </param>
        public MenuItem(string name, string displayName, bool championUnique = false)
        {
            if (championUnique)
            {
                name = ObjectManager.Player.ChampionName + name;
            }

            this.Name = name;
            this.DisplayName = MenuGlobals.Function001(displayName);
            this.FontStyle = FontStyle.Regular;
            this.FontColor = Color.White;
            this.ShowItem = true;
            this.Tag = 0;
            this.configName = Assembly.GetCallingAssembly().GetName().Name
                              + Assembly.GetCallingAssembly().GetType().GUID;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     The value changed event.
        /// </summary>
        public event EventHandler<OnValueChangeEventArgs> ValueChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the tooltip duration.
        /// </summary>
        public int TooltipDuration
        {
            get
            {
                return CommonMenu.Instance.Item("LeagueSharp.Common.TooltipDuration").GetValue<Slider>().Value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the height.
        /// </summary>
        internal int Height
        {
            get
            {
                return MenuSettings.MenuItemHeight;
            }
        }

        /// <summary>
        ///     Gets the item base position.
        /// </summary>
        internal Vector2 MyBasePosition
        {
            get
            {
                if (Menu.IsCompact || this.Parent == null)
                {
                    return MenuSettings.BasePosition;
                }

                return this.Parent.MyBasePosition;
            }
        }

        /// <summary>
        ///     Gets the needed width.
        /// </summary>
        internal int NeededWidth
        {
            get
            {
                return MenuDrawHelper.Font.MeasureText(MultiLanguage._(this.DisplayName)).Width + this.Height * 2 + 10
                       + ((this.ValueType == MenuValueType.StringList)
                              ? this.GetValue<StringList>()
                                    .SList.Select(v => MenuDrawHelper.Font.MeasureText(v).Width + 25)
                                    .Concat(new[] { 0 })
                                    .Max()
                              : (this.ValueType == MenuValueType.KeyBind)
                                    ? this.GetValue<KeyBind>().SecondaryKey == 0
                                          ? MenuDrawHelper.Font.MeasureText(
                                              " [" + Utils.KeyToText(this.GetValue<KeyBind>().Key) + "]").Width
                                          : MenuDrawHelper.Font.MeasureText(
                                              " [" + Utils.KeyToText(this.GetValue<KeyBind>().Key) + "]").Width
                                            + MenuDrawHelper.Font.MeasureText(
                                                " [" + Utils.KeyToText(this.GetValue<KeyBind>().SecondaryKey) + "]")
                                                  .Width
                                            + MenuDrawHelper.Font.MeasureText(
                                                " [" + Utils.KeyToText(this.GetValue<KeyBind>().Key) + "]").Width / 4
                                    : 0);
            }
        }

        /// <summary>
        ///     Gets the position.
        /// </summary>
        internal Vector2 Position
        {
            get
            {
                var xOffset = 0;

                if (this.Parent != null)
                {
                    xOffset = (int)(this.Parent.Position.X + this.Parent.Width);
                }

                return new Vector2(0, this.MyBasePosition.Y) + new Vector2(xOffset, 0)
                       + this.YLevel * new Vector2(0, MenuSettings.MenuItemHeight);
            }
        }

        /// <summary>
        ///     Gets the save file name.
        /// </summary>
        internal string SaveFileName
        {
            get
            {
                return this.isShared ? "SharedConfig" : this.configName;
            }
        }

        /// <summary>
        ///     Gets the save key.
        /// </summary>
        internal string SaveKey
        {
            get
            {
                return Utils.Md5Hash("v3" + this.DisplayName + this.Name);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the item is visible.
        /// </summary>
        internal bool Visible
        {
            get
            {
                return MenuSettings.DrawMenu && this.isVisible && this.ShowItem;
            }

            set
            {
                this.isVisible = value;
            }
        }

        /// <summary>
        ///     Gets the width.
        /// </summary>
        internal int Width
        {
            get
            {
                return this.Parent != null ? this.Parent.ChildrenMenuWidth : MenuSettings.MenuItemWidth;
            }
        }

        /// <summary>
        ///     Gets the Y level.
        /// </summary>
        internal int YLevel
        {
            get
            {
                if (this.Parent == null)
                {
                    return 0;
                }

                return (Menu.IsCompact ? 0 : this.Parent.YLevel) + this.Parent.Children.Count
                       + this.Parent.Items.TakeWhile(test => test.Name != this.Name).Count(c => c.ShowItem);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Sets the menu item to not save.
        /// </summary>
        /// <returns>
        ///     The item instance.
        /// </returns>
        public MenuItem DontSave()
        {
            this.dontSave = true;
            return this;
        }

        /// <summary>
        ///     Gets the item value.
        /// </summary>
        /// <typeparam name="T">
        ///     The item type.
        /// </typeparam>
        /// <returns>
        ///     The value.
        /// </returns>
        public T GetValue<T>()
        {
            return (T)this.value;
        }

        /// <summary>
        ///     Gets a value indicating whether the item is active.
        /// </summary>
        /// <returns>
        ///     Value indicating whether the item is active.
        /// </returns>
        public bool IsActive()
        {
            switch (this.ValueType)
            {
                case MenuValueType.Boolean:
                    return this.GetValue<bool>();
                case MenuValueType.Circle:
                    return this.GetValue<Circle>().Active;
                case MenuValueType.KeyBind:
                    return this.GetValue<KeyBind>().Active;
                default:
                    return false;
            }
        }

        /// <summary>
        ///     Sets the font style.
        /// </summary>
        /// <param name="fontStyle">
        ///     The font style.
        /// </param>
        /// <param name="fontColor">
        ///     The font color.
        /// </param>
        /// <returns>
        ///     The item instance.
        /// </returns>
        public MenuItem SetFontStyle(FontStyle fontStyle = FontStyle.Regular, Color? fontColor = null)
        {
            this.FontStyle = fontStyle;
            this.FontColor = fontColor ?? Color.White;

            return this;
        }

        /// <summary>
        ///     Sets the menu item to be shared.
        /// </summary>
        /// <returns>
        ///     The item instance.
        /// </returns>
        public MenuItem SetShared()
        {
            this.isShared = true;

            return this;
        }

        /// <summary>
        ///     Sets the menu item tag.
        /// </summary>
        /// <param name="tag">
        ///     The tag.
        /// </param>
        /// <returns>
        ///     The item instance.
        /// </returns>
        public MenuItem SetTag(int tag = 0)
        {
            this.Tag = tag;

            return this;
        }

        /// <summary>
        ///     Sets the tooltip.
        /// </summary>
        /// <param name="tooltip">
        ///     The tooltip string.
        /// </param>
        /// <param name="tooltipColor">
        ///     The tooltip color.
        /// </param>
        /// <returns>
        ///     The menu instance.
        /// </returns>
        public MenuItem SetTooltip(string tooltip, Color? tooltipColor = null)
        {
            this.Tooltip = tooltip;
            this.TooltipColor = tooltipColor ?? Color.White;

            return this;
        }

        /// <summary>
        ///     Sets the value.
        /// </summary>
        /// <typeparam name="T">
        ///     The value type.
        /// </typeparam>
        /// <param name="newValue">
        ///     The new value.
        /// </param>
        /// <returns>
        ///     The item instance.
        /// </returns>
        public MenuItem SetValue<T>(T newValue)
        {
            this.ValueType = MenuValueType.None;
            if (newValue.GetType().ToString().Contains("Boolean"))
            {
                this.ValueType = MenuValueType.Boolean;
            }
            else if (newValue.GetType().ToString().Contains("Slider"))
            {
                this.ValueType = MenuValueType.Slider;
            }
            else if (newValue.GetType().ToString().Contains("KeyBind"))
            {
                this.ValueType = MenuValueType.KeyBind;
            }
            else if (newValue.GetType().ToString().Contains("Int"))
            {
                this.ValueType = MenuValueType.Integer;
            }
            else if (newValue.GetType().ToString().Contains("Circle"))
            {
                this.ValueType = MenuValueType.Circle;
            }
            else if (newValue.GetType().ToString().Contains("StringList"))
            {
                this.ValueType = MenuValueType.StringList;
            }
            else if (newValue.GetType().ToString().Contains("Color"))
            {
                this.ValueType = MenuValueType.Color;
            }
            else
            {
                Console.WriteLine(@"CommonLibMenu: Data type not supported");
            }

            var readBytes = SavedSettings.GetSavedData(this.SaveFileName, this.SaveKey);
            var v = newValue;

            try
            {
                if (!this.ValueSet && readBytes != null)
                {
                    switch (this.ValueType)
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
                            if (savedSliderValue.MinValue == newSliderValue.MinValue
                                && savedSliderValue.MaxValue == newSliderValue.MaxValue)
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

            if (this.ValueSet)
            {
                var handler = this.ValueChanged;
                if (handler != null)
                {
                    valueChangedEvent = new OnValueChangeEventArgs(this.value, newValue);
                    handler(this, valueChangedEvent);
                }
            }

            if (valueChangedEvent != null)
            {
                if (valueChangedEvent.Process)
                {
                    this.value = newValue;
                }
            }
            else
            {
                this.value = newValue;
            }

            this.ValueSet = true;
            this.serialized = Utils.Serialize(this.value);
            return this;
        }

        /// <summary>
        ///     Shows the item.
        /// </summary>
        /// <param name="showItem">
        ///     Indicates whether to show the item.
        /// </param>
        /// <returns>
        ///     The item instance.
        /// </returns>
        public MenuItem Show(bool showItem = true)
        {
            this.ShowItem = showItem;

            return this;
        }

        /// <summary>
        ///     Show the tooltip.
        /// </summary>
        /// <param name="hide"></param>
        public void ShowTooltip(bool hide = false)
        {
            if (!string.IsNullOrEmpty(this.Tooltip))
            {
                this.DrawingTooltip = !hide;
            }
        }

        /// <summary>
        ///     Shows the tooltip notification.
        /// </summary>
        public void ShowTooltipNotification()
        {
            if (!string.IsNullOrEmpty(this.Tooltip))
            {
                var notif = new Notification(this.Tooltip).SetTextColor(System.Drawing.Color.White);
                Notifications.AddNotification(notif);
                Utility.DelayAction.Add(this.TooltipDuration, () => notif.Dispose());
            }
        }

        /// <summary>
        ///     Gets the item value, safely.
        /// </summary>
        /// <typeparam name="T">
        ///     The item type.
        /// </typeparam>
        /// <returns>
        ///     The value.
        /// </returns>
        public T TryGetValue<T>()
        {
            return this.value is T ? (T)this.value : default(T);
        }

        /// <summary>
        ///     Gets a value indicating if the value is of item type.
        /// </summary>
        /// <typeparam name="T">
        ///     The item type.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="bool" /> indicating whether the value is of type.
        /// </returns>
        public bool TypeOf<T>()
        {
            return this.value is T;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Indicates whether the position is inside the menu item.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        internal bool IsInside(Vector2 position)
        {
            return Utils.IsUnderRectangle(
                position,
                this.Position.X,
                this.Position.Y,
                !string.IsNullOrEmpty(this.Tooltip) ? this.Width + this.Height : this.Width,
                this.Height);
        }

        /// <summary>
        ///     On draw event.
        /// </summary>
        internal void OnDraw()
        {
            var s = MultiLanguage._(this.DisplayName);

            MenuDrawHelper.DrawBox(
                this.Position,
                this.Width,
                this.Height,
                MenuSettings.BackgroundColor,
                1,
                System.Drawing.Color.Black);

            if (this.DrawingTooltip)
            {
                MenuDrawHelper.DrawToolTipText(
                    new Vector2(this.Position.X + this.Width, this.Position.Y),
                    this,
                    this.TooltipColor);
            }

            var style = this.FontStyle;
            style &= ~FontStyle.Strikeout;
            style &= ~FontStyle.Underline;

            var font = MenuDrawHelper.GetFont(style);

            switch (this.ValueType)
            {
                case MenuValueType.Boolean:
                    MenuDrawHelper.DrawOnOff(
                        this.GetValue<bool>(),
                        new Vector2(this.Position.X + this.Width - this.Height, this.Position.Y),
                        this);
                    break;

                case MenuValueType.Slider:
                    MenuDrawHelper.DrawSlider(this.Position, this);
                    break;

                case MenuValueType.KeyBind:
                    var val = this.GetValue<KeyBind>();

                    if (this.Interacting)
                    {
                        s = MultiLanguage._("Press new key(s)");
                    }

                    if (val.Key != 0)
                    {
                        var x = !string.IsNullOrEmpty(this.Tooltip)
                                    ? (int)this.Position.X + this.Width - this.Height
                                      - font.MeasureText("[" + Utils.KeyToText(val.Key) + "]").Width - 35
                                    : (int)this.Position.X + this.Width - this.Height
                                      - font.MeasureText("[" + Utils.KeyToText(val.Key) + "]").Width - 10;

                        font.DrawText(
                            null,
                            "[" + Utils.KeyToText(val.Key) + "]",
                            new Rectangle(x, (int)this.Position.Y, this.Width, this.Height),
                            FontDrawFlags.VerticalCenter,
                            new ColorBGRA(1, 169, 234, 255));
                    }

                    if (val.SecondaryKey != 0)
                    {
                        var x_secondary = !string.IsNullOrEmpty(this.Tooltip)
                                              ? (int)this.Position.X + this.Width - this.Height
                                                - font.MeasureText("[" + Utils.KeyToText(val.Key) + "]").Width
                                                - font.MeasureText("[" + Utils.KeyToText(val.Key) + "]").Width / 4
                                                - font.MeasureText("[" + Utils.KeyToText(val.SecondaryKey) + "]").Width
                                                - 35
                                              : (int)this.Position.X + this.Width - this.Height
                                                - font.MeasureText("[" + Utils.KeyToText(val.Key) + "]").Width
                                                - font.MeasureText("[" + Utils.KeyToText(val.Key) + "]").Width / 4
                                                - font.MeasureText("[" + Utils.KeyToText(val.SecondaryKey) + "]").Width
                                                - 10;

                        font.DrawText(
                            null,
                            "[" + Utils.KeyToText(val.SecondaryKey) + "]",
                            new Rectangle(x_secondary, (int)this.Position.Y, this.Width, this.Height),
                            FontDrawFlags.VerticalCenter,
                            new ColorBGRA(1, 169, 234, 255));
                    }

                    MenuDrawHelper.DrawOnOff(
                        val.Active,
                        new Vector2(this.Position.X + this.Width - this.Height, this.Position.Y),
                        this);

                    break;

                case MenuValueType.Integer:
                    var intVal = this.GetValue<int>();
                    MenuDrawHelper.Font.DrawText(
                        null,
                        intVal.ToString(),
                        new Rectangle((int)this.Position.X + 5, (int)this.Position.Y, this.Width, this.Height),
                        FontDrawFlags.VerticalCenter | FontDrawFlags.Right,
                        new ColorBGRA(255, 255, 255, 255));
                    break;

                case MenuValueType.Color:
                    var colorVal = this.GetValue<System.Drawing.Color>();
                    MenuDrawHelper.DrawBox(
                        this.Position + new Vector2(this.Width - this.Height, 0),
                        this.Height,
                        this.Height,
                        colorVal,
                        1,
                        System.Drawing.Color.Black);
                    break;

                case MenuValueType.Circle:
                    var circleVal = this.GetValue<Circle>();
                    MenuDrawHelper.DrawBox(
                        this.Position + new Vector2(this.Width - this.Height * 2, 0),
                        this.Height,
                        this.Height,
                        circleVal.Color,
                        1,
                        System.Drawing.Color.Black);
                    MenuDrawHelper.DrawOnOff(
                        circleVal.Active,
                        new Vector2(this.Position.X + this.Width - this.Height, this.Position.Y),
                        this);
                    break;

                case MenuValueType.StringList:
                    var slVal = this.GetValue<StringList>();

                    var t = slVal.SList[slVal.SelectedIndex];

                    MenuDrawHelper.DrawArrow(
                        "<<",
                        this.Position + new Vector2(this.Width - this.Height * 2, 0),
                        this,
                        System.Drawing.Color.Black);
                    MenuDrawHelper.DrawArrow(
                        ">>",
                        this.Position + new Vector2(this.Width - this.Height, 0),
                        this,
                        System.Drawing.Color.Black);

                    MenuDrawHelper.Font.DrawText(
                        null,
                        MultiLanguage._(t),
                        new Rectangle(
                            (int)this.Position.X - 5 - 2 * this.Height,
                            (int)this.Position.Y,
                            this.Width,
                            this.Height),
                        FontDrawFlags.VerticalCenter | FontDrawFlags.Right,
                        new ColorBGRA(255, 255, 255, 255));
                    break;
            }

            if (!string.IsNullOrEmpty(this.Tooltip))
            {
                MenuDrawHelper.DrawToolTipButton(new Vector2(this.Position.X + this.Width, this.Position.Y), this);
            }

            font.DrawText(
                null,
                s,
                new Rectangle((int)this.Position.X + 5, (int)this.Position.Y, this.Width, this.Height),
                FontDrawFlags.VerticalCenter,
                this.FontColor);

            var textWidth = font.MeasureText(null, MultiLanguage._(this.DisplayName));
            if ((this.FontStyle & FontStyle.Strikeout) != 0)
            {
                Drawing.DrawLine(
                    this.Position.X + 5,
                    this.Position.Y + (MenuSettings.MenuItemHeight / 2f),
                    this.Position.X + 5 + textWidth.Width,
                    this.Position.Y + (MenuSettings.MenuItemHeight / 2f),
                    1f,
                    System.Drawing.Color.FromArgb(
                        this.FontColor.A,
                        this.FontColor.R,
                        this.FontColor.G,
                        this.FontColor.B));
            }

            if ((this.FontStyle & FontStyle.Underline) != 0)
            {
                Drawing.DrawLine(
                    this.Position.X + 5,
                    this.Position.Y + (MenuSettings.MenuItemHeight / 1.5f),
                    this.Position.X + 5 + textWidth.Width,
                    this.Position.Y + (MenuSettings.MenuItemHeight / 1.5f),
                    1f,
                    System.Drawing.Color.FromArgb(
                        this.FontColor.A,
                        this.FontColor.R,
                        this.FontColor.G,
                        this.FontColor.B));
            }
        }

        /// <summary>
        ///     Called when the game receives a window message.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="cursorPos">
        ///     The cursor position.
        /// </param>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="wndArgs">
        ///     The windows arguments.
        /// </param>
        internal void OnReceiveMessage(
            WindowsMessages message,
            Vector2 cursorPos,
            uint key,
            WndEventComposition wndArgs)
        {
            if (message == WindowsMessages.WM_MOUSEMOVE)
            {
                if (this.Visible && this.IsInside(cursorPos))
                {
                    if (cursorPos.X > this.Position.X + this.Width - 67
                        && cursorPos.X < this.Position.X + this.Width - 67 + this.Height + 8)
                    {
                        this.ShowTooltip();
                    }
                }
                else
                {
                    this.ShowTooltip(true);
                }
            }

            switch (this.ValueType)
            {
                case MenuValueType.Boolean:
                    if (message != WindowsMessages.WM_LBUTTONDOWN)
                    {
                        return;
                    }

                    if (!this.Visible)
                    {
                        return;
                    }

                    if (!this.IsInside(cursorPos))
                    {
                        return;
                    }

                    if (cursorPos.X > this.Position.X + this.Width)
                    {
                        break;
                    }

                    if (cursorPos.X > this.Position.X + this.Width - this.Height)
                    {
                        this.SetValue(!this.GetValue<bool>());
                    }

                    break;
                case MenuValueType.Slider:
                    if (!this.Visible)
                    {
                        this.Interacting = false;
                        return;
                    }

                    if (message == WindowsMessages.WM_MOUSEMOVE && this.Interacting
                        || message == WindowsMessages.WM_LBUTTONDOWN && !this.Interacting && this.IsInside(cursorPos))
                    {
                        var val = this.GetValue<Slider>();
                        var t = val.MinValue
                                + ((cursorPos.X - this.Position.X) * (val.MaxValue - val.MinValue)) / this.Width;
                        val.Value = (int)t;
                        this.SetValue(val);
                    }

                    if (message != WindowsMessages.WM_LBUTTONDOWN && message != WindowsMessages.WM_LBUTTONUP)
                    {
                        return;
                    }

                    if (!this.IsInside(cursorPos) && message == WindowsMessages.WM_LBUTTONDOWN)
                    {
                        return;
                    }

                    this.Interacting = message == WindowsMessages.WM_LBUTTONDOWN;
                    break;
                case MenuValueType.Color:
                    if (message != WindowsMessages.WM_LBUTTONDOWN)
                    {
                        return;
                    }

                    if (!this.Visible)
                    {
                        return;
                    }

                    if (!this.IsInside(cursorPos))
                    {
                        return;
                    }

                    if (cursorPos.X > this.Position.X + this.Width)
                    {
                        break;
                    }

                    if (cursorPos.X > this.Position.X + this.Width - this.Height)
                    {
                        var c = this.GetValue<System.Drawing.Color>();
                        ColorPicker.Load(delegate(System.Drawing.Color args) { this.SetValue(args); }, c);
                    }

                    break;
                case MenuValueType.Circle:
                    if (message != WindowsMessages.WM_LBUTTONDOWN)
                    {
                        return;
                    }

                    if (!this.Visible)
                    {
                        return;
                    }

                    if (!this.IsInside(cursorPos))
                    {
                        return;
                    }

                    if (cursorPos.X > this.Position.X + this.Width)
                    {
                        break;
                    }

                    if (cursorPos.X - this.Position.X > this.Width - this.Height)
                    {
                        var val = this.GetValue<Circle>();
                        val.Active = !val.Active;
                        this.SetValue(val);
                    }
                    else if (cursorPos.X - this.Position.X > this.Width - 2 * this.Height)
                    {
                        var c = this.GetValue<Circle>();
                        ColorPicker.Load(
                            delegate(System.Drawing.Color args)
                                {
                                    var val = this.GetValue<Circle>();
                                    val.Color = args;
                                    this.SetValue(val);
                                },
                            c.Color);
                    }

                    break;
                case MenuValueType.KeyBind:
                    if (!MenuGUI.IsChatOpen && !MenuGUI.IsShopOpen)
                    {
                        switch (message)
                        {
                            case WindowsMessages.WM_KEYDOWN:
                                var val = this.GetValue<KeyBind>();
                                if (key == val.Key || key == val.SecondaryKey)
                                {
                                    if (val.Type == KeyBindType.Press)
                                    {
                                        if (!val.Active)
                                        {
                                            val.Active = true;
                                            this.SetValue(val);
                                        }
                                    }
                                }
                                break;
                            case WindowsMessages.WM_KEYUP:

                                var val2 = this.GetValue<KeyBind>();
                                if (key == val2.Key || key == val2.SecondaryKey)
                                {
                                    if (val2.Type == KeyBindType.Press)
                                    {
                                        val2.Active = false;
                                        this.SetValue(val2);
                                    }
                                    else
                                    {
                                        val2.Active = !val2.Active;
                                        this.SetValue(val2);
                                    }
                                }
                                break;
                        }
                    }

                    if (key == 8 && message == WindowsMessages.WM_KEYUP && this.Interacting)
                    {
                        var val = this.GetValue<KeyBind>();
                        val.Key = 0;
                        val.SecondaryKey = 0;
                        this.SetValue(val);
                        this.Interacting = false;
                        this.KeybindSettingStage = KeybindSetStage.NotSetting;
                    }

                    if (message == WindowsMessages.WM_KEYUP && this.Interacting
                        && this.KeybindSettingStage != KeybindSetStage.NotSetting)
                    {
                        if (this.KeybindSettingStage == KeybindSetStage.Keybind1)
                        {
                            var val = this.GetValue<KeyBind>();
                            val.Key = key;
                            this.SetValue(val);
                            this.KeybindSettingStage = KeybindSetStage.Keybind2;
                        }
                        else if (this.KeybindSettingStage == KeybindSetStage.Keybind2)
                        {
                            var val = this.GetValue<KeyBind>();
                            val.SecondaryKey = key;
                            this.SetValue(val);
                            this.Interacting = false;
                            this.KeybindSettingStage = KeybindSetStage.NotSetting;
                        }
                    }

                    if (message == WindowsMessages.WM_KEYUP && this.Interacting
                        && this.KeybindSettingStage == KeybindSetStage.NotSetting)
                    {
                        var val = this.GetValue<KeyBind>();
                        val.Key = key;
                        val.SecondaryKey = 0;
                        this.SetValue(val);
                        this.Interacting = false;
                    }

                    if (!this.Visible)
                    {
                        return;
                    }

                    if (message != WindowsMessages.WM_LBUTTONDOWN && wndArgs.Msg != WindowsMessages.WM_RBUTTONDOWN)
                    {
                        return;
                    }

                    if (!this.IsInside(cursorPos))
                    {
                        return;
                    }

                    if (cursorPos.X > this.Position.X + this.Width)
                    {
                        break;
                    }

                    if (cursorPos.X > this.Position.X + this.Width - this.Height)
                    {
                        var val = this.GetValue<KeyBind>();
                        val.Active = !val.Active;
                        this.SetValue(val);
                    }
                    else
                    {
                        if (wndArgs.Msg == WindowsMessages.WM_RBUTTONDOWN)
                        {
                            this.KeybindSettingStage = KeybindSetStage.Keybind1;
                        }
                        //this.Stage = KeybindSetStage.NotSetting;
                        this.Interacting = !this.Interacting;
                    }

                    break;
                case MenuValueType.StringList:
                    if (!this.Visible)
                    {
                        return;
                    }

                    if (message != WindowsMessages.WM_LBUTTONDOWN)
                    {
                        return;
                    }

                    if (!this.IsInside(cursorPos))
                    {
                        return;
                    }

                    if (cursorPos.X > this.Position.X + this.Width)
                    {
                        break;
                    }

                    var slVal = this.GetValue<StringList>();
                    if (cursorPos.X > this.Position.X + this.Width - this.Height)
                    {
                        slVal.SelectedIndex = slVal.SelectedIndex == slVal.SList.Length - 1
                                                  ? 0
                                                  : (slVal.SelectedIndex + 1);
                        this.SetValue(slVal);
                    }
                    else if (cursorPos.X > this.Position.X + this.Width - 2 * this.Height)
                    {
                        slVal.SelectedIndex = slVal.SelectedIndex == 0
                                                  ? slVal.SList.Length - 1
                                                  : (slVal.SelectedIndex - 1);
                        this.SetValue(slVal);
                    }

                    break;
            }
        }

        /// <summary>
        ///     Save to file.
        /// </summary>
        /// <param name="dics">
        ///     Data collection.
        /// </param>
        internal void SaveToFile(ref Dictionary<string, Dictionary<string, byte[]>> dics)
        {
            if (!this.dontSave)
            {
                if (!dics.ContainsKey(this.SaveFileName))
                {
                    dics[this.SaveFileName] = new Dictionary<string, byte[]>();
                }

                dics[this.SaveFileName][this.SaveKey] = this.serialized;
            }
        }

        #endregion
    }
}