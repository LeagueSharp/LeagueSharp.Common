// <copyright file="MapAdapter.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The utility class.
    /// </summary>
    public partial class Utility
    {
        /// <summary>
        ///     The Map.
        /// </summary>
        [Export(typeof(Map))]
        public partial class Map
        {
            #region Enums

            /// <summary>
            ///     The map type.
            /// </summary>
            public enum MapType
            {
                /// <summary>
                ///     The unknown type.
                /// </summary>
                Unknown,

                /// <summary>
                ///     Summoner's rift.
                /// </summary>
                SummonersRift,

                /// <summary>
                ///     Crystal Scar.
                /// </summary>
                CrystalScar,

                /// <summary>
                ///     Twisted Treeline.
                /// </summary>
                TwistedTreeline,

                /// <summary>
                ///     Howling Abyss.
                /// </summary>
                HowlingAbyss
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Gets the current map.
            /// </summary>
            /// <returns>
            ///     The <see cref="IMap" />.
            /// </returns>
            public static IMap GetMap() => Library.Instance?.Map?.GetCurrentMap();

            #endregion
        }
    }
}