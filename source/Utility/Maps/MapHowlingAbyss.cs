// <copyright file="MapHowlingAbyss.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    using SharpDX;

    /// <summary>
    ///     The howling abyss map.
    /// </summary>
    [Export(typeof(IMap))]
    [ExportMetadata("MapId", 12)]
    public class MapHowlingAbyss : IMap
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MapHowlingAbyss" /> class.
        /// </summary>
        public MapHowlingAbyss()
        {
            this.Name = "Howling Abyss";
            this.ShortName = "howlingAbyss";
            this.Type = Utility.Map.MapType.HowlingAbyss;
            this.Grid = new Vector2(13120f / 2, 12618f / 2);
            this.StartingLevel = 3;
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