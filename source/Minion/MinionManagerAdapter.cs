// <copyright file="MinionManagerAdapter.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using JetBrains.Annotations;

    using log4net;

    using PlaySharp.Toolkit.Logging;

    using SharpDX;

    /// <summary>
    ///     Provides a minion AI manager.
    /// </summary>
    public partial class MinionManager
    {
        #region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static MinionManager Instance => Library.Instance?.MinionManager;

        #endregion

        #region Properties

        private static ILog Log { get; } = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Calculates the best position, with the maximum amount of minion hits for a circular farm.
        /// </summary>
        /// <param name="positions">
        ///     The minion positions.
        /// </param>
        /// <param name="width">
        ///     The widht.
        /// </param>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <param name="useConvexHullMax">
        ///     The max convex hull usage.
        /// </param>
        /// <returns>
        ///     The <see cref="FarmLocation" />.
        /// </returns>
        public static FarmLocation GetBestCircularFarmLocation(
            List<Vector2> positions,
            float width,
            float range,
            int useConvexHullMax = 9)
        {
            var result = default(Vector2);
            var minionCount = 0;
            var startPos = ObjectManager.Player.ServerPosition.To2D();

            if (positions.Any())
            {
                if (positions.Count <= useConvexHullMax)
                {
                    var combinations = GetCombinations(positions);
                    foreach (var combination in combinations)
                    {
                        if (combination.Any())
                        {
                            var circle = MEC.GetMec(combination);
                            if (circle.Radius <= width && circle.Center.InRange(startPos, range, true))
                            {
                                return new FarmLocation(circle.Center, combination.Count);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var pos in positions)
                    {
                        if (pos.InRange(startPos, range, true))
                        {
                            var count = positions.Count(p => pos.InRange(p, width, true));
                            if (count >= minionCount)
                            {
                                result = pos;
                                minionCount = count;
                            }
                        }
                    }
                }
            }

            return new FarmLocation(result, minionCount);
        }

        /// <summary>
        ///     Calculates the best position, with the maximum amount of minion hits for a linear farm.
        /// </summary>
        /// <param name="positions">
        ///     The minion positions.
        /// </param>
        /// <param name="width">
        ///     The width.
        /// </param>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <returns>
        ///     The <see cref="FarmLocation" />.
        /// </returns>
        public static FarmLocation GetBestLineFarmLocation(List<Vector2> positions, float width, float range)
        {
            var result = default(Vector2);
            var minionCount = 0;
            var startPos = ObjectManager.Player.ServerPosition.To2D();

            var possiblePositions = new List<Vector2>();
            possiblePositions.AddRange(positions);

            var max = positions.Count;
            for (var i = 0; i < max; ++i)
            {
                for (var j = 0; j < max; ++j)
                {
                    if (positions[j] != positions[i])
                    {
                        possiblePositions.Add((positions[j] + positions[i]) / 2);
                    }
                }
            }

            foreach (var pos in possiblePositions)
            {
                if (pos.InRange(startPos, range, true))
                {
                    var endPos = startPos + (range * (pos - startPos).Normalized());
                    var count = positions.Count(p => p.Distance(startPos, endPos, true, true) <= width * width);
                    if (count >= minionCount)
                    {
                        result = endPos;
                        minionCount = count;
                    }
                }
            }

            return new FarmLocation(result, minionCount);
        }

        /// <summary>
        ///     Gets the minion info.
        /// </summary>
        /// <param name="minion">
        ///     The minion.
        /// </param>
        /// <returns>
        ///     The <see cref="MinionInfo" />.
        /// </returns>
        public static MinionInfo GetMinionInfo(Obj_AI_Minion minion)
        {
            MinionInfo info;
            if (Instance != null && Instance.MinionInfos.TryGetValue(minion.NetworkId, out info))
            {
                return info;
            }

            return null;
        }

        /// <summary>
        ///     Allocates a list of minions with the specific request.
        /// </summary>
        /// <param name="from">
        ///     The from position.
        /// </param>
        /// <param name="range">
        ///     The range from the position.
        /// </param>
        /// <param name="type">
        ///     The minion type.
        /// </param>
        /// <param name="team">
        ///     The minion team.
        /// </param>
        /// <param name="order">
        ///     The minion order.
        /// </param>
        /// <returns>
        ///     The <see cref="List{Obj_AI_Base}" />.
        /// </returns>
        [CanBeNull]
        public static List<Obj_AI_Base> GetMinions(
            Vector3 from,
            float range,
            MinionTypes type = MinionTypes.All,
            MinionTeam team = MinionTeam.Enemy,
            MinionOrderTypes order = MinionOrderTypes.Health)
        {
            if (Instance == null)
            {
                Log.Fatal("MinionManager instance is not available.");
                return null;
            }

            var returnList = new List<Obj_AI_Base>();
            var allyTeam = ObjectManager.Player.Team;
            foreach (var minion in Instance.MinionInfos.Values)
            {
                if (!minion.Instance.IsValidTarget(range, false, from))
                {
                    continue;
                }

                var mType = minion.Instance.IsMelee;
                var mTeam = minion.Instance.Team;
                var typePass = false;
                var teamPass = false;

                switch (type)
                {
                    case MinionTypes.Melee:
                        typePass = mType;
                        break;
                    case MinionTypes.Ranged:
                        typePass = !mType;
                        break;
                    case MinionTypes.Wards:
                        typePass = minion.IsWard;
                        break;
                    case MinionTypes.All:
                        typePass = true;
                        break;
                }

                switch (team)
                {
                    case MinionTeam.Ally:
                        teamPass = mTeam == allyTeam;
                        break;
                    case MinionTeam.Enemy:
                        teamPass = mTeam != allyTeam;
                        break;
                    case MinionTeam.Neutral:
                        teamPass = mTeam == GameObjectTeam.Neutral;
                        break;
                    case MinionTeam.All:
                        teamPass = true;
                        break;
                }

                if (typePass && teamPass)
                {
                    returnList.Add(minion.Instance);
                }
            }

            switch (order)
            {
                case MinionOrderTypes.Health:
                    return returnList.OrderBy(o => o.Health).ToList();
                case MinionOrderTypes.MaxHealth:
                    return returnList.OrderByDescending(o => o.MaxHealth).ToList();
                case MinionOrderTypes.None:
                    return returnList;
                default:
                    Log.Warn("Order is not recongized.");
                    return returnList;
            }
        }

        /// <summary>
        ///     Allocates a list of minions with the specific request.
        /// </summary>
        /// <param name="range">
        ///     The range from the position.
        /// </param>
        /// <param name="type">
        ///     The minion type.
        /// </param>
        /// <param name="team">
        ///     The minion team.
        /// </param>
        /// <param name="order">
        ///     The minion order.
        /// </param>
        /// <returns>
        ///     The <see cref="List{Obj_AI_Base}" />.
        /// </returns>
        [CanBeNull]
        public static List<Obj_AI_Base> GetMinions(
                float range,
                MinionTypes type = MinionTypes.All,
                MinionTeam team = MinionTeam.Enemy,
                MinionOrderTypes order = MinionOrderTypes.Health)
            => GetMinions(ObjectManager.Player.ServerPosition, range, type, team, order);

        /// <summary>
        ///     Gets the minions predicted positions.
        /// </summary>
        /// <param name="minions">
        ///     The minions.
        /// </param>
        /// <param name="delay">
        ///     The delay.
        /// </param>
        /// <param name="width">
        ///     The width.
        /// </param>
        /// <param name="speed">
        ///     The speed.
        /// </param>
        /// <param name="from">
        ///     The from position.
        /// </param>
        /// <param name="range">
        ///     The max range from position.
        /// </param>
        /// <param name="collision">
        ///     A value indciating whether to include collision.
        /// </param>
        /// <param name="type">
        ///     The skillshot type.
        /// </param>
        /// <param name="rangeCheckFrom">
        ///     The range check from position.
        /// </param>
        /// <returns>
        ///     The <see cref="List{Vector2}" />.
        /// </returns>
        public static List<Vector2> GetMinionsPredictedPositions(
            [NotNull] List<Obj_AI_Base> minions,
            float delay,
            float width,
            float speed,
            Vector3 from,
            float range,
            bool collision,
            SkillshotType type,
            Vector3 rangeCheckFrom = default(Vector3))
        {
            from = !from.IsZero ? from : ObjectManager.Player.ServerPosition;

            return (from minion in minions
                    select
                    Prediction.GetPrediction(
                        new PredictionInput
                            {
                                Unit = minion, Delay = delay, Radius = width, Speed = speed, From = @from,
                                Range = range, Collision = collision, Type = type, RangeCheckFrom = rangeCheckFrom
                            })
                    into pos
                    where pos.Hitchance >= HitChance.High
                    select pos.UnitPosition.To2D()).ToList();
        }

        /// <summary>
        ///     Determines if the minion is a jungle mob.
        /// </summary>
        /// <param name="minion">
        ///     The minion.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsJungleMob(Obj_AI_Minion minion)
        {
            MinionInfo info;
            if (Instance != null && Instance.MinionInfos.TryGetValue(minion.NetworkId, out info))
            {
                return info.IsJungleMob;
            }

            return false;
        }

        /// <summary>
        ///     Determines if the minion is part of minion AI.
        /// </summary>
        /// <param name="minion">
        ///     The minion.
        /// </param>
        /// <param name="includeWards">
        ///     A value indicating whether to include wards.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsMinion(Obj_AI_Minion minion, bool includeWards = false)
        {
            if (minion == null)
            {
                return false;
            }

            MinionInfo info;
            if (Instance != null && Instance.MinionInfos.TryGetValue(minion.NetworkId, out info))
            {
                return info.IsMinion || (includeWards && info.IsWard);
            }

            return minion.Name.Contains("Minion") || (includeWards && IsWard(minion));
        }

        /// <summary>
        ///     Determines if the minion is a ward by skin name.
        /// </summary>
        /// <param name="baseSkinName">
        ///     The base skin name.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsWard(string baseSkinName)
        {
            return baseSkinName.Contains("ward");
        }

        /// <summary>
        ///     Determines if the minion is a ward.
        /// </summary>
        /// <param name="minion">
        ///     The minion.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsWard(Obj_AI_Minion minion)
        {
            if (minion == null)
            {
                return false;
            }

            MinionInfo info;
            if (Instance != null && Instance.MinionInfos.TryGetValue(minion.NetworkId, out info))
            {
                return info.IsWard;
            }

            return minion.Name.Contains("Ward") && minion.IsHPBarRendered;
        }

        #endregion

        #region Methods

        private static IEnumerable<List<Vector2>> GetCombinations(IReadOnlyCollection<Vector2> values)
        {
            var collection = new List<List<Vector2>>();
            for (var index = 0; index < (1 << values.Count); ++index)
            {
                collection.Add(values.Where((t, i) => (index & (1 << i)) == 0).ToList());
            }

            return collection;
        }

        #endregion
    }
}