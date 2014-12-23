﻿// This file is part of LeagueSharp.Common.
// 
// LeagueSharp.Common is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// LeagueSharp.Common is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with LeagueSharp.Common.  If not, see <http://www.gnu.org/licenses/>.

#region

using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    public static class MEC
    {
        // For debugging.
        public static Vector2[] g_MinMaxCorners;
        public static RectangleF g_MinMaxBox;
        public static Vector2[] g_NonCulledPoints;

        /// <summary>
        /// Returns the mininimum enclosing circle from a list of points.
        /// </summary>
        public static MecCircle GetMec(List<Vector2> points)
        {
            var center = new Vector2();
            float radius;

            var ConvexHull = MakeConvexHull(points);
            FindMinimalBoundingCircle(ConvexHull, out center, out radius);
            return new MecCircle(center, radius);
        }

        // Find the points nearest the upper left, upper right,
        // lower left, and lower right corners.
        private static void GetMinMaxCorners(List<Vector2> points,
            ref Vector2 ul,
            ref Vector2 ur,
            ref Vector2 ll,
            ref Vector2 lr)
        {
            // Start with the first point as the solution.
            ul = points[0];
            ur = ul;
            ll = ul;
            lr = ul;

            // Search the other points.
            foreach (var pt in points)
            {
                if (-pt.X - pt.Y > -ul.X - ul.Y)
                {
                    ul = pt;
                }
                if (pt.X - pt.Y > ur.X - ur.Y)
                {
                    ur = pt;
                }
                if (-pt.X + pt.Y > -ll.X + ll.Y)
                {
                    ll = pt;
                }
                if (pt.X + pt.Y > lr.X + lr.Y)
                {
                    lr = pt;
                }
            }

            g_MinMaxCorners = new[] { ul, ur, lr, ll }; // For debugging.
        }

        // Find a box that fits inside the MinMax quadrilateral.
        private static RectangleF GetMinMaxBox(List<Vector2> points)
        {
            // Find the MinMax quadrilateral.
            Vector2 ul = new Vector2(0, 0), ur = ul, ll = ul, lr = ul;
            GetMinMaxCorners(points, ref ul, ref ur, ref ll, ref lr);

            // Get the coordinates of a box that lies inside this quadrilateral.
            float xmin, xmax, ymin, ymax;
            xmin = ul.X;
            ymin = ul.Y;

            xmax = ur.X;
            if (ymin < ur.Y)
            {
                ymin = ur.Y;
            }

            if (xmax > lr.X)
            {
                xmax = lr.X;
            }
            ymax = lr.Y;

            if (xmin < ll.X)
            {
                xmin = ll.X;
            }
            if (ymax > ll.Y)
            {
                ymax = ll.Y;
            }

            var result = new RectangleF(xmin, ymin, xmax - xmin, ymax - ymin);
            g_MinMaxBox = result; // For debugging.
            return result;
        }

        // Cull points out of the convex hull that lie inside the
        // trapezoid defined by the vertices with smallest and
        // largest X and Y coordinates.
        // Return the points that are not culled.
        private static List<Vector2> HullCull(List<Vector2> points)
        {
            // Find a culling box.
            var culling_box = GetMinMaxBox(points);

            // Cull the points.
            var results =
                points.Where(
                    pt =>
                        pt.X <= culling_box.Left || pt.X >= culling_box.Right || pt.Y <= culling_box.Top ||
                        pt.Y >= culling_box.Bottom).ToList();

            g_NonCulledPoints = new Vector2[results.Count]; // For debugging.
            results.CopyTo(g_NonCulledPoints); // For debugging.
            return results;
        }

        // Return the points that make up a polygon's convex hull.
        // This method leaves the points list unchanged.
        public static List<Vector2> MakeConvexHull(List<Vector2> points)
        {
            // Cull.
            points = HullCull(points);

            // Find the remaining point with the smallest Y value.
            // if (there's a tie, take the one with the smaller X value.
            var best_pt = points[0];
            foreach (var pt in points)
            {
                if ((pt.Y < best_pt.Y) || ((pt.Y == best_pt.Y) && (pt.X < best_pt.X)))
                {
                    best_pt = pt;
                }
            }

            // Move this point to the convex hull.
            var hull = new List<Vector2>();
            hull.Add(best_pt);
            points.Remove(best_pt);

            // Start wrapping up the other points.
            float sweep_angle = 0;
            for (;;)
            {
                // If all of the points are on the hull, we're done.
                if (points.Count == 0)
                {
                    break;
                }

                // Find the point with smallest AngleValue
                // from the last point.
                var X = hull[hull.Count - 1].X;
                var Y = hull[hull.Count - 1].Y;
                best_pt = points[0];
                float best_angle = 3600;

                // Search the rest of the points.
                foreach (var pt in points)
                {
                    var test_angle = AngleValue(X, Y, pt.X, pt.Y);
                    if ((test_angle >= sweep_angle) && (best_angle > test_angle))
                    {
                        best_angle = test_angle;
                        best_pt = pt;
                    }
                }

                // See if the first point is better.
                // If so, we are done.
                var first_angle = AngleValue(X, Y, hull[0].X, hull[0].Y);
                if ((first_angle >= sweep_angle) && (best_angle >= first_angle))
                {
                    // The first point is better. We're done.
                    break;
                }

                // Add the best point to the convex hull.
                hull.Add(best_pt);
                points.Remove(best_pt);

                sweep_angle = best_angle;
            }

            return hull;
        }

        // Return a number that gives the ordering of angles
        // WRST horizontal from the point (x1, y1) to (x2, y2).
        // In other words, AngleValue(x1, y1, x2, y2) is not
        // the angle, but if:
        //   Angle(x1, y1, x2, y2) > Angle(x1, y1, x2, y2)
        // then
        //   AngleValue(x1, y1, x2, y2) > AngleValue(x1, y1, x2, y2)
        // this angle is greater than the angle for another set
        // of points,) this number for
        //
        // This function is dy / (dy + dx).
        private static float AngleValue(float x1, float y1, float x2, float y2)
        {
            float dx, dy, ax, ay, t;

            dx = x2 - x1;
            ax = Math.Abs(dx);
            dy = y2 - y1;
            ay = Math.Abs(dy);
            if (ax + ay == 0)
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

        // Find a minimal bounding circle.
        public static void FindMinimalBoundingCircle(List<Vector2> points, out Vector2 center, out float radius)
        {
            // Find the convex hull.
            var hull = MakeConvexHull(points);

            // The best solution so far.
            var best_center = points[0];
            var best_radius2 = float.MaxValue;

            // Look at pairs of hull points.
            for (var i = 0; i < hull.Count - 1; i++)
            {
                for (var j = i + 1; j < hull.Count; j++)
                {
                    // Find the circle through these two points.
                    var test_center = new Vector2((hull[i].X + hull[j].X) / 2f, (hull[i].Y + hull[j].Y) / 2f);
                    var dx = test_center.X - hull[i].X;
                    var dy = test_center.Y - hull[i].Y;
                    var test_radius2 = dx * dx + dy * dy;

                    // See if this circle would be an improvement.
                    if (test_radius2 < best_radius2)
                    {
                        // See if this circle encloses all of the points.
                        if (CircleEnclosesPoints(test_center, test_radius2, points, i, j, -1))
                        {
                            // Save this solution.
                            best_center = test_center;
                            best_radius2 = test_radius2;
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
                        Vector2 test_center;
                        float test_radius2;
                        FindCircle(hull[i], hull[j], hull[k], out test_center, out test_radius2);

                        // See if this circle would be an improvement.
                        if (test_radius2 < best_radius2)
                        {
                            // See if this circle encloses all of the points.
                            if (CircleEnclosesPoints(test_center, test_radius2, points, i, j, k))
                            {
                                // Save this solution.
                                best_center = test_center;
                                best_radius2 = test_radius2;
                            }
                        }
                    } // for k
                } // for i
            } // for j

            center = best_center;
            if (best_radius2 == float.MaxValue)
            {
                radius = 0;
            }
            else
            {
                radius = (float) Math.Sqrt(best_radius2);
            }
        }

        // Return true if the indicated circle encloses all of the points.
        private static bool CircleEnclosesPoints(Vector2 center,
            float radius2,
            List<Vector2> points,
            int skip1,
            int skip2,
            int skip3)
        {
            for (var i = 0; i < points.Count; i++)
            {
                if ((i != skip1) && (i != skip2) && (i != skip3))
                {
                    var point = points[i];
                    var dx = center.X - point.X;
                    var dy = center.Y - point.Y;
                    var test_radius2 = dx * dx + dy * dy;
                    if (test_radius2 > radius2)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // Find a circle through the three points.
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
            var cx = (y1 * dx1 * dx2 + x2 * dx1 * dy2 - x1 * dy1 * dx2 - y2 * dx1 * dx2) / (dx1 * dy2 - dy1 * dx2);
            var cy = (cx - x1) * dy1 / dx1 + y1;
            center = new Vector2(cx, cy);

            var dx = cx - a.X;
            var dy = cy - a.Y;
            radius2 = dx * dx + dy * dy;
        }

        public struct MecCircle
        {
            public Vector2 Center;
            public float Radius;

            public MecCircle(Vector2 center, float radius)
            {
                Center = center;
                Radius = radius;
            }
        }
    }
}