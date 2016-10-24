// <copyright file="Rectangle.cs" company="LeagueSharp">
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
        ///     The polygon.
        /// </summary>
        public partial class Polygon
        {
            /// <summary>
            ///     Rectangle Polygon.
            /// </summary>
            public class Rectangle : Polygon
            {
                #region Constructors and Destructors

                /// <summary>
                ///     Initializes a new instance of the <see cref="Rectangle" /> class.
                /// </summary>
                /// <param name="start">
                ///     The starting position.
                /// </param>
                /// <param name="end">
                ///     The ending position.
                /// </param>
                /// <param name="width">
                ///     The width.
                /// </param>
                public Rectangle(Vector3 start, Vector3 end, float width)
                    : this(start.To2D(), end.To2D(), width)
                {
                }

                /// <summary>
                ///     Initializes a new instance of the <see cref="Rectangle" /> class.
                /// </summary>
                /// <param name="start">
                ///     The starting position.
                /// </param>
                /// <param name="end">
                ///     The ending position.
                /// </param>
                /// <param name="width">
                ///     The width.
                /// </param>
                public Rectangle(Vector2 start, Vector2 end, float width)
                {
                    this.Start = start;
                    this.End = end;
                    this.Width = width;
                    this.UpdatePolygon();
                }

                #endregion

                #region Public Properties

                /// <summary>
                ///     Gets the direction.
                /// </summary>
                public Vector2 Direction => (this.End - this.Start).Normalized();

                /// <summary>
                ///     Gets or sets the ending position.
                /// </summary>
                public Vector2 End { get; set; }

                /// <summary>
                ///     Gets the direction perpendicular.
                /// </summary>
                public Vector2 Perpendicular => this.Direction.Perpendicular();

                /// <summary>
                ///     Gets or sets the starting position.
                /// </summary>
                public Vector2 Start { get; set; }

                /// <summary>
                ///     Gets or sets the width.
                /// </summary>
                public float Width { get; set; }

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Updates the polygon.
                /// </summary>
                /// <param name="offset">
                ///     The width offset.
                /// </param>
                /// <param name="overrideWidth">
                ///     The width to override with.
                /// </param>
                public void UpdatePolygon(int offset = 0, float overrideWidth = -1)
                {
                    this.Points.Clear();

                    var startF = ((overrideWidth > 0 ? overrideWidth : this.Width + offset) * this.Perpendicular)
                                 - (offset * this.Direction);
                    var endF = ((overrideWidth > 0 ? overrideWidth : this.Width + offset) * this.Perpendicular)
                               + (offset * this.Direction);

                    this.Points.AddRange(
                        new[] { this.Start + startF, this.Start - startF, this.End - endF, this.End + endF });
                }

                #endregion
            }
        }
    }
}