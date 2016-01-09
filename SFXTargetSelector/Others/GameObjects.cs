#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 GameObjects.cs is part of SFXTargetSelector.

 SFXTargetSelector is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 SFXTargetSelector is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with SFXTargetSelector. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion License

#region

using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace SFXTargetSelector.Others
{
    /// <summary>
    ///     A static (stack) class which contains a sort-of cached versions of the important game objects.
    /// </summary>
    public static class GameObjects
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="GameObjects" /> class.
        /// </summary>
        static GameObjects()
        {
            if (!_initialized)
            {
                Initialize();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Compares two <see cref="GameObject" /> and returns if they are identical.
        /// </summary>
        /// <param name="gameObject">The GameObject</param>
        /// <param name="object">The Compare GameObject</param>
        /// <returns>Whether the <see cref="GameObject" />s are identical.</returns>
        public static bool Compare(this GameObject gameObject, GameObject @object)
        {
            return gameObject.NetworkId == @object.NetworkId;
        }

        #endregion

        #region Static Fields

        /// <summary>
        ///     The ally heroes list.
        /// </summary>
        private static readonly HashSet<Obj_AI_Hero> AllyHeroesList = new HashSet<Obj_AI_Hero>();

        /// <summary>
        ///     The ally list.
        /// </summary>
        private static readonly HashSet<Obj_AI_Base> AllyList = new HashSet<Obj_AI_Base>();

        /// <summary>
        ///     The ally minions list.
        /// </summary>
        private static readonly HashSet<Obj_AI_Minion> AllyMinionsList = new HashSet<Obj_AI_Minion>();

        /// <summary>
        ///     The ally shops list.
        /// </summary>
        private static readonly HashSet<Obj_Shop> AllyShopsList = new HashSet<Obj_Shop>();

        /// <summary>
        ///     The ally spawn points list.
        /// </summary>
        private static readonly HashSet<Obj_SpawnPoint> AllySpawnPointsList = new HashSet<Obj_SpawnPoint>();

        /// <summary>
        ///     The ally inhibitor turrets list.
        /// </summary>
        private static readonly HashSet<Obj_BarracksDampener> AllyInhibitorsList = new HashSet<Obj_BarracksDampener>();

        /// <summary>
        ///     The ally turrets list.
        /// </summary>
        private static readonly HashSet<Obj_AI_Turret> AllyTurretsList = new HashSet<Obj_AI_Turret>();

        /// <summary>
        ///     The ally wards list.
        /// </summary>
        private static readonly HashSet<Obj_AI_Minion> AllyWardsList = new HashSet<Obj_AI_Minion>();

        /// <summary>
        ///     The enemy heroes list.
        /// </summary>
        private static readonly HashSet<Obj_AI_Hero> EnemyHeroesList = new HashSet<Obj_AI_Hero>();

        /// <summary>
        ///     The enemy list.
        /// </summary>
        private static readonly HashSet<Obj_AI_Base> EnemyList = new HashSet<Obj_AI_Base>();

        /// <summary>
        ///     The enemy minions list.
        /// </summary>
        private static readonly HashSet<Obj_AI_Minion> EnemyMinionsList = new HashSet<Obj_AI_Minion>();

        /// <summary>
        ///     The enemy shops list.
        /// </summary>
        private static readonly HashSet<Obj_Shop> EnemyShopsList = new HashSet<Obj_Shop>();

        /// <summary>
        ///     The enemy spawn points list.
        /// </summary>
        private static readonly HashSet<Obj_SpawnPoint> EnemySpawnPointsList = new HashSet<Obj_SpawnPoint>();

        /// <summary>
        ///     The enemy inhibitor turrets list.
        /// </summary>
        private static readonly HashSet<Obj_BarracksDampener> EnemyInhibitorsList = new HashSet<Obj_BarracksDampener>();

        /// <summary>
        ///     The enemy turrets list.
        /// </summary>
        private static readonly HashSet<Obj_AI_Turret> EnemyTurretsList = new HashSet<Obj_AI_Turret>();

        /// <summary>
        ///     The enemy wards list.
        /// </summary>
        private static readonly HashSet<Obj_AI_Minion> EnemyWardsList = new HashSet<Obj_AI_Minion>();

        /// <summary>
        ///     The game objects list.
        /// </summary>
        private static readonly HashSet<GameObject> GameObjectsList = new HashSet<GameObject>();

        /// <summary>
        ///     The heroes list.
        /// </summary>
        private static readonly HashSet<Obj_AI_Hero> HeroesList = new HashSet<Obj_AI_Hero>();

        /// <summary>
        ///     The jungle list.
        /// </summary>
        private static readonly HashSet<Obj_AI_Minion> JungleList = new HashSet<Obj_AI_Minion>();

        /// <summary>
        ///     The minions list.
        /// </summary>
        private static readonly HashSet<Obj_AI_Minion> MinionsList = new HashSet<Obj_AI_Minion>();

        /// <summary>
        ///     The shops list.
        /// </summary>
        private static readonly HashSet<Obj_Shop> ShopsList = new HashSet<Obj_Shop>();

        /// <summary>
        ///     The spawn points list.
        /// </summary>
        private static readonly HashSet<Obj_SpawnPoint> SpawnPointsList = new HashSet<Obj_SpawnPoint>();

        /// <summary>
        ///     The inhibitor turrets list.
        /// </summary>
        private static readonly HashSet<Obj_BarracksDampener> InhibitorsList = new HashSet<Obj_BarracksDampener>();

        /// <summary>
        ///     The turrets list.
        /// </summary>
        private static readonly HashSet<Obj_AI_Turret> TurretsList = new HashSet<Obj_AI_Turret>();

        /// <summary>
        ///     The wards list.
        /// </summary>
        private static readonly HashSet<Obj_AI_Minion> WardsList = new HashSet<Obj_AI_Minion>();

        /// <summary>
        ///     Indicates whether the <see cref="GameObjects" /> stack was initialized and saved required instances.
        /// </summary>
        private static bool _initialized;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the game objects.
        /// </summary>
        public static IEnumerable<GameObject> AllGameObjects
        {
            get { return GameObjectsList; }
        }

        /// <summary>
        ///     Gets the ally.
        /// </summary>
        public static IEnumerable<Obj_AI_Base> Ally
        {
            get { return AllyList; }
        }

        /// <summary>
        ///     Gets the ally heroes.
        /// </summary>
        public static IEnumerable<Obj_AI_Hero> AllyHeroes
        {
            get { return AllyHeroesList; }
        }

        /// <summary>
        ///     Gets the ally minions.
        /// </summary>
        public static IEnumerable<Obj_AI_Minion> AllyMinions
        {
            get { return AllyMinionsList; }
        }

        /// <summary>
        ///     Gets the ally shops.
        /// </summary>
        public static IEnumerable<Obj_Shop> AllyShops
        {
            get { return AllyShopsList; }
        }

        /// <summary>
        ///     Gets the ally spawn points.
        /// </summary>
        public static IEnumerable<Obj_SpawnPoint> AllySpawnPoints
        {
            get { return AllySpawnPointsList; }
        }

        /// <summary>
        ///     Gets the ally turrets.
        /// </summary>
        public static IEnumerable<Obj_AI_Turret> AllyTurrets
        {
            get { return AllyTurretsList; }
        }

        /// <summary>
        ///     Gets the ally inhibitors.
        /// </summary>
        public static IEnumerable<Obj_BarracksDampener> AllyInhibitors
        {
            get { return AllyInhibitorsList; }
        }

        /// <summary>
        ///     Gets the ally wards.
        /// </summary>
        public static IEnumerable<Obj_AI_Minion> AllyWards
        {
            get { return AllyWardsList; }
        }

        /// <summary>
        ///     Gets the enemy.
        /// </summary>
        public static IEnumerable<Obj_AI_Base> Enemy
        {
            get { return EnemyList; }
        }

        /// <summary>
        ///     Gets the enemy heroes.
        /// </summary>
        public static IEnumerable<Obj_AI_Hero> EnemyHeroes
        {
            get { return EnemyHeroesList; }
        }

        /// <summary>
        ///     Gets the enemy minions.
        /// </summary>
        public static IEnumerable<Obj_AI_Minion> EnemyMinions
        {
            get { return EnemyMinionsList; }
        }

        /// <summary>
        ///     Gets the enemy shops.
        /// </summary>
        public static IEnumerable<Obj_Shop> EnemyShops
        {
            get { return EnemyShopsList; }
        }

        /// <summary>
        ///     Gets the enemy spawn points.
        /// </summary>
        public static IEnumerable<Obj_SpawnPoint> EnemySpawnPoints
        {
            get { return EnemySpawnPointsList; }
        }

        /// <summary>
        ///     Gets the enemy inhibitors.
        /// </summary>
        public static IEnumerable<Obj_BarracksDampener> EnemyInhibitors
        {
            get { return EnemyInhibitorsList; }
        }

        /// <summary>
        ///     Gets the enemy turrets.
        /// </summary>
        public static IEnumerable<Obj_AI_Turret> EnemyTurrets
        {
            get { return EnemyTurretsList; }
        }

        /// <summary>
        ///     Gets the enemy wards.
        /// </summary>
        public static IEnumerable<Obj_AI_Minion> EnemyWards
        {
            get { return EnemyWardsList; }
        }

        /// <summary>
        ///     Gets the heroes.
        /// </summary>
        public static IEnumerable<Obj_AI_Hero> Heroes
        {
            get { return HeroesList; }
        }

        /// <summary>
        ///     Gets the jungle.
        /// </summary>
        public static IEnumerable<Obj_AI_Minion> Jungle
        {
            get { return JungleList; }
        }

        /// <summary>
        ///     Gets the minions.
        /// </summary>
        public static IEnumerable<Obj_AI_Minion> Minions
        {
            get { return MinionsList; }
        }

        /// <summary>
        ///     Gets or sets the player.
        /// </summary>
        public static Obj_AI_Hero Player { get; private set; }

        /// <summary>
        ///     Gets or sets the ally nexus.
        /// </summary>
        public static Obj_HQ AllyNexus { get; private set; }

        /// <summary>
        ///     Gets or sets the enemy nexus.
        /// </summary>
        public static Obj_HQ EnemyNexus { get; private set; }

        /// <summary>
        ///     Gets the shops.
        /// </summary>
        public static IEnumerable<Obj_Shop> Shops
        {
            get { return ShopsList; }
        }

        /// <summary>
        ///     Gets the spawn points.
        /// </summary>
        public static IEnumerable<Obj_SpawnPoint> SpawnPoints
        {
            get { return SpawnPointsList; }
        }

        /// <summary>
        ///     Gets the inhibitors.
        /// </summary>
        public static IEnumerable<Obj_BarracksDampener> Inhibitors
        {
            get { return InhibitorsList; }
        }

        /// <summary>
        ///     Gets the turrets.
        /// </summary>
        public static IEnumerable<Obj_AI_Turret> Turrets
        {
            get { return TurretsList; }
        }

        /// <summary>
        ///     Gets the wards.
        /// </summary>
        public static IEnumerable<Obj_AI_Minion> Wards
        {
            get { return WardsList; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The initialize method.
        /// </summary>
        public static void Initialize()
        {
            try
            {
                if (_initialized)
                {
                    return;
                }

                _initialized = true;

                CustomEvents.Game.OnGameLoad += delegate
                {
                    Player = ObjectManager.Player;

                    AllyNexus = ObjectManager.Get<Obj_HQ>().FirstOrDefault(o => o.IsAlly);
                    EnemyNexus = ObjectManager.Get<Obj_HQ>().FirstOrDefault(o => o.IsEnemy);

                    HeroesList.UnionWith(ObjectManager.Get<Obj_AI_Hero>());
                    MinionsList.UnionWith(
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Where(
                                o =>
                                    o.Team != GameObjectTeam.Neutral &&
                                    !o.CharData.BaseSkinName.ToLower().Contains("jarvanivstandard") &&
                                    !o.CharData.BaseSkinName.ToLower().Contains("ward") &&
                                    !o.CharData.BaseSkinName.ToLower().Contains("trinket")));
                    InhibitorsList.UnionWith(ObjectManager.Get<Obj_BarracksDampener>());
                    TurretsList.UnionWith(ObjectManager.Get<Obj_AI_Turret>());
                    JungleList.UnionWith(
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Where(
                                o =>
                                    o.Team == GameObjectTeam.Neutral &&
                                    !o.CharData.BaseSkinName.ToLower().Contains("barrel")));
                    WardsList.UnionWith(
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Where(
                                o =>
                                    o.CharData.BaseSkinName.ToLower().Contains("ward") ||
                                    o.CharData.BaseSkinName.ToLower().Contains("trinket")));
                    ShopsList.UnionWith(ObjectManager.Get<Obj_Shop>());
                    SpawnPointsList.UnionWith(ObjectManager.Get<Obj_SpawnPoint>());
                    GameObjectsList.UnionWith(ObjectManager.Get<GameObject>());

                    EnemyHeroesList.UnionWith(HeroesList.Where(o => o.IsEnemy));
                    EnemyMinionsList.UnionWith(MinionsList.Where(o => o.IsEnemy));
                    EnemyInhibitorsList.UnionWith(InhibitorsList.Where(o => o.IsEnemy));
                    EnemyTurretsList.UnionWith(TurretsList.Where(o => o.IsEnemy));
                    EnemyList.UnionWith(
                        EnemyHeroesList.Concat(EnemyMinionsList.Cast<Obj_AI_Base>()).Concat(EnemyTurretsList));

                    AllyHeroesList.UnionWith(HeroesList.Where(o => o.IsAlly));
                    AllyMinionsList.UnionWith(MinionsList.Where(o => o.IsAlly));
                    AllyInhibitorsList.UnionWith(InhibitorsList.Where(o => o.IsAlly));
                    AllyTurretsList.UnionWith(TurretsList.Where(o => o.IsAlly));
                    AllyList.UnionWith(
                        AllyHeroesList.Concat(AllyMinionsList.Cast<Obj_AI_Base>()).Concat(AllyTurretsList));

                    AllyWardsList.UnionWith(WardsList.Where(o => o.IsAlly));
                    EnemyWardsList.UnionWith(WardsList.Where(o => o.IsEnemy));

                    AllyShopsList.UnionWith(ShopsList.Where(o => o.IsAlly));
                    EnemyShopsList.UnionWith(ShopsList.Where(o => o.IsEnemy));

                    AllySpawnPointsList.UnionWith(SpawnPointsList.Where(o => o.IsAlly));
                    EnemySpawnPointsList.UnionWith(SpawnPointsList.Where(o => o.IsEnemy));

                    GameObject.OnCreate += OnGameObjectCreate;
                    GameObject.OnDelete += OnGameObjectDelete;
                    Game.OnUpdate += OnGameUpdate;
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        ///     OnCreate event.
        /// </summary>
        /// <param name="sender">
        ///     The sender
        /// </param>
        /// <param name="args">
        ///     The event data
        /// </param>
        private static void OnGameObjectCreate(GameObject sender, EventArgs args)
        {
            GameObjectsList.Add(sender);

            var hero = sender as Obj_AI_Hero;
            if (hero != null)
            {
                HeroesList.Add(hero);
                if (hero.IsEnemy)
                {
                    EnemyHeroesList.Add(hero);
                    EnemyList.Add(hero);
                }
                else
                {
                    AllyHeroesList.Add(hero);
                    AllyList.Add(hero);
                }

                return;
            }

            var minion = sender as Obj_AI_Minion;
            if (minion != null && !minion.CharData.BaseSkinName.ToLower().Contains("jarvanivstandard"))
            {
                if (minion.Team != GameObjectTeam.Neutral)
                {
                    if (!minion.CharData.BaseSkinName.Contains("ward") ||
                        !minion.CharData.BaseSkinName.Contains("trinket"))
                    {
                        MinionsList.Add(minion);
                        if (minion.IsEnemy)
                        {
                            EnemyMinionsList.Add(minion);
                            EnemyList.Add(minion);
                        }
                        else
                        {
                            AllyMinionsList.Add(minion);
                            AllyList.Add(minion);
                        }
                    }
                    else
                    {
                        WardsList.Add(minion);
                        if (minion.IsEnemy)
                        {
                            EnemyWardsList.Add(minion);
                        }
                        else
                        {
                            AllyWardsList.Add(minion);
                        }
                    }
                }
                else if (!minion.CharData.BaseSkinName.ToLower().Contains("barrel"))
                {
                    JungleList.Add(minion);
                }

                return;
            }

            var inhibitor = sender as Obj_BarracksDampener;
            if (inhibitor != null)
            {
                InhibitorsList.Add(inhibitor);
                if (inhibitor.IsEnemy)
                {
                    EnemyInhibitorsList.Add(inhibitor);
                }
                else
                {
                    AllyInhibitorsList.Add(inhibitor);
                }
                return;
            }

            var turret = sender as Obj_AI_Turret;
            if (turret != null)
            {
                TurretsList.Add(turret);
                if (turret.IsEnemy)
                {
                    EnemyTurretsList.Add(turret);
                    EnemyList.Add(turret);
                }
                else
                {
                    AllyTurretsList.Add(turret);
                    AllyList.Add(turret);
                }

                return;
            }

            var shop = sender as Obj_Shop;
            if (shop != null)
            {
                ShopsList.Add(shop);
                if (shop.IsAlly)
                {
                    AllyShopsList.Add(shop);
                }
                else
                {
                    EnemyShopsList.Add(shop);
                }

                return;
            }

            var spawnPoint = sender as Obj_SpawnPoint;
            if (spawnPoint != null)
            {
                SpawnPointsList.Add(spawnPoint);
                if (spawnPoint.IsAlly)
                {
                    AllySpawnPointsList.Add(spawnPoint);
                }
                else
                {
                    EnemySpawnPointsList.Add(spawnPoint);
                }
            }
        }

        /// <summary>
        ///     OnDelete event.
        /// </summary>
        /// <param name="sender">
        ///     The sender
        /// </param>
        /// <param name="args">
        ///     The event data
        /// </param>
        private static void OnGameObjectDelete(GameObject sender, EventArgs args)
        {
            GameObjectsList.Remove(sender);

            var hero = sender as Obj_AI_Hero;
            if (hero != null)
            {
                HeroesList.Remove(hero);
                if (hero.IsEnemy)
                {
                    EnemyHeroesList.Remove(hero);
                    EnemyList.Remove(hero);
                }
                else
                {
                    AllyHeroesList.Remove(hero);
                    AllyList.Remove(hero);
                }

                return;
            }

            var minion = sender as Obj_AI_Minion;
            if (minion != null && !minion.CharData.BaseSkinName.ToLower().Contains("jarvanivstandard"))
            {
                if (minion.Team != GameObjectTeam.Neutral)
                {
                    if (!minion.CharData.BaseSkinName.Contains("ward") ||
                        !minion.CharData.BaseSkinName.Contains("trinket"))
                    {
                        MinionsList.Remove(minion);
                        if (minion.IsEnemy)
                        {
                            EnemyMinionsList.Remove(minion);
                        }
                        else
                        {
                            AllyMinionsList.Remove(minion);
                        }
                    }
                    else
                    {
                        WardsList.Remove(minion);
                        if (minion.IsEnemy)
                        {
                            EnemyWardsList.Remove(minion);
                        }
                        else
                        {
                            AllyWardsList.Remove(minion);
                        }
                    }

                    if (minion.IsEnemy)
                    {
                        EnemyList.Remove(minion);
                    }
                    else
                    {
                        AllyList.Remove(minion);
                    }
                }
                else if (!minion.CharData.BaseSkinName.ToLower().Contains("barrel"))
                {
                    JungleList.Remove(minion);
                }
                return;
            }

            var inhibitor = sender as Obj_BarracksDampener;
            if (inhibitor != null)
            {
                InhibitorsList.Remove(inhibitor);
                if (inhibitor.IsEnemy)
                {
                    EnemyInhibitorsList.Remove(inhibitor);
                }
                else
                {
                    AllyInhibitorsList.Remove(inhibitor);
                }
                return;
            }

            var turret = sender as Obj_AI_Turret;
            if (turret != null)
            {
                TurretsList.Remove(turret);
                if (turret.IsEnemy)
                {
                    EnemyTurretsList.Remove(turret);
                    EnemyList.Remove(turret);
                }
                else
                {
                    AllyTurretsList.Remove(turret);
                    AllyList.Remove(turret);
                }

                return;
            }

            var shop = sender as Obj_Shop;
            if (shop != null)
            {
                ShopsList.Remove(shop);
                if (shop.IsAlly)
                {
                    AllyShopsList.Remove(shop);
                }
                else
                {
                    EnemyShopsList.Remove(shop);
                }

                return;
            }

            var spawnPoint = sender as Obj_SpawnPoint;
            if (spawnPoint != null)
            {
                SpawnPointsList.Remove(spawnPoint);
                if (spawnPoint.IsAlly)
                {
                    AllySpawnPointsList.Remove(spawnPoint);
                }
                else
                {
                    EnemySpawnPointsList.Remove(spawnPoint);
                }
            }
        }

        /// <summary>
        ///     OnUpdate event.
        /// </summary>
        /// <param name="args">
        ///     The event data
        /// </param>
        private static void OnGameUpdate(EventArgs args)
        {
            foreach (var minion in MinionsList.Where(m => !m.IsValid).ToArray())
            {
                MinionsList.Remove(minion);
                AllyMinionsList.Remove(minion);
                EnemyMinionsList.Remove(minion);
            }

            foreach (var mob in JungleList.Where(m => !m.IsValid).ToArray())
            {
                JungleList.Remove(mob);
            }

            foreach (var ward in WardsList.Where(w => !w.IsValid).ToArray())
            {
                WardsList.Remove(ward);
                AllyWardsList.Remove(ward);
                EnemyWardsList.Remove(ward);
            }

            foreach (var turret in TurretsList.Where(t => !t.IsValid).ToArray())
            {
                TurretsList.Remove(turret);
                AllyTurretsList.Remove(turret);
                EnemyTurretsList.Remove(turret);
            }
        }

        #endregion
    }
}