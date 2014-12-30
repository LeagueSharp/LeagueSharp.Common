#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Items.cs is part of LeagueSharp.Common.
 
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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Items
    {
        [Flags]
        public enum ItemCategory
        {
            None = 0,
            CriticalStrike = 1 << 0,
            HealthRegen = 1 << 1,
            Consumable = 1 << 2,
            Health = 1 << 3,
            Damage = 1 << 4,
            ManaRegen = 1 << 5,
            SpellBlock = 1 << 6,
            AttackSpeed = 1 << 7,
            LifeSteal = 1 << 8,
            SpellDamage = 1 << 9,
            Mana = 1 << 10,
            Armor = 1 << 11
        }

        public enum ItemTier
        {
            None,
            Basic,
            Advanced,
            Legendary,
            Mythical,
            Enchantment,
            Consumable,
            RengarsTrinket,
            BasicTrinket,
            AdvancedTrinket
        }

        /// <summary>
        ///     Returns if the Player has an item.
        /// </summary>
        public static bool HasItem(int id)
        {
            return HasItem(id, ObjectManager.Player);
        }

        /// <summary>
        ///     Returns if the Player has an item.
        /// </summary>
        public static bool HasItem(string name)
        {
            return HasItem(name, ObjectManager.Player);
        }

        /// <summary>
        ///     Returns true if the hero has the item.
        /// </summary>
        public static bool HasItem(string name, Obj_AI_Hero hero)
        {
            return hero.InventoryItems.Any(slot => slot.Name == name);
        }

        /// <summary>
        ///     Returns true if the hero has the item.
        /// </summary>
        public static bool HasItem(int id, Obj_AI_Hero hero)
        {
            return hero.InventoryItems.Any(slot => slot.Id == (ItemId) id);
        }

        /// <summary>
        ///     Retruns true if the player has the item and its not on cooldown.
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

            var inst =
                ObjectManager.Player.Spellbook.Spells.FirstOrDefault(
                    spell => (int) spell.Slot == islot.Slot + (int) SpellSlot.Item1);
            return inst != null && inst.State == SpellState.Ready;
        }

        /// <summary>
        ///     Retruns true if the player has the item and its not on cooldown.
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

            var inst =
                ObjectManager.Player.Spellbook.Spells.FirstOrDefault(
                    spell => (int) spell.Slot == islot.Slot + (int) SpellSlot.Item1);
            return inst != null && inst.State == SpellState.Ready;
        }

        /// <summary>
        ///     Casts the item on the target.
        /// </summary>
        public static void UseItem(string name, Obj_AI_Base target = null)
        {
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Name == name))
            {
                if (target != null)
                {
                    ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot, target);
                }
                else
                {
                    ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot);
                }
            }
        }

        /// <summary>
        ///     Casts the item on the target.
        /// </summary>
        public static void UseItem(int id, Obj_AI_Base target = null)
        {
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId) id))
            {
                if (target != null)
                {
                    ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot, target);
                }
                else
                {
                    ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot);
                }
            }
        }

        /// <summary>
        ///     Casts the item on a Vector2 position.
        /// </summary>
        public static void UseItem(int id, Vector2 position)
        {
            foreach (var slot in
                ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId) id)
                    .Where(slot => position != Vector2.Zero))
            {
                ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot, position.To3D());
            }
        }

        /// <summary>
        ///     Casts the item on a Vector3 position.
        /// </summary>
        public static void UseItem(int id, Vector3 position)
        {
            foreach (var slot in
                ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId) id)
                    .Where(slot => position != Vector3.Zero))
            {
                ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot, position);
            }
        }

        /// <summary>
        ///     Returns the ward slot.
        /// </summary>
        public static InventorySlot GetWardSlot()
        {
            var wardIds = new[] { 3340, 3350, 3361, 3154, 2045, 2049, 2050, 2044 };
            return (from wardId in wardIds
                where CanUseItem(wardId)
                select ObjectManager.Player.InventoryItems.FirstOrDefault(slot => slot.Id == (ItemId) wardId))
                .FirstOrDefault();
        }

        /// <summary>
        ///     Returns if item should have reduced sell price.
        /// </summary>
        /// <param name="itemId">Item Id</param>
        /// <returns></returns>
        private static bool IsReducedSellItem(int itemId)
        {
            switch (itemId)
            {
                case 3069:
                case 3092:
                case 1055:
                case 1054:
                case 1039:
                case 1062:
                case 1063:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        ///     Returns the sell price.
        /// </summary>
        /// <param name="itemId">Item Id</param>
        /// <param name="fullPrice">Full Price of the Item Id</param>
        /// <returns>Returns the sell price.</returns>
        private static int GetReducedPrice(int itemId, int fullPrice)
        {
            return IsReducedSellItem(itemId) ? ((fullPrice * 30) / 100) : ((fullPrice * 70) / 100);
        }

        public struct Item
        {
            public float FlatArmorMod;
            public float FlatCritChanceMod;
            public float FlatCritDamageMod;
            public float FlatHPPoolMod;
            public float FlatHPRegenMod;
            public float FlatMagicDamageMod;
            public float FlatMovementSpeedMod;
            public float FlatPhysicalDamageMod;
            public float FlatSpellBlockMod;
            public int Id;
            public bool IsRecipe;
            public ItemCategory ItemCategory;
            public ItemTier ItemTier;
            public int MaxStacks;
            public string Name;
            public float PercentArmorMod;
            public float PercentCritDamageMod;
            public float PercentEXPBonus;
            public float PercentHPPoolMod;
            public float PercentHPRegenMod;
            public float PercentMagicDamageMod;
            public float PercentMovementSpeedMod;
            public float PercentPhysicalDamageMod;
            public float PercentSpellBlockMod;
            public float PercentAttackSpeedMod;
            public int Price;
            public float Range;
            public int[] RecipeItems;
            public int SellPrice;

            [Obsolete("Use Items.Item_Name", false)]
            public Item(int id, float range)
            {
                Id = id;
                Name = string.Empty;
                Range = range;
                MaxStacks = 0;

                IsRecipe = false;

                Price = 0;
                RecipeItems = null;
                SellPrice = 0;

                ItemCategory = ItemCategory.None;
                ItemTier = ItemTier.None;

                PercentArmorMod = 0f;
                PercentCritDamageMod = 0f;
                PercentEXPBonus = 0f;
                PercentHPPoolMod = 0f;
                PercentHPRegenMod = 0f;
                PercentMagicDamageMod = 0f;
                PercentMovementSpeedMod = 0f;
                PercentPhysicalDamageMod = 0f;
                PercentSpellBlockMod = 0f;
                PercentAttackSpeedMod = 0f;

                FlatArmorMod = 0f;
                FlatCritChanceMod = 0f;
                FlatCritDamageMod = 0f;
                FlatHPPoolMod = 0f;
                FlatHPRegenMod = 0f;
                FlatMagicDamageMod = 0f;
                FlatMovementSpeedMod = 0f;
                FlatPhysicalDamageMod = 0f;
                FlatSpellBlockMod = 0f;
            }

            public bool IsReady()
            {
                return CanUseItem(Id);
            }

            public bool HasItem()
            {
                return Items.HasItem(Id);
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
                ObjectManager.Player.BuyItem((ItemId) Id);
            }
        }

        #region Items

        #region Boots of Speed

        public static Item Boots_of_Speed = new Item
        {
            Id = 1001,
            Name = "Boots of Speed",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 325,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1001, 325),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Faerie Charm

        public static Item Faerie_Charm = new Item
        {
            Id = 1004,
            Name = "Faerie Charm",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 180,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1004, 180),
            //
            ItemCategory = ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Meki Pendant

        public static Item Meki_Pendant = new Item
        {
            Id = 1005,
            Name = "Meki Pendant",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 390,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1005, 390),
            //
            ItemCategory = ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Rejuvenation Bead

        public static Item Rejuvenation_Bead = new Item
        {
            Id = 1006,
            Name = "Rejuvenation Bead",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 180,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1006, 180),
            //
            ItemCategory = ItemCategory.HealthRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Regrowth Pendant

        public static Item Regrowth_Pendant = new Item
        {
            Id = 1007,
            Name = "Regrowth Pendant",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 435,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1007, 435),
            //
            ItemCategory = ItemCategory.HealthRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Giant's Belt

        public static Item Giants_Belt = new Item
        {
            Id = 1011,
            Name = "Giant's Belt",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 1000,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1011, 1000),
            //
            ItemCategory = ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Cloak of Agility

        public static Item Cloak_of_Agility = new Item
        {
            Id = 1018,
            Name = "Cloak of Agility",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 730,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1018, 730),
            //
            ItemCategory = ItemCategory.CriticalStrike,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0.15f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Blasting Wand

        public static Item Blasting_Wand = new Item
        {
            Id = 1026,
            Name = "Blasting Wand",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 860,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1026, 860),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Sapphire Crystal

        public static Item Sapphire_Crystal = new Item
        {
            Id = 1027,
            Name = "Sapphire Crystal",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 400,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1027, 400),
            //
            ItemCategory = ItemCategory.Mana,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ruby Crystal

        public static Item Ruby_Crystal = new Item
        {
            Id = 1028,
            Name = "Ruby Crystal",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 400,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1028, 400),
            //
            ItemCategory = ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Cloth Armor

        public static Item Cloth_Armor = new Item
        {
            Id = 1029,
            Name = "Cloth Armor",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 300,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1029, 300),
            //
            ItemCategory = ItemCategory.Armor,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Chain Vest

        public static Item Chain_Vest = new Item
        {
            Id = 1031,
            Name = "Chain Vest",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 450,
            RecipeItems = new[] { 1029 },
            SellPrice = GetReducedPrice(1031, 750),
            //
            ItemCategory = ItemCategory.Armor,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Null-Magic Mantle

        public static Item NullMagic_Mantle = new Item
        {
            Id = 1033,
            Name = "Null-Magic Mantle",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 500,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1033, 500),
            //
            ItemCategory = ItemCategory.SpellBlock,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Long Sword

        public static Item Long_Sword = new Item
        {
            Id = 1036,
            Name = "Long Sword",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 360,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1036, 360),
            //
            ItemCategory = ItemCategory.Damage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Pickaxe

        public static Item Pickaxe = new Item
        {
            Id = 1037,
            Name = "Pickaxe",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 875,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1037, 875),
            //
            ItemCategory = ItemCategory.Damage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region B. F. Sword

        public static Item B__F__Sword = new Item
        {
            Id = 1038,
            Name = "B. F. Sword",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 1550,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1038, 1550),
            //
            ItemCategory = ItemCategory.Damage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Hunter's Machete

        public static Item Hunters_Machete = new Item
        {
            Id = 1039,
            Name = "Hunter's Machete",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 400,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1039, 400),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.HealthRegen & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Dagger

        public static Item Dagger = new Item
        {
            Id = 1042,
            Name = "Dagger",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 450,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1042, 450),
            //
            ItemCategory = ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Recurve Bow

        public static Item Recurve_Bow = new Item
        {
            Id = 1043,
            Name = "Recurve Bow",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 900,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1043, 900),
            //
            ItemCategory = ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Brawler's Gloves

        public static Item Brawlers_Gloves = new Item
        {
            Id = 1051,
            Name = "Brawler's Gloves",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 400,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1051, 400),
            //
            ItemCategory = ItemCategory.CriticalStrike,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0.08f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Amplifying Tome

        public static Item Amplifying_Tome = new Item
        {
            Id = 1052,
            Name = "Amplifying Tome",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 435,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1052, 435),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Vampiric Scepter

        public static Item Vampiric_Scepter = new Item
        {
            Id = 1053,
            Name = "Vampiric Scepter",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 440,
            RecipeItems = new[] { 1036 },
            SellPrice = GetReducedPrice(1053, 800),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.LifeSteal,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Doran's Shield

        public static Item Dorans_Shield = new Item
        {
            Id = 1054,
            Name = "Doran's Shield",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 440,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1054, 440),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 1.2f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Doran's Blade

        public static Item Dorans_Blade = new Item
        {
            Id = 1055,
            Name = "Doran's Blade",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 440,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1055, 440),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.LifeSteal & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Doran's Ring

        public static Item Dorans_Ring = new Item
        {
            Id = 1056,
            Name = "Doran's Ring",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 400,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1056, 400),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.ManaRegen & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Negatron Cloak

        public static Item Negatron_Cloak = new Item
        {
            Id = 1057,
            Name = "Negatron Cloak",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 350,
            RecipeItems = new[] { 1033 },
            SellPrice = GetReducedPrice(1057, 850),
            //
            ItemCategory = ItemCategory.SpellBlock,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Needlessly Large Rod

        public static Item Needlessly_Large_Rod = new Item
        {
            Id = 1058,
            Name = "Needlessly Large Rod",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 1600,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1058, 1600),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Prospector's Blade

        public static Item Prospectors_Blade = new Item
        {
            Id = 1062,
            Name = "Prospector's Blade",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 950,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1062, 950),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.AttackSpeed & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Prospector's Ring

        public static Item Prospectors_Ring = new Item
        {
            Id = 1063,
            Name = "Prospector's Ring",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 950,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1063, 950),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.ManaRegen & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Doran's Shield (Showdown)

        public static Item Dorans_Shield_Showdown = new Item
        {
            Id = 1074,
            Name = "Doran's Shield (Showdown)",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 440,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1074, 440),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Doran's Blade (Showdown)

        public static Item Dorans_Blade_Showdown = new Item
        {
            Id = 1075,
            Name = "Doran's Blade (Showdown)",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 440,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1075, 440),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.LifeSteal & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Doran's Ring (Showdown)

        public static Item Dorans_Ring_Showdown = new Item
        {
            Id = 1076,
            Name = "Doran's Ring (Showdown)",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 400,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1076, 400),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.ManaRegen & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Spirit Stone

        public static Item Spirit_Stone = new Item
        {
            Id = 1080,
            Name = "Spirit Stone",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 15,
            RecipeItems = new[] { 1004, 1006, 1039 },
            SellPrice = GetReducedPrice(1080, 775),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Penetrating Bullets

        public static Item Penetrating_Bullets = new Item
        {
            Id = 1500,
            Name = "Penetrating Bullets",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1500, 0),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Fortification

        public static Item Fortification = new Item
        {
            Id = 1501,
            Name = "Fortification",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1501, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Reinforced Armor

        public static Item Reinforced_Armor = new Item
        {
            Id = 1502,
            Name = "Reinforced Armor",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1502, 0),
            //
            ItemCategory = ItemCategory.Armor & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Warden's Eye

        public static Item Wardens_Eye = new Item
        {
            Id = 1503,
            Name = "Warden's Eye",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1503, 0),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Vanguard

        public static Item Vanguard = new Item
        {
            Id = 1504,
            Name = "Vanguard",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1504, 0),
            //
            ItemCategory = ItemCategory.Armor & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Lightning Rod

        public static Item Lightning_Rod = new Item
        {
            Id = 1505,
            Name = "Lightning Rod",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1505, 0),
            //
            ItemCategory = ItemCategory.Armor & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region ???

        public static Item Unkown = new Item
        {
            Id = 1506,
            Name = "???",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(1506, 0),
            //
            ItemCategory = ItemCategory.Armor & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Health Potion

        public static Item Health_Potion = new Item
        {
            Id = 2003,
            Name = "Health Potion",
            Range = 0f,
            MaxStacks = 5,
            //
            IsRecipe = false,
            //
            Price = 35,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2003, 35),
            //
            ItemCategory = ItemCategory.Consumable,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Mana Potion

        public static Item Mana_Potion = new Item
        {
            Id = 2004,
            Name = "Mana Potion",
            Range = 0f,
            MaxStacks = 5,
            //
            IsRecipe = false,
            //
            Price = 35,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2004, 35),
            //
            ItemCategory = ItemCategory.Consumable,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Total Biscuit of Rejuvenation

        public static Item Total_Biscuit_of_Rejuvenation = new Item
        {
            Id = 2009,
            Name = "Total Biscuit of Rejuvenation",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2009, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Total Biscuit of Rejuvenation

        public static Item Total_Biscuit_of_Rejuvenation2 = new Item
        {
            Id = 2010,
            Name = "Total Biscuit of Rejuvenation",
            Range = 0f,
            MaxStacks = 5,
            //
            IsRecipe = false,
            //
            Price = 35,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2010, 35),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Elixir of Fortitude

        public static Item Elixir_of_Fortitude = new Item
        {
            Id = 2037,
            Name = "Elixir of Fortitude",
            Range = 0f,
            MaxStacks = 3,
            //
            IsRecipe = false,
            //
            Price = 350,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2037, 350),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.Consumable & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Elixir of Agility

        public static Item Elixir_of_Agility = new Item
        {
            Id = 2038,
            Name = "Elixir of Agility",
            Range = 0f,
            MaxStacks = 3,
            //
            IsRecipe = false,
            //
            Price = 250,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2038, 250),
            //
            ItemCategory = ItemCategory.AttackSpeed & ItemCategory.CriticalStrike & ItemCategory.Consumable,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Elixir of Brilliance

        public static Item Elixir_of_Brilliance = new Item
        {
            Id = 2039,
            Name = "Elixir of Brilliance",
            Range = 0f,
            MaxStacks = 3,
            //
            IsRecipe = false,
            //
            Price = 250,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2039, 250),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.Consumable,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ichor of Rage

        public static Item Ichor_of_Rage = new Item
        {
            Id = 2040,
            Name = "Ichor of Rage",
            Range = 0f,
            MaxStacks = 3,
            //
            IsRecipe = false,
            //
            Price = 500,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2040, 500),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.AttackSpeed & ItemCategory.Consumable,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Crystalline Flask

        public static Item Crystalline_Flask = new Item
        {
            Id = 2041,
            Name = "Crystalline Flask",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 345,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2041, 345),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.ManaRegen & ItemCategory.Consumable,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Oracle's Elixir

        public static Item Oracles_Elixir = new Item
        {
            Id = 2042,
            Name = "Oracle's Elixir",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 400,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2042, 400),
            //
            ItemCategory = ItemCategory.Consumable,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Vision Ward

        public static Item Vision_Ward = new Item
        {
            Id = 2043,
            Name = "Vision Ward",
            Range = 600f,
            MaxStacks = 2,
            //
            IsRecipe = false,
            //
            Price = 100,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2043, 100),
            //
            ItemCategory = ItemCategory.Consumable,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Stealth Ward

        public static Item Stealth_Ward = new Item
        {
            Id = 2044,
            Name = "Stealth Ward",
            Range = 600f,
            MaxStacks = 3,
            //
            IsRecipe = false,
            //
            Price = 75,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2044, 75),
            //
            ItemCategory = ItemCategory.Consumable,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ruby Sightstone

        public static Item Ruby_Sightstone = new Item
        {
            Id = 2045,
            Name = "Ruby Sightstone",
            Range = 600f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 400,
            RecipeItems = new[] { 2049, 1028 },
            SellPrice = GetReducedPrice(2045, 1200),
            //
            ItemCategory = ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Oracle's Extract

        public static Item Oracles_Extract = new Item
        {
            Id = 2047,
            Name = "Oracle's Extract",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 250,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2047, 250),
            //
            ItemCategory = ItemCategory.Consumable,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ichor of Illumination

        public static Item Ichor_of_Illumination = new Item
        {
            Id = 2048,
            Name = "Ichor of Illumination",
            Range = 0f,
            MaxStacks = 3,
            //
            IsRecipe = false,
            //
            Price = 500,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2048, 500),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.ManaRegen & ItemCategory.Consumable,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Sightstone

        public static Item Sightstone = new Item
        {
            Id = 2049,
            Name = "Sightstone",
            Range = 600f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 400,
            RecipeItems = new[] { 1028 },
            SellPrice = GetReducedPrice(2049, 800),
            //
            ItemCategory = ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Explorer's Ward

        public static Item Explorers_Ward = new Item
        {
            Id = 2050,
            Name = "Explorer's Ward",
            Range = 600f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2050, 0),
            //
            ItemCategory = ItemCategory.Consumable,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Guardian's Horn

        public static Item Guardians_Horn = new Item
        {
            Id = 2051,
            Name = "Guardian's Horn",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 445,
            RecipeItems = new[] { 1006, 1028 },
            SellPrice = GetReducedPrice(2051, 1025),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.Armor & ItemCategory.SpellBlock & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Poro-Snax

        public static Item PoroSnax = new Item
        {
            Id = 2052,
            Name = "Poro-Snax",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2052, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Raptor Cloak

        public static Item Raptor_Cloak = new Item
        {
            Id = 2053,
            Name = "Raptor Cloak",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 520,
            RecipeItems = new[] { 1006, 1029 },
            SellPrice = GetReducedPrice(2053, 1000),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.Armor,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Diet Poro-Snax

        public static Item Diet_PoroSnax = new Item
        {
            Id = 2054,
            Name = "Diet Poro-Snax",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2054, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Elixir of Ruin

        public static Item Elixir_of_Ruin = new Item
        {
            Id = 2137,
            Name = "Elixir of Ruin",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 400,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2137, 400),
            //
            ItemCategory = ItemCategory.Consumable & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Elixir of Iron

        public static Item Elixir_of_Iron = new Item
        {
            Id = 2138,
            Name = "Elixir of Iron",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 400,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2138, 400),
            //
            ItemCategory = ItemCategory.Consumable,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Elixir of Sorcery

        public static Item Elixir_of_Sorcery = new Item
        {
            Id = 2139,
            Name = "Elixir of Sorcery",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 400,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2139, 400),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.ManaRegen & ItemCategory.Consumable,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Elixir of Wrath

        public static Item Elixir_of_Wrath = new Item
        {
            Id = 2140,
            Name = "Elixir of Wrath",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 400,
            RecipeItems = null,
            SellPrice = GetReducedPrice(2140, 400),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.LifeSteal & ItemCategory.Consumable,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Abyssal Scepter

        public static Item Abyssal_Scepter = new Item
        {
            Id = 3001,
            Name = "Abyssal Scepter",
            Range = 700f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 730,
            RecipeItems = new[] { 1026, 1057 },
            SellPrice = GetReducedPrice(3001, 1940),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.SpellBlock,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Archangel's Staff

        public static Item Archangels_Staff = new Item
        {
            Id = 3003,
            Name = "Archangel's Staff",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1120,
            RecipeItems = new[] { 3070, 1026 },
            SellPrice = GetReducedPrice(3003, 2120),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.SpellDamage & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Manamune

        public static Item Manamune = new Item
        {
            Id = 3004,
            Name = "Manamune",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 605,
            RecipeItems = new[] { 1037, 3070 },
            SellPrice = GetReducedPrice(3004, 1620),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.Damage & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Atma's Impaler

        public static Item Atmas_Impaler = new Item
        {
            Id = 3005,
            Name = "Atma's Impaler",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 700,
            RecipeItems = new[] { 3093, 1031 },
            SellPrice = GetReducedPrice(3005, 1550),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.Armor & ItemCategory.CriticalStrike,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0.15f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Berserker's Greaves

        public static Item Berserkers_Greaves = new Item
        {
            Id = 3006,
            Name = "Berserker's Greaves",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 225,
            RecipeItems = new[] { 1001, 1042 },
            SellPrice = GetReducedPrice(3006, 1000),
            //
            ItemCategory = ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Archangel's Staff (Crystal Scar)

        public static Item Archangels_Staff_Crystal_Scar = new Item
        {
            Id = 3007,
            Name = "Archangel's Staff (Crystal Scar)",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1120,
            RecipeItems = new[] { 3073, 1026 },
            SellPrice = GetReducedPrice(3007, 2120),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.SpellDamage & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Manamune (Crystal Scar)

        public static Item Manamune_Crystal_Scar = new Item
        {
            Id = 3008,
            Name = "Manamune (Crystal Scar)",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 605,
            RecipeItems = new[] { 1037, 3073 },
            SellPrice = GetReducedPrice(3008, 1620),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.Damage & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Boots of Swiftness

        public static Item Boots_of_Swiftness = new Item
        {
            Id = 3009,
            Name = "Boots of Swiftness",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 675,
            RecipeItems = new[] { 1001 },
            SellPrice = GetReducedPrice(3009, 1000),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Catalyst the Protector

        public static Item Catalyst_the_Protector = new Item
        {
            Id = 3010,
            Name = "Catalyst the Protector",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 400,
            RecipeItems = new[] { 1027, 1028 },
            SellPrice = GetReducedPrice(3010, 1200),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.HealthRegen & ItemCategory.ManaRegen & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Sorcerer's Shoes

        public static Item Sorcerers_Shoes = new Item
        {
            Id = 3020,
            Name = "Sorcerer's Shoes",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 775,
            RecipeItems = new[] { 1001 },
            SellPrice = GetReducedPrice(3020, 1100),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Frozen Mallet

        public static Item Frozen_Mallet = new Item
        {
            Id = 3022,
            Name = "Frozen Mallet",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1025,
            RecipeItems = new[] { 1037, 1011, 1028 },
            SellPrice = GetReducedPrice(3022, 3300),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Twin Shadows

        public static Item Twin_Shadows = new Item
        {
            Id = 3023,
            Name = "Twin Shadows",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 630,
            RecipeItems = new[] { 3113, 3108 },
            SellPrice = GetReducedPrice(3023, 1530),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0.06f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Glacial Shroud

        public static Item Glacial_Shroud = new Item
        {
            Id = 3024,
            Name = "Glacial Shroud",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 250,
            RecipeItems = new[] { 1027, 1029 },
            SellPrice = GetReducedPrice(3024, 950),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.Armor,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Iceborn Gauntlet

        public static Item Iceborn_Gauntlet = new Item
        {
            Id = 3025,
            Name = "Iceborn Gauntlet",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 750,
            RecipeItems = new[] { 3057, 3024 },
            SellPrice = GetReducedPrice(3025, 1365),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.SpellDamage & ItemCategory.Armor,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Guardian Angel

        public static Item Guardian_Angel = new Item
        {
            Id = 3026,
            Name = "Guardian Angel",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1250,
            RecipeItems = new[] { 1057, 1031 },
            SellPrice = GetReducedPrice(3026, 2050),
            //
            ItemCategory = ItemCategory.Armor & ItemCategory.SpellBlock,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Rod of Ages

        public static Item Rod_of_Ages = new Item
        {
            Id = 3027,
            Name = "Rod of Ages",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 740,
            RecipeItems = new[] { 3010, 1026 },
            SellPrice = GetReducedPrice(3027, 2000),
            //
            ItemCategory =
                ItemCategory.Mana & ItemCategory.SpellDamage & ItemCategory.HealthRegen & ItemCategory.ManaRegen &
                ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Chalice of Harmony

        public static Item Chalice_of_Harmony = new Item
        {
            Id = 3028,
            Name = "Chalice of Harmony",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 140,
            RecipeItems = new[] { 1033, 1004 },
            SellPrice = GetReducedPrice(3028, 820),
            //
            ItemCategory = ItemCategory.SpellBlock & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Rod of Ages (Crystal Scar)

        public static Item Rod_of_Ages_Crystal_Scar = new Item
        {
            Id = 3029,
            Name = "Rod of Ages (Crystal Scar)",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 740,
            RecipeItems = new[] { 3010, 1026 },
            SellPrice = GetReducedPrice(3029, 2000),
            //
            ItemCategory =
                ItemCategory.Mana & ItemCategory.SpellDamage & ItemCategory.HealthRegen & ItemCategory.ManaRegen &
                ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Infinity Edge

        public static Item Infinity_Edge = new Item
        {
            Id = 3031,
            Name = "Infinity Edge",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 645,
            RecipeItems = new[] { 1018, 1037, 1038 },
            SellPrice = GetReducedPrice(3031, 3800),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.CriticalStrike,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0.25f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Last Whisper

        public static Item Last_Whisper = new Item
        {
            Id = 3035,
            Name = "Last Whisper",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1065,
            RecipeItems = new[] { 1036, 1037 },
            SellPrice = GetReducedPrice(3035, 2300),
            //
            ItemCategory = ItemCategory.Damage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Seraph's Embrace

        public static Item Seraphs_Embrace = new Item
        {
            Id = 3040,
            Name = "Seraph's Embrace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 2700,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3040, 2700),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Mejai's Soulstealer

        public static Item Mejais_Soulstealer = new Item
        {
            Id = 3041,
            Name = "Mejai's Soulstealer",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 965,
            RecipeItems = new[] { 1052 },
            SellPrice = GetReducedPrice(3041, 1400),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Muramana

        public static Item Muramana = new Item
        {
            Id = 3042,
            Name = "Muramana",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 2200,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3042, 2200),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Muramana

        public static Item Muramana2 = new Item
        {
            Id = 3043,
            Name = "Muramana",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 2200,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3043, 2200),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Phage

        public static Item Phage = new Item
        {
            Id = 3044,
            Name = "Phage",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 565,
            RecipeItems = new[] { 1036, 1028 },
            SellPrice = GetReducedPrice(3044, 1325),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Phantom Dancer

        public static Item Phantom_Dancer = new Item
        {
            Id = 3046,
            Name = "Phantom Dancer",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 520,
            RecipeItems = new[] { 1018, 3086, 1042 },
            SellPrice = GetReducedPrice(3046, 1950),
            //
            ItemCategory = ItemCategory.AttackSpeed & ItemCategory.CriticalStrike,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0.05f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0.3f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ninja Tabi

        public static Item Ninja_Tabi = new Item
        {
            Id = 3047,
            Name = "Ninja Tabi",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 375,
            RecipeItems = new[] { 1001, 1029 },
            SellPrice = GetReducedPrice(3047, 1000),
            //
            ItemCategory = ItemCategory.Armor,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Seraph's Embrace

        public static Item Seraphs_Embrace2 = new Item
        {
            Id = 3048,
            Name = "Seraph's Embrace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 2700,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3048, 2700),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Zeke's Herald

        public static Item Zekes_Herald = new Item
        {
            Id = 3050,
            Name = "Zeke's Herald",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 800,
            RecipeItems = new[] { 3067, 1053 },
            SellPrice = GetReducedPrice(3050, 1690),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.LifeSteal & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ohmwrecker

        public static Item Ohmwrecker = new Item
        {
            Id = 3056,
            Name = "Ohmwrecker",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 750,
            RecipeItems = new[] { 3067, 2053 },
            SellPrice = GetReducedPrice(3056, 1720),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.Armor & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Sheen

        public static Item Sheen = new Item
        {
            Id = 3057,
            Name = "Sheen",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 365,
            RecipeItems = new[] { 1052, 1027 },
            SellPrice = GetReducedPrice(3057, 1200),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Banner of Command

        public static Item Banner_of_Command = new Item
        {
            Id = 3060,
            Name = "Banner of Command",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 280,
            RecipeItems = new[] { 3105, 3108 },
            SellPrice = GetReducedPrice(3060, 1485),
            //
            ItemCategory =
                ItemCategory.SpellDamage & ItemCategory.HealthRegen & ItemCategory.SpellBlock & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Spirit Visage

        public static Item Spirit_Visage = new Item
        {
            Id = 3065,
            Name = "Spirit Visage",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 700,
            RecipeItems = new[] { 3067, 3211 },
            SellPrice = GetReducedPrice(3065, 1450),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.SpellBlock & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Kindlegem

        public static Item Kindlegem = new Item
        {
            Id = 3067,
            Name = "Kindlegem",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 450,
            RecipeItems = new[] { 1028 },
            SellPrice = GetReducedPrice(3067, 850),
            //
            ItemCategory = ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Sunfire Cape

        public static Item Sunfire_Cape = new Item
        {
            Id = 3068,
            Name = "Sunfire Cape",
            Range = 400f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 850,
            RecipeItems = new[] { 1011, 1031 },
            SellPrice = GetReducedPrice(3068, 2300),
            //
            ItemCategory = ItemCategory.Armor & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Talisman of Ascension

        public static Item Talisman_of_Ascension = new Item
        {
            Id = 3069,
            Name = "Talisman of Ascension",
            Range = 600f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 635,
            RecipeItems = new[] { 3114, 3096 },
            SellPrice = GetReducedPrice(3069, 1375),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Tear of the Goddess

        public static Item Tear_of_the_Goddess = new Item
        {
            Id = 3070,
            Name = "Tear of the Goddess",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 140,
            RecipeItems = new[] { 1004, 1027 },
            SellPrice = GetReducedPrice(3070, 720),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region The Black Cleaver

        public static Item The_Black_Cleaver = new Item
        {
            Id = 3071,
            Name = "The Black Cleaver",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1263,
            RecipeItems = new[] { 3134, 1028 },
            SellPrice = GetReducedPrice(3071, 2280),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region The Bloodthirster

        public static Item The_Bloodthirster = new Item
        {
            Id = 3072,
            Name = "The Bloodthirster",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1150,
            RecipeItems = new[] { 1053, 1038 },
            SellPrice = GetReducedPrice(3072, 3140),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.LifeSteal,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Tear of the Goddess (Crystal Scar)

        public static Item Tear_of_the_Goddess_Crystal_Scar = new Item
        {
            Id = 3073,
            Name = "Tear of the Goddess (Crystal Scar)",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 140,
            RecipeItems = new[] { 1004, 1027 },
            SellPrice = GetReducedPrice(3073, 720),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ravenous Hydra (Melee Only)

        public static Item Ravenous_Hydra_Melee_Only = new Item
        {
            Id = 3074,
            Name = "Ravenous Hydra (Melee Only)",
            Range = 400f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 600,
            RecipeItems = new[] { 1053, 3077 },
            SellPrice = GetReducedPrice(3074, 1345),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.HealthRegen & ItemCategory.LifeSteal,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Thornmail

        public static Item Thornmail = new Item
        {
            Id = 3075,
            Name = "Thornmail",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1050,
            RecipeItems = new[] { 1029, 1031 },
            SellPrice = GetReducedPrice(3075, 1800),
            //
            ItemCategory = ItemCategory.Armor,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Tiamat (Melee Only)

        public static Item Tiamat_Melee_Only = new Item
        {
            Id = 3077,
            Name = "Tiamat (Melee Only)",
            Range = 400f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 305,
            RecipeItems = new[] { 1036, 1037, 1006 },
            SellPrice = GetReducedPrice(3077, 1720),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.HealthRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Trinity Force

        public static Item Trinity_Force = new Item
        {
            Id = 3078,
            Name = "Trinity Force",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 78,
            RecipeItems = new[] { 3086, 3057, 3044 },
            SellPrice = GetReducedPrice(3078, 1258),
            //
            ItemCategory =
                ItemCategory.Mana & ItemCategory.SpellDamage & ItemCategory.Damage & ItemCategory.AttackSpeed &
                ItemCategory.CriticalStrike & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0.08f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0.1f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Warden's Mail

        public static Item Wardens_Mail = new Item
        {
            Id = 3082,
            Name = "Warden's Mail",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 450,
            RecipeItems = new[] { 1029 },
            SellPrice = GetReducedPrice(3082, 750),
            //
            ItemCategory = ItemCategory.Armor,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Warmog's Armor

        public static Item Warmogs_Armor = new Item
        {
            Id = 3083,
            Name = "Warmog's Armor",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 300,
            RecipeItems = new[] { 3801, 1011 },
            SellPrice = GetReducedPrice(3083, 1320),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Overlord's Bloodmail

        public static Item Overlords_Bloodmail = new Item
        {
            Id = 3084,
            Name = "Overlord's Bloodmail",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1055,
            RecipeItems = new[] { 1011, 1028 },
            SellPrice = GetReducedPrice(3084, 2455),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Runaan's Hurricane (Ranged Only)

        public static Item Runaans_Hurricane_Ranged_Only = new Item
        {
            Id = 3085,
            Name = "Runaan's Hurricane (Ranged Only)",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 600,
            RecipeItems = new[] { 1043, 1042 },
            SellPrice = GetReducedPrice(3085, 1950),
            //
            ItemCategory = ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Zeal

        public static Item Zeal = new Item
        {
            Id = 3086,
            Name = "Zeal",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 250,
            RecipeItems = new[] { 1051, 1042 },
            SellPrice = GetReducedPrice(3086, 1100),
            //
            ItemCategory = ItemCategory.AttackSpeed & ItemCategory.CriticalStrike,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0.05f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0.1f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Statikk Shiv

        public static Item Statikk_Shiv = new Item
        {
            Id = 3087,
            Name = "Statikk Shiv",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 600,
            RecipeItems = new[] { 3086, 3093 },
            SellPrice = GetReducedPrice(3087, 1250),
            //
            ItemCategory = ItemCategory.AttackSpeed & ItemCategory.CriticalStrike,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0.06f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0.2f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Rabadon's Deathcap

        public static Item Rabadons_Deathcap = new Item
        {
            Id = 3089,
            Name = "Rabadon's Deathcap",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 840,
            RecipeItems = new[] { 1058, 1026 },
            SellPrice = GetReducedPrice(3089, 3300),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Wooglet's Witchcap

        public static Item Wooglets_Witchcap = new Item
        {
            Id = 3090,
            Name = "Wooglet's Witchcap",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1045,
            RecipeItems = new[] { 1052, 3191, 1026 },
            SellPrice = GetReducedPrice(3090, 2805),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.Armor,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Wit's End

        public static Item Wits_End = new Item
        {
            Id = 3091,
            Name = "Wit's End",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 750,
            RecipeItems = new[] { 1033, 1043, 1042 },
            SellPrice = GetReducedPrice(3091, 2600),
            //
            ItemCategory = ItemCategory.AttackSpeed & ItemCategory.SpellBlock,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Frost Queen's Claim

        public static Item Frost_Queens_Claim = new Item
        {
            Id = 3092,
            Name = "Frost Queen's Claim",
            Range = 850f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 515,
            RecipeItems = new[] { 3098, 3108 },
            SellPrice = GetReducedPrice(3092, 1400),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Avarice Blade

        public static Item Avarice_Blade = new Item
        {
            Id = 3093,
            Name = "Avarice Blade",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 400,
            RecipeItems = new[] { 1051 },
            SellPrice = GetReducedPrice(3093, 800),
            //
            ItemCategory = ItemCategory.CriticalStrike,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0.1f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Nomad's Medallion

        public static Item Nomads_Medallion = new Item
        {
            Id = 3096,
            Name = "Nomad's Medallion",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 500,
            RecipeItems = new[] { 3301 },
            SellPrice = GetReducedPrice(3096, 865),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Targon's Brace

        public static Item Targons_Brace = new Item
        {
            Id = 3097,
            Name = "Targon's Brace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 500,
            RecipeItems = new[] { 3302 },
            SellPrice = GetReducedPrice(3097, 865),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Frostfang

        public static Item Frostfang = new Item
        {
            Id = 3098,
            Name = "Frostfang",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 500,
            RecipeItems = new[] { 3303 },
            SellPrice = GetReducedPrice(3098, 865),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Soul Shroud

        public static Item Soul_Shroud = new Item
        {
            Id = 3099,
            Name = "Soul Shroud",
            Range = 1200f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 485,
            RecipeItems = new[] { 3067, 1028 },
            SellPrice = GetReducedPrice(3099, 1335),
            //
            ItemCategory = ItemCategory.ManaRegen & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Lich Bane

        public static Item Lich_Bane = new Item
        {
            Id = 3100,
            Name = "Lich Bane",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 850,
            RecipeItems = new[] { 3113, 3057 },
            SellPrice = GetReducedPrice(3100, 1730),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0.05f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Stinger

        public static Item Stinger = new Item
        {
            Id = 3101,
            Name = "Stinger",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 350,
            RecipeItems = new[] { 1042 },
            SellPrice = GetReducedPrice(3101, 800),
            //
            ItemCategory = ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Banshee's Veil

        public static Item Banshees_Veil = new Item
        {
            Id = 3102,
            Name = "Banshee's Veil",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1150,
            RecipeItems = new[] { 3211, 1028 },
            SellPrice = GetReducedPrice(3102, 1850),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.SpellBlock & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Lord Van Damm's Pillager

        public static Item Lord_Van_Damms_Pillager = new Item
        {
            Id = 3104,
            Name = "Lord Van Damm's Pillager",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 995,
            RecipeItems = new[] { 1018, 1037, 3122 },
            SellPrice = GetReducedPrice(3104, 3040),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.CriticalStrike,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0.25f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Aegis of the Legion

        public static Item Aegis_of_the_Legion = new Item
        {
            Id = 3105,
            Name = "Aegis of the Legion",
            Range = 1100f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 820,
            RecipeItems = new[] { 1033, 1006, 1028 },
            SellPrice = GetReducedPrice(3105, 1900),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.SpellBlock & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Madred's Razors

        public static Item Madreds_Razors = new Item
        {
            Id = 3106,
            Name = "Madred's Razors",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 300,
            RecipeItems = new[] { 1042 },
            SellPrice = GetReducedPrice(3106, 750),
            //
            ItemCategory = ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Runic Bulwark

        public static Item Runic_Bulwark = new Item
        {
            Id = 3107,
            Name = "Runic Bulwark",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 400,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3107, 400),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.Armor & ItemCategory.SpellBlock & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Fiendish Codex

        public static Item Fiendish_Codex = new Item
        {
            Id = 3108,
            Name = "Fiendish Codex",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 385,
            RecipeItems = new[] { 1052 },
            SellPrice = GetReducedPrice(3108, 820),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Force of Nature

        public static Item Force_of_Nature = new Item
        {
            Id = 3109,
            Name = "Force of Nature",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1000,
            RecipeItems = new[] { 1006, 1057 },
            SellPrice = GetReducedPrice(3109, 1530),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.SpellBlock,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0.08f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Frozen Heart

        public static Item Frozen_Heart = new Item
        {
            Id = 3110,
            Name = "Frozen Heart",
            Range = 700f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 450,
            RecipeItems = new[] { 3082, 3024 },
            SellPrice = GetReducedPrice(3110, 1150),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.Armor,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Mercury's Treads

        public static Item Mercurys_Treads = new Item
        {
            Id = 3111,
            Name = "Mercury's Treads",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 375,
            RecipeItems = new[] { 1001, 1033 },
            SellPrice = GetReducedPrice(3111, 1200),
            //
            ItemCategory = ItemCategory.SpellBlock,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Orb of Winter

        public static Item Orb_of_Winter = new Item
        {
            Id = 3112,
            Name = "Orb of Winter",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 850,
            RecipeItems = new[] { 1033, 1006 },
            SellPrice = GetReducedPrice(3112, 1530),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.SpellBlock,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Aether Wisp

        public static Item Aether_Wisp = new Item
        {
            Id = 3113,
            Name = "Aether Wisp",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 515,
            RecipeItems = new[] { 1052 },
            SellPrice = GetReducedPrice(3113, 950),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Forbidden Idol

        public static Item Forbidden_Idol = new Item
        {
            Id = 3114,
            Name = "Forbidden Idol",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 240,
            RecipeItems = new[] { 1004 },
            SellPrice = GetReducedPrice(3114, 420),
            //
            ItemCategory = ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Nashor's Tooth

        public static Item Nashors_Tooth = new Item
        {
            Id = 3115,
            Name = "Nashor's Tooth",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 850,
            RecipeItems = new[] { 3101, 3108 },
            SellPrice = GetReducedPrice(3115, 1585),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Rylai's Crystal Scepter

        public static Item Rylais_Crystal_Scepter = new Item
        {
            Id = 3116,
            Name = "Rylai's Crystal Scepter",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 605,
            RecipeItems = new[] { 1052, 1026, 1011 },
            SellPrice = GetReducedPrice(3116, 2900),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Boots of Mobility

        public static Item Boots_of_Mobility = new Item
        {
            Id = 3117,
            Name = "Boots of Mobility",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 1001 },
            SellPrice = GetReducedPrice(3117, 800),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Wicked Hatchet

        public static Item Wicked_Hatchet = new Item
        {
            Id = 3122,
            Name = "Wicked Hatchet",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 440,
            RecipeItems = new[] { 1051, 1036 },
            SellPrice = GetReducedPrice(3122, 1200),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.CriticalStrike,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0.1f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Executioner's Calling

        public static Item Executioners_Calling = new Item
        {
            Id = 3123,
            Name = "Executioner's Calling",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 740,
            RecipeItems = new[] { 1036, 3093 },
            SellPrice = GetReducedPrice(3123, 1500),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.CriticalStrike,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0.2f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Guinsoo's Rageblade

        public static Item Guinsoos_Rageblade = new Item
        {
            Id = 3124,
            Name = "Guinsoo's Rageblade",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 865,
            RecipeItems = new[] { 1037, 1026 },
            SellPrice = GetReducedPrice(3124, 2600),
            //
            ItemCategory =
                ItemCategory.SpellDamage & ItemCategory.Damage & ItemCategory.AttackSpeed & ItemCategory.LifeSteal,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Madred's Bloodrazor

        public static Item Madreds_Bloodrazor = new Item
        {
            Id = 3126,
            Name = "Madred's Bloodrazor",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 775,
            RecipeItems = new[] { 1037, 3106, 1043 },
            SellPrice = GetReducedPrice(3126, 2850),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Deathfire Grasp

        public static Item Deathfire_Grasp = new Item
        {
            Id = 3128,
            Name = "Deathfire Grasp",
            Range = 750f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 680,
            RecipeItems = new[] { 1058, 3108 },
            SellPrice = GetReducedPrice(3128, 2665),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Sword of the Divine

        public static Item Sword_of_the_Divine = new Item
        {
            Id = 3131,
            Name = "Sword of the Divine",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 800,
            RecipeItems = new[] { 1043, 1042 },
            SellPrice = GetReducedPrice(3131, 2150),
            //
            ItemCategory = ItemCategory.AttackSpeed & ItemCategory.CriticalStrike,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Heart of Gold

        public static Item Heart_of_Gold = new Item
        {
            Id = 3132,
            Name = "Heart of Gold",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 350,
            RecipeItems = new[] { 1028 },
            SellPrice = GetReducedPrice(3132, 750),
            //
            ItemCategory = ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region The Brutalizer

        public static Item The_Brutalizer = new Item
        {
            Id = 3134,
            Name = "The Brutalizer",
            Range = 0f,
            MaxStacks = 0,
            //
            IsRecipe = true,
            //
            Price = 617,
            RecipeItems = new[] { 1036 },
            SellPrice = GetReducedPrice(3134, 977),
            //
            ItemCategory = ItemCategory.Damage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Void Staff

        public static Item Void_Staff = new Item
        {
            Id = 3135,
            Name = "Void Staff",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1000,
            RecipeItems = new[] { 1052, 1026 },
            SellPrice = GetReducedPrice(3135, 2295),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Haunting Guise

        public static Item Haunting_Guise = new Item
        {
            Id = 3136,
            Name = "Haunting Guise",
            Range = 0f,
            MaxStacks = 0,
            //
            IsRecipe = true,
            //
            Price = 650,
            RecipeItems = new[] { 1052, 1028 },
            SellPrice = GetReducedPrice(3136, 1485),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Dervish Blade

        public static Item Dervish_Blade = new Item
        {
            Id = 3137,
            Name = "Dervish Blade",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 200,
            RecipeItems = new[] { 3101, 3140 },
            SellPrice = GetReducedPrice(3137, 1300),
            //
            ItemCategory = ItemCategory.AttackSpeed & ItemCategory.SpellBlock,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Leviathan

        public static Item Leviathan = new Item
        {
            Id = 3138,
            Name = "Leviathan",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 800,
            RecipeItems = new[] { 1028 },
            SellPrice = GetReducedPrice(3138, 1200),
            //
            ItemCategory = ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Mercurial Scimitar

        public static Item Mercurial_Scimitar = new Item
        {
            Id = 3139,
            Name = "Mercurial Scimitar",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 900,
            RecipeItems = new[] { 1038, 3140 },
            SellPrice = GetReducedPrice(3139, 3200),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.SpellBlock,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Quicksilver Sash

        public static Item Quicksilver_Sash = new Item
        {
            Id = 3140,
            Name = "Quicksilver Sash",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 750,
            RecipeItems = new[] { 1033 },
            SellPrice = GetReducedPrice(3140, 1250),
            //
            ItemCategory = ItemCategory.SpellBlock,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Sword of the Occult

        public static Item Sword_of_the_Occult = new Item
        {
            Id = 3141,
            Name = "Sword of the Occult",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1040,
            RecipeItems = new[] { 1036 },
            SellPrice = GetReducedPrice(3141, 1400),
            //
            ItemCategory = ItemCategory.Damage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Youmuu's Ghostblade

        public static Item Youmuus_Ghostblade = new Item
        {
            Id = 3142,
            Name = "Youmuu's Ghostblade",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 563,
            RecipeItems = new[] { 3134, 3093 },
            SellPrice = GetReducedPrice(3142, 1580),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.AttackSpeed & ItemCategory.CriticalStrike,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0.15f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Randuin's Omen

        public static Item Randuins_Omen = new Item
        {
            Id = 3143,
            Name = "Randuin's Omen",
            Range = 500f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 800,
            RecipeItems = new[] { 3082, 1011 },
            SellPrice = GetReducedPrice(3143, 2250),
            //
            ItemCategory = ItemCategory.Armor & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bilgewater Cutlass

        public static Item Bilgewater_Cutlass = new Item
        {
            Id = 3144,
            Name = "Bilgewater Cutlass",
            Range = 450f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 240,
            RecipeItems = new[] { 1053, 1036 },
            SellPrice = GetReducedPrice(3144, 1040),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.LifeSteal,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Hextech Revolver

        public static Item Hextech_Revolver = new Item
        {
            Id = 3145,
            Name = "Hextech Revolver",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 330,
            RecipeItems = new[] { 1052 },
            SellPrice = GetReducedPrice(3145, 765),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Hextech Gunblade

        public static Item Hextech_Gunblade = new Item
        {
            Id = 3146,
            Name = "Hextech Gunblade",
            Range = 700f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 800,
            RecipeItems = new[] { 3144, 3145 },
            SellPrice = GetReducedPrice(3146, 1370),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.Damage & ItemCategory.LifeSteal,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Liandry's Torment

        public static Item Liandrys_Torment = new Item
        {
            Id = 3151,
            Name = "Liandry's Torment",
            Range = 0f,
            MaxStacks = 0,
            //
            IsRecipe = true,
            //
            Price = 980,
            RecipeItems = new[] { 1052, 3136 },
            SellPrice = GetReducedPrice(3151, 2065),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Will of the Ancients

        public static Item Will_of_the_Ancients = new Item
        {
            Id = 3152,
            Name = "Will of the Ancients",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 480,
            RecipeItems = new[] { 3145, 3108 },
            SellPrice = GetReducedPrice(3152, 1195),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Blade of the Ruined King

        public static Item Blade_of_the_Ruined_King = new Item
        {
            Id = 3153,
            Name = "Blade of the Ruined King",
            Range = 450f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 900,
            RecipeItems = new[] { 3144, 1042 },
            SellPrice = GetReducedPrice(3153, 1590),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.AttackSpeed & ItemCategory.LifeSteal,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Wriggle's Lantern

        public static Item Wriggles_Lantern = new Item
        {
            Id = 3154,
            Name = "Wriggle's Lantern",
            Range = 600f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 215,
            RecipeItems = new[] { 1036, 3106, 1042 },
            SellPrice = GetReducedPrice(3154, 1325),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Hexdrinker

        public static Item Hexdrinker = new Item
        {
            Id = 3155,
            Name = "Hexdrinker",
            Range = 0f,
            MaxStacks = 0,
            //
            IsRecipe = true,
            //
            Price = 590,
            RecipeItems = new[] { 1033, 1036 },
            SellPrice = GetReducedPrice(3155, 1450),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.SpellBlock,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Maw of Malmortius

        public static Item Maw_of_Malmortius = new Item
        {
            Id = 3156,
            Name = "Maw of Malmortius",
            Range = 0f,
            MaxStacks = 0,
            //
            IsRecipe = true,
            //
            Price = 875,
            RecipeItems = new[] { 1037, 3155 },
            SellPrice = GetReducedPrice(3156, 2340),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.SpellBlock,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Zhonya's Hourglass

        public static Item Zhonyas_Hourglass = new Item
        {
            Id = 3157,
            Name = "Zhonya's Hourglass",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 500,
            RecipeItems = new[] { 1058, 3191 },
            SellPrice = GetReducedPrice(3157, 2565),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.Armor,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ionian Boots of Lucidity

        public static Item Ionian_Boots_of_Lucidity = new Item
        {
            Id = 3158,
            Name = "Ionian Boots of Lucidity",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 675,
            RecipeItems = new[] { 1001 },
            SellPrice = GetReducedPrice(3158, 1000),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Grez's Spectral Lantern

        public static Item Grezs_Spectral_Lantern = new Item
        {
            Id = 3159,
            Name = "Grez's Spectral Lantern",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 180,
            RecipeItems = new[] { 1036, 3106, 1042 },
            SellPrice = GetReducedPrice(3159, 1290),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Feral Flare

        public static Item Feral_Flare = new Item
        {
            Id = 3160,
            Name = "Feral Flare",
            Range = 600f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1800,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3160, 1800),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Morellonomicon

        public static Item Morellonomicon = new Item
        {
            Id = 3165,
            Name = "Morellonomicon",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 680,
            RecipeItems = new[] { 3114, 3108 },
            SellPrice = GetReducedPrice(3165, 1305),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace = new Item
        {
            Id = 3166,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3166, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace2 = new Item
        {
            Id = 3167,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3167, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace3 = new Item
        {
            Id = 3168,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3168, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace4 = new Item
        {
            Id = 3169,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3169, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Moonflair Spellblade

        public static Item Moonflair_Spellblade = new Item
        {
            Id = 3170,
            Name = "Moonflair Spellblade",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 920,
            RecipeItems = new[] { 1033, 3191 },
            SellPrice = GetReducedPrice(3170, 1885),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.Armor & ItemCategory.SpellBlock,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace30 = new Item
        {
            Id = 3171,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3171, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Zephyr

        public static Item Zephyr = new Item
        {
            Id = 3172,
            Name = "Zephyr",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 725,
            RecipeItems = new[] { 3101, 1037 },
            SellPrice = GetReducedPrice(3172, 1950),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0.1f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Athene's Unholy Grail

        public static Item Athenes_Unholy_Grail = new Item
        {
            Id = 3174,
            Name = "Athene's Unholy Grail",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 880,
            RecipeItems = new[] { 3028, 3108 },
            SellPrice = GetReducedPrice(3174, 1405),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.SpellBlock & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Head of Kha'Zix

        public static Item Head_of_Kha_Zix = new Item
        {
            Id = 3175,
            Name = "Head of Kha'Zix",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3175, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Arcane Helix

        public static Item Arcane_Helix = new Item
        {
            Id = 3176,
            Name = "Arcane Helix",
            Range = 0f,
            MaxStacks = 0,
            //
            IsRecipe = true,
            //
            Price = 500,
            RecipeItems = new[] { 3010, 1028 },
            SellPrice = GetReducedPrice(3176, 1300),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ionic Spark

        public static Item Ionic_Spark = new Item
        {
            Id = 3178,
            Name = "Ionic Spark",
            Range = 400f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 575,
            RecipeItems = new[] { 1043, 1028 },
            SellPrice = GetReducedPrice(3178, 1875),
            //
            ItemCategory = ItemCategory.AttackSpeed & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Odyn's Veil

        public static Item Odyns_Veil = new Item
        {
            Id = 3180,
            Name = "Odyn's Veil",
            Range = 525f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 800,
            RecipeItems = new[] { 1033, 3010 },
            SellPrice = GetReducedPrice(3180, 1700),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.SpellBlock & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Sanguine Blade

        public static Item Sanguine_Blade = new Item
        {
            Id = 3181,
            Name = "Sanguine Blade",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 600,
            RecipeItems = new[] { 1053, 1037 },
            SellPrice = GetReducedPrice(3181, 1915),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.LifeSteal,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Priscilla's Blessing

        public static Item Priscillas_Blessing = new Item
        {
            Id = 3183,
            Name = "Priscilla's Blessing",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 9999999,
            RecipeItems = new[] { 1006 },
            SellPrice = GetReducedPrice(3183, 10000179),
            //
            ItemCategory = ItemCategory.HealthRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Entropy

        public static Item Entropy = new Item
        {
            Id = 3184,
            Name = "Entropy",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 500,
            RecipeItems = new[] { 1037, 3044 },
            SellPrice = GetReducedPrice(3184, 1940),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region The Lightbringer

        public static Item The_Lightbringer = new Item
        {
            Id = 3185,
            Name = "The Lightbringer",
            Range = 800f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 350,
            RecipeItems = new[] { 1018, 3122 },
            SellPrice = GetReducedPrice(3185, 1520),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.CriticalStrike,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0.3f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Kitae's Bloodrazor

        public static Item Kitaes_Bloodrazor = new Item
        {
            Id = 3186,
            Name = "Kitae's Bloodrazor",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 700,
            RecipeItems = new[] { 1037, 1043 },
            SellPrice = GetReducedPrice(3186, 2475),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Hextech Sweeper

        public static Item Hextech_Sweeper = new Item
        {
            Id = 3187,
            Name = "Hextech Sweeper",
            Range = 800f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 330,
            RecipeItems = new[] { 3067, 3024 },
            SellPrice = GetReducedPrice(3187, 1030),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.Armor & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Blackfire Torch

        public static Item Blackfire_Torch = new Item
        {
            Id = 3188,
            Name = "Blackfire Torch",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 970,
            RecipeItems = new[] { 1026, 3108 },
            SellPrice = GetReducedPrice(3188, 2215),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Locket of the Iron Solari

        public static Item Locket_of_the_Iron_Solari = new Item
        {
            Id = 3190,
            Name = "Locket of the Iron Solari",
            Range = 600f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 50,
            RecipeItems = new[] { 3067, 3105 },
            SellPrice = GetReducedPrice(3190, 1320),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.SpellBlock & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Seeker's Armguard

        public static Item Seekers_Armguard = new Item
        {
            Id = 3191,
            Name = "Seeker's Armguard",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 465,
            RecipeItems = new[] { 1052, 1029 },
            SellPrice = GetReducedPrice(3191, 1200),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.Armor,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region The Hex Core mk-1

        public static Item The_Hex_Core_mk1 = new Item
        {
            Id = 3196,
            Name = "The Hex Core mk-1",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1000,
            RecipeItems = new[] { 3200 },
            SellPrice = GetReducedPrice(3196, 1000),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region The Hex Core mk-2

        public static Item The_Hex_Core_mk2 = new Item
        {
            Id = 3197,
            Name = "The Hex Core mk-2",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1000,
            RecipeItems = new[] { 3196 },
            SellPrice = GetReducedPrice(3197, 2000),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Perfect Hex Core

        public static Item Perfect_Hex_Core = new Item
        {
            Id = 3198,
            Name = "Perfect Hex Core",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 1000,
            RecipeItems = new[] { 3197 },
            SellPrice = GetReducedPrice(3198, 2000),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Prototype Hex Core

        public static Item Prototype_Hex_Core = new Item
        {
            Id = 3200,
            Name = "Prototype Hex Core",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3200, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Quill Coat

        public static Item Quill_Coat = new Item
        {
            Id = 3204,
            Name = "Quill Coat",
            Range = 600f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 75,
            RecipeItems = new[] { 1039, 1029 },
            SellPrice = GetReducedPrice(3204, 775),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.Armor & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Quill Coat

        public static Item Quill_Coat2 = new Item
        {
            Id = 3205,
            Name = "Quill Coat",
            Range = 600f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 75,
            RecipeItems = new[] { 1039, 1029 },
            SellPrice = GetReducedPrice(3205, 775),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.Armor & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Spirit of the Spectral Wraith

        public static Item Spirit_of_the_Spectral_Wraith = new Item
        {
            Id = 3206,
            Name = "Spirit of the Spectral Wraith",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 480,
            RecipeItems = new[] { 1080, 3108 },
            SellPrice = GetReducedPrice(3206, 880),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Spirit of the Ancient Golem

        public static Item Spirit_of_the_Ancient_Golem = new Item
        {
            Id = 3207,
            Name = "Spirit of the Ancient Golem",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 450,
            RecipeItems = new[] { 3067, 3205 },
            SellPrice = GetReducedPrice(3207, 975),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.Armor & ItemCategory.ManaRegen & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Spirit of the Ancient Golem

        public static Item Spirit_of_the_Ancient_Golem2 = new Item
        {
            Id = 3208,
            Name = "Spirit of the Ancient Golem",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 450,
            RecipeItems = new[] { 3067, 3204 },
            SellPrice = GetReducedPrice(3208, 975),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.Armor & ItemCategory.ManaRegen & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Spirit of the Elder Lizard

        public static Item Spirit_of_the_Elder_Lizard2 = new Item
        {
            Id = 3209,
            Name = "Spirit of the Elder Lizard",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 580,
            RecipeItems = new[] { 1080, 1036 },
            SellPrice = GetReducedPrice(3209, 955),
            //
            ItemCategory = ItemCategory.Damage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Spellbreaker (Melee Only)

        public static Item Spellbreaker_Melee_Only = new Item
        {
            Id = 3210,
            Name = "Spellbreaker (Melee Only)",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 650,
            RecipeItems = new[] { 3093, 3155 },
            SellPrice = GetReducedPrice(3210, 1640),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.SpellBlock & ItemCategory.CriticalStrike,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0.2f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Spectre's Cowl

        public static Item Spectres_Cowl = new Item
        {
            Id = 3211,
            Name = "Spectre's Cowl",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 300,
            RecipeItems = new[] { 1033, 1028 },
            SellPrice = GetReducedPrice(3211, 1200),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.SpellBlock & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Mikael's Crucible

        public static Item Mikaels_Crucible = new Item
        {
            Id = 3222,
            Name = "Mikael's Crucible",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 850,
            RecipeItems = new[] { 3114, 3028 },
            SellPrice = GetReducedPrice(3222, 1230),
            //
            ItemCategory = ItemCategory.SpellBlock & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Berserker's Greaves - Homeguard

        public static Item Berserkers_Greaves__Homeguard = new Item
        {
            Id = 3250,
            Name = "Berserker's Greaves - Homeguard",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3006 },
            SellPrice = GetReducedPrice(3250, 700),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Berserker's Greaves - Captain

        public static Item Berserkers_Greaves__Captain = new Item
        {
            Id = 3251,
            Name = "Berserker's Greaves - Captain",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 600,
            RecipeItems = new[] { 3006 },
            SellPrice = GetReducedPrice(3251, 825),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Berserker's Greaves - Furor

        public static Item Berserkers_Greaves__Furor = new Item
        {
            Id = 3252,
            Name = "Berserker's Greaves - Furor",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3006 },
            SellPrice = GetReducedPrice(3252, 700),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Berserker's Greaves - Distortion

        public static Item Berserkers_Greaves__Distortion = new Item
        {
            Id = 3253,
            Name = "Berserker's Greaves - Distortion",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3006 },
            SellPrice = GetReducedPrice(3253, 700),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Berserker's Greaves - Alacrity

        public static Item Berserkers_Greaves__Alacrity = new Item
        {
            Id = 3254,
            Name = "Berserker's Greaves - Alacrity",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3006 },
            SellPrice = GetReducedPrice(3254, 700),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Sorcerer's Shoes - Homeguard

        public static Item Sorcerers_Shoes__Homeguard = new Item
        {
            Id = 3255,
            Name = "Sorcerer's Shoes - Homeguard",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3020 },
            SellPrice = GetReducedPrice(3255, 1250),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Sorcerer's Shoes - Captain

        public static Item Sorcerers_Shoes__Captain = new Item
        {
            Id = 3256,
            Name = "Sorcerer's Shoes - Captain",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 600,
            RecipeItems = new[] { 3020 },
            SellPrice = GetReducedPrice(3256, 1375),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Sorcerer's Shoes - Furor

        public static Item Sorcerers_Shoes__Furor = new Item
        {
            Id = 3257,
            Name = "Sorcerer's Shoes - Furor",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3020 },
            SellPrice = GetReducedPrice(3257, 1250),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Sorcerer's Shoes - Distortion

        public static Item Sorcerers_Shoes__Distortion = new Item
        {
            Id = 3258,
            Name = "Sorcerer's Shoes - Distortion",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3020 },
            SellPrice = GetReducedPrice(3258, 1250),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Sorcerer's Shoes - Alacrity

        public static Item Sorcerers_Shoes__Alacrity = new Item
        {
            Id = 3259,
            Name = "Sorcerer's Shoes - Alacrity",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3020 },
            SellPrice = GetReducedPrice(3259, 1250),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ninja Tabi - Homeguard

        public static Item Ninja_Tabi__Homeguard = new Item
        {
            Id = 3260,
            Name = "Ninja Tabi - Homeguard",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3047 },
            SellPrice = GetReducedPrice(3260, 850),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ninja Tabi - Captain

        public static Item Ninja_Tabi__Captain = new Item
        {
            Id = 3261,
            Name = "Ninja Tabi - Captain",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 600,
            RecipeItems = new[] { 3047 },
            SellPrice = GetReducedPrice(3261, 975),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ninja Tabi - Furor

        public static Item Ninja_Tabi__Furor = new Item
        {
            Id = 3262,
            Name = "Ninja Tabi - Furor",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3047 },
            SellPrice = GetReducedPrice(3262, 850),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ninja Tabi - Distortion

        public static Item Ninja_Tabi__Distortion = new Item
        {
            Id = 3263,
            Name = "Ninja Tabi - Distortion",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3047 },
            SellPrice = GetReducedPrice(3263, 850),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ninja Tabi - Alacrity

        public static Item Ninja_Tabi__Alacrity = new Item
        {
            Id = 3264,
            Name = "Ninja Tabi - Alacrity",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3047 },
            SellPrice = GetReducedPrice(3264, 850),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Mercury's Treads - Homeguard

        public static Item Mercurys_Treads__Homeguard = new Item
        {
            Id = 3265,
            Name = "Mercury's Treads - Homeguard",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3111 },
            SellPrice = GetReducedPrice(3265, 850),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Mercury's Treads - Captain

        public static Item Mercurys_Treads__Captain = new Item
        {
            Id = 3266,
            Name = "Mercury's Treads - Captain",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 600,
            RecipeItems = new[] { 3111 },
            SellPrice = GetReducedPrice(3266, 975),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Mercury's Treads - Furor

        public static Item Mercurys_Treads__Furor = new Item
        {
            Id = 3267,
            Name = "Mercury's Treads - Furor",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3111 },
            SellPrice = GetReducedPrice(3267, 850),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Mercury's Treads - Distortion

        public static Item Mercurys_Treads__Distortion = new Item
        {
            Id = 3268,
            Name = "Mercury's Treads - Distortion",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3111 },
            SellPrice = GetReducedPrice(3268, 850),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Mercury's Treads - Alacrity

        public static Item Mercurys_Treads__Alacrity = new Item
        {
            Id = 3269,
            Name = "Mercury's Treads - Alacrity",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3111 },
            SellPrice = GetReducedPrice(3269, 850),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Boots of Mobility - Homeguard

        public static Item Boots_of_Mobility__Homeguard = new Item
        {
            Id = 3270,
            Name = "Boots of Mobility - Homeguard",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3117 },
            SellPrice = GetReducedPrice(3270, 950),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Boots of Mobility - Captain

        public static Item Boots_of_Mobility__Captain = new Item
        {
            Id = 3271,
            Name = "Boots of Mobility - Captain",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 600,
            RecipeItems = new[] { 3117 },
            SellPrice = GetReducedPrice(3271, 1075),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Boots of Mobility - Furor

        public static Item Boots_of_Mobility__Furor = new Item
        {
            Id = 3272,
            Name = "Boots of Mobility - Furor",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3117 },
            SellPrice = GetReducedPrice(3272, 950),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Boots of Mobility - Distortion

        public static Item Boots_of_Mobility__Distortion = new Item
        {
            Id = 3273,
            Name = "Boots of Mobility - Distortion",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3117 },
            SellPrice = GetReducedPrice(3273, 950),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Boots of Mobility - Alacrity

        public static Item Boots_of_Mobility__Alacrity = new Item
        {
            Id = 3274,
            Name = "Boots of Mobility - Alacrity",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3117 },
            SellPrice = GetReducedPrice(3274, 950),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ionian Boots of Lucidity - Homeguard

        public static Item Ionian_Boots_of_Lucidity__Homeguard = new Item
        {
            Id = 3275,
            Name = "Ionian Boots of Lucidity - Homeguard",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3158 },
            SellPrice = GetReducedPrice(3275, 1150),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ionian Boots of Lucidity - Captain

        public static Item Ionian_Boots_of_Lucidity__Captain = new Item
        {
            Id = 3276,
            Name = "Ionian Boots of Lucidity - Captain",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 600,
            RecipeItems = new[] { 3158 },
            SellPrice = GetReducedPrice(3276, 1275),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ionian Boots of Lucidity - Furor

        public static Item Ionian_Boots_of_Lucidity__Furor = new Item
        {
            Id = 3277,
            Name = "Ionian Boots of Lucidity - Furor",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3158 },
            SellPrice = GetReducedPrice(3277, 1150),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ionian Boots of Lucidity - Distortion

        public static Item Ionian_Boots_of_Lucidity__Distortion = new Item
        {
            Id = 3278,
            Name = "Ionian Boots of Lucidity - Distortion",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3158 },
            SellPrice = GetReducedPrice(3278, 1150),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ionian Boots of Lucidity - Alacrity

        public static Item Ionian_Boots_of_Lucidity__Alacrity = new Item
        {
            Id = 3279,
            Name = "Ionian Boots of Lucidity - Alacrity",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3158 },
            SellPrice = GetReducedPrice(3279, 1150),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Boots of Swiftness - Homeguard

        public static Item Boots_of_Swiftness__Homeguard = new Item
        {
            Id = 3280,
            Name = "Boots of Swiftness - Homeguard",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3009 },
            SellPrice = GetReducedPrice(3280, 1150),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Boots of Swiftness - Captain

        public static Item Boots_of_Swiftness__Captain = new Item
        {
            Id = 3281,
            Name = "Boots of Swiftness - Captain",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 600,
            RecipeItems = new[] { 3009 },
            SellPrice = GetReducedPrice(3281, 1275),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Boots of Swiftness - Furor

        public static Item Boots_of_Swiftness__Furor = new Item
        {
            Id = 3282,
            Name = "Boots of Swiftness - Furor",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3009 },
            SellPrice = GetReducedPrice(3282, 1150),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Boots of Swiftness - Distortion

        public static Item Boots_of_Swiftness__Distortion = new Item
        {
            Id = 3283,
            Name = "Boots of Swiftness - Distortion",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3009 },
            SellPrice = GetReducedPrice(3283, 1150),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Boots of Swiftness - Alacrity

        public static Item Boots_of_Swiftness__Alacrity = new Item
        {
            Id = 3284,
            Name = "Boots of Swiftness - Alacrity",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3009 },
            SellPrice = GetReducedPrice(3284, 1150),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Twin Shadows

        public static Item Twin_Shadows2 = new Item
        {
            Id = 3290,
            Name = "Twin Shadows",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 630,
            RecipeItems = new[] { 3113, 3108 },
            SellPrice = GetReducedPrice(3290, 1530),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0.06f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ancient Coin

        public static Item Ancient_Coin = new Item
        {
            Id = 3301,
            Name = "Ancient Coin",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 365,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3301, 365),
            //
            ItemCategory = ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Relic Shield

        public static Item Relic_Shield = new Item
        {
            Id = 3302,
            Name = "Relic Shield",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 365,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3302, 365),
            //
            ItemCategory = ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Spellthief's Edge

        public static Item Spellthiefs_Edge = new Item
        {
            Id = 3303,
            Name = "Spellthief's Edge",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 365,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3303, 365),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Warding Totem (Trinket)

        public static Item Warding_Totem_Trinket = new Item
        {
            Id = 3340,
            Name = "Warding Totem (Trinket)",
            Range = 600f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3340, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Sweeping Lens (Trinket)

        public static Item Sweeping_Lens_Trinket = new Item
        {
            Id = 3341,
            Name = "Sweeping Lens (Trinket)",
            Range = 400f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3341, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Scrying Orb (Trinket)

        public static Item Scrying_Orb_Trinket = new Item
        {
            Id = 3342,
            Name = "Scrying Orb (Trinket)",
            Range = 600f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3342, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Soul Anchor (Trinket)

        public static Item Soul_Anchor_Trinket = new Item
        {
            Id = 3345,
            Name = "Soul Anchor (Trinket)",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3345, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Greater Totem (Trinket)

        public static Item Greater_Totem_Trinket = new Item
        {
            Id = 3350,
            Name = "Greater Totem (Trinket)",
            Range = 600f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3350, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Greater Lens (Trinket)

        public static Item Greater_Lens_Trinket = new Item
        {
            Id = 3351,
            Name = "Greater Lens (Trinket)",
            Range = 400f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3351, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Greater Orb (Trinket)

        public static Item Greater_Orb_Trinket = new Item
        {
            Id = 3352,
            Name = "Greater Orb (Trinket)",
            Range = 2500f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3352, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Greater Stealth Totem (Trinket)

        public static Item Greater_Stealth_Totem_Trinket = new Item
        {
            Id = 3361,
            Name = "Greater Stealth Totem (Trinket)",
            Range = 600f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3340 },
            SellPrice = GetReducedPrice(3361, 475),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Greater Vision Totem (Trinket)

        public static Item Greater_Vision_Totem_Trinket = new Item
        {
            Id = 3362,
            Name = "Greater Vision Totem (Trinket)",
            Range = 600f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3340 },
            SellPrice = GetReducedPrice(3362, 475),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Farsight Orb (Trinket)

        public static Item Farsight_Orb_Trinket = new Item
        {
            Id = 3363,
            Name = "Farsight Orb (Trinket)",
            Range = 4000f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3342 },
            SellPrice = GetReducedPrice(3363, 475),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Oracle's Lens (Trinket)

        public static Item Oracles_Lens_Trinket = new Item
        {
            Id = 3364,
            Name = "Oracle's Lens (Trinket)",
            Range = 600f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 475,
            RecipeItems = new[] { 3341 },
            SellPrice = GetReducedPrice(3364, 475),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Face of the Mountain

        public static Item Face_of_the_Mountain = new Item
        {
            Id = 3401,
            Name = "Face of the Mountain",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 485,
            RecipeItems = new[] { 3097, 3067 },
            SellPrice = GetReducedPrice(3401, 1435),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace5 = new Item
        {
            Id = 3405,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3405, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace6 = new Item
        {
            Id = 3406,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3406, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace7 = new Item
        {
            Id = 3407,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3407, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace8 = new Item
        {
            Id = 3408,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3408, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace9 = new Item
        {
            Id = 3409,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3409, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Head of Kha'Zix

        public static Item Head_of_Kha_Zix2 = new Item
        {
            Id = 3410,
            Name = "Head of Kha'Zix",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3410, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace10 = new Item
        {
            Id = 3411,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3411, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace11 = new Item
        {
            Id = 3412,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3412, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace12 = new Item
        {
            Id = 3413,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3413, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace13 = new Item
        {
            Id = 3414,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3414, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace14 = new Item
        {
            Id = 3415,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3415, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Head of Kha'Zix

        public static Item Head_of_Kha_Zix3 = new Item
        {
            Id = 3416,
            Name = "Head of Kha'Zix",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3416, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace15 = new Item
        {
            Id = 3417,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3417, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace16 = new Item
        {
            Id = 3418,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3418, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace17 = new Item
        {
            Id = 3419,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3419, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace18 = new Item
        {
            Id = 3420,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3420, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace19 = new Item
        {
            Id = 3421,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3421, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Head of Kha'Zix

        public static Item Head_of_Kha_Zix4 = new Item
        {
            Id = 3422,
            Name = "Head of Kha'Zix",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3422, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace20 = new Item
        {
            Id = 3450,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3450, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace21 = new Item
        {
            Id = 3451,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3451, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace22 = new Item
        {
            Id = 3452,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3452, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace23 = new Item
        {
            Id = 3453,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3453, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Bonetooth Necklace

        public static Item Bonetooth_Necklace24 = new Item
        {
            Id = 3454,
            Name = "Bonetooth Necklace",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3454, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Head of Kha'Zix

        public static Item Head_of_Kha_Zix5 = new Item
        {
            Id = 3455,
            Name = "Head of Kha'Zix",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3455, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Golden Transcendence

        public static Item Golden_Transcendence = new Item
        {
            Id = 3460,
            Name = "Golden Transcendence",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3460, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ardent Censer

        public static Item Ardent_Censer = new Item
        {
            Id = 3504,
            Name = "Ardent Censer",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 550,
            RecipeItems = new[] { 3114, 3113 },
            SellPrice = GetReducedPrice(3504, 1305),
            //
            ItemCategory = ItemCategory.SpellDamage & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Essence Reaver

        public static Item Essence_Reaver = new Item
        {
            Id = 3508,
            Name = "Essence Reaver",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 850,
            RecipeItems = new[] { 1053, 1038 },
            SellPrice = GetReducedPrice(3508, 2840),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.ManaRegen & ItemCategory.LifeSteal,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region The Black Spear

        public static Item The_Black_Spear = new Item
        {
            Id = 3599,
            Name = "The Black Spear",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = false,
            //
            Price = 0,
            RecipeItems = null,
            SellPrice = GetReducedPrice(3599, 0),
            //
            ItemCategory = ItemCategory.None,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Stalker's Blade

        public static Item Stalkers_Blade = new Item
        {
            Id = 3706,
            Name = "Stalker's Blade",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 350,
            RecipeItems = new[] { 1039 },
            SellPrice = GetReducedPrice(3706, 750),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.HealthRegen & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Stalker's Blade - Warrior

        public static Item Stalkers_Blade__Warrior = new Item
        {
            Id = 3707,
            Name = "Stalker's Blade - Warrior",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 163,
            RecipeItems = new[] { 3706, 3134 },
            SellPrice = GetReducedPrice(3707, 1130),
            //
            ItemCategory = ItemCategory.Damage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Stalker's Blade - Magus

        public static Item Stalkers_Blade__Magus = new Item
        {
            Id = 3708,
            Name = "Stalker's Blade - Magus",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 680,
            RecipeItems = new[] { 3706, 3108 },
            SellPrice = GetReducedPrice(3708, 1415),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Stalker's Blade - Juggernaut

        public static Item Stalkers_Blade__Juggernaut = new Item
        {
            Id = 3709,
            Name = "Stalker's Blade - Juggernaut",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 250,
            RecipeItems = new[] { 3706, 3067, 1028 },
            SellPrice = GetReducedPrice(3709, 1450),
            //
            ItemCategory = ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Stalker's Blade - Devourer

        public static Item Stalkers_Blade__Devourer = new Item
        {
            Id = 3710,
            Name = "Stalker's Blade - Devourer",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 600,
            RecipeItems = new[] { 3706, 1042 },
            SellPrice = GetReducedPrice(3710, 1400),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Poacher's Knife

        public static Item Poachers_Knife = new Item
        {
            Id = 3711,
            Name = "Poacher's Knife",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 350,
            RecipeItems = new[] { 1039 },
            SellPrice = GetReducedPrice(3711, 750),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.HealthRegen & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Cultivator's Staff

        public static Item Cultivators_Staff = new Item
        {
            Id = 3712,
            Name = "Cultivator's Staff",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 350,
            RecipeItems = new[] { 1039 },
            SellPrice = GetReducedPrice(3712, 750),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.HealthRegen & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ranger's Trailblazer

        public static Item Rangers_Trailblazer = new Item
        {
            Id = 3713,
            Name = "Ranger's Trailblazer",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 350,
            RecipeItems = new[] { 1039 },
            SellPrice = GetReducedPrice(3713, 750),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.HealthRegen & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Skirmisher's Sabre - Warrior

        public static Item Skirmishers_Sabre__Warrior = new Item
        {
            Id = 3714,
            Name = "Skirmisher's Sabre - Warrior",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 163,
            RecipeItems = new[] { 3134, 3715 },
            SellPrice = GetReducedPrice(3714, 1130),
            //
            ItemCategory = ItemCategory.Damage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Skirmisher's Sabre

        public static Item Skirmishers_Sabre = new Item
        {
            Id = 3715,
            Name = "Skirmisher's Sabre",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 350,
            RecipeItems = new[] { 1039 },
            SellPrice = GetReducedPrice(3715, 750),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.HealthRegen & ItemCategory.ManaRegen,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Skirmisher's Sabre - Magus

        public static Item Skirmishers_Sabre__Magus = new Item
        {
            Id = 3716,
            Name = "Skirmisher's Sabre - Magus",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 680,
            RecipeItems = new[] { 3715, 3108 },
            SellPrice = GetReducedPrice(3716, 1415),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Skirmisher's Sabre - Juggernaut

        public static Item Skirmishers_Sabre__Juggernaut = new Item
        {
            Id = 3717,
            Name = "Skirmisher's Sabre - Juggernaut",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 250,
            RecipeItems = new[] { 3067, 3715, 1028 },
            SellPrice = GetReducedPrice(3717, 1450),
            //
            ItemCategory = ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Skirmisher's Sabre - Devourer

        public static Item Skirmishers_Sabre__Devourer = new Item
        {
            Id = 3718,
            Name = "Skirmisher's Sabre - Devourer",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 600,
            RecipeItems = new[] { 3715, 1042 },
            SellPrice = GetReducedPrice(3718, 1400),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Poacher's Knife - Warrior

        public static Item Poachers_Knife__Warrior = new Item
        {
            Id = 3719,
            Name = "Poacher's Knife - Warrior",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 163,
            RecipeItems = new[] { 3711, 3134 },
            SellPrice = GetReducedPrice(3719, 1130),
            //
            ItemCategory = ItemCategory.Damage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Poacher's Knife - Magus

        public static Item Poachers_Knife__Magus = new Item
        {
            Id = 3720,
            Name = "Poacher's Knife - Magus",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 680,
            RecipeItems = new[] { 3711, 3108 },
            SellPrice = GetReducedPrice(3720, 1415),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Poacher's Knife - Juggernaut

        public static Item Poachers_Knife__Juggernaut = new Item
        {
            Id = 3721,
            Name = "Poacher's Knife - Juggernaut",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 250,
            RecipeItems = new[] { 3067, 3711, 1028 },
            SellPrice = GetReducedPrice(3721, 1450),
            //
            ItemCategory = ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Poacher's Knife - Devourer

        public static Item Poachers_Knife__Devourer = new Item
        {
            Id = 3722,
            Name = "Poacher's Knife - Devourer",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 600,
            RecipeItems = new[] { 3711, 1042 },
            SellPrice = GetReducedPrice(3722, 1400),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ranger's Trailblazer - Warrior

        public static Item Rangers_Trailblazer__Warrior = new Item
        {
            Id = 3723,
            Name = "Ranger's Trailblazer - Warrior",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 163,
            RecipeItems = new[] { 3713, 3134 },
            SellPrice = GetReducedPrice(3723, 1130),
            //
            ItemCategory = ItemCategory.Damage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ranger's Trailblazer - Magus

        public static Item Rangers_Trailblazer__Magus = new Item
        {
            Id = 3724,
            Name = "Ranger's Trailblazer - Magus",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 680,
            RecipeItems = new[] { 3713, 3108 },
            SellPrice = GetReducedPrice(3724, 1415),
            //
            ItemCategory = ItemCategory.SpellDamage,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ranger's Trailblazer - Juggernaut

        public static Item Rangers_Trailblazer__Juggernaut = new Item
        {
            Id = 3725,
            Name = "Ranger's Trailblazer - Juggernaut",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 250,
            RecipeItems = new[] { 3067, 3713, 1028 },
            SellPrice = GetReducedPrice(3725, 1450),
            //
            ItemCategory = ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Ranger's Trailblazer - Devourer

        public static Item Rangers_Trailblazer__Devourer = new Item
        {
            Id = 3726,
            Name = "Ranger's Trailblazer - Devourer",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 600,
            RecipeItems = new[] { 3713, 1042 },
            SellPrice = GetReducedPrice(3726, 1400),
            //
            ItemCategory = ItemCategory.Damage & ItemCategory.AttackSpeed,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Righteous Glory

        public static Item Righteous_Glory = new Item
        {
            Id = 3800,
            Name = "Righteous Glory",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 700,
            RecipeItems = new[] { 3801, 3010 },
            SellPrice = GetReducedPrice(3800, 1120),
            //
            ItemCategory = ItemCategory.Mana & ItemCategory.HealthRegen & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #region Crystalline Bracer

        public static Item Crystalline_Bracer = new Item
        {
            Id = 3801,
            Name = "Crystalline Bracer",
            Range = 0f,
            MaxStacks = 1,
            //
            IsRecipe = true,
            //
            Price = 20,
            RecipeItems = new[] { 1006, 1028 },
            SellPrice = GetReducedPrice(3801, 600),
            //
            ItemCategory = ItemCategory.HealthRegen & ItemCategory.Health,
            ItemTier = ItemTier.None,
            //
            PercentArmorMod = 0f,
            PercentCritDamageMod = 0f,
            PercentEXPBonus = 0f,
            PercentHPPoolMod = 0f,
            PercentHPRegenMod = 0f,
            PercentMagicDamageMod = 0f,
            PercentMovementSpeedMod = 0f,
            PercentPhysicalDamageMod = 0f,
            PercentSpellBlockMod = 0f,
            PercentAttackSpeedMod = 0f,
            //
            FlatArmorMod = 0f,
            FlatCritChanceMod = 0f,
            FlatCritDamageMod = 0f,
            FlatHPPoolMod = 0f,
            FlatHPRegenMod = 0f,
            FlatMagicDamageMod = 0f,
            FlatMovementSpeedMod = 0f,
            FlatPhysicalDamageMod = 0f,
            FlatSpellBlockMod = 0f,
        };

        #endregion

        #endregion
    }
}