#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Focus.cs is part of SFXTargetSelector.

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

using LeagueSharp.Common;

#endregion

namespace SFXTargetSelector
{
    public static partial class TargetSelector
    {
        public static partial class Selected
        {
            public static class Focus
            {
                private static bool _force;
                private static bool _enabled = true;

                public static bool Force
                {
                    get { return _force; }
                    set
                    {
                        _force = value;
                        Utils.UpdateMenuItem(Menu, ".force-focus", Enabled);
                    }
                }

                public static bool Enabled
                {
                    get { return _enabled; }
                    set
                    {
                        _enabled = value;
                        Utils.UpdateMenuItem(Menu, ".focus", Enabled);
                    }
                }

                internal static void AddToMainMenu()
                {
                    Menu.AddItem(
                        new MenuItem(Menu.Name + ".focus", "Focus Selected Target").SetShared().SetValue(_enabled))
                        .ValueChanged += (sender, args) => _enabled = args.GetNewValue<bool>();
                    Menu.AddItem(
                        new MenuItem(Menu.Name + ".force-focus", "Only Attack Selected Target").SetShared()
                            .SetValue(_force)).ValueChanged += (sender, args) => _force = args.GetNewValue<bool>();


                    _enabled = Utils.GetMenuItemValue<bool>(Menu, ".focus");
                    _force = Utils.GetMenuItemValue<bool>(Menu, ".force-focus");
                }
            }
        }
    }
}