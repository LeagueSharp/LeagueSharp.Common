// <copyright file="ProjectionInfo.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using SharpDX;

    /// <summary>
    ///     Geometry class.
    /// </summary>
    public partial class Geometry
    {
        /// <summary>
        ///     Intersection Result.
        /// </summary>
        public struct ProjectionInfo
        {
            #region Fields

            /// <summary>
            ///     A value indicating whether the point is on segment.
            /// </summary>
            public bool IsOnSegment;

            /// <summary>
            ///     The lint point.
            /// </summary>
            public Vector2 LinePoint;

            /// <summary>
            ///     The segment point.
            /// </summary>
            public Vector2 SegmentPoint;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="ProjectionInfo" /> struct.
            /// </summary>
            /// <param name="isOnSegment">
            ///     A value indicating whether the point is on segment.
            /// </param>
            /// <param name="segmentPoint">
            ///     The segment point.
            /// </param>
            /// <param name="linePoint">
            ///     The line point.
            /// </param>
            public ProjectionInfo(bool isOnSegment, Vector2 segmentPoint, Vector2 linePoint)
            {
                this.IsOnSegment = isOnSegment;
                this.SegmentPoint = segmentPoint;
                this.LinePoint = linePoint;
            }

            #endregion
        }
    }
}