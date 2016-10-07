// <copyright file="Ring.cs" company="LeagueSharp">
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
            ///     Ring Polygon.
            /// </summary>
            public class Ring : Polygon
            {
                #region Constructors and Destructors

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Ring" /> class.
                /// </summary>
                /// <param name="center">
                ///     The center.
                /// </param>
                /// <param name="innerRadius">
                ///     The inner radius.
                /// </param>
                /// <param name="outerRadius">
                ///     The outer radius.
                /// </param>
                /// <param name="quality">
                ///     The quality.
                /// </param>
                public Ring(Vector3 center, float innerRadius, float outerRadius, int quality = 20)
                    : this(center.To2D(), innerRadius, outerRadius, quality)
                {
                }

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Ring" /> class.
                /// </summary>
                /// <param name="center">
                ///     The center.
                /// </param>
                /// <param name="innerRadius">
                ///     The inner radius.
                /// </param>
                /// <param name="outerRadius">
                ///     The outer radius.
                /// </param>
                /// <param name="quality">
                ///     The quality.
                /// </param>
                public Ring(Vector2 center, float innerRadius, float outerRadius, int quality = 20)
                {
                    this.Center = center;
                    this.InnerRadius = innerRadius;
                    this.OuterRadius = outerRadius;
                    this.Quality = quality;
                    this.UpdatePolygon();
                }

                #endregion

                #region Public Properties

                /// <summary>
                ///     Gets or sets the center position.
                /// </summary>
                public Vector2 Center { get; set; }

                /// <summary>
                ///     Gets or sets the inner radius.
                /// </summary>
                public float InnerRadius { get; set; }

                /// <summary>
                ///     Gets or sets the outer radius.
                /// </summary>
                public float OuterRadius { get; set; }

                /// <summary>
                ///     Gets or sets the quality.
                /// </summary>
                public int Quality { get; set; }

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

                    var outRadius = (offset + this.InnerRadius + this.OuterRadius)
                                    / (float)Math.Cos(2 * Math.PI / this.Quality);
                    var innerRadius = this.InnerRadius - this.OuterRadius - offset;
                    for (var i = 0; i <= this.Quality; i++)
                    {
                        var angle = i * 2 * Math.PI / this.Quality;
                        var point = new Vector2(
                                        this.Center.X - (outRadius * (float)Math.Cos(angle)),
                                        this.Center.Y - (outRadius * (float)Math.Sin(angle)));
                        this.Points.Add(point);
                    }

                    for (var i = 0; i <= this.Quality; i++)
                    {
                        var angle = i * 2 * Math.PI / this.Quality;
                        var point = new Vector2(
                                        this.Center.X + (innerRadius * (float)Math.Cos(angle)),
                                        this.Center.Y - (innerRadius * (float)Math.Sin(angle)));
                        this.Points.Add(point);
                    }
                }

                #endregion
            }
        }
    }
}