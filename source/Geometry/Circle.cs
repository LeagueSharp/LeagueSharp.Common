// <copyright file="Circle.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;

    using SharpDX;

    /// <summary>
    ///     Geometry class.
    /// </summary>
    public partial class Geometry
    {
        /// <summary>
        ///     The polygon.
        /// </summary>
        public partial class Polygon
        {
            /// <summary>
            ///     Circle Polygon.
            /// </summary>
            public class Circle : Polygon
            {
                #region Constructors and Destructors

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Circle" /> class.
                /// </summary>
                /// <param name="center">
                ///     The center.
                /// </param>
                /// <param name="radius">
                ///     The radius.
                /// </param>
                /// <param name="quality">
                ///     The quality.
                /// </param>
                public Circle(Vector3 center, float radius, int quality = 20)
                    : this(center.To2D(), radius, quality)
                {
                }

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Circle" /> class.
                /// </summary>
                /// <param name="center">
                ///     The center.
                /// </param>
                /// <param name="radius">
                ///     The radius.
                /// </param>
                /// <param name="quality">
                ///     The quality.
                /// </param>
                public Circle(Vector2 center, float radius, int quality = 20)
                {
                    this.Center = center;
                    this.Radius = radius;
                    this.Quality = quality;
                    this.UpdatePolygon();
                }

                #endregion

                #region Public Properties

                /// <summary>
                ///     Gets or sets the center.
                /// </summary>
                public Vector2 Center { get; set; }

                /// <summary>
                ///     Gets or sets the quality.
                /// </summary>
                public int Quality { get; set; }

                /// <summary>
                ///     Gets or sets the radius.
                /// </summary>
                public float Radius { get; set; }

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Updates the polygon.
                /// </summary>
                /// <param name="offset">
                ///     The radius offset.
                /// </param>
                /// <param name="overrideWidth">
                ///     The width to override with.
                /// </param>
                public void UpdatePolygon(int offset = 0, float overrideWidth = -1)
                {
                    this.Points.Clear();
                    var radius = overrideWidth > 0
                                     ? overrideWidth
                                     : (offset + this.Radius) / (float)Math.Cos(2 * Math.PI / this.Quality);

                    for (var i = 1; i <= this.Quality; ++i)
                    {
                        var angle = i * 2 * Math.PI / this.Quality;
                        var point = new Vector2(
                                        this.Center.X + (radius * (float)Math.Cos(angle)),
                                        this.Center.Y + (radius * (float)Math.Sin(angle)));
                        this.Points.Add(point);
                    }
                }

                #endregion
            }
        }
    }
}