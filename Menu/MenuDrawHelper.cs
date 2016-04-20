namespace LeagueSharp.Common
{
    using System;
    using System.Drawing;

    using SharpDX;
    using SharpDX.Direct3D9;

    using Color = System.Drawing.Color;
    using Font = SharpDX.Direct3D9.Font;
    using Rectangle = SharpDX.Rectangle;

    /// <summary>
    ///     The menu draw helper.
    /// </summary>
    internal static class MenuDrawHelper
    {
        #region Static Fields

        /// <summary>
        ///     The font.
        /// </summary>
        internal static Font Font;

        /// <summary>
        ///     The bold font.
        /// </summary>
        internal static Font FontBold;

        /// <summary>
        ///     The italic font.
        /// </summary>
        internal static Font FontItalic;
        
        /// <summary>
        ///     The bold and italic font.
        /// </summary>
        internal static Font FontBoldItalic;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a static instance of the <see cref="MenuDrawHelper" /> class.
        /// </summary>
        static MenuDrawHelper()
        {
            var device = Drawing.Direct3DDevice;
            var faceName = Menu.Root.Item("FontName").GetValue<StringList>().SelectedValue;
            var height = Menu.Root.Item("FontSize").GetValue<Slider>().Value;
            var outputPercision = FontPrecision.Default;
            var quality =
                (FontQuality)
                Enum.Parse(
                    typeof(FontQuality),
                    Menu.Root.Item("FontQuality").GetValue<StringList>().SelectedValue,
                    true);

            Font = new Font(
                device,
                new FontDescription
                    { FaceName = faceName, Height = height, OutputPrecision = outputPercision, Quality = quality });

            FontBold = new Font(
                device,
                new FontDescription
                    {
                        FaceName = faceName, Height = height, OutputPrecision = outputPercision, Weight = FontWeight.Bold,
                        Quality = quality
                    });

            FontItalic = new Font(
                device,
                new FontDescription
                    {
                        FaceName = faceName, Height = height, OutputPrecision = outputPercision, Italic = true,
                        Quality = quality
                    });

            FontBoldItalic = new Font(
                device,
                new FontDescription
                    {
                        FaceName = faceName, Height = height, OutputPrecision = outputPercision, Weight = FontWeight.Bold,
                        Italic = true, Quality = quality
                    });

            Drawing.OnPreReset += OnPreReset;
            Drawing.OnPostReset += OnPostReset;
            AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
        }

        #endregion

        internal static Font GetFont(FontStyle fontStyle)
        {
            switch (fontStyle)
            {
                case FontStyle.Bold:
                    return FontBold;
                case FontStyle.Italic:
                    return FontItalic;
                case FontStyle.Bold | FontStyle.Italic:
                    return FontBoldItalic;
                default:
                    return Font;
            }
        }

        #region Methods

        /// <summary>
        ///     Draws an arrow.
        /// </summary>
        /// <param name="s">
        ///     The string.
        /// </param>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="item">
        ///     The item.
        /// </param>
        /// <param name="color">
        ///     The color.
        /// </param>
        internal static void DrawArrow(string s, Vector2 position, MenuItem item, Color color)
        {
            DrawBox(position, item.Height, item.Height, Color.FromArgb(0, 37, 53), 1, color);
            Font.DrawText(
                null,
                s,
                new Rectangle((int)(position.X), (int)item.Position.Y, item.Height, item.Height),
                FontDrawFlags.VerticalCenter | FontDrawFlags.Center,
                new ColorBGRA(255, 255, 255, 255));
        }

        /// <summary>
        ///     Draws a box.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="width">
        ///     The width.
        /// </param>
        /// <param name="height">
        ///     The height.
        /// </param>
        /// <param name="color">
        ///     The color.
        /// </param>
        /// <param name="borderwidth">
        ///     The border width.
        /// </param>
        /// <param name="borderColor">
        ///     The border color.
        /// </param>
        internal static void DrawBox(
            Vector2 position,
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
                    position.X,
                    position.Y + height,
                    position.X + width,
                    position.Y + height,
                    borderwidth,
                    borderColor);
                Drawing.DrawLine(position.X, position.Y, position.X, position.Y + height, borderwidth, borderColor);
                Drawing.DrawLine(
                    position.X + width,
                    position.Y,
                    position.X + width,
                    position.Y + height,
                    borderwidth,
                    borderColor);
            }
        }

        /// <summary>
        ///     Draws the on and off box.
        /// </summary>
        /// <param name="on">
        ///     Indicates whether the value is on.
        /// </param>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="item">
        ///     The item.
        /// </param>
        internal static void DrawOnOff(bool on, Vector2 position, MenuItem item)
        {
            DrawBox(
                position,
                item.Height,
                item.Height,
                on ? Color.FromArgb(1, 169, 234) : Color.FromArgb(37, 37, 37),
                1,
                Color.Black);
            var s = on ? "ON" : "OFF";
            Font.DrawText(
                null,
                s,
                new Rectangle(
                    (int)(item.Position.X + item.Width - item.Height),
                    (int)item.Position.Y,
                    item.Height,
                    item.Height),
                FontDrawFlags.VerticalCenter | FontDrawFlags.Center,
                new ColorBGRA(255, 255, 255, 255));
        }

        /// <summary>
        ///     Draws a slider.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="item">
        ///     The item.
        /// </param>
        /// <param name="width">
        ///     The width.
        /// </param>
        /// <param name="drawText">
        ///     Indicates whether to draw informative text.
        /// </param>
        internal static void DrawSlider(Vector2 position, MenuItem item, int width = -1, bool drawText = true)
        {
            var val = item.GetValue<Slider>();
            DrawSlider(position, item, val.MinValue, val.MaxValue, val.Value, width, drawText);
        }

        /// <summary>
        ///     Draws a slider.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="item">
        ///     The item.
        /// </param>
        /// <param name="min">
        ///     The minimum value.
        /// </param>
        /// <param name="max">
        ///     The maximum value.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="width">
        ///     The width.
        /// </param>
        /// <param name="drawText">
        ///     Indicates whether to draw informative text.
        /// </param>
        internal static void DrawSlider(
            Vector2 position,
            MenuItem item,
            int min,
            int max,
            int value,
            int width,
            bool drawText)
        {
            width = (width > 0 ? width : item.Width);
            var percentage = 100 * (value - min) / (max - min);
            var x = position.X + 3 + (percentage * (width - 3)) / 100f;
            var x2D = 3 + (percentage * (width - 3)) / 100;
            Drawing.DrawLine(x, position.Y + 2, x, position.Y + item.Height, 2, Color.FromArgb(0, 74, 103));
            DrawBox(
                new Vector2(position.X, position.Y),
                x2D - 2,
                item.Height,
                Color.FromArgb(0, 37, 53),
                0,
                Color.Black);

            if (drawText)
            {
                Font.DrawText(
                    null,
                    value.ToString(),
                    new Rectangle((int)position.X - 5, (int)position.Y, item.Width, item.Height),
                    FontDrawFlags.VerticalCenter | FontDrawFlags.Right,
                    new ColorBGRA(255, 255, 255, 255));
            }
        }

        /// <summary>
        ///     Draws the tooltip button.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="item">
        ///     The item.
        /// </param>
        internal static void DrawToolTipButton(Vector2 position, MenuItem item)
        {
            if (item.ValueType == MenuValueType.StringList)
            {
                return;
            }

            const string S = "[?]";
            var x = (int)item.Position.X + item.Width - item.Height - Font.MeasureText(S).Width - 7;

            Font.DrawText(
                null,
                S,
                new Rectangle(x, (int)item.Position.Y, item.Width, item.Height),
                FontDrawFlags.VerticalCenter,
                new ColorBGRA(255, 255, 255, 255));
        }

        /// <summary>
        ///     Draws the tooltip text.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="item">
        ///     The item.
        /// </param>
        /// <param name="textColor">
        ///     The text color.
        /// </param>
        internal static void DrawToolTipText(Vector2 position, MenuItem item, SharpDX.Color? textColor = null)
        {
            if (item.ValueType == MenuValueType.StringList)
            {
                return;
            }

            DrawBox(
                new Vector2(position.X + item.Height - 28, position.Y + 1),
                Font.MeasureText(item.Tooltip).Width + 8,
                item.Height,
                MenuSettings.BackgroundColor,
                1,
                Color.Black);

            var s = item.Tooltip;
            Font.DrawText(
                null,
                s,
                new Rectangle(
                    (int)(item.Position.X + item.Width - 33 + item.Height + 8),
                    (int)item.Position.Y - 3,
                    Font.MeasureText(item.Tooltip).Width + 8,
                    item.Height + 8),
                FontDrawFlags.VerticalCenter,
                textColor ?? SharpDX.Color.White);
        }

        /// <summary>
        ///     The domain unload event.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="eventArgs">
        ///     The event args.
        /// </param>
        private static void OnDomainUnload(object sender, EventArgs eventArgs)
        {
            if (Font != null)
            {
                Font.OnLostDevice();
                Font.Dispose();
                Font = null;
            }

            if (FontBold != null)
            {
                FontBold.OnLostDevice();
                FontBold.Dispose();
                FontBold = null;
            }

            if (FontBoldItalic != null)
            {
                FontBoldItalic.OnLostDevice();
                FontBoldItalic.Dispose();
                FontBoldItalic = null;
            }

            if (FontItalic != null)
            {
                FontItalic.OnLostDevice();
                FontItalic.Dispose();
                FontItalic = null;
            }
        }

        /// <summary>
        ///     The post reset event.
        /// </summary>
        /// <param name="args">The event args.</param>
        private static void OnPostReset(EventArgs args)
        {
            Font.OnResetDevice();
            FontBold.OnResetDevice();
            FontBoldItalic.OnResetDevice();
            FontItalic.OnResetDevice();
        }

        /// <summary>
        ///     The pre reset event.
        /// </summary>
        /// <param name="args">
        ///     The event args.
        /// </param>
        private static void OnPreReset(EventArgs args)
        {
            Font.OnLostDevice();
            FontBold.OnLostDevice();
            FontItalic.OnLostDevice();
            FontBoldItalic.OnLostDevice();
        }

        #endregion
    }
}