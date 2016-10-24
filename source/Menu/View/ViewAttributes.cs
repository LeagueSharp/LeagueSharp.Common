// <copyright file="ViewAttributes.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.View
{
    using System.Drawing;

    using SharpDX.Menu;

    /// <summary>
    ///     The view attributes localizer.
    /// </summary>
    public class ViewAttributes : IViewAttributes
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ViewAttributes" /> class.
        /// </summary>
        /// <param name="view">
        ///     The view.
        /// </param>
        public ViewAttributes(IView view)
        {
            this.FontStyle = view?.GetAttribute<FontStyle>(typeof(FontStyle).ToString()) ?? FontStyle.Regular;
            this.FontBrush = view?.GetAttribute<Brush>(typeof(Brush).ToString()) ?? Brushes.White;
            this.FontName = view?.GetAttribute<string>("FontName") ?? "Tahoma";
        }

        #endregion

        #region Public Properties

        /// <inheritdoc />
        public Brush FontBrush { get; set; }

        /// <inheritdoc />
        public string FontName { get; set; }

        /// <inheritdoc />
        public FontStyle FontStyle { get; set; }

        #endregion
    }
}