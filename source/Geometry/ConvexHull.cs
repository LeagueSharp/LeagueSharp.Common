// <copyright file="ConvexHull.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using SharpDX;

    /// <summary>
    ///     Provides methods for finding the minimum enclosing circles.
    /// </summary>
#pragma warning disable SA1300
    public static partial class MEC
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the min max box debug value.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Compability")]
        public static RectangleF g_MinMaxBox { get; set; }

        /// <summary>
        ///     Gets or sets the min max corners debug value.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Compability")]
        public static Vector2[] g_MinMaxCorners { get; set; }

        /// <summary>
        ///     Gets or sets the non culled points debug value.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Compability")]
        public static Vector2[] g_NonCulledPoints { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Finds the minimal bounding circle.
        /// </summary>
        /// <param name="points">
        ///     The points.
        /// </param>
        /// <param name="center">
        ///     The center.
        /// </param>
        /// <param name="radius">
        ///     The radius.
        /// </param>
        public static void FindMinimalBoundingCircle(List<Vector2> points, out Vector2 center, out float radius)
        {
            // Find the convex hull.
            var hull = MakeConvexHull(points);

            // The best solution so far.
            var bestCenter = points[0];
            var bestRadius2 = float.MaxValue;

            // Look at pairs of hull points.
            for (var i = 0; i < hull.Count - 1; i++)
            {
                for (var j = i + 1; j < hull.Count; j++)
                {
                    // Find the circle through these two points.
                    var testCenter = new Vector2((hull[i].X + hull[j].X) / 2f, (hull[i].Y + hull[j].Y) / 2f);
                    var dx = testCenter.X - hull[i].X;
                    var dy = testCenter.Y - hull[i].Y;
                    var testRadius2 = (dx * dx) + (dy * dy);

                    // See if this circle would be an improvement.
                    if (testRadius2 < bestRadius2)
                    {
                        // See if this circle encloses all of the points.
                        if (CircleEnclosesPoints(testCenter, testRadius2, points, i, j, -1))
                        {
                            // Save this solution.
                            bestCenter = testCenter;
                            bestRadius2 = testRadius2;
                        }
                    }
                } // for i
            } // for j

            // Look at triples of hull points.
            for (var i = 0; i < hull.Count - 2; i++)
            {
                for (var j = i + 1; j < hull.Count - 1; j++)
                {
                    for (var k = j + 1; k < hull.Count; k++)
                    {
                        // Find the circle through these three points.
                        Vector2 testCenter;
                        float testRadius2;
                        FindCircle(hull[i], hull[j], hull[k], out testCenter, out testRadius2);

                        // See if this circle would be an improvement.
                        if (testRadius2 < bestRadius2)
                        {
                            // See if this circle encloses all of the points.
                            if (CircleEnclosesPoints(testCenter, testRadius2, points, i, j, k))
                            {
                                // Save this solution.
                                bestCenter = testCenter;
                                bestRadius2 = testRadius2;
                            }
                        }
                    } // for k
                } // for i
            } // for j

            center = bestCenter;
            if (Math.Abs(bestRadius2 - float.MaxValue) < float.Epsilon)
            {
                radius = 0;
            }
            else
            {
                radius = (float)Math.Sqrt(bestRadius2);
            }
        }

        /// <summary>
        ///     Returns the mininimum enclosing circle from a list of points.
        /// </summary>
        /// <param name="points">
        ///     The points.
        /// </param>
        /// <returns>
        ///     The <see cref="MecCircle" />.
        /// </returns>
        public static MecCircle GetMec(List<Vector2> points)
        {
            Vector2 center;
            float radius;

            var convexHull = MakeConvexHull(points);
            FindMinimalBoundingCircle(convexHull, out center, out radius);
            return new MecCircle(center, radius);
        }

        /// <summary>
        ///     Makes the convex hull.
        /// </summary>
        /// <param name="points">
        ///     The points.
        /// </param>
        /// <returns>
        ///     Points that make up a polygon's convex hull.
        /// </returns>
        public static List<Vector2> MakeConvexHull(List<Vector2> points)
        {
            // Cull.
            points = HullCull(points);

            // Find the remaining point with the smallest Y value.
            // if (there's a tie, take the one with the smaller X value.
            Vector2[] bestPt = { points[0] };
            foreach (var pt in
                points.Where(
                    pt =>
                        (pt.Y < bestPt[0].Y)
                        || ((Math.Abs(pt.Y - bestPt[0].Y) < float.Epsilon) && (pt.X < bestPt[0].X))))
            {
                bestPt[0] = pt;
            }

            // Move this point to the convex hull.
            var hull = new List<Vector2> { bestPt[0] };
            points.Remove(bestPt[0]);

            // Start wrapping up the other points.
            float sweepAngle = 0;
            for (; ;)
            {
                // If all of the points are on the hull, we're done.
                if (points.Count == 0)
                {
                    break;
                }

                // Find the point with smallest AngleValue
                // from the last point.
                var x = hull[hull.Count - 1].X;
                var y = hull[hull.Count - 1].Y;
                bestPt[0] = points[0];
                float bestAngle = 3600;

                // Search the rest of the points.
                foreach (var pt in points)
                {
                    var testAngle = AngleValue(x, y, pt.X, pt.Y);
                    if ((testAngle >= sweepAngle) && (bestAngle > testAngle))
                    {
                        bestAngle = testAngle;
                        bestPt[0] = pt;
                    }
                }

                // See if the first point is better.
                // If so, we are done.
                var firstAngle = AngleValue(x, y, hull[0].X, hull[0].Y);
                if ((firstAngle >= sweepAngle) && (bestAngle >= firstAngle))
                {
                    // The first point is better. We're done.
                    break;
                }

                // Add the best point to the convex hull.
                hull.Add(bestPt[0]);
                points.Remove(bestPt[0]);

                sweepAngle = bestAngle;
            }

            return hull;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Return a number that gives the ordering of angles
        ///     WRST horizontal from the point (x1, y1) to (x2, y2).
        ///     In other words, AngleValue(x1, y1, x2, y2) is not
        ///     the angle, but if:
        ///     Angle(x1, y1, x2, y2) > Angle(x1, y1, x2, y2)
        ///     then
        ///     AngleValue(x1, y1, x2, y2) > AngleValue(x1, y1, x2, y2)
        ///     this angle is greater than the angle for another set
        ///     of points,) this number for
        ///     This function is <c>dy</c> / (<c>dy</c> + dx).
        /// </summary>
        /// <param name="x1">
        ///     First X.
        /// </param>
        /// <param name="y1">
        ///     First Y.
        /// </param>
        /// <param name="x2">
        ///     Second X.
        /// </param>
        /// <param name="y2">
        ///     Second Y.
        /// </param>
        /// <returns>
        ///     The <see cref="float" />.
        /// </returns>
        private static float AngleValue(float x1, float y1, float x2, float y2)
        {
            float t;

            var dx = x2 - x1;
            var ax = Math.Abs(dx);
            var dy = y2 - y1;
            var ay = Math.Abs(dy);
            if ((ax + ay).Equals(0))
            {
                // if (the two points are the same, return 360.
                t = 360f / 9f;
            }
            else
            {
                t = dy / (ax + ay);
            }

            if (dx < 0)
            {
                t = 2 - t;
            }
            else if (dy < 0)
            {
                t = 4 + t;
            }

            return t * 90;
        }

        /// <summary>
        ///     Returns whether the indicated circle encloses all of the points.
        /// </summary>
        /// <param name="center">
        ///     Center of the Circle.
        /// </param>
        /// <param name="radius2">
        ///     Circle Radius.
        /// </param>
        /// <param name="points">
        ///     Points List.
        /// </param>
        /// <param name="skip1">
        ///     Skip certain point 1.
        /// </param>
        /// <param name="skip2">
        ///     Skip certain point 2.
        /// </param>
        /// <param name="skip3">
        ///     Skip certain point 3.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        private static bool CircleEnclosesPoints(
            Vector2 center,
            float radius2,
            IEnumerable<Vector2> points,
            int skip1,
            int skip2,
            int skip3)
        {
            return (from point in points.Where((t, i) => (i != skip1) && (i != skip2) && (i != skip3))
                    let dx = center.X - point.X
                    let dy = center.Y - point.Y
                    select (dx * dx) + (dy * dy)).All(testRadius2 => !(testRadius2 > radius2));
        }

        /// <summary>
        ///     Find a circle through three Vector2 points
        /// </summary>
        /// <param name="a">
        ///     Vector2 point A.
        /// </param>
        /// <param name="b">
        ///     Vector2 point B.
        /// </param>
        /// <param name="c">
        ///     Vector2 point C.
        /// </param>
        /// <param name="center">
        ///     Returned Vector2 Center.
        /// </param>
        /// <param name="radius2">
        ///     Retuned Circle Radius.
        /// </param>
        private static void FindCircle(Vector2 a, Vector2 b, Vector2 c, out Vector2 center, out float radius2)
        {
            // Get the perpendicular bisector of (x1, y1) and (x2, y2).
            var x1 = (b.X + a.X) / 2;
            var y1 = (b.Y + a.Y) / 2;
            var dy1 = b.X - a.X;
            var dx1 = -(b.Y - a.Y);

            // Get the perpendicular bisector of (x2, y2) and (x3, y3).
            var x2 = (c.X + b.X) / 2;
            var y2 = (c.Y + b.Y) / 2;
            var dy2 = c.X - b.X;
            var dx2 = -(c.Y - b.Y);

            // See where the lines intersect.
            var cx = ((y1 * dx1 * dx2) + (x2 * dx1 * dy2) - (x1 * dy1 * dx2) - (y2 * dx1 * dx2))
                     / ((dx1 * dy2) - (dy1 * dx2));
            var cy = ((cx - x1) * dy1 / dx1) + y1;
            center = new Vector2(cx, cy);

            var dx = cx - a.X;
            var dy = cy - a.Y;
            radius2 = (dx * dx) + (dy * dy);
        }

        /// <summary>
        ///     Find a box that fits inside the MinMax quadrilateral.
        /// </summary>
        /// <param name="points">
        ///     The Points.
        /// </param>
        /// <returns>
        ///     <see cref="RectangleF" />.
        /// </returns>
        private static RectangleF GetMinMaxBox(IEnumerable<Vector2> points)
        {
            // Find the MinMax quadrilateral.
            Vector2 ul = new Vector2(0, 0), ur = ul, ll = ul, lr = ul;
            var minMaxCornersInfo = GetMinMaxCorners(points, ul, ur, ll, lr);
            ul = minMaxCornersInfo.UpperLeft;
            ur = minMaxCornersInfo.UpperRight;
            ll = minMaxCornersInfo.LowerLeft;
            lr = minMaxCornersInfo.LowerRight;

            // Get the coordinates of a box that lies inside this quadrilateral.
            var xmin = ul.X;
            var ymin = ul.Y;

            var xmax = ur.X;
            if (ymin < ur.Y)
            {
                ymin = ur.Y;
            }

            if (xmax > lr.X)
            {
                xmax = lr.X;
            }

            var ymax = lr.Y;

            if (xmin < ll.X)
            {
                xmin = ll.X;
            }

            if (ymax > ll.Y)
            {
                ymax = ll.Y;
            }

            var result = new RectangleF(xmin, ymin, xmax - xmin, ymax - ymin);
            g_MinMaxBox = result;
            return result;
        }

        /// <summary>
        ///     Find the points nearest the upper left, upper right, lower left, and lower right corners.
        /// </summary>
        /// <param name="points">
        ///     The Points.
        /// </param>
        /// <param name="upperLeft">
        ///     Upper left <see cref="Vector2" />.
        /// </param>
        /// <param name="upperRight">
        ///     Upper right <see cref="Vector2" />.
        /// </param>
        /// <param name="lowerLeft">
        ///     Lower left <see cref="Vector2" />.
        /// </param>
        /// <param name="lowerRight">
        ///     Lower right <see cref="Vector2" />.
        /// </param>
        /// <returns>
        ///     The <see cref="Vector2" /> list.
        /// </returns>
        private static MinMaxCornersInfo GetMinMaxCorners(
            IEnumerable<Vector2> points,
            Vector2 upperLeft,
            Vector2 upperRight,
            Vector2 lowerLeft,
            Vector2 lowerRight)
        {
            // Search the other points.
            foreach (var pt in points)
            {
                if (-pt.X - pt.Y > -upperLeft.X - upperLeft.Y)
                {
                    upperLeft = pt;
                }

                if (pt.X - pt.Y > upperRight.X - upperRight.Y)
                {
                    upperRight = pt;
                }

                if (-pt.X + pt.Y > -lowerLeft.X + lowerLeft.Y)
                {
                    lowerLeft = pt;
                }

                if (pt.X + pt.Y > lowerRight.X + lowerRight.Y)
                {
                    lowerRight = pt;
                }
            }

            g_MinMaxCorners = new[] { upperLeft, upperRight, lowerRight, lowerLeft };
            return new MinMaxCornersInfo(upperLeft, upperRight, lowerLeft, lowerRight);
        }

        /// <summary>
        ///     Cull points out of the convex hull that lie inside the trapezoid defined by the vertices with smallest and largest
        ///     X and Y coordinates. Return the points that are not culled.
        /// </summary>
        /// <param name="points">
        ///     The Points.
        /// </param>
        /// <returns>
        ///     List of <see cref="Vector2" />.
        /// </returns>
        private static List<Vector2> HullCull(IReadOnlyList<Vector2> points)
        {
            // Find a culling box.
            var cullingBox = GetMinMaxBox(points);

            var results =
                points.Where(
                    pt =>
                        pt.X <= cullingBox.Left || pt.X >= cullingBox.Right || pt.Y <= cullingBox.Top
                        || pt.Y >= cullingBox.Bottom).ToList();

            g_NonCulledPoints = new Vector2[results.Count];
            results.CopyTo(g_NonCulledPoints);

            return results;
        }

        #endregion
    }
}