// <copyright file="IViewAttributes.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.View
{
    using System.Drawing;

    /// <summary>
    ///     The view attributes interface.
    /// </summary>
    public interface IViewAttributes
    {
        #region Public Properties

        /// <summary>
        ///     Gets the font brush.
        /// </summary>
        Brush FontBrush { get; }

        /// <summary>
        ///     Gets the font name.
        /// </summary>
        string FontName { get; }

        /// <summary>
        ///     Gets the font style.
        /// </summary>
        FontStyle FontStyle { get; }

        #endregion
    }
}