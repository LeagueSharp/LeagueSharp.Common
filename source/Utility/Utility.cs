// <copyright file="Utility.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SharpDX;

    using Color = System.Drawing.Color;

    /// <summary>
    ///     The utility class.
    /// </summary>
    public static partial class Utility
    {
        #region Enums

        /// <summary>
        ///     The fountain type.
        /// </summary>
        public enum FountainType
        {
            /// <summary>
            ///     Ally Fountain.
            /// </summary>
            OwnFountain,

            /// <summary>
            ///     Enemy Fountain.
            /// </summary>
            EnemyFountain
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the unit total magic damage (ablility power).
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="float" />.
        /// </returns>
        public static float AbilityPower(this Obj_AI_Base unit) => unit.TotalMagicalDamage;

        /// <summary>
        ///     Counts the allies in range of the player.
        /// </summary>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        public static int CountAlliesInRange(float range) => ObjectManager.Player.CountAlliesInRange(range);

        /// <summary>
        ///     Counts the allies in range of the unit.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        public static int CountAlliesInRange(this Obj_AI_Base unit, float range)
            => unit.ServerPosition.CountAlliesInRange(range, unit);

        /// <summary>
        ///     Counts the allies in range of the point.
        /// </summary>
        /// <param name="point">
        ///     The point.
        /// </param>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <param name="originalUnit">
        ///     The original unit.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />
        /// </returns>
        public static int CountAlliesInRange(this Vector3 point, float range, Obj_AI_Base originalUnit = null)
            =>
            ObjectManager.Get<Obj_AI_Hero>()
                .Count(
                    o =>
                        (originalUnit == null || o.NetworkId != originalUnit.NetworkId)
                        && o.IsValidTarget(range, false, point));

        /// <summary>
        ///     Counts the enemies in range of the player.
        /// </summary>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        public static int CountEnemiesInRange(float range) => ObjectManager.Player.CountEnemiesInRange(range);

        /// <summary>
        ///     Counts the enemies in range of the position.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        public static int CountEnemiesInRange(this Obj_AI_Base unit, float range)
            => unit.ServerPosition.CountEnemiesInRange(range);

        /// <summary>
        ///     Counts the enemies in range of the position.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        public static int CountEnemiesInRange(this Vector3 position, float range)
            => ObjectManager.Get<Obj_AI_Hero>().Count(c => c.IsValidTarget(range, true, position));

        /// <summary>
        ///     Cuts the path after a certain distance.
        /// </summary>
        /// <param name="path">
        ///     The path list.
        /// </param>
        /// <param name="distance">
        ///     The distance to cut after.
        /// </param>
        /// <returns>
        ///     The <see cref="List{Vector2}" />.
        /// </returns>
        public static List<Vector2> CutPath(this List<Vector2> path, float distance)
        {
            var result = new List<Vector2>();
            var d = distance;
            if (distance < 0)
            {
                path[0] = path[0] + (distance * (path[1] - path[0]).Normalized());
                return path;
            }

            for (var i = 0; i < path.Count - 1; i++)
            {
                var dist = path[i].Distance(path[i + 1]);
                if (dist > d)
                {
                    result.Add(path[i] + (d * (path[i + 1] - path[i]).Normalized()));
                    for (var j = i + 1; j < path.Count; j++)
                    {
                        result.Add(path[j]);
                    }

                    break;
                }

                d -= dist;
            }

            return result.Count > 0 ? result : new List<Vector2> { path.Last() };
        }

        /// <summary>
        ///     Draws a circle.
        /// </summary>
        /// <param name="center">
        ///     The center.
        /// </param>
        /// <param name="radius">
        ///     The radius.
        /// </param>
        /// <param name="color">
        ///     The color.
        /// </param>
        /// <param name="thickness">
        ///     The thickness.
        /// </param>
        /// <param name="quality">
        ///     The quality.
        /// </param>
        /// <param name="onMinimap">
        ///     A value indicating whether to draw the circle onto the minimap.
        /// </param>
        public static void DrawCircle(
            Vector3 center,
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
                        center.X + (radius * (float)Math.Cos(angle)),
                        center.Y + (radius * (float)Math.Sin(angle)),
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

        /// <summary>
        ///     Gets the allies in range of the unit.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <returns>
        ///     The <see cref="List{Obj_AI_Hero}" />.
        /// </returns>
        public static List<Obj_AI_Hero> GetAlliesInRange(this Obj_AI_Base unit, float range)
            => unit.ServerPosition.GetAlliesInRange(range, unit);

        /// <summary>
        ///     Gets the allies in range of the point.
        /// </summary>
        /// <param name="point">
        ///     The point.
        /// </param>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <param name="originalUnit">
        ///     The original unit.
        /// </param>
        /// <returns>
        ///     The <see cref="List{Obj_AI_Hero}" />.
        /// </returns>
        public static List<Obj_AI_Hero> GetAlliesInRange(
                this Vector3 point,
                float range,
                Obj_AI_Base originalUnit = null)
            =>
            ObjectManager.Get<Obj_AI_Hero>()
                .Where(
                    e =>
                        ((originalUnit == null && ObjectManager.Player.Team == e.Team)
                         || (originalUnit?.Team == e.Team)) && point.Distance(e.ServerPosition) <= range)
                .ToList();

        /// <summary>
        ///     Gets the enemies in range of the unit.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <returns>
        ///     The <see cref="List{Obj_AI_Hero}" />.
        /// </returns>
        public static List<Obj_AI_Hero> GetEnemiesInRange(this Obj_AI_Base unit, float range)
            => unit.ServerPosition.GetEnemiesInRange(range, unit);

        /// <summary>
        ///     Gets the enemies in range of the point.
        /// </summary>
        /// <param name="point">
        ///     The point.
        /// </param>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <param name="originalUnit">
        ///     The original unit.
        /// </param>
        /// <returns>
        ///     The <see cref="List{Obj_AI_Hero}" />.
        /// </returns>
        public static List<Obj_AI_Hero> GetEnemiesInRange(
                this Vector3 point,
                float range,
                Obj_AI_Base originalUnit = null)
            =>
            ObjectManager.Get<Obj_AI_Hero>()
                .Where(
                    e =>
                        ((originalUnit == null && ObjectManager.Player.Team != e.Team)
                         || (originalUnit?.Team != e.Team)) && point.Distance(e.ServerPosition) <= range)
                .ToList();

        /// <summary>
        ///     Gets the objects within the range of the position.
        /// </summary>
        /// <typeparam name="T">
        ///     The object type.
        /// </typeparam>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <returns>
        ///     The <see cref="List{T}" />.
        /// </returns>
        public static List<T> GetObjects<T>(this Vector3 position, float range)
            where T : GameObject, new()
            => ObjectManager.Get<T>().Where(o => position.Distance(o.Position) < range).ToList();

        /// <summary>
        ///     Gets the packet id.
        /// </summary>
        /// <param name="gamePacketEventArgs">
        ///     The game packet event args.
        /// </param>
        /// <returns>
        ///     The <see cref="short" />.
        /// </returns>
        public static short GetPacketId(this GamePacketEventArgs gamePacketEventArgs)
        {
            var packetData = gamePacketEventArgs.PacketData;
            return packetData.Length < 2 ? (short)0 : (short)(packetData[0] + (packetData[1] * 256));
        }

        /// <summary>
        ///     Gets the recall time duration.
        /// </summary>
        /// <param name="recallName">
        ///     The recall name.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
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

        /// <summary>
        ///     Gets the spell.
        /// </summary>
        /// <param name="hero">
        ///     The hero.
        /// </param>
        /// <param name="slot">
        ///     The slot.
        /// </param>
        /// <returns>
        ///     The <see cref="SpellDataInst" />.
        /// </returns>
        public static SpellDataInst GetSpell(this Obj_AI_Hero hero, SpellSlot slot) => hero.Spellbook.GetSpell(slot);

        /// <summary>
        ///     Calculates the real time spell cooldown.
        /// </summary>
        /// <param name="hero">
        ///     The hero.
        /// </param>
        /// <param name="spell">
        ///     The spell.
        /// </param>
        /// <returns>
        ///     The <see cref="float" />.
        /// </returns>
        public static float GetSpellCooldownEx(this Obj_AI_Hero hero, SpellSlot spell)
        {
            var expire = hero.Spellbook.GetSpell(spell).CooldownExpires;
            var cd = expire - (Game.Time - 1);

            return cd <= 0 ? 0 : cd;
        }

        /// <summary>
        ///     Gets the spell slot.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <param name="name">
        ///     The spell slot name.
        /// </param>
        /// <returns>
        ///     The <see cref="SpellSlot" />.
        /// </returns>
        public static SpellSlot GetSpellSlot(this Obj_AI_Hero unit, string name)
        {
            foreach (var spell in
                unit.Spellbook.Spells.Where(s => s.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)))
            {
                return spell.Slot;
            }

            return SpellSlot.Unknown;
        }

        /// <summary>
        ///     Gets the waypoints of the unit.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="List{Vector2}" />.
        /// </returns>
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

                    for (var i = 1; i < path.Length; i++)
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
                    result = CutPath(path, (int)(unit.MoveSpeed * timePassed));
                }
            }

            return result;
        }

        /// <summary>
        ///     Gets the waypoints of the unit with time.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="List{Vector2Time}" />.
        /// </returns>
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
        ///     Determines if the unit has a buff.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <param name="buffName">
        ///     The buff name.
        /// </param>
        /// <param name="arg2">
        ///     The bool argument 2, ignored.
        /// </param>
        /// <param name="arg3">
        ///     The bool argument 3, ignored.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool HasBuff(this Obj_AI_Base unit, string buffName, bool arg2 = false, bool arg3 = true)
            => unit.HasBuff(buffName);

        /// <summary>
        ///     Determines if the buff is valid in advance.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <param name="displayName">
        ///     The display name.
        /// </param>
        /// <param name="tickCount">
        ///     The tick count.
        /// </param>
        /// <param name="includePing">
        ///     A value indicating whether to include ping.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool HasBuffIn(
                this Obj_AI_Base unit,
                string displayName,
                float tickCount,
                bool includePing = true)
            =>
            unit.Buffs.Any(
                buff =>
                    buff.IsValid && buff.DisplayName == displayName
                    && buff.EndTime - Game.Time > tickCount - (includePing ? (Game.Ping / 2000f) : 0));

        /// <summary>
        ///     Gets the health percent of the unit.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="float" />.
        /// </returns>
        public static float HealthPercentage(this Obj_AI_Base unit) => unit.HealthPercent;

        /// <summary>
        ///     Determines if the position is in the fountain area.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <param name="fountainType">
        ///     The fountain type.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool InFountain(this Obj_AI_Base unit, FountainType fountainType = FountainType.OwnFountain)
        {
            float fountainRange = Map.GetMap()?.Type == Map.MapType.SummonersRift ? 1100 : 750;
            var pos = fountainType == FountainType.OwnFountain
                          ? (unit.Team == ObjectManager.Player.Team ? MiniCache.AllyFountain : MiniCache.EnemyFountain)
                          : (unit.Team == ObjectManager.Player.Team ? MiniCache.EnemyFountain : MiniCache.AllyFountain);
            return unit.IsVisible && unit.InRange(pos, fountainRange, true);
        }

        /// <summary>
        ///     Determines if the position is in the fountain area.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="fountainType">
        ///     The fountain type.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool InFountain(this Vector3 position, FountainType fountainType)
            => position.To2D().InFountain(fountainType);

        /// <summary>
        ///     Determines if the position is in the fountain area.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="fountainType">
        ///     The fountain type.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool InFountain(this Vector2 position, FountainType fountainType)
            =>
            position.InRange(
                fountainType == FountainType.OwnFountain ? MiniCache.AllyFountain : MiniCache.EnemyFountain,
                Map.GetMap()?.Type == Map.MapType.SummonersRift ? 1100 : 750,
                true);

        /// <summary>
        ///     Determines if the unit is in the shop area.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool InShop(this Obj_AI_Base unit)
            =>
            unit.IsVisible
            && unit.InRange(
                unit.Team == ObjectManager.Player.Team ? MiniCache.AllyFountain : MiniCache.EnemyFountain,
                Map.GetMap()?.Type == Map.MapType.SummonersRift ? 1000 : 750,
                true);

        /// <summary>
        ///     Determines if the spell is an auto attack.
        /// </summary>
        /// <param name="spellData">
        ///     The spell data.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsAutoAttack(this SpellData spellData) => Orbwalking.IsAutoAttack(spellData.Name);

        /// <summary>
        ///     Determines if the spell is an auto attack.
        /// </summary>
        /// <param name="spellData">
        ///     The spell data.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsAutoAttack(this SpellDataInst spellData) => Orbwalking.IsAutoAttack(spellData.Name);

        /// <summary>
        ///     Determines if both source and target are facing each other.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsBothFacing(Obj_AI_Hero source, Obj_AI_Base target)
            => source.IsFacing(target) && target.IsFacing(source);

        /// <summary>
        ///     Determines if the spell was casted, by the cast state.
        /// </summary>
        /// <param name="state">
        ///     The state.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsCasted(this Spell.CastStates state) => state == Spell.CastStates.SuccessfullyCasted;

        /// <summary>
        ///     Determines if the unit is a champion.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsChampion(this Obj_AI_Base unit) => IsChampion(unit, null, false);

        /// <summary>
        ///     Determines if the unit is a champion.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <param name="championName">
        ///     The champion name.
        /// </param>
        /// <param name="nameCheck">
        ///     A value indicating whether the function should name check.
        /// </param>
        /// <param name="stringComparison">
        ///     The string comparison type.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsChampion(
            this Obj_AI_Base unit,
            string championName,
            bool nameCheck = true,
            StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            var hero = unit as Obj_AI_Hero;
            return hero != null && hero.IsValid
                   && (!nameCheck || hero.ChampionName.Equals(championName, stringComparison));
        }

        /// <summary>
        ///     Determines if source is facing the target.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsFacing(this Obj_AI_Base source, Obj_AI_Base target)
        {
            if (source == null || target == null)
            {
                return false;
            }

            const float Angle = 90;
            return source.Direction.To2D().Perpendicular().AngleBetween((target.Position - source.Position).To2D())
                   < Angle;
        }

        /// <summary>
        ///     Determines if the hero's movement is imparied.
        /// </summary>
        /// <param name="hero">
        ///     The hero.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsMovementImpaired(this Obj_AI_Hero hero)
        {
            return hero.HasBuffOfType(BuffType.Flee) || hero.HasBuffOfType(BuffType.Charm)
                   || hero.HasBuffOfType(BuffType.Slow) || hero.HasBuffOfType(BuffType.Snare)
                   || hero.HasBuffOfType(BuffType.Stun) || hero.HasBuffOfType(BuffType.Taunt);
        }

        /// <summary>
        ///     Determines if the hero's movement is imparied.
        /// </summary>
        /// <param name="hero">
        ///     The hero.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsMovementImparied(this Obj_AI_Hero hero)
            =>
            hero.HasBuffOfType(BuffType.Flee) || hero.HasBuffOfType(BuffType.Charm) || hero.HasBuffOfType(BuffType.Slow)
            || hero.HasBuffOfType(BuffType.Snare) || hero.HasBuffOfType(BuffType.Stun)
            || hero.HasBuffOfType(BuffType.Taunt);

        /// <summary>
        ///     Determines if the position is on the screen.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsOnScreen(this Vector3 position)
        {
            var pos = Drawing.WorldToScreen(position);
            return pos.X > 0 && pos.X <= Drawing.Width && pos.Y > 0 && pos.Y <= Drawing.Height;
        }

        /// <summary>
        ///     Determines if the position is on the screen.
        /// </summary>
        /// <param name="position">
        ///     The position/
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsOnScreen(this Vector2 position) => position.To3D().IsOnScreen();

        /// <summary>
        ///     Determines if the spell is ready.
        /// </summary>
        /// <param name="spell">
        ///     The spell.
        /// </param>
        /// <param name="t">
        ///     The time.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsReady(this SpellDataInst spell, int t = 0)
            =>
            spell?.Slot != SpellSlot.Unknown && t == 0
                ? spell?.State == SpellState.Ready
                : spell?.State == SpellState.Ready
                  || (spell?.State == SpellState.Cooldown && (spell.CooldownExpires - Game.Time) <= t / 1000f);

        /// <summary>
        ///     Determines if the spell is ready.
        /// </summary>
        /// <param name="spell">
        ///     The spell.
        /// </param>
        /// <param name="t">
        ///     The time.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsReady(this Spell spell, int t = 0) => IsReady(spell?.Instance, t);

        /// <summary>
        ///     Determines if the spell is ready.
        /// </summary>
        /// <param name="slot">
        ///     The slot.
        /// </param>
        /// <param name="t">
        ///     The time.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsReady(this SpellSlot slot, int t = 0)
            => IsReady(ObjectManager.Player.Spellbook.GetSpell(slot), t);

        /// <summary>
        ///     Determines if the unit is recalling.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsRecalling(this Obj_AI_Hero unit)
            =>
            unit.Buffs.Any(
                buff =>
                    buff.Name.Equals("recall", StringComparison.CurrentCultureIgnoreCase)
                    && buff.Type == BuffType.Aura);

        /// <summary>
        ///     Determines if the object is valid.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the object.
        /// </typeparam>
        /// <param name="obj">
        ///     The object.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsValid<T>(this GameObject obj)
            where T : GameObject
            => (obj?.IsValid ?? false) && obj is T;

        /// <summary>
        ///     Determines if the buff is valid.
        /// </summary>
        /// <param name="buff">
        ///     The buff.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsValidBuff(this BuffInstance buff)
            => buff != null && buff.IsActive && buff.EndTime - Game.Time > 0;

        /// <summary>
        ///     Determines if the SpellSlot of the InventorySlot is valid.
        /// </summary>
        /// <param name="slot">
        ///     The slot.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsValidSlot(this InventorySlot slot) => slot?.SpellSlot != SpellSlot.Unknown;

        /// <summary>
        ///     Determines if the target is valid.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <param name="checkTeam">
        ///     A value indicating whether to check team (if ally).
        /// </param>
        /// <param name="from">
        ///     The from location.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsValidTarget(
            this AttackableUnit unit,
            float range = float.MaxValue,
            bool checkTeam = true,
            Vector3 from = default(Vector3))
        {
            if (unit == null || !unit.IsValid || !unit.IsVisible || unit.IsDead || !unit.IsTargetable)
            {
                return false;
            }

            if (unit.IsInvulnerable || (checkTeam && unit.Team == ObjectManager.Player.Team))
            {
                return false;
            }

            if (unit.Name.Equals("WardCorpse", StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            if (range < float.MaxValue)
            {
                var @base = unit as Obj_AI_Hero;
                var value1 = (from.To2D().IsValid() ? from : ObjectManager.Player.ServerPosition).To2D();
                var value2 = (@base?.ServerPosition ?? unit.Position).To2D();

                return Vector2.DistanceSquared(value1, value2) > range * range;
            }

            return true;
        }

        /// <summary>
        ///     Determines if the position is a wall.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsWall(this Vector3 position)
            => NavMesh.GetCollisionFlags(position).HasFlag(CollisionFlags.Wall);

        /// <summary>
        ///     Determines if the position is a wall.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsWall(this Vector2 position) => position.To3D().IsWall();

        /// <summary>
        ///     Levels up a spell.
        /// </summary>
        /// <param name="book">
        ///     The spell book.
        /// </param>
        /// <param name="slot">
        ///     The slot.
        /// </param>
        /// <param name="evolve">
        ///     A value indicating whether to evolve or level up.
        /// </param>
        public static void LevelUpSpell(this Spellbook book, SpellSlot slot, bool evolve = false)
        {
            if (evolve)
            {
                book.LevelSpell(slot);
            }
            else
            {
                book.EvolveSpell(slot);
            }
        }

        /// <summary>
        ///     Gets the mana percent of the unit.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="float" />.
        /// </returns>
        public static float ManaPercentage(this Obj_AI_Base unit) => unit.ManaPercent;

        /// <summary>
        ///     Process the packet data as a game packet.
        /// </summary>
        /// <param name="packetData">
        ///     The packet data.
        /// </param>
        /// <param name="packetChannel">
        ///     The packet channel.
        /// </param>
        public static void ProcessAsPacket(this byte[] packetData, PacketChannel packetChannel = PacketChannel.S2C)
            => Game.ProcessPacket(packetData, packetChannel);

        /// <summary>
        ///     Randomizes the given position.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="min">
        ///     The minimum value of randomization.
        /// </param>
        /// <param name="max">
        ///     The max value of randomization.
        /// </param>
        /// <returns>
        ///     The <see cref="Vector3" />.
        /// </returns>
        public static Vector3 Randomize(this Vector3 position, int min, int max)
        {
            var ran = new Random(Utils.TickCount);
            return position + new Vector2(ran.Next(min, max), ran.Next(min, max)).To3D();
        }

        /// <summary>
        ///     Randomizes the given position.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="min">
        ///     The minimum value of randomization.
        /// </param>
        /// <param name="max">
        ///     The max value of randomization.
        /// </param>
        /// <returns>
        ///     The <see cref="Vector3" />.
        /// </returns>
        public static Vector2 Randomize(this Vector2 position, int min, int max)
            => position.To3D().Randomize(min, max).To2D();

        /// <summary>
        ///     Send the packet data as a game packet.
        /// </summary>
        /// <param name="packetData">
        ///     The packet data.
        /// </param>
        /// <param name="packetChannel">
        ///     The packet channel.
        /// </param>
        /// <param name="protocolFlags">
        ///     The protocol flags.
        /// </param>
        public static void SendAsPacket(
                this byte[] packetData,
                PacketChannel packetChannel = PacketChannel.C2S,
                PacketProtocolFlags protocolFlags = PacketProtocolFlags.Reliable)
            => Game.SendPacket(packetData, packetChannel, protocolFlags);

        /// <summary>
        ///     Transforms the position into a <see cref="NavMeshCell" />.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="NavMeshCell" />.
        /// </returns>
        public static NavMeshCell ToNavMeshCell(this Vector3 position)
        {
            var nav = NavMesh.WorldToGrid(position.X, position.Y);
            return NavMesh.GetCell((short)nav.X, (short)nav.Y);
        }

        /// <summary>
        ///     Gets the total attack damage of the unit.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="float" />.
        /// </returns>
        public static float TotalAttackDamage(this Obj_AI_Hero unit) => unit.TotalAttackDamage;

        /// <summary>
        ///     Gets the total magical damage of the unit.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="float" />.
        /// </returns>
        public static float TotalMagicalDamage(this Obj_AI_Hero unit) => unit.TotalMagicalDamage;

        /// <summary>
        ///     Deterins if the unit is under an ally turret.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool UnderAllyTurret(this GameObject unit) => UnderAllyTurret(unit.Position);

        /// <summary>
        ///     Determines if the position is under an ally turret.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool UnderAllyTurret(this Vector3 position)
            =>
            ObjectManager.Get<Obj_AI_Turret>()
                .Any(turret => turret.IsValidTarget(950, false, position) && turret.IsAlly);

        /// <summary>
        ///     Determines if the unit is under a turret.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool UnderTurret(this GameObject unit) => UnderTurret(unit.Position, true);

        /// <summary>
        ///     Determines if the unit is under a turret.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <param name="enemyTurretsOnly">
        ///     A value indicating if enemy turrets only.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool UnderTurret(this GameObject unit, bool enemyTurretsOnly)
            => UnderTurret(unit.Position, enemyTurretsOnly);

        /// <summary>
        ///     Determines if the position is under the turret.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="enemyTurretsOnly">
        ///     A value indicating if enemy turrets only.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool UnderTurret(this Vector3 position, bool enemyTurretsOnly)
            => ObjectManager.Get<Obj_AI_Turret>().Any(turret => turret.IsValidTarget(950, enemyTurretsOnly, position));

        #endregion
    }
}