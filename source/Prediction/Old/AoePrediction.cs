namespace LeagueSharp.Common
{
    using System.Collections.Generic;
    using System.Linq;

    using SharpDX;

    /// <summary>
    ///     Calculates area of effect prediction.
    /// </summary>
    internal static class AoePrediction
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the prediction.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>PredictionOutput.</returns>
        public static PredictionOutput GetPrediction(PredictionInput input)
        {
            switch (input.Type)
            {
                case SkillshotType.SkillshotCircle:
                    return Circle.GetPrediction(input);
                case SkillshotType.SkillshotCone:
                    return Cone.GetPrediction(input);
                case SkillshotType.SkillshotLine:
                    return Line.GetPrediction(input);
            }
            return new PredictionOutput();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the possible targets.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>List&lt;PossibleTarget&gt;.</returns>
        internal static List<PossibleTarget> GetPossibleTargets(PredictionInput input)
        {
            var result = new List<PossibleTarget>();
            var originalUnit = input.Unit;
            foreach (var enemy in
                HeroManager.Enemies.FindAll(
                    h =>
                        h.NetworkId != originalUnit.NetworkId
                        && h.IsValidTarget((input.Range + 200 + input.RealRadius), true, input.RangeCheckFrom)))
            {
                input.Unit = enemy;
                var prediction = Prediction.GetPrediction(input, false, false);
                if (prediction.Hitchance >= HitChance.High)
                {
                    result.Add(new PossibleTarget { Position = prediction.UnitPosition.To2D(), Unit = enemy });
                }
            }
            return result;
        }

        #endregion

        /// <summary>
        ///     Represents a circular skillshot.
        /// </summary>
        public static class Circle
        {
            #region Public Methods and Operators

            /// <summary>
            ///     Gets the prediction.
            /// </summary>
            /// <param name="input">The input.</param>
            /// <returns>PredictionOutput.</returns>
            public static PredictionOutput GetPrediction(PredictionInput input)
            {
                var mainTargetPrediction = Prediction.GetPrediction(input, false, true);
                var posibleTargets = new List<PossibleTarget>
                                         {
                                             new PossibleTarget
                                                 {
                                                     Position = mainTargetPrediction.UnitPosition.To2D(),
                                                     Unit = input.Unit
                                                 }
                                         };

                if (mainTargetPrediction.Hitchance >= HitChance.Medium)
                {
                    //Add the posible targets  in range:
                    posibleTargets.AddRange(GetPossibleTargets(input));
                }

                while (posibleTargets.Count > 1)
                {
                    var mecCircle = MEC.GetMec(posibleTargets.Select(h => h.Position).ToList());

                    if (mecCircle.Radius <= input.RealRadius - 10
                        && Vector2.DistanceSquared(mecCircle.Center, input.RangeCheckFrom.To2D())
                        < input.Range * input.Range)
                    {
                        return new PredictionOutput
                                   {
                                       AoeTargetsHit = posibleTargets.Select(h => (Obj_AI_Hero)h.Unit).ToList(),
                                       CastPosition = mecCircle.Center.To3D(),
                                       UnitPosition = mainTargetPrediction.UnitPosition,
                                       Hitchance = mainTargetPrediction.Hitchance, Input = input,
                                       AoeTargetsHitCountBackingField = posibleTargets.Count
                                   };
                    }

                    float maxdist = -1;
                    var maxdistindex = 1;
                    for (var i = 1; i < posibleTargets.Count; i++)
                    {
                        var distance = Vector2.DistanceSquared(posibleTargets[i].Position, posibleTargets[0].Position);
                        if (distance > maxdist || maxdist.CompareTo(-1) == 0)
                        {
                            maxdistindex = i;
                            maxdist = distance;
                        }
                    }
                    posibleTargets.RemoveAt(maxdistindex);
                }

                return mainTargetPrediction;
            }

            #endregion
        }

        /// <summary>
        ///     Represents a conical skillshot.
        /// </summary>
        public static class Cone
        {
            #region Public Methods and Operators

            /// <summary>
            ///     Gets the prediction.
            /// </summary>
            /// <param name="input">The input.</param>
            /// <returns>PredictionOutput.</returns>
            public static PredictionOutput GetPrediction(PredictionInput input)
            {
                var mainTargetPrediction = Prediction.GetPrediction(input, false, true);
                var posibleTargets = new List<PossibleTarget>
                                         {
                                             new PossibleTarget
                                                 {
                                                     Position = mainTargetPrediction.UnitPosition.To2D(),
                                                     Unit = input.Unit
                                                 }
                                         };

                if (mainTargetPrediction.Hitchance >= HitChance.Medium)
                {
                    //Add the posible targets  in range:
                    posibleTargets.AddRange(GetPossibleTargets(input));
                }

                if (posibleTargets.Count > 1)
                {
                    var candidates = new List<Vector2>();

                    foreach (var target in posibleTargets)
                    {
                        target.Position = target.Position - input.From.To2D();
                    }

                    for (var i = 0; i < posibleTargets.Count; i++)
                    {
                        for (var j = 0; j < posibleTargets.Count; j++)
                        {
                            if (i != j)
                            {
                                var p = (posibleTargets[i].Position + posibleTargets[j].Position) * 0.5f;
                                if (!candidates.Contains(p))
                                {
                                    candidates.Add(p);
                                }
                            }
                        }
                    }

                    var bestCandidateHits = -1;
                    var bestCandidate = new Vector2();
                    var positionsList = posibleTargets.Select(t => t.Position).ToList();

                    foreach (var candidate in candidates)
                    {
                        var hits = GetHits(candidate, input.Range, input.Radius, positionsList);
                        if (hits > bestCandidateHits)
                        {
                            bestCandidate = candidate;
                            bestCandidateHits = hits;
                        }
                    }

                    if (bestCandidateHits > 1 && input.From.To2D().Distance(bestCandidate, true) > 50 * 50)
                    {
                        return new PredictionOutput
                                   {
                                       Hitchance = mainTargetPrediction.Hitchance,
                                       AoeTargetsHitCountBackingField = bestCandidateHits,
                                       UnitPosition = mainTargetPrediction.UnitPosition,
                                       CastPosition = bestCandidate.To3D(), Input = input
                                   };
                    }
                }
                return mainTargetPrediction;
            }

            #endregion

            #region Methods

            /// <summary>
            ///     Gets the hits.
            /// </summary>
            /// <param name="end">The end.</param>
            /// <param name="range">The range.</param>
            /// <param name="angle">The angle.</param>
            /// <param name="points">The points.</param>
            /// <returns>System.Int32.</returns>
            internal static int GetHits(Vector2 end, double range, float angle, List<Vector2> points)
            {
                return (from point in points
                        let edge1 = end.Rotated(-angle / 2)
                        let edge2 = edge1.Rotated(angle)
                        where
                        point.Distance(new Vector2(), true) < range * range && edge1.CrossProduct(point) > 0
                        && point.CrossProduct(edge2) > 0
                        select point).Count();
            }

            #endregion
        }

        /// <summary>
        ///     Represents a linear skillshot.
        /// </summary>
        public static class Line
        {
            #region Public Methods and Operators

            /// <summary>
            ///     Gets the prediction.
            /// </summary>
            /// <param name="input">The input.</param>
            /// <returns>PredictionOutput.</returns>
            public static PredictionOutput GetPrediction(PredictionInput input)
            {
                var mainTargetPrediction = Prediction.GetPrediction(input, false, true);
                var posibleTargets = new List<PossibleTarget>
                                         {
                                             new PossibleTarget
                                                 {
                                                     Position = mainTargetPrediction.UnitPosition.To2D(),
                                                     Unit = input.Unit
                                                 }
                                         };
                if (mainTargetPrediction.Hitchance >= HitChance.Medium)
                {
                    //Add the posible targets  in range:
                    posibleTargets.AddRange(GetPossibleTargets(input));
                }

                if (posibleTargets.Count > 1)
                {
                    var candidates = new List<Vector2>();
                    foreach (var target in posibleTargets)
                    {
                        var targetCandidates = GetCandidates(
                            input.From.To2D(),
                            target.Position,
                            (input.Radius),
                            input.Range);
                        candidates.AddRange(targetCandidates);
                    }

                    var bestCandidateHits = -1;
                    var bestCandidate = new Vector2();
                    var bestCandidateHitPoints = new List<Vector2>();
                    var positionsList = posibleTargets.Select(t => t.Position).ToList();

                    foreach (var candidate in candidates)
                    {
                        if (
                            GetHits(
                                input.From.To2D(),
                                candidate,
                                (input.Radius + input.Unit.BoundingRadius / 3 - 10),
                                new List<Vector2> { posibleTargets[0].Position }).Count() == 1)
                        {
                            var hits = GetHits(input.From.To2D(), candidate, input.Radius, positionsList).ToList();
                            var hitsCount = hits.Count;
                            if (hitsCount >= bestCandidateHits)
                            {
                                bestCandidateHits = hitsCount;
                                bestCandidate = candidate;
                                bestCandidateHitPoints = hits.ToList();
                            }
                        }
                    }

                    if (bestCandidateHits > 1)
                    {
                        float maxDistance = -1;
                        Vector2 p1 = new Vector2(), p2 = new Vector2();

                        //Center the position
                        for (var i = 0; i < bestCandidateHitPoints.Count; i++)
                        {
                            for (var j = 0; j < bestCandidateHitPoints.Count; j++)
                            {
                                var startP = input.From.To2D();
                                var endP = bestCandidate;
                                var proj1 = positionsList[i].ProjectOn(startP, endP);
                                var proj2 = positionsList[j].ProjectOn(startP, endP);
                                var dist = Vector2.DistanceSquared(bestCandidateHitPoints[i], proj1.LinePoint)
                                           + Vector2.DistanceSquared(bestCandidateHitPoints[j], proj2.LinePoint);
                                if (dist >= maxDistance
                                    && (proj1.LinePoint - positionsList[i]).AngleBetween(
                                        proj2.LinePoint - positionsList[j]) > 90)
                                {
                                    maxDistance = dist;
                                    p1 = positionsList[i];
                                    p2 = positionsList[j];
                                }
                            }
                        }

                        return new PredictionOutput
                                   {
                                       Hitchance = mainTargetPrediction.Hitchance,
                                       AoeTargetsHitCountBackingField = bestCandidateHits,
                                       UnitPosition = mainTargetPrediction.UnitPosition,
                                       CastPosition = ((p1 + p2) * 0.5f).To3D(), Input = input
                                   };
                    }
                }

                return mainTargetPrediction;
            }

            #endregion

            #region Methods

            /// <summary>
            ///     Gets the candidates.
            /// </summary>
            /// <param name="from">From.</param>
            /// <param name="to">To.</param>
            /// <param name="radius">The radius.</param>
            /// <param name="range">The range.</param>
            /// <returns>Vector2[].</returns>
            internal static Vector2[] GetCandidates(Vector2 from, Vector2 to, float radius, float range)
            {
                var middlePoint = (from + to) / 2;
                var intersections = Geometry.CircleCircleIntersection(
                    from,
                    middlePoint,
                    radius,
                    from.Distance(middlePoint));

                if (intersections.Length > 1)
                {
                    var c1 = intersections[0];
                    var c2 = intersections[1];

                    c1 = from + range * (to - c1).Normalized();
                    c2 = from + range * (to - c2).Normalized();

                    return new[] { c1, c2 };
                }

                return new Vector2[] { };
            }

            /// <summary>
            ///     Gets the hits.
            /// </summary>
            /// <param name="start">The start.</param>
            /// <param name="end">The end.</param>
            /// <param name="radius">The radius.</param>
            /// <param name="points">The points.</param>
            /// <returns>IEnumerable&lt;Vector2&gt;.</returns>
            internal static IEnumerable<Vector2> GetHits(
                Vector2 start,
                Vector2 end,
                double radius,
                List<Vector2> points)
            {
                return points.Where(p => p.Distance(start, end, true, true) <= radius * radius);
            }

            #endregion
        }

        /// <summary>
        ///     Represents a possible target.
        /// </summary>
        internal class PossibleTarget
        {
            #region Fields

            /// <summary>
            ///     The position
            /// </summary>
            public Vector2 Position;

            /// <summary>
            ///     The unit
            /// </summary>
            public Obj_AI_Base Unit;

            #endregion
        }
    }
}