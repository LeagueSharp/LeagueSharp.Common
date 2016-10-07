// <copyright file="Sector.cs" company="LeagueSharp">
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
            ///     Sector Polygon.
            /// </summary>
            public class Sector : Polygon
            {
                #region Constructors and Destructors

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Sector" /> class.
                /// </summary>
                /// <param name="center">
                ///     The center.
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
                public Sector(Vector3 center, Vector3 direction, float angle, float radius, int quality = 20)
                    : this(center.To2D(), direction.To2D(), angle, radius, quality)
                {
                }

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Sector" /> class.
                /// </summary>
                /// <param name="center">
                ///     The center.
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
                public Sector(Vector2 center, Vector2 direction, float angle, float radius, int quality = 20)
                {
                    this.Center = center;
                    this.Direction = direction;
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
                ///     Gets or sets the center.
                /// </summary>
                public Vector2 Center { get; set; }

                /// <summary>
                ///     Gets or sets the direction.
                /// </summary>
                public Vector2 Direction { get; set; }

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
                ///     Rotates the line from the point by a specific degree(or radian).
                /// </summary>
                /// <param name="point1">
                ///     The first point.
                /// </param>
                /// <param name="point2">
                ///     The second point.
                /// </param>
                /// <param name="value">
                ///     The value.
                /// </param>
                /// <param name="radian">
                ///     A value indicating whether the angle value is in degrees or radians.
                /// </param>
                /// <returns>
                ///     The <see cref="Vector2" />.
                /// </returns>
                public Vector2 RotateLineFromPoint(Vector2 point1, Vector2 point2, float value, bool radian = true)
                {
                    var angle = !radian ? value * Math.PI / 180 : value;
                    var line = Vector2.Subtract(point2, point1);

                    var newline = new Vector2
                                      {
                                          X = (float)((line.X * Math.Cos(angle)) - (line.Y * Math.Sin(angle))),
                                          Y = (float)((line.X * Math.Sin(angle)) + (line.Y * Math.Cos(angle)))
                                      };

                    return Vector2.Add(newline, point1);
                }

                /// <summary>
                ///     Updates the polygon.
                /// </summary>
                /// <param name="offset">
                ///     The radius offset.
                /// </param>
                public void UpdatePolygon(int offset = 0)
                {
                    this.Points.Clear();
                    this.Points.Add(this.Center);

                    var radius = (this.Radius + offset) / (float)Math.Cos(2 * Math.PI / this.Quality);
                    var side = this.Direction.Rotated(-this.Angle * 0.5f);

                    for (var i = 0; i <= this.Quality; i++)
                    {
                        var cDirection = side.Rotated(i * this.Angle / this.Quality).Normalized();
                        this.Points.Add(
                            new Vector2(
                                this.Center.X + (radius * cDirection.X),
                                this.Center.Y + (radius * cDirection.Y)));
                    }
                }

                #endregion
            }
        }
    }
}