namespace LeagueSharp.Common
{
    using System.Collections.Generic;
    using System.Linq;

    using ClipperLib;

    using SharpDX;

    using static System.Math;

    public static partial class Geometry
    {
        #region Public Methods and Operators

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

        public static Vector2[] CircleCircleIntersection(Vector2 center1, Vector2 center2, float radius1, float radius2)
        {
            var d = center1.Distance(center2);

            // The Circles don't intersect:
            if (d > radius1 + radius2 || (d <= Abs(radius1 - radius2)))
            {
                return new Vector2[] { };
            }

            var a = ((radius1 * radius1) - (radius2 * radius2) + (d * d)) / (2 * d);
            var h = (float)Sqrt((radius1 * radius1) - (a * a));
            var direction = (center2 - center1).Normalized();
            var pa = center1 + (a * direction);
            var s1 = pa + (h * direction.Perpendicular());
            var s2 = pa - (h * direction.Perpendicular());
            return new[] { s1, s2 };
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

        public static bool Close(float a, float b, float eps)
        {
            if (Abs(eps) < float.Epsilon)
            {
                eps = (float)1e-9;
            }

            return Abs(a - b) <= eps;
        }

        public static Vector2 Closest(this Vector2 v, List<Vector2> vList)
        {
            var result = default(Vector2);
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

        public static float CrossProduct(this Vector2 self, Vector2 other) => (other.Y * self.X) - (other.X * self.Y);

        public static float DegreeToRadian(double angle) => (float)(PI * angle / 180.0);

        /// <summary>
        ///     Calculates the distance between the player and the unit.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <param name="squared">
        ///     A value indicating whether to square the distance.
        /// </param>
        /// <returns>
        ///     The <see cref="float" />.
        /// </returns>
        public static float Distance(this GameObject unit, bool squared = false)
            => ObjectManager.Player.Distance(unit, squared);

        public static float Distance<T, T1>(this T unit, T1 unit2, bool squared = false) where T : GameObject
                                                                                             where T1 : GameObject
        {
            return unit?.Position.To2D().Distance(unit2?.Position.To2D() ?? default(Vector2), squared) ?? 0f;
        }

        public static float Distance(this Vector3 v, Vector3 other, bool squared = false)
            => squared ? Vector3.DistanceSquared(v, other) : Vector3.Distance(v, other);

        public static float Distance(this Vector2 v, Vector2 other, bool squared = false)
            => squared ? Vector2.DistanceSquared(v, other) : Vector2.Distance(v, other);

        public static float Distance(this Vector3 v, Vector2 other, bool squared = false)
            => squared ? Vector2.DistanceSquared(v.To2D(), other) : Vector2.Distance(v.To2D(), other);

        public static float Distance(this Vector2 v, Vector3 other, bool squared = false)
            => squared ? Vector2.DistanceSquared(v, other.To2D()) : Vector2.Distance(v, other.To2D());

        public static float Distance<T>(this Vector2 v, T to, bool squared = false) where T : GameObject
        => v.Distance(to.Position.To2D(), squared);

        public static float Distance<T>(this Vector3 v, T to, bool squared = false) where T : GameObject
        => v.Distance(to.Position.To2D(), squared);

        public static float Distance<T>(this T from, Vector2 to, bool squared = false) where T : GameObject
        => to.Distance(from.Position.To2D(), squared);

        public static float Distance<T>(this T from, Vector3 to, bool squared = false) where T : GameObject
        => to.Distance(from.Position.To2D(), squared);

        public static float Distance(
            this Vector2 point,
            Vector2 segmentStart,
            Vector2 segmentEnd,
            bool onlyIfOnSegment = false,
            bool squared = false)
        {
            var objects = point.ProjectOn(segmentStart, segmentEnd);
            return objects.IsOnSegment || onlyIfOnSegment == false
                       ? objects.SegmentPoint.Distance(point, squared)
                       : float.MaxValue;
        }

        public static float Distance3D<T, T1>(this T unit, T1 unit2, bool squared = false) where T : GameObject
                                                                                               where T1 : GameObject
        {
            return unit?.Position.Distance(unit2?.Position ?? default(Vector3), squared) ?? 0f;
        }

        public static Vector2 Extend(this Vector2 v, Vector2 to, float distance)
            => v + (distance * (to - v).Normalized());

        public static Vector3 Extend(this Vector3 v, Vector3 to, float distance)
            => v + (distance * (to - v).Normalized());

        public static Vector3 Extend(this Vector3 v, Vector2 to, float distance) => v.Extend(to.To3D(), distance);

        public static Vector3 Extend(this Vector2 v, Vector3 to, float distance) => v.To3D().Extend(to, distance);

        public static bool InRange<T, T1>(this T unit, T1 unit2, float range, bool squared = false) where T : GameObject
                                                                                                        where T1 :
                                                                                                        GameObject
        {
            return unit.Distance(unit2, squared) <= GetRange(range, squared);
        }

        public static bool InRange(this GameObject unit, float range, bool squared = false)
            => ObjectManager.Player.InRange(unit, range, squared);

        public static bool InRange(this Vector3 v, Vector3 other, float range, bool squared = false)
            => v.Distance(other, squared) <= GetRange(range, squared);

        public static bool InRange(this Vector2 v, Vector2 other, float range, bool squared = false)
            => v.Distance(other, squared) <= GetRange(range, squared);

        public static bool InRange(this Vector3 v, Vector2 other, float range, bool squared = false)
            => v.Distance(other, squared) <= GetRange(range, squared);

        public static bool InRange(this Vector2 v, Vector3 other, float range, bool squared = false)
            => v.Distance(other, squared) <= GetRange(range, squared);

        public static bool InRange<T>(this Vector2 v, T to, float range, bool squared = false) where T : GameObject
        => v.InRange(to.Position.To2D(), range, squared);

        public static bool InRange<T>(this Vector3 v, T to, float range, bool squared = false) where T : GameObject
        => v.InRange(to.Position.To2D(), range, squared);

        public static bool InRange<T>(this T from, Vector2 to, float range, bool squared = false) where T : GameObject
        => to.InRange(from.Position.To2D(), range, squared);

        public static bool InRange<T>(this T from, Vector3 to, float range, bool squared = false) where T : GameObject
        => to.InRange(from.Position.To2D(), range, squared);

        public static IntersectionResult Intersection(
            this Vector2 lineSegment1Start,
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

            var denominator = (deltaBAx * deltaDCy) - (deltaBAy * deltaDCx);
            var numerator = (deltaACy * deltaDCx) - (deltaACx * deltaDCy);

            if (Abs(denominator) < float.Epsilon)
            {
                if (Abs(numerator) < float.Epsilon)
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

                    return default(IntersectionResult);
                }

                // parallel
                return default(IntersectionResult);
            }

            var r = numerator / denominator;
            if (r < 0 || r > 1)
            {
                return default(IntersectionResult);
            }

            var s = ((deltaACy * deltaBAx) - (deltaACx * deltaBAy)) / denominator;
            if (s < 0 || s > 1)
            {
                return default(IntersectionResult);
            }

            var vector2 = new Vector2(
                              (float)(lineSegment1Start.X + (r * deltaBAx)),
                              (float)(lineSegment1Start.Y + (r * deltaBAy)));
            return new IntersectionResult(true, vector2);
        }

        public static bool IsValid(this Vector2 v) => !v.IsZero;

        public static bool IsValid(this Vector3 v) => !v.IsZero;

        public static List<Polygon> JoinPolygons(this List<Polygon> sList)
        {
            var p = ClipPolygons(sList);
            var tList = new List<List<IntPoint>>();

            var c = new Clipper();
            c.AddPaths(p, PolyType.ptClip, true);
            c.Execute(ClipType.ctUnion, tList, PolyFillType.pftNonZero, PolyFillType.pftNonZero);

            return ToPolygons(tList);
        }

        public static List<Polygon> JoinPolygons(
            this List<Polygon> sList,
            ClipType cType,
            PolyType pType = PolyType.ptClip,
            PolyFillType pFType1 = PolyFillType.pftNonZero,
            PolyFillType pFType2 = PolyFillType.pftNonZero)
        {
            var p = ClipPolygons(sList);
            var tList = new List<List<IntPoint>>();

            var c = new Clipper();
            c.AddPaths(p, pType, true);
            c.Execute(cType, tList, pFType1, pFType2);

            return ToPolygons(tList);
        }

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
                        moveTo.X + (polygonePoint.X - startPoint.X),
                        moveTo.Y + (polygonePoint.Y - startPoint.Y)));
            }

            return p;
        }

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

        public static float PathLength(this List<Vector2> path)
        {
            var distance = 0f;
            for (var i = 0; i < path.Count - 1; ++i)
            {
                distance += path[i].Distance(path[i + 1]);
            }

            return distance;
        }

        public static Vector2 Perpendicular(this Vector2 v) => new Vector2(-v.Y, v.X);

        public static Vector2 Perpendicular2(this Vector2 v) => new Vector2(v.Y, -v.X);

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

            var theta = RadianToDegree(Atan(v1.Y / v1.X));
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

        public static Vector2 PositionAfter(this List<Vector2> self, int t, int s, int delay = 0)
        {
            var distance = Max(0, t - delay) * s / 1000;
            for (var i = 0; i <= self.Count - 2; i++)
            {
                var from = self[i];
                var to = self[i + 1];
                var d = (int)to.Distance(from);
                if (d > distance)
                {
                    return from + (distance * (to - from).Normalized());
                }

                distance -= d;
            }

            return self[self.Count - 1];
        }

        public static ProjectionInfo ProjectOn(this Vector2 point, Vector2 segmentStart, Vector2 segmentEnd)
        {
            var cX = point.X;
            var cY = point.Y;
            var aX = segmentStart.X;
            var aY = segmentStart.Y;
            var bX = segmentEnd.X;
            var bY = segmentEnd.Y;

            var rL = (((cX - aX) * (bX - aX)) + ((cY - aY) * (bY - aY)))
                     / ((float)Pow(bX - aX, 2) + (float)Pow(bY - aY, 2));
            var pointLine = new Vector2(aX + (rL * (bX - aX)), aY + (rL * (bY - aY)));
            var rS = rL < 0 ? 0 : rL > 1 ? 1 : rL;
            var isOnSegment = rS.CompareTo(rL) == 0;
            var pointSegment = isOnSegment ? pointLine : new Vector2(aX + (rS * (bX - aX)), aY + (rS * (bY - aY)));

            return new ProjectionInfo(isOnSegment, pointSegment, pointLine);
        }

        public static float RadianToDegree(double angle) => (float)(angle * (180.0 / PI));

        public static Vector2 RotateAroundPoint(this Vector2 rotated, Vector2 around, float angle)
        {
            var sin = Sin(angle);
            var cos = Cos(angle);

            var x = (cos * (rotated.X - around.X)) - (sin * (rotated.Y - around.Y)) + around.X;
            var y = (sin * (rotated.X - around.X)) + (cos * (rotated.Y - around.Y)) + around.Y;

            return new Vector2((float)x, (float)y);
        }

        public static Vector2 Rotated(this Vector2 v, float angle)
        {
            var c = Cos(angle);
            var s = Sin(angle);

            return new Vector2((float)((v.X * c) - (v.Y * s)), (float)((v.Y * c) + (v.X * s)));
        }

        public static Polygon RotatePolygon(this Polygon polygon, Vector2 around, float angle)
        {
            var p = new Polygon();

            foreach (var polygonePoint in polygon.Points.Select(poinit => poinit.RotateAroundPoint(around, angle)))
            {
                p.Add(polygonePoint);
            }

            return p;
        }

        public static Polygon RotatePolygon(this Polygon polygon, Vector2 around, Vector2 direction)
        {
            var deltaX = around.X - direction.X;
            var deltaY = around.Y - direction.Y;
            var angle = (float)Atan2(deltaY, deltaX);
            return RotatePolygon(polygon, around, angle - DegreeToRadian(90));
        }

        public static float ServerDistance<T, T1>(this T unit, T1 unit2, bool squared = false) where T : Obj_AI_Base
                                                                                                   where T1 :
                                                                                                   Obj_AI_Base
        {
            return unit?.ServerPosition.Distance(unit2?.ServerPosition ?? default(Vector3), squared) ?? 0f;
        }

        public static Vector3 SetZ(this Vector3 v, float? value = null)
        {
            v.Z = value ?? Game.CursorPos.Z;
            return v;
        }

        public static Vector2 Shorten(this Vector2 v, Vector2 to, float distance)
            => v - (distance * (to - v).Normalized());

        public static Vector3 Shorten(this Vector3 v, Vector3 to, float distance)
            => v - (distance * (to - v).Normalized());

        public static Vector3 Shorten(this Vector3 v, Vector2 to, float distance) => v.Shorten(to.To3D(), distance);

        public static Vector3 Shorten(this Vector2 v, Vector3 to, float distance) => v.To3D().Shorten(to, distance);

        public static Vector3 SwitchYZ(this Vector3 v) => new Vector3(v.X, v.Z, v.Y);

        public static Vector2 To2D(this Vector3 v) => new Vector2(v.X, v.Y);

        public static List<Vector2> To2D(this List<Vector3> path) => path.Select(p => p.To2D()).ToList();

        public static Vector3 To3D(this Vector2 v) => new Vector3(v.X, v.Y, ObjectManager.Player.ServerPosition.Z);

        public static Vector3 To3D(this Vector2 v, float z) => new Vector3(v.X, v.Y, z);

        public static Vector3 To3D2(this Vector2 v) => new Vector3(v.X, v.Y, NavMesh.GetHeightForPosition(v.X, v.Y));

        public static Polygon ToPolygon(this List<IntPoint> v)
        {
            var polygon = new Polygon();
            foreach (var point in v)
            {
                polygon.Add(new Vector2(point.X, point.Y));
            }

            return polygon;
        }

        public static List<Polygon> ToPolygons(this List<List<IntPoint>> v)
        {
            return v.Select(path => path.ToPolygon()).ToList();
        }

        public static object[] VectorMovementCollision(
            Vector2 startPoint1,
            Vector2 endPoint1,
            float v1,
            Vector2 startPoint2,
            float v2,
            float delay = 0f)
        {
            float sP1X = startPoint1.X,
                  sP1Y = startPoint1.Y,
                  eP1X = endPoint1.X,
                  eP1Y = endPoint1.Y,
                  sP2X = startPoint2.X,
                  sP2Y = startPoint2.Y;

            float d = eP1X - sP1X, e = eP1Y - sP1Y;
            float dist = (float)Sqrt((d * d) + (e * e)), t1 = float.NaN;
            float s = Abs(dist) > float.Epsilon ? v1 * d / dist : 0,
                  k = (Abs(dist) > float.Epsilon) ? v1 * e / dist : 0f;

            float r = sP2X - sP1X, j = sP2Y - sP1Y;
            var c = (r * r) + (j * j);

            if (dist > 0f)
            {
                if (Abs(v1 - float.MaxValue) < float.Epsilon)
                {
                    var t = dist / v1;
                    t1 = v2 * t >= 0f ? t : float.NaN;
                }
                else if (Abs(v2 - float.MaxValue) < float.Epsilon)
                {
                    t1 = 0f;
                }
                else
                {
                    float a = (s * s) + (k * k) - (v2 * v2), b = (-r * s) - (j * k);

                    if (Abs(a) < float.Epsilon)
                    {
                        if (Abs(b) < float.Epsilon)
                        {
                            t1 = (Abs(c) < float.Epsilon) ? 0f : float.NaN;
                        }
                        else
                        {
                            var t = -c / (2 * b);
                            t1 = (v2 * t >= 0f) ? t : float.NaN;
                        }
                    }
                    else
                    {
                        var sqr = (b * b) - (a * c);
                        if (sqr >= 0)
                        {
                            var nom = (float)Sqrt(sqr);
                            var t = (-nom - b) / a;
                            t1 = v2 * t >= 0f ? t : float.NaN;
                            t = (nom - b) / a;
                            var t2 = (v2 * t >= 0f) ? t : float.NaN;

                            if (!float.IsNaN(t2) && !float.IsNaN(t1))
                            {
                                if (t1 >= delay && t2 >= delay)
                                {
                                    t1 = Min(t1, t2);
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
            else if (Abs(dist) < float.Epsilon)
            {
                t1 = 0f;
            }

            return new object[]
                           { t1, (!float.IsNaN(t1)) ? new Vector2(sP1X + (s * t1), sP1Y + (k * t1)) : default(Vector2) };
        }

        #endregion

        #region Methods

        private static float GetRange(float range, bool squared) => squared ? range * range : range;

        #endregion
    }
}