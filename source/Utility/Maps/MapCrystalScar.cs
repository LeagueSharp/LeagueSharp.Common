// <copyright file="MapCrystalScar.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    using SharpDX;

    /// <summary>
    ///     The crystal scar map.
    /// </summary>
    [Export(typeof(IMap))]
    [ExportMetadata("MapId", 8)]
    public class MapCrystalScar : IMap
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MapCrystalScar" /> class.
        /// </summary>
        public MapCrystalScar()
        {
            this.Name = "The Crystal Scar";
            this.ShortName = "crystalscar";
            this.Type = Utility.Map.MapType.CrystalScar;
            this.Grid = new Vector2(13894f / 2, 13218f / 2);
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