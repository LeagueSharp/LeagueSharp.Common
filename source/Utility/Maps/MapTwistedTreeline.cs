// <copyright file="MapTwistedTreeline.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    using SharpDX;

    /// <summary>
    ///     The twisted treeline map.
    /// </summary>
    [Export(typeof(IMap))]
    [ExportMetadata("MapId", 10)]
    public class MapTwistedTreeline : IMap
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MapTwistedTreeline" /> class.
        /// </summary>
        public MapTwistedTreeline()
        {
            this.Name = "The Twisted Treeline";
            this.ShortName = "twistedTreeline";
            this.Type = Utility.Map.MapType.TwistedTreeline;
            this.Grid = new Vector2(15436f / 2, 14474f / 2);
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