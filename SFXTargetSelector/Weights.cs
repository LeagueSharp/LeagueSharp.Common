#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Weights.cs is part of SFXTargetSelector.

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
using ItemData = LeagueSharp.Common.Data.ItemData;

#endregion

/*
 * Don't copy paste this without asking & giving credits fuckers :^) 
 */

namespace SFXTargetSelector
{
    public static partial class TargetSelector
    {
        public static partial class Weights
        {
            public const int MinWeight = 0;
            public const int MaxWeight = 20;
            public const int MinCaching = 0;
            public const int MaxCaching = 3000;
            private static float _range;
            private static readonly List<Item> PItems;
            private static float _maxRange = 2000f;
            private static readonly SimpleCache<float> ItemCache;
            private static readonly SimpleCache<float> LongCache;

            static Weights()
            {
                ItemCache = new SimpleCache<float> { MaxAge = MinCaching };
                LongCache = new SimpleCache<float> { MaxAge = 3000 };
                InvertedPrefix = "[i]";
                CustomPrefix = "[c]";
                PItems = new List<Item>();
                PItems.AddRange(
                    new List<Item>
                    {
                        new Item(
                            "killable", "AA Killable", 20, false,
                            t => t.Health < ObjectManager.Player.GetAutoAttackDamage(t, true) ? 1 : 0,
                            "Checks if target is killable with one auto attack."),
                        new Item(
                            "attack-damage", "Attack Damage", 15, false, delegate(Obj_AI_Hero t)
                            {
                                var ad = t.FlatPhysicalDamageMod;
                                var infinity = LongCache.GetOrAdd(
                                    "infinity-edge-" + t.ChampionName,
                                    () => ItemData.Infinity_Edge.GetItem().IsOwned(t) ? 2.5f : 2);
                                ad += ad / 100 * (t.Crit * 100) * (infinity ?? 1f);
                                var averageArmor = LongCache.GetOrAdd(
                                    "average-armor",
                                    delegate
                                    {
                                        return HeroManager.Allies.Select(a => a.Armor).DefaultIfEmpty(0).Average() *
                                               t.PercentArmorPenetrationMod - t.FlatArmorPenetrationMod;
                                    });
                                return ad * (100 / (100 + (averageArmor ?? 0))) *
                                       (1f / ObjectManager.Player.AttackDelay);
                            }, "AD + Pen + Crit + Speed = Higher Weight"),
                        new Item(
                            "ability-power", "Ability Power", 15, false, delegate(Obj_AI_Hero t)
                            {
                                var averageMr = LongCache.GetOrAdd(
                                    "average-magic-resist",
                                    delegate
                                    {
                                        return
                                            HeroManager.Allies.Select(a => a.SpellBlock).DefaultIfEmpty(0).Average() *
                                            t.PercentMagicPenetrationMod - t.FlatMagicPenetrationMod;
                                    });
                                return t.FlatMagicDamageMod * (100 / (100 + (averageMr ?? 0)));
                            }, "AP + Pen = Higher Weight"),
                        new Item(
                            "low-resists", "Resists", 0, true,
                            t =>
                                ObjectManager.Player.FlatPhysicalDamageMod >= ObjectManager.Player.FlatMagicDamageMod
                                    ? t.Armor
                                    : t.SpellBlock, "Low Armor / Magic-Resist = Higher Weight"),
                        new Item("low-health", "Health", 20, true, t => t.Health, "Low Health = Higher Weight"),
                        new Item(
                            "short-distance-player", "Distance to Player", 5, true,
                            t => t.Distance(ObjectManager.Player), "Close to Player = Higher Weight"),
                        new Item(
                            "short-distance-cursor", "Distance to Cursor", 0, true, t => t.Distance(Game.CursorPos),
                            "Close to Cursor = Higher Weight"),
                        new Item(
                            "crowd-control", "Crowd Control", 0, false, delegate(Obj_AI_Hero t)
                            {
                                var buffs =
                                    t.Buffs.Where(
                                        x =>
                                            x.Type == BuffType.Charm || x.Type == BuffType.Knockback ||
                                            x.Type == BuffType.Suppression || x.Type == BuffType.Fear ||
                                            x.Type == BuffType.Taunt || x.Type == BuffType.Stun ||
                                            x.Type == BuffType.Slow || x.Type == BuffType.Silence ||
                                            x.Type == BuffType.Snare || x.Type == BuffType.Polymorph).ToList();
                                return buffs.Any() ? buffs.Select(x => x.EndTime).DefaultIfEmpty(0).Max() : 0;
                            }, "Checks if the target suffers from any form of crowd control."),
                        new Item(
                            "gold", "Acquired Gold", 0, false,
                            t =>
                                (t.MinionsKilled + t.NeutralMinionsKilled) * 27.35f + t.ChampionsKilled * 300f +
                                t.Assists * 85f, "Calculates the approx. amount of Gold."),
                        new Item(
                            "team-focus", "Team Focus", 0, false,
                            t =>
                                Aggro.Items.Where(a => a.Value.Target.Hero.NetworkId == t.NetworkId)
                                    .Select(a => a.Value.Value)
                                    .DefaultIfEmpty(0)
                                    .Sum(), "Checks who your allies are targeting."),
                        new Item(
                            "focus-me", "Focus Me", 0, false, delegate(Obj_AI_Hero t)
                            {
                                var entry = Aggro.GetSenderTargetItem(t, ObjectManager.Player);
                                return entry != null ? entry.Value : 0;
                            }, "Checks who is targeting you.")
                    });
            }

