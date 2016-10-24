// <copyright file="FarmLocation.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using SharpDX;

    /// <summary>
    ///     Provides a minion AI manager.
    /// </summary>
    public partial class MinionManager
    {
        /// <summary>
        ///     Farm location.
        /// </summary>
        public struct FarmLocation
        {
            #region Fields

            /// <summary>
            ///     The Minions Hit at position.
            /// </summary>
            public int MinionsHit;

            /// <summary>
            ///     The position.
            /// </summary>
            public Vector2 Position;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="FarmLocation" /> struct.
            /// </summary>
            /// <param name="position">
            ///     The position.
            /// </param>
            /// <param name="hit">
            ///     The hit count.
            /// </param>
            public FarmLocation(Vector2 position, int hit)
            {
                this.Position = position;
                this.MinionsHit = hit;
            }

            #endregion
        }
    }
}