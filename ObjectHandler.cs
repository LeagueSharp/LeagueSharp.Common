using System;
using System.Collections.Generic;

namespace LeagueSharp.Common
{
    [Obsolete(
        "This will most likely not be needed anymore when Jodus adds it to LeagueSharp.dll," +
        "only use it if you know what you are doing!", false)]
    public class ObjectHandler
    {
        private static readonly Dictionary<Type, Dictionary<int, GameObject>> GameObjects =
            new Dictionary<Type, Dictionary<int, GameObject>>();

        static ObjectHandler()
        {
            // All existing objects
            var i = 0;
            foreach (var obj in ObjectManager.Get<GameObject>())
            {
                var type = obj.GetType();
                if (!GameObjects.ContainsKey(type))
                {
                    GameObjects.Add(type, new Dictionary<int, GameObject>());
                }

                var index = obj.NetworkId;
                if (index == 0)
                {
                    index = i;
                    i++;
                }

                GameObjects[type][index] = obj;
            }

            // Listen to events
            GameObject.OnCreate += Obj_AI_Base_OnCreate;
            GameObject.OnDelete += Obj_AI_Base_OnDelete;
        }

        public static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }

        private static void Obj_AI_Base_OnCreate(GameObject sender, EventArgs args)
        {
            var type = sender.GetType();
            if (!GameObjects.ContainsKey(type))
            {
                GameObjects.Add(type, new Dictionary<int, GameObject>());
            }

            GameObjects[type][sender.NetworkId] = sender;
        }

        private static void Obj_AI_Base_OnDelete(GameObject sender, EventArgs args)
        {
            foreach (var dictionary in GameObjects.Values)
            {
                dictionary.Remove(sender.NetworkId);
            }
        }

        public static GameObjectWrapper<T> Get<T>() where T : GameObject, new()
        {
            var type = typeof(T);
            var found = new GameObjectWrapper<T>();

            foreach (var key in GameObjects.Keys)
            {
                if (type.IsAssignableFrom(key))
                {
                    found.AddRange(GameObjects[key].Values.FindAll(o => o.IsValid<T>()).ConvertAll(o => (T)o));
                }
            }

            return found;
        }

        public static T GetUnitByNetworkId<T>(int networkId) where T : GameObject, new()
        {
            foreach (var dict in GameObjects.Values)
            {
                if (dict.ContainsKey(networkId) && dict[networkId].IsValid)
                {
                    return dict[networkId] as T;
                }
            }

            return null;
        }

        public class GameObjectWrapper<T> : List<T> where T : GameObject, new()
        {
            public List<T> Allies
            {
                get { return FindAll(o => o.IsValid<T>() && o.IsAlly); }
            }

            public List<T> Enemies
            {
                get { return FindAll(o => o.IsValid<T>() && o.IsEnemy); }
            }

            public List<T> Neutrals
            {
                get { return FindAll(o => o.IsValid<T>() && o.Team == GameObjectTeam.Neutral); }
            }
        }
    }
}