#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Invulnerable.cs is part of SFXTargetSelector.

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
using System.Collections.ObjectModel;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace SFXTargetSelector.Others
{
    public class Invulnerable
    {
        private static readonly List<Item> PItems;

        static Invulnerable()
        {
            PItems = new List<Item>
            {
                new Item(
                    "Alistar", "FerociousHowl", null, false, -1,
                    (target, type) =>
                        ObjectManager.Player.CountEnemiesInRange(Orbwalking.GetRealAutoAttackRange(target)) > 1),
                new Item(
                    "MasterYi", "Meditate", null, false, -1,
                    (target, type) =>
                        ObjectManager.Player.CountEnemiesInRange(Orbwalking.GetRealAutoAttackRange(target)) > 1),
                new Item("Tryndamere", "UndyingRage", null, false, 1, (target, type) => target.HealthPercent <= 5),
                new Item("Kayle", "JudicatorIntervention", null, false),
                new Item("Fizz", "fizztrickslamsounddummy", null, false),
                new Item("Vladimir", "VladimirSanguinePool", null, false),
                new Item("Fiora", "FioraW", null, false),
                new Item("Jax", "JaxCounterStrike", DamageType.Physical, false),
                new Item(null, "BlackShield", DamageType.Magical, true),
                new Item(null, "BansheesVeil", DamageType.Magical, true),
                new Item(null, "KindredrNoDeathBuff", null, false, 10, (target, type) => target.HealthPercent <= 10),
                new Item("Sivir", "SivirE", null, true),
                new Item("Nocturne", "ShroudofDarkness", null, true)
            };
        }

        public static ReadOnlyCollection<Item> Items
        {
            get { return PItems.AsReadOnly(); }
        }

        public static bool Check(Obj_AI_Hero target, DamageType damageType = DamageType.True, bool ignoreShields = true)
        {
            return target.HasBuffOfType(BuffType.Invulnerability) || target.IsInvulnerable ||
                   (from invulnerable in Items
                       where invulnerable.Champion == null || invulnerable.Champion == target.ChampionName
                       where invulnerable.DamageType == null || invulnerable.DamageType == damageType
                       where target.HasBuff(invulnerable.BuffName)
                       where !ignoreShields || !invulnerable.IsShield
                       select invulnerable).Any(
                           invulnerable =>
                               invulnerable.CheckFunction == null || CheckFunction(invulnerable, target, damageType));
        }

        public static bool Check(Obj_AI_Hero target,
            float damage,
            DamageType damageType = DamageType.True,
            bool ignoreShields = true)
        {
            if (target.HasBuffOfType(BuffType.Invulnerability) || target.IsInvulnerable)
            {
                return true;
            }
            foreach (var invulnerable in Items)
            {
                if (invulnerable.Champion == null || invulnerable.Champion == target.ChampionName)
                {
                    if (invulnerable.DamageType == null || invulnerable.DamageType == damageType)
                    {
                        if (target.HasBuff(invulnerable.BuffName))
                        {
                            if (!ignoreShields || !invulnerable.IsShield)
                            {
                                if (invulnerable.CheckFunction == null ||
                                    CheckFunction(invulnerable, target, damageType))
                                {
                                    if (invulnerable.MinHealthPercent > 0 &&
                                        (target.Health - damage) / target.MaxHealth * 100 <
                                        invulnerable.MinHealthPercent)
                                    {
                                        return true;
                                    }
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static Item GetItem(string buffName, StringComparison comp = StringComparison.OrdinalIgnoreCase)
        {
            return PItems.FirstOrDefault(w => w.BuffName.Equals(buffName, comp));
        }

        public static void Register(Item item)
        {
            if (!PItems.Any(i => i.BuffName.Equals(item.BuffName)))
            {
                PItems.Add(item);
            }
        }

        public static void Deregister(Item item)
        {
            if (PItems.Any(i => i.BuffName.Equals(item.BuffName)))
            {
                PItems.Remove(item);
            }
        }

        private static bool CheckFunction(Item invulnerable, Obj_AI_Hero target, DamageType damageType)
        {
            try
            {
                return invulnerable != null && invulnerable.CheckFunction(target, damageType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        public class Item
        {
            public Item(string champion,
                string buffName,
                DamageType? damageType,
                bool isShield,
                int minHealthPercent = -1,
                Func<Obj_AI_Base, DamageType, bool> checkFunction = null)
            {
                Champion = champion;
                BuffName = buffName;
                DamageType = damageType;
                IsShield = isShield;
                MinHealthPercent = minHealthPercent;
                CheckFunction = checkFunction;
            }

            public string Champion { get; set; }
            public string BuffName { get; set; }
            public DamageType? DamageType { get; set; }
            public bool IsShield { get; private set; }
            public int MinHealthPercent { get; set; }
            public Func<Obj_AI_Base, DamageType, bool> CheckFunction { get; set; }
        }
    }
}