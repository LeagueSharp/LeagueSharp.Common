// This file is part of LeagueSharp.Common.
// 
// LeagueSharp.Common is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// LeagueSharp.Common is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with LeagueSharp.Common.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Linq;

namespace LeagueSharp.Common
{
    public class HealManager
    {
        //The main player for the heal.
        private readonly Obj_AI_Hero _player;
        //the healspell
        private readonly Spell spell;
        //The menu
        private Menu _menu;

        /// <summary>
        ///     Creates a new instance of Heal Manager
        /// </summary>
        public HealManager()
        {
            if (!IsCompatibleChampion())
            {
                throw new Exception("Champion not supported");
            }
            _player = ObjectManager.Player;
            spell = GetHealingSpell();

            Game.OnGameUpdate += OnGameUpdate;
        }

        /// <summary>
        ///     Checks if the player is playing a champion that has a healing effect
        /// </summary>
        /// <returns>true / false </returns>
        public bool IsCompatibleChampion()
        {
            switch (_player.ChampionName)
            {
                case "Nidalee":
                case "Kayle":
                case "Nami":
                case "Soraka":
                case "Taric":
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     Gets the spell that the player will use to heal with.
        /// </summary>
        /// <returns></returns>
        public Spell GetHealingSpell()
        {
            switch (_player.ChampionName)
            {
                case "Nidalee":
                    return new Spell(SpellSlot.E, 600);
                case "Kayle":
                    return new Spell(SpellSlot.W, 900);
                case "Nami":
                    return new Spell(SpellSlot.W, 725);
                case "Sona":
                    return new Spell(SpellSlot.Q, 1000);
                case "Taric":
                    return new Spell(SpellSlot.Q, 750);
            }
            return null;
        }

        /// <summary>
        ///     Adds the options to the main menu
        /// </summary>
        /// <param name="attachMenu"></param>
        public void AddToMenu(ref Menu attachMenu)
        {
            _menu = attachMenu;
            CreateMenu();

            Game.PrintChat(string.Format("{0} loaded by {1}", "Heal Manager", "iJabba"));
        }

        /// <summary>
        ///     Actually creates the menu
        /// </summary>
        public void CreateMenu()
        {
            var healMenu = new Menu("Heal Manager", "healManager");
            {
                healMenu.AddItem(new MenuItem("healSelf", "Heal Self").SetValue(true));
                healMenu.AddItem(new MenuItem("healSelfPercent", "Heal if health < %").SetValue(new Slider(30)));
                healMenu.AddItem(new MenuItem("sep", "----"));
                healMenu.AddItem(new MenuItem("healAllies", "Heal Allies").SetValue(true));
                var healAllies = healMenu.AddSubMenu(new Menu("Allies", "allies"));
                {
                    foreach (var hero in HeroManager.Allies.Where(ally => ally.IsAlly && !ally.IsMe))
                    {
                        healAllies.AddItem(
                            new MenuItem("heal" + hero.ChampionName, "Heal - " + hero.ChampionName).SetValue(true));
                    }
                }
                healMenu.AddItem(new MenuItem("healAllyPercent", "Heal if ally health < %").SetValue(new Slider(30)));
                healMenu.AddItem(
                    new MenuItem("healList", "Heal Mode").SetValue(
                        new StringList(new[] { "Closest", "Most AD", "Most AP", "Least Health" })));
            }
            _menu.AddSubMenu(healMenu);
        }

        /// <summary>
        ///     The Update method, in this we are calling the healing methods if the conditions are met.
        /// </summary>
        /// <param name="args"></param>
        private void OnGameUpdate(EventArgs args)
        {
            HealSelf();
            HealAllies();
        }

        /// <summary>
        ///     Heal our player
        /// </summary>
        public void HealSelf()
        {
            if (!_menu.Item("healSelf").GetValue<bool>())
            {
                return;
            }

            var healthPercentage = _player.Health / _player.MaxHealth * 100;
            if (_player.IsRecalling() || _player.IsDead || _player.InFountain())
            {
                return;
            }

            if (!(healthPercentage < _menu.Item("healSelfPercent").GetValue<Slider>().Value))
            {
                return;
            }

            if (spell.IsReady())
            {
                spell.Cast(_player);
            }
        }

        /// <summary>
        ///     Heal allies, according to which allies are closest.
        /// </summary>
        public void HealAllies()
        {
            if (!_menu.Item("healAllies").GetValue<bool>())
            {
                return;
            }

            Obj_AI_Hero selectedAlly;

            switch (_menu.Item("healList").GetValue<StringList>().SelectedIndex)
            {
                case 0: // closest
                    selectedAlly =
                        HeroManager.Allies.Where(hero => !hero.IsMe)
                            .OrderBy(closest => closest.Distance(_player.Position))
                            .FirstOrDefault();
                    break;
                case 1: // most ad
                    selectedAlly =
                        HeroManager.Allies.Where(hero => !hero.IsMe)
                            .OrderBy(damage => damage.BaseAttackDamage + damage.FlatPhysicalDamageMod)
                            .FirstOrDefault();
                    break;
                case 2: // most ap
                    selectedAlly =
                        HeroManager.Allies.Where(hero => !hero.IsMe)
                            .OrderBy(damage => damage.BaseAbilityDamage + damage.FlatMagicDamageMod)
                            .FirstOrDefault();
                    break;
                case 3: // least health
                    selectedAlly =
                        HeroManager.Allies.Where(hero => !hero.IsMe)
                            .OrderByDescending(health => health.Health)
                            .FirstOrDefault();
                    break;
                default:
                    selectedAlly =
                        HeroManager.Allies.Where(hero => hero.IsAlly && !hero.IsMe)
                            .OrderBy(closest => closest.Distance(_player.Position))
                            .FirstOrDefault();
                    break;
            }

            if (selectedAlly == null || selectedAlly.IsRecalling() || selectedAlly.IsDead || selectedAlly.InFountain())
            {
                return;
            }

            var allyHealthPercentage = selectedAlly.Health / selectedAlly.MaxHealth * 100;

            if (!(allyHealthPercentage < _menu.Item("healAllyPercent").GetValue<Slider>().Value))
            {
                return;
            }

            if (spell.IsReady() && _player.Distance(selectedAlly.Position) < spell.Range)
            {
                spell.Cast(selectedAlly);
            }
        }
    }
}