            public static ReadOnlyCollection<Item> Items
            {
                get { return PItems.AsReadOnly(); }
            }

            public static string InvertedPrefix { get; set; }
            public static string CustomPrefix { get; set; }
            public static Menu Menu { get; private set; }

            public static float Range
            {
                get { return _range; }
                set { _range = Math.Min(MaxRange, value); }
            }

            public static float MaxRange
            {
                get { return _maxRange; }
                set { _maxRange = value; }
            }

            public static int Caching
            {
                get { return ItemCache.MaxAge; }
                set
                {
                    ItemCache.MaxAge = Math.Min(MaxCaching, Math.Max(MinCaching, value));
                    Utils.UpdateMenuItem(Menu, ".caching", ItemCache.MaxAge);
                }
            }

            internal static void AddToMainMenu()
            {
                Menu = TargetSelector.Menu.AddSubMenu(new Menu("Weights", TargetSelector.Menu.Name + ".weights"));

                Heroes.AddToWeightMenu();

                Menu.AddItem(
                    new MenuItem(Menu.Name + ".caching", "Caching %").SetShared()
                        .SetValue(new Slider(ItemCache.MaxAge, MinCaching, MaxCaching)))
                    .SetTooltip("0 = Real Time Calculations | Milliseconds")
                    .ValueChanged += (sender, args) => ItemCache.MaxAge = args.GetNewValue<Slider>().Value;

                foreach (var item in Items)
                {
                    var localItem = item;
                    var weightItem =
                        new MenuItem(Menu.Name + "." + item.UniqueName, GetDisplayNamePrefix(item) + item.DisplayName)
                            .SetShared().SetValue(new Slider(localItem.Weight, MinWeight, MaxWeight));
                    if (!string.IsNullOrWhiteSpace(item.Tooltip))
                    {
                        weightItem.SetTooltip(item.Tooltip);
                    }
                    Menu.AddItem(weightItem);
                    Menu.Item(Menu.Name + "." + item.UniqueName).ValueChanged +=
                        delegate(object sender, OnValueChangeEventArgs args)
                        {
                            localItem.OnWeightChange(args.GetNewValue<Slider>().Value);
                        };
                    item.Weight = TargetSelector.Menu.Item(Menu.Name + "." + item.UniqueName).GetValue<Slider>().Value;
                }

                ItemCache.MaxAge = Utils.GetMenuItemValue<int>(Menu, ".caching");

                Game.OnInput += OnGameInput;
            }

            private static string GetDisplayNamePrefix(Item item)
            {
                var prefix = string.Empty;
                if (item.Custom)
                {
                    prefix += CustomPrefix;
                }
                if (item.Inverted)
                {
                    prefix += InvertedPrefix;
                }
                if (!string.IsNullOrEmpty(prefix))
                {
                    prefix += " ";
                }
                return prefix;
            }

            private static void OnGameInput(GameInputEventArgs args)
            {
                if (args.Input == null)
                {
                    return;
                }
                var input = args.Input.ToLower();
                if (input.Equals("/weights reset", StringComparison.OrdinalIgnoreCase))
                {
                    args.Process = false;
                    RestoreDefaultWeights();
                }
            }

            public static void RestoreDefaultWeights()
            {
                foreach (var item in Items)
                {
                    if (Menu != null)
                    {
                        var menuItem = Menu.Item(Menu.Name + "." + item.UniqueName);
                        if (menuItem != null)
                        {
                            menuItem.SetValue(new Slider(item.DefaultWeight, MinWeight, MaxWeight));
                        }
                    }
                    item.Weight = item.DefaultWeight;
                }
            }

