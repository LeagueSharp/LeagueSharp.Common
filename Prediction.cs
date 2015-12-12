#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Prediction.cs is part of LeagueSharp.Common.
 
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
using System.Text.RegularExpressions;
using SharpDX;

#endregion

namespace LeagueSharp.Common
{

    /// <summary>
    /// Represents the chance of hitting an enemy.
    /// </summary>
    public enum HitChance
    {
        /// <summary>
        /// The target is immobile.
        /// </summary>
        Immobile = 8,

        /// <summary>
        /// The unit is dashing.
        /// </summary>
        Dashing = 7,

        /// <summary>
        /// Very high probability of hitting the target.
        /// </summary>
        VeryHigh = 6,

        /// <summary>
        /// High probability of hitting the target.
        /// </summary>
        High = 5,

        /// <summary>
        /// Medium probability of hitting the target.
        /// </summary>
        Medium = 4,

        /// <summary>
        /// Low probability of hitting the target.
        /// </summary>
        Low = 3,

        /// <summary>
        /// Impossible to hit the target.
        /// </summary>
        Impossible = 2,

        /// <summary>
        /// The target is out of range.
        /// </summary>
        OutOfRange = 1,

        /// <summary>
        /// The target is blocked by other units.
        /// </summary>
        Collision = 0
    }

    /// <summary>
    /// The type of skillshot.
    /// </summary>
    public enum SkillshotType
    {
        /// <summary>
        /// The skillshot is linear.
        /// </summary>
        SkillshotLine,

        /// <summary>
        /// The skillshot is circular.
        /// </summary>
        SkillshotCircle,

        /// <summary>
        /// The skillshot is conical.
        /// </summary>
        SkillshotCone
    }

    /// <summary>
    /// Objects that cause collision to the spell.
    /// </summary>
    public enum CollisionableObjects
    {
        /// <summary>
        /// Minions.
        /// </summary>
        Minions,

        /// <summary>
        /// Enemy heroes.
        /// </summary>
        Heroes,

        /// <summary>
        /// Yasuo's Wind Wall (W)
        /// </summary>
        YasuoWall,

        /// <summary>
        /// Walls.
        /// </summary>
        Walls,

        /// <summary>
        /// Ally heroes.
        /// </summary>
        Allies
    }

    /// <summary>
    /// Contains information necessary to calculate the prediction.
    /// </summary>
    public class PredictionInput
    {
        /// <summary>
        /// The position that the skillshot will be launched from.
        /// </summary>
        private Vector3 _from;

        /// <summary>
        /// The position to check the range from.
        /// </summary>
        private Vector3 _rangeCheckFrom;

        /// <summary>
        /// If set to <c>true</c> the prediction will hit as many enemy heroes as posible.
        /// </summary>
        public bool Aoe = false;

        /// <summary>
        /// <c>true</c> if the spell collides with units.
        /// </summary>
        public bool Collision = false;

        /// <summary>
        /// Array that contains the unit types that the skillshot can collide with.
        /// </summary>
        public CollisionableObjects[] CollisionObjects =
        {
            CollisionableObjects.Minions, CollisionableObjects.YasuoWall
        };

        /// <summary>
        /// The skillshot delay in seconds.
        /// </summary>
        public float Delay;

        /// <summary>
        /// The skillshot width's radius or the angle in case of the cone skillshots.
        /// </summary>
        public float Radius = 1f;

        /// <summary>
        /// The skillshot range in units.
        /// </summary>
        public float Range = float.MaxValue;

        /// <summary>
        /// The skillshot speed in units per second.
        /// </summary>
        public float Speed = float.MaxValue;

        /// <summary>
        /// The skillshot type.
        /// </summary>
        public SkillshotType Type = SkillshotType.SkillshotLine;

        /// <summary>
        /// The unit that the prediction will made for.
        /// </summary>
        public Obj_AI_Base Unit = ObjectManager.Player;

        /// <summary>
        /// Set to true to increase the prediction radius by the unit bounding radius.
        /// </summary>
        public bool UseBoundingRadius = true;

        /// <summary>
        /// The position from where the skillshot missile gets fired.
        /// </summary>
        /// <value>From.</value>
        public Vector3 From
        {
            get { return _from.To2D().IsValid() ? _from : ObjectManager.Player.ServerPosition; }
            set { _from = value; }
        }

        /// <summary>
        /// The position from where the range is checked.
        /// </summary>
        /// <value>The range check from.</value>
        public Vector3 RangeCheckFrom
        {
            get
            {
                return _rangeCheckFrom.To2D().IsValid()
                    ? _rangeCheckFrom
                    : (From.To2D().IsValid() ? From : ObjectManager.Player.ServerPosition);
            }
            set { _rangeCheckFrom = value; }
        }

