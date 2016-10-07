// <copyright file="MecCircle.cs" company="LeagueSharp">
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
        ///     The ConvexHull Circle.
        /// </summary>
        public struct MecCircle
        {
            #region Fields

            /// <summary>
            ///     The center of the circle.
            /// </summary>
            public Vector2 Center;

            /// <summary>
            ///     The radius of the circle.
            /// </summary>
            public float Radius;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="MecCircle" /> struct.
            /// </summary>
            /// <param name="center">
            ///     The center.
            /// </param>
            /// <param name="radius">
            ///     The radius.
            /// </param>
            public MecCircle(Vector2 center, float radius)
            {
                this.Center = center;
                this.Radius = radius;
            }

            #endregion
        }
    }
}