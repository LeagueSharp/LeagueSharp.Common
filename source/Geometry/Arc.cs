// <copyright file="Arc.cs" company="LeagueSharp">
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
            ///     Arc Polygon.
            /// </summary>
            public class Arc : Polygon
            {
                #region Constructors and Destructors

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Arc" /> class.
                /// </summary>
                /// <param name="start">
                ///     The start.
                /// </param>
                /// <param name="direction">
                ///     The direction.
                /// </param>
                /// <param name="angle">
                ///     The angle.
                /// </param>
                /// <param name="radius">
                ///     The radius.
                /// </param>
                /// <param name="quality">
                ///     The quality.
                /// </param>
                public Arc(Vector3 start, Vector3 direction, float angle, float radius, int quality = 20)
                    : this(start.To2D(), direction.To2D(), angle, radius, quality)
                {
                }

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Arc" /> class.
                /// </summary>
                /// <param name="start">
                ///     The start.
                /// </param>
                /// <param name="direction">
                ///     The direction.
                /// </param>
                /// <param name="angle">
                ///     The angle.
                /// </param>
                /// <param name="radius">
                ///     The radius.
                /// </param>
                /// <param name="quality">
                ///     The quality.
                /// </param>
                public Arc(Vector2 start, Vector2 direction, float angle, float radius, int quality = 20)
                {
                    this.StartPos = start;
                    this.EndPos = (direction - start).Normalized();
                    this.Angle = angle;
                    this.Radius = radius;
                    this.Quality = quality;
                    this.UpdatePolygon();
                }

                #endregion

                #region Public Properties

                /// <summary>
                ///     Gets or sets the angle.
                /// </summary>
                public float Angle { get; set; }

                /// <summary>
                ///     Gets or sets the ending position.
                /// </summary>
                public Vector2 EndPos { get; set; }

                /// <summary>
                ///     Gets or sets the quality.
                /// </summary>
                public int Quality { get; set; }

                /// <summary>
                ///     Gets or sets the radius.
                /// </summary>
                public float Radius { get; set; }

                /// <summary>
                ///     Gets or sets the starting position.
                /// </summary>
                public Vector2 StartPos { get; set; }

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Updates the polygon.
                /// </summary>
                /// <param name="offset">
                ///     The radius offset.
                /// </param>
                public void UpdatePolygon(int offset = 0)
                {
                    this.Points.Clear();

                    var radius = (this.Radius + offset) / (float)Math.Cos(2 * Math.PI / this.Quality);
                    var side = this.EndPos.Rotated(-this.Angle * .5f);

                    for (var i = 0; i <= this.Quality; ++i)
                    {
                        var direction = side.Rotated(i * this.Angle / this.Quality).Normalized();
                        this.Points.Add(
                            new Vector2(
                                this.StartPos.X + (radius * direction.X),
                                this.StartPos.Y + (radius * direction.Y)));
                    }
                }

                #endregion
            }
        }
    }
}