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

#endregion

namespace LeagueSharp.Common
{
    public class TargetSelector
    {
        public enum TargetingMode
        {
            LowHP,
            MostAD,
            MostAP,
            Closest,
            NearMouse,
            AutoPriority,
            LessAttack,
            LessCast,
        }

        private static double _lasttick;

        private static readonly string[] ap =
        {
            "Ahri", "Akali", "Anivia", "Annie", "Brand", "Cassiopeia", "Diana",
            "FiddleSticks", "Fizz", "Gragas", "Heimerdinger", "Karthus", "Kassadin", "Katarina", "Kayle", "Kennen",
            "Leblanc", "Lissandra", "Lux", "Malzahar", "Mordekaiser", "Morgana", "Nidalee", "Orianna", "Ryze", "Sion",
            "Swain", "Syndra", "Teemo", "TwistedFate", "Veigar", "Viktor", "Vladimir", "Xerath", "Ziggs", "Zyra",
            "Velkoz"
        };

        private static readonly string[] sup =
        {
            "Blitzcrank", "Janna", "Karma", "Leona", "Lulu", "Nami", "Sona",
            "Soraka", "Thresh", "Zilean"
        };

        private static readonly string[] tank =
        {
            "Amumu", "Chogath", "DrMundo", "Galio", "Hecarim", "Malphite",
            "Maokai", "Nasus", "Rammus", "Sejuani", "Shen", "Singed", "Skarner", "Volibear", "Warwick", "Yorick", "Zac",
            "Nunu", "Taric", "Alistar", "Garen", "Nautilus", "Braum"
        };

        private static readonly string[] ad =
        {
            "Ashe", "Caitlyn", "Corki", "Draven", "Ezreal", "Graves", "KogMaw",
            "MissFortune", "Quinn", "Sivir", "Talon", "Tristana", "Twitch", "Urgot", "Varus", "Vayne", "Zed", "Jinx",
            "Yasuo", "Lucian"
        };

        private static readonly string[] bruiser =
        {
            "Darius", "Elise", "Evelynn", "Fiora", "Gangplank", "Gnar", "Jayce",
            "Pantheon", "Irelia", "JarvanIV", "Jax", "Khazix", "LeeSin", "Nocturne", "Olaf", "Poppy", "Renekton",
            "Rengar", "Riven", "Shyvana", "Trundle", "Tryndamere", "Udyr", "Vi", "MonkeyKing", "XinZhao", "Aatrox",
            "Rumble", "Shaco", "MasterYi"
        };

        public Obj_AI_Hero Target;
        private bool _drawcircle;
        private Obj_AI_Hero _maintarget;
        private TargetingMode _mode;
        private float _range;
        private bool _update = true;

        public TargetSelector(float range, TargetingMode mode)
        {
            _range = range;
            _mode = mode;

            Game.OnGameUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnWndProc += Game_OnWndProc;
        }

        private void Game_OnWndProc(WndEventArgs args)
        {
            if (MenuGUI.IsChatOpen || ObjectManager.Player.Spellbook.SelectedSpellSlot != SpellSlot.Unknown)
            {
                return;
            }

            if (args.WParam == 1) // LMouse
            {
                switch (args.Msg)
                {
                    case 257:
                        foreach (var hero in ObjectManager.Get<Obj_AI_Hero>())
                        {
                            if (hero.IsValidTarget() &&
                                SharpDX.Vector2.Distance(Game.CursorPos.To2D(), hero.ServerPosition.To2D()) < 300)
                            {
                                Target = hero;
                                _maintarget = hero;
                                Game.PrintChat("TargetSelector: New main target: " + _maintarget.ChampionName);
                            }
                        }
                        break;
                }
            }
        }

        private void Drawing_OnDraw(EventArgs args)
        {
            if (!ObjectManager.Player.IsDead && _drawcircle && Target != null && Target.IsVisible && !Target.IsDead)
            {
                Drawing.DrawCircle(Target.Position, 125, System.Drawing.Color.White);
            }
        }

        private void Game_OnGameUpdate(EventArgs args)
        {
            if (Environment.TickCount > _lasttick + 100)
            {
                _lasttick = Environment.TickCount;
                if (!_update)
                {
                    return;
                }
                if (_maintarget == null)
                {
                    GetNormalTarget();
                }
                else
                {
                    if (Geometry.Distance(_maintarget) > _range)
                    {
                        GetNormalTarget();
                    }
                    else
                    {
                        if (_maintarget.IsValidTarget())
                        {
                            Target = _maintarget;
                        }
                        else
                        {
                            GetNormalTarget();
                        }
                    }
                }
            }
        }

