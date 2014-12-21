#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Orbwalking.cs is part of LeagueSharp.Common.
 
 LeagueSharp.Common is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 LeagueSharp.Common is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with LeagueSharp.Common. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

#region

using System;
using System.Linq;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace LeagueSharp.Common
{
    public class TargetSelector
    {
        #region Main

        static TargetSelector()
        {
            Game.OnWndProc += GameOnOnWndProc;
            Drawing.OnDraw += DrawingOnOnDraw;
        }

        #endregion

        #region Enum

        public enum DamageType
        {
            Magical,
            Physical,
            True
        }

        public enum TargetingMode
        {
            LowHp,
            MostAd,
            MostAp,
            Closest,
            NearMouse,
            AutoPriority,
            LessAttack,
            LessCast
        }

        #endregion

        #region Vars

        private static Menu _configMenu;
        private static Obj_AI_Hero _selectedTargetObjAiHero;

        #endregion

        #region EventArgs

        private static void DrawingOnOnDraw(EventArgs args)
        {
            if (_selectedTargetObjAiHero.IsValidTarget() && _configMenu != null &&
                _configMenu.Item("FocusSelected").GetValue<bool>() &&
                _configMenu.Item("SelTColor").GetValue<Circle>().Active)
            {
                Render.Circle.DrawCircle(
                    _selectedTargetObjAiHero.Position, 150, _configMenu.Item("SelTColor").GetValue<Circle>().Color, 7,
                    true);
            }
        }

        private static void GameOnOnWndProc(WndEventArgs args)
        {
            if (args.Msg != (uint) WindowsMessages.WM_LBUTTONDOWN)
            {
                return;
            }
            _selectedTargetObjAiHero = null;
            foreach (var enemy in
                ObjectManager.Get<Obj_AI_Hero>()
                    .Where(hero => hero.IsValidTarget())
                    .OrderByDescending(h => h.Distance(Game.CursorPos))
                    .Where(enemy => enemy.Distance(Game.CursorPos) < 200))
            {
                _selectedTargetObjAiHero = enemy;
            }
        }

        #endregion

        #region Functions

        public static Obj_AI_Hero SelectedTarget
        {
            get
            {
                return (_configMenu != null && _configMenu.Item("FocusSelected").GetValue<bool>()
                    ? _selectedTargetObjAiHero
                    : null);
            }
        }

        /// <summary>
        ///     Sets the priority of the hero
        /// </summary>
        public static void SetPriority(Obj_AI_Hero hero, int newPriority)
        {
            if (_configMenu == null || _configMenu.Item("TargetSelector" + hero.ChampionName + "Priority") == null)
            {
                return;
            }
            var p = _configMenu.Item("TargetSelector" + hero.ChampionName + "Priority").GetValue<Slider>();
            p.Value = Math.Max(1, Math.Min(5, newPriority));
            _configMenu.Item("TargetSelector" + hero.ChampionName + "Priority").SetValue(p);
        }

        /// <summary>
        ///     Returns the priority of the hero
        /// </summary>
        public static float GetPriority(Obj_AI_Hero hero)
        {
            var p = 1;
            if (_configMenu != null && _configMenu.Item("TargetSelector" + hero.ChampionName + "Priority") != null)
            {
                p = _configMenu.Item("TargetSelector" + hero.ChampionName + "Priority").GetValue<Slider>().Value;
            }

            switch (p)
            {
                case 2:
                    return 1.5f;
                case 3:
                    return 1.75f;
                case 4:
                    return 2f;
                case 5:
                    return 2.5f;
                default:
                    return 1f;
            }
        }

        private static int GetPriorityFromDb(string championName)
        {
            string[] p1 =
            {
                "Alistar", "Amumu", "Blitzcrank", "Braum", "Cho'Gath", "Dr. Mundo", "Garen", "Gnar",
                "Hecarim", "Janna", "Jarvan IV", "Leona", "Lulu", "Malphite", "Nami", "Nasus", "Nautilus", "Nunu",
                "Olaf", "Rammus", "Renekton", "Sejuani", "Shen", "Shyvana", "Singed", "Sion", "Skarner", "Sona",
                "Soraka", "Taric", "Thresh", "Volibear", "Warwick", "MonkeyKing", "Yorick", "Zac", "Zyra"
            };

            string[] p2 =
            {
                "Aatrox", "Darius", "Elise", "Evelynn", "Galio", "Gangplank", "Gragas", "Irelia", "Jax",
                "Lee Sin", "Maokai", "Morgana", "Nocturne", "Pantheon", "Poppy", "Rengar", "Rumble", "Ryze", "Swain",
                "Trundle", "Tryndamere", "Udyr", "Urgot", "Vi", "XinZhao"
            };

            string[] p3 =
            {
                "Akali", "Diana", "Fiddlesticks", "Fiora", "Fizz", "Heimerdinger", "Jayce", "Kassadin",
                "Kayle", "Kha'Zix", "Lissandra", "Mordekaiser", "Nidalee", "Riven", "Shaco", "Vladimir", "Yasuo",
                "Zilean"
            };

            string[] p4 =
            {
                "Ahri", "Anivia", "Annie", "Ashe", "Brand", "Caitlyn", "Cassiopeia", "Corki", "Draven",
                "Ezreal", "Graves", "Jinx", "Karma", "Karthus", "Katarina", "Kennen", "KogMaw", "LeBlanc", "Lucian",
                "Lux", "Malzahar", "MasterYi", "MissFortune", "Orianna", "Quinn", "Sivir", "Syndra", "Talon", "Teemo",
                "Tristana", "TwistedFate", "Twitch", "Varus", "Vayne", "Veigar", "VelKoz", "Viktor", "Xerath", "Zed",
                "Ziggs"
            };

            if (p1.Contains(championName))
            {
                return 1;
            }
            if (p2.Contains(championName))
            {
                return 2;
            }
            if (p3.Contains(championName))
            {
                return 3;
            }
            return p4.Contains(championName) ? 4 : 1;
        }

        public static void AddToMenu(Menu config)
        {
            _configMenu = config;
            config.AddItem(new MenuItem("FocusSelected", "Focus selected target").SetShared().SetValue(true));
            config.AddItem(
                new MenuItem("SelTColor", "Selected target color").SetShared()
                    .SetValue(new Circle(true, Color.Red)));
            config.AddItem(new MenuItem("Sep", "").SetShared());
            var autoPriorityItem = new MenuItem("AutoPriority", "Auto arrange priorities").SetShared().SetValue(false);
            autoPriorityItem.ValueChanged += autoPriorityItem_ValueChanged;

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.Team != ObjectManager.Player.Team)
                )
            {
                config.AddItem(
                    new MenuItem("TargetSelector" + enemy.ChampionName + "Priority", enemy.ChampionName).SetShared()
                        .SetValue(
                            new Slider(
                                autoPriorityItem.GetValue<bool>() ? GetPriorityFromDb(enemy.ChampionName) : 1, 5, 1)));
                if (autoPriorityItem.GetValue<bool>())
                {
                    config.Item("TargetSelector" + enemy.ChampionName + "Priority")
                        .SetValue(new Slider(
                            autoPriorityItem.GetValue<bool>() ? GetPriorityFromDb(enemy.ChampionName) : 1, 5, 1));
                }
            }
            config.AddItem(autoPriorityItem);
            config.AddItem(
                new MenuItem("TargetingMode", "Target Mode").SetShared()
                    .SetValue(
                        new StringList(
                            new[]
                            {
                                "LowHP", "MostAD", "MostAP", "Closest", "NearMouse", "Priority", "LessAttack",
                                "LessCast"
                            }, 5)));
        }

        private static void autoPriorityItem_ValueChanged(object sender, OnValueChangeEventArgs e)
        {
            if (!e.GetNewValue<bool>())
            {
                return;
            }
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.Team != ObjectManager.Player.Team)
                )
            {
                _configMenu.Item("TargetSelector" + enemy.ChampionName + "Priority")
                    .SetValue(new Slider(GetPriorityFromDb(enemy.ChampionName), 5, 1));
            }
        }

        public static bool IsInvulnerable(Obj_AI_Base target, DamageType damageType)
        {
            if ((damageType.Equals(DamageType.Magical) || damageType.Equals(DamageType.True)) &&
                target.HasBuff("Undying Rage") && target.Health >= 2f)
            {
                return true;
            }

            if (target.HasBuff("JudicatorIntervention"))
            {
                return true;
            }

            // TODO: Add Yasuo Windshield, spellshields and etc.

            return false;
        }


        public static void SetTarget(Obj_AI_Hero hero)
        {
            if (hero.IsValidTarget())
            {
                _selectedTargetObjAiHero = hero;
            }
        }

        public static Obj_AI_Hero GetSelectedTarget()
        {
            return SelectedTarget;
        }

        public static Obj_AI_Hero GetTarget(float range, DamageType damageType)
        {
            return GetTarget(ObjectManager.Player, range, damageType);
        }

        public static Obj_AI_Hero GetTarget(Obj_AI_Base champion, float range, DamageType damageType)
        {
            Obj_AI_Hero bestTarget = null;

            if (SelectedTarget.IsValidTarget() && !IsInvulnerable(SelectedTarget, damageType) &&
                (range < 0 && Orbwalking.InAutoAttackRange(SelectedTarget) || champion.Distance(SelectedTarget) < range))
            {
                return SelectedTarget;
            }

            var bestRatio = 0f;

            var targetingMode = TargetingMode.AutoPriority;
            if (_configMenu != null && _configMenu.Item("TargetingMode") != null)
            {
                var menuItem = _configMenu.Item("TargetingMode").GetValue<StringList>();
                Enum.TryParse(menuItem.SList[menuItem.SelectedIndex], out targetingMode);
            }

            foreach (
                var hero in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(
                            hero =>
                                hero.IsValidTarget() && !IsInvulnerable(hero, damageType) &&
                                ((range < 0 && Orbwalking.InAutoAttackRange(hero)) || champion.Distance(hero) < range)))
            {
                if (bestTarget == null)
                {
                    bestTarget = hero;
                    continue;
                }

                switch (targetingMode)
                {
                    case TargetingMode.LowHp:
                        if (hero.Health < bestTarget.Health)
                        {
                            bestTarget = hero;
                        }
                        break;

                    case TargetingMode.MostAd:
                        if (hero.BaseAttackDamage + hero.FlatPhysicalDamageMod >
                            bestTarget.BaseAttackDamage + bestTarget.FlatPhysicalDamageMod)
                        {
                            bestTarget = hero;
                        }
                        break;

                    case TargetingMode.MostAp:
                        if (hero.BaseAbilityDamage + hero.FlatMagicDamageMod >
                            bestTarget.BaseAbilityDamage + bestTarget.FlatMagicDamageMod)
                        {
                            bestTarget = hero;
                        }
                        break;

                    case TargetingMode.Closest:
                        if (Geometry.Distance(hero) < Geometry.Distance(bestTarget))
                        {
                            bestTarget = hero;
                        }
                        break;

                    case TargetingMode.NearMouse:
                        if (Vector2.Distance(Game.CursorPos.To2D(), hero.Position.To2D()) + 50 <
                            Vector2.Distance(Game.CursorPos.To2D(), bestTarget.Position.To2D()))
                        {
                            bestTarget = hero;
                        }
                        break;

                    case TargetingMode.AutoPriority:
                        var damage = 0f;

                        switch (damageType)
                        {
                            case DamageType.Magical:
                                damage = (float) ObjectManager.Player.CalcDamage(hero, Damage.DamageType.Magical, 100);
                                break;
                            case DamageType.Physical:
                                damage = (float) ObjectManager.Player.CalcDamage(hero, Damage.DamageType.Physical, 100);
                                break;
                            case DamageType.True:
                                damage = 100;
                                break;
                        }

                        var ratio = damage/(1 + hero.Health)*GetPriority(hero);

                        if (ratio > bestRatio)
                        {
                            bestRatio = ratio;
                            bestTarget = hero;
                        }
                        break;

                    case TargetingMode.LessAttack:
                        if ((hero.Health -
                             ObjectManager.Player.CalcDamage(hero, Damage.DamageType.Physical, hero.Health) <
                             (bestTarget.Health -
                              ObjectManager.Player.CalcDamage(bestTarget, Damage.DamageType.Physical, bestTarget.Health))))
                        {
                            bestTarget = hero;
                        }
                        break;

                    case TargetingMode.LessCast:
                        if ((hero.Health - ObjectManager.Player.CalcDamage(hero, Damage.DamageType.Magical, hero.Health) <
                             (bestTarget.Health -
                              ObjectManager.Player.CalcDamage(bestTarget, Damage.DamageType.Magical, bestTarget.Health))))
                        {
                            bestTarget = hero;
                        }
                        break;
                }
            }

            return bestTarget;
        }

        #endregion
    }
}