#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Modes.cs is part of SFXTargetSelector.

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

namespace SFXTargetSelector
{
    public static partial class TargetSelector
    {
        public static partial class Modes
        {
            private static readonly List<Item> PItems;
            private static Item _current;

            static Modes()
            {
                CustomPrefix = "[c]";
                var protectedModes = new List<Item>
                {
                    new Item("weights", "Weights", Weights.OrderChampions) { Mode = Mode.Weights },
                    new Item("priorities", "Priorities", Priorities.OrderChampions) { Mode = Mode.Priorities }
                };

                PItems =
                    protectedModes.Union(
                        new List<Item>
                        {
                            new Item(
                                "less-attacks-to-kill", "Less Attacks To Kill",
                                list => list.OrderBy(x => x.Hero.Health / ObjectManager.Player.TotalAttackDamage))
                            {
                                Mode = Mode.LessAttacksToKill
                            },
                            new Item(
                                "less-cast-priority", "Less Cast Priority",
                                list => list.OrderBy(x => x.Hero.Health / ObjectManager.Player.TotalMagicalDamage))
                            {
                                Mode = Mode.LessCastPriority
                            },
                            new Item(
                                "most-ability-power", "Most Ability Power",
                                list => list.OrderByDescending(x => x.Hero.TotalMagicalDamage))
                            {
                                Mode = Mode.MostAbilityPower
                            },
                            new Item(
                                "most-attack-damage", "Most Attack Damage",
                                list => list.OrderByDescending(x => x.Hero.TotalAttackDamage))
                            {
                                Mode = Mode.MostAttackDamage
                            },
                            new Item(
                                "closest", "Closest", list => list.OrderBy(x => x.Hero.Distance(ObjectManager.Player)))
                            {
                                Mode = Mode.Closest
                            },
                            new Item(
                                "near-mouse", "Near Mouse", list => list.OrderBy(x => x.Hero.Distance(Game.CursorPos)))
                            {
                                Mode = Mode.NearMouse
                            },
                            new Item("least-health", "Least Health", list => list.OrderBy(x => x.Hero.Health))
                            {
                                Mode = Mode.LeastHealth
                            }
                        }).ToList();

                ProtectedModes = protectedModes.AsReadOnly();

                Current = Default;
            }

            public static string CustomPrefix { get; set; }
            public static ReadOnlyCollection<Item> ProtectedModes { get; private set; }

            public static ReadOnlyCollection<Item> Items
            {
                get { return PItems.AsReadOnly(); }
            }

            public static Item Default
            {
                get { return ProtectedModes.FirstOrDefault(); }
            }

            public static Item Current
            {
                get { return _current; }
                set
                {
                    if (value != null && PItems.Any(i => i.UniqueName.Equals(value.UniqueName)))
                    {
                        var raiseEvent = _current == null || !_current.UniqueName.Equals(value.UniqueName);
                        _current = value;
                        if (raiseEvent)
                        {
                            UpdateModeMenu();
                            Utils.RaiseEvent(OnChange, null, new OnChangeArgs(value));
                        }
                    }
                }
            }

            internal static void AddToMainMenu()
            {
                Menu.AddItem(
                    new MenuItem(Menu.Name + ".mode", "Mode").SetShared()
                        .SetValue(new StringList(PItems.Select(i => GetDisplayNamePrefix(i) + i.DisplayName).ToArray())))
                    .ValueChanged +=
                    (sender, args) => Current = GetItemBySelectedIndex(args.GetNewValue<StringList>().SelectedIndex);

                Current = GetItemBySelectedIndex(Utils.GetMenuItemValue<int>(Menu, ".mode", 1));
            }

            private static string GetDisplayNamePrefix(Item item)
            {
                var prefix = string.Empty;
                if (item.Mode == Mode.Custom)
                {
                    prefix += CustomPrefix;
                }
                if (!string.IsNullOrEmpty(prefix))
                {
                    prefix += " ";
                }
                return prefix;
            }

            public static Item GetItem(string name, StringComparison comp = StringComparison.OrdinalIgnoreCase)
            {
                return PItems.FirstOrDefault(w => w.UniqueName.Equals(name, comp));
            }

            private static Item GetItemBySelectedIndex(int index)
            {
                try
                {
                    if (index < PItems.Count && index >= 0)
                    {
                        return PItems[index];
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return Default;
            }

            private static int GetIndexBySelectedItem(Item item)
            {
                try
                {
                    var index = PItems.FindIndex(i => i.UniqueName.Equals(item.UniqueName));
                    if (index >= 0)
                    {
                        return index;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return 0;
            }

            private static void UpdateModeMenu()
            {
                if (Menu != null)
                {
                    var item = Menu.Item(Menu.Name + ".mode");
                    if (item != null)
                    {
                        item.SetShared()
                            .SetValue(
                                new StringList(
                                    PItems.Select(i => GetDisplayNamePrefix(i) + i.DisplayName).ToArray(),
                                    GetIndexBySelectedItem(Current)));
                    }
                }
            }

            public static IEnumerable<Targets.Item> GetOrderedChampions(IEnumerable<Targets.Item> items)
            {
                var targetList = items.ToList();
                try
                {
                    return Current.OrderFunction(targetList);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return targetList;
            }

            public static event EventHandler<OnChangeArgs> OnChange;

            public static void Register(Item item)
            {
                if (!PItems.Any(i => i.UniqueName.Equals(item.UniqueName)) && !string.IsNullOrEmpty(item.DisplayName) &&
                    item.OrderFunction != null)
                {
                    item.Mode = Mode.Custom;
                    PItems.Add(item);
                    UpdateModeMenu();
                }
            }

            public static void Deregister(Item item)
            {
                if (!ProtectedModes.Any(m => m.Equals(item)) && PItems.Any(i => i.UniqueName.Equals(item.UniqueName)))
                {
                    if (Current.Mode.Equals(item.Mode))
                    {
                        Current = Default;
                    }
                    PItems.Remove(item);
                }
            }

            public class Item
            {
                private Mode _mode;
                private Func<IEnumerable<Targets.Item>, IEnumerable<Targets.Item>> _orderFunction;

                public Item(string uniqueName,
                    string displayName,
                    Func<IEnumerable<Targets.Item>, IEnumerable<Targets.Item>> orderFunction)
                {
                    UniqueName = uniqueName;
                    DisplayName = displayName;
                    _mode = Mode.Custom;
                    _orderFunction = orderFunction;
                }

                public string UniqueName { get; private set; }
                public string DisplayName { get; private set; }

                public Mode Mode
                {
                    get { return _mode; }
                    set
                    {
                        if (!IsProtected)
                        {
                            _mode = value;
                        }
                    }
                }

                public Func<IEnumerable<Targets.Item>, IEnumerable<Targets.Item>> OrderFunction
                {
                    get { return _orderFunction; }
                    set
                    {
                        if (!IsProtected)
                        {
                            _orderFunction = value;
                        }
                    }
                }

                public bool IsProtected
                {
                    get { return ProtectedModes != null && ProtectedModes.Contains(this); }
                }
            }
        }
    }
}