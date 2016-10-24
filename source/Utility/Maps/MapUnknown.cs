// <copyright file="MapUnknown.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using SharpDX;

    /// <summary>
    ///     The unknown map.
    /// </summary>
    public class MapUnknown : IMap
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MapUnknown" /> class.
        /// </summary>
        public MapUnknown()
        {
            this.Name = "Unknown";
            this.ShortName = "unknown";
            this.Type = Utility.Map.MapType.Unknown;
            this.Grid = new Vector2(0, 0);
            this.StartingLevel = 0;
        }

        #endregion

        #region Public Properties

        /// <inheritdoc />
        public Vector2 Grid { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string ShortName { get; }

        /// <inheritdoc />
        public int StartingLevel { get; }

        /// <inheritdoc />
        public Utility.Map.MapType Type { get; }

        #endregion
    }
}