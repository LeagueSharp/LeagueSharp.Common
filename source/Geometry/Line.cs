// <copyright file="Line.cs" company="LeagueSharp">
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
            ///     Line Arc.
            /// </summary>
            public class Line : Polygon
            {
                #region Constructors and Destructors

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Line" /> class.
                /// </summary>
                /// <param name="start">
                ///     The start.
                /// </param>
                /// <param name="end">
                ///     The end.
                /// </param>
                /// <param name="length">
                ///     The length.
                /// </param>
                public Line(Vector3 start, Vector3 end, float length = -1)
                    : this(start.To2D(), end.To2D(), length)
                {
                }

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Line" /> class.
                /// </summary>
                /// <param name="start">
                ///     The start.
                /// </param>
                /// <param name="end">
                ///     The end.
                /// </param>
                /// <param name="length">
                ///     The length.
                /// </param>
                public Line(Vector2 start, Vector2 end, float length = -1)
                {
                    this.LineStart = start;
                    this.LineEnd = end;

                    if (length > 0)
                    {
                        this.Length = length;
                    }

                    this.UpdatePolygon();
                }

                #endregion

                #region Public Properties

                /// <summary>
                ///     Gets or sets the length of the line.
                /// </summary>
                public float Length
                {
                    get
                    {
                        return this.LineStart.Distance(this.LineEnd);
                    }

                    set
                    {
                        this.LineEnd = ((this.LineEnd - this.LineStart).Normalized() * value) + this.LineStart;
                    }
                }

                /// <summary>
                ///     Gets or sets the line ending.
                /// </summary>
                public Vector2 LineEnd { get; set; }

                /// <summary>
                ///     Gets or sets the line start.
                /// </summary>
                public Vector2 LineStart { get; set; }

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Updates the polygon.
                /// </summary>
                public void UpdatePolygon()
                {
                    this.Points.Clear();
                    this.Points.AddRange(new[] { this.LineStart, this.LineEnd });
                }

                #endregion
            }
        }
    }
}