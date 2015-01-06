using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;

namespace LeagueSharp.Common
{
    public static class MM
    {
        internal static Obj_AI_Hero Player;
        internal static List<SpellSlot> spells = new List<SpellSlot>();
        public static bool canUseSpell(SpellSlot spell)
        {
            var cost = 0f;
            foreach(var z in spells.Where(s => s != spell))
                cost += getManaCost(z);
            return Player.Mana > cost ? true : false;
        }
        public static float getManaCost(SpellSlot spell)
        {
            return Player.Spellbook.GetSpell(spell).ManaCost;
        }
        public static void insertSkill(SpellSlot spell)
        {
            spells.Add(spell);
        }
        internal static float getSkillsCost(List<SpellSlot> data)
        {
            var cost = 0f;
            foreach(var spell in data)
                cost += getManaCost(spell);
            return cost;
        }
        public class ManaManager
        {
            internal static Menu Config;
            internal static Items.Item healthPot = new Items.Item(ItemData.Health_Potion.Id);
            internal static Items.Item manaPot = new Items.Item(ItemData.Mana_Potion.Id);
            internal static Items.Item crystallineFlask = new Items.Item(ItemData.Crystalline_Flask.Id);
            public ManaManager(Menu menu)
            {
                Config = menu;
                Config.AddItem(new MenuItem("range", "Only use pots within enemy range").SetValue(new Slider(1000, 1000, 2000)));
                Config.AddItem(new MenuItem("enabled", "Enabled").SetValue(true));
                Player = ObjectManager.Player;
                Game.OnGameUpdate += Game_OnGameUpdate;
            }

            void Game_OnGameUpdate(EventArgs args)
            {
                if (Player.InFountain() || Player.IsRecalling() ) return;
                var range = Config.Item("range").GetValue<Slider>().Value;

                if (healthPot.IsReady() && !ObjectManager.Player.HasBuff("RegenerationPotion", true))
                {
                    if (Utility.CountEnemysInRange(range) > 0 && ObjectManager.Player.Health + 150 < ObjectManager.Player.MaxHealth
                        || ObjectManager.Player.Health < ObjectManager.Player.MaxHealth * 0.5)
                        healthPot.Cast();
                }
                if (manaPot.IsReady() && Utility.CountEnemysInRange(range) > 0 && ObjectManager.Player.Mana < getSkillsCost(spells))
                    manaPot.Cast();
                if (crystallineFlask.IsReady() && Utility.CountEnemysInRange(range) > 0 && (ObjectManager.Player.Mana < getSkillsCost(spells) || ObjectManager.Player.Health + 120 < ObjectManager.Player.MaxHealth))
                    crystallineFlask.Cast();
            }
        }
    }
}
