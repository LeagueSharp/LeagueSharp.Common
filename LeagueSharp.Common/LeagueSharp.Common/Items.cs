#region

using System;
using System.Linq;

#endregion

namespace LeagueSharp.Common
{
    public static class Items
    {
        static Items()
        {
            if (Common.isInitialized == false)
            {
                Common.InitializeCommonLib();
            }
        }

        public class Item
        {
            public int Id;
            public float Range;
            public Item(int id, float range)
            {
                Id = id;
                Range = range;
            }

            public bool IsReady()
            {
                return CanUseItem(Id);
            }

            public void Cast(Obj_AI_Base target)
            {
                if(ObjectManager.Player.Distance(target) < Range)
                    UseItem(Id, target);
            }
        }

        /// <summary>
        /// Returns if the Player has an item.
        /// </summary>
        public static bool HasItem(int id)
        {
            return HasItem(id, ObjectManager.Player);
        }

        /// <summary>
        /// Returns if the Player has an item.
        /// </summary>
        public static bool HasItem(string name)
        {
            return HasItem(name, ObjectManager.Player);
        }

        /// <summary>
        /// Returns true if the hero has the item.
        /// </summary>
        public static bool HasItem(string name, Obj_AI_Hero hero)
        {
            return hero.InventoryItems.Any(slot => slot.Name == name);
        }

        /// <summary>
        /// Returns true if the hero has the item.
        /// </summary>
        public static bool HasItem(int id, Obj_AI_Hero hero)
        {
            return hero.InventoryItems.Any(slot => slot.Id == (ItemId)id);
        }

        /// <summary>
        /// Retruns true if the player has the item and its not on cooldown.
        /// </summary>
        public static bool CanUseItem(string name)
        {
            InventorySlot islot = null;
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Name == name))
            {
                islot = slot;
            }
            if (islot == null) return false;
            var inst = ObjectManager.Player.Spellbook.Spells.FirstOrDefault(spell => (int)spell.Slot == islot.Slot + 4);
            return inst != null && inst.State == SpellState.Ready;
        }

        /// <summary>
        /// Retruns true if the player has the item and its not on cooldown.
        /// </summary>
        public static bool CanUseItem(int id)
        {
            InventorySlot islot = null;
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId)id))
            {
                islot = slot;
            }
            if (islot == null) return false;
            var inst = ObjectManager.Player.Spellbook.Spells.FirstOrDefault(spell => (int)spell.Slot == islot.Slot + 4);
            return inst != null && inst.State == SpellState.Ready;
        }

        /// <summary>
        /// Casts the item on the target.
        /// </summary>
        public static void UseItem(string name, Obj_AI_Base target = null)
        {
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Name == name))
            {
                if (target != null)
                {
                    slot.UseItem(target);
                }
                else
                {
                    slot.UseItem();
                }
            }
        }

        /// <summary>
        /// Casts the item on the target.
        /// </summary>
        public static void UseItem(int id, Obj_AI_Base target = null)
        {
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId)id))
            {
                if (target != null)
                {
                    slot.UseItem(target);
                }
                else
                {
                    slot.UseItem();
                }
            }
        }

        /// <summary>
        /// Returns the ward slot.
        /// </summary>
        public static InventorySlot GetWardSlot()
        {
            var wardIds = new int[] { 3340, 3350, 3361, 3154, 2045, 2049, 2050, 2044 };
            foreach (var wardId in wardIds)
                if (CanUseItem(wardId))
                    return ObjectManager.Player.InventoryItems.FirstOrDefault(slot => slot.Id == (ItemId)wardId);

            return null;
        }

    }
}