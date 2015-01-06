using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;

namespace LeagueSharp.Common
{
    public static class ManaManager
    {
        internal static Obj_AI_Hero Player;
        internal static List<SpellSlot> spells = new List<SpellSlot>();
        /// <summary>
        /// Checks if specific spell can be used if mana > other selected skills cost
        /// </summary>
        /// <param name="spell">SpellSlot</param>
        /// <returns>bool</returns>
        public static bool canUseSpell(SpellSlot spell)
        {
            var cost = 0f;
            foreach(var z in spells.Where(s => s != spell))
                cost += getManaCost(z);
            return Player.Mana > cost ? true : false;
        }
        /// <summary>
        /// Retrieves mana cost shorter way
        /// </summary>
        /// <param name="spell">SpellSlot</param>
        /// <returns>float</returns>
        public static float getManaCost(SpellSlot spell)
        {
            return Player.Spellbook.GetSpell(spell).ManaCost;
        }
        /// <summary>
        /// Inserts skill into checks.
        /// </summary>
        /// <param name="spell">SpellSlot</param>
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
        /// <summary>
        /// Menu class and main potion manager.
        /// </summary>
        public class ManagerInit
        {
            internal static Menu Config;
            internal static Items.Item healthPot = new Items.Item(ItemData.Health_Potion.Id);
            internal static Items.Item manaPot = new Items.Item(ItemData.Mana_Potion.Id);
            internal static Items.Item crystallineFlask = new Items.Item(ItemData.Crystalline_Flask.Id);
            public ManagerInit(Menu menu)
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
                // FlaskOfCrystalWater - Mana Potion
                // RegenerationPotion - Health Potion
                // ItemCrystalFlask - Flask

                // Health Potion
                if (healthPot.IsReady() && !ObjectManager.Player.HasBuff("RegenerationPotion",true) && !ObjectManager.Player.HasBuff("ItemCrystalFlask",true))
                {
                    if (ObjectManager.Player.CountEnemysInRange(range) > 0 &&
                            ObjectManager.Player.Health + 150 < ObjectManager.Player.MaxHealth 
                                || ObjectManager.Player.Health < ObjectManager.Player.MaxHealth * 0.5)
                    {
                        healthPot.Cast();
                    }
                }

                // Mana Potion
                if (!ObjectManager.Player.HasBuff("FlaskOfCrystalWater", true) && !ObjectManager.Player.HasBuff("ItemCrystalFlask", true) && manaPot.IsReady() && ObjectManager.Player.CountEnemysInRange(range) > 0 && ObjectManager.Player.Mana < getSkillsCost(spells))
                {
                    manaPot.Cast();
                }

                // Crystalline Flask
                if (!ObjectManager.Player.HasBuff("FlaskOfCrystalWater",true) 
                        && !ObjectManager.Player.HasBuff("ItemCrystalFlask",true)
                            && !ObjectManager.Player.HasBuff("RegenerationPotion", true)
                                && crystallineFlask.IsReady() && ObjectManager.Player.CountEnemysInRange(range) > 0 && (ObjectManager.Player.Mana < getSkillsCost(spells) || ObjectManager.Player.Health + 120 < ObjectManager.Player.MaxHealth))
                    crystallineFlask.Cast();
            }
        }
    }
}
