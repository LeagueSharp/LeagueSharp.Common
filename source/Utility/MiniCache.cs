// <copyright file="MiniCache.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using SharpDX;

    /// <summary>
    ///     The utility class.
    /// </summary>
    public partial class Utility
    {
        /// <summary>
        ///     The mini cache.
        /// </summary>
        public static class MiniCache
        {
            #region Static Fields

            private static Vector3 allyFountain;

            private static Vector3 enemyFountain;

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the ally fountain.
            /// </summary>
            public static Vector3 AllyFountain
            {
                get
                {
                    if (!allyFountain.IsZero)
                    {
                        return allyFountain;
                    }

                    return allyFountain = ObjectManager.Get<Obj_SpawnPoint>().Find(o => o.IsAlly).Position;
                }
            }

            /// <summary>
            ///     Gets the enemy fountain.
            /// </summary>
            public static Vector3 EnemyFountain
            {
                get
                {
                    if (!enemyFountain.IsZero)
                    {
                        return enemyFountain;
                    }

                    return enemyFountain = ObjectManager.Get<Obj_SpawnPoint>().Find(o => o.IsEnemy).Position;
                }
            }

            #endregion
        }
    }
}