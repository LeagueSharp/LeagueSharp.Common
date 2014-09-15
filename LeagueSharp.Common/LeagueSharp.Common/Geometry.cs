#region LICENSE
/*
 Copyright 2014 - 2014 LeagueSharp
 Orbwalking.cs is part of LeagueSharp.Common.
 
 LeagueSharp.Common is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 LeagueSharp.Common is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with LeagueSharp.Common. If not, see <http://www.gnu.org/licenses/>.
*/
#endregion


#region

using System;
using System.Collections.Generic;

using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    public static class Geometry
    {
        //Obj_AI_Base class extended methods:
        public static float Distance(Obj_AI_Base anotherUnit, bool squared = false)
        {
            return ObjectManager.Player.Distance(anotherUnit, squared);
        }

        /// <summary>
        ///     Calculates the 2D distance to the unit.
        /// </summary>
        public static float Distance(this Obj_AI_Base unit, Obj_AI_Base anotherUnit, bool squared = false)
        {
            return unit.ServerPosition.To2D().Distance(anotherUnit.ServerPosition.To2D(), squared);
        }

        /// <summary>
        ///     Calculates the 2D distance to the point.
        /// </summary>
        public static float Distance(this Obj_AI_Base unit, Vector3 point, bool squared = false)
        {
            return unit.ServerPosition.To2D().Distance(point.To2D(), squared);
        }

        /// <summary>
        ///     Calculates the 2D distance to the point.
        /// </summary>
        public static float Distance(this Obj_AI_Base unit, Vector2 point, bool squared = false)
        {
            return unit.ServerPosition.To2D().Distance(point, squared);
        }

        /// <summary>
        ///     Calculates the 3D distance to the unit.
        /// </summary>
        public static float Distance3D(this Obj_AI_Base unit, Obj_AI_Base anotherUnit, bool squared = false)
        {
            return squared ? Vector3.DistanceSquared(unit.Position, anotherUnit.Position) :  Vector3.Distance(unit.Position, anotherUnit.Position);
        }

        //Vector3 class extended methods:

        /// <summary>
        ///     Converts a Vector3 to Vector2
        /// </summary>
        public static Vector2 To2D(this Vector3 v)
        {
            return new Vector2(v.X, v.Y);
        }

        /// <summary>
        ///     Returns the 2D distance (XY plane) between two vector.
        /// </summary>
        public static float Distance(this Vector3 v, Vector3 other, bool squared = false)
        {
            return v.To2D().Distance(other, squared);
        }

        //Vector2 class extended methods:

        /// <summary>
        /// Returns true if the Vector2 is valid.
        /// </summary>
        public static bool IsValid(this Vector2 v)
        {
            return (v.X != 0 && v.Y != 0);
        }

        /// <summary>
        ///     Converts the Vector2 to Vector3. (Z = Player.ServerPosition.Z)
        /// </summary>
        public static Vector3 To3D(this Vector2 v)
        {
            return new Vector3(v.X, v.Y, ObjectManager.Player.ServerPosition.Z);
        }

        /// <summary>
        ///     Calculates the distance to the Vector2.
        /// </summary>
        public static float Distance(this Vector2 v, Vector2 to, bool squared = false)
        {
            return squared ? Vector2.DistanceSquared(v, to) : Vector2.Distance(v, to);
        }

        /// <summary>
        ///     Calculates the distance to the Vector3.
        /// </summary>
        public static float Distance(this Vector2 v, Vector3 to, bool squared = false)
        {
            return v.Distance(to.To2D(), squared);
        }

        /// <summary>
        ///     Calculates the distance to the unit.
        /// </summary>
        public static float Distance(this Vector2 v, Obj_AI_Base to, bool squared = false)
        {
            return v.Distance(to.ServerPosition.To2D());
        }

        /// <summary>
        /// Retursn the distance to the line segment.
        /// </summary>
        public static float Distance(this Vector2 point,
            Vector2 segmentStart,
            Vector2 segmentEnd,
            bool onlyIfOnSegment = false,
            bool squared = false)
        {
            var objects = point.ProjectOn(segmentStart, segmentEnd);

            if (objects.IsOnSegment || onlyIfOnSegment == false)
            {
                return squared
                    ? Vector2.DistanceSquared(objects.SegmentPoint, point)
                    : Vector2.Distance(objects.SegmentPoint, point);
            }
            return float.MaxValue;
        }

        /// <summary>
        ///     Returns the vector normalized.
        /// </summary>
        public static Vector2 Normalized(this Vector2 v)
        {
            v.Normalize();
            return v;
        }

        public static Vector2 Extend(this Vector2 v, Vector2 to, float distance)
        {
            return v + distance * (to - v).Normalized();
        }

        public static Vector3 SwitchYZ(this Vector3 v)
        {
            return new Vector3(v.X, v.Z, v.Y);
        }

        /// <summary>
        ///     Returns the perpendicular vector.
        /// </summary>
        public static Vector2 Perpendicular(this Vector2 v)
        {
            return new Vector2(-v.Y, v.X);
        }

        /// <summary>
        ///     Returns the second perpendicular vector.
        /// </summary>
        public static Vector2 Perpendicular2(this Vector2 v)
        {
            return new Vector2(v.Y, -v.X);
        }

        /// <summary>
        ///     Rotates the vector a set angle (angle in radians).
        /// </summary>
        public static Vector2 Rotated(this Vector2 v, float angle)
        {
            double c;
            double s;
            c = Math.Cos(angle);
            s = Math.Sin(angle);

            return new Vector2((float) (v.X * c - v.Y * s), (float) (v.Y * c + v.X * s));
        }

        /// <summary>
        ///     Returns the cross product Z value.
        /// </summary>
        public static float CrossProduct(this Vector2 self, Vector2 other)
        {
            return other.Y * self.X - other.X * self.Y;
        }

        public static float RadianToDegree(double angle)
        {
            return (float) (angle * (180.0 / Math.PI));
        }

        public static float DegreeToRadian(double angle)
        {
            return (float) (Math.PI * angle / 180.0);
        }

        /// <summary>
        ///     Returns the polar for vector angle (in Degrees).
        /// </summary>
        public static float Polar(this Vector2 v1)
        {
            if (Close(v1.X, 0, 0))
            {
                if (v1.Y > 0)
                {
                    return 90;
                }
                if (v1.Y < 0)
                {
                    return 270;
                }

                return 0;
            }

            var theta = RadianToDegree(Math.Atan((v1.Y) / v1.X));
            if (v1.X < 0)
            {
                theta = theta + 180;
            }
            if (theta < 0)
            {
                theta = theta + 360;
            }
            return theta;
        }

        /// <summary>
        ///     Returns the angle with the vector p2.
        /// </summary>
        public static float AngleBetween(this Vector2 p1, Vector2 p2)
        {
            var theta = p1.Polar() - p2.Polar();
            if (theta < 0)
            {
                theta = theta + 360;
            }
            if (theta > 180)
            {
                theta = 360 - theta;
            }
            return theta;
        }

        /// <summary>
        /// Returns the closest vector from a list.
        /// </summary>
        public static Vector2 Closest(this Vector2 v, List<Vector2> vList)
        {
            var result = new Vector2();
            var dist = float.MaxValue;

            foreach (var vector in vList)
            {
                var distance = Vector2.DistanceSquared(v, vector);
                if (distance < dist)
                {
                    dist = distance;
                    result = vector;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the projection of the Vector2 on the segment.
        /// </summary>
        public static ProjectionInfo ProjectOn(this Vector2 point, Vector2 segmentStart, Vector2 segmentEnd)
        {
            var cx = point.X;
            var cy = point.Y;
            var ax = segmentStart.X;
            var ay = segmentStart.Y;
            var bx = segmentEnd.X;
            var by = segmentEnd.Y;
            var rL = ((cx - ax) * (bx - ax) + (cy - ay) * (by - ay)) /
                     ((float) Math.Pow(bx - ax, 2) + (float) Math.Pow(by - ay, 2));
            var pointLine = new Vector2(ax + rL * (bx - ax), ay + rL * (by - ay));
            float rS;
            if (rL < 0)
            {
                rS = 0;
            }
            else if (rL > 1)
            {
                rS = 1;
            }
            else
            {
                rS = rL;
            }

            var isOnSegment = rS.CompareTo(rL) == 0;
            var pointSegment = new Vector2();
            pointSegment = isOnSegment ? pointLine : new Vector2(ax + rS * (bx - ax), ay + rS * (@by - ay));
            return new ProjectionInfo(isOnSegment, pointSegment, pointLine);
        }


        //From: http://social.msdn.microsoft.com/Forums/vstudio/en-US/e5993847-c7a9-46ec-8edc-bfb86bd689e3/help-on-line-segment-intersection-algorithm
        /// <summary>
        /// Intersects two line segments.
        /// </summary>
        public static IntersectionResult Intersection(this Vector2 lineSegment1Start,
            Vector2 lineSegment1End,
            Vector2 lineSegment2Start,
            Vector2 lineSegment2End)
        {
            double deltaACy = lineSegment1Start.Y - lineSegment2Start.Y;
            double deltaDCx = lineSegment2End.X - lineSegment2Start.X;
            double deltaACx = lineSegment1Start.X - lineSegment2Start.X;
            double deltaDCy = lineSegment2End.Y - lineSegment2Start.Y;
            double deltaBAx = lineSegment1End.X - lineSegment1Start.X;
            double deltaBAy = lineSegment1End.Y - lineSegment1Start.Y;

            var denominator = deltaBAx * deltaDCy - deltaBAy * deltaDCx;
            var numerator = deltaACy * deltaDCx - deltaACx * deltaDCy;

            if (denominator == 0)
            {
                if (numerator == 0)
                {
                    // collinear. Potentially infinite intersection points.
                    // Check and return one of them.
                    if (lineSegment1Start.X >= lineSegment2Start.X && lineSegment1Start.X <= lineSegment2End.X)
                    {
                        return new IntersectionResult(true, lineSegment1Start);
                    }
                    if (lineSegment2Start.X >= lineSegment1Start.X && lineSegment2Start.X <= lineSegment1End.X)
                    {
                        return new IntersectionResult(true, lineSegment2Start);
                    }
                    return new IntersectionResult();
                }
                // parallel
                return new IntersectionResult();
            }

            var r = numerator / denominator;
            if (r < 0 || r > 1)
            {
                return new IntersectionResult();
            }

            var s = (deltaACy * deltaBAx - deltaACx * deltaBAy) / denominator;
            if (s < 0 || s > 1)
            {
                return new IntersectionResult();
            }

            return new IntersectionResult(
                true,
                new Vector2((float) (lineSegment1Start.X + r * deltaBAx), (float) (lineSegment1Start.Y + r * deltaBAy)));
        }

        public static Object[] VectorMovementCollision(Vector2 startPoint1,
            Vector2 endPoint1,
            float v1,
            Vector2 startPoint2,
            float v2,
            float delay = 0f)
        {
            float sP1x = startPoint1.X,
                sP1y = startPoint1.Y,
                eP1x = endPoint1.X,
                eP1y = endPoint1.Y,
                sP2x = startPoint2.X,
                sP2y = startPoint2.Y;

            float d = eP1x - sP1x, e = eP1y - sP1y;
            float dist = (float) Math.Sqrt(d * d + e * e), t1 = float.NaN, t2 = float.NaN;
            float S = dist != 0f ? v1 * d / dist : 0, K = (dist != 0) ? v1 * e / dist : 0f;

            float r = sP2x - sP1x, j = sP2y - sP1y;
            var c = r * r + j * j;


            if (dist > 0f)
            {
                if (v1 == float.MaxValue)
                {
                    var t = dist / v1;
                    t1 = v2 * t >= 0f ? t : float.NaN;
                }
                else if (v2 == float.MaxValue)
                {
                    t1 = 0f;
                }
                else
                {
                    float a = S * S + K * K - v2 * v2, b = -r * S - j * K;

                    if (a == 0f)
                    {
                        if (b == 0f)
                        {
                            t1 = (c == 0f) ? 0f : float.NaN;
                        }
                        else
                        {
                            var t = -c / (2 * b);
                            t1 = (v2 * t >= 0f) ? t : float.NaN;
                        }
                    }
                    else
                    {
                        var sqr = b * b - a * c;
                        if (sqr >= 0)
                        {
                            var nom = (float) Math.Sqrt(sqr);
                            var t = (-nom - b) / a;
                            t1 = v2 * t >= 0f ? t : float.NaN;
                            t = (nom - b) / a;
                            t2 = (v2 * t >= 0f) ? t : float.NaN;

                            if (!float.IsNaN(t2) && !float.IsNaN(t1))
                            {
                                if(t1 >= delay && t2 >= delay)
                                    t1 = Math.Min(t1, t2);
                                else if (t2 >= delay)
                                    t1 = t2;
                            }
                        }
                    }
                }
            }
            else if (dist == 0f)
            {
                t1 = 0f;
            }

            return new Object[2] { t1, (!float.IsNaN(t1)) ? new Vector2(sP1x + S * t1, sP1y + K * t1) : new Vector2() };
        }

        /// <summary>
        /// Returns the total distance of a path.
        /// </summary>
        public static float PathLength(this List<Vector2> path)
        {
            var distance = 0f;
            for (var i = 0; i < path.Count - 1; i++)
            {
                distance += path[i].Distance(path[i + 1]);
            }
            return distance;
        }

        /// <summary>
        /// Converts a 3D path to 2D
        /// </summary>
        public static List<Vector2> To2D(this List<Vector3> path)
        {
            var result = new List<Vector2>();
            foreach (var point in path)
            {
                result.Add(point.To2D());
            }
            return result;
        }


        /// <summary>
        /// Returns the two intersection points between two circles.
        /// </summary>
        public static Vector2[] CircleCircleIntersection(Vector2 center1, Vector2 center2, float radius1, float radius2)
        {
            var D = center1.Distance(center2);
            //The Circles dont intersect:
            if (D > radius1 + radius2)
            {
                return new Vector2[] { };
            }

            var A = (radius1 * radius1 - radius2 * radius2 + D * D) / (2 * D);
            var H = (float) Math.Sqrt(radius1 * radius1 - A * A);
            var Direction = (center2 - center1).Normalized();
            var PA = center1 + A * Direction;
            var S1 = PA + H * Direction.Perpendicular();
            var S2 = PA - H * Direction.Perpendicular();
            return new[] { S1, S2 };
        }

        public static bool Close(float a, float b, float eps)
        {
            if (eps == 0)
            {
                eps = (float) 1e-9;
            }
            return Math.Abs(a - b) <= eps;
        }

        public struct IntersectionResult
        {
            public bool Intersects;
            public Vector2 Point;

            public IntersectionResult(bool Intersects = false, Vector2 Point = new Vector2())
            {
                this.Intersects = Intersects;
                this.Point = Point;
            }
        }

        public struct ProjectionInfo
        {
            public bool IsOnSegment;
            public Vector2 LinePoint;
            public Vector2 SegmentPoint;

            public ProjectionInfo(bool isOnSegment, Vector2 segmentPoint, Vector2 linePoint)
            {
                IsOnSegment = isOnSegment;
                SegmentPoint = segmentPoint;
                LinePoint = linePoint;
            }
        }
    }
}