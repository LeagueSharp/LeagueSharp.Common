#region LICENSE

/*
 Copyright 2014 - 2015 LeagueSharp
 Utility.cs is part of LeagueSharp.Common.
 
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
using LeagueSharp.Common.Data;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    ///     Game functions related utilities.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        ///     Returns if the source is facing the target.
        /// </summary>
        public static bool IsFacing(this Obj_AI_Base source, Obj_AI_Base target)
        {
            if (source == null || target == null)
            {
                return false;
            }

            const float angle = 90;
            return source.Direction.To2D().Perpendicular().AngleBetween((target.Position - source.Position).To2D()) < angle;
        }

        /// <summary>
        ///     Returns if both source and target are Facing Themselves.
        /// </summary>
        public static bool IsBothFacing(Obj_AI_Base source, Obj_AI_Base target)
        {
            return source.IsFacing(target) && target.IsFacing(source);
        }

        /// <summary>
        ///     Returns if the target is valid (not dead, targetable, visible...).
        /// </summary>
        public static bool IsValidTarget(this AttackableUnit unit,
            float range = float.MaxValue,
            bool checkTeam = true,
            Vector3 from = new Vector3())
        {
            if (unit == null || !unit.IsValid || unit.IsDead || !unit.IsVisible || !unit.IsTargetable ||
                unit.IsInvulnerable)
            {
                return false;
            }

            var @base = unit as Obj_AI_Base;
            if (@base != null)
            {
                if (@base.HasBuff("kindredrnodeathbuff") && @base.HealthPercent <= 10)
                {
                    return false;
                }
            }

            if (checkTeam && unit.Team == ObjectManager.Player.Team)
            {
                return false;
            }

            var unitPosition = @base != null ? @base.ServerPosition : unit.Position;

            return !(range < float.MaxValue) ||
                   !(Vector2.DistanceSquared(
                       (@from.To2D().IsValid() ? @from : ObjectManager.Player.ServerPosition).To2D(),
                       unitPosition.To2D()) > range * range);
        }

        public static SpellDataInst GetSpell(this Obj_AI_Hero hero, SpellSlot slot)
        {
            return hero.Spellbook.GetSpell(slot);
        }

        /// <summary>
        ///     Returns if the unit's movement is impaired (Slows, Taunts, Charms, Taunts, Snares, Fear)
        /// </summary>
        public static bool IsMovementImpaired(this Obj_AI_Hero hero)
        {
            return hero.HasBuffOfType(BuffType.Flee) || hero.HasBuffOfType(BuffType.Charm) || hero.HasBuffOfType(BuffType.Slow)
                   || hero.HasBuffOfType(BuffType.Snare) || hero.HasBuffOfType(BuffType.Stun)
                   || hero.HasBuffOfType(BuffType.Taunt);
        }

        /// <summary>
        ///     Returns if the spell is ready to use.
        /// </summary>
        public static bool IsReady(this SpellDataInst spell, int t = 0)
        {
            return spell != null && spell.Slot != SpellSlot.Unknown && t == 0
                       ? spell.State == SpellState.Ready
                       : (spell.State == SpellState.Ready
                          || (spell.State == SpellState.Cooldown && (spell.CooldownExpires - Game.Time) <= t / 1000f));
        }

        /// <summary>
        ///     Returns if the spell is ready to use.
        /// </summary>
        public static bool IsReady(this Spell spell, int t = 0)
        {
            return IsReady(spell.Instance, t);
        }

        /// <summary>
        ///     Returns if the spell is ready to use.
        /// </summary>
        public static bool IsReady(this SpellSlot slot, int t = 0)
        {
            var s = ObjectManager.Player.Spellbook.GetSpell(slot);
            return s != null && IsReady(s, t);
        }

        /// <summary>
        ///     Returns if the GameObject is valid
        /// </summary>
        public static bool IsValid<T>(this GameObject obj) where T : GameObject
        {
            return obj as T != null && obj.IsValid;
        }

        /// <summary>
        ///     Returns if the SpellSlot of the InventorySlot is valid
        /// </summary>
        public static bool IsValidSlot(this InventorySlot slot)
        {
            return slot != null && slot.SpellSlot != SpellSlot.Unknown;
        }

        /// <summary>
        /// Returns the unit's ability power
        /// </summary>
        /// 
        public static float AbilityPower(this Obj_AI_Base @base)
        {
            return @base.FlatMagicDamageMod + (@base.PercentMagicDamageMod * @base.FlatMagicDamageMod);
        }

        /// <summary>
        ///     Returns the unit's health percentage (From 0 to 100).
        /// </summary>
        [Obsolete("Use HealthPercent attribute.", false)]
        public static float HealthPercentage(this Obj_AI_Base unit)
        {
            return unit.HealthPercent;
        }

        /// <summary>
        ///     Returns the unit's mana percentage (From 0 to 100).
        /// </summary>
        [Obsolete("Use ManaPercent attribute.", false)]
        public static float ManaPercentage(this Obj_AI_Base unit)
        {
            return unit.ManaPercent;
        }

        [Obsolete("Use TotalMagicalDamage from Leaguesharp.Core.", false)]
        public static float TotalMagicalDamage(this Obj_AI_Hero target)
        {
            return target.TotalMagicalDamage;
        }

        [Obsolete("Use TotalAttackDamage attribute from LeagueSharp.Core", false)]
        public static float TotalAttackDamage(this Obj_AI_Hero target)
        {
            return target.TotalAttackDamage;
        }

        /// <summary>
        ///     Checks if the unit is a Hero or Champion
        /// </summary>
        public static bool IsChampion(this Obj_AI_Base unit)
        {
            var hero = unit as Obj_AI_Hero;
            return hero != null && hero.IsValid;
        }

        public static bool IsChampion(this Obj_AI_Base unit, string championName)
        {
            var hero = unit as Obj_AI_Hero;
            return hero != null && hero.IsValid && hero.ChampionName.Equals(championName);
        }

        public static bool IsRecalling(this Obj_AI_Hero unit)
        {
            return unit.Buffs.Any(buff => buff.Name.ToLower().Contains("recall") && buff.Type == BuffType.Aura);
        }

        public static bool IsOnScreen(this Vector3 position)
        {
            var pos = Drawing.WorldToScreen(position);
            return pos.X > 0 && pos.X <= Drawing.Width && pos.Y > 0 && pos.Y <= Drawing.Height;
        }

        public static bool IsOnScreen(this Vector2 position)
        {
            return position.To3D().IsOnScreen();
        }

        public static Vector3 Randomize(this Vector3 position, int min, int max)
        {
            var ran = new Random(Utils.TickCount);
            return position + new Vector2(ran.Next(min, max), ran.Next(min, max)).To3D();
        }

        public static Vector2 Randomize(this Vector2 position, int min, int max)
        {
            return position.To3D().Randomize(min, max).To2D();
        }

        public static bool IsAutoAttack(this SpellData spellData)
        {
            return Orbwalking.IsAutoAttack(spellData.Name);
        }

        public static bool IsAutoAttack(this SpellDataInst spellData)
        {
            return Orbwalking.IsAutoAttack(spellData.Name);
        }

        public static bool IsWall(this Vector3 position)
        {
            return NavMesh.GetCollisionFlags(position).HasFlag(CollisionFlags.Wall);
        }

        public static bool IsWall(this Vector2 position)
        {
            return position.To3D().IsWall();
        }

        public static bool IsCasted(this Spell.CastStates state)
        {
            return state == Spell.CastStates.SuccessfullyCasted;
        }

        public static int GetRecallTime(Obj_AI_Hero obj)
        {
            return GetRecallTime(obj.Spellbook.GetSpell(SpellSlot.Recall).Name);
        }

        public static int GetRecallTime(string recallName)
        {
            var duration = 0;

            switch (recallName.ToLower())
            {
                case "recall":
                    duration = 8000;
                    break;
                case "recallimproved":
                    duration = 7000;
                    break;
                case "odinrecall":
                    duration = 4500;
                    break;
                case "odinrecallimproved":
                    duration = 4000;
                    break;
                case "superrecall":
                    duration = 4000;
                    break;
                case "superrecallimproved":
                    duration = 4000;
                    break;
            }
            return duration;
        }

        public static void LevelUpSpell(this Spellbook book, SpellSlot slot, bool evolve = false)
        {
            book.LevelSpell(slot);
        }

        public static List<Vector2> CutPath(this List<Vector2> path, float distance)
        {
            var result = new List<Vector2>();
            var Distance = distance;
            if (distance < 0)
            {
                path[0] = path[0] + distance * (path[1] - path[0]).Normalized();
                return path;
            }
            
            for (var i = 0; i < path.Count - 1; i++)
            {
                var dist = path[i].Distance(path[i + 1]);
                if (dist > Distance)
                {
                    result.Add(path[i] + Distance * (path[i + 1] - path[i]).Normalized());
                    for (var j = i + 1; j < path.Count; j++)
                    {
                        result.Add(path[j]);
                    }

                    break;
                }
                Distance -= dist;
            }
            return result.Count > 0 ? result : new List<Vector2> { path.Last() };
        }

        /// <summary>
        ///     Returns the path of the unit appending the ServerPosition at the start, works even if the unit just entered fow.
        /// </summary>
        public static List<Vector2> GetWaypoints(this Obj_AI_Base unit)
        {
            var result = new List<Vector2>();

            if (unit.IsVisible)
            {
                result.Add(unit.ServerPosition.To2D());
                var path = unit.Path;
                if (path.Length > 0)
                {
                    var first = path[0].To2D();
                    if (first.Distance(result[0], true) > 40)
                    {
                        result.Add(first);    
                    }
                    
                    for (int i = 1; i < path.Length; i++)
                    {
                        result.Add(path[i].To2D());
                    }    
                }
            }
            else if (WaypointTracker.StoredPaths.ContainsKey(unit.NetworkId))
            {
                var path = WaypointTracker.StoredPaths[unit.NetworkId];
                var timePassed = (Utils.TickCount - WaypointTracker.StoredTick[unit.NetworkId]) / 1000f;
                if (path.PathLength() >= unit.MoveSpeed * timePassed)
                {
                    result = CutPath(path, (int) (unit.MoveSpeed * timePassed));
                }
            }

            return result;
        }

        public static List<Vector2Time> GetWaypointsWithTime(this Obj_AI_Base unit)
        {
            var wp = unit.GetWaypoints();

            if (wp.Count < 1)
            {
                return null;
            }

            var result = new List<Vector2Time>();
            var speed = unit.MoveSpeed;
            var lastPoint = wp[0];
            var time = 0f;

            foreach (var point in wp)
            {
                time += point.Distance(lastPoint) / speed;
                result.Add(new Vector2Time(point, time));
                lastPoint = point;
            }

            return result;
        }

        /// <summary>
        ///     Returns true if the buff is active and didn't expire.
        /// </summary>
        public static bool IsValidBuff(this BuffInstance buff)
        {
            return buff.IsActive && buff.EndTime - Game.Time > 0;
        }

        /// <summary>
        ///     Returns if the unit has the specified buff in the indicated amount of time
        /// </summary>
        public static bool HasBuffIn(this Obj_AI_Base unit, string displayName, float tickCount, bool includePing = true)
        { 
            return
                unit.Buffs.Any(
                    buff =>
                        buff.IsValid && buff.DisplayName == displayName &&
                        buff.EndTime - Game.Time > tickCount - (includePing ? (Game.Ping/2000f) : 0));
        }
 
        /// <summary>
        ///     Returns if the unit has the buff and it is active
        /// </summary>
        [Obsolete("Use Obj_AI_Base.HasBuff")]
        public static bool HasBuff(this Obj_AI_Base unit,
            string buffName,
            bool dontUseDisplayName = false,
            bool kappa = true)
        {
            return
                unit.Buffs.Any(
                    buff =>
                        ((dontUseDisplayName &&
                          String.Equals(buff.Name, buffName, StringComparison.CurrentCultureIgnoreCase)) ||
                         (!dontUseDisplayName &&
                          String.Equals(buff.DisplayName, buffName, StringComparison.CurrentCultureIgnoreCase))) &&
                        buff.IsValidBuff());
        }

        /// <summary>
        ///     Returns the spell slot with the name.
        /// </summary>
        public static SpellSlot GetSpellSlot(this Obj_AI_Hero unit, string name)
        {
            foreach (var spell in
                unit.Spellbook.Spells.Where(
                    spell => String.Equals(spell.Name, name, StringComparison.CurrentCultureIgnoreCase)))
            {
                return spell.Slot;
            }

            return SpellSlot.Unknown;
        }

        /// <summary>
        ///     Returns true if the unit is under tower range.
        /// </summary>
        public static bool UnderTurret(this Obj_AI_Base unit)
        {
            return UnderTurret(unit.Position, true);
        }

        /// <summary>
        ///     Returns true if the unit is under turret range.
        /// </summary>
        public static bool UnderTurret(this Obj_AI_Base unit, bool enemyTurretsOnly)
        {
            return UnderTurret(unit.Position, enemyTurretsOnly);
        }

        public static bool UnderTurret(this Vector3 position, bool enemyTurretsOnly)
        {
            return
                ObjectManager.Get<Obj_AI_Turret>().Any(turret => turret.IsValidTarget(950, enemyTurretsOnly, position));
        }
        /// <summary>
        ///  Return true if unit is under ally turret range.
        /// <returns></returns>
        public static bool UnderAllyTurret(this Obj_AI_Base unit)
        {
            return UnderAllyTurret(unit.Position);
        }
        public static bool UnderAllyTurret(this Vector3 position)
        {
            return
                ObjectManager.Get<Obj_AI_Turret>().Any(turret => turret.IsValidTarget(950, false, position) && turret.IsAlly);
        }
        public static NavMeshCell ToNavMeshCell(this Vector3 position)
        {
            var nav = NavMesh.WorldToGrid(position.X, position.Y);
            return NavMesh.GetCell((short) nav.X, (short) nav.Y);
        }

        [Obsolete("Use CountEnemiesInRange", false)]
        public static int CountEnemysInRange(this Obj_AI_Base unit, float range)
        {
            return unit.ServerPosition.CountEnemiesInRange(range);
        }

        [Obsolete("Use CountEnemiesInRange", false)]
        public static int CountEnemysInRange(this Vector3 point, float range)
        {
            return point.CountEnemiesInRange(range);
        }

        /// <summary>
        ///     Counts the enemies in range of Player.
        /// </summary>
        public static int CountEnemiesInRange(float range)
        {
            return ObjectManager.Player.CountEnemiesInRange(range);
        }

        /// <summary>
        ///     Counts the enemies in range of Unit.
        /// </summary>
        public static int CountEnemiesInRange(this Obj_AI_Base unit, float range)
        {
            return unit.ServerPosition.CountEnemiesInRange(range);
        }

        /// <summary>
        ///     Counts the enemies in range of point.
        /// </summary>
        public static int CountEnemiesInRange(this Vector3 point, float range)
        {
            return HeroManager.Enemies.Count(h => h.IsValidTarget(range, true, point));
        }

        // Use same interface as CountEnemiesInRange
        /// <summary>
        ///     Count the allies in range of the Player.
        /// </summary>
        public static int CountAlliesInRange(float range)
        {
            return ObjectManager.Player.CountAlliesInRange(range);
        }

        /// <summary>
        ///     Counts the allies in range of the Unit.
        /// </summary>
        public static int CountAlliesInRange(this Obj_AI_Base unit, float range)
        {
            return unit.ServerPosition.CountAlliesInRange(range, unit);
        }

        /// <summary>
        ///     Counts the allies in the range of the Point. 
        /// </summary>
        public static int CountAlliesInRange(this Vector3 point, float range, Obj_AI_Base originalunit = null)
        {
            if (originalunit != null)
            {
                return HeroManager.Allies
                    .Count(x => x.NetworkId != originalunit.NetworkId && x.IsValidTarget(range, false, point));
            }
                return HeroManager.Allies
                 .Count(x => x.IsValidTarget(range, false, point));
        }

        public static List<Obj_AI_Hero> GetAlliesInRange(this Obj_AI_Base unit, float range)
        {
            return GetAlliesInRange(unit.ServerPosition, range, unit);
        }

        public static List<Obj_AI_Hero> GetAlliesInRange(this Vector3 point, float range, Obj_AI_Base originalunit = null)
        {
            if (originalunit != null)
            {
                return
                    HeroManager.Allies
                        .FindAll(x => x.NetworkId != originalunit.NetworkId && point.Distance(x.ServerPosition, true) <= range*range);
            }
            return
                   HeroManager.Allies
                       .FindAll(x => point.Distance(x.ServerPosition, true) <= range * range);
        }

        public static List<Obj_AI_Hero> GetEnemiesInRange(this Obj_AI_Base unit, float range)
        {
            return GetEnemiesInRange(unit.ServerPosition, range);
        }

        public static List<Obj_AI_Hero> GetEnemiesInRange(this Vector3 point, float range)
        {
            return
                HeroManager.Enemies
                    .FindAll(x => point.Distance(x.ServerPosition, true) <= range * range);
        }

        public static List<T> GetObjects<T>(this Vector3 position, float range) where T : GameObject, new()
        {
            return ObjectManager.Get<T>().Where(x => position.Distance(x.Position, true) < range * range).ToList();
        }

        public static List<T> GetObjects<T>(string objectName, float range, Vector3 rangeCheckFrom = new Vector3())
            where T : GameObject, new()
        {
            if (rangeCheckFrom.Equals(Vector3.Zero))
            {
                rangeCheckFrom = ObjectManager.Player.ServerPosition;
            }

            return ObjectManager.Get<T>().Where(x => rangeCheckFrom.Distance(x.Position, true) < range * range).ToList();
        }


        /// <summary>
        ///     Returns true if hero is in shop range.
        /// </summary>
        /// <returns></returns>
        public static bool InShop(this Obj_AI_Hero hero)
        {
            return hero.IsVisible &&
                   ObjectManager.Get<Obj_Shop>()
                       .Any(s => s.Team == hero.Team && hero.Distance(s.Position, true) < 1562500); // 1250²
        }

        /// <summary>
        ///     Draws a "lag-free" circle
        /// </summary>
        [Obsolete("Use Render.Circle", false)]
        public static void DrawCircle(Vector3 center,
            float radius,
            Color color,
            int thickness = 5,
            int quality = 30,
            bool onMinimap = false)
        {
            if (!onMinimap)
            {
                Render.Circle.DrawCircle(center, radius, color, thickness);
                return;
            }

            var pointList = new List<Vector3>();
            for (var i = 0; i < quality; i++)
            {
                var angle = i * Math.PI * 2 / quality;
                pointList.Add(
                    new Vector3(
                        center.X + radius * (float) Math.Cos(angle), center.Y + radius * (float) Math.Sin(angle),
                        center.Z));
            }

            for (var i = 0; i < pointList.Count; i++)
            {
                var a = pointList[i];
                var b = pointList[i == pointList.Count - 1 ? 0 : i + 1];

                var aonScreen = Drawing.WorldToMinimap(a);
                var bonScreen = Drawing.WorldToMinimap(b);

                Drawing.DrawLine(aonScreen.X, aonScreen.Y, bonScreen.X, bonScreen.Y, thickness, color);
            }
        }

        public static bool InFountain(this Obj_AI_Hero hero)
        {
            float fountainRange = 562500; //750 * 750
            var map = Map.GetMap();
            if (map != null && map.Type == Map.MapType.SummonersRift)
            {
                fountainRange = 1102500; //1050 * 1050
            }
            return hero.IsVisible &&
                   ObjectManager.Get<Obj_SpawnPoint>()
                       .Any(sp => sp.Team == hero.Team && hero.Distance(sp.Position, true) < fountainRange);
        }

        public static short GetPacketId(this GamePacketEventArgs gamePacketEventArgs)
        {
            var packetData = gamePacketEventArgs.PacketData;
            if (packetData.Length < 2)
            {
                return 0;
            }
            return (short) (packetData[0] + packetData[1] * 256);
        }

        public static void SendAsPacket(this byte[] packetData,
            PacketChannel channel = PacketChannel.C2S,
            PacketProtocolFlags protocolFlags = PacketProtocolFlags.Reliable)
        {
            Game.SendPacket(packetData, channel, protocolFlags);
        }

        public static void ProcessAsPacket(this byte[] packetData, PacketChannel channel = PacketChannel.S2C)
        {
            Game.ProcessPacket(packetData, channel);
        }

        public static class DelayAction
        {
            public delegate void Callback();

            public static List<Action> ActionList = new List<Action>();

            static DelayAction()
            {
                Game.OnUpdate += GameOnOnGameUpdate;
            }

            private static void GameOnOnGameUpdate(EventArgs args)
            {
                for (var i = ActionList.Count - 1; i >= 0; i--)
                {
                    if (ActionList[i].Time <= Utils.GameTimeTickCount)
                    {
                        try
                        {
                            if (ActionList[i].CallbackObject != null)
                            {
                                ActionList[i].CallbackObject();
                                //Will somehow result in calling ALL non-internal marked classes of the called assembly and causes NullReferenceExceptions.
                            }
                        }
                        catch (Exception)
                        {
                            // ignored
                        }

                        ActionList.RemoveAt(i);
                    }
                }
            }

            public static void Add(int time, Callback func)
            {
                var action = new Action(time, func);
                ActionList.Add(action);
            }

            public struct Action
            {
                public Callback CallbackObject;
                public int Time;

                public Action(int time, Callback callback)
                {
                    Time = time + Utils.GameTimeTickCount;
                    CallbackObject = callback;
                }
            }
        }

        public static class HpBarDamageIndicator
        {
            public delegate float DamageToUnitDelegate(Obj_AI_Hero hero);

            private const int XOffset = 10;
            private const int YOffset = 20;
            private const int Width = 103;
            private const int Height = 8;
            public static Color Color = Color.Lime;
            public static bool Enabled = true;
            private static DamageToUnitDelegate _damageToUnit;

            private static readonly Render.Text Text = new Render.Text(
                0, 0, string.Empty, 11, new ColorBGRA(255, 0, 0, 255), "monospace");

            public static DamageToUnitDelegate DamageToUnit
            {
                get { return _damageToUnit; }

                set
                {
                    if (_damageToUnit == null)
                    {
                        Drawing.OnDraw += Drawing_OnDraw;
                    }
                    _damageToUnit = value;
                }
            }

            private static void Drawing_OnDraw(EventArgs args)
            {
                if (!Enabled || _damageToUnit == null)
                {
                    return;
                }

                foreach (var unit in
                    HeroManager.Enemies.FindAll(h => h.IsValid && h.IsHPBarRendered))
                {
                    var barPos = unit.HPBarPosition;
                    var damage = _damageToUnit(unit);
                    var percentHealthAfterDamage = Math.Max(0, unit.Health - damage) / unit.MaxHealth;
                    var xPos = barPos.X + XOffset + Width * percentHealthAfterDamage;

                    //if (damage > unit.Health)
                    {
                        Text.X = (int) barPos.X + XOffset;
                        Text.Y = (int) barPos.Y + YOffset - 13;
                        Text.text = ((int) (unit.Health - damage)).ToString();
                        Text.OnEndScene();
                    }

                    Drawing.DrawLine(xPos, barPos.Y + YOffset, xPos, barPos.Y + YOffset + Height, 2, Color);
                }
            }
        }

        public class Map
        {
            public enum MapType
            {
                Unknown,
                SummonersRift,
                CrystalScar,
                TwistedTreeline,
                HowlingAbyss
            }

            private static readonly IDictionary<int, Map> MapById = new Dictionary<int, Map>
            {
                {
                    8,
                    new Map
                    {
                        Name = "The Crystal Scar",
                        ShortName = "crystalScar",
                        Type = MapType.CrystalScar,
                        Grid = new Vector2(13894f / 2, 13218f / 2),
                        StartingLevel = 3
                    }
                },
                {
                    10,
                    new Map
                    {
                        Name = "The Twisted Treeline",
                        ShortName = "twistedTreeline",
                        Type = MapType.TwistedTreeline,
                        Grid = new Vector2(15436f / 2, 14474f / 2),
                        StartingLevel = 1
                    }
                },
                {
                    11,
                    new Map
                    {
                        Name = "Summoner's Rift",
                        ShortName = "summonerRift",
                        Type = MapType.SummonersRift,
                        Grid = new Vector2(13982f / 2, 14446f / 2),
                        StartingLevel = 1
                    }
                },
                {
                    12,
                    new Map
                    {
                        Name = "Howling Abyss",
                        ShortName = "howlingAbyss",
                        Type = MapType.HowlingAbyss,
                        Grid = new Vector2(13120f / 2, 12618f / 2),
                        StartingLevel = 3
                    }
                }
            };

            public MapType Type { get; private set; }
            public Vector2 Grid { get; private set; }
            public string Name { get; private set; }
            public string ShortName { get; private set; }
            public int StartingLevel { get; private set; }

            /// <summary>
            ///     Returns the current map.
            /// </summary>
            public static Map GetMap()
            {
                if (MapById.ContainsKey((int) Game.MapId))
                {
                    return MapById[(int) Game.MapId];
                }

                return new Map
                {
                    Name = "Unknown",
                    ShortName = "unknown",
                    Type = MapType.Unknown,
                    Grid = new Vector2(0, 0),
                    StartingLevel = 1
                };
            }
        }

        /// <summary>
        ///     Internal class used to get the waypoints even when the enemy enters the fow of war.
        /// </summary>
        internal static class WaypointTracker
        {
            public static readonly Dictionary<int, List<Vector2>> StoredPaths = new Dictionary<int, List<Vector2>>();
            public static readonly Dictionary<int, int> StoredTick = new Dictionary<int, int>();
        }
    }

    public static class Version
    {
        public static int MajorVersion;
        public static int MinorVersion;
        public static int Build;
        public static int Revision;
        private static readonly int[] VersionArray;

        static Version()
        {
            var d = Game.Version.Split('.');
            MajorVersion = Convert.ToInt32(d[0]);
            MinorVersion = Convert.ToInt32(d[1]);
            Build = Convert.ToInt32(d[2]);
            Revision = Convert.ToInt32(d[3]);

            VersionArray = new[] { MajorVersion, MinorVersion, Build, Revision };
        }

        public static bool IsOlder(string version)
        {
            var d = version.Split('.');
            return MinorVersion < Convert.ToInt32(d[1]);
        }

        public static bool IsNewer(string version)
        {
            var d = version.Split('.');
            return MinorVersion > Convert.ToInt32(d[1]);
        }

        public static bool IsEqual(string version)
        {
            var d = version.Split('.');
            for (var i = 0; i <= d.Length; i++)
            {
                if (d[i] == null || Convert.ToInt32(d[i]) != VersionArray[i])
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class Vector2Time
    {
        public Vector2 Position;
        public float Time;

        public Vector2Time(Vector2 pos, float time)
        {
            Position = pos;
            Time = time;
        }
    }
}