            public static void Deregister(Item item)
            {
                if (Menu != null)
                {
                    var menuItem = Menu.Item(Menu.Name + "." + item.UniqueName);
                    if (menuItem != null)
                    {
                        Menu.Items.Remove(menuItem);
                    }
                }
                PItems.Remove(item);
            }

            public static void Register(Item item)
            {
                if (!Items.Any(i => i.UniqueName.Equals(item.UniqueName)) && !string.IsNullOrEmpty(item.DisplayName) &&
                    item.ValueFunction != null)
                {
                    item.Custom = true;
                    PItems.Add(item);

                    if (Menu != null)
                    {
                        var weightItem =
                            new MenuItem(
                                Menu.Name + "." + item.UniqueName, GetDisplayNamePrefix(item) + item.DisplayName)
                                .SetValue(new Slider(item.Weight, MinWeight, MaxWeight)).SetShared();
                        if (!string.IsNullOrWhiteSpace(item.Tooltip))
                        {
                            weightItem.SetTooltip(item.Tooltip);
                        }
                        Menu.AddItem(weightItem);
                        Menu.Item(Menu.Name + "." + item.UniqueName).ValueChanged +=
                            delegate(object sender, OnValueChangeEventArgs args)
                            {
                                item.OnWeightChange(args.GetNewValue<Slider>().Value);
                            };
                        item.Weight =
                            TargetSelector.Menu.Item(Menu.Name + "." + item.UniqueName).GetValue<Slider>().Value;
                    }
                }
            }

            public static Item GetItem(string uniqueName, StringComparison comp = StringComparison.OrdinalIgnoreCase)
            {
                return PItems.FirstOrDefault(w => w.UniqueName.Equals(uniqueName, comp));
            }

            public static float CalculatedWeight(Item item,
                Targets.Item target,
                bool simulation = false,
                bool forceRealTime = false)
            {
                try
                {
                    var minValue = simulation ? item.SimulationMinValue : item.MinValue;
                    var maxValue = simulation ? item.SimulationMaxValue : item.MaxValue;
                    if (item.Weight <= MinWeight || maxValue <= 0)
                    {
                        return MinWeight;
                    }
                    var minWeight = minValue > 0 ? item.Weight / (maxValue / minValue) : MinWeight;
                    var weight = item.Inverted
                        ? item.Weight - item.Weight * GetValue(item, target, forceRealTime) / maxValue + minWeight
                        : item.Weight * GetValue(item, target, forceRealTime) / maxValue;
                    return float.IsNaN(weight) || float.IsInfinity(weight)
                        ? MinWeight
                        : Math.Min(MaxWeight, Math.Min(item.Weight, Math.Max(MinWeight, Math.Max(weight, minWeight))));
                }
                catch
                {
                    // Ignored
                }
                return MinWeight;
            }

            public static float GetValue(Item item, Targets.Item target, bool forceRealTime = false)
            {
                try
                {
                    if (ItemCache.MaxAge > 0 && !forceRealTime)
                    {
                        var cacheValue = ItemCache[item.UniqueName];
                        if (cacheValue != null)
                        {
                            return (float) cacheValue;
                        }
                    }
                    var value = item.ValueFunction(target.Hero);
                    value = Math.Max(0, value);
                    ItemCache.AddOrUpdate(item.UniqueName, value);
                    return value;
                }
                catch
                {
                    return item.Inverted ? item.MaxValue : item.MinValue;
                }
            }

            internal static void UpdateMaxMinValue(Item item,
                IEnumerable<Targets.Item> targets,
                bool simulation = false,
                bool forceRealTime = false)
            {
                var min = float.MaxValue;
                var max = float.MinValue;
                foreach (var target in targets)
                {
                    var value = GetValue(item, target, forceRealTime);
                    if (value < min)
                    {
                        min = value;
                    }
                    if (value > max)
                    {
                        max = value;
                    }
                }
                if (!simulation)
                {
                    item.MinValue = Math.Max(0, min);
                    item.MaxValue = max > min ? max : min;
                }
                else
                {
                    item.SimulationMinValue = Math.Max(0, min);
                    item.SimulationMaxValue = max > min ? max : min;
                }
            }