        private void GetNormalTarget()
        {
            Obj_AI_Hero newtarget = null;
            if (_mode != TargetingMode.AutoPriority)
            {
                foreach (var target in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(target => target.IsValidTarget() && Geometry.Distance(target) <= _range))
                {
                    if (newtarget == null)
                    {
                        newtarget = target;
                    }
                    else
                    {
                        switch (_mode)
                        {
                            case TargetingMode.LowHP:
                                if (target.Health < newtarget.Health)
                                {
                                    newtarget = target;
                                }
                                break;
                            case TargetingMode.MostAD:
                                if (target.BaseAttackDamage + target.FlatPhysicalDamageMod <
                                    newtarget.BaseAttackDamage + newtarget.FlatPhysicalDamageMod)
                                {
                                    newtarget = target;
                                }
                                break;
                            case TargetingMode.MostAP:
                                if (target.FlatMagicDamageMod < newtarget.FlatMagicDamageMod)
                                {
                                    newtarget = target;
                                }
                                break;
                            case TargetingMode.Closest:
                                if (Geometry.Distance(target) < Geometry.Distance(newtarget))
                                {
                                    newtarget = target;
                                }
                                break;
                            case TargetingMode.NearMouse:
                                if (SharpDX.Vector2.Distance(Game.CursorPos.To2D(), target.Position.To2D()) + 50 <
                                    SharpDX.Vector2.Distance(Game.CursorPos.To2D(), newtarget.Position.To2D()))
                                {
                                    newtarget = target;
                                }
                                break;

                            case TargetingMode.LessAttack:
                                if ((target.Health -
                                     ObjectManager.Player.CalcDamage(target, Damage.DamageType.Physical, target.Health) <
                                     (newtarget.Health -
                                      ObjectManager.Player.CalcDamage(
                                          newtarget, Damage.DamageType.Physical, newtarget.Health))))
                                {
                                    newtarget = target;
                                }
                                break;
                            case TargetingMode.LessCast:
                                if ((target.Health -
                                     ObjectManager.Player.CalcDamage(target, Damage.DamageType.Magical, target.Health) <
                                     (newtarget.Health -
                                      ObjectManager.Player.CalcDamage(
                                          newtarget, Damage.DamageType.Magical, newtarget.Health))))
                                {
                                    newtarget = target;
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                newtarget = AutoPriority();
            }
            Target = newtarget;
        }


        private Obj_AI_Hero AutoPriority()
        {
            Obj_AI_Hero autopriority = null;
            var prio = 5;
            foreach (var target in
                ObjectManager.Get<Obj_AI_Hero>()
                    .Where(target => target != null && target.IsValidTarget() && Geometry.Distance(target) <= _range))
            {
                var priority = FindPrioForTarget(target.ChampionName);
                if (autopriority == null)
                {
                    autopriority = target;
                    prio = priority;
                }
                else
                {
                    if (priority < prio)
                    {
                        autopriority = target;
                        prio = FindPrioForTarget(target.ChampionName);
                    }
                    else if (priority == prio)
                    {
                        if (!(target.Health < autopriority.Health))
                        {
                            continue;
                        }
                        autopriority = target;
                        prio = priority;
                    }
                }
            }
            return autopriority;
        }

        private static int FindPrioForTarget(string ChampionName)
        {
            if (ap.Contains(ChampionName))
            {
                return 2;
            }
            if (ad.Contains(ChampionName))
            {
                return 1;
            }
            if (sup.Contains(ChampionName))
            {
                return 3;
            }
            if (bruiser.Contains(ChampionName))
            {
                return 4;
            }
            if (tank.Contains(ChampionName))
            {
                return 5;
            }
            return 5;
        }

        public void SetDrawCircleOfTarget(bool draw)
        {
            _drawcircle = draw;
        }

        public void OverrideTarget(Obj_AI_Hero newtarget)
        {
            Target = newtarget;
            _update = false;
        }

        public void DisableTargetOverride()
        {
            _update = true;
        }

        public float GetRange()
        {
            return _range;
        }

        public void SetRange(float range)
        {
            _range = range;
        }

        public TargetingMode GetTargetingMode()
        {
            return _mode;
        }

        public void SetTargetingMode(TargetingMode mode)
        {
            _mode = mode;
        }

        public override string ToString()
        {
            return "Target: " + Target.ChampionName + "Range: " + _range + "Mode: " + _mode;
        }
    }

    /// <summary>
    /// Simple target selector that selects the hero that will die faster.
    /// </summary>
    public static class SimpleTs
    {
        public enum DamageType
        {
            Magical,
            Physical,
            True,
        }

        private static Menu _config;
        private static Obj_AI_Hero _selectedTarget;

        static SimpleTs()
        {
            Game.OnGameSendPacket += Game_OnGameSendPacket;
            Game.OnWndProc += Game_OnWndProc;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        internal static Obj_AI_Hero SelectedTarget
        {
            get { return (_config != null && _config.Item("FocusSelected").GetValue<bool>() ? _selectedTarget : null); }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (_selectedTarget.IsValidTarget() && _config != null && _config.Item("FocusSelected").GetValue<bool>() &&
                _config.Item("SelTColor").GetValue<Circle>().Active)
            {
                Render.Circle.DrawCircle(
                    _selectedTarget.Position, 150, _config.Item("SelTColor").GetValue<Circle>().Color, 7, true);
            }
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg != (uint) WindowsMessages.WM_LBUTTONDOWN)
            {
                return;
            }
            _selectedTarget = null;
            foreach (var enemy in
                ObjectManager.Get<Obj_AI_Hero>()
                    .Where(hero => hero.IsValidTarget())
                    .OrderByDescending(h => h.Distance(Game.CursorPos))
                    .Where(enemy => enemy.Distance(Game.CursorPos) < 200))
            {
                _selectedTarget = enemy;
            }
        }


        private static void Game_OnGameSendPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] != Packet.C2S.SetTarget.Header)
            {
                return;
            }

            var packet = Packet.C2S.SetTarget.Decoded(args.PacketData);

            if (packet.NetworkId != 0 && packet.Unit.IsValid && packet.Unit is Obj_AI_Hero &&
                packet.Unit.IsValidTarget())
            {
                _selectedTarget = (Obj_AI_Hero) packet.Unit;
            }
        }


        /// <summary>
        /// Sets the priority of the hero
        /// </summary>
        public static void SetPriority(Obj_AI_Hero hero, int newPriority)
        {
            if (_config == null || _config.Item("SimpleTS" + hero.ChampionName + "Priority") == null)
            {
                return;
            }
            var p = _config.Item("SimpleTS" + hero.ChampionName + "Priority").GetValue<Slider>();
            p.Value = Math.Max(1, Math.Min(5, newPriority));
            _config.Item("SimpleTS" + hero.ChampionName + "Priority").SetValue(p);
        }

        /// <summary>
        /// Returns the priority of the hero
        /// </summary>
        public static float GetPriority(Obj_AI_Hero hero)
        {
            var p = 1;
            if (_config != null && _config.Item("SimpleTS" + hero.ChampionName + "Priority") != null)
            {
                p = _config.Item("SimpleTS" + hero.ChampionName + "Priority").GetValue<Slider>().Value;
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

        public static void AddToMenu(Menu Config)
        {
            _config = Config;
            Config.AddItem(new MenuItem("FocusSelected", "Focus selected target").SetShared().SetValue(true));
            Config.AddItem(
                new MenuItem("SelTColor", "Selected target color").SetShared()
                    .SetValue(new Circle(true, System.Drawing.Color.Red)));
            Config.AddItem(new MenuItem("Sep", "").SetShared());
            var autoPriorityItem = new MenuItem("AutoPriority", "Auto arrange priorities").SetShared().SetValue(false);
            autoPriorityItem.ValueChanged += autoPriorityItem_ValueChanged;

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.Team != ObjectManager.Player.Team)
                )
            {
                Config.AddItem(
                    new MenuItem("SimpleTS" + enemy.ChampionName + "Priority", enemy.ChampionName).SetShared()
                        .SetValue(
                            new Slider(
                                autoPriorityItem.GetValue<bool>() ? GetPriorityFromDb(enemy.ChampionName) : 1, 5, 1)));
            }
            Config.AddItem(autoPriorityItem);
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
                _config.Item("SimpleTS" + enemy.ChampionName + "Priority")
                    .SetValue(new Slider(GetPriorityFromDb(enemy.ChampionName), 5, 1));
            }
        }

