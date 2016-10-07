// <copyright file="IMap.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using SharpDX;

    /// <summary>
    ///     The map interface.
    /// </summary>
    public interface IMap
    {
        #region Public Properties

        /// <summary>
        ///     Gets the grid.
        /// </summary>
        Vector2 Grid { get; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Gets the short name.
        /// </summary>
        string ShortName { get; }

        /// <summary>
        ///     Gets the starting level.
        /// </summary>
        int StartingLevel { get; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        Utility.Map.MapType Type { get; }

        #endregion
    }
}