        /// <summary>
        /// Gets the real radius.
        /// </summary>
        /// <value>The real radius.</value>
        internal float RealRadius
        {
            get { return UseBoundingRadius ? Radius + Unit.BoundingRadius : Radius; }
        }
    }

    /// <summary>
    /// The output after calculating the prediction.
    /// </summary>
    public class PredictionOutput
    {
        /// <summary>
        /// The AoE target hit.
        /// </summary>
        internal int _aoeTargetsHitCount;

        /// <summary>
        /// The calculated cast position
        /// </summary>
        private Vector3 _castPosition;

        /// <summary>
        /// The predicted unit position
        /// </summary>
        private Vector3 _unitPosition;

        /// <summary>
        /// The list of the targets that the spell will hit (only if aoe was enabled).
        /// </summary>
        public List<Obj_AI_Hero> AoeTargetsHit = new List<Obj_AI_Hero>();

        /// <summary>
        /// The list of the units that the skillshot will collide with.
        /// </summary>
        public List<Obj_AI_Base> CollisionObjects = new List<Obj_AI_Base>();

        /// <summary>
        /// Returns the hitchance.
        /// </summary>
        public HitChance Hitchance = HitChance.Impossible;

        /// <summary>
        /// The input
        /// </summary>
        internal PredictionInput Input;

        /// <summary>
        /// The position where the skillshot should be casted to increase the accuracy.
        /// </summary>
        /// <value>The cast position.</value>
        public Vector3 CastPosition
        {
            get
            {
                return _castPosition.IsValid() && _castPosition.To2D().IsValid()
                    ? _castPosition.SetZ()
                    : Input.Unit.ServerPosition;
            }
            set { _castPosition = value; }
        }

        /// <summary>
        /// The number of targets the skillshot will hit (only if aoe was enabled).
        /// </summary>
        /// <value>The aoe targets hit count.</value>
        public int AoeTargetsHitCount
        {
            get { return Math.Max(_aoeTargetsHitCount, AoeTargetsHit.Count); }
        }

        /// <summary>
        /// The position where the unit is going to be when the skillshot reaches his position.
        /// </summary>
        /// <value>The unit position.</value>
        public Vector3 UnitPosition
        {
            get { return _unitPosition.To2D().IsValid() ? _unitPosition.SetZ() : Input.Unit.ServerPosition; }
            set { _unitPosition = value; }
        }
    }