        private static bool IsInvulnerable(Obj_AI_Base target)
        {
            //TODO: add yasuo wall, spellshields, etc.
            if (target.HasBuff("Undying Rage") && target.Health >= 2f)
            {
                return true;
            }

            if (target.HasBuff("JudicatorIntervention"))
            {
                return true;
            }

            return false;
        }

        public static Obj_AI_Hero GetTarget(float range, DamageType damageType)
        {
            return GetTarget(ObjectManager.Player, range, damageType);
        }

        public static Obj_AI_Hero GetTarget(Obj_AI_Base champion, float range, DamageType damageType)
        {
            Obj_AI_Hero bestTarget = null;
            var bestRatio = 0f;

            if (SelectedTarget.IsValidTarget() && !IsInvulnerable(SelectedTarget) &&
                (range < 0 && Orbwalking.InAutoAttackRange(SelectedTarget) || champion.Distance(SelectedTarget) < range))
            {
                return SelectedTarget;
            }

            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (!hero.IsValidTarget() || IsInvulnerable(hero) ||
                    ((!(range < 0) || !Orbwalking.InAutoAttackRange(hero)) && !(champion.Distance(hero) < range)))
                {
                    continue;
                }
                var damage = 0f;

                switch (damageType)
                {
                    case DamageType.Magical:
                        damage = (float) ObjectManager.Player.CalcDamage(hero, Damage.DamageType.Physical, 100);
                        break;
                    case DamageType.Physical:
                        damage = (float) ObjectManager.Player.CalcDamage(hero, Damage.DamageType.Physical, 100);
                        break;
                    case DamageType.True:
                        damage = 100;
                        break;
                }

                var ratio = damage / (1 + hero.Health) * GetPriority(hero);

                if (ratio > bestRatio)
                {
                    bestRatio = ratio;
                    bestTarget = hero;
                }
            }

            return bestTarget;
        }
    }
}