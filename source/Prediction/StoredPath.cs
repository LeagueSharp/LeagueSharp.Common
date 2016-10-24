// <copyright file="StoredPath.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.Collections.Generic;
    using System.Linq;

    using SharpDX;

    /// <summary>
    ///     Stored path information of a unit.
    /// </summary>
    public class StoredPath
    {
        #region Public Properties

        /// <summary>
        ///     Gets the end point.
        /// </summary>
        public Vector2 EndPoint => this.Path.LastOrDefault();

        /// <summary>
        ///     Gets or sets the path.
        /// </summary>
        public List<Vector2> Path { get; set; }

        /// <summary>
        ///     Gets the start point.
        /// </summary>
        public Vector2 StartPoint => this.Path.FirstOrDefault();

        /// <summary>
        ///     Gets or sets the tick.
        /// </summary>
        public int Tick { get; set; }

        /// <summary>
        ///     Gets the time.
        /// </summary>
        public double Time => (Utils.GameTimeTickCount - this.Tick) / 1000d;

        /// <summary>
        ///     Gets the waypoint count.
        /// </summary>
        public int WaypointCount => this.Path.Count;

        #endregion
    }
}