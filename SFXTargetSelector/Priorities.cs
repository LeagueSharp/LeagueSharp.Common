#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Priorities.cs is part of SFXTargetSelector.

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

namespace SFXTargetSelector
{
    public static partial class TargetSelector
    {
        public static partial class Priorities
        {
            public const int MinPriority = 1;
            public const int MaxPriority = 5;
            private static bool _autoPriority;
            private static readonly Dictionary<int, int> _priorities;

            static Priorities()
            {
                _priorities = new Dictionary<int, int>();
                Items = new HashSet<Item>
                {
                    new Item
                    {
                        Champions =
                            new[]
                            {
                                "Ahri", "Anivia", "Annie", "Ashe", "Azir", "Brand", "Caitlyn", "Cassiopeia", "Corki",
                                "Draven", "Ezreal", "Graves", "Jinx", "Kalista", "Karma", "Karthus", "Katarina",
                                "Kennen", "KogMaw", "Leblanc", "Lucian", "Lux", "Malzahar", "MasterYi", "MissFortune",
                                "Orianna", "Quinn", "Sivir", "Syndra", "Talon", "Teemo", "Tristana", "TwistedFate",
                                "Twitch", "Varus", "Vayne", "Veigar", "Velkoz", "Viktor", "Xerath", "Zed", "Ziggs"
                            },
                        Priority = Priority.Highest
                    },
                    new Item
                    {
                        Champions =
                            new[]
                            {
                                "Akali", "Diana", "Ekko", "Fiddlesticks", "Fiora", "Fizz", "Heimerdinger", "Illaoi",
                                "Jayce", "Kassadin", "Kayle", "Kha'Zix", "Kindred", "Lissandra", "Mordekaiser",
                                "Nidalee", "Riven", "Shaco", "Vladimir", "Yasuo", "Zilean"
                            },
                        Priority = Priority.High
                    },
                    new Item
                    {
                        Champions =
                            new[]
                            {
                                "Aatrox", "Darius", "Elise", "Evelynn", "Galio", "Gangplank", "Gragas", "Irelia", "Jax",
                                "Lee Sin", "Maokai", "Morgana", "Nocturne", "Pantheon", "Poppy", "Rengar", "Rumble",
                                "Ryze", "Swain", "Trundle", "Tryndamere", "Udyr", "Urgot", "Vi", "XinZhao", "RekSai"
                            },
                        Priority = Priority.Medium
                    },
                    new Item
                    {
                        Champions =
                            new[]
                            {
                                "Alistar", "Amumu", "Bard", "Blitzcrank", "Braum", "Cho'Gath", "Dr. Mundo", "Garen",
                                "Gnar", "Hecarim", "Janna", "Jarvan IV", "Leona", "Lulu", "Malphite", "Nami", "Nasus",
                                "Nautilus", "Nunu", "Olaf", "Rammus", "Renekton", "Sejuani", "Shen", "Shyvana", "Singed",
                                "Sion", "Skarner", "Sona", "Soraka", "TahmKench", "Taric", "Thresh", "Volibear",
                                "Warwick", "MonkeyKing", "Yorick", "Zac", "Zyra"
                            },
                        Priority = Priority.Low
                    }
                };
            }

            public static Menu Menu { get; private set; }
            public static HashSet<Item> Items { get; private set; }

            public static bool AutoPriority
            {
                get { return _autoPriority; }
                set
                {
                    _autoPriority = value;
                    Utils.UpdateMenuItem(Menu, ".auto-priority", _autoPriority);
                }
            }

            internal static void AddToMainMenu()
            {
                Menu = TargetSelector.Menu.AddSubMenu(new Menu("Priorities", TargetSelector.Menu.Name + ".priorities"));

                var autoPriority =
                    new MenuItem(Menu.Name + ".auto-priority", "Auto Priority").SetShared()
                        .SetValue(_autoPriority)
                        .SetTooltip("5 = Highest Priority");

                foreach (var enemy in Targets.Items)
                {
                    var item =
                        new MenuItem(Menu.Name + "." + enemy.Hero.ChampionName, enemy.Hero.ChampionName).SetShared()
                            .SetValue(
                                new Slider(
                                    _priorities.ContainsKey(enemy.Hero.NetworkId)
                                        ? _priorities[enemy.Hero.NetworkId]
                                        : MinPriority, MinPriority, MaxPriority));
                    Menu.AddItem(item);
                    if (autoPriority.GetValue<bool>())
                    {
                        item.SetShared()
                            .SetValue(new Slider((int) GetDefaultPriority(enemy.Hero), MinPriority, MaxPriority));
                    }
                }

                Menu.AddItem(autoPriority).ValueChanged += delegate(object sender, OnValueChangeEventArgs args)
                {
                    _autoPriority = args.GetNewValue<bool>();
                    if (_autoPriority)
                    {
                        foreach (var enemy in Targets.Items)
                        {
                            Menu.Item(Menu.Name + "." + enemy.Hero.ChampionName)
                                .SetShared()
                                .SetValue(new Slider((int) GetDefaultPriority(enemy.Hero), MinPriority, MaxPriority));
                        }
                    }
                };

                _autoPriority = Utils.GetMenuItemValue<bool>(Menu, ".auto-priority");
            }

            public static Priority GetDefaultPriority(Obj_AI_Hero hero)
            {
                var item = Items.FirstOrDefault(i => i.Champions.Contains(hero.ChampionName));
                if (item != null)
                {
                    return item.Priority;
                }
                return Priority.Low;
            }

            public static int GetPriority(Obj_AI_Hero hero)
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
                    return _priorities.ContainsKey(hero.NetworkId)
                        ? _priorities[hero.NetworkId]
                        : (int) GetDefaultPriority(hero);
                }
                return (int) Priority.Low;
            }

            public static void SetPriority(Obj_AI_Hero hero, int value)
            {
                value = Math.Max(MinPriority, Math.Min(MaxPriority, value));
                if (hero != null)
                {
                    if (Menu != null)
                    {
                        var item = Menu.Item(Menu.Name + "." + hero.ChampionName);
                        if (item != null)
                        {
                            item.SetValue(new Slider(value, MinPriority, MaxPriority));
                        }
                    }
                    _priorities[hero.NetworkId] = value;
                }
            }

            public static void SetPriority(Obj_AI_Hero hero, Priority type)
            {
                SetPriority(hero, (int) type);
            }

            public static IEnumerable<Targets.Item> OrderChampions(IEnumerable<Targets.Item> targets)
            {
                if (targets == null)
                {
                    return new List<Targets.Item>();
                }
                return targets.OrderByDescending(x => GetPriority(x.Hero));
            }

            public class Item
            {
                public Priority Priority { get; set; }
                public string[] Champions { get; set; }
            }
        }
    }
}