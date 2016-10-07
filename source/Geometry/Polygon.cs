// <copyright file="Polygon.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.Collections.Generic;
    using System.Linq;

    using ClipperLib;

    using SharpDX;

    using Color = System.Drawing.Color;

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
            #region Public Properties

            /// <summary>
            ///     Gets the polygon points.
            /// </summary>
            public List<Vector2> Points { get; } = new List<Vector2>();

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Adds a point.
            /// </summary>
            /// <param name="point">
            ///     The point.
            /// </param>
            public void Add(Vector2 point) => this.Points.Add(point);

            /// <summary>
            ///     Adds a point.
            /// </summary>
            /// <param name="point">
            ///     The point.
            /// </param>
            public void Add(Vector3 point) => this.Points.Add(point.To2D());

            /// <summary>
            ///     Adds a point.
            /// </summary>
            /// <param name="gameObject">
            ///     The game object.
            /// </param>
            public void Add(GameObject gameObject) => this.Points.Add(gameObject.Position.To2D());

            /// <summary>
            ///     Adds another polygon points.
            /// </summary>
            /// <param name="polygon">
            ///     The polygon.
            /// </param>
            public void Add(Polygon polygon)
            {
                foreach (var point in polygon.Points)
                {
                    this.Points.Add(point);
                }
            }

            /// <summary>
            ///     Draws the polygon.
            /// </summary>
            /// <param name="color">
            ///     The color.
            /// </param>
            /// <param name="width">
            ///     The width.
            /// </param>
            public virtual void Draw(Color color, int width = 1)
            {
                for (var i = 0; i <= this.Points.Count - 1; i++)
                {
                    var nextIndex = (this.Points.Count - 1 == i) ? 0 : (i + 1);
                    var from = Drawing.WorldToScreen(this.Points[i].To3D());
                    var to = Drawing.WorldToScreen(this.Points[nextIndex].To3D());
                    Drawing.DrawLine(from[0], from[1], to[0], to[1], width, color);
                }
            }

            /// <summary>
            ///     determines if the point is inside the polygon.
            /// </summary>
            /// <param name="point">
            ///     The point.
            /// </param>
            /// <returns>
            ///     The <see cref="bool" />.
            /// </returns>
            public bool IsInside(Vector2 point) => !this.IsOutside(point);

            /// <summary>
            ///     determines if the point is inside the polygon.
            /// </summary>
            /// <param name="point">
            ///     The point.
            /// </param>
            /// <returns>
            ///     The <see cref="bool" />.
            /// </returns>
            public bool IsInside(Vector3 point) => !this.IsOutside(point.To2D());

            /// <summary>
            ///     determines if the point is inside the polygon.
            /// </summary>
            /// <param name="gameObject">
            ///     The game object.
            /// </param>
            /// <returns>
            ///     The <see cref="bool" />.
            /// </returns>
            public bool IsInside(GameObject gameObject) => !this.IsOutside(gameObject.Position.To2D());

            /// <summary>
            ///     determines if the point is outside the polygon.
            /// </summary>
            /// <param name="point">
            ///     The point.
            /// </param>
            /// <returns>
            ///     The <see cref="bool" />.
            /// </returns>
            public bool IsOutside(Vector2 point)
                => Clipper.PointInPolygon(new IntPoint(point.X, point.Y), this.ToClipperPath()) != 1;

            /// <summary>
            ///     Converts the points into a clipper path.
            /// </summary>
            /// <returns>
            ///     The <see cref="List{IntPoint}" />.
            /// </returns>
            public List<IntPoint> ToClipperPath()
            {
                var result = new List<IntPoint>(this.Points.Count);
                result.AddRange(this.Points.Select(p => new IntPoint(p.X, p.Y)));
                return result;
            }

            #endregion
        }
    }
}