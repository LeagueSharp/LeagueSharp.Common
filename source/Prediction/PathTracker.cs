// <copyright file="PathTracker.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SharpDX;

    /// <summary>
    ///     Provides a path tracker for units.
    /// </summary>
    public static class PathTracker
    {
        #region Constants

        /// <summary>
        ///     Max track time.
        /// </summary>
        public const double MaxTime = 1.5d;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="PathTracker" /> class.
        /// </summary>
        static PathTracker()
        {
            Obj_AI_Base.OnNewPath += OnNewPath;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the stored paths.
        /// </summary>
        public static IDictionary<int, List<StoredPath>> StoredPaths { get; } = new Dictionary<int, List<StoredPath>>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the current unit path.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="StoredPath" />.
        /// </returns>
        public static StoredPath GetCurrentPath(Obj_AI_Base unit)
        {
            List<StoredPath> paths;
            return StoredPaths.TryGetValue(unit.NetworkId, out paths) ? paths.LastOrDefault() : new StoredPath();
        }

        /// <summary>
        ///     Gets the mean speed of the unit.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <param name="maxTime">
        ///     The max time.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public static double GetMeanSpeed(Obj_AI_Base unit, double maxTime)
        {
            var paths = GetStoredPaths(unit, MaxTime);
            var distance = 0d;
            if (paths.Any())
            {
                distance += (maxTime - paths[0].Time) * unit.MoveSpeed;
                for (var i = 0; i < paths.Count - 1; ++i)
                {
                    var currentPath = paths[i];
                    var nextPath = paths[i + 1];

                    if (currentPath.WaypointCount > 0)
                    {
                        distance += Math.Min(
                            (currentPath.Time - nextPath.Time) * unit.MoveSpeed,
                            currentPath.Path.PathLength());
                    }
                }

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

            return distance / maxTime;
        }

        /// <summary>
        ///     Gets the stored paths of the unit.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <param name="maxTime">
        ///     The max time.
        /// </param>
        /// <returns>
        ///     The <see cref="List{StoredPath}" />.
        /// </returns>
        public static List<StoredPath> GetStoredPaths(Obj_AI_Base unit, double maxTime)
        {
            List<StoredPath> paths;
            return StoredPaths.TryGetValue(unit.NetworkId, out paths) ? paths : new List<StoredPath>();
        }

        /// <summary>
        ///     Gets the tendency of the unit.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="Vector3" />.
        /// </returns>
        public static Vector3 GetTendency(Obj_AI_Base unit)
        {
            var paths = GetStoredPaths(unit, MaxTime);
            var result = default(Vector2);

            foreach (var path in paths)
            {
                var k = 1; // (MaxTime - path.Time); TODO
                result = result + (k * (path.EndPoint - unit.ServerPosition.To2D()).Normalized());
            }

            return (result / paths.Count).To3D();
        }

        #endregion

        #region Methods

        private static void OnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
        {
            if (!(sender is Obj_AI_Hero))
            {
                return;
            }

            if (!StoredPaths.ContainsKey(sender.NetworkId))
            {
                StoredPaths.Add(sender.NetworkId, new List<StoredPath>());
            }

            var path = new StoredPath { Tick = Utils.GameTimeTickCount, Path = args.Path.ToList().To2D() };
            StoredPaths[sender.NetworkId].Add(path);

            if (StoredPaths[sender.NetworkId].Count > 50)
            {
                StoredPaths[sender.NetworkId].RemoveRange(0, 40);
            }
        }

        #endregion
    }
}