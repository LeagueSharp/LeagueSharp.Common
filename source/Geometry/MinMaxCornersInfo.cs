// <copyright file="MinMaxCornersInfo.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using SharpDX;

    /// <summary>
    ///     Provides methods for finding the minimum enclosing circles.
    /// </summary>
    public static partial class MEC
    {
        /// <summary>
        ///     The min max corners info.
        /// </summary>
        public struct MinMaxCornersInfo
        {
            #region Fields

            /// <summary>
            ///     The lower left component.
            /// </summary>
            public Vector2 LowerLeft;

            /// <summary>
            ///     The lower right component.
            /// </summary>
            public Vector2 LowerRight;

            /// <summary>
            ///     The upper left component.
            /// </summary>
            public Vector2 UpperLeft;

            /// <summary>
            ///     The upper right component.
            /// </summary>
            public Vector2 UpperRight;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="MinMaxCornersInfo" /> struct.
            /// </summary>
            /// <param name="upperLeft">
            ///     The upper left component.
            /// </param>
            /// <param name="upperRight">
            ///     The upper right component.
            /// </param>
            /// <param name="lowerLeft">
            ///     The lower left component.
            /// </param>
            /// <param name="lowerRight">
            ///     The lower right component.
            /// </param>
            public MinMaxCornersInfo(Vector2 upperLeft, Vector2 upperRight, Vector2 lowerLeft, Vector2 lowerRight)
            {
                this.UpperLeft = upperLeft;
                this.UpperRight = upperRight;
                this.LowerLeft = lowerLeft;
                this.LowerRight = lowerRight;
            }

            #endregion
        }
    }
}