            public static IEnumerable<Targets.Item> OrderChampions(IEnumerable<Targets.Item> targets)
            {
                if (targets == null)
                {
                    return new List<Targets.Item>();
                }
                var targetList = targets.ToList();

                foreach (var item in Items.Where(w => w.Weight > 0))
                {
                    UpdateMaxMinValue(item, targetList);
                }

                foreach (var target in targetList)
                {
                    target.Weight = Items.Where(w => w.Weight > 0).Sum(w => CalculatedWeight(w, target)) / 100 *
                                    Heroes.GetPercentage(target.Hero);
                }

                return Selected.Focus.Enabled && Selected.Focus.Force && targetList.Count > 1
                    ? new List<Targets.Item> { targetList.OrderByDescending(t => t.Weight).First() }
                    : targetList.OrderByDescending(t => t.Weight).ToList();
            }

            public class Heroes
            {
                public const int MinPercentage = 0;
                public const int MaxPercentage = 200;
                public const int DefaultPercentage = 100;
                private static readonly Dictionary<int, int> Percentages = new Dictionary<int, int>();
                public static Menu Menu { get; private set; }

                internal static void AddToWeightMenu()
                {
                    Menu = Weights.Menu.AddSubMenu(new Menu("Heroes Percentage", Weights.Menu.Name + ".heroes"));

                    foreach (var enemy in Targets.Items)
                    {
                        Menu.AddItem(
                            new MenuItem(Menu.Name + "." + enemy.Hero.ChampionName, enemy.Hero.ChampionName).SetValue(
                                new Slider(
                                    Percentages.ContainsKey(enemy.Hero.NetworkId)
                                        ? Percentages[enemy.Hero.NetworkId]
                                        : DefaultPercentage, MinPercentage, MaxPercentage)).DontSave());
                    }
                }

                public static int GetPercentage(Obj_AI_Hero hero)
                {
                    if (hero != null)
                    {
                        if (Menu != null)
                        {
                            var item = Menu.Item(Menu.Name + "." + hero.ChampionName);
                            if (item != null)
                            {
                                return item.GetValue<Slider>().Value;
                            }
                        }
                        return Percentages.ContainsKey(hero.NetworkId) ? Percentages[hero.NetworkId] : DefaultPercentage;
                    }
                    return DefaultPercentage;
                }

                public static void SetPercentage(Obj_AI_Hero hero, int percentage)
                {
                    percentage = Math.Max(MinPercentage, Math.Min(MaxPercentage, percentage));
                    if (hero != null)
                    {
                        if (Menu != null)
                        {
                            var item = Menu.Item(Menu.Name + "." + hero.ChampionName);
                            if (item != null)
                            {
                                item.SetValue(new Slider(percentage, MinPercentage, MaxPercentage));
                            }
                        }
                        Percentages[hero.NetworkId] = percentage;
                    }
                }
            }

            public class Item
            {
                private string _displayName;
                private string _tooltip;
                private int _weight;

                public Item(string uniqueName,
                    string displayName,
                    int weight,
                    bool inverted,
                    Func<Obj_AI_Hero, float> valueFunction,
                    string tooltip = null)
                {
                    ValueFunction = valueFunction;
                    UniqueName = uniqueName;
                    DisplayName = displayName;
                    Weight = weight;
                    DefaultWeight = weight;
                    Inverted = inverted;
                    Tooltip = tooltip;
                }

                public Func<Obj_AI_Hero, float> ValueFunction { get; set; }
                public string UniqueName { get; private set; }

                public string DisplayName
                {
                    get { return _displayName; }
                    set
                    {
                        _displayName = value;
                        if (Menu != null)
                        {
                            var item = TargetSelector.Menu.Item(Menu.Name + "." + UniqueName);
                            if (item != null)
                            {
                                item.DisplayName = _displayName;
                            }
                        }
                    }
                }

                public string Tooltip
                {
                    get { return _tooltip; }
                    set
                    {
                        _tooltip = value;
                        if (Menu != null)
                        {
                            var item = TargetSelector.Menu.Item(Menu.Name + "." + UniqueName);
                            if (item != null)
                            {
                                item.SetTooltip(_tooltip);
                            }
                        }
                    }
                }

                public int Weight
                {
                    get { return _weight; }
                    set
                    {
                        OnWeightChange(value);
                        Utils.UpdateMenuItem(Menu, "." + UniqueName, _weight);
                    }
                }

                public bool Custom { get; internal set; }
                public int DefaultWeight { get; private set; }
                public bool Inverted { get; set; }
                public float MaxValue { get; internal set; }
                public float MinValue { get; internal set; }
                public float SimulationMaxValue { get; internal set; }
                public float SimulationMinValue { get; internal set; }

                internal void OnWeightChange(int newWeight)
                {
                    _weight = Math.Min(MaxWeight, Math.Max(MinWeight, newWeight));
                }
            }
        }
    }
}