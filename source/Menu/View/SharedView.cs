// <copyright file="SharedView.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.View
{
    using System.Drawing;

    /// <summary>
    ///     Shared View, providing common functionality which is usually shared across more than one registry view.
    /// </summary>
    public static class SharedView
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Creates a background for a view.
        /// </summary>
        /// <param name="graphics">
        ///     The graphics.
        /// </param>
        /// <param name="x">
        ///     The X.
        /// </param>
        /// <param name="y">
        ///     The Y.
        /// </param>
        /// <param name="width">
        ///     The width.
        /// </param>
        /// <param name="height">
        ///     The height.
        /// </param>
        public static void CreateBackgroundView(Graphics graphics, float x, float y, float width, float height)
        {
            // Background
            using (var pen = new Pen(Color.FromArgb(150, 0, 0, 0), height))
            {
                graphics.DrawLine(pen, x, y + (height / 2f), x + width, y + (height / 2f));
            }

            // Border
            using (var pen = new Pen(Color.Black, 1.5f))
            {
                graphics.DrawLine(pen, x, y, x + width, y);
                graphics.DrawLine(pen, x, y + height, x + width, y + height);
                graphics.DrawLine(pen, x, y, x, y + height);
                graphics.DrawLine(pen, x + width, y, x + width, y + height);
            }
        }

        /// <summary>
        ///     Creates a border for a view.
        /// </summary>
        /// <param name="graphics">
        ///     The graphics.
        /// </param>
        /// <param name="x">
        ///     The X.
        /// </param>
        /// <param name="y">
        ///     The Y.
        /// </param>
        /// <param name="width">
        ///     The width.
        /// </param>
        /// <param name="height">
        ///     The height.
        /// </param>
        /// <param name="color">
        ///     The color of the border.
        /// </param>
        public static void CreateBorder(Graphics graphics, float x, float y, float width, float height, Color color)
        {
            using (var pen = new Pen(color, 1.5f))
            {
                graphics.DrawLine(pen, x, y, x + width, y);
                graphics.DrawLine(pen, x, y + height, x + width, y + height);
                graphics.DrawLine(pen, x, y, x, y + height);
                graphics.DrawLine(pen, x + width, y, x + width, y + height);
            }
        }

        /// <summary>
        ///     Creates a box for a view.
        /// </summary>
        /// <param name="graphics">
        ///     The graphics.
        /// </param>
        /// <param name="x">
        ///     The X.
        /// </param>
        /// <param name="y">
        ///     The Y.
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
        /// <param name="content">
        ///     The content.
        /// </param>
        public static void CreateBox(
            Graphics graphics,
            float x,
            float y,
            float width,
            float height,
            Color color,
            string content = null)
        {
            using (var pen = new Pen(color, height))
            {
                graphics.DrawLine(pen, x, y + (height / 2f), x + width, y + (height / 2f));
            }

            if (!string.IsNullOrEmpty(content))
            {
                CreateText(graphics, x, y, width, height, content, new ViewAttributes(null));
            }
        }

        /// <summary>
        ///     Creates a slider for a view.
        /// </summary>
        /// <param name="graphics">
        ///     The graphics.
        /// </param>
        /// <param name="x">
        ///     The X.
        /// </param>
        /// <param name="y">
        ///     The Y.
        /// </param>
        /// <param name="width">
        ///     The width.
        /// </param>
        /// <param name="height">
        ///     The height.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="minValue">
        ///     The min value.
        /// </param>
        /// <param name="maxValue">
        ///     The max value.
        /// </param>
        public static void CreateSlider(
            Graphics graphics,
            float x,
            float y,
            float width,
            float height,
            int value,
            int minValue,
            int maxValue)
        {
            width -= 3;
            var valuePercentage = 100 * (value - minValue) / (maxValue - minValue);
            var indicatorX = x + 3 + ((valuePercentage * (width - 3)) / 100f);
            using (var pen = new Pen(Color.CornflowerBlue, 1f))
            {
                graphics.DrawLine(pen, indicatorX, y + 2, indicatorX, y + height);
            }

            if (value != minValue)
            {
                var trackBarX = 3 + ((valuePercentage * (width - 3)) / 100f);
                CreateBox(graphics, x, y, trackBarX - 2, height, Color.FromArgb(150, 0, 37, 53));
            }
        }

        /// <summary>
        ///     Creates a text for a view.
        /// </summary>
        /// <param name="graphics">
        ///     The graphics.
        /// </param>
        /// <param name="x">
        ///     The X.
        /// </param>
        /// <param name="y">
        ///     The Y.
        /// </param>
        /// <param name="width">
        ///     The width.
        /// </param>
        /// <param name="height">
        ///     The height.
        /// </param>
        /// <param name="text">
        ///     The text.
        /// </param>
        /// <param name="viewAttributes">
        ///     The view attributes.
        /// </param>
        public static void CreateText(
            Graphics graphics,
            float x,
            float y,
            float width,
            float height,
            string text,
            IViewAttributes viewAttributes)
        {
            using (var font = new Font(viewAttributes.FontName, 12, viewAttributes.FontStyle, GraphicsUnit.Pixel))
            {
                var rectangle = new RectangleF(x, y, width, height);
                var format = new StringFormat
                                     { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };

                graphics.DrawString(text, font, viewAttributes.FontBrush, rectangle, format);
            }
        }

        /// <summary>
        ///     Creates a title for a view.
        /// </summary>
        /// <param name="graphics">
        ///     The graphics.
        /// </param>
        /// <param name="x">
        ///     The X.
        /// </param>
        /// <param name="y">
        ///     The Y.
        /// </param>
        /// <param name="width">
        ///     The width.
        /// </param>
        /// <param name="height">
        ///     The height.
        /// </param>
        /// <param name="title">
        ///     The title.
        /// </param>
        /// <param name="viewAttributes">
        ///     The view attributes.
        /// </param>
        public static void CreateTitle(
            Graphics graphics,
            float x,
            float y,
            float width,
            float height,
            string title,
            IViewAttributes viewAttributes)
        {
            using (var font = new Font(viewAttributes.FontName, 12, viewAttributes.FontStyle, GraphicsUnit.Pixel))
            {
                var rectangle = new RectangleF(x + 5, y, width - 5, height);
                var format = new StringFormat
                                 {
                                     LineAlignment = StringAlignment.Center,
                                     Trimming = StringTrimming.EllipsisCharacter,
                                     FormatFlags = StringFormatFlags.LineLimit
                                 };

                graphics.DrawString(title, font, viewAttributes.FontBrush, rectangle, format);
            }
        }

        #endregion
    }
}