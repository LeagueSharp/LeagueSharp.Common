#region

using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace LeagueSharp.Common
{
    public class Prediction
    {
        public enum HitChance
        {
            Dashing = 4,
            Immobile = 3,
            HighHitchance = 2,
            LowHitchance = 1,
            CantHit = 0,
            Collision = -1,
        }

        public enum SkillshotType
        {
            SkillshotLine,
            SkillshotCircle,
            SkillshotCone,
        }

        private static readonly Dictionary<int, Dash> Dashes = new Dictionary<int, Dash>();
        private static readonly Dictionary<int, float> ImmobileT = new Dictionary<int, float>();

        private static readonly Dictionary<string, BlinkData> Blinks = new Dictionary<string, BlinkData>();
        private static readonly Dictionary<string, float> ImmobileData = new Dictionary<string, float>();
        private static readonly Dictionary<string, float> DashData = new Dictionary<string, float>();

        static Prediction()
        {
            if (Common.isInitialized == false)
            {
                Common.InitializeCommonLib();
            }

            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
            Game.OnGameProcessPacket += OnProcessPacket;
            Game.OnGameUpdate += OnTick;
            //Drawing.OnDraw += Draw;

            Blinks.Add("summonerflash", new BlinkData(400, 0.6f, false));
            Blinks.Add("ezrealarcaneshift", new BlinkData(475, 0.8f, false));
            Blinks.Add("deceive", new BlinkData(400, 0.8f, false));
            Blinks.Add("riftwalk", new BlinkData(700, 0.8f, false));
            Blinks.Add("katarinae", new BlinkData(float.MaxValue, 0.8f, false));
            Blinks.Add("elisespideredescent", new BlinkData(float.MaxValue, 0.8f, false));
            Blinks.Add("elisespidere", new BlinkData(float.MaxValue, 0.8f, false));


            ImmobileData.Add("katarinar", 1.0f); //Katarina's R
            ImmobileData.Add("drain", 1.0f); //Fiddlesticks W
            ImmobileData.Add("consume", 0.5f); //Nunu's Q
            ImmobileData.Add("absolutezero", 1.0f); //Nunu's R
            ImmobileData.Add("rocketgrab", 0.5f); //Blitzcrank's Q
            ImmobileData.Add("staticfield", 0.5f); //Blitzcrank's R
            ImmobileData.Add("cassiopeiapetrifyinggaze", 0.5f); //Hackssiopeia's R
            ImmobileData.Add("ezrealtrueshotbarrage", 1.0f); //Ezreal's R
            ImmobileData.Add("galioidolofdurand", 1.0f); //Galio's R
            ImmobileData.Add("luxmalicecannon", 1.0f); //Lux's R
            ImmobileData.Add("reapthewhirlwind", 1.0f); //Janna's R
            ImmobileData.Add("jinxw", 0.6f); //Jinx's W
            ImmobileData.Add("jinxr", 0.6f); //Jinx's R
            ImmobileData.Add("missfortunebullettime", 1.0f); //MissFortune's R
            ImmobileData.Add("shenstandunited", 1.0f); //Shen's R
            ImmobileData.Add("threshe", 0.5f); //Tresh's E
            ImmobileData.Add("threshrpenta", 0.7f); //Tresh's R
            ImmobileData.Add("infiniteduress", 1.0f); //WarWicks R
            ImmobileData.Add("meditate", 1.0f); //MasterYi's W

            DashData.Add("ahritumble", 0.25f); //ahri's r
            DashData.Add("akalishadowdance", 0.25f); //akali r
            DashData.Add("headbutt", 0.25f); //alistar w
            DashData.Add("caitlynentrapment", 0.25f); //caitlyn e
            DashData.Add("carpetbomb", 0.25f); //corki w
            DashData.Add("dianateleport", 0.25f); //diana r
            DashData.Add("fizzpiercingstrike", 0.25f); //fizz q
            DashData.Add("fizzjump", 0.25f); //fizz e
            DashData.Add("gragasbodyslam", 0.25f); //gragas e
            DashData.Add("gravesmove", 0.25f); //graves e
            DashData.Add("ireliagatotsu", 0.25f); //irelia q
            DashData.Add("jarvanivdragonstrike", 0.25f); //jarvan q
            DashData.Add("jaxleapstrike", 0.25f); //jax q
            DashData.Add("khazixe", 0.25f); //khazix e and e evolved
            DashData.Add("leblancslide", 0.25f); //leblanc w
            DashData.Add("leblancslidem", 0.25f); //leblanc w (r)
            DashData.Add("blindmonkqtwo", 0.25f); //lee sin q
            DashData.Add("blindmonkwone", 0.25f); //lee sin w
            DashData.Add("luciane", 0.25f); //lucian e
            DashData.Add("maokaiunstablegrowth", 0.25f); //maokai w
            DashData.Add("pounce", 0.25f); //nidalees w
            DashData.Add("nocturneparanoia2", 0.25f); //nocturne r
            DashData.Add("pantheon_leapbash", 0.25f); //pantheon e?
            DashData.Add("renektonsliceanddice", 0.25f); //renekton e                 
            DashData.Add("riventricleave", 0.25f); //riven q          
            DashData.Add("rivenfeint", 0.25f); //riven e      
            DashData.Add("sejuaniarcticassault", 0.25f); //sejuani q
            DashData.Add("shenshadowdash", 0.25f); //shen e
            DashData.Add("shyvanatransformcast", 0.25f); //shyvana r
            DashData.Add("rocketjump", 0.25f); //tristana w
            DashData.Add("slashcast", 0.25f); //tryndamere e
            DashData.Add("vaynetumble", 0.25f); //vayne q
            DashData.Add("viq", 0.25f); //vi q
            DashData.Add("monkeykingnimbus", 0.25f); //wukong q
            DashData.Add("xenzhaosweep", 0.25f); //xin xhao q
            DashData.Add("yasuodashwrapper", 0.25f); //yasuo e
        }

        private static void OnTick(EventArgs args)
        {
            foreach (var unit in ObjectManager.Get<Obj_AI_Hero>())
                if (Dashes.ContainsKey(unit.NetworkId))
                {
                    if (!Dashes[unit.NetworkId].processed)
                    {
                        var duration = unit.GetWaypoints().PathLength() / Dashes[unit.NetworkId].Speed;
                        Dashes[unit.NetworkId].endT = Game.Time + duration;
                        Dashes[unit.NetworkId].processed = true;
                        //Game.PrintChat("New Dash" + duration);
                    }
                }
        }

        private static void OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs args)
        {
            if (unit.Type == ObjectManager.Player.Type)
            {
                if (ImmobileData.ContainsKey(args.SData.Name.ToLower()))
                {
                    if (!ImmobileT.ContainsKey(unit.NetworkId))
                    {
                        ImmobileT.Add(unit.NetworkId, 0f);
                    }
                    ImmobileT[unit.NetworkId] = Game.Time + ImmobileData[args.SData.Name.ToLower()];
                }

                if (Blinks.ContainsKey(args.SData.Name.ToLower()))
                {
                    var bdata = Blinks[args.SData.Name.ToLower()];
                    var endPos = args.End.To2D();
                    if (Vector2.DistanceSquared(endPos, unit.ServerPosition.To2D()) > bdata.range * bdata.range)
                    {
                        var Direction = endPos - unit.ServerPosition.To2D();
                        Direction.Normalize();
                        endPos = unit.ServerPosition.To2D() + Direction * bdata.range;
                    }

                    var p = unit.GetPath(endPos.To3D());
                    endPos = p[p.Count() - 1].To2D();

                    if (!Dashes.ContainsKey(unit.NetworkId))
                        Dashes.Add(unit.NetworkId, new Dash(0, 0, true, new Vector2()));

                    Dashes[unit.NetworkId].endT = Game.Time + bdata.delay;
                    Dashes[unit.NetworkId].EndPos = endPos;
                    Dashes[unit.NetworkId].IsBlink = true;
                }

                if (DashData.ContainsKey(args.SData.Name.ToLower()))
                {
                    //TODO: Needs to get calculated
                }
            }
        }

        private static void OnProcessPacket(GamePacketEventArgs args)
        {
            /*Dash*/
            if (args.PacketData[0] == Packet.S2C.Dash.Header)
            {
                var decodedPacket = Packet.S2C.Dash.Decoded(args.PacketData);
                var unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(decodedPacket.UnitNetworkId);
                if (unit.IsValid && unit.Type == GameObjectType.obj_AI_Hero)
                {
                    if (!Dashes.ContainsKey(unit.NetworkId))
                        Dashes.Add(unit.NetworkId, new Dash(0, 0, false, new Vector2()));

                    Dashes[unit.NetworkId].processed = false;
                    Dashes[unit.NetworkId].Speed = decodedPacket.Speed;
                    Dashes[unit.NetworkId].IsBlink = false;
                }
            }
        }

        private static void Draw(EventArgs args)
        {
            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (hero.Team != ObjectManager.Player.Team)
                {
                    var Out = GetBestPosition(hero, 0.25f, 75, 1000, Game.CursorPos, 900, true,
                        SkillshotType.SkillshotCircle, Game.CursorPos);

                    Drawing.DrawCircle(Out.CastPosition, 200, Color.White);
                    Drawing.DrawCircle(Out.Position, 200, Color.GreenYellow);
                    Drawing.DrawText(100, 100, Color.White, Out.HitChance.ToString());

                    foreach (var units in Out.CollisionUnitsList)
                    {
                        Drawing.DrawCircle(units.ServerPosition, 100, Color.Red);
                    }
                }
            }
        }

        private static float GetHitBox(Obj_AI_Base unit)
        {
            return unit.BoundingRadius;
        }

        private static float ExtraDelay()
        {
            return 0.07f + Game.Ping / 1000;
        }

        public static float ImmobileTime(Obj_AI_Base unit)
        {
            var result = ImmobileT.ContainsKey(unit.NetworkId) ? ImmobileT[unit.NetworkId] : 0f;

            foreach (var buff in unit.Buffs)
            {
                if (buff.IsActive && Game.Time <= buff.EndTime &&
                    (buff.Type == BuffType.Charm ||
                     buff.Type == BuffType.Knockup ||
                     buff.Type == BuffType.Stun ||
                     buff.Type == BuffType.Suppression ||
                     buff.Type == BuffType.Snare))
                {
                    result = Math.Max(result, buff.EndTime);
                }
            }
            return result;
        }

        public static PredictionOutput GetBestPosition(Obj_AI_Base unit, float delay, float width, float speed,
            Vector3 from,
            float range, bool collision, SkillshotType stype, Vector3 rangeCheckFrom = new Vector3())
        {
            var result = new PredictionOutput(new Vector2(), new Vector2(), 0);
            if (!rangeCheckFrom.To2D().IsValid())
            {
                rangeCheckFrom = ObjectManager.Player.ServerPosition;
            }
            if (!unit.IsValidTarget(float.MaxValue, false))
            {
                return result;
            }
            delay += ExtraDelay();
            width = Math.Max(width - 5, 1) + GetHitBox(unit);

            var ImmobileT = ImmobileTime(unit);
            if (Dashes.ContainsKey(unit.NetworkId) && Dashes[unit.NetworkId].endT >= Game.Time)
            {
                /*Unit Dashing*/

                if (!Dashes[unit.NetworkId].IsBlink)
                {
                    var iPrediction = GetUnitPosition(unit.GetWaypoints(),
                        Dashes[unit.NetworkId].Speed, delay, speed, 1, @from.To2D());

                    if (iPrediction.Valid)
                    {
                        /* Mid air */
                        result.CastPosition = iPrediction.CastPosition;
                        result.Position = iPrediction.Position;
                        result.HitChance = HitChance.Dashing;
                    }
                    else
                    {
                        /* Check if we can hit after landing */
                        var endPoint = unit.GetWaypoints()[unit.GetWaypoints().Count - 1];
                        var landtime = Game.Time +
                                       Vector2.Distance(endPoint,
                                           from.To2D()) / speed + delay;

                        if ((landtime - Dashes[unit.NetworkId].endT) * unit.MoveSpeed < width * 1.25 ||
                            unit.BaseSkinName == "Riven")
                        {
                            result.CastPosition = endPoint.To3D();
                            result.Position = endPoint.To3D();
                            result.HitChance = HitChance.Dashing;
                        }
                        else
                        {
                            result.CastPosition = endPoint.To3D();
                            result.Position = endPoint.To3D();
                            result.HitChance = HitChance.CantHit;
                        }
                    }
                }
                    /*Blinks*/
                else
                {
                    var endPoint = Dashes[unit.NetworkId].EndPos;
                    var totaldelay = delay + Vector2.Distance(endPoint, @from.To2D()) / speed;

                    result.Position = endPoint.To3D();
                    result.CastPosition = endPoint.To3D();

                    if ((Dashes[unit.NetworkId].endT - Game.Time + width / unit.MoveSpeed) >= totaldelay)
                    {
                        result.HitChance = HitChance.Dashing;
                    }
                    else //TODO:Get the location where he is most likely going after blinking
                    {
                        result.HitChance = HitChance.CantHit;
                    }
                }
            }
            else if (ImmobileT >= Game.Time)
            {
                var timeToHit = delay - width / unit.MoveSpeed +
                                Vector2.Distance(unit.ServerPosition.To2D(), @from.To2D()) / speed;

                if (ImmobileT - Game.Time >= timeToHit)
                {
                    /* Unit will  be immobile */
                    result.CastPosition = unit.ServerPosition;
                    result.Position = unit.ServerPosition;
                    result.HitChance = HitChance.Immobile;
                }
                else
                {
                    /* Unit will be able to escape if we cast just in the position he is. TODO: Calculate the escape route he will likely take */
                    result.CastPosition = unit.ServerPosition;
                    result.Position = unit.ServerPosition;
                    result.HitChance = HitChance.HighHitchance;
                }
            }
            else
            {
                var Waypoints = unit.GetWaypoints();
                if (Waypoints.Count == 1)
                {
                    /*Unit not moving*/
                    result.CastPosition = unit.ServerPosition;
                    result.Position = unit.ServerPosition;
                    result.HitChance = HitChance.HighHitchance;
                }
                else
                {
                    var iPrediction = GetUnitPosition(Waypoints, unit.MoveSpeed, delay, speed,
                        width, @from.To2D());

                    if (iPrediction.Valid)
                    {
                        result.CastPosition = iPrediction.CastPosition;
                        result.Position = iPrediction.Position;
                        result.HitChance = HitChance.HighHitchance;
                        /* Change the hitchance according to the path change rate */
                    }
                    else
                    {
                        result.CastPosition = iPrediction.CastPosition;
                        result.Position = iPrediction.Position;
                        result.HitChance = HitChance.CantHit;
                    }
                }
            }

            if (range != float.MaxValue)
            {
                if (stype != SkillshotType.SkillshotCircle)
                {
                    if (Vector2.DistanceSquared(rangeCheckFrom.To2D(), result.Position.To2D()) >= range * range)
                    {
                        result.HitChance = HitChance.CantHit;
                    }

                    if (Vector2.DistanceSquared(rangeCheckFrom.To2D(), result.CastPosition.To2D()) >=
                        range * range)
                    {
                        result.HitChance = HitChance.CantHit;
                    }
                }
                else
                {
                    if (Vector2.DistanceSquared(rangeCheckFrom.To2D(), result.Position.To2D()) >=
                        Math.Pow(range + width, 2))
                    {
                        result.HitChance = HitChance.CantHit;
                    }
                    if (Vector2.DistanceSquared(rangeCheckFrom.To2D(), result.CastPosition.To2D()) >=
                        Math.Pow(range + width, 2))
                    {
                        result.HitChance = HitChance.CantHit;
                    }

                    if (Vector2.DistanceSquared(rangeCheckFrom.To2D(), result.CastPosition.To2D()) >= range * range)
                        result.CastPosition = rangeCheckFrom +
                                              (range - 10) *
                                              (result.CastPosition - rangeCheckFrom).To2D().Normalized().To3D();
                }
            }

            if (collision && result.HitChance > HitChance.CantHit)
            {
                var CheckLocations = new List<Vector2>();
                CheckLocations.Add(result.Position.To2D());
                CheckLocations.Add(unit.ServerPosition.To2D());
                CheckLocations.Add(result.CastPosition.To2D());

                var Col1 = GetCollision(from.To2D(), CheckLocations, stype, width - GetHitBox(unit),
                    delay, speed, range);

                if (Col1.Count > 0)
                {
                    result.HitChance = HitChance.Collision;
                    result.CollisionUnitsList.AddRange(Col1);
                }
            }

            return result;
        }

        public static PredictionOutput GetBestAOEPosition(Obj_AI_Base unit, float delay, float width, float speed,
            Vector3 from, float range, bool collision,
            SkillshotType spelltype, Vector3 rangeCheckFrom = new Vector3(), float accel = -1483)
        {
            var objects = new PredictionOutput(new Vector2(), new Vector2(), 0);
            if (rangeCheckFrom.X.CompareTo(0) == 0)
            {
                rangeCheckFrom = ObjectManager.Player.ServerPosition;
            }
            switch (spelltype)
            {
                case SkillshotType.SkillshotLine:
                    objects = GetAoeLinePrediction(unit, width, range, delay, speed, collision, from, rangeCheckFrom);
                    break;
                case SkillshotType.SkillshotCircle:
                    objects = GetAoeCirclePrediction(unit, width, range, delay, speed, collision, from, rangeCheckFrom);
                    break;
                case SkillshotType.SkillshotCone:
                    objects = GetAoeConePrediction(unit, width, range, delay, speed, collision, from, rangeCheckFrom);
                    break;
                    //case SkillshotAOEType.SkillshotArc:
                    //    objects = GetAoeArcPrediction(unit, width, range, delay, speed, collision, from, rangeCheckFrom, accel);
                    //    break;
            }

            return objects;
        }

        public static List<Obj_AI_Base> GetCollision(Vector2 from, List<Vector2> To, SkillshotType stype, float width,
            float delay, float speed, float range)
        {
            var result = new List<Obj_AI_Base>();
            delay -= ExtraDelay();

            foreach (var TestPosition in To)
            {
                foreach (var collisionObject in ObjectManager.Get<Obj_AI_Minion>())
                {
                    if (collisionObject.IsValidTarget() && collisionObject.Team != ObjectManager.Player.Team &&
                        Vector2.DistanceSquared(from, collisionObject.Position.To2D()) <= Math.Pow(range * 1.5, 2))
                    {
                        var objectPrediction = GetBestPosition(collisionObject, delay, width, speed,
                            from.To3D(), float.MaxValue,
                            false, stype, @from.To3D());
                        if (
                            objectPrediction.Position.To2D().Distance(from, TestPosition, true, true) <=
                            Math.Pow((width + 15 + collisionObject.BoundingRadius), 2))
                        {
                            result.Add(collisionObject);
                            Drawing.DrawCircle(objectPrediction.Position, width + collisionObject.BoundingRadius,
                                Color.Red);
                        }
                    }
                }
            }

            /*Remove the duplicates*/
            result = result.Distinct().ToList();
            return result;
        }

        private static PredictionInternalOutput GetUnitPosition(List<Vector2> waypoints, float unitSpeed, float delay,
            float missileSpeed, float width, Vector2 from)
        {
            var result = new PredictionInternalOutput(new Vector2(), new Vector2(), false);

            if (waypoints.PathLength() > (delay * unitSpeed - width))
            {
                var path = Utils.CutPath(waypoints, delay * unitSpeed, width);

                if (missileSpeed == float.MaxValue)
                {
                    /*Spell with only a delay*/
                    var direction = (path[1] - path[0]).Normalized();
                    result.Position = path[0].To3D();
                    result.CastPosition = (path[0] - direction * width).To3D();

                    if ((path.Count == 2) && (Vector2.DistanceSquared(path[0], path[1]) <= width * width))
                    {
                        result.Position = (path[1] - direction * width).To3D();
                    }

                    result.Valid = true;
                    return result;
                }

                /*Spell with delay and missile*/
                var T = 0f;
                var Tb = 0f;
                for (var i = 0; i < path.Count - 1; i++)
                {
                    var A = path[i];
                    var B = path[i + 1];


                    var Sol = Geometry.VectorMovementCollision(A, B, unitSpeed, from, missileSpeed);
                    var t1 = (float)Sol[0];
                    var p1 = (Vector2)Sol[1];
                    var t = float.NaN;
                    var Tc = Tb;
                    Tb = T + Vector2.Distance(A, B) / unitSpeed;

                    if (!float.IsNaN(t1))
                    {
                        if (((t1 >= T) && (t1 <= Tb))
                            )
                        {
                            t = t1;
                        }
                        else if (t1 > Tb && i != path.Count - 2)
                        {
                            t = t1 - (Tb - T);
                            var nDirection = path[i + 2] - B;
                            nDirection.Normalize();
                            p1 = B + t * unitSpeed * nDirection;
                        }
                    }

                    if (!float.IsNaN(t))
                    {
                        var Direction = B - A;
                        Direction.Normalize();
                        var hitPosition = p1;
                        result.CastPosition = (hitPosition - width * Direction).To3D();
                        result.Valid = true;

                        if ((i == path.Count - 2) && (Vector2.DistanceSquared(hitPosition, B) <= width * width))
                        {
                            hitPosition = B - width * Direction;
                        }

                        result.Position = hitPosition.To3D();
                        return result;
                    }

                    T = Tb;
                }

                /*No solution found*/
                result.CastPosition = waypoints[waypoints.Count - 1].To3D();
                result.Position = waypoints[waypoints.Count - 1].To3D();
                result.Valid = false;
                return result;
            }
            /*Not enough waypoints. (Path too short)*/
            result.CastPosition = waypoints[waypoints.Count - 1].To3D();
            result.Position = waypoints[waypoints.Count - 1].To3D();
            result.Valid = false;
            return result;
        }

        private static PredictionOutput GetAoeCirclePrediction(Obj_AI_Base unit, float width, float range,
            float delay, float speed, bool collision, Vector3 from, Vector3 rangeCheckFrom)
        {
            var result = GetBestPosition(unit, delay, width, speed, from, range, collision,
                SkillshotType.SkillshotCircle, rangeCheckFrom);
            var Points = new List<Vector2>();

            Points.Add(result.Position.To2D());

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (enemy.Team != ObjectManager.Player.Team && enemy.IsValidTarget() &&
                    enemy.NetworkId != unit.NetworkId && Vector3.Distance(from, enemy.ServerPosition) <= range * 1.2)
                {
                    var pred = GetBestPosition(enemy, delay, width, speed, from, range, collision,
                        SkillshotType.SkillshotCircle, rangeCheckFrom);

                    if (pred.HitChance >= HitChance.CantHit)
                    {
                        Points.Add(pred.Position.To2D());
                    }
                }
            }

            while (Points.Count > 1)
            {
                var MecCircle = MEC.GetMec(Points);

                if (MecCircle.Radius <= width * 0.8 + GetHitBox(unit) - 8 &&
                    Vector2.DistanceSquared(MecCircle.Center, rangeCheckFrom.To2D()) < range * range)
                {
                    result.CastPosition = MecCircle.Center.To3D();
                    result.TargetsHit = Points.Count;
                    return result;
                }

                float maxdist = -1;
                var maxdistindex = 1;

                for (var i = 1; i < Points.Count; i++)
                {
                    var distance = Vector2.DistanceSquared(Points[i], Points[0]);
                    if (distance > maxdist || maxdist.CompareTo(-1) == 0)
                    {
                        maxdistindex = i;
                        maxdist = distance;
                    }
                }

                Points.RemoveAt(maxdistindex);
            }

            return result;
        }

        private static Vector2[] GetPossiblePoints(Vector2 from, Vector2 pos, float width, float range)
        {
            var middlePoint = (from + pos) / 2;
            var vectors = Geometry.CircleCircleIntersection(from, middlePoint, width,
                Vector2.Distance(middlePoint, from));
            var P1 = vectors[0];
            var P2 = vectors[1];

            var V1 = (P1 - from);
            var V2 = (P2 - from);

            V1 = (pos - V1 - from);
            V1.Normalize();
            V1 = Vector2.Multiply(V1, range);
            V1 = V1 + from;
            V2 = (pos - V2 - from);
            V2.Normalize();
            V2 = Vector2.Multiply(V2, range);
            V2 = V2 + from;
            return new[] { V1, V2 };
        }

        private static Object[] CountHits(Vector2 P1, Vector2 P2, float width, List<Vector2> points)
        {
            var hits = 0;
            var nPoints = new List<Vector2>();
            width = width + 2;
            for (var i = 0; i < points.Count; i++)
            {
                var point = points[i];
                var objects = point.ProjectOn(P1, P2);
                if (objects.IsOnSegment && Vector2.DistanceSquared(objects.SegmentPoint, point) <= width * width)
                {
                    hits = hits + 1;
                    nPoints.Add(point);
                }
                else if (i == 0)
                {
                    return new Object[] { hits, nPoints };
                }
            }
            return new Object[] { hits, nPoints };
        }

        private static PredictionOutput GetAoeLinePrediction(Obj_AI_Base unit, float width, float range, float delay,
            float speed,
            bool collision, Vector3 from, Vector3 rangeCheckFrom)
        {
            var result = GetBestPosition(unit, delay, width, speed, from, range, collision,
                SkillshotType.SkillshotLine, rangeCheckFrom);
            var points = new List<Vector2>();

            points.Add(result.Position.To2D());

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (enemy.IsEnemy && enemy.NetworkId != unit.NetworkId && enemy.IsValidTarget() && !enemy.IsDead &&
                    enemy.IsValid &&
                    Vector3.DistanceSquared(enemy.ServerPosition, ObjectManager.Player.ServerPosition) <=
                    (range * 1.2) * (range * 1.2))
                {
                    var pred = GetBestPosition(enemy, delay, width, speed, from, range, collision,
                        SkillshotType.SkillshotLine, rangeCheckFrom);

                    if (pred.HitChance >= HitChance.CantHit)
                    {
                        points.Add(pred.Position.To2D());
                    }
                }
            }

            var maxHit = 1;
            var maxHitPos = new Vector2();
            var maxHitPoints = new List<Vector2>();

            if (points.Count > 1)
            {
                width += unit.BoundingRadius * 3 / 4;
                for (var i = 0; i < points.Count; i++)
                {
                    var possiblePoints = GetPossiblePoints(@from.To2D(), points[i], width - 20, range);
                    Vector2 C1 = possiblePoints[0], C2 = possiblePoints[1];
                    var countHits1 = CountHits(@from.To2D(), C1, width, points);
                    var countHits2 = CountHits(@from.To2D(), C2, width, points);
                    if ((int)countHits1[0] >= maxHit)
                    {
                        maxHitPos = C1;
                        maxHit = (int)countHits1[0];
                        maxHitPoints = (List<Vector2>)countHits1[1];
                    }
                    if ((int)countHits2[0] >= maxHit)
                    {
                        maxHitPos = C2;
                        maxHit = (int)countHits2[0];
                        maxHitPoints = (List<Vector2>)countHits2[1];
                    }
                }
            }

            if (maxHit > 1)
            {
                float maxDistance = -1;
                Vector2 p1 = new Vector2(), p2 = new Vector2();
                for (var i = 0; i < maxHitPoints.Count; i++)
                {
                    for (var j = 0; j < maxHitPoints.Count; j++)
                    {
                        var startP = @from.To2D();
                        var endP = (maxHitPoints[i] + maxHitPoints[j]) / 2;
                        var objects01 = maxHitPoints[i].ProjectOn(startP, endP);
                        var objects02 = maxHitPoints[j].ProjectOn(startP, endP);

                        var dist =
                            Vector2.DistanceSquared(maxHitPoints[i], objects01.LinePoint) +
                            Vector2.DistanceSquared(maxHitPoints[j], objects02.LinePoint);
                        if (dist >= maxDistance)
                        {
                            maxDistance = dist;
                            result.CastPosition = ((p1 + p2) / 2).To3D();
                            result.TargetsHit = maxHit;
                            p1 = maxHitPoints[i];
                            p2 = maxHitPoints[j];
                        }
                    }
                }
                return result;
            }
            return result;
        }

        private static Object[] CountVectorBetween(Vector2 V1, Vector2 V2, List<Vector2> points)
        {
            var result = 0;
            var hitpoints = new List<Vector2>();
            for (var i = 0; i < points.Count; i++)
            {
                var t = points[i];
                var NVector = V1.CrossProduct(t);
                var NVector2 = t.CrossProduct(V2);
                if (NVector >= 0 && NVector2 >= 0)
                {
                    result = result + 1;
                    hitpoints.Add(t);
                }
                else if (i == 0)
                {
                    return new object[] { -1, hitpoints };
                }
            }
            return new object[] { result, hitpoints };
        }

        private static Object[] CheckHit(Vector2 position, float angle, List<Vector2> points)
        {
            var v1 = position.Rotated(-angle / 2);
            var v2 = position.Rotated(angle / 2);
            return CountVectorBetween(v1, v2, points);
        }

        private static PredictionOutput GetAoeConePrediction(Obj_AI_Base unit, float angle, float range, float delay,
            float speed,
            bool collision, Vector3 from, Vector3 rangeCheckFrom)
        {
            var result = GetBestPosition(unit, delay, 1, speed, from, range, collision,
                SkillshotType.SkillshotLine, rangeCheckFrom);
            var points = new List<Vector2>();

            points.Add(result.Position.To2D());

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (enemy.IsEnemy && enemy.NetworkId != unit.NetworkId && enemy.IsValidTarget() && !enemy.IsDead &&
                    enemy.IsValid &&
                    Vector3.Distance(from, enemy.ServerPosition) <= range)
                {
                    var pred = GetBestPosition(enemy, delay, 1, speed, from, range, collision,
                        SkillshotType.SkillshotLine, rangeCheckFrom);

                    if (pred.HitChance >= HitChance.CantHit)
                    {
                        points.Add(pred.Position.To2D());
                    }
                }
            }

            var maxHit = 1;
            var maxHitPos = new Vector2();
            var maxHitPoints = new List<Vector2>();

            if (points.Count > 1)
            {
                for (var i = 0; i < points.Count; i++)
                {
                    var point = points[i];
                    var pos1 = point.Rotated(angle / 2);
                    var pos2 = point.Rotated(-angle / 2);

                    var objects3 = CheckHit(pos1, angle, points);
                    var hits3 = (int)objects3[0];
                    var points3 = (List<Vector2>)objects3[1];
                    var objects4 = CheckHit(pos2, angle, points);
                    var hits4 = (int)objects4[0];
                    var points4 = (List<Vector2>)objects4[1];

                    if (hits3 >= maxHit)
                    {
                        maxHitPos = pos1;
                        maxHit = hits3;
                        maxHitPoints = points3;
                    }
                    if (hits4 >= maxHit)
                    {
                        maxHitPos = pos2;
                        maxHit = hits4;
                        maxHitPoints = points4;
                    }
                }
            }

            if (maxHit > 1)
            {
                float maxangle = -1;
                var p1 = new Vector2();
                var p2 = new Vector2();
                for (var i = 0; i < maxHitPoints.Count; i++)
                {
                    var hitp = maxHitPoints[i];
                    for (var j = 0; j < maxHitPoints.Count; j++)
                    {
                        var hitp2 = maxHitPoints[j];
                        var cangle = hitp2.AngleBetween(hitp);
                        if (cangle > maxangle)
                        {
                            maxangle = cangle;
                            result.CastPosition = (((p1) + (p2)) / 2).To3D();
                            result.TargetsHit = maxHit;
                            p1 = hitp;
                            p2 = hitp2;
                        }
                    }
                }
                return result;
            }
            return result;
        }

        private static bool AreClockwise(Vector2 vec1, Vector2 vec2)
        {
            return ((-vec1.X * vec2.Y + vec1.Y * vec2.X) > 0);
        }

        private static float[] GetBoundingVectors(List<Vector2> targets)
        {
            var largeN = 0;
            Vector2 v1 = new Vector2(), v2 = new Vector2(), v3 = new Vector2();
            Vector2 largeV1 = new Vector2(), largeV2 = new Vector2();
            float theta1 = 0, theta2 = 0;

            if (targets.Count >= 2)
            {
                for (var i = 0; i < targets.Count; i++)
                {
                    for (var j = 0; j < targets.Count; j++)
                    {
                        if (i != j)
                        {
                            v1 = new Vector2(targets[i].X - ObjectManager.Player.ServerPosition.X,
                                targets[i].Y - ObjectManager.Player.ServerPosition.Y);
                            v2 = new Vector2(targets[j].X - ObjectManager.Player.ServerPosition.X,
                                targets[j].Y - ObjectManager.Player.ServerPosition.Y);
                            if (targets.Count == 2)
                            {
                                largeV1 = v1;
                                largeV2 = v2;
                            }
                            else
                            {
                                var tempN = 0;
                                for (var k = 0; k < targets.Count; k++)
                                {
                                    if (k != i && k != j)
                                    {
                                        v3 = new Vector2(targets[k].X - ObjectManager.Player.ServerPosition.X,
                                            targets[k].Y - ObjectManager.Player.ServerPosition.Y);
                                        if (AreClockwise(v3, v1) && !AreClockwise(v3, v2))
                                        {
                                            tempN = tempN + 1;
                                        }
                                    }
                                }
                                if (tempN > largeN)
                                {
                                    largeN = tempN;
                                    largeV1 = v1;
                                    largeV2 = v2;
                                }
                            }
                        }
                    }
                }
            }
            theta1 = largeV1.Polar() - 20;
            theta2 = largeV2.Polar() + 20;
            if (theta2 < theta1)
            {
                theta1 = theta1 - 360;
            }
            return new[] { theta1, theta2 };
        }

        private static Object[] CrescentCollision(List<Vector2> points, Vector2 from, float rangeMax, float accel)
        {
            float thetaIterator = 5; //increase to improve performance (0 - 10)
            float rangeIterator = 5; //increase to improve performance (from 0-100)
            float roundRange = 200; //higher means more minions collected, but possibly less accurate.

            var targetOriginal = new Vector2();
            var targetArray = points;
            var tsTargetOriginal = new Vector2();
            float theta, tsTargetAngle = 0.0f, targetAngle, tsAngle, tsVo, tsTestZ, angle, vo, testZ;
            Vector2 tsTarget, target = new Vector2();
            var tsFlag = false;
            var highestCollision = 0;
            float highestAngle = 0;
            float highestRange = 0;
            if (points.Count > 0)
            {
                tsTargetOriginal = new Vector2(points[0].X - from.X, points[0].Y - from.Y);
                tsTargetAngle = tsTargetOriginal.Polar();
            }
            if (points.Count > 1)
            {
                var thetas = GetBoundingVectors(targetArray);
                float rightTheta = thetas[0], leftTheta = thetas[1];
                for (var newTheta = rightTheta; newTheta < leftTheta; newTheta = newTheta + thetaIterator)
                {
                    theta = Geometry.DegreeToRadian(newTheta);
                    for (float range = 400; range < rangeMax; range = range + rangeIterator)
                    {
                        if (highestCollision < targetArray.Count)
                        {
                            var collisionCount = 0;
                            tsTargetOriginal = new Vector2(points[0].X - from.X, points[0].Y - from.Y);
                            tsTarget = tsTargetOriginal.Rotated(theta);
                            tsAngle = Geometry.DegreeToRadian((-47) - (830 - range) / (-20)); //interpolate launch angle
                            tsVo = (float)Math.Sqrt((range * accel) / Math.Sin(2 * tsAngle)); //initial velocity
                            tsTestZ = (float)(Math.Tan(tsAngle) * tsTarget.X -
                                              (accel / (Math.Pow(2 * tsVo, 2) * Math.Pow(Math.Cos(tsAngle), 2))) *
                                              Math.Pow(tsTarget.X, 2));
                            if (Math.Abs(Math.Ceiling(tsTestZ) - Math.Ceiling(points[0].Y)) <= roundRange)
                            {
                                tsFlag = true;
                                collisionCount = collisionCount + 1;
                            }
                            else
                            {
                                tsFlag = false;
                            }

                            if (tsFlag)
                            {
                                foreach (var hero in targetArray)
                                {
                                    if (hero.X.CompareTo(targetArray[0].X) == 0)
                                    {
                                        continue;
                                    }
                                    targetOriginal = new Vector2(hero.X - from.X, hero.Y - from.Y);
                                    targetAngle = targetOriginal.Polar();

                                    if ((targetAngle <= newTheta) &&
                                        ((tsTargetAngle <= newTheta)))
                                        //angle of theta must be greater than target
                                    {
                                        target = targetOriginal.Rotated(theta); //rotate to neutral axis
                                        angle = Geometry.DegreeToRadian((-47) - (830 - range) / (-20));
                                        //interpolate launch angle
                                        vo = (float)Math.Sqrt((range * accel) / Math.Sin(2 * angle)); //initial velocity
                                        testZ =
                                            (float)
                                                (Math.Tan(angle) * target.X -
                                                 (accel / (Math.Pow(2 * vo, 2) * Math.Pow(Math.Cos(angle), 2))) *
                                                 Math.Pow(target.X, 2));

                                        if (Math.Abs(Math.Ceiling(testZ) - Math.Ceiling(target.Y)) <= roundRange)
                                            //compensate for rounding
                                            //collision detected
                                        {
                                            collisionCount = collisionCount + 1;
                                        }

                                        if (collisionCount > highestCollision)
                                        {
                                            highestCollision = collisionCount;
                                            highestAngle = theta; //in radians
                                            highestRange = range;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return new Object[]
            {
                (float)(ObjectManager.Player.ServerPosition.X + highestRange * Math.Cos(highestAngle)),
                (float)(ObjectManager.Player.ServerPosition.Y + highestRange * Math.Sin(highestAngle)),
                highestCollision
            };
        }

        //Not working like it should
        private static PredictionOutput GetAoeArcPrediction(Obj_AI_Base unit, float width, float range, float delay,
            float speed,
            bool collision, Vector3 from, Vector3 rangeCheckFrom, float accel)
        {
            var result = GetBestPosition(unit, delay, width, speed, from, range, collision,
                SkillshotType.SkillshotLine, rangeCheckFrom);
            var points = new List<Vector2>();

            points.Add(result.Position.To2D());

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (enemy.IsEnemy && enemy.NetworkId != unit.NetworkId && enemy.IsValidTarget() && !enemy.IsDead &&
                    enemy.IsValid &&
                    Vector3.Distance(from, enemy.ServerPosition) <= range)
                {
                    var pred = GetBestPosition(enemy, delay, width, speed, from, range, collision,
                        SkillshotType.SkillshotLine, rangeCheckFrom);

                    if (pred.HitChance >= HitChance.CantHit)
                    {
                        points.Add(pred.Position.To2D());
                    }
                }
            }
            var maxHitPoints = new List<Vector2>();

            if (points.Count > 1)
            {
                var obj = CrescentCollision(points, @from.To2D(), range, accel);
                result.CastPosition = new Vector2((float)obj[0], (float)obj[1]).To3D();
                result.TargetsHit = (int)obj[2];
            }

            return result;
        }

        internal struct BlinkData
        {
            public bool MC;
            public float delay;
            public float range;

            public BlinkData(float range, float delay, bool MC)
            {
                this.range = range;
                this.delay = delay;
                this.MC = MC;
            }
        }

        internal class Dash
        {
            public Vector2 EndPos;
            public bool IsBlink;
            public float Speed;
            public float endT;
            public bool processed;

            public Dash(float endT, float Speed, bool IsBlink, Vector2 EndPos)
            {
                this.endT = endT;
                this.Speed = Speed;
                this.IsBlink = IsBlink;
                this.EndPos = EndPos;

                processed = true;
            }
        }

        internal struct PredictionInternalOutput
        {
            public Vector3 CastPosition;
            public Vector3 Position;
            public bool Valid;

            public PredictionInternalOutput(Vector2 cp, Vector2 pos, bool Valid)
            {
                CastPosition = cp.To3D();
                Position = pos.To3D();
                this.Valid = Valid;
            }
        }

        public struct PredictionOutput
        {
            public Vector3 CastPosition;
            public List<Obj_AI_Base> CollisionUnitsList;
            public HitChance HitChance;
            public Vector3 Position;

            public int TargetsHit;

            public PredictionOutput(Vector2 cp, Vector2 pos, HitChance HitChance)
            {
                CastPosition = cp.To3D();
                Position = pos.To3D();
                this.HitChance = HitChance;
                CollisionUnitsList = new List<Obj_AI_Base>();
                TargetsHit = 1;
            }
        }
    }
}

internal class Utils
{
    public static List<Vector2> CutPath(List<Vector2> path, float Distance, float Width)
    {
        var ElongedPath = new List<Vector2>();
        var result = new List<Vector2>();
        var Direction = path[1] - path[0];
        Direction.Normalize();

        ElongedPath.AddRange(path);

        Direction = path[path.Count - 1] - path[path.Count - 2];
        Direction.Normalize();

        ElongedPath.Add(path[path.Count - 1] + Direction * Width);

        for (var i = 0; i < ElongedPath.Count - 1; i++)
        {
            var Dist = Vector2.Distance(ElongedPath[i], ElongedPath[i + 1]);

            if (Dist > Distance)
            {
                Direction = ElongedPath[i + 1] - ElongedPath[i];
                Direction.Normalize();

                var FirstPoint = ElongedPath[i] + Direction * Distance;
                result.Add(FirstPoint);

                for (var j = i + 1; j < ElongedPath.Count; j++)
                    result.Add(ElongedPath[j]);

                return result;
            }
            Distance -= Dist;
        }

        return ElongedPath;
    }
}