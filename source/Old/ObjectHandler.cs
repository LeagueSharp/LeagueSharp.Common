namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Provides cached game objects.
    /// </summary>
    [Obsolete("Cache enforced in LeagueSharp Core, class obsolete and prone to delete.", false)]
    public class ObjectHandler
    {
        #region Static Fields

        /// <summary>
        ///     The game objects
        /// </summary>
        private static readonly Dictionary<Type, Dictionary<int, GameObject>> gameObjects =
            new Dictionary<Type, Dictionary<int, GameObject>>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="ObjectHandler" /> class.
        /// </summary>
        static ObjectHandler()
        {
            // All existing objects
            var i = 0;
            foreach (var obj in ObjectManager.Get<GameObject>())
            {
                var type = obj.GetType();
                if (!gameObjects.ContainsKey(type))
                {
                    gameObjects.Add(type, new Dictionary<int, GameObject>());
                }

                var index = obj.NetworkId;
                if (index == 0)
                {
                    index = i;
                    i++;
                }

                gameObjects[type][index] = obj;
            }

            // Listen to events
            GameObject.OnCreate += Obj_AI_Base_OnCreate;
            GameObject.OnDelete += Obj_AI_Base_OnDelete;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the player.
        /// </summary>
        /// <value>The player.</value>
        public static Obj_AI_Hero Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets all of the game objects of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>GameObjectWrapper&lt;T&gt;.</returns>
        public static GameObjectWrapper<T> Get<T>() where T : GameObject, new()
        {
            var type = typeof(T);
            var found = new GameObjectWrapper<T>();

            foreach (var key in gameObjects.Keys)
            {
                if (type.IsAssignableFrom(key))
                {
                    found.AddRange(gameObjects[key].Values.Where(o => o.IsValid<T>()).ToList().ConvertAll(o => (T)o));
                }
            }

            return found;
        }

        /// <summary>
        ///     Gets the unit by network identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="networkId">The network identifier.</param>
        /// <returns>T.</returns>
        public static T GetUnitByNetworkId<T>(int networkId) where T : GameObject, new()
        {
            foreach (var dict in gameObjects.Values)
            {
                if (dict.ContainsKey(networkId) && dict[networkId].IsValid)
                {
                    return dict[networkId] as T;
                }
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Fired when a <see cref="Obj_AI_Base" /> is created.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void Obj_AI_Base_OnCreate(GameObject sender, EventArgs args)
        {
            var type = sender.GetType();
            if (!gameObjects.ContainsKey(type))
            {
                gameObjects.Add(type, new Dictionary<int, GameObject>());
            }

            gameObjects[type][sender.NetworkId] = sender;
        }

        /// <summary>
        ///     Fired when a <see cref="Obj_AI_Base" /> is deleted.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void Obj_AI_Base_OnDelete(GameObject sender, EventArgs args)
        {
            foreach (var dictionary in gameObjects.Values)
            {
                dictionary.Remove(sender.NetworkId);
            }
        }

        #endregion

        /// <summary>
        ///     A wrapper around a <see cref="List{T}" /> that provides extensions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class GameObjectWrapper<T> : List<T>
            where T : GameObject, new()
        {
            #region Public Properties

            /// <summary>
            ///     Gets the allies.
            /// </summary>
            /// <value>The allies.</value>
            public List<T> Allies
            {
                get
                {
                    return this.FindAll(o => o.IsValid<T>() && o.IsAlly);
                }
            }

            /// <summary>
            ///     Gets the enemies.
            /// </summary>
            /// <value>The enemies.</value>
            public List<T> Enemies
            {
                get
                {
                    return this.FindAll(o => o.IsValid<T>() && o.IsEnemy);
                }
            }

            /// <summary>
            ///     Gets the neutrals.
            /// </summary>
            /// <value>The neutrals.</value>
            public List<T> Neutrals
            {
                get
                {
                    return this.FindAll(o => o.IsValid<T>() && o.Team == GameObjectTeam.Neutral);
                }
            }

            #endregion
        }
    }
}