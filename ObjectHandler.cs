using System;
using System.Collections.Generic;
using System.Linq;

using LeagueSharp;

namespace LeagueSharp.Common
{
    [Obsolete(
        "This will most likely not be needed anymore when Jodus adds it to LeagueSharp.dll," + 
        "only use it if you know what you are doing!", false)]
    public class ObjectHandler
    {
        public static Obj_AI_Hero Player
        {
            get { return ObjectHandler.Player; }
        }

        private static readonly Dictionary<Type, Dictionary<int, GameObject>> gameObjects = new Dictionary<Type, Dictionary<int, GameObject>>();

        static ObjectHandler()
        {
            // All existing objects
            foreach (var obj in ObjectManager.Get<GameObject>())
            {
                var type = obj.GetType();
                if (!gameObjects.ContainsKey(type))
                {
                    gameObjects.Add(type, new Dictionary<int, GameObject>());
                }

                gameObjects[type].Add(obj.NetworkId, obj);
            }

            // Listen to events
            GameObject.OnCreate += Obj_AI_Base_OnCreate;
            GameObject.OnDelete += Obj_AI_Base_OnDelete;
        }

        private static void Obj_AI_Base_OnCreate(GameObject sender, EventArgs args)
        {
            var type = sender.GetType();
            if (!gameObjects.ContainsKey(type))
            {
                gameObjects.Add(type, new Dictionary<int, GameObject>());
            }

            gameObjects[type][sender.NetworkId] = sender;
        }

        private static void Obj_AI_Base_OnDelete(GameObject sender, EventArgs args)
        {
            foreach (var dictionary in gameObjects.Values)
            {
                dictionary.Remove(sender.NetworkId);
            }
        }

        public static GameObjectWrapper<T> Get<T>() where T : GameObject, new()
        {
            var type = typeof(T);
            var found = new GameObjectWrapper<T>();

            foreach(var key in gameObjects.Keys)
            {
                if (type.IsAssignableFrom(key))
                {
                    found.AddRange(gameObjects[key].Values.ToList().ConvertAll(o => (T)o));
                }
            }

            return found;
        }

        public static T GetUnitByNetworkId<T>(int networkId) where T : GameObject, new()
        {
            foreach(var dict in gameObjects.Values)
            {
                if (dict.ContainsKey(networkId))
                {
                    return (T)dict[networkId];
                }
            }

            return null;
        }

        public class GameObjectWrapper<T> : List<T> where T : GameObject, new()
        {
            public List<T> Allies
            {
                get { return base.FindAll(o => o.IsAlly); }
            }

            public List<T> Enemies
            {
                get { return base.FindAll(o => o.IsEnemy); }
            }

            public List<T> Neutrals
            {
                get { return base.FindAll(o => o.Team == GameObjectTeam.Neutral); }
            }
        }
    }
}
