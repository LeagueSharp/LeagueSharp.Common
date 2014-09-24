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
using System.Linq;
using System.Reflection;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    /// Game functions related utilities.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Returns if the target is valid (not dead, targetable, visible...).
        /// </summary>
        public static bool IsValidTarget(this Obj_AI_Base unit,
            float range = float.MaxValue,
            bool checkTeam = true,
            Vector3 from = new Vector3())
        {
            if (unit == null || !unit.IsValid || unit.IsDead || !unit.IsVisible || !unit.IsTargetable ||
                unit.IsInvulnerable)
            {
                return false;
            }

            if (checkTeam && unit.Team == ObjectManager.Player.Team)
            {
                return false;
            }

            if (range < float.MaxValue &&
                Vector2.DistanceSquared(
                    (from.To2D().IsValid() ? from : ObjectManager.Player.ServerPosition).To2D(),
                    unit.ServerPosition.To2D()) > range * range)
            {
                return false;
            }

            return true;
        }

        public static void HighlightUnit(Obj_AI_Base unit, bool showHighlight = true)
        {
            if (showHighlight)
            {
                Packet.S2C.HighlightUnit.Encoded(unit.NetworkId).Process();
            }
            else
            {
                Packet.S2C.RemoveHighlightUnit.Encoded(unit.NetworkId).Process();
            }
        }

        public static void DebugMessage(string debugMessage)
        {
            Packet.S2C.DebugMessage.Encoded(debugMessage).Process();
        }

        public static void PrintFloatText(GameObject obj, string text, Packet.FloatTextPacket type)
        {
            Packet.S2C.FloatText.Encoded(new Packet.S2C.FloatText.Struct(text, type, obj.NetworkId)).Process();
        }

        public static string GetAssemblyLocation()
        {
            var fileLoc = Assembly.GetExecutingAssembly().Location;
            return fileLoc.Remove(fileLoc.LastIndexOf("\\System\\", StringComparison.Ordinal));
        }

        public static string FormatTime(float time)
        {
            var t = TimeSpan.FromSeconds(time);
            return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        }

        public static string FormatTime(double time)
        {
            var t = TimeSpan.FromSeconds(time);
            return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        }

        /// <summary>
        /// Searches in the haystack array for the given needle using the default equality operator and returns the index at which the needle starts.
        /// </summary>
        /// <typeparam name="T">Type of the arrays.</typeparam>
        /// <param name="haystack">Sequence to operate on.</param>
        /// <param name="needle">Sequence to search for.</param>
        /// <returns>Index of the needle within the haystack or -1 if the needle isn't contained.</returns>
        public static IEnumerable<int> IndexOf<T>(this T[] haystack, T[] needle)
        {
            if ((needle == null) || (haystack.Length < needle.Length))
            {
                yield break;
            }

            for (var l = 0; l < haystack.Length - needle.Length + 1; l++)
            {
                if (!needle.Where((data, index) => !haystack[l + index].Equals(data)).Any())
                {
                    yield return l;
                }
            }
        }

        public static bool IsWall(Vector3 position)
        {
            var cFlags = NavMesh.GetCollisionFlags(position);
            return (cFlags == CollisionFlags.Wall || cFlags == CollisionFlags.Building || cFlags == CollisionFlags.Prop);
        }

        public static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof (char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof (char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static int GetRecallTime(Obj_AI_Hero obj)
        {
            var duration = 0;

            switch (obj.Spellbook.GetSpell(SpellSlot.Recall).Name)
            {
                case "Recall":
                    duration = 8000;
                    break;
                case "RecallImproved":
                    duration = 7000;
                    break;
                case "OdinRecall":
                    duration = 4500;
                    break;
                case "OdinRecallImproved":
                    duration = 4000;
                    break;
            }
            return duration;
        }

        public static int GetRecallTime(string recallName)
        {
            var duration = 0;

            switch (recallName)
            {
                case "Recall":
                    duration = 8000;
                    break;
                case "RecallImproved":
                    duration = 7000;
                    break;
                case "OdinRecall":
                    duration = 4500;
                    break;
                case "OdinRecallImproved":
                    duration = 4000;
                    break;
            }
            return duration;
        }

        public static bool LevelUpSpell(this Spellbook book, SpellSlot slot)
        {
            Packet.C2S.LevelUpSpell.Encoded(new Packet.C2S.LevelUpSpell.Struct(ObjectManager.Player.NetworkId, slot))
                .Send();
            return true;
        }

        public static List<Vector2> CutPath(this List<Vector2> path, float distance)
        {
            var result = new List<Vector2>();
            var Distance = distance;
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
        /// Returns the path of the unit appending the ServerPosition at the start, works even if the unit just entered fow.
        /// </summary>
        public static List<Vector2> GetWaypoints(this Obj_AI_Base unit)
        {
            var result = new List<Vector2>();

            if (unit.IsVisible)
            {
                result.Add(unit.ServerPosition.To2D());
                result.AddRange(unit.Path.Select(point => point.To2D()));
            }
            else if (WaypointTracker.StoredPaths.ContainsKey(unit.NetworkId))
            {
                var path = WaypointTracker.StoredPaths[unit.NetworkId];
                var timePassed = (Environment.TickCount - WaypointTracker.StoredTick[unit.NetworkId]) / 1000f;
                if (path.PathLength() >= unit.MoveSpeed * timePassed)
                {
                    result = CutPath(path, (int) (unit.MoveSpeed * timePassed));
                }
            }

            return result;
        }

        /// <summary>
        /// Returns if the unit has the buff and it is active
        /// </summary>
        public static bool HasBuff(this Obj_AI_Base unit, string buffName, bool dontUseDisplayName = false)
        {
            return
                unit.Buffs.Any(
                    buff =>
                        ((!dontUseDisplayName && buff.DisplayName == buffName) ||
                         (dontUseDisplayName && buff.Name == buffName)) && buff.IsActive &&
                        buff.EndTime - Game.Time >= 0);
        }

        /// <summary>
        /// Returns the spell slot with the name.
        /// </summary>
        public static SpellSlot GetSpellSlot(this Obj_AI_Hero unit, string name, bool searchInSummoners = true)
        {
            name = name.ToLower();
            foreach (var spell in unit.Spellbook.Spells.Where(spell => spell.Name.ToLower() == name))
            {
                return spell.Slot;
            }

            if (searchInSummoners)
            {
                foreach (var spell in unit.SummonerSpellbook.Spells.Where(spell => spell.Name.ToLower() == name))
                {
                    return spell.Slot;
                }
            }

            return SpellSlot.Unknown;
        }

        /// <summary>
        /// Returns true if Player is under tower range.
        /// </summary>
        public static bool UnderTurret()
        {
            return UnderTurret(ObjectManager.Player, true);
        }

        /// <summary>
        /// Returns true if the unit is under tower range.
        /// </summary>
        public static bool UnderTurret(Obj_AI_Base unit)
        {
            return UnderTurret(unit, true);
        }

        /// <summary>
        /// Returns true if the unit is under turret range.
        /// </summary>
        public static bool UnderTurret(Obj_AI_Base unit, bool enemyTurretsOnly)
        {
            foreach (var turret in ObjectManager.Get<Obj_AI_Turret>())
            {
                if (enemyTurretsOnly)
                {
                    if (turret != null && turret.IsValid && turret.IsEnemy && turret.Health > 0)
                    {
                        if (Vector2.Distance(unit.Position.To2D(), turret.Position.To2D()) < 950)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (turret != null && turret.IsValid && turret.Health > 0)
                    {
                        if (Vector2.Distance(unit.Position.To2D(), turret.Position.To2D()) < 950)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Counts the enemies in range of Player.
        /// </summary>
        public static int CountEnemysInRange(int range)
        {
            return CountEnemysInRange(range, ObjectManager.Player);
        }

        /// <summary>
        /// Counts the enemies in range of Unit.
        /// </summary>
        public static int CountEnemysInRange(int range, Obj_AI_Base unit)
        {
            return
                ObjectManager.Get<Obj_AI_Hero>()
                    .Where(units => units.IsValidTarget())
                    .Count(units => Vector2.Distance(unit.Position.To2D(), units.Position.To2D()) <= range);
        }

        /// <summary>
        /// Returns true if Player is in shop range.
        /// </summary>
        /// <returns></returns>
        public static bool InShopRange()
        {
            return
                ObjectManager.Get<Obj_Shop>()
                    .Where(shop => shop.IsAlly)
                    .Any(shop => Vector2.Distance(ObjectManager.Player.Position.To2D(), shop.Position.To2D()) < 1250);
        }

        /// <summary>
        /// Draws a "lag-free" circle
        /// </summary>
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

        public static bool InFountain()
        {
            float fountainRange = 750;
            var map = Map.GetMap();
            if (map != null && map._MapType == Map.MapType.SummonersRift)
            {
                fountainRange = 1050;
            }
            return
                ObjectManager.Get<GameObject>()
                    .Where(spawnPoint => spawnPoint is Obj_SpawnPoint && spawnPoint.IsAlly)
                    .Any(
                        spawnPoint =>
                            Vector2.Distance(ObjectManager.Player.Position.To2D(), spawnPoint.Position.To2D()) <
                            fountainRange);
        }

        public static class DelayAction
        {
            public delegate void Callback();

            public static List<Action> ActionList = new List<Action>();

            static DelayAction()
            {
                Game.OnGameUpdate += GameOnOnGameUpdate;
            }

            private static void GameOnOnGameUpdate(EventArgs args)
            {
                for (var i = ActionList.Count - 1; i >= 0; i--)
                {
                    if (ActionList[i].Time <= Environment.TickCount)
                    {
                        ActionList[i].CallbackObject();
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
                    Time = time + Environment.TickCount;
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
                0, 0, "", 11, new ColorBGRA(255, 0, 0, 255), "monospace");

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

                foreach (
                    var unit in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.IsValid && h.IsHPBarRendered && h.IsEnemy)
                    )
                {
                    var barPos = unit.HPBarPosition;
                    var damage = _damageToUnit(unit);
                    var percentHealthAfterDamage = Math.Max(0, unit.Health - damage) / unit.MaxHealth;
                    var xPos = barPos.X + XOffset + Width * percentHealthAfterDamage;

                    if (damage > unit.Health)
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

            public Vector2 Grid;

            public string Name;
            public string ShortName;
            public MapType _MapType;

            public Map(string name, string shortName, MapType map, Vector2 grid)
            {
                Name = name;
                ShortName = shortName;
                _MapType = map;
                Grid = grid;
            }

            private static bool SameVector(Vector3 v1, Vector3 v2)
            {
                return (Math.Abs(Math.Floor(v1.X) - Math.Floor(v2.X)) < float.Epsilon &&
                        Math.Abs(Math.Floor(v1.Y) - Math.Floor(v2.Y)) < float.Epsilon &&
                        Math.Abs(Math.Floor(v1.Z) - Math.Floor(v2.Z)) < float.Epsilon);
            }

            /// <summary>
            /// Returns the current map.
            /// </summary>
            public static Map GetMap()
            {
                Vector3[] sr =
                {
                    new Vector3(13360.61f, 14586.56f, 218.222f),
                    new Vector3(-174.2087f, 1056.653f, 163.7132f)
                };
                Vector3[] dom =
                {
                    new Vector3(16.54065f, 4452.441f, 168.618f),
                    new Vector3(13876.07f, 4445.496f, 99.3553f)
                };
                Vector3[] ttt =
                {
                    new Vector3(14125.37f, 8005.887f, 123.4631f),
                    new Vector3(1313.361f, 8005.887f, 123.4631f)
                };
                Vector3[] ha =
                {
                    new Vector3(497.0624f, 1932.652f, -39.8721f),
                    new Vector3(11065.5f, 12306.48f, -185.1475f)
                };

                if (
                    sr.Any(
                        pos =>
                            ObjectManager.Get<Obj_Shop>().ToList().Find(shop => SameVector(shop.Position, pos)) != null))
                {
                    return new Map(
                        "Summoner's Rift", "summonerRift", MapType.SummonersRift, new Vector2(13982 / 2, 14446 / 2));
                }
                if (
                    dom.Any(
                        pos =>
                            ObjectManager.Get<Obj_Shop>().ToList().Find(shop => SameVector(shop.Position, pos)) != null))
                {
                    return new Map(
                        "The Crystal Scar", "crystalScar", MapType.CrystalScar, new Vector2(13894 / 2, 13218 / 2));
                }
                if (
                    ttt.Any(
                        pos =>
                            ObjectManager.Get<Obj_Shop>().ToList().Find(shop => SameVector(shop.Position, pos)) != null))
                {
                    return new Map(
                        "The Twisted Treeline", "twistedTreeline", MapType.TwistedTreeline,
                        new Vector2(15436 / 2, 14474 / 2));
                }
                if (
                    ha.Any(
                        pos =>
                            ObjectManager.Get<Obj_Shop>().ToList().Find(shop => SameVector(shop.Position, pos)) != null))
                {
                    return new Map(
                        "Howling Abyss", "howlingAbyss", MapType.HowlingAbyss, new Vector2(13120 / 2, 12618 / 2));
                }

                return new Map("Unknown", "unknown", MapType.Unknown, new Vector2(0, 0));
            }
        }


        /// <summary>
        /// Internal class used to get the waypoints even when the enemy enters the fow of war.
        /// </summary>
        internal static class WaypointTracker
        {
            public static readonly Dictionary<int, List<Vector2>> StoredPaths = new Dictionary<int, List<Vector2>>();
            public static readonly Dictionary<int, int> StoredTick = new Dictionary<int, int>();

            static WaypointTracker()
            {
                Game.OnGameProcessPacket += Game_OnGameProcessPacket;
            }

            private static void Game_OnGameProcessPacket(GamePacketEventArgs args)
            {
                if (args.PacketData[0] == Packet.S2C.LoseVision.Header)
                {
                    var decodedPacket = Packet.S2C.LoseVision.Decoded(args.PacketData);
                    var networkId = decodedPacket.UnitNetworkId;
                    var unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(networkId);
                    if (unit != null && unit.IsValid && unit.IsVisible)
                    {
                        if (!StoredPaths.ContainsKey(networkId))
                        {
                            StoredPaths.Add(networkId, GetWaypoints(unit));
                            StoredTick.Add(networkId, Environment.TickCount);
                        }
                        else
                        {
                            StoredPaths[networkId] = GetWaypoints(unit);
                            StoredTick[networkId] = Environment.TickCount;
                        }
                    }
                }
            }
        }
    }
}