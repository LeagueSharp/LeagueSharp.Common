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

using System.Linq;

using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    public static class Items
    {
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
            return hero.InventoryItems.Any(slot => slot.Id == (ItemId) id);
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
            if (islot == null)
            {
                return false;
            }
            var inst = ObjectManager.Player.Spellbook.Spells.FirstOrDefault(spell => (int) spell.Slot == islot.Slot + 4);
            return inst != null && inst.State == SpellState.Ready;
        }

        /// <summary>
        /// Retruns true if the player has the item and its not on cooldown.
        /// </summary>
        public static bool CanUseItem(int id)
        {
            InventorySlot islot = null;
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId) id))
            {
                islot = slot;
            }
            if (islot == null)
            {
                return false;
            }
            var inst = ObjectManager.Player.Spellbook.Spells.FirstOrDefault(spell => (int) spell.Slot == islot.Slot + 4);
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
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId) id))
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
        /// Casts the item on a Vector2 position.
        /// </summary>
        public static void UseItem(int id, Vector2 position)
        {
            foreach (
                var slot in
                    ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId) id)
                        .Where(slot => position != null))
            {
                slot.UseItem(position.To3D());
            }
        }

        /// <summary>
        /// Casts the item on a Vector3 position.
        /// </summary>
        public static void UseItem(int id, Vector3 position)
        {
            foreach (
                var slot in
                    ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId) id)
                        .Where(slot => position != null))
            {
                slot.UseItem(position);
            }
        }

        /// <summary>
        /// Returns the ward slot.
        /// </summary>
        public static InventorySlot GetWardSlot()
        {
            var wardIds = new[] { 3340, 3350, 3361, 3154, 2045, 2049, 2050, 2044 };
            return (from wardId in wardIds
                where CanUseItem(wardId)
                select ObjectManager.Player.InventoryItems.FirstOrDefault(slot => slot.Id == (ItemId) wardId))
                .FirstOrDefault();
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

            public void Cast()
            {
                UseItem(Id);
            }

            public void Cast(Obj_AI_Base target)
            {
                if (ObjectManager.Player.Distance(target) < Range)
                {
                    UseItem(Id, target);
                }
            }

            public void Cast(Vector2 position)
            {
                if (ObjectManager.Player.Distance(position) < Range)
                {
                    UseItem(Id, position.To3D());
                }
            }

            public void Cast(Vector3 position)
            {
                if (ObjectManager.Player.Distance(position) < Range)
                {
                    UseItem(Id, position);
                }
            }

            public void Buy()
            {
                Packet.C2S.BuyItem.Encoded(new Packet.C2S.BuyItem.Struct(Id, ObjectManager.Player.NetworkId)).Send();
            }
        }
    }
}