// <copyright file="Map.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    /// <summary>
    ///     The utility class.
    /// </summary>
    public partial class Utility
    {
        /// <summary>
        ///     The Map.
        /// </summary>
        public partial class Map
        {
            #region Public Properties

            /// <summary>
            ///     Gets or sets the map enumerable lazies.
            /// </summary>
            [ImportMany]
            public IEnumerable<Lazy<IMap, IMapMetadata>> MapLazies { get; set; }

            #endregion

            #region Properties

            private IMap CurrentMap { get; set; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Gets the current map.
            /// </summary>
            /// <returns>
            ///     The <see cref="IMap" />.
            /// </returns>
            public IMap GetCurrentMap()
            {
                if (this.CurrentMap != null)
                {
                    return this.CurrentMap;
                }

                var map = this.MapLazies.FirstOrDefault(f => f.Metadata.MapId == (int)Game.MapId);
                if (map != null)
                {
                    return this.CurrentMap = map.Value;
                }

                return this.CurrentMap = new MapUnknown();
            }

            #endregion
        }
    }
}