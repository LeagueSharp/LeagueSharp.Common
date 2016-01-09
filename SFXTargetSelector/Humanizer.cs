#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Humanizer.cs is part of SFXTargetSelector.

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
        public static class Humanizer
        {
            public const int MinDelay = 0;
            public const int MaxDelay = 1500;
            private static int _fowDelay = 250;

            public static int FowDelay
            {
                get { return _fowDelay; }
                set
                {
                    _fowDelay = Math.Min(MaxDelay, Math.Max(MinDelay, value));
                    Utils.UpdateMenuItem(Menu, ".fow", _fowDelay);
                }
            }

            internal static void AddToMainMenu()
            {
                Menu.AddItem(
                    new MenuItem(Menu.Name + ".fow", "Target Acquire Delay").SetShared()
                        .SetValue(new Slider(_fowDelay, MinDelay, MaxDelay))).ValueChanged +=
                    (sender, args) => _fowDelay = args.GetNewValue<Slider>().Value;

                _fowDelay = Utils.GetMenuItemValue<int>(Menu, ".fow");
            }

            public static IEnumerable<Targets.Item> FilterTargets(IEnumerable<Targets.Item> targets)
            {
                if (targets == null)
                {
                    return new List<Targets.Item>();
                }
                var finalTargets = targets.ToList();
                if (FowDelay > 0)
                {
                    finalTargets =
                        finalTargets.Where(item => Game.Time - item.LastVisibleChange > FowDelay / 1000f).ToList();
                }
                return finalTargets;
            }
        }
    }
}