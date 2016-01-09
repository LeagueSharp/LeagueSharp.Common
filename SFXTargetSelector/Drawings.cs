#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Drawings.cs is part of SFXTargetSelector.

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
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace SFXTargetSelector
{
    public static partial class TargetSelector
    {
        public static class Drawings
        {
            public const int MinCircleThickness = 1;
            public const int MaxCircleThickness = 10;
            private static int _circleThickness = 5;
            private static bool _initialized;
            public static Menu Menu { get; private set; }

            public static int CircleThickness
            {
                get { return _circleThickness; }
                set
                {
                    _circleThickness = Math.Min(MaxCircleThickness, Math.Max(MinCircleThickness, value));
                    Utils.UpdateMenuItem(Menu, ".circle-thickness", _circleThickness);
                }
            }

            internal static void Initialize()
            {
                if (!_initialized)
                {
                    _initialized = true;
                    Drawing.OnDraw += OnDrawingDraw;
                }
            }

            internal static void AddToMainMenu()
            {
                Initialize();
                Menu = TargetSelector.Menu.AddSubMenu(new Menu("Drawings", TargetSelector.Menu.Name + ".drawing"));
                Menu.AddItem(
                    new MenuItem(Menu.Name + ".circle-thickness", "Circle Thickness").SetShared()
                        .SetValue(new Slider(_circleThickness, MinCircleThickness, MaxCircleThickness))).ValueChanged +=
                    (sender, args) => _circleThickness = args.GetNewValue<Slider>().Value;

                Selected.AddToDrawingMenu();
                Weights.AddToDrawingMenu();

                _circleThickness = Utils.GetMenuItemValue<int>(Menu, ".circle-thickness");
            }

            private static void OnDrawingDraw(EventArgs args)
            {
                if (Selected.Enabled && TargetSelector.Selected.Target != null &&
                    TargetSelector.Selected.Target.IsValidTarget() &&
                    TargetSelector.Selected.Target.Position.IsOnScreen() && TargetSelector.Selected.Focus.Enabled)
                {
                    if (Selected.Enabled)
                    {
                        Render.Circle.DrawCircle(
                            TargetSelector.Selected.Target.Position,
                            TargetSelector.Selected.Target.BoundingRadius + Selected.Radius, Selected.Color,
                            CircleThickness, true);
                    }
                }

                if (Modes.Current.Mode == Mode.Weights && (Weights.Simple || Weights.BestTarget.Enabled))
                {
                    if (Weights.Simple)
                    {
                        foreach (var target in
                            Weights.DrawingTargets.Where(
                                target =>
                                    !target.Hero.IsDead && target.Hero.IsVisible && target.Hero.Position.IsOnScreen()))
                        {
                            Drawing.DrawText(
                                target.Hero.HPBarPosition.X + 55f, target.Hero.HPBarPosition.Y - 20f, Color.White,
                                target.SimulatedWeight.ToString("0.0").Replace(",", "."));
                        }
                    }
                    if (Weights.BestTarget.Enabled && Weights.BestTarget.Unit != null &&
                        !Weights.BestTarget.Unit.Hero.IsDead && Weights.BestTarget.Unit.Hero.IsVisible &&
                        Weights.DrawingTargets.Count(
                            e => !e.Hero.IsDead && e.Hero.IsVisible && e.Hero.Position.IsOnScreen()) >= 2)
                    {
                        Render.Circle.DrawCircle(
                            Weights.BestTarget.Unit.Hero.Position,
                            Weights.BestTarget.Unit.Hero.BoundingRadius + Weights.BestTarget.Radius,
                            Weights.BestTarget.Color, CircleThickness, true);
                    }
                }
            }

            public static class Selected
            {
                public const int MinRadius = 0;
                public const int MaxRadius = 100;
                private static Color _color = Color.Yellow;
                private static int _radius = 35;
                private static bool _enabled = true;
                public static Menu Menu { get; private set; }

                public static Color Color
                {
                    get { return _color; }
                    set
                    {
                        _color = value;
                        Utils.UpdateMenuItem(Menu, ".color", _color);
                    }
                }

                public static int Radius
                {
                    get { return _radius; }
                    set
                    {
                        _radius = Math.Min(MaxRadius, Math.Max(MinRadius, value));
                        Utils.UpdateMenuItem(Menu, ".radius", _radius);
                    }
                }

                public static bool Enabled
                {
                    get { return _enabled; }
                    set
                    {
                        _enabled = value;
                        Utils.UpdateMenuItem(Menu, ".enabled", _enabled);
                    }
                }

                internal static void AddToDrawingMenu()
                {
                    Menu = Drawings.Menu.AddSubMenu(new Menu("Selected Target", Drawings.Menu.Name + ".selected"));
                    Menu.AddItem(new MenuItem(Menu.Name + ".color", "Color").SetShared().SetValue(_color)).ValueChanged
                        += (sender, args) => _color = args.GetNewValue<Color>();
                    Menu.AddItem(
                        new MenuItem(Menu.Name + ".radius", "Radius").SetShared().SetValue(new Slider(_radius)))
                        .ValueChanged += (sender, args) => _radius = args.GetNewValue<Slider>().Value;
                    Menu.AddItem(new MenuItem(Menu.Name + ".enabled", "Enabled").SetShared().SetValue(_enabled))
                        .ValueChanged += (sender, args) => _enabled = args.GetNewValue<bool>();

                    _color = Utils.GetMenuItemValue<Color>(Menu, ".color");
                    _radius = Utils.GetMenuItemValue<int>(Menu, ".radius");
                    _enabled = Utils.GetMenuItemValue<bool>(Menu, ".enabled");
                }
            }

            public static class Weights
            {
                private static bool _simple;

                static Weights()
                {
                    DrawingTargets = new List<Targets.Item>();
                    Game.OnUpdate += OnGameUpdate;
                }

                public static Menu Menu { get; private set; }
                internal static List<Targets.Item> DrawingTargets { get; private set; }

                public static bool Simple
                {
                    get { return _simple; }
                    set
                    {
                        _simple = value;
                        Utils.UpdateMenuItem(Menu, ".simple", _simple);
                    }
                }

                private static void OnGameUpdate(EventArgs args)
                {
                    if (Modes.Current.Mode == Mode.Weights && (BestTarget.Enabled || Simple))
                    {
                        var enemies =
                            Targets.Items.Where(h => h.Hero.IsValidTarget(TargetSelector.Weights.Range)).ToList();
                        foreach (var weight in TargetSelector.Weights.Items.Where(w => w.Weight > 0))
                        {
                            TargetSelector.Weights.UpdateMaxMinValue(weight, enemies, true);
                        }
                        foreach (var target in enemies)
                        {
                            var weight =
                                TargetSelector.Weights.Items.Where(w => w.Weight > 0)
                                    .Sum(w => TargetSelector.Weights.CalculatedWeight(w, target, true));
                            var heroPercent = TargetSelector.Weights.Heroes.GetPercentage(target.Hero);
                            target.SimulatedWeight = heroPercent > 0 ? weight / 100 * heroPercent : 0;
                        }
                        DrawingTargets = enemies.OrderByDescending(t => t.SimulatedWeight).ToList();
                        if (Game.Time - BestTarget.LastSwitch >= BestTarget.SwitchDelay)
                        {
                            BestTarget.Unit = DrawingTargets.FirstOrDefault();
                        }
                    }
                }

                internal static void AddToDrawingMenu()
                {
                    Menu = Drawings.Menu.AddSubMenu(new Menu("Weights", Drawings.Menu.Name + ".weights"));

                    BestTarget.AddToDrawingWeightsMenu();

                    Menu.AddItem(new MenuItem(Menu.Name + ".simple", "Simple").SetShared().SetValue(_simple))
                        .ValueChanged += (sender, args) => _simple = args.GetNewValue<bool>();

                    _simple = Utils.GetMenuItemValue<bool>(Menu, ".simple");
                }

                public static class BestTarget
                {
                    public const int MinRadius = 0;
                    public const int MaxRadius = 100;
                    private static Color _color = Color.SpringGreen;
                    private static int _radius = 55;
                    private static bool _enabled = true;
                    private static float _switchDelay = 0.5f;
                    private static Targets.Item _unit;
                    public static Menu Menu { get; private set; }

                    public static float SwitchDelay
                    {
                        get { return _switchDelay; }
                        set { _switchDelay = value; }
                    }

                    public static float LastSwitch { get; private set; }

                    public static Targets.Item Unit
                    {
                        get { return _unit; }
                        set
                        {
                            if (_unit != null && value != null && _unit.Hero.NetworkId != value.Hero.NetworkId)
                            {
                                LastSwitch = Game.Time;
                            }
                            _unit = value;
                        }
                    }

                    public static Color Color
                    {
                        get { return _color; }
                        set
                        {
                            _color = value;
                            Utils.UpdateMenuItem(Menu, ".color", _color);
                        }
                    }

                    public static int Radius
                    {
                        get { return _radius; }
                        set
                        {
                            _radius = Math.Min(MaxRadius, Math.Max(MinRadius, value));
                            Utils.UpdateMenuItem(Menu, ".radius", _radius);
                        }
                    }

                    public static bool Enabled
                    {
                        get { return _enabled; }
                        set
                        {
                            _enabled = value;
                            Utils.UpdateMenuItem(Menu, ".enabled", _enabled);
                        }
                    }

                    internal static void AddToDrawingWeightsMenu()
                    {
                        Menu = Weights.Menu.AddSubMenu(new Menu("Best Target", Weights.Menu.Name + ".best-target"));
                        Menu.AddItem(new MenuItem(Menu.Name + ".color", "Color").SetShared().SetValue(_color))
                            .ValueChanged += (sender, args) => _color = args.GetNewValue<Color>();
                        Menu.AddItem(
                            new MenuItem(Menu.Name + ".radius", "Radius").SetShared().SetValue(new Slider(_radius)))
                            .ValueChanged += (sender, args) => _radius = args.GetNewValue<Slider>().Value;
                        Menu.AddItem(new MenuItem(Menu.Name + ".enabled", "Enabled").SetShared().SetValue(_enabled))
                            .ValueChanged += (sender, args) => _enabled = args.GetNewValue<bool>();

                        _color = Utils.GetMenuItemValue<Color>(Menu, ".color");
                        _radius = Utils.GetMenuItemValue<int>(Menu, ".radius");
                        _enabled = Utils.GetMenuItemValue<bool>(Menu, ".enabled");
                    }
                }
            }
        }
    }
}