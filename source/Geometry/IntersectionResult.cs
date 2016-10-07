// <copyright file="IntersectionResult.cs" company="LeagueSharp">
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
        public struct IntersectionResult
        {
            #region Fields

            /// <summary>
            ///     A value indicating whether the result intersects.
            /// </summary>
            public bool Intersects;

            /// <summary>
            ///     The point.
            /// </summary>
            public Vector2 Point;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="IntersectionResult" /> struct.
            /// </summary>
            /// <param name="intersects">
            ///     A value indicating whether the result intersects.
            /// </param>
            /// <param name="point">
            ///     The point.
            /// </param>
            public IntersectionResult(bool intersects = false, Vector2 point = default(Vector2))
            {
                this.Intersects = intersects;
                this.Point = point;
            }

            #endregion
        }
    }
}