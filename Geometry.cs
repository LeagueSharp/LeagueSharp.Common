#region LICENSE

/*
 Copyright 2014 - 2015 LeagueSharp
 Geometry.cs is part of LeagueSharp.Common.
 
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
using System.Linq;
using ClipperLib;
using SharpDX;
using Color = System.Drawing.Color;

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
        ///     Calculates the 2D distance to the unit.
        /// </summary>
        public static float Distance(this Obj_AI_Base unit, AttackableUnit anotherUnit, bool squared = false)
        {
            return unit.ServerPosition.To2D().Distance(anotherUnit.Position.To2D(), squared);
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
            return squared
                ? Vector3.DistanceSquared(unit.Position, anotherUnit.Position)
                : Vector3.Distance(unit.Position, anotherUnit.Position);
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
        ///     Returns true if the vector is valid.
        /// </summary>
        public static bool IsValid(this Vector2 v)
        {
            return v != Vector2.Zero;
        }

        public static bool IsValid(this Vector3 v)
        {
            return v != Vector3.Zero;
        }

        /// <summary>
        ///     Converts the Vector2 to Vector3. (Z = Player.ServerPosition.Z)
        /// </summary>
        public static Vector3 To3D(this Vector2 v)
        {
            return new Vector3(v.X, v.Y, ObjectManager.Player.ServerPosition.Z);
        }

        /// <summary>
        ///     Converts the Vector2 to Vector3. (Z = NavMesh.GetHeightForPosition)
        /// </summary>
        public static Vector3 To3D2(this Vector2 v)
        {
            return new Vector3(v.X, v.Y, NavMesh.GetHeightForPosition(v.X, v.Y));
        }

        public static Vector3 SetZ(this Vector3 v, float? value = null)
        {
            if (value == null)
            {
                v.Z = Game.CursorPos.Z;
            }
            else
            {
                v.Z = (float) value;
            }
            return v;
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
            return v.Distance(to.ServerPosition.To2D(), squared);
        }

        /// <summary>
        ///     Retursn the distance to the line segment.
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

        public static Vector3 Normalized(this Vector3 v)
        {
            v.Normalize();
            return v;
        }

        public static Vector2 Extend(this Vector2 v, Vector2 to, float distance)
        {
            return v + distance * (to - v).Normalized();
        }

        public static Vector3 Extend(this Vector3 v, Vector3 to, float distance)
        {
            return v + distance * (to - v).Normalized();
        }
        
        public static Vector2 Shorten(this Vector2 v, Vector2 to, float distance)
        {
            return v - distance * (to - v).Normalized();
        }

        public static Vector3 Shorten(this Vector3 v, Vector3 to, float distance)
        {
            return v - distance * (to - v).Normalized();
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
            var c = Math.Cos(angle);
            var s = Math.Sin(angle);

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
                return v1.Y < 0 ? 270 : 0;
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
        ///     Returns the angle with the vector p2 in degrees;
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
        ///     Returns the closest vector from a list.
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
        ///     Returns the projection of the Vector2 on the segment.
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
            var pointSegment = isOnSegment ? pointLine : new Vector2(ax + rS * (bx - ax), ay + rS * (@by - ay));
            return new ProjectionInfo(isOnSegment, pointSegment, pointLine);
        }

        //From: http://social.msdn.microsoft.com/Forums/vstudio/en-US/e5993847-c7a9-46ec-8edc-bfb86bd689e3/help-on-line-segment-intersection-algorithm
        /// <summary>
        ///     Intersects two line segments.
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

            if (Math.Abs(denominator) < float.Epsilon)
            {
                if (Math.Abs(numerator) < float.Epsilon)
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
            float dist = (float) Math.Sqrt(d * d + e * e), t1 = float.NaN;
            float S = Math.Abs(dist) > float.Epsilon ? v1 * d / dist : 0,
                K = (Math.Abs(dist) > float.Epsilon) ? v1 * e / dist : 0f;

            float r = sP2x - sP1x, j = sP2y - sP1y;
            var c = r * r + j * j;


            if (dist > 0f)
            {
                if (Math.Abs(v1 - float.MaxValue) < float.Epsilon)
                {
                    var t = dist / v1;
                    t1 = v2 * t >= 0f ? t : float.NaN;
                }
                else if (Math.Abs(v2 - float.MaxValue) < float.Epsilon)
                {
                    t1 = 0f;
                }
                else
                {
                    float a = S * S + K * K - v2 * v2, b = -r * S - j * K;

                    if (Math.Abs(a) < float.Epsilon)
                    {
                        if (Math.Abs(b) < float.Epsilon)
                        {
                            t1 = (Math.Abs(c) < float.Epsilon) ? 0f : float.NaN;
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
                            var t2 = (v2 * t >= 0f) ? t : float.NaN;

                            if (!float.IsNaN(t2) && !float.IsNaN(t1))
                            {
                                if (t1 >= delay && t2 >= delay)
                                {
                                    t1 = Math.Min(t1, t2);
                                }
                                else if (t2 >= delay)
                                {
                                    t1 = t2;
                                }
                            }
                        }
                    }
                }
            }
            else if (Math.Abs(dist) < float.Epsilon)
            {
                t1 = 0f;
            }

            return new Object[] { t1, (!float.IsNaN(t1)) ? new Vector2(sP1x + S * t1, sP1y + K * t1) : new Vector2() };
        }

        /// <summary>
        ///     Returns the total distance of a path.
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
        ///     Converts a 3D path to 2D
        /// </summary>
        public static List<Vector2> To2D(this List<Vector3> path)
        {
            return path.Select(point => point.To2D()).ToList();
        }

        /// <summary>
        ///     Returns the two intersection points between two circles.
        /// </summary>
        public static Vector2[] CircleCircleIntersection(Vector2 center1, Vector2 center2, float radius1, float radius2)
        {
            var D = center1.Distance(center2);
            //The Circles dont intersect:
            if (D > radius1 + radius2 || (D <= Math.Abs(radius1 - radius2)))
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
            if (Math.Abs(eps) < float.Epsilon)
            {
                eps = (float) 1e-9;
            }
            return Math.Abs(a - b) <= eps;
        }

        /// <summary>
        ///     Rotates the vector around the set position.
        ///     Angle is in radians.
        /// </summary>
        public static Vector2 RotateAroundPoint(this Vector2 rotated, Vector2 around, float angle)
        {
            var sin = Math.Sin(angle);
            var cos = Math.Cos(angle);

            var x = ((rotated.X - around.X) * cos) - ((around.Y - rotated.Y) * sin) + around.X;
            var y = ((around.Y - rotated.Y) * cos) + ((rotated.X - around.X) * sin) + around.Y;

            return new Vector2((float) x, (float) y);
        }

        /// <summary>
        ///     Rotates the polygon around the set position.
        ///     Angle is in radians.
        /// </summary>
        public static Polygon RotatePolygon(this Polygon polygon, Vector2 around, float angle)
        {
            var p = new Polygon();

            foreach (var polygonePoint in polygon.Points.Select(poinit => poinit.RotateAroundPoint(around, angle)))
            {
                p.Add(polygonePoint);
            }
            return p;
        }

        /// <summary>
        ///     Rotates the polygon around to the set direction.
        /// </summary>
        public static Polygon RotatePolygon(this Polygon polygon, Vector2 around, Vector2 direction)
        {
            var deltaX = around.X - direction.X;
            var deltaY = around.Y - direction.Y;
            var angle = (float) Math.Atan2(deltaY, deltaX);
            return RotatePolygon(polygon, around, angle - DegreeToRadian(90));
        }

        /// <summary>
        ///     Moves the polygone to the set position. It dosent rotate the polygone.
        /// </summary>
        public static Polygon MovePolygone(this Polygon polygon, Vector2 moveTo)
        {
            var p = new Polygon();

            p.Add(moveTo);

            var count = polygon.Points.Count;

            var startPoint = polygon.Points[0];

            for (var i = 1; i < count; i++)
            {
                var polygonePoint = polygon.Points[i];

                p.Add(
                    new Vector2(
                        moveTo.X + (polygonePoint.X - startPoint.X), moveTo.Y + (polygonePoint.Y - startPoint.Y)));
            }
            return p;
        }

        /// <summary>
        ///     Returns a Vector2 at center of the polygone.
        /// </summary>
        public static Vector2 CenterOfPolygone(this Polygon p)
        {
            var cX = 0f;
            var cY = 0f;
            var pc = p.Points.Count;
            foreach (var point in p.Points)
            {
                cX += point.X;
                cY += point.Y;
            }
            return new Vector2(cX / pc, cY / pc);
        }

        /// <summary>
        ///     Joins all the polygones in the list in one polygone if they interect.
        /// </summary>
        public static List<Polygon> JoinPolygons(this List<Polygon> sList)
        {
            var p = ClipPolygons(sList);
            List<List<IntPoint>> tList = new List<List<IntPoint>>();

            Clipper c = new Clipper();
            c.AddPaths(p, PolyType.ptClip, true);
            c.Execute(ClipType.ctUnion, tList, PolyFillType.pftNonZero, PolyFillType.pftNonZero);

            return ToPolygons(tList);
        }
        
        /// <summary>
        ///     Joins all the polygones.
        ///     ClipType: http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/ClipType.htm
        ///     PolyFillType: http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/PolyFillType.htm
        /// </summary>
        public static List<Polygon> JoinPolygons(this List<Polygon> sList, ClipType cType, PolyType pType = PolyType.ptClip, PolyFillType pFType1 = PolyFillType.pftNonZero, PolyFillType pFType2 = PolyFillType.pftNonZero)
        {
            var p = ClipPolygons(sList);
            List<List<IntPoint>> tList = new List<List<IntPoint>>();

            Clipper c = new Clipper();
            c.AddPaths(p, pType, true);
            c.Execute(cType, tList, pFType1, pFType2);

            return ToPolygons(tList);
        }

        public static List<Polygon> ToPolygons(this List<List<IntPoint>> v)
        {
            return v.Select(path => path.ToPolygon()).ToList();
        }

        /// <summary>
        ///     Returns the position where the vector will be after t(time) with s(speed) and delay.
        /// </summary>
        public static Vector2 PositionAfter(this List<Vector2> self, int t, int s, int delay = 0)
        {
            var distance = Math.Max(0, t - delay) * s / 1000;
            for (var i = 0; i <= self.Count - 2; i++)
            {
                var from = self[i];
                var to = self[i + 1];
                var d = (int) to.Distance(from);
                if (d > distance)
                {
                    return from + distance * (to - from).Normalized();
                }
                distance -= d;
            }
            return self[self.Count - 1];
        }

        public static Polygon ToPolygon(this List<IntPoint> v)
        {
            var polygon = new Polygon();
            foreach (var point in v)
            {
                polygon.Add(new Vector2(point.X, point.Y));
            }
            return polygon;
        }

        public static List<List<IntPoint>> ClipPolygons(List<Polygon> polygons)
        {
            var subj = new List<List<IntPoint>>(polygons.Count);
            var clip = new List<List<IntPoint>>(polygons.Count);
            foreach (var polygon in polygons)
            {
                subj.Add(polygon.ToClipperPath());
                clip.Add(polygon.ToClipperPath());
            }
            var solution = new List<List<IntPoint>>();
            var c = new Clipper();
            c.AddPaths(subj, PolyType.ptSubject, true);
            c.AddPaths(clip, PolyType.ptClip, true);
            c.Execute(ClipType.ctUnion, solution, PolyFillType.pftPositive, PolyFillType.pftEvenOdd);
            return solution;
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

        public class Polygon
        {
            public List<Vector2> Points = new List<Vector2>();

            public void Add(Vector2 point)
            {
                Points.Add(point);
            }

            public void Add(Vector3 point)
            {
                Points.Add(point.To2D());
            }

            public void Add(Polygon polygon)
            {
                foreach (var point in polygon.Points)
                {
                    Points.Add(point);
                }
            }

            public List<IntPoint> ToClipperPath()
            {
                var result = new List<IntPoint>(Points.Count);
                result.AddRange(Points.Select(point => new IntPoint(point.X, point.Y)));
                return result;
            }

            public virtual void Draw(Color color, int width = 1)
            {
                for (var i = 0; i <= Points.Count - 1; i++)
                {
                    var nextIndex = (Points.Count - 1 == i) ? 0 : (i + 1);
                    var from = Drawing.WorldToScreen(Points[i].To3D());
                    var to = Drawing.WorldToScreen(Points[nextIndex].To3D());
                    Drawing.DrawLine(from[0], from[1], to[0], to[1], width, color);
                }
            }

            public bool IsInside(Vector2 point)
            {
                return !IsOutside(point);
            }

            public bool IsInside(Vector3 point)
            {
                return !IsOutside(point.To2D());
            }

            public bool IsInside(GameObject point)
            {
                return !IsOutside(point.Position.To2D());
            }

            public bool IsOutside(Vector2 point)
            {
                var p = new IntPoint(point.X, point.Y);
                return Clipper.PointInPolygon(p, ToClipperPath()) != 1;
            }

            public class Arc : Polygon
            {
                public float Angle;
                public Vector2 EndPos;
                public float Radius;
                public Vector2 StartPos;
                private readonly int _quality;

                public Arc(Vector3 start, Vector3 direction, float angle, float radius, int quality = 20)
                    : this(start.To2D(), direction.To2D(), angle, radius, quality) {}

                public Arc(Vector2 start, Vector2 direction, float angle, float radius, int quality = 20)
                {
                    StartPos = start;
                    EndPos = (direction - start).Normalized();
                    Angle = angle;
                    Radius = radius;
                    _quality = quality;
                    UpdatePolygon();
                }

                public void UpdatePolygon(int offset = 0)
                {
                    Points.Clear();
                    var outRadius = (Radius + offset) / (float)Math.Cos(2 * Math.PI / _quality);
                    var side1 = EndPos.Rotated(-Angle * 0.5f);
                    for (var i = 0; i <= _quality; i++)
                    {
                        var cDirection = side1.Rotated(i * Angle / _quality).Normalized();
                        Points.Add(
                            new Vector2(StartPos.X + outRadius * cDirection.X, StartPos.Y + outRadius * cDirection.Y));
                    }
                }
            }

            public class Line : Polygon
            {
                public Vector2 LineStart;
                public Vector2 LineEnd;
                public float Length
                {
                    get { return LineStart.Distance(LineEnd); }
                    set { LineEnd = (LineEnd - LineStart).Normalized() * value + LineStart; }
                }

                public Line(Vector3 start, Vector3 end, float length = -1) : this(start.To2D(), end.To2D(), length) {}

                public Line(Vector2 start, Vector2 end, float length = -1)
                {
                    LineStart = start;
                    LineEnd = end;
                    if (length > 0)
                    {
                        Length = length;
                    }
                    UpdatePolygon();
                }

                public void UpdatePolygon()
                {
                    Points.Clear();
                    Points.Add(LineStart);
                    Points.Add(LineEnd);
                }
            }

            public class Circle : Polygon
            {
                public Vector2 Center;
                public float Radius;
                private readonly int _quality;

                public Circle(Vector3 center, float radius, int quality = 20) : this(center.To2D(), radius, quality) {}

                public Circle(Vector2 center, float radius, int quality = 20)
                {
                    Center = center;
                    Radius = radius;
                    _quality = quality;
                    UpdatePolygon();
                }

               public void UpdatePolygon(int offset = 0, float overrideWidth = -1)
                {
                   Points.Clear();
                    var outRadius = (overrideWidth > 0
                        ? overrideWidth
                        : (offset + Radius) / (float)Math.Cos(2 * Math.PI / _quality));
                    for (var i = 1; i <= _quality; i++)
                    {
                        var angle = i * 2 * Math.PI / _quality;
                        var point = new Vector2(
                            Center.X + outRadius * (float)Math.Cos(angle), Center.Y + outRadius * (float)Math.Sin(angle));
                        Points.Add(point);
                    }
                }
            }

            public class Rectangle : Polygon
            {
                public Vector2 Direction { get { return (End - Start).Normalized(); } }
                public Vector2 Perpendicular { get { return Direction.Perpendicular(); } }
                public Vector2 End;
                public Vector2 Start;
                public float Width;
                public Rectangle(Vector3 start, Vector3 end, float width) : this(start.To2D(), end.To2D(), width) {}

                public Rectangle(Vector2 start, Vector2 end, float width)
                {
                    Start = start;
                    End = end;
                    Width = width;
                    UpdatePolygon();
                }

                public void UpdatePolygon(int offset = 0, float overrideWidth = -1)
                {
                    Points.Clear();
                    Points.Add(
                        Start + (overrideWidth > 0 ? overrideWidth : Width + offset) * Perpendicular - offset * Direction);
                    Points.Add(
                        Start - (overrideWidth > 0 ? overrideWidth : Width + offset) * Perpendicular - offset * Direction);
                    Points.Add(
                        End - (overrideWidth > 0 ? overrideWidth : Width + offset) * Perpendicular + offset * Direction);
                    Points.Add(
                        End + (overrideWidth > 0 ? overrideWidth : Width + offset) * Perpendicular + offset * Direction);
                }
            }

            public class Ring : Polygon
            {
                public Vector2 Center;
                public float InnerRadius;
                public float OuterRadius;
                private readonly int _quality;

                public Ring(Vector3 center, float innerRadius, float outerRadius, int quality = 20)
                    : this(center.To2D(), innerRadius, outerRadius, quality) {}

                public Ring(Vector2 center, float innerRadius, float outerRadius, int quality = 20)
                {
                    Center = center;
                    InnerRadius = innerRadius;
                    OuterRadius = outerRadius;
                    _quality = quality;
                    UpdatePolygon();
                }

                public void UpdatePolygon(int offset = 0)
                {
                    Points.Clear();
                    var outRadius = (offset + InnerRadius + OuterRadius) / (float)Math.Cos(2 * Math.PI / _quality);
                    var innerRadius = InnerRadius - OuterRadius - offset;
                    for (var i = 0; i <= _quality; i++)
                    {
                        var angle = i * 2 * Math.PI / _quality;
                        var point = new Vector2(
                            Center.X - outRadius * (float)Math.Cos(angle), Center.Y - outRadius * (float)Math.Sin(angle));
                        Points.Add(point);
                    }
                    for (var i = 0; i <= _quality; i++)
                    {
                        var angle = i * 2 * Math.PI / _quality;
                        var point = new Vector2(
                            Center.X + innerRadius * (float)Math.Cos(angle),
                            Center.Y - innerRadius * (float)Math.Sin(angle));
                        Points.Add(point);
                    }
                }
            }

            public class Sector : Polygon
            {
                public float Angle;
                public Vector2 Center;
                public Vector2 Direction;
                public float Radius;
                private readonly int _quality;

                public Sector(Vector3 center, Vector3 direction, float angle, float radius, int quality = 20)
                    : this(center.To2D(), direction.To2D(), angle, radius, quality) {}

                public Sector(Vector2 center, Vector2 direction, float angle, float radius, int quality = 20)
                {
                    Center = center;
                    Direction = (direction - center).Normalized();
                    Angle = angle;
                    Radius = radius;
                    _quality = quality;
                }

                public void UpdatePolygon(int offset = 0)
                {
                    Points.Clear();
                    var outRadius = (Radius + offset) / (float)Math.Cos(2 * Math.PI / _quality);
                    Points.Add(Center);
                    var side1 = Direction.Rotated(-Angle * 0.5f);
                    for (var i = 0; i <= _quality; i++)
                    {
                        var cDirection = side1.Rotated(i * Angle / _quality).Normalized();
                        Points.Add(new Vector2(Center.X + outRadius * cDirection.X, Center.Y + outRadius * cDirection.Y));
                    }
                }
            }
        }
    }
}
