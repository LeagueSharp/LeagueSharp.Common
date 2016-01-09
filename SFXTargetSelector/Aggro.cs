#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Aggro.cs is part of SFXTargetSelector.

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

#endregion

namespace SFXTargetSelector
{
    public static partial class TargetSelector
    {
        public static partial class Weights
        {
            public static class Aggro
            {
                private static float _fadeTime = 10;
                private static readonly Dictionary<int, Item> PItems;

                static Aggro()
                {
                    PItems = new Dictionary<int, Item>();
                    Obj_AI_Base.OnAggro += OnObjAiBaseAggro;
                }

                public static ReadOnlyDictionary<int, Item> Items
                {
                    get { return new ReadOnlyDictionary<int, Item>(PItems); }
                }

                public static float FadeTime
                {
                    get { return _fadeTime; }
                    set { _fadeTime = value; }
                }

                public static Item GetSenderTargetItem(Obj_AI_Base sender, Obj_AI_Base target)
                {
                    return
                        GetSenderItems(sender)
                            .FirstOrDefault(entry => entry.Target.Hero.NetworkId.Equals(target.NetworkId));
                }

                public static IEnumerable<Item> GetSenderItems(Obj_AI_Base sender)
                {
                    return PItems.Where(i => i.Key.Equals(sender.NetworkId)).Select(i => i.Value);
                }

                public static IEnumerable<Item> GetTargetItems(Obj_AI_Base target)
                {
                    return PItems.Where(i => i.Value.Target.Hero.NetworkId.Equals(target.NetworkId))
                        .Select(i => i.Value);
                }

                private static void OnObjAiBaseAggro(Obj_AI_Base sender, GameObjectAggroEventArgs args)
                {
                    if (!sender.IsEnemy || Modes.Current.Mode != Mode.Weights)
                    {
                        return;
                    }
                    var hero = sender as Obj_AI_Hero;
                    var target = Targets.Items.FirstOrDefault(h => h.Hero.NetworkId == args.NetworkId);
                    if (hero != null && target != null)
                    {
                        Item aggro;
                        if (PItems.TryGetValue(hero.NetworkId, out aggro))
                        {
                            aggro.Target = target;
                        }
                        else
                        {
                            PItems[target.Hero.NetworkId] = new Item(hero, target);
                        }
                    }
                }

                public class Item
                {
                    public Item(Obj_AI_Hero sender, Targets.Item target)
                    {
                        Sender = sender;
                        Target = target;
                        Timestamp = Game.Time;
                    }

                    public float Value
                    {
                        get { return Math.Max(0f, FadeTime - (Game.Time - Timestamp)); }
                    }

                    public Obj_AI_Hero Sender { get; set; }
                    public Targets.Item Target { get; set; }
                    public float Timestamp { get; private set; }
                }
            }
        }
    }
}