    /// <summary>
    /// Class used for calculating the position of the given unit after a delay.
    /// </summary>
    public static class Prediction
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            CustomEvents.Game.OnGameLoad += eventArgs =>
            {
                var menu = new Menu("Prediction", "Prediction");
                var slider = new MenuItem("PredMaxRange", "Max Range %").SetValue(new Slider(100, 70, 100));
                menu.AddItem(slider);
                CommonMenu.Instance.AddSubMenu(menu);
            };
        }

        /// <summary>
        /// Gets the prediction.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="delay">The delay.</param>
        /// <returns>PredictionOutput.</returns>
        public static PredictionOutput GetPrediction(Obj_AI_Base unit, float delay)
        {
            return GetPrediction(new PredictionInput { Unit = unit, Delay = delay });
        }

        /// <summary>
        /// Gets the prediction.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="radius">The radius.</param>
        /// <returns>PredictionOutput.</returns>
        public static PredictionOutput GetPrediction(Obj_AI_Base unit, float delay, float radius)
        {
            return GetPrediction(new PredictionInput { Unit = unit, Delay = delay, Radius = radius });
        }

        /// <summary>
        /// Gets the prediction.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="speed">The speed.</param>
        /// <returns>PredictionOutput.</returns>
        public static PredictionOutput GetPrediction(Obj_AI_Base unit, float delay, float radius, float speed)
        {
            return GetPrediction(new PredictionInput { Unit = unit, Delay = delay, Radius = radius, Speed = speed });
        }

        /// <summary>
        /// Gets the prediction.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="collisionable">The collisionable objects.</param>
        /// <returns>PredictionOutput.</returns>
        public static PredictionOutput GetPrediction(Obj_AI_Base unit,
            float delay,
            float radius,
            float speed,
            CollisionableObjects[] collisionable)
        {
            return
                GetPrediction(
                    new PredictionInput
                    {
                        Unit = unit,
                        Delay = delay,
                        Radius = radius,
                        Speed = speed,
                        CollisionObjects = collisionable
                    });
        }

        /// <summary>
        /// Gets the prediction.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>PredictionOutput.</returns>
        public static PredictionOutput GetPrediction(PredictionInput input)
        {
            return GetPrediction(input, true, true);
        }

        /// <summary>
        /// Gets the prediction.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="ft">if set to <c>true</c>, will add extra delay to the spell..</param>
        /// <param name="checkCollision">if set to <c>true</c>, checks collision.</param>
        /// <returns>PredictionOutput.</returns>
        internal static PredictionOutput GetPrediction(PredictionInput input, bool ft, bool checkCollision)
        {
            PredictionOutput result = null;

            if (!input.Unit.IsValidTarget(float.MaxValue, false))
            {
                return new PredictionOutput();
            }

            if (ft)
            {
                //Increase the delay due to the latency and server tick:
                input.Delay += Game.Ping / 2000f + 0.06f;

                if (input.Aoe)
                {
                    return AoePrediction.GetPrediction(input);
                }
            }

            //Target too far away.
            if (Math.Abs(input.Range - float.MaxValue) > float.Epsilon &&
                input.Unit.Distance(input.RangeCheckFrom, true) > Math.Pow(input.Range * 1.5, 2))
            {
                return new PredictionOutput { Input = input };
            }

            //Unit is dashing.
            if (input.Unit.IsDashing())
            {
                result = GetDashingPrediction(input);
            }
            else
            {
                //Unit is immobile.
                var remainingImmobileT = UnitIsImmobileUntil(input.Unit);
                if (remainingImmobileT >= 0d)
                {
                    result = GetImmobilePrediction(input, remainingImmobileT);
                }
                else
                {
                    input.Range = input.Range * CommonMenu.Instance.Item("PredMaxRange").GetValue<Slider>().Value / 100f;
                }
            }

            //Normal prediction
            if (result == null)
            {
                result = GetStandardPrediction(input);
            }

            //Check if the unit position is in range
            if (Math.Abs(input.Range - float.MaxValue) > float.Epsilon)
            {
                if (result.Hitchance >= HitChance.High &&
                    input.RangeCheckFrom.Distance(input.Unit.Position, true) >
                    Math.Pow(input.Range + input.RealRadius * 3 / 4, 2))
                {
                    result.Hitchance = HitChance.Medium;
                }

                if (input.RangeCheckFrom.Distance(result.UnitPosition, true) >
                    Math.Pow(input.Range + (input.Type == SkillshotType.SkillshotCircle ? input.RealRadius : 0), 2))
                {
                    result.Hitchance = HitChance.OutOfRange;
                }

                if (input.RangeCheckFrom.Distance(result.CastPosition, true) > Math.Pow(input.Range, 2))
                {
                    if (result.Hitchance != HitChance.OutOfRange)
                    {
                        result.CastPosition = input.RangeCheckFrom +
                                              input.Range *
                                              (result.UnitPosition - input.RangeCheckFrom).To2D().Normalized().To3D();
                    }
                    else
                    {
                        result.Hitchance = HitChance.OutOfRange;
                    }
                }
            }

            //Check for collision
            if (checkCollision && input.Collision)
            {
                var positions = new List<Vector3> { result.UnitPosition, result.CastPosition, input.Unit.Position };
                var originalUnit = input.Unit;
                result.CollisionObjects = Collision.GetCollision(positions, input);
                result.CollisionObjects.RemoveAll(x => x.NetworkId == originalUnit.NetworkId);
                result.Hitchance = result.CollisionObjects.Count > 0 ? HitChance.Collision : result.Hitchance;
            }

            return result;
        }

        /// <summary>
        /// Gets the dashing prediction.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>PredictionOutput.</returns>
        internal static PredictionOutput GetDashingPrediction(PredictionInput input)
        {
            var dashData = input.Unit.GetDashInfo();
            var result = new PredictionOutput { Input = input };

            //Normal dashes.
            if (!dashData.IsBlink)
            {
                //Mid air:
                var endP = dashData.Path.Last();
                var dashPred = GetPositionOnPath(
                    input, new List<Vector2> { input.Unit.ServerPosition.To2D(), endP }, dashData.Speed);
                if (dashPred.Hitchance >= HitChance.High && dashPred.UnitPosition.To2D().Distance(input.Unit.Position.To2D(), endP, true) < 200 )
                {
                    dashPred.CastPosition = dashPred.UnitPosition;
                    dashPred.Hitchance = HitChance.Dashing;
                    return dashPred;
                }
                
                //At the end of the dash:
                if (dashData.Path.PathLength() > 200)
                {
                    
                    var timeToPoint = input.Delay / 2f + input.From.To2D().Distance(endP) / input.Speed - 0.25f;
                    if (timeToPoint <=
                        input.Unit.Distance(endP) / dashData.Speed + input.RealRadius / input.Unit.MoveSpeed)
                    {
                        return new PredictionOutput
                        {
                            CastPosition = endP.To3D(),
                            UnitPosition = endP.To3D(),
                            Hitchance = HitChance.Dashing
                        };
                    }
                }

                result.CastPosition = dashData.Path.Last().To3D();
                result.UnitPosition = result.CastPosition;

                //Figure out where the unit is going.
            }

            return result;
        }

        /// <summary>
        /// Gets the immobile prediction.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="remainingImmobileT">The remaining immobile t.</param>
        /// <returns>PredictionOutput.</returns>
        internal static PredictionOutput GetImmobilePrediction(PredictionInput input, double remainingImmobileT)
        {
            var timeToReachTargetPosition = input.Delay + input.Unit.Distance(input.From) / input.Speed;

            if (timeToReachTargetPosition <= remainingImmobileT + input.RealRadius / input.Unit.MoveSpeed)
            {
                return new PredictionOutput
                {
                    CastPosition = input.Unit.ServerPosition,
                    UnitPosition = input.Unit.Position,
                    Hitchance = HitChance.Immobile
                };
            }

            return new PredictionOutput
            {
                Input = input,
                CastPosition = input.Unit.ServerPosition,
                UnitPosition = input.Unit.ServerPosition,
                Hitchance = HitChance.High
                /*timeToReachTargetPosition - remainingImmobileT + input.RealRadius / input.Unit.MoveSpeed < 0.4d ? HitChance.High : HitChance.Medium*/
            };
        }

        /// <summary>
        /// Gets the standard prediction.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>PredictionOutput.</returns>
        internal static PredictionOutput GetStandardPrediction(PredictionInput input)
        {
            var speed = input.Unit.MoveSpeed;

            if (input.Unit.Distance(input.From, true) < 200 * 200)
            {
                //input.Delay /= 2;
                speed /= 1.5f;
            }

            var result = GetPositionOnPath(input, input.Unit.GetWaypoints(), speed);

            if (result.Hitchance >= HitChance.High && input.Unit is Obj_AI_Hero) { }

            return result;
        }

        /// <summary>
        /// Gets the time the unit is immobile untill.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns>System.Double.</returns>
        internal static double UnitIsImmobileUntil(Obj_AI_Base unit)
        {
            var result =
                unit.Buffs.Where(
                    buff =>
                        buff.IsActive && Game.Time <= buff.EndTime &&
                        (buff.Type == BuffType.Charm || buff.Type == BuffType.Knockup || buff.Type == BuffType.Stun ||
                         buff.Type == BuffType.Suppression || buff.Type == BuffType.Snare))
                    .Aggregate(0d, (current, buff) => Math.Max(current, buff.EndTime));
            return (result - Game.Time);
        }

        /// <summary>
        /// Gets the position on path.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="path">The path.</param>
        /// <param name="speed">The speed.</param>
        /// <returns>PredictionOutput.</returns>
        internal static PredictionOutput GetPositionOnPath(PredictionInput input, List<Vector2> path, float speed = -1)
        {
            speed = (Math.Abs(speed - (-1)) < float.Epsilon) ? input.Unit.MoveSpeed : speed;

            if (path.Count <= 1)
            {
                return new PredictionOutput
                {
                    Input = input,
                    UnitPosition = input.Unit.ServerPosition,
                    CastPosition = input.Unit.ServerPosition,
                    Hitchance = HitChance.VeryHigh
                };
            }

            var pLength = path.PathLength();

            //Skillshots with only a delay
            if (pLength >= input.Delay * speed - input.RealRadius && Math.Abs(input.Speed - float.MaxValue) < float.Epsilon)
            {
                var tDistance = input.Delay * speed - input.RealRadius;

                for (var i = 0; i < path.Count - 1; i++)
                {
                    var a = path[i];
                    var b = path[i + 1];
                    var d = a.Distance(b);

                    if (d >= tDistance)
                    {
                        var direction = (b - a).Normalized();

                        var cp = a + direction * tDistance;
                        var p = a +
                                direction *
                                ((i == path.Count - 2)
                                    ? Math.Min(tDistance + input.RealRadius, d)
                                    : (tDistance + input.RealRadius));

                        return new PredictionOutput
                        {
                            Input = input,
                            CastPosition = cp.To3D(),
                            UnitPosition = p.To3D(),
                            Hitchance =
                                PathTracker.GetCurrentPath(input.Unit).Time < 0.1d ? HitChance.VeryHigh : HitChance.High
                        };
                    }

                    tDistance -= d;
                }
            }

            //Skillshot with a delay and speed.
            if (pLength >= input.Delay * speed - input.RealRadius &&
                Math.Abs(input.Speed - float.MaxValue) > float.Epsilon)
            {
                var d = input.Delay * speed - input.RealRadius;
                if (input.Type == SkillshotType.SkillshotLine || input.Type == SkillshotType.SkillshotCone)
                {
                    if (input.From.Distance(input.Unit.ServerPosition, true) < 200 * 200)
                    {
                        d = input.Delay * speed;
                    }
                }

                path = path.CutPath(d);
                var tT = 0f;
                for (var i = 0; i < path.Count - 1; i++)
                {
                    var a = path[i];
                    var b = path[i + 1];
                    var tB = a.Distance(b) / speed;
                    var direction = (b - a).Normalized();
                    a = a - speed * tT * direction;
                    var sol = Geometry.VectorMovementCollision(a, b, speed, input.From.To2D(), input.Speed, tT);
                    var t = (float)sol[0];
                    var pos = (Vector2)sol[1];

                    if (pos.IsValid() && t >= tT && t <= tT + tB)
                    {
                        if (pos.Distance(b, true) < 20)
                            break;
                        var p = pos + input.RealRadius * direction;

                        if (input.Type == SkillshotType.SkillshotLine && false)
                        {
                            var alpha = (input.From.To2D() - p).AngleBetween(a - b);
                            if (alpha > 30 && alpha < 180 - 30)
                            {
                                var beta = (float)Math.Asin(input.RealRadius / p.Distance(input.From));
                                var cp1 = input.From.To2D() + (p - input.From.To2D()).Rotated(beta);
                                var cp2 = input.From.To2D() + (p - input.From.To2D()).Rotated(-beta);

                                pos = cp1.Distance(pos, true) < cp2.Distance(pos, true) ? cp1 : cp2;
                            }
                        }

                        return new PredictionOutput
                        {
                            Input = input,
                            CastPosition = pos.To3D(),
                            UnitPosition = p.To3D(),
                            Hitchance =
                                PathTracker.GetCurrentPath(input.Unit).Time < 0.1d ? HitChance.VeryHigh : HitChance.High
                        };
                    }
                    tT += tB;
                }
            }

            var position = path.Last();
            return new PredictionOutput
            {
                Input = input,
                CastPosition = position.To3D(),
                UnitPosition = position.To3D(),
                Hitchance = HitChance.Medium
            };
        }
    }

    /// <summary>
    /// Calculates area of effect prediction.
    /// </summary>
    internal static class AoePrediction
    {
        /// <summary>
        /// Gets the prediction.
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

        /// <summary>
        /// Gets the possible targets.
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
                        h.NetworkId != originalUnit.NetworkId &&
                        h.IsValidTarget((input.Range + 200 + input.RealRadius), true, input.RangeCheckFrom)))
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

        /// <summary>
        /// Represents a circular skillshot.
        /// </summary>
        public static class Circle
        {
            /// <summary>
            /// Gets the prediction.
            /// </summary>
            /// <param name="input">The input.</param>
            /// <returns>PredictionOutput.</returns>
            public static PredictionOutput GetPrediction(PredictionInput input)
            {
                var mainTargetPrediction = Prediction.GetPrediction(input, false, true);
                var posibleTargets = new List<PossibleTarget>
                {
                    new PossibleTarget { Position = mainTargetPrediction.UnitPosition.To2D(), Unit = input.Unit }
                };

                if (mainTargetPrediction.Hitchance >= HitChance.Medium)
                {
                    //Add the posible targets  in range:
                    posibleTargets.AddRange(GetPossibleTargets(input));
                }

                while (posibleTargets.Count > 1)
                {
                    var mecCircle = MEC.GetMec(posibleTargets.Select(h => h.Position).ToList());

                    if (mecCircle.Radius <= input.RealRadius - 10 &&
                        Vector2.DistanceSquared(mecCircle.Center, input.RangeCheckFrom.To2D()) <
                        input.Range * input.Range)
                    {
                        return new PredictionOutput
                        {
                            AoeTargetsHit = posibleTargets.Select(h => (Obj_AI_Hero)h.Unit).ToList(),
                            CastPosition = mecCircle.Center.To3D(),
                            UnitPosition = mainTargetPrediction.UnitPosition,
                            Hitchance = mainTargetPrediction.Hitchance,
                            Input = input,
                            _aoeTargetsHitCount = posibleTargets.Count
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
        }

        /// <summary>
        /// Represents a conical skillshot.
        /// </summary>
        public static class Cone
        {
            /// <summary>
            /// Gets the hits.
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
                            point.Distance(new Vector2(), true) < range * range && edge1.CrossProduct(point) > 0 &&
                            point.CrossProduct(edge2) > 0
                        select point).Count();
            }

            /// <summary>
            /// Gets the prediction.
            /// </summary>
            /// <param name="input">The input.</param>
            /// <returns>PredictionOutput.</returns>
            public static PredictionOutput GetPrediction(PredictionInput input)
            {
                var mainTargetPrediction = Prediction.GetPrediction(input, false, true);
                var posibleTargets = new List<PossibleTarget>
                {
                    new PossibleTarget { Position = mainTargetPrediction.UnitPosition.To2D(), Unit = input.Unit }
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
                            _aoeTargetsHitCount = bestCandidateHits,
                            UnitPosition = mainTargetPrediction.UnitPosition,
                            CastPosition = bestCandidate.To3D(),
                            Input = input
                        };
                    }
                }
                return mainTargetPrediction;
            }
        }

        /// <summary>
        /// Represents a linear skillshot.
        /// </summary>
        public static class Line
        {
            /// <summary>
            /// Gets the hits.
            /// </summary>
            /// <param name="start">The start.</param>
            /// <param name="end">The end.</param>
            /// <param name="radius">The radius.</param>
            /// <param name="points">The points.</param>
            /// <returns>IEnumerable&lt;Vector2&gt;.</returns>
            internal static IEnumerable<Vector2> GetHits(Vector2 start, Vector2 end, double radius, List<Vector2> points)
            {
                return points.Where(p => p.Distance(start, end, true, true) <= radius * radius);
            }

            /// <summary>
            /// Gets the candidates.
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
                    from, middlePoint, radius, from.Distance(middlePoint));

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
            /// Gets the prediction.
            /// </summary>
            /// <param name="input">The input.</param>
            /// <returns>PredictionOutput.</returns>
            public static PredictionOutput GetPrediction(PredictionInput input)
            {
                var mainTargetPrediction = Prediction.GetPrediction(input, false, true);
                var posibleTargets = new List<PossibleTarget>
                {
                    new PossibleTarget { Position = mainTargetPrediction.UnitPosition.To2D(), Unit = input.Unit }
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
                            input.From.To2D(), target.Position, (input.Radius), input.Range);
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
                                input.From.To2D(), candidate, (input.Radius + input.Unit.BoundingRadius / 3 - 10),
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
                                var dist = Vector2.DistanceSquared(bestCandidateHitPoints[i], proj1.LinePoint) +
                                           Vector2.DistanceSquared(bestCandidateHitPoints[j], proj2.LinePoint);
                                if (dist >= maxDistance &&
                                    (proj1.LinePoint - positionsList[i]).AngleBetween(
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
                            _aoeTargetsHitCount = bestCandidateHits,
                            UnitPosition = mainTargetPrediction.UnitPosition,
                            CastPosition = ((p1 + p2) * 0.5f).To3D(),
                            Input = input
                        };
                    }
                }

                return mainTargetPrediction;
            }
        }

        /// <summary>
        /// Represents a possible target.
        /// </summary>
        internal class PossibleTarget
        {
            /// <summary>
            /// The position
            /// </summary>
            public Vector2 Position;

            /// <summary>
            /// The unit
            /// </summary>
            public Obj_AI_Base Unit;
        }
    }

    /// <summary>
    /// Class that helps in calculating collision.
    /// </summary>
    public static class Collision
    {
        /// <summary>
        /// The tick yasuo casted wind wall.
        /// </summary>
        private static int _wallCastT;

        /// <summary>
        /// The yasuo wind wall casted position.
        /// </summary>
        private static Vector2 _yasuoWallCastedPos;

        /// <summary>
        /// Initializes static members of the <see cref="Collision"/> class.
        /// </summary>
        static Collision()
        {
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
        }

        /// <summary>
        /// Fired when the game processes a spell cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsValid && sender.Team != ObjectManager.Player.Team && args.SData.Name == "YasuoWMovingWall")
            {
                _wallCastT = Utils.TickCount;
                _yasuoWallCastedPos = sender.ServerPosition.To2D();
            }
        }

        /// <summary>
        /// Returns the list of the units that the skillshot will hit before reaching the set positions.
        /// </summary>
        /// <param name="positions">The positions.</param>
        /// <param name="input">The input.</param>
        /// <returns>List&lt;Obj_AI_Base&gt;.</returns>
        public static List<Obj_AI_Base> GetCollision(List<Vector3> positions, PredictionInput input)
        {
            var result = new List<Obj_AI_Base>();

            foreach (var position in positions)
            {
                foreach (var objectType in input.CollisionObjects)
                {
                    switch (objectType)
                    {
                        case CollisionableObjects.Minions:
                            foreach (var minion in
                                ObjectManager.Get<Obj_AI_Minion>()
                                    .Where(
                                        minion =>
                                            minion.IsValidTarget(
                                                Math.Min(input.Range + input.Radius + 100, 2000), true,
                                                input.RangeCheckFrom)))
                            {
                                input.Unit = minion;
                                var minionPrediction = Prediction.GetPrediction(input, false, false);
                                if (
                                    minionPrediction.UnitPosition.To2D()
                                        .Distance(input.From.To2D(), position.To2D(), true, true) <=
                                    Math.Pow((input.Radius + 15 + minion.BoundingRadius), 2))
                                {
                                    result.Add(minion);
                                }
                            }
                            break;
                        case CollisionableObjects.Heroes:
                            foreach (var hero in
                                HeroManager.Enemies.FindAll(
                                    hero =>
                                        hero.IsValidTarget(
                                            Math.Min(input.Range + input.Radius + 100, 2000), true, input.RangeCheckFrom))
                                )
                            {
                                input.Unit = hero;
                                var prediction = Prediction.GetPrediction(input, false, false);
                                if (
                                    prediction.UnitPosition.To2D()
                                        .Distance(input.From.To2D(), position.To2D(), true, true) <=
                                    Math.Pow((input.Radius + 50 + hero.BoundingRadius), 2))
                                {
                                    result.Add(hero);
                                }
                            }
                            break;
                            
                        case CollisionableObjects.Allies:
                            foreach (var hero in
                                HeroManager.Allies.FindAll(
                                    hero =>
                                       Vector3.Distance(ObjectManager.Player.ServerPosition, hero.ServerPosition) <= Math.Min(input.Range + input.Radius + 100, 2000))
                                )
                            {
                                input.Unit = hero;
                                var prediction = Prediction.GetPrediction(input, false, false);
                                if (
                                    prediction.UnitPosition.To2D()
                                        .Distance(input.From.To2D(), position.To2D(), true, true) <=
                                    Math.Pow((input.Radius + 50 + hero.BoundingRadius), 2))
                                {
                                    result.Add(hero);
                                }
                            }
                            break;
                            

                        case CollisionableObjects.Walls:
                            var step = position.Distance(input.From) / 20;
                            for (var i = 0; i < 20; i++)
                            {
                                var p = input.From.To2D().Extend(position.To2D(), step * i);
                                if (NavMesh.GetCollisionFlags(p.X, p.Y).HasFlag(CollisionFlags.Wall))
                                {
                                    result.Add(ObjectManager.Player);
                                }
                            }
                            break;

                        case CollisionableObjects.YasuoWall:

                            if (Utils.TickCount - _wallCastT > 4000)
                            {
                                break;
                            }

                            GameObject wall = null;
                            foreach (var gameObject in
                                ObjectManager.Get<GameObject>()
                                    .Where(
                                        gameObject =>
                                            gameObject.IsValid &&
                                            Regex.IsMatch(
                                                gameObject.Name, "_w_windwall_enemy_0.\\.troy", RegexOptions.IgnoreCase))
                                )
                            {
                                wall = gameObject;
                            }
                            if (wall == null)
                            {
                                break;
                            }
                            var level = wall.Name.Substring(wall.Name.Length - 6, 1);
                            var wallWidth = (300 + 50 * Convert.ToInt32(level));

                            var wallDirection =
                                (wall.Position.To2D() - _yasuoWallCastedPos).Normalized().Perpendicular();
                            var wallStart = wall.Position.To2D() + wallWidth / 2f * wallDirection;
                            var wallEnd = wallStart - wallWidth * wallDirection;

                            if (wallStart.Intersection(wallEnd, position.To2D(), input.From.To2D()).Intersects)
                            {
                                var t = Utils.TickCount +
                                        (wallStart.Intersection(wallEnd, position.To2D(), input.From.To2D())
                                            .Point.Distance(input.From) / input.Speed + input.Delay) * 1000;
                                if (t < _wallCastT + 4000)
                                {
                                    result.Add(ObjectManager.Player);
                                }
                            }

                            break;
                    }
                }
            }

            return result.Distinct().ToList();
        }
    }

    /// <summary>
    /// Represents the path of a unit.
    /// </summary>
    internal class StoredPath
    {
        /// <summary>
        /// The path
        /// </summary>
        public List<Vector2> Path;

        /// <summary>
        /// The tick
        /// </summary>
        public int Tick;

        /// <summary>
        /// Gets the time.
        /// </summary>
        /// <value>The time.</value>
        public double Time
        {
            get { return (Utils.TickCount - Tick) / 1000d; }
        }

        /// <summary>
        /// Gets the waypoint count.
        /// </summary>
        /// <value>The waypoint count.</value>
        public int WaypointCount
        {
            get { return Path.Count; }
        }

        /// <summary>
        /// Gets the start point.
        /// </summary>
        /// <value>The start point.</value>
        public Vector2 StartPoint
        {
            get { return Path.FirstOrDefault(); }
        }

        /// <summary>
        /// Gets the end point.
        /// </summary>
        /// <value>The end point.</value>
        public Vector2 EndPoint
        {
            get { return Path.LastOrDefault(); }
        }
    }

    /// <summary>
    /// Tracks the path of units.
    /// </summary>
    internal static class PathTracker
    {
        /// <summary>
        /// The maximum time
        /// </summary>
        private const double MaxTime = 1.5d;

        /// <summary>
        /// The stored paths
        /// </summary>
        private static readonly Dictionary<int, List<StoredPath>> StoredPaths = new Dictionary<int, List<StoredPath>>();

        /// <summary>
        /// Initializes static members of the <see cref="PathTracker"/> class.
        /// </summary>
        static PathTracker()
        {
            Obj_AI_Base.OnNewPath += Obj_AI_Hero_OnNewPath;
        }

        /// <summary>
        /// Fired when a unit changes it's path.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectNewPathEventArgs"/> instance containing the event data.</param>
        private static void Obj_AI_Hero_OnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
        {
            if (!(sender is Obj_AI_Hero))
            {
                return;
            }

            if (!StoredPaths.ContainsKey(sender.NetworkId))
            {
                StoredPaths.Add(sender.NetworkId, new List<StoredPath>());
            }

            var newPath = new StoredPath { Tick = Utils.TickCount, Path = args.Path.ToList().To2D() };
            StoredPaths[sender.NetworkId].Add(newPath);

            if (StoredPaths[sender.NetworkId].Count > 50)
            {
                StoredPaths[sender.NetworkId].RemoveRange(0, 40);
            }
        }

        /// <summary>
        /// Gets the stored paths.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="maxT">The maximum t.</param>
        /// <returns>List&lt;StoredPath&gt;.</returns>
        public static List<StoredPath> GetStoredPaths(Obj_AI_Base unit, double maxT)
        {
            return StoredPaths.ContainsKey(unit.NetworkId)
                ? StoredPaths[unit.NetworkId].Where(p => p.Time < maxT).ToList()
                : new List<StoredPath>();
        }

        /// <summary>
        /// Gets the current path.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns>StoredPath.</returns>
        public static StoredPath GetCurrentPath(Obj_AI_Base unit)
        {
            return StoredPaths.ContainsKey(unit.NetworkId)
                ? StoredPaths[unit.NetworkId].LastOrDefault()
                : new StoredPath();
        }

        /// <summary>
        /// Gets the tendency.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns>Vector3.</returns>
        public static Vector3 GetTendency(Obj_AI_Base unit)
        {
            var paths = GetStoredPaths(unit, MaxTime);
            var result = new Vector2();

            foreach (var path in paths)
            {
                var k = 1; //(MaxTime - path.Time);
                result = result + k * (path.EndPoint - unit.ServerPosition.To2D() /*path.StartPoint*/).Normalized();
            }

            result /= paths.Count;

            return result.To3D();
        }

        /// <summary>
        /// Gets the mean speed.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="maxT">The maximum t.</param>
        /// <returns>System.Double.</returns>
        public static double GetMeanSpeed(Obj_AI_Base unit, double maxT)
        {
            var paths = GetStoredPaths(unit, MaxTime);
            var distance = 0d;
            if (paths.Count > 0)
            {
                //Assume that the unit was moving for the first path:
                distance += (maxT - paths[0].Time) * unit.MoveSpeed;

                for (var i = 0; i < paths.Count - 1; i++)
                {
                    var currentPath = paths[i];
                    var nextPath = paths[i + 1];

                    if (currentPath.WaypointCount > 0)
                    {
                        distance += Math.Min(
                            (currentPath.Time - nextPath.Time) * unit.MoveSpeed, currentPath.Path.PathLength());
                    }
                }

                //Take into account the last path:
                var lastPath = paths.Last();
                if (lastPath.WaypointCount > 0)
                {
                    distance += Math.Min(lastPath.Time * unit.MoveSpeed, lastPath.Path.PathLength());
                }
            }
            else
            {
                return unit.MoveSpeed;
            }


            return distance / maxT;
        }
    }
}
