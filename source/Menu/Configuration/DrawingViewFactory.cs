// <copyright file="DrawingViewFactory.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Configuration
{
    using System.ComponentModel.Composition;

    using SharpDX.Menu;

    /// <summary>
    ///     The drawing view factory.
    /// </summary>
    [Export(typeof(IDrawingViewFactory))]
    public class DrawingViewFactory : SharpDX.Menu.DrawingViewFactory
    {
    }
}