namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SharpDX;
    using SharpDX.Direct3D9;

    /// <summary>
    ///     The PermaShow class allows you to add important items to permashow easily.
    /// </summary>
    public static class PermaShow
    {
        #region Static Fields

        /// <summary>
        ///     The default perma show width
        /// </summary>
        private static readonly float DefaultPermaShowWidth = 170f;

        /// <summary>
        ///     The default small box width
        /// </summary>
        private static readonly float DefaultSmallBoxWidth = 45f;

        /// <summary>
        ///     List of items for PermaShow
        /// </summary>
        private static readonly List<PermaShowItem> PermaShowItems = new List<PermaShowItem>();

        /// <summary>
        ///     The x factor
        /// </summary>
        private static readonly float XFactor = Drawing.Width / 1366f;

        /// <summary>
        ///     The y factor
        /// </summary>
        private static readonly float YFactor = Drawing.Height / 768f;

        /// <summary>
        ///     The line for the box.
        /// </summary>
        private static Line BoxLine;

        /// <summary>
        ///     The box position
        /// </summary>
        private static Vector2 BoxPosition;

        /// <summary>
        ///     The default position
        /// </summary>
        private static Vector2 DefaultPosition = new Vector2(
            Drawing.Width - (0.79f * Drawing.Width),
            Drawing.Height - (0.98f * Drawing.Height));

        /// <summary>
        ///     The dragging
        /// </summary>
        private static bool Dragging;

        /// <summary>
        ///     The menu to save position and bool for moving the menu
        /// </summary>
        private static Menu placetosave;

        /// <summary>
        ///     The sprite
        /// </summary>
        private static Sprite sprite;

        /// <summary>
        ///     <c>true</c> if this instance has subscribed to the L# events.
        /// </summary>
        private static bool subbed;

        /// <summary>
        ///     The font for text.
        /// </summary>
        private static Font Text;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="PermaShow" /> class.
        /// </summary>
        static PermaShow()
        {
            CreateMenu();
            BoxPosition = GetPosition();
            PrepareDrawing();
        }

        #endregion

        #region Enums

        /// <summary>
        ///     Represents a direction.
        /// </summary>
        private enum Direction
        {
            /// <summary>
            ///     The X direction. (Horizontal)
            /// </summary>
            X,

            /// <summary>
            ///     The Y direction. (Vertical)
            /// </summary>
            Y
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the width of the perma show.
        /// </summary>
        /// <value>The width of the perma show.</value>
        private static float PermaShowWidth
        {
            get
            {
                return ScaleValue(placetosave.Item("bwidth").GetValue<Slider>().Value, Direction.X);
            }
        }

        /// <summary>
        ///     Gets the width of the small box.
        /// </summary>
        /// <value>The width of the small box.</value>
        private static float SmallBoxWidth
        {
            get
            {
                return ScaleValue(placetosave.Item("swidth").GetValue<Slider>().Value, Direction.X);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds a menuitem to PermaShow, can be used without any arguements or with if you want to customize. The bool can be
        ///     set to false to remove the item from permashow.
        ///     When removing, you can simply set the bool parameter to false and everything else can be null. The default color is
        ///     White.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="enabled">if set to <c>true</c> the instance will be enabled.</param>
        /// <param name="customdisplayname">The customdisplayname.</param>
        /// <param name="col">The color.</param>
        public static void Permashow(
            this MenuItem item,
            bool enabled = true,
            string customdisplayname = null,
            Color? col = null)
        {
            if (!IsEnabled())
            {
                return;
            }
            if (enabled && PermaShowItems.All(x => x.Item != item))
            {
                if (!PermaShowItems.Any())
                {
                    Sub();
                }
                var dispName = customdisplayname ?? item.DisplayName;
                Color? color = col ?? new ColorBGRA(255, 255, 255, 255);
                PermaShowItems.Add(
                    new PermaShowItem
                        { DisplayName = dispName, Item = item, ItemType = item.ValueType, Color = (Color)color });
            }

            else if (!enabled)
            {
                var itemtoremove = PermaShowItems.FirstOrDefault(x => x.Item == item);
                if (itemtoremove != null)
                {
                    PermaShowItems.Remove(itemtoremove);
                    if (!PermaShowItems.Any())
                    {
                        Unsub();
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Create the menu
        /// </summary>
        private static void CreateMenu()
        {
            placetosave = new Menu("PermaShow", "Permashow");
            var enablepermashow = new MenuItem("enablepermashow", "Enable PermaShow").SetValue(true);
            placetosave.AddItem(enablepermashow);
            var xvalue = new MenuItem("X", "X").SetValue(new Slider((int)DefaultPosition.X, 0, Drawing.Width));
            var yvalue = new MenuItem("Y", "Y").SetValue(new Slider((int)DefaultPosition.Y, 0, Drawing.Height));
            xvalue.ShowItem = false;
            yvalue.ShowItem = false;
            placetosave.AddItem(xvalue);
            placetosave.AddItem(yvalue);

            var bigwidth = new MenuItem("bwidth", "Width").SetValue(new Slider((int)DefaultPermaShowWidth, 100, 400));
            var smallwidth =
                new MenuItem("swidth", "Indicator Width").SetValue(new Slider((int)DefaultSmallBoxWidth, 30, 90));

            var moveable = new MenuItem("moveable", "Moveable").SetValue(true);

            placetosave.AddItem(moveable);
            placetosave.AddItem(bigwidth);
            placetosave.AddItem(smallwidth);

            var def = new MenuItem("defaults", "Default").SetValue(false);
            def.ValueChanged += (sender, args) =>
                {
                    if (args.GetNewValue<bool>())
                    {
                        bigwidth.SetValue(new Slider((int)DefaultPermaShowWidth, 100, 400));
                        smallwidth.SetValue(new Slider((int)DefaultSmallBoxWidth, 30, 90));
                    }
                };

            placetosave.AddItem(def);

            CommonMenu.Instance.AddSubMenu(placetosave);

            enablepermashow.ValueChanged += (sender, args) =>
                {
                    if (args.GetNewValue<bool>() && !subbed)
                    {
                        Sub();
                    }
                    else if (!args.GetNewValue<bool>())
                    {
                        Unsub();
                    }
                };
        }

        /// <summary>
        ///     Fired when the AppDomain is unloaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void CurrentDomainOnDomainUnload(object sender, EventArgs eventArgs)
        {
            BoxLine.OnLostDevice();
            Text.OnLostDevice();
            sprite.OnLostDevice();
            sprite.Dispose();
            Text.Dispose();
            BoxLine.Dispose();
        }

        /// <summary>
        ///     Draws a colored box to indicate booleans true or false with green and red respectively
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <param name="ison">if set to <c>true</c> [ison].</param>
        private static void DrawBox(Vector2 pos, bool ison)
        {
            BoxLine.Width = SmallBoxWidth;

            BoxLine.Begin();

            var positions = new[]
                                {
                                    new Vector2(pos.X, pos.Y),
                                    new Vector2(pos.X, pos.Y + (Text.Description.Height * 1.2f))
                                };

            var col = ison ? Color.DarkGreen : Color.DarkRed;
            col.A = 150;
            BoxLine.Draw(positions, col);

            BoxLine.End();
        }

        /// <summary>
        ///     Fired when everything has been drawn.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="Exception">[PermaShow] - MenuItem not supported</exception>
        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (Drawing.Direct3DDevice == null || Drawing.Direct3DDevice.IsDisposed)
            {
                return;
            }

            if (!placetosave.Item("enablepermashow").GetValue<bool>())
            {
                Unsub();
                return;
            }

            PermaArea();

            var halfwidth = 0.96f * (PermaShowWidth / 2);

            var baseposition = new Vector2(BoxPosition.X - halfwidth, BoxPosition.Y);

            var boxx = BoxPosition.X + (PermaShowWidth / 2) - (SmallBoxWidth / 2);

            foreach (var permaitem in PermaShowItems)
            {
                var index = PermaShowItems.IndexOf(permaitem);
                var boxpos = new Vector2(boxx, baseposition.Y + (Text.Description.Height * 1.2f * index));
                var endpos = new Vector2(
                    BoxPosition.X + (PermaShowWidth / 2),
                    baseposition.Y + (Text.Description.Height * 1.2f * index));
                var itempos = new Vector2(baseposition.X, baseposition.Y + (Text.Description.Height * 1.2f * index));

                var textpos = (int)(endpos.X - (SmallBoxWidth / 1.2f));

                switch (permaitem.Item.ValueType)
                {
                    case MenuValueType.Boolean:
                        DrawBox(boxpos, permaitem.Item.GetValue<bool>());
                        Text.DrawText(
                            null,
                            permaitem.DisplayName + ":",
                            (int)itempos.X,
                            (int)itempos.Y,
                            permaitem.Color);
                        Text.DrawText(
                            null,
                            permaitem.Item.GetValue<bool>().ToString(),
                            textpos,
                            (int)itempos.Y,
                            permaitem.Color);
                        break;
                    case MenuValueType.Slider:
                        Text.DrawText(
                            null,
                            permaitem.DisplayName + ":",
                            (int)itempos.X,
                            (int)itempos.Y,
                            permaitem.Color);
                        Text.DrawText(
                            null,
                            permaitem.Item.GetValue<Slider>().Value.ToString(),
                            textpos,
                            (int)itempos.Y,
                            permaitem.Color);
                        break;
                    case MenuValueType.KeyBind:
                        DrawBox(boxpos, permaitem.Item.GetValue<KeyBind>().Active);
                        Text.DrawText(
                            null,
                            permaitem.DisplayName + " [" + Utils.KeyToText(permaitem.Item.GetValue<KeyBind>().Key)
                            + "] :",
                            (int)itempos.X,
                            (int)itempos.Y,
                            permaitem.Color);

                        Text.DrawText(
                            null,
                            permaitem.Item.GetValue<KeyBind>().Active.ToString(),
                            textpos,
                            (int)(boxpos.Y),
                            permaitem.Color);
                        break;
                    case MenuValueType.StringList:
                        Text.DrawText(
                            null,
                            permaitem.DisplayName + ":",
                            (int)itempos.X,
                            (int)itempos.Y,
                            permaitem.Color);
                        var dimen = Text.MeasureText(sprite, permaitem.Item.GetValue<StringList>().SelectedValue);
                        Text.DrawText(
                            null,
                            permaitem.Item.GetValue<StringList>().SelectedValue,
                            (int)(textpos + dimen.Width < endpos.X ? textpos : endpos.X - dimen.Width),
                            (int)itempos.Y,
                            permaitem.Color);
                        break;
                    case MenuValueType.Integer:
                        Text.DrawText(
                            null,
                            permaitem.DisplayName + ":",
                            (int)itempos.X,
                            (int)itempos.Y,
                            permaitem.Color);
                        Text.DrawText(
                            null,
                            permaitem.Item.GetValue<int>().ToString(),
                            textpos,
                            (int)itempos.Y,
                            permaitem.Color);
                        break;
                    case MenuValueType.None:
                        break;
                    default:
                        throw new Exception("[PermaShow] - MenuItem not supported");
                }
            }
        }

        /// <summary>
        ///     Fired after the DirectX device has been reset.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void DrawingOnOnPostReset(EventArgs args)
        {
            sprite.OnResetDevice();
            Text.OnResetDevice();
            BoxLine.OnResetDevice();
        }

        /// <summary>
        ///     Fired before the DirectX device has been reset.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void DrawingOnPreReset(EventArgs args)
        {
            sprite.OnLostDevice();
            Text.OnLostDevice();
            BoxLine.OnLostDevice();
        }

        /// <summary>
        ///     Return current positions from file
        /// </summary>
        /// <returns>Vector2.</returns>
        private static Vector2 GetPosition()
        {
            return new Vector2(
                placetosave.Item("X").GetValue<Slider>().Value,
                placetosave.Item("Y").GetValue<Slider>().Value);
        }

        /// <summary>
        ///     Determines whether this instance is enabled.
        /// </summary>
        /// <returns><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</returns>
        private static bool IsEnabled()
        {
            return placetosave.Item("enablepermashow").GetValue<bool>();
        }

        /// <summary>
        ///     Gets if the mouse of over the perma show.
        /// </summary>
        /// <returns><c>true</c> if the mouse of over the perma show, <c>false</c> otherwise.</returns>
        private static bool MouseOverArea()
        {
            var pos = Utils.GetCursorPos();
            return ((pos.X >= BoxPosition.X - (PermaShowWidth / 2f)) && pos.X <= (BoxPosition.X + (PermaShowWidth / 2f))
                    && pos.Y >= BoxPosition.Y
                    && pos.Y <= (BoxPosition.Y + PermaShowItems.Count * (Text.Description.Height * 1.2f)));
        }

        /// <summary>
        ///     Fired when the window receives a message.
        /// </summary>
        /// <param name="args">The <see cref="WndEventArgs" /> instance containing the event data.</param>
        private static void OnWndProc(WndEventArgs args)
        {
            if (MenuSettings.DrawMenu || !placetosave.Item("moveable").GetValue<bool>())
            {
                Dragging = false;
                return;
            }

            if (Dragging)
            {
                BoxPosition = Utils.GetCursorPos();
            }

            if (MouseOverArea() && args.Msg == (uint)WindowsMessages.WM_LBUTTONDOWN)
            {
                if (!Dragging)
                {
                    Dragging = true;
                }
            }
            else if (Dragging && args.Msg == (uint)WindowsMessages.WM_LBUTTONUP)
            {
                Dragging = false;
                BoxPosition = Utils.GetCursorPos();
                SavePosition();
            }
        }

        /// <summary>
        ///     Draw area where text will be drawn
        /// </summary>
        private static void PermaArea()
        {
            BoxLine.Width = PermaShowWidth;

            BoxLine.Begin();

            var pos = BoxPosition;

            var positions = new[]
                                {
                                    new Vector2(pos.X, pos.Y),
                                    new Vector2(pos.X, pos.Y + PermaShowItems.Count * (Text.Description.Height * 1.2f))
                                };

            var col = Color.Black;

            BoxLine.Draw(positions, new ColorBGRA(col.B, col.G, col.R, 0.4f));

            BoxLine.End();
        }

        /// <summary>
        ///     Initialize the Drawing tools
        /// </summary>
        private static void PrepareDrawing()
        {
            string FontName;
            int FontHeight;
            FontQuality Quality;
            try
            {
                FontName = CommonMenu.Instance.Item("FontName").GetValue<StringList>().SelectedValue;
                FontHeight = CommonMenu.Instance.Item("FontSize").GetValue<Slider>().Value;
                Quality =
                    (FontQuality)
                    Enum.Parse(
                        typeof(FontQuality),
                        CommonMenu.Instance.Item("FontQuality").GetValue<StringList>().SelectedValue);
            }

            catch (Exception e)
            {
                Console.WriteLine("Common Menu not initialized yet. Resorting to Default Settings " + e);
                FontName = "Tahoma";
                FontHeight = 13;
                Quality = FontQuality.Default;
            }

            try
            {
                Text = new Font(
                    Drawing.Direct3DDevice,
                    new FontDescription
                        {
                            FaceName = FontName, Height = FontHeight, OutputPrecision = FontPrecision.Default,
                            Quality = Quality,
                        });

                sprite = new Sprite(Drawing.Direct3DDevice);
                BoxLine = new Line(Drawing.Direct3DDevice) { Width = 1 };
            }
            catch (DllNotFoundException ex)
            {
                if (ex.Message.Contains("d3dx9_43"))
                {
                    var msg =
                        "Drawings won't work because DirectX (June 2010) is not installed, install https://www.microsoft.com/en-us/download/details.aspx?id=8109 to fix this problem.";
                    Console.WriteLine(msg);
                    Game.PrintChat(msg);
                }
            }
        }

        /// <summary>
        ///     Saves the current position
        /// </summary>
        private static void SavePosition()
        {
            placetosave.Item("X").ValueSet = true;
            placetosave.Item("Y").ValueSet = true;
            placetosave.Item("X").SetValue(new Slider((int)BoxPosition.X, 0, Drawing.Width));
            placetosave.Item("Y").SetValue(new Slider((int)BoxPosition.Y, 0, Drawing.Height));
        }

        /// <summary>
        ///     Scales the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>System.Single.</returns>
        private static float ScaleValue(float value, Direction direction)
        {
            var returnvalue = direction == Direction.X ? value * XFactor : value * YFactor;
            return returnvalue;
        }

        /// <summary>
        ///     Subscribes this instance to L# events.
        /// </summary>
        private static void Sub()
        {
            subbed = true;
            Drawing.OnPreReset += DrawingOnPreReset;
            Drawing.OnPostReset += DrawingOnOnPostReset;
            Drawing.OnDraw += Drawing_OnEndScene;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainOnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnDomainUnload;
            Game.OnWndProc += OnWndProc;
        }

        /// <summary>
        ///     Unsubscribes this instance to L# events.
        /// </summary>
        private static void Unsub()
        {
            subbed = false;
            Drawing.OnPreReset -= DrawingOnPreReset;
            Drawing.OnPostReset -= DrawingOnOnPostReset;
            Drawing.OnDraw -= Drawing_OnEndScene;
            AppDomain.CurrentDomain.DomainUnload -= CurrentDomainOnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit -= CurrentDomainOnDomainUnload;
            Game.OnWndProc -= OnWndProc;
        }

        #endregion

        /// <summary>
        ///     Class PermaShowItem.
        /// </summary>
        internal class PermaShowItem
        {
            #region Properties

            /// <summary>
            ///     Gets or sets the color.
            /// </summary>
            /// <value>The color.</value>
            internal Color Color { get; set; }

            /// <summary>
            ///     Gets or sets the display name.
            /// </summary>
            /// <value>The display name.</value>
            internal string DisplayName { get; set; }

            /// <summary>
            ///     Gets or sets the item.
            /// </summary>
            /// <value>The item.</value>
            internal MenuItem Item { get; set; }

            /// <summary>
            ///     Gets or sets the type of the item.
            /// </summary>
            /// <value>The type of the item.</value>
            internal MenuValueType ItemType { get; set; }

            #endregion
        }
    }
}