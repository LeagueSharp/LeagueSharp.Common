#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Utils.cs is part of SFXTargetSelector.

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
using LeagueSharp;
using LeagueSharp.Common;
using SFXTargetSelector.Others;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace SFXTargetSelector
{
    internal class Utils
    {
        internal static void RaiseEvent<T>(EventHandler<T> @event, object sender, T e) where T : EventArgs
        {
            try
            {
                if (@event != null)
                {
                    @event(sender, e);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        internal static bool IsValidTarget(Obj_AI_Hero target,
            float range,
            DamageType damageType,
            bool ignoreShields = true,
            Vector3 from = default(Vector3))
        {
            return target.IsValidTarget() &&
                   target.Distance(@from.Equals(default(Vector3)) ? ObjectManager.Player.ServerPosition : @from, true) <
                   Math.Pow(range <= 0 ? Orbwalking.GetRealAutoAttackRange(target) : range, 2) &&
                   !Invulnerable.Check(target, damageType, ignoreShields);
        }

        internal static DamageType ConvertDamageType(LeagueSharp.Common.TargetSelector.DamageType damageType)
        {
            return damageType == LeagueSharp.Common.TargetSelector.DamageType.True
                ? DamageType.True
                : (damageType == LeagueSharp.Common.TargetSelector.DamageType.Physical
                    ? DamageType.Physical
                    : (damageType == LeagueSharp.Common.TargetSelector.DamageType.Magical
                        ? DamageType.Magical
                        : DamageType.Mixed));
        }

        internal static T GetMenuItemValue<T>(Menu menu, string name, int param = -1)
        {
            var defaultValue = default(T);
            if (menu != null)
            {
                var item = menu.Item(menu.Name + name);
                if (item != null)
                {
                    var oldValue = item.GetValue<object>();
                    if (oldValue is bool)
                    {
                        return (T) oldValue;
                    }
                    if (oldValue is Color)
                    {
                        return (T) oldValue;
                    }
                    if (oldValue is Circle)
                    {
                        var circle = (Circle) oldValue;
                        if (defaultValue is bool)
                        {
                            return (T) (object) circle.Active;
                        }
                        if (defaultValue is Color)
                        {
                            return (T) (object) circle.Color;
                        }
                        if (defaultValue is float)
                        {
                            return (T) (object) circle.Radius;
                        }
                    }
                    if (oldValue is Slider)
                    {
                        var slider = (Slider) oldValue;
                        if (param >= 0)
                        {
                            switch (param)
                            {
                                case 0:
                                    return (T) (object) slider.Value;
                                case 1:
                                    return (T) (object) slider.MinValue;
                                case 2:
                                    return (T) (object) slider.MaxValue;
                            }
                        }
                        return (T) (object) slider.Value;
                    }
                    if (oldValue is KeyBind)
                    {
                        var keybind = (KeyBind) oldValue;
                        if (param >= 0)
                        {
                            switch (param)
                            {
                                case 0:
                                    return (T) (object) keybind.Key;
                                case 1:
                                    return (T) (object) keybind.Type;
                                case 2:
                                    return (T) (object) keybind.Active;
                            }
                        }
                        return (T) (object) keybind.Key;
                    }
                    if (oldValue is StringList)
                    {
                        var stringList = (StringList) oldValue;
                        if (param >= 0)
                        {
                            switch (param)
                            {
                                case 0:
                                    return (T) (object) stringList.SList;
                                case 1:
                                    return (T) (object) stringList.SelectedIndex;
                            }
                        }
                        return (T) (object) stringList.SList;
                    }
                }
            }
            return default(T);
        }

        internal static void UpdateMenuItem(Menu menu,
            string name,
            object param1,
            object param2 = null,
            object param3 = null)
        {
            if (menu != null)
            {
                var item = menu.Item(menu.Name + name);
                if (item != null)
                {
                    var oldValue = item.GetValue<object>();
                    if (oldValue is bool)
                    {
                        var oBool = (bool) oldValue;
                        if (param1 == null)
                        {
                            param1 = oBool;
                        }
                        var newValue = (bool) param1;
                        if (newValue != oBool)
                        {
                            item.SetValue(newValue);
                        }
                    }
                    if (oldValue is Color)
                    {
                        var color = (Color) oldValue;
                        if (param1 == null)
                        {
                            param1 = color;
                        }
                        var newValue = (Color) param1;
                        if (!newValue.Equals(color))
                        {
                            item.SetValue(newValue);
                        }
                    }
                    if (oldValue is Circle)
                    {
                        var circle = (Circle) oldValue;
                        if (param1 == null)
                        {
                            param1 = circle.Active;
                        }
                        if (param2 == null)
                        {
                            param2 = circle.Color;
                        }
                        if (param3 == null)
                        {
                            param3 = circle.Radius;
                        }
                        var newValue1 = (bool) param1;
                        var newValue2 = (Color) param2;
                        var newValue3 = (float) param3;
                        if (newValue1 != circle.Active || !newValue2.Equals(circle.Color) ||
                            Math.Abs(newValue3 - circle.Radius) > 0)
                        {
                            item.SetValue(new Circle((bool) param1, (Color) param2, (float) param3));
                        }
                    }
                    if (oldValue is Slider)
                    {
                        var slider = (Slider) oldValue;
                        if (param1 == null)
                        {
                            param1 = slider.Value;
                        }
                        if (param2 == null)
                        {
                            param2 = slider.MinValue;
                        }
                        if (param3 == null)
                        {
                            param3 = slider.MaxValue;
                        }
                        var newValue1 = (int) param1;
                        var newValue2 = (int) param2;
                        var newValue3 = (int) param3;
                        if (newValue1 != slider.Value || newValue2 != slider.MinValue || newValue3 != slider.MaxValue)
                        {
                            item.SetValue(new Slider((int) param1, (int) param2, (int) param3));
                        }
                    }
                    if (oldValue is KeyBind)
                    {
                        var keybind = (KeyBind) oldValue;
                        if (param1 == null)
                        {
                            param1 = keybind.Key;
                        }
                        if (param2 == null)
                        {
                            param2 = keybind.Type;
                        }
                        if (param3 == null)
                        {
                            param3 = keybind.Active;
                        }
                        var newValue1 = (uint) param1;
                        var newValue2 = (KeyBindType) param2;
                        var newValue3 = (bool) param3;
                        if (newValue1 != keybind.Key || newValue2 != keybind.Type || newValue3 != keybind.Active)
                        {
                            item.SetValue(new KeyBind((uint) param1, (KeyBindType) param2, (bool) param3));
                        }
                    }
                    if (oldValue is StringList)
                    {
                        var stringList = (StringList) oldValue;
                        if (param1 == null)
                        {
                            param1 = stringList.SList;
                        }
                        if (param2 == null)
                        {
                            param2 = stringList.SelectedIndex;
                        }
                        var newValue1 = (string[]) param1;
                        var newValue2 = (int) param2;
                        if (!newValue1.Equals(stringList.SList) || newValue2 != stringList.SelectedIndex)
                        {
                            item.SetValue(new StringList((string[]) param1, (int) param2));
                        }
                    }
                }
            }
        }
    }
}