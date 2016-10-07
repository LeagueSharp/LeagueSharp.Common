// <copyright file="MapSummonersRift.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    using SharpDX;

    /// <summary>
    ///     The summoners rift map.
    /// </summary>
    [Export(typeof(IMap))]
    [ExportMetadata("MapId", 11)]
    public class MapSummonersRift : IMap
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MapSummonersRift" /> class.
        /// </summary>
        public MapSummonersRift()
        {
            this.Name = "Summoner's Rift";
            this.ShortName = "summonerRift";
            this.Type = Utility.Map.MapType.SummonersRift;
            this.Grid = new Vector2(13982f / 2, 14446f / 2);
            this.StartingLevel = 1;
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