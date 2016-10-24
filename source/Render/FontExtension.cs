// <copyright file="FontExtension.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.Collections.Generic;

    using SharpDX;
    using SharpDX.Direct3D9;

    /// <summary>
    ///     Font Extensions.
    /// </summary>
    public static class FontExtension
    {
        #region Static Fields

        /// <summary>
        ///     Collection of saved widths for each font.
        /// </summary>
        private static readonly Dictionary<Font, Dictionary<string, Rectangle>> Widths =
            new Dictionary<Font, Dictionary<string, Rectangle>>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Measures the text.
        /// </summary>
        /// <param name="font">
        ///     The font.
        /// </param>
        /// <param name="sprite">
        ///     The sprite.
        /// </param>
        /// <param name="text">
        ///     The text.
        /// </param>
        /// <returns>
        ///     The <see cref="Rectangle" />.
        /// </returns>
        public static Rectangle MeasureText(this Font font, Sprite sprite, string text)
        {
            Dictionary<string, Rectangle> rectangles;
            if (!Widths.TryGetValue(font, out rectangles))
            {
                rectangles = new Dictionary<string, Rectangle>();
                Widths[font] = rectangles;
            }

            Rectangle rectangle;
            if (rectangles.TryGetValue(text, out rectangle))
            {
                return rectangle;
            }

            rectangle = font.MeasureText(sprite, text, 0);
            rectangles[text] = rectangle;
            return rectangle;
        }

        /// <summary>
        ///     Measures the text.
        /// </summary>
        /// <param name="font">
        ///     The font.
        /// </param>
        /// <param name="text">
        ///     The text.
        /// </param>
        /// <returns>
        ///     The <see cref="Rectangle" />.
        /// </returns>
        public static Rectangle MeasureText(this Font font, string text) => font.MeasureText(null, text);

        #endregion
    }
}