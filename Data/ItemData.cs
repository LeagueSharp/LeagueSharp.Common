#region LICENSE

/*
 Copyright 2014 - 2015 LeagueSharp
 ItemData.cs is part of LeagueSharp.Common.Data.
 
 LeagueSharp.Common is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 LeagueSharp.Common is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with LeagueSharp.Common. If not, see <http:www.gnu.org/licenses/>.
*/

#endregion

#region

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace LeagueSharp.Common.Data
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ItemData
    {
        public struct Item
        {
            /// <summary>
            ///     Item Consumed
            /// </summary>
            public bool Consumed;

            /// <summary>
            ///     Consume Item On Full
            /// </summary>
            public bool ConsumeOnFull;

            /// <summary>
            ///     Item Depth
            /// </summary>
            public int Depth;

            /// <summary>
            ///     Item Disabled Maps
            /// </summary>
            public int[] DisabledMaps;

            /// <summary>
            ///     Item From Item Ids
            /// </summary>
            public int[] From;

            /// <summary>
            ///     Gold Base Price
            /// </summary>
            public int GoldBase;

            /// <summary>
            ///     Gold Price
            /// </summary>
            public int GoldPrice;

            /// <summary>
            ///     Gold Sell Price
            /// </summary>
            public int GoldSell;

            /// <summary>
            ///     Item Group
            /// </summary>
            public string Group;

            /// <summary>
            ///     Item Hide from All
            /// </summary>
            public bool HideFromAll;

            /// <summary>
            ///     Item Id
            /// </summary>
            public int Id;

            /// <summary>
            ///     Item In Store
            /// </summary>
            public bool InStore;

            /// <summary>
            ///     Item into Items
            /// </summary>
            public int[] Into;

            /// <summary>
            ///     Item Name
            /// </summary>
            public string Name;

            /// <summary>
            ///     Item Purchasable
            /// </summary>
            public bool Purchasable;

            /// <summary>
            ///     Item Range
            /// </summary>
            public float Range;

            /// <summary>
            ///     Required Champion for Item
            /// </summary>
            public string RequiredChampion;

            /// <summary>
            ///     Special Recipe
            /// </summary>
            public int SpecialRecipe;

            /// <summary>
            ///     Item Stacks
            /// </summary>
            public int Stacks;

            /// <summary>
            ///     Item Tags
            /// </summary>
            public string[] Tags;

            public Items.Item GetItem()
            {
                return new Items.Item(Id, Range);
            }

            #region Item Stats

            public float FlatHPPoolMod;
            public float rFlatHPModPerLevel;
            public float FlatMPPoolMod;
            public float rFlatMPModPerLevel;
            public float PercentHPPoolMod;
            public float PercentMPPoolMod;
            public float FlatHPRegenMod;
            public float rFlatHPRegenModPerLevel;
            public float PercentHPRegenMod;
            public float FlatMPRegenMod;
            public float rFlatMPRegenModPerLevel;
            public float PercentMPRegenMod;
            public float FlatArmorMod;
            public float rFlatArmorModPerLevel;
            public float PercentArmorMod;
            public float rFlatArmorPenetrationMod;
            public float rFlatArmorPenetrationModPerLevel;
            public float rPercentArmorPenetrationMod;
            public float rPercentArmorPenetrationModPerLevel;
            public float FlatPhysicalDamageMod;
            public float rFlatPhysicalDamageModPerLevel;
            public float PercentPhysicalDamageMod;
            public float FlatMagicDamageMod;
            public float rFlatMagicDamageModPerLevel;
            public float PercentMagicDamageMod;
            public float FlatMovementSpeedMod;
            public float rFlatMovementSpeedModPerLevel;
            public float PercentMovementSpeedMod;
            public float rPercentMovementSpeedModPerLevel;
            public float FlatAttackSpeedMod;
            public float PercentAttackSpeedMod;
            public float rPercentAttackSpeedModPerLevel;
            public float rFlatDodgeMod;
            public float rFlatDodgeModPerLevel;
            public float PercentDodgeMod;
            public float FlatCritChanceMod;
            public float rFlatCritChanceModPerLevel;
            public float PercentCritChanceMod;
            public float FlatCritDamageMod;
            public float rFlatCritDamageModPerLevel;
            public float PercentCritDamageMod;
            public float FlatBlockMod;
            public float PercentBlockMod;
            public float FlatSpellBlockMod;
            public float rFlatSpellBlockModPerLevel;
            public float PercentSpellBlockMod;
            public float FlatEXPBonus;
            public float PercentEXPBonus;
            public float rPercentCooldownMod;
            public float rPercentCooldownModPerLevel;
            public float rFlatTimeDeadMod;
            public float rFlatTimeDeadModPerLevel;
            public float rPercentTimeDeadMod;
            public float rPercentTimeDeadModPerLevel;
            public float rFlatGoldPer10Mod;
            public float rFlatMagicPenetrationMod;
            public float rFlatMagicPenetrationModPerLevel;
            public float rPercentMagicPenetrationMod;
            public float rPercentMagicPenetrationModPerLevel;
            public float FlatEnergyRegenMod;
            public float rFlatEnergyRegenModPerLevel;
            public float FlatEnergyPoolMod;
            public float rFlatEnergyModPerLevel;
            public float PercentLifeStealMod;
            public float PercentSpellVampMod;

            #endregion
        }

        #region Items

        #region Boots of Speed

        public static Item Boots_of_Speed = new Item
        {
            Name = "Boots of Speed",
            GoldBase = 325,
            GoldPrice = 325,
            GoldSell = 227,
            Purchasable = true,
            Group = "BootsNormal",
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3006, 3047, 3020, 3158, 3111, 3117, 3009 },
            InStore = true,
            FlatMovementSpeedMod = 25f,
            Tags = new[] { "Boots" },
            Id = 1001
        };

        #endregion

        #region Faerie Charm

        public static Item Faerie_Charm = new Item
        {
            Name = "Faerie Charm",
            GoldBase = 180,
            GoldPrice = 180,
            GoldSell = 126,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3028, 3070, 3073, 3114 },
            InStore = true,
            Tags = new[] { "ManaRegen" },
            Id = 1004
        };

        #endregion

        #region Rejuvenation Bead

        public static Item Rejuvenation_Bead = new Item
        {
            Name = "Rejuvenation Bead",
            GoldBase = 180,
            GoldPrice = 180,
            GoldSell = 126,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3077, 3112, 2051, 2053, 3105, 3801 },
            InStore = true,
            Tags = new[] { "HealthRegen" },
            Id = 1006
        };

        #endregion

        #region Giant's Belt

        public static Item Giants_Belt = new Item
        {
            Name = "Giant's Belt",
            GoldBase = 600,
            GoldPrice = 1000,
            GoldSell = 700,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1028 },
            Into = new[] { 3083, 3143, 3116, 3022, 3084 },
            InStore = true,
            FlatHPPoolMod = 380f,
            Tags = new[] { "Health" },
            Id = 1011
        };

        #endregion

        #region Cloak of Agility

        public static Item Cloak_of_Agility = new Item
        {
            Name = "Cloak of Agility",
            GoldBase = 730,
            GoldPrice = 730,
            GoldSell = 511,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3046, 3031, 3104, 3185 },
            InStore = true,
            FlatCritChanceMod = 0.15f,
            Tags = new[] { "CriticalStrike" },
            Id = 1018
        };

        #endregion

        #region Blasting Wand

        public static Item Blasting_Wand = new Item
        {
            Name = "Blasting Wand",
            GoldBase = 860,
            GoldPrice = 860,
            GoldSell = 602,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3001, 3135, 3027, 3029, 3089, 3116, 3124, 3090, 3003, 3007, 3286 },
            InStore = true,
            FlatMagicDamageMod = 40f,
            Tags = new[] { "SpellDamage" },
            Id = 1026
        };

        #endregion

        #region Sapphire Crystal

        public static Item Sapphire_Crystal = new Item
        {
            Name = "Sapphire Crystal",
            GoldBase = 400,
            GoldPrice = 400,
            GoldSell = 280,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3057, 3070, 3073, 3010, 3024 },
            InStore = true,
            FlatMPPoolMod = 200f,
            Tags = new[] { "Mana" },
            Id = 1027
        };

        #endregion

        #region Ruby Crystal

        public static Item Ruby_Crystal = new Item
        {
            Name = "Ruby Crystal",
            GoldBase = 400,
            GoldPrice = 400,
            GoldSell = 280,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 1011, 2049, 2045, 2051, 3010, 3022, 3044, 3067, 3105, 3211, 3751, 3071, 3801, 3084, 3102, 3136 },
            InStore = true,
            FlatHPPoolMod = 150f,
            Tags = new[] { "Health" },
            Id = 1028
        };

        #endregion

        #region Cloth Armor

        public static Item Cloth_Armor = new Item
        {
            Name = "Cloth Armor",
            GoldBase = 300,
            GoldPrice = 300,
            GoldSell = 210,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3047, 1031, 3191, 3024, 3082, 3075, 2053 },
            InStore = true,
            FlatArmorMod = 15f,
            Tags = new[] { "Armor" },
            Id = 1029
        };

        #endregion

        #region Chain Vest

        public static Item Chain_Vest = new Item
        {
            Name = "Chain Vest",
            GoldBase = 450,
            GoldPrice = 750,
            GoldSell = 525,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1029 },
            Into = new[] { 3075, 3068, 3026 },
            InStore = true,
            FlatArmorMod = 40f,
            Tags = new[] { "Armor" },
            Id = 1031
        };

        #endregion

        #region Null-Magic Mantle

        public static Item Null_Magic_Mantle = new Item
        {
            Name = "Null-Magic Mantle",
            GoldBase = 500,
            GoldPrice = 500,
            GoldSell = 350,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3111, 3211, 1057, 3028, 3112, 3140, 3155, 3105, 3091, 3170, 3180 },
            InStore = true,
            FlatSpellBlockMod = 25f,
            Tags = new[] { "SpellBlock" },
            Id = 1033
        };

        #endregion

        #region Long Sword

        public static Item Long_Sword = new Item
        {
            Name = "Long Sword",
            GoldBase = 360,
            GoldPrice = 360,
            GoldSell = 252,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 1053, 3044, 3134, 3155, 3077, 3035, 3154, 3141, 3144, 3122, 3159 },
            InStore = true,
            FlatPhysicalDamageMod = 10f,
            Tags = new[] { "Damage", "Lane" },
            Id = 1036
        };

        #endregion

        #region Pickaxe

        public static Item Pickaxe = new Item
        {
            Name = "Pickaxe",
            GoldBase = 875,
            GoldPrice = 875,
            GoldSell = 613,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3035, 3124, 3031, 3156, 3077, 3104, 3184, 3004, 3008, 3022, 3172, 3181 },
            InStore = true,
            FlatPhysicalDamageMod = 25f,
            Tags = new[] { "Damage" },
            Id = 1037
        };

        #endregion

        #region B. F. Sword

        public static Item B_F_Sword = new Item
        {
            Name = "B. F. Sword",
            GoldBase = 1550,
            GoldPrice = 1550,
            GoldSell = 1085,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3031, 3072, 3139, 3508 },
            InStore = true,
            FlatPhysicalDamageMod = 50f,
            Tags = new[] { "Damage" },
            Id = 1038
        };

        #endregion

        #region Hunter's Machete

        public static Item Hunters_Machete = new Item
        {
            Name = "Hunter's Machete",
            GoldBase = 400,
            GoldPrice = 400,
            GoldSell = 280,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3706, 3711, 3715, 3713 },
            InStore = true,
            Tags = new[] { "Damage", "HealthRegen", "Jungle", "ManaRegen", "OnHit" },
            Id = 1039
        };

        #endregion

        #region Dagger

        public static Item Dagger = new Item
        {
            Name = "Dagger",
            GoldBase = 450,
            GoldPrice = 450,
            GoldSell = 315,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3006, 3106, 3086, 3101, 3153, 3046, 3154, 3091, 3085, 3159, 3710, 3718, 3722, 3726 },
            InStore = true,
            PercentAttackSpeedMod = 0.15f,
            Tags = new[] { "AttackSpeed" },
            Id = 1042
        };

        #endregion

        #region Recurve Bow

        public static Item Recurve_Bow = new Item
        {
            Name = "Recurve Bow",
            GoldBase = 900,
            GoldPrice = 900,
            GoldSell = 630,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3091, 3085 },
            InStore = true,
            PercentAttackSpeedMod = 0.3f,
            Tags = new[] { "AttackSpeed" },
            Id = 1043
        };

        #endregion

        #region Brawler's Gloves

        public static Item Brawlers_Gloves = new Item
        {
            Name = "Brawler's Gloves",
            GoldBase = 400,
            GoldPrice = 400,
            GoldSell = 280,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3086, 3093, 3122 },
            InStore = true,
            FlatCritChanceMod = 0.08f,
            Tags = new[] { "CriticalStrike" },
            Id = 1051
        };

        #endregion

        #region Amplifying Tome

        public static Item Amplifying_Tome = new Item
        {
            Name = "Amplifying Tome",
            GoldBase = 435,
            GoldPrice = 435,
            GoldSell = 305,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3108, 3191, 3057, 3136, 3135, 3145, 3113, 3090, 3116, 3151, 3041 },
            InStore = true,
            FlatMagicDamageMod = 20f,
            Tags = new[] { "SpellDamage" },
            Id = 1052
        };

        #endregion

        #region Vampiric Scepter

        public static Item Vampiric_Scepter = new Item
        {
            Name = "Vampiric Scepter",
            GoldBase = 440,
            GoldPrice = 800,
            GoldSell = 560,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1036 },
            Into = new[] { 3144, 3181, 3072, 3074, 3508, 3050 },
            InStore = true,
            FlatPhysicalDamageMod = 10f,
            PercentLifeStealMod = 0.08f,
            Tags = new[] { "Damage", "LifeSteal" },
            Id = 1053
        };

        #endregion

        #region Doran's Shield

        public static Item Dorans_Shield = new Item
        {
            Name = "Doran's Shield",
            GoldBase = 440,
            GoldPrice = 440,
            GoldSell = 176,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            InStore = true,
            FlatHPPoolMod = 80f,
            FlatHPRegenMod = 1.2f,
            Tags = new[] { "Health", "HealthRegen", "Lane" },
            Id = 1054
        };

        #endregion

        #region Doran's Blade

        public static Item Dorans_Blade = new Item
        {
            Name = "Doran's Blade",
            GoldBase = 440,
            GoldPrice = 440,
            GoldSell = 176,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            InStore = true,
            FlatHPPoolMod = 70f,
            FlatPhysicalDamageMod = 7f,
            PercentLifeStealMod = 0.03f,
            Tags = new[] { "Damage", "Health", "Lane", "LifeSteal" },
            Id = 1055
        };

        #endregion

        #region Doran's Ring

        public static Item Dorans_Ring = new Item
        {
            Name = "Doran's Ring",
            GoldBase = 400,
            GoldPrice = 400,
            GoldSell = 160,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            InStore = true,
            FlatHPPoolMod = 60f,
            FlatMPRegenMod = 0.6f,
            FlatMagicDamageMod = 15f,
            Tags = new[] { "Health", "Lane", "ManaRegen", "SpellDamage" },
            Id = 1056
        };

        #endregion

        #region Negatron Cloak

        public static Item Negatron_Cloak = new Item
        {
            Name = "Negatron Cloak",
            GoldBase = 350,
            GoldPrice = 850,
            GoldSell = 595,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1033 },
            Into = new[] { 3001, 3026, 3112, 3512 },
            InStore = true,
            FlatSpellBlockMod = 45f,
            Tags = new[] { "SpellBlock" },
            Id = 1057
        };

        #endregion

        #region Needlessly Large Rod

        public static Item Needlessly_Large_Rod = new Item
        {
            Name = "Needlessly Large Rod",
            GoldBase = 1600,
            GoldPrice = 1600,
            GoldSell = 1120,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3089, 3157, 3285 },
            InStore = true,
            FlatMagicDamageMod = 80f,
            Tags = new[] { "SpellDamage" },
            Id = 1058
        };

        #endregion

        #region Prospector's Blade

        public static Item Prospectors_Blade = new Item
        {
            Name = "Prospector's Blade",
            GoldBase = 950,
            GoldPrice = 950,
            GoldSell = 380,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            InStore = true,
            FlatPhysicalDamageMod = 16f,
            PercentAttackSpeedMod = 0.15f,
            Tags = new[] { "AttackSpeed", "Damage", "Health" },
            Id = 1062
        };

        #endregion

        #region Prospector's Ring

        public static Item Prospectors_Ring = new Item
        {
            Name = "Prospector's Ring",
            GoldBase = 950,
            GoldPrice = 950,
            GoldSell = 380,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            InStore = true,
            FlatMPRegenMod = 1.2f,
            FlatMagicDamageMod = 35f,
            Tags = new[] { "Health", "ManaRegen", "SpellDamage" },
            Id = 1063
        };

        #endregion

        #region Doran's Shield (Showdown)

        public static Item Dorans_Shield_Showdown = new Item
        {
            Name = "Doran's Shield (Showdown)",
            GoldBase = 440,
            GoldPrice = 440,
            GoldSell = 176,
            Purchasable = true,
            Group = "DoransShowdown",
            Stacks = 1,
            Depth = 1,
            InStore = true,
            FlatHPPoolMod = 100f,
            FlatHPRegenMod = 2f,
            Tags = new[] { "Health", "HealthRegen" },
            Id = 1074
        };

        #endregion

        #region Doran's Blade (Showdown)

        public static Item Dorans_Blade_Showdown = new Item
        {
            Name = "Doran's Blade (Showdown)",
            GoldBase = 440,
            GoldPrice = 440,
            GoldSell = 176,
            Purchasable = true,
            Group = "DoransShowdown",
            Stacks = 1,
            Depth = 1,
            InStore = true,
            FlatHPPoolMod = 70f,
            FlatPhysicalDamageMod = 7f,
            PercentLifeStealMod = 0.03f,
            Tags = new[] { "Damage", "Health", "LifeSteal", "OnHit" },
            Id = 1075
        };

        #endregion

        #region Doran's Ring (Showdown)

        public static Item Dorans_Ring_Showdown = new Item
        {
            Name = "Doran's Ring (Showdown)",
            GoldBase = 400,
            GoldPrice = 400,
            GoldSell = 160,
            Purchasable = true,
            Group = "DoransShowdown",
            Stacks = 1,
            Depth = 1,
            InStore = true,
            FlatHPPoolMod = 60f,
            FlatMPRegenMod = 0.6f,
            FlatMagicDamageMod = 15f,
            Tags = new[] { "Health", "ManaRegen", "SpellDamage" },
            Id = 1076
        };

        #endregion

        #region Health Potion

        public static Item Health_Potion = new Item
        {
            Name = "Health Potion",
            GoldBase = 35,
            GoldPrice = 35,
            GoldSell = 14,
            Purchasable = true,
            Group = "HealthPotion",
            Consumed = true,
            Stacks = 5,
            Depth = 1,
            InStore = true,
            Tags = new[] { "Consumable", "Jungle", "Lane" },
            Id = 2003
        };

        #endregion

        #region Mana Potion

        public static Item Mana_Potion = new Item
        {
            Name = "Mana Potion",
            GoldBase = 35,
            GoldPrice = 35,
            GoldSell = 14,
            Purchasable = true,
            Group = "ManaPotion",
            Consumed = true,
            Stacks = 5,
            Depth = 1,
            InStore = true,
            Tags = new[] { "Consumable", "Lane" },
            Id = 2004
        };

        #endregion

        #region Total Biscuit of Rejuvenation

        public static Item Total_Biscuit_of_Rejuvenation = new Item
        {
            Name = "Total Biscuit of Rejuvenation",
            Consumed = true,
            Stacks = 1,
            Depth = 1,
            InStore = false,
            Id = 2009
        };

        #endregion

        #region Total Biscuit of Rejuvenation

        public static Item Total_Biscuit_of_Rejuvenation2 = new Item
        {
            Name = "Total Biscuit of Rejuvenation",
            GoldBase = 35,
            GoldPrice = 35,
            GoldSell = 14,
            Group = "HealthPotion",
            Consumed = true,
            Stacks = 5,
            Depth = 1,
            InStore = false,
            Id = 2010
        };

        #endregion

        #region Crystalline Flask

        public static Item Crystalline_Flask = new Item
        {
            Name = "Crystalline Flask",
            GoldBase = 345,
            GoldPrice = 345,
            GoldSell = 138,
            Purchasable = true,
            Stacks = 1,
            Depth = 1,
            InStore = true,
            Tags = new[] { "Active", "Consumable", "HealthRegen", "Lane", "ManaRegen" },
            Id = 2041
        };

        #endregion

        #region Vision Ward

        public static Item Vision_Ward = new Item
        {
            Name = "Vision Ward",
            GoldBase = 100,
            GoldPrice = 100,
            GoldSell = 40,
            Purchasable = true,
            Group = "PinkWards",
            Consumed = true,
            Stacks = 2,
            Depth = 1,
            InStore = true,
            Tags = new[] { "Consumable", "Lane", "Stealth", "Vision" },
            Id = 2043
        };

        #endregion

        #region Stealth Ward

        public static Item Stealth_Ward = new Item
        {
            Name = "Stealth Ward",
            GoldBase = 75,
            GoldPrice = 75,
            GoldSell = 30,
            Purchasable = true,
            Group = "GreenWards",
            Consumed = true,
            Stacks = 3,
            Depth = 1,
            InStore = true,
            Tags = new[] { "Consumable", "Lane", "Vision" },
            Id = 2044
        };

        #endregion

        #region Ruby Sightstone

        public static Item Ruby_Sightstone = new Item
        {
            Name = "Ruby Sightstone",
            GoldBase = 400,
            GoldPrice = 1600,
            GoldSell = 640,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 2049, 1028 },
            InStore = true,
            FlatHPPoolMod = 400f,
            Tags = new[] { "Active", "Health", "Vision" },
            Id = 2045
        };

        #endregion

        #region Oracle's Extract

        public static Item Oracles_Extract = new Item
        {
            Name = "Oracle's Extract",
            GoldBase = 250,
            GoldPrice = 250,
            GoldSell = 100,
            Purchasable = true,
            Consumed = true,
            Stacks = 1,
            Depth = 1,
            ConsumeOnFull = true,
            InStore = true,
            Tags = new[] { "Consumable", "Stealth", "Vision" },
            Id = 2047
        };

        #endregion

        #region Sightstone

        public static Item Sightstone = new Item
        {
            Name = "Sightstone",
            GoldBase = 400,
            GoldPrice = 800,
            GoldSell = 320,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1028 },
            Into = new[] { 2045 },
            InStore = true,
            FlatHPPoolMod = 150f,
            Tags = new[] { "Active", "Health", "Vision" },
            Id = 2049
        };

        #endregion

        #region Explorer's Ward

        public static Item Explorers_Ward = new Item
        {
            Name = "Explorer's Ward",
            Stacks = 1,
            Depth = 1,
            InStore = false,
            Tags = new[] { "Consumable" },
            Id = 2050
        };

        #endregion

        #region Guardian's Horn

        public static Item Guardians_Horn = new Item
        {
            Name = "Guardian's Horn",
            GoldBase = 435,
            GoldPrice = 1015,
            GoldSell = 711,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1006, 1028 },
            InStore = true,
            FlatHPPoolMod = 200f,
            Tags = new[] { "Active", "Armor", "Health", "HealthRegen", "NonbootsMovement", "SpellBlock" },
            Id = 2051
        };

        #endregion

        #region Poro-Snax

        public static Item Poro_Snax = new Item
        {
            Name = "Poro-Snax",
            Group = "RelicBase",
            Consumed = true,
            Stacks = 1,
            Depth = 1,
            InStore = false,
            Id = 2052
        };

        #endregion

        #region Raptor Cloak

        public static Item Raptor_Cloak = new Item
        {
            Name = "Raptor Cloak",
            GoldBase = 520,
            GoldPrice = 1000,
            GoldSell = 700,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1006, 1029 },
            Into = new[] { 3056, 3512 },
            InStore = true,
            FlatArmorMod = 40f,
            Tags = new[] { "Armor", "HealthRegen", "NonbootsMovement" },
            Id = 2053
        };

        #endregion

        #region Diet Poro-Snax

        public static Item Diet_Poro_Snax = new Item
        {
            Name = "Diet Poro-Snax",
            Group = "RelicBase",
            Consumed = true,
            Stacks = 1,
            Depth = 1,
            InStore = false,
            Id = 2054
        };

        #endregion

        #region Elixir of Ruin

        public static Item Elixir_of_Ruin = new Item
        {
            Name = "Elixir of Ruin",
            GoldBase = 400,
            GoldPrice = 400,
            GoldSell = 160,
            Purchasable = true,
            Group = "Flasks",
            Consumed = true,
            Stacks = 1,
            Depth = 1,
            ConsumeOnFull = true,
            InStore = true,
            Tags = new[] { "Consumable", "Health" },
            Id = 2137
        };

        #endregion

        #region Elixir of Iron

        public static Item Elixir_of_Iron = new Item
        {
            Name = "Elixir of Iron",
            GoldBase = 400,
            GoldPrice = 400,
            GoldSell = 160,
            Purchasable = true,
            Group = "Flasks",
            Consumed = true,
            Stacks = 1,
            Depth = 1,
            ConsumeOnFull = true,
            InStore = true,
            Tags = new[] { "Consumable", "NonbootsMovement", "Tenacity" },
            Id = 2138
        };

        #endregion

        #region Elixir of Sorcery

        public static Item Elixir_of_Sorcery = new Item
        {
            Name = "Elixir of Sorcery",
            GoldBase = 400,
            GoldPrice = 400,
            GoldSell = 160,
            Purchasable = true,
            Group = "Flasks",
            Consumed = true,
            Stacks = 1,
            Depth = 1,
            ConsumeOnFull = true,
            InStore = true,
            Tags = new[] { "Consumable", "ManaRegen", "SpellDamage" },
            Id = 2139
        };

        #endregion

        #region Elixir of Wrath

        public static Item Elixir_of_Wrath = new Item
        {
            Name = "Elixir of Wrath",
            GoldBase = 400,
            GoldPrice = 400,
            GoldSell = 160,
            Purchasable = true,
            Group = "Flasks",
            Consumed = true,
            Stacks = 1,
            Depth = 1,
            ConsumeOnFull = true,
            InStore = true,
            Tags = new[] { "Consumable", "Damage", "LifeSteal", "SpellVamp" },
            Id = 2140
        };

        #endregion

        #region Abyssal Scepter

        public static Item Abyssal_Scepter = new Item
        {
            Name = "Abyssal Scepter",
            Range = 700f,
            GoldBase = 730,
            GoldPrice = 2440,
            GoldSell = 1708,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1026, 1057 },
            InStore = true,
            FlatMagicDamageMod = 70f,
            FlatSpellBlockMod = 50f,
            Tags = new[] { "Aura", "SpellBlock", "SpellDamage" },
            Id = 3001
        };

        #endregion

        #region Archangel's Staff

        public static Item Archangels_Staff = new Item
        {
            Name = "Archangel's Staff",
            GoldBase = 1120,
            GoldPrice = 2700,
            GoldSell = 1890,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3070, 1026 },
            Into = new[] { 3040 },
            InStore = true,
            FlatMPPoolMod = 250f,
            FlatMagicDamageMod = 60f,
            Tags = new[] { "Mana", "ManaRegen", "SpellDamage" },
            Id = 3003
        };

        #endregion

        #region Manamune

        public static Item Manamune = new Item
        {
            Name = "Manamune",
            GoldBase = 605,
            GoldPrice = 2200,
            GoldSell = 1540,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3070, 1037 },
            Into = new[] { 3042 },
            InStore = true,
            FlatMPPoolMod = 250f,
            FlatPhysicalDamageMod = 25f,
            Tags = new[] { "Damage", "Mana", "ManaRegen", "OnHit" },
            Id = 3004
        };

        #endregion

        #region Berserker's Greaves

        public static Item Berserkers_Greaves = new Item
        {
            Name = "Berserker's Greaves",
            GoldBase = 225,
            GoldPrice = 1000,
            GoldSell = 700,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1001, 1042 },
            Into = new[] { 3254, 3253, 3252, 3251, 3250 },
            InStore = true,
            FlatMovementSpeedMod = 45f,
            PercentAttackSpeedMod = 0.25f,
            Tags = new[] { "AttackSpeed", "Boots" },
            Id = 3006
        };

        #endregion

        #region Archangel's Staff (Crystal Scar)

        public static Item Archangels_Staff_Crystal_Scar = new Item
        {
            Name = "Archangel's Staff (Crystal Scar)",
            GoldBase = 1120,
            GoldPrice = 2700,
            GoldSell = 1890,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3073, 1026 },
            Into = new[] { 3048 },
            InStore = true,
            FlatMPPoolMod = 250f,
            FlatMagicDamageMod = 60f,
            Tags = new[] { "Mana", "ManaRegen", "SpellDamage" },
            Id = 3007
        };

        #endregion

        #region Manamune (Crystal Scar)

        public static Item Manamune_Crystal_Scar = new Item
        {
            Name = "Manamune (Crystal Scar)",
            GoldBase = 605,
            GoldPrice = 2200,
            GoldSell = 1540,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3073, 1037 },
            Into = new[] { 3043 },
            InStore = true,
            FlatMPPoolMod = 250f,
            FlatPhysicalDamageMod = 25f,
            Tags = new[] { "Damage", "Mana", "ManaRegen", "OnHit" },
            Id = 3008
        };

        #endregion

        #region Boots of Swiftness

        public static Item Boots_of_Swiftness = new Item
        {
            Name = "Boots of Swiftness",
            GoldBase = 675,
            GoldPrice = 1000,
            GoldSell = 700,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1001 },
            Into = new[] { 3284, 3283, 3282, 3281, 3280 },
            InStore = true,
            FlatMovementSpeedMod = 60f,
            Tags = new[] { "Boots" },
            Id = 3009
        };

        #endregion

        #region Catalyst the Protector

        public static Item Catalyst_the_Protector = new Item
        {
            Name = "Catalyst the Protector",
            GoldBase = 400,
            GoldPrice = 1200,
            GoldSell = 840,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1028, 1027 },
            Into = new[] { 3027, 3029, 3180, 3800 },
            InStore = true,
            FlatHPPoolMod = 200f,
            FlatMPPoolMod = 300f,
            Tags = new[] { "Health", "HealthRegen", "Mana", "ManaRegen" },
            Id = 3010
        };

        #endregion

        #region Sorcerer's Shoes

        public static Item Sorcerers_Shoes = new Item
        {
            Name = "Sorcerer's Shoes",
            GoldBase = 775,
            GoldPrice = 1100,
            GoldSell = 770,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1001 },
            Into = new[] { 3259, 3258, 3257, 3256, 3255 },
            InStore = true,
            FlatMovementSpeedMod = 45f,
            Tags = new[] { "Boots", "MagicPenetration" },
            Id = 3020
        };

        #endregion

        #region Frozen Mallet

        public static Item Frozen_Mallet = new Item
        {
            Name = "Frozen Mallet",
            GoldBase = 1025,
            GoldPrice = 3300,
            GoldSell = 2310,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1028, 1011, 1037 },
            InStore = true,
            FlatHPPoolMod = 700f,
            FlatPhysicalDamageMod = 30f,
            Tags = new[] { "Damage", "Health", "OnHit", "Slow" },
            Id = 3022
        };

        #endregion

        #region Twin Shadows

        public static Item Twin_Shadows = new Item
        {
            Name = "Twin Shadows",
            GoldBase = 730,
            GoldPrice = 2400,
            GoldSell = 1680,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3108, 3113 },
            InStore = true,
            FlatMagicDamageMod = 80f,
            PercentMovementSpeedMod = 0.06f,
            Tags = new[] { "Active", "CooldownReduction", "NonbootsMovement", "Slow", "SpellDamage" },
            Id = 3023
        };

        #endregion

        #region Glacial Shroud

        public static Item Glacial_Shroud = new Item
        {
            Name = "Glacial Shroud",
            GoldBase = 250,
            GoldPrice = 950,
            GoldSell = 665,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1027, 1029 },
            Into = new[] { 3110, 3025, 3187 },
            InStore = true,
            FlatMPPoolMod = 250f,
            FlatArmorMod = 20f,
            Tags = new[] { "Armor", "CooldownReduction", "Mana" },
            Id = 3024
        };

        #endregion

        #region Iceborn Gauntlet

        public static Item Iceborn_Gauntlet = new Item
        {
            Name = "Iceborn Gauntlet",
            GoldBase = 750,
            GoldPrice = 2900,
            GoldSell = 2030,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3057, 3024 },
            InStore = true,
            FlatMPPoolMod = 500f,
            FlatArmorMod = 60f,
            FlatMagicDamageMod = 30f,
            Tags = new[] { "Armor", "CooldownReduction", "Mana", "Slow", "SpellDamage" },
            Id = 3025
        };

        #endregion

        #region Guardian Angel

        public static Item Guardian_Angel = new Item
        {
            Name = "Guardian Angel",
            GoldBase = 1200,
            GoldPrice = 2800,
            GoldSell = 1120,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1057, 1031 },
            InStore = true,
            FlatArmorMod = 50f,
            FlatSpellBlockMod = 50f,
            Tags = new[] { "Armor", "SpellBlock" },
            Id = 3026
        };

        #endregion

        #region Rod of Ages

        public static Item Rod_of_Ages = new Item
        {
            Name = "Rod of Ages",
            GoldBase = 740,
            GoldPrice = 2800,
            GoldSell = 1960,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3010, 1026 },
            InStore = true,
            FlatHPPoolMod = 450f,
            FlatMPPoolMod = 450f,
            FlatMagicDamageMod = 60f,
            Tags = new[] { "Health", "HealthRegen", "Mana", "ManaRegen", "SpellDamage" },
            Id = 3027
        };

        #endregion

        #region Chalice of Harmony

        public static Item Chalice_of_Harmony = new Item
        {
            Name = "Chalice of Harmony",
            GoldBase = 40,
            GoldPrice = 900,
            GoldSell = 630,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1004, 1033, 1004 },
            Into = new[] { 3174, 3222 },
            InStore = true,
            FlatSpellBlockMod = 25f,
            Tags = new[] { "ManaRegen", "SpellBlock" },
            Id = 3028
        };

        #endregion

        #region Rod of Ages (Crystal Scar)

        public static Item Rod_of_Ages_Crystal_Scar = new Item
        {
            Name = "Rod of Ages (Crystal Scar)",
            GoldBase = 740,
            GoldPrice = 2800,
            GoldSell = 1960,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3010, 1026 },
            InStore = true,
            FlatHPPoolMod = 450f,
            FlatMPPoolMod = 450f,
            FlatMagicDamageMod = 60f,
            Tags = new[] { "Health", "HealthRegen", "Mana", "ManaRegen", "SpellDamage" },
            Id = 3029
        };

        #endregion

        #region Infinity Edge

        public static Item Infinity_Edge = new Item
        {
            Name = "Infinity Edge",
            GoldBase = 645,
            GoldPrice = 3800,
            GoldSell = 2660,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1038, 1037, 1018 },
            InStore = true,
            FlatPhysicalDamageMod = 80f,
            FlatCritChanceMod = 0.2f,
            Tags = new[] { "CriticalStrike", "Damage" },
            Id = 3031
        };

        #endregion

        #region Last Whisper

        public static Item Last_Whisper = new Item
        {
            Name = "Last Whisper",
            GoldBase = 1065,
            GoldPrice = 2300,
            GoldSell = 1610,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1037, 1036 },
            InStore = true,
            FlatPhysicalDamageMod = 40f,
            Tags = new[] { "ArmorPenetration", "Damage" },
            Id = 3035
        };

        #endregion

        #region Seraph's Embrace

        public static Item Seraphs_Embrace = new Item
        {
            Name = "Seraph's Embrace",
            GoldBase = 2700,
            GoldPrice = 2700,
            GoldSell = 3780,
            Stacks = 1,
            Depth = 4,
            From = new[] { 3003 },
            SpecialRecipe = 3003,
            InStore = false,
            FlatMPPoolMod = 1000f,
            FlatMagicDamageMod = 60f,
            Tags = new[] { "Active" },
            Id = 3040
        };

        #endregion

        #region Mejai's Soulstealer

        public static Item Mejais_Soulstealer = new Item
        {
            Name = "Mejai's Soulstealer",
            GoldBase = 965,
            GoldPrice = 1400,
            GoldSell = 980,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1052 },
            InStore = true,
            FlatMagicDamageMod = 20f,
            Tags = new[] { "SpellDamage" },
            Id = 3041
        };

        #endregion

        #region Muramana

        public static Item Muramana = new Item
        {
            Name = "Muramana",
            GoldBase = 2200,
            GoldPrice = 2200,
            GoldSell = 3080,
            Stacks = 1,
            Depth = 4,
            From = new[] { 3004 },
            SpecialRecipe = 3004,
            InStore = false,
            FlatMPPoolMod = 1000f,
            FlatPhysicalDamageMod = 25f,
            Tags = new[] { "OnHit" },
            Id = 3042
        };

        #endregion

        #region Muramana

        public static Item Muramana2 = new Item
        {
            Name = "Muramana",
            GoldBase = 2200,
            GoldPrice = 2200,
            GoldSell = 3080,
            Stacks = 1,
            Depth = 4,
            From = new[] { 3008 },
            SpecialRecipe = 3008,
            InStore = false,
            FlatMPPoolMod = 1000f,
            FlatPhysicalDamageMod = 25f,
            Tags = new[] { "OnHit" },
            Id = 3043
        };

        #endregion

        #region Phage

        public static Item Phage = new Item
        {
            Name = "Phage",
            GoldBase = 565,
            GoldPrice = 1325,
            GoldSell = 927,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1028, 1036 },
            Into = new[] { 3078, 3184 },
            InStore = true,
            FlatHPPoolMod = 200f,
            FlatPhysicalDamageMod = 20f,
            Tags = new[] { "Damage", "Health", "NonbootsMovement", "OnHit" },
            Id = 3044
        };

        #endregion

        #region Phantom Dancer

        public static Item Phantom_Dancer = new Item
        {
            Name = "Phantom Dancer",
            GoldBase = 520,
            GoldPrice = 2800,
            GoldSell = 1960,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1018, 3086, 1042 },
            InStore = true,
            PercentMovementSpeedMod = 0.05f,
            PercentAttackSpeedMod = 0.5f,
            FlatCritChanceMod = 0.35f,
            Tags = new[] { "AttackSpeed", "CriticalStrike", "NonbootsMovement" },
            Id = 3046
        };

        #endregion

        #region Ninja Tabi

        public static Item Ninja_Tabi = new Item
        {
            Name = "Ninja Tabi",
            GoldBase = 375,
            GoldPrice = 1000,
            GoldSell = 700,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1001, 1029 },
            Into = new[] { 3264, 3263, 3262, 3261, 3260 },
            InStore = true,
            FlatArmorMod = 25f,
            FlatMovementSpeedMod = 45f,
            Tags = new[] { "Armor", "Boots" },
            Id = 3047
        };

        #endregion

        #region Seraph's Embrace

        public static Item Seraphs_Embrace2 = new Item
        {
            Name = "Seraph's Embrace",
            GoldBase = 2700,
            GoldPrice = 2700,
            GoldSell = 3780,
            Stacks = 1,
            Depth = 4,
            From = new[] { 3007 },
            SpecialRecipe = 3007,
            InStore = false,
            FlatMPPoolMod = 1000f,
            FlatMagicDamageMod = 60f,
            Tags = new[] { "Active" },
            Id = 3048
        };

        #endregion

        #region Zeke's Herald

        public static Item Zekes_Herald = new Item
        {
            Name = "Zeke's Herald",
            GoldBase = 800,
            GoldPrice = 2450,
            GoldSell = 1715,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3067, 1053 },
            InStore = true,
            FlatHPPoolMod = 250f,
            Tags = new[] { "Aura", "CooldownReduction", "Damage", "Health", "LifeSteal" },
            Id = 3050
        };

        #endregion

        #region Ohmwrecker

        public static Item Ohmwrecker = new Item
        {
            Name = "Ohmwrecker",
            GoldBase = 750,
            GoldPrice = 2600,
            GoldSell = 1820,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 2053, 3067 },
            InStore = true,
            FlatHPPoolMod = 300f,
            FlatArmorMod = 50f,
            Tags = new[] { "Active", "Armor", "CooldownReduction", "Health", "HealthRegen", "NonbootsMovement" },
            Id = 3056
        };

        #endregion

        #region Sheen

        public static Item Sheen = new Item
        {
            Name = "Sheen",
            GoldBase = 365,
            GoldPrice = 1200,
            GoldSell = 840,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1027, 1052 },
            Into = new[] { 3078, 3100, 3025 },
            InStore = true,
            FlatMPPoolMod = 200f,
            FlatMagicDamageMod = 25f,
            Tags = new[] { "Mana", "SpellDamage" },
            Id = 3057
        };

        #endregion

        #region Banner of Command

        public static Item Banner_of_Command = new Item
        {
            Name = "Banner of Command",
            GoldBase = 280,
            GoldPrice = 3000,
            GoldSell = 2100,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3105, 3108 },
            InStore = true,
            FlatHPPoolMod = 200f,
            FlatMagicDamageMod = 60f,
            FlatSpellBlockMod = 20f,
            Tags = new[] { "Active", "Aura", "CooldownReduction", "Health", "HealthRegen", "SpellBlock", "SpellDamage" },
            Id = 3060
        };

        #endregion

        #region Spirit Visage

        public static Item Spirit_Visage = new Item
        {
            Name = "Spirit Visage",
            GoldBase = 700,
            GoldPrice = 2750,
            GoldSell = 1925,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3211, 3067 },
            InStore = true,
            FlatHPPoolMod = 400f,
            FlatSpellBlockMod = 55f,
            Tags = new[] { "CooldownReduction", "Health", "HealthRegen", "SpellBlock" },
            Id = 3065
        };

        #endregion

        #region Kindlegem

        public static Item Kindlegem = new Item
        {
            Name = "Kindlegem",
            GoldBase = 450,
            GoldPrice = 850,
            GoldSell = 595,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1028 },
            Into = new[] { 3187, 3190, 3401, 3065, 3050, 3056 },
            InStore = true,
            FlatHPPoolMod = 200f,
            Tags = new[] { "CooldownReduction", "Health" },
            Id = 3067
        };

        #endregion

        #region Sunfire Cape

        public static Item Sunfire_Cape = new Item
        {
            Name = "Sunfire Cape",
            GoldBase = 850,
            GoldPrice = 2600,
            GoldSell = 1820,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1031, 3751 },
            InStore = true,
            FlatHPPoolMod = 450f,
            FlatArmorMod = 45f,
            Tags = new[] { "Armor", "Health" },
            Id = 3068
        };

        #endregion

        #region Talisman of Ascension

        public static Item Talisman_of_Ascension = new Item
        {
            Name = "Talisman of Ascension",
            GoldBase = 635,
            GoldPrice = 2100,
            GoldSell = 840,
            Purchasable = true,
            Group = "GoldBase",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3096, 3114 },
            InStore = true,
            Tags = new[] { "Active", "CooldownReduction", "GoldPer", "HealthRegen", "ManaRegen", "NonbootsMovement" },
            Id = 3069
        };

        #endregion

        #region Tear of the Goddess

        public static Item Tear_of_the_Goddess = new Item
        {
            Name = "Tear of the Goddess",
            GoldBase = 140,
            GoldPrice = 720,
            GoldSell = 504,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1027, 1004 },
            Into = new[] { 3003, 3004 },
            InStore = true,
            FlatMPPoolMod = 250f,
            Tags = new[] { "Mana", "ManaRegen" },
            Id = 3070
        };

        #endregion

        #region The Black Cleaver

        public static Item The_Black_Cleaver = new Item
        {
            Name = "The Black Cleaver",
            GoldBase = 1263,
            GoldPrice = 3000,
            GoldSell = 2100,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3134, 1028 },
            InStore = true,
            FlatHPPoolMod = 200f,
            FlatPhysicalDamageMod = 50f,
            Tags = new[] { "ArmorPenetration", "CooldownReduction", "Damage", "Health", "OnHit" },
            Id = 3071
        };

        #endregion

        #region The Bloodthirster

        public static Item The_Bloodthirster = new Item
        {
            Name = "The Bloodthirster",
            GoldBase = 1150,
            GoldPrice = 3500,
            GoldSell = 2450,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1053, 1038 },
            InStore = true,
            FlatPhysicalDamageMod = 80f,
            Tags = new[] { "Damage", "LifeSteal" },
            Id = 3072
        };

        #endregion

        #region Tear of the Goddess (Crystal Scar)

        public static Item Tear_of_the_Goddess_Crystal_Scar = new Item
        {
            Name = "Tear of the Goddess (Crystal Scar)",
            GoldBase = 140,
            GoldPrice = 720,
            GoldSell = 504,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1027, 1004 },
            Into = new[] { 3007, 3008 },
            InStore = true,
            FlatMPPoolMod = 250f,
            Tags = new[] { "Mana", "ManaRegen" },
            Id = 3073
        };

        #endregion

        #region Ravenous Hydra (Melee Only)

        public static Item Ravenous_Hydra_Melee_Only = new Item
        {
            Name = "Ravenous Hydra (Melee Only)",
            Range = 400f,
            GoldBase = 600,
            GoldPrice = 3300,
            GoldSell = 2310,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3077, 1053 },
            InStore = true,
            FlatPhysicalDamageMod = 75f,
            PercentLifeStealMod = 0.12f,
            Tags = new[] { "Active", "Damage", "HealthRegen", "LifeSteal", "OnHit" },
            Id = 3074
        };

        #endregion

        #region Thornmail

        public static Item Thornmail = new Item
        {
            Name = "Thornmail",
            GoldBase = 1050,
            GoldPrice = 2100,
            GoldSell = 1470,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1029, 1031 },
            InStore = true,
            FlatArmorMod = 100f,
            Tags = new[] { "Armor" },
            Id = 3075
        };

        #endregion

        #region Tiamat (Melee Only)

        public static Item Tiamat_Melee_Only = new Item
        {
            Name = "Tiamat (Melee Only)",
            Range = 400f,
            GoldBase = 305,
            GoldPrice = 1900,
            GoldSell = 1330,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1037, 1036, 1006, 1006 },
            Into = new[] { 3074 },
            InStore = true,
            FlatPhysicalDamageMod = 40f,
            Tags = new[] { "Active", "Damage", "HealthRegen", "OnHit" },
            Id = 3077
        };

        #endregion

        #region Trinity Force

        public static Item Trinity_Force = new Item
        {
            Name = "Trinity Force",
            GoldBase = 78,
            GoldPrice = 3703,
            GoldSell = 2592,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3086, 3057, 3044 },
            InStore = true,
            FlatHPPoolMod = 250f,
            FlatMPPoolMod = 200f,
            FlatPhysicalDamageMod = 30f,
            FlatMagicDamageMod = 30f,
            PercentMovementSpeedMod = 0.08f,
            PercentAttackSpeedMod = 0.3f,
            FlatCritChanceMod = 0.1f,
            Tags = new[] { "AttackSpeed", "CriticalStrike", "Damage", "Health", "Mana", "NonbootsMovement", "OnHit", "SpellDamage" },
            Id = 3078
        };

        #endregion

        #region Warden's Mail

        public static Item Wardens_Mail = new Item
        {
            Name = "Warden's Mail",
            GoldBase = 450,
            GoldPrice = 1050,
            GoldSell = 735,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1029, 1029 },
            Into = new[] { 3110, 3143 },
            InStore = true,
            FlatArmorMod = 45f,
            Tags = new[] { "Armor", "Slow" },
            Id = 3082
        };

        #endregion

        #region Warmog's Armor

        public static Item Warmogs_Armor = new Item
        {
            Name = "Warmog's Armor",
            GoldBase = 300,
            GoldPrice = 2500,
            GoldSell = 1750,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3801, 1011, 3801 },
            InStore = true,
            FlatHPPoolMod = 800f,
            Tags = new[] { "Health", "HealthRegen" },
            Id = 3083
        };

        #endregion

        #region Overlord's Bloodmail

        public static Item Overlords_Bloodmail = new Item
        {
            Name = "Overlord's Bloodmail",
            GoldBase = 1055,
            GoldPrice = 2455,
            GoldSell = 1719,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1011, 1028 },
            InStore = true,
            FlatHPPoolMod = 850f,
            Tags = new[] { "Health", "HealthRegen" },
            Id = 3084
        };

        #endregion

        #region Runaan's Hurricane (Ranged Only)

        public static Item Runaans_Hurricane_Ranged_Only = new Item
        {
            Name = "Runaan's Hurricane (Ranged Only)",
            GoldBase = 600,
            GoldPrice = 2400,
            GoldSell = 1680,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1042, 1043, 1042 },
            InStore = true,
            PercentAttackSpeedMod = 0.7f,
            Tags = new[] { "AttackSpeed", "OnHit" },
            Id = 3085
        };

        #endregion

        #region Zeal

        public static Item Zeal = new Item
        {
            Name = "Zeal",
            GoldBase = 250,
            GoldPrice = 1100,
            GoldSell = 770,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1051, 1042 },
            Into = new[] { 3046, 3078, 3087 },
            InStore = true,
            PercentMovementSpeedMod = 0.05f,
            PercentAttackSpeedMod = 0.2f,
            FlatCritChanceMod = 0.1f,
            Tags = new[] { "AttackSpeed", "CriticalStrike", "NonbootsMovement" },
            Id = 3086
        };

        #endregion

        #region Statikk Shiv

        public static Item Statikk_Shiv = new Item
        {
            Name = "Statikk Shiv",
            GoldBase = 600,
            GoldPrice = 2500,
            GoldSell = 1750,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3086, 3093 },
            InStore = true,
            PercentMovementSpeedMod = 0.06f,
            PercentAttackSpeedMod = 0.4f,
            FlatCritChanceMod = 0.2f,
            Tags = new[] { "AttackSpeed", "CriticalStrike", "NonbootsMovement", "OnHit" },
            Id = 3087
        };

        #endregion

        #region Rabadon's Deathcap

        public static Item Rabadons_Deathcap = new Item
        {
            Name = "Rabadon's Deathcap",
            GoldBase = 840,
            GoldPrice = 3300,
            GoldSell = 2310,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1026, 1058 },
            InStore = true,
            FlatMagicDamageMod = 120f,
            Tags = new[] { "SpellDamage" },
            Id = 3089
        };

        #endregion

        #region Wooglet's Witchcap

        public static Item Wooglets_Witchcap = new Item
        {
            Name = "Wooglet's Witchcap",
            GoldBase = 1045,
            GoldPrice = 3540,
            GoldSell = 2478,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3191, 1026, 1052 },
            InStore = true,
            FlatArmorMod = 45f,
            FlatMagicDamageMod = 100f,
            Tags = new[] { "Active", "Armor", "SpellDamage" },
            Id = 3090
        };

        #endregion

        #region Wit's End

        public static Item Wits_End = new Item
        {
            Name = "Wit's End",
            GoldBase = 750,
            GoldPrice = 2600,
            GoldSell = 1820,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1043, 1033, 1042 },
            InStore = true,
            PercentAttackSpeedMod = 0.5f,
            FlatSpellBlockMod = 30f,
            Tags = new[] { "AttackSpeed", "OnHit", "SpellBlock" },
            Id = 3091
        };

        #endregion

        #region Frost Queen's Claim

        public static Item Frost_Queens_Claim = new Item
        {
            Name = "Frost Queen's Claim",
            Range = 850f,
            GoldBase = 515,
            GoldPrice = 2200,
            GoldSell = 880,
            Purchasable = true,
            Group = "GoldBase",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3098, 3108 },
            InStore = true,
            FlatMagicDamageMod = 50f,
            Tags = new[] { "Active", "CooldownReduction", "GoldPer", "ManaRegen", "Slow", "SpellDamage" },
            Id = 3092
        };

        #endregion

        #region Avarice Blade

        public static Item Avarice_Blade = new Item
        {
            Name = "Avarice Blade",
            GoldBase = 400,
            GoldPrice = 800,
            GoldSell = 320,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1051 },
            Into = new[] { 3087, 3142 },
            InStore = true,
            FlatCritChanceMod = 0.1f,
            Tags = new[] { "CriticalStrike", "GoldPer" },
            Id = 3093
        };

        #endregion

        #region Nomad's Medallion

        public static Item Nomads_Medallion = new Item
        {
            Name = "Nomad's Medallion",
            GoldBase = 500,
            GoldPrice = 865,
            GoldSell = 346,
            Purchasable = true,
            Group = "GoldBase",
            Stacks = 1,
            Depth = 2,
            From = new[] { 3301 },
            Into = new[] { 3069 },
            InStore = true,
            Tags = new[] { "Active", "GoldPer", "HealthRegen", "ManaRegen", "NonbootsMovement" },
            Id = 3096
        };

        #endregion

        #region Targon's Brace

        public static Item Targons_Brace = new Item
        {
            Name = "Targon's Brace",
            GoldBase = 500,
            GoldPrice = 865,
            GoldSell = 346,
            Purchasable = true,
            Group = "GoldBase",
            Stacks = 1,
            Depth = 2,
            From = new[] { 3302 },
            Into = new[] { 3401 },
            InStore = true,
            FlatHPPoolMod = 175f,
            Tags = new[] { "Aura", "GoldPer", "Health", "HealthRegen" },
            Id = 3097
        };

        #endregion

        #region Frostfang

        public static Item Frostfang = new Item
        {
            Name = "Frostfang",
            GoldBase = 500,
            GoldPrice = 865,
            GoldSell = 346,
            Purchasable = true,
            Group = "GoldBase",
            Stacks = 1,
            Depth = 2,
            From = new[] { 3303 },
            Into = new[] { 3092 },
            InStore = true,
            FlatMagicDamageMod = 10f,
            Tags = new[] { "Active", "GoldPer", "ManaRegen", "SpellDamage" },
            Id = 3098
        };

        #endregion

        #region Lich Bane

        public static Item Lich_Bane = new Item
        {
            Name = "Lich Bane",
            GoldBase = 950,
            GoldPrice = 3000,
            GoldSell = 2100,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3057, 3113 },
            InStore = true,
            FlatMPPoolMod = 250f,
            FlatMagicDamageMod = 80f,
            PercentMovementSpeedMod = 0.05f,
            Tags = new[] { "Mana", "NonbootsMovement", "SpellDamage" },
            Id = 3100
        };

        #endregion

        #region Stinger

        public static Item Stinger = new Item
        {
            Name = "Stinger",
            GoldBase = 350,
            GoldPrice = 1250,
            GoldSell = 875,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1042, 1042 },
            Into = new[] { 3115, 3172, 3137 },
            InStore = true,
            PercentAttackSpeedMod = 0.4f,
            Tags = new[] { "AttackSpeed", "CooldownReduction" },
            Id = 3101
        };

        #endregion

        #region Banshee's Veil

        public static Item Banshees_Veil = new Item
        {
            Name = "Banshee's Veil",
            GoldBase = 1150,
            GoldPrice = 2750,
            GoldSell = 1925,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3211, 1028 },
            InStore = true,
            FlatHPPoolMod = 450f,
            FlatSpellBlockMod = 55f,
            Tags = new[] { "Health", "HealthRegen", "SpellBlock" },
            Id = 3102
        };

        #endregion

        #region Lord Van Damm's Pillager

        public static Item Lord_Van_Damms_Pillager = new Item
        {
            Name = "Lord Van Damm's Pillager",
            GoldBase = 995,
            GoldPrice = 3800,
            GoldSell = 2660,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3122, 1037, 1018 },
            InStore = true,
            FlatPhysicalDamageMod = 80f,
            FlatCritChanceMod = 0.25f,
            Tags = new[] { "CriticalStrike", "Damage" },
            Id = 3104
        };

        #endregion

        #region Aegis of the Legion

        public static Item Aegis_of_the_Legion = new Item
        {
            Name = "Aegis of the Legion",
            GoldBase = 820,
            GoldPrice = 1900,
            GoldSell = 1330,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1028, 1033, 1006 },
            Into = new[] { 3190, 3060 },
            InStore = true,
            FlatHPPoolMod = 200f,
            FlatSpellBlockMod = 20f,
            Tags = new[] { "Aura", "Health", "HealthRegen", "SpellBlock" },
            Id = 3105
        };

        #endregion

        #region Madred's Razors

        public static Item Madreds_Razors = new Item
        {
            Name = "Madred's Razors",
            GoldBase = 300,
            GoldPrice = 750,
            GoldSell = 525,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1042 },
            Into = new[] { 3154, 3159 },
            InStore = true,
            PercentAttackSpeedMod = 0.15f,
            Tags = new[] { "AttackSpeed", "OnHit" },
            Id = 3106
        };

        #endregion

        #region Fiendish Codex

        public static Item Fiendish_Codex = new Item
        {
            Name = "Fiendish Codex",
            GoldBase = 385,
            GoldPrice = 820,
            GoldSell = 574,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1052 },
            Into = new[] { 3174, 3092, 3115, 3290, 3165, 3152, 3060, 3023, 3708, 3716, 3720, 3724 },
            InStore = true,
            FlatMagicDamageMod = 30f,
            Tags = new[] { "CooldownReduction", "SpellDamage" },
            Id = 3108
        };

        #endregion

        #region Frozen Heart

        public static Item Frozen_Heart = new Item
        {
            Name = "Frozen Heart",
            GoldBase = 450,
            GoldPrice = 2450,
            GoldSell = 1715,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3082, 3024 },
            InStore = true,
            FlatMPPoolMod = 400f,
            FlatArmorMod = 100f,
            Tags = new[] { "Armor", "Aura", "CooldownReduction", "Mana" },
            Id = 3110
        };

        #endregion

        #region Mercury's Treads

        public static Item Mercurys_Treads = new Item
        {
            Name = "Mercury's Treads",
            GoldBase = 375,
            GoldPrice = 1200,
            GoldSell = 840,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1001, 1033 },
            Into = new[] { 3269, 3268, 3267, 3266, 3265 },
            InStore = true,
            FlatMovementSpeedMod = 45f,
            FlatSpellBlockMod = 25f,
            Tags = new[] { "Boots", "SpellBlock", "Tenacity" },
            Id = 3111
        };

        #endregion

        #region Orb of Winter

        public static Item Orb_of_Winter = new Item
        {
            Name = "Orb of Winter",
            GoldBase = 990,
            GoldPrice = 2200,
            GoldSell = 1540,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1006, 1006, 1057 },
            InStore = true,
            FlatSpellBlockMod = 70f,
            Tags = new[] { "HealthRegen", "SpellBlock" },
            Id = 3112
        };

        #endregion

        #region Aether Wisp

        public static Item Aether_Wisp = new Item
        {
            Name = "Aether Wisp",
            GoldBase = 415,
            GoldPrice = 850,
            GoldSell = 595,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1052 },
            Into = new[] { 3023, 3290, 3100, 3285, 3286, 3504 },
            InStore = true,
            FlatMagicDamageMod = 30f,
            Tags = new[] { "NonbootsMovement", "SpellDamage" },
            Id = 3113
        };

        #endregion

        #region Forbidden Idol

        public static Item Forbidden_Idol = new Item
        {
            Name = "Forbidden Idol",
            GoldBase = 240,
            GoldPrice = 600,
            GoldSell = 420,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1004, 1004 },
            Into = new[] { 3069, 3165, 3222, 3504 },
            InStore = true,
            Tags = new[] { "CooldownReduction", "ManaRegen" },
            Id = 3114
        };

        #endregion

        #region Nashor's Tooth

        public static Item Nashors_Tooth = new Item
        {
            Name = "Nashor's Tooth",
            GoldBase = 850,
            GoldPrice = 2920,
            GoldSell = 2044,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3101, 3108 },
            InStore = true,
            FlatMagicDamageMod = 60f,
            PercentAttackSpeedMod = 0.5f,
            Tags = new[] { "AttackSpeed", "CooldownReduction", "OnHit", "SpellDamage" },
            Id = 3115
        };

        #endregion

        #region Rylai's Crystal Scepter

        public static Item Rylais_Crystal_Scepter = new Item
        {
            Name = "Rylai's Crystal Scepter",
            GoldBase = 605,
            GoldPrice = 2900,
            GoldSell = 2030,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1026, 1052, 1011 },
            InStore = true,
            FlatHPPoolMod = 400f,
            FlatMagicDamageMod = 100f,
            Tags = new[] { "Health", "Slow", "SpellDamage" },
            Id = 3116
        };

        #endregion

        #region Boots of Mobility

        public static Item Boots_of_Mobility = new Item
        {
            Name = "Boots of Mobility",
            GoldBase = 475,
            GoldPrice = 800,
            GoldSell = 560,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1001 },
            Into = new[] { 3274, 3273, 3272, 3271, 3270 },
            InStore = true,
            FlatMovementSpeedMod = 105f,
            Tags = new[] { "Boots" },
            Id = 3117
        };

        #endregion

        #region Wicked Hatchet

        public static Item Wicked_Hatchet = new Item
        {
            Name = "Wicked Hatchet",
            GoldBase = 440,
            GoldPrice = 1200,
            GoldSell = 840,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1051, 1036 },
            Into = new[] { 3104, 3185 },
            InStore = true,
            FlatPhysicalDamageMod = 20f,
            FlatCritChanceMod = 0.1f,
            Tags = new[] { "CriticalStrike", "Damage", "OnHit" },
            Id = 3122
        };

        #endregion

        #region Guinsoo's Rageblade

        public static Item Guinsoos_Rageblade = new Item
        {
            Name = "Guinsoo's Rageblade",
            GoldBase = 865,
            GoldPrice = 2600,
            GoldSell = 1820,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1026, 1037 },
            InStore = true,
            FlatPhysicalDamageMod = 30f,
            FlatMagicDamageMod = 40f,
            Tags = new[] { "AttackSpeed", "Damage", "LifeSteal", "OnHit", "SpellDamage", "SpellVamp" },
            Id = 3124
        };

        #endregion

        #region The Brutalizer

        public static Item The_Brutalizer = new Item
        {
            Name = "The Brutalizer",
            GoldBase = 617,
            GoldPrice = 1337,
            GoldSell = 936,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1036, 1036 },
            Into = new[] { 3142, 3071, 3707, 3714, 3719, 3723 },
            InStore = true,
            FlatPhysicalDamageMod = 25f,
            Tags = new[] { "ArmorPenetration", "CooldownReduction", "Damage" },
            Id = 3134
        };

        #endregion

        #region Void Staff

        public static Item Void_Staff = new Item
        {
            Name = "Void Staff",
            GoldBase = 1000,
            GoldPrice = 2295,
            GoldSell = 1607,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1026, 1052 },
            InStore = true,
            FlatMagicDamageMod = 70f,
            Tags = new[] { "MagicPenetration", "SpellDamage" },
            Id = 3135
        };

        #endregion

        #region Haunting Guise

        public static Item Haunting_Guise = new Item
        {
            Name = "Haunting Guise",
            GoldBase = 650,
            GoldPrice = 1485,
            GoldSell = 1040,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1028, 1052 },
            Into = new[] { 3151 },
            InStore = true,
            FlatHPPoolMod = 200f,
            FlatMagicDamageMod = 25f,
            Tags = new[] { "Health", "MagicPenetration", "SpellDamage" },
            Id = 3136
        };

        #endregion

        #region Dervish Blade

        public static Item Dervish_Blade = new Item
        {
            Name = "Dervish Blade",
            GoldBase = 200,
            GoldPrice = 2700,
            GoldSell = 1890,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3140, 3101 },
            InStore = true,
            PercentAttackSpeedMod = 0.5f,
            FlatSpellBlockMod = 45f,
            Tags = new[] { "Active", "AttackSpeed", "CooldownReduction", "NonbootsMovement", "SpellBlock" },
            Id = 3137
        };

        #endregion

        #region Mercurial Scimitar

        public static Item Mercurial_Scimitar = new Item
        {
            Name = "Mercurial Scimitar",
            GoldBase = 900,
            GoldPrice = 3700,
            GoldSell = 2590,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1038, 3140 },
            InStore = true,
            FlatPhysicalDamageMod = 80f,
            FlatSpellBlockMod = 35f,
            Tags = new[] { "Active", "Damage", "NonbootsMovement", "SpellBlock" },
            Id = 3139
        };

        #endregion

        #region Quicksilver Sash

        public static Item Quicksilver_Sash = new Item
        {
            Name = "Quicksilver Sash",
            GoldBase = 750,
            GoldPrice = 1250,
            GoldSell = 875,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1033 },
            Into = new[] { 3139, 3137 },
            InStore = true,
            FlatSpellBlockMod = 30f,
            Tags = new[] { "Active", "SpellBlock" },
            Id = 3140
        };

        #endregion

        #region Sword of the Occult

        public static Item Sword_of_the_Occult = new Item
        {
            Name = "Sword of the Occult",
            GoldBase = 1040,
            GoldPrice = 1400,
            GoldSell = 980,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1036 },
            InStore = true,
            FlatPhysicalDamageMod = 10f,
            Tags = new[] { "Damage" },
            Id = 3141
        };

        #endregion

        #region Youmuu's Ghostblade

        public static Item Youmuus_Ghostblade = new Item
        {
            Name = "Youmuu's Ghostblade",
            GoldBase = 563,
            GoldPrice = 2700,
            GoldSell = 1890,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3093, 3134 },
            InStore = true,
            FlatPhysicalDamageMod = 30f,
            FlatCritChanceMod = 0.15f,
            Tags = new[] { "Active", "ArmorPenetration", "AttackSpeed", "CooldownReduction", "CriticalStrike", "Damage", "NonbootsMovement" },
            Id = 3142
        };

        #endregion

        #region Randuin's Omen

        public static Item Randuins_Omen = new Item
        {
            Name = "Randuin's Omen",
            Range = 500f,
            GoldBase = 800,
            GoldPrice = 2850,
            GoldSell = 1995,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3082, 1011 },
            InStore = true,
            FlatHPPoolMod = 500f,
            FlatArmorMod = 70f,
            Tags = new[] { "Active", "Armor", "Health", "Slow" },
            Id = 3143
        };

        #endregion

        #region Bilgewater Cutlass

        public static Item Bilgewater_Cutlass = new Item
        {
            Name = "Bilgewater Cutlass",
            Range = 450f,
            GoldBase = 240,
            GoldPrice = 1400,
            GoldSell = 980,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1036, 1053 },
            Into = new[] { 3146, 3153 },
            InStore = true,
            FlatPhysicalDamageMod = 25f,
            PercentLifeStealMod = 0.08f,
            Tags = new[] { "Active", "Damage", "LifeSteal", "Slow" },
            Id = 3144
        };

        #endregion

        #region Hextech Revolver

        public static Item Hextech_Revolver = new Item
        {
            Name = "Hextech Revolver",
            GoldBase = 330,
            GoldPrice = 1200,
            GoldSell = 840,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1052, 1052 },
            Into = new[] { 3146, 3152 },
            InStore = true,
            FlatMagicDamageMod = 40f,
            Tags = new[] { "SpellDamage", "SpellVamp" },
            Id = 3145
        };

        #endregion

        #region Hextech Gunblade

        public static Item Hextech_Gunblade = new Item
        {
            Name = "Hextech Gunblade",
            Range = 700f,
            GoldBase = 800,
            GoldPrice = 3400,
            GoldSell = 2380,
            Purchasable = true,
            Stacks = 1,
            Depth = 4,
            From = new[] { 3144, 3145 },
            InStore = true,
            FlatPhysicalDamageMod = 40f,
            FlatMagicDamageMod = 80f,
            PercentLifeStealMod = 0.1f,
            Tags = new[] { "Active", "Damage", "LifeSteal", "Slow", "SpellDamage", "SpellVamp" },
            Id = 3146
        };

        #endregion

        #region Liandry's Torment

        public static Item Liandrys_Torment = new Item
        {
            Name = "Liandry's Torment",
            GoldBase = 980,
            GoldPrice = 2900,
            GoldSell = 2030,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3136, 1052 },
            InStore = true,
            FlatHPPoolMod = 300f,
            FlatMagicDamageMod = 50f,
            Tags = new[] { "Health", "MagicPenetration", "SpellDamage" },
            Id = 3151
        };

        #endregion

        #region Will of the Ancients

        public static Item Will_of_the_Ancients = new Item
        {
            Name = "Will of the Ancients",
            GoldBase = 480,
            GoldPrice = 2500,
            GoldSell = 1750,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3145, 3108 },
            InStore = true,
            FlatMagicDamageMod = 80f,
            Tags = new[] { "CooldownReduction", "SpellDamage", "SpellVamp" },
            Id = 3152
        };

        #endregion

        #region Blade of the Ruined King

        public static Item Blade_of_the_Ruined_King = new Item
        {
            Name = "Blade of the Ruined King",
            Range = 450f,
            GoldBase = 900,
            GoldPrice = 3200,
            GoldSell = 2240,
            Purchasable = true,
            Stacks = 1,
            Depth = 4,
            From = new[] { 1042, 3144, 1042 },
            InStore = true,
            FlatPhysicalDamageMod = 25f,
            PercentAttackSpeedMod = 0.4f,
            PercentLifeStealMod = 0.1f,
            Tags = new[] { "Active", "AttackSpeed", "Damage", "LifeSteal", "NonbootsMovement", "OnHit" },
            Id = 3153
        };

        #endregion

        #region Wriggle's Lantern

        public static Item Wriggles_Lantern = new Item
        {
            Name = "Wriggle's Lantern",
            Range = 600f,
            GoldBase = 215,
            GoldPrice = 1775,
            GoldSell = 710,
            Purchasable = true,
            Group = "GoldBase",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3106, 1036, 1042 },
            InStore = true,
            FlatPhysicalDamageMod = 12f,
            PercentAttackSpeedMod = 0.3f,
            Tags = new[] { "Active", "AttackSpeed", "Damage", "GoldPer", "OnHit", "Vision" },
            Id = 3154
        };

        #endregion

        #region Hexdrinker

        public static Item Hexdrinker = new Item
        {
            Name = "Hexdrinker",
            GoldBase = 590,
            GoldPrice = 1450,
            GoldSell = 1015,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1036, 1033 },
            Into = new[] { 3156 },
            InStore = true,
            FlatPhysicalDamageMod = 25f,
            FlatSpellBlockMod = 30f,
            Tags = new[] { "Damage", "SpellBlock" },
            Id = 3155
        };

        #endregion

        #region Maw of Malmortius

        public static Item Maw_of_Malmortius = new Item
        {
            Name = "Maw of Malmortius",
            GoldBase = 875,
            GoldPrice = 3200,
            GoldSell = 2240,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3155, 1037 },
            InStore = true,
            FlatPhysicalDamageMod = 60f,
            FlatSpellBlockMod = 40f,
            Tags = new[] { "Damage", "SpellBlock" },
            Id = 3156
        };

        #endregion

        #region Zhonya's Hourglass

        public static Item Zhonyas_Hourglass = new Item
        {
            Name = "Zhonya's Hourglass",
            GoldBase = 500,
            GoldPrice = 3300,
            GoldSell = 2310,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3191, 1058 },
            InStore = true,
            FlatArmorMod = 50f,
            FlatMagicDamageMod = 120f,
            Tags = new[] { "Active", "Armor", "SpellDamage" },
            Id = 3157
        };

        #endregion

        #region Ionian Boots of Lucidity

        public static Item Ionian_Boots_of_Lucidity = new Item
        {
            Name = "Ionian Boots of Lucidity",
            GoldBase = 675,
            GoldPrice = 1000,
            GoldSell = 700,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1001 },
            Into = new[] { 3279, 3278, 3277, 3276, 3275 },
            InStore = true,
            FlatMovementSpeedMod = 45f,
            Tags = new[] { "Boots", "CooldownReduction" },
            Id = 3158
        };

        #endregion

        #region Grez's Spectral Lantern

        public static Item Grezs_Spectral_Lantern = new Item
        {
            Name = "Grez's Spectral Lantern",
            GoldBase = 180,
            GoldPrice = 1740,
            GoldSell = 696,
            Purchasable = true,
            Group = "GoldBase",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3106, 1036, 1042 },
            InStore = true,
            FlatPhysicalDamageMod = 15f,
            PercentAttackSpeedMod = 0.3f,
            Tags = new[] { "Active", "AttackSpeed", "Damage", "GoldPer", "OnHit", "Stealth", "Vision" },
            Id = 3159
        };

        #endregion

        #region Morellonomicon

        public static Item Morellonomicon = new Item
        {
            Name = "Morellonomicon",
            GoldBase = 880,
            GoldPrice = 2300,
            GoldSell = 1610,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3108, 3114 },
            InStore = true,
            FlatMagicDamageMod = 80f,
            Tags = new[] { "CooldownReduction", "ManaRegen", "SpellDamage" },
            Id = 3165
        };

        #endregion

        #region Moonflair Spellblade

        public static Item Moonflair_Spellblade = new Item
        {
            Name = "Moonflair Spellblade",
            GoldBase = 920,
            GoldPrice = 2620,
            GoldSell = 1834,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3191, 1033 },
            InStore = true,
            FlatArmorMod = 50f,
            FlatMagicDamageMod = 50f,
            FlatSpellBlockMod = 50f,
            Tags = new[] { "Armor", "SpellBlock", "SpellDamage", "Tenacity" },
            Id = 3170
        };

        #endregion

        #region Zephyr

        public static Item Zephyr = new Item
        {
            Name = "Zephyr",
            GoldBase = 725,
            GoldPrice = 2850,
            GoldSell = 1995,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3101, 1037 },
            InStore = true,
            FlatPhysicalDamageMod = 25f,
            PercentMovementSpeedMod = 0.1f,
            PercentAttackSpeedMod = 0.5f,
            Tags = new[] { "AttackSpeed", "CooldownReduction", "Damage", "NonbootsMovement", "Tenacity" },
            Id = 3172
        };

        #endregion

        #region Athene's Unholy Grail

        public static Item Athenes_Unholy_Grail = new Item
        {
            Name = "Athene's Unholy Grail",
            GoldBase = 980,
            GoldPrice = 2700,
            GoldSell = 1890,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3108, 3028 },
            InStore = true,
            FlatMagicDamageMod = 60f,
            FlatSpellBlockMod = 25f,
            Tags = new[] { "CooldownReduction", "ManaRegen", "SpellBlock", "SpellDamage" },
            Id = 3174
        };

        #endregion

        #region Odyn's Veil

        public static Item Odyns_Veil = new Item
        {
            Name = "Odyn's Veil",
            GoldBase = 800,
            GoldPrice = 2500,
            GoldSell = 1750,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1033, 3010 },
            InStore = true,
            FlatHPPoolMod = 350f,
            FlatMPPoolMod = 350f,
            FlatSpellBlockMod = 50f,
            Tags = new[] { "Active", "Health", "Mana", "SpellBlock" },
            Id = 3180
        };

        #endregion

        #region Sanguine Blade

        public static Item Sanguine_Blade = new Item
        {
            Name = "Sanguine Blade",
            GoldBase = 600,
            GoldPrice = 2275,
            GoldSell = 1593,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1037, 1053 },
            InStore = true,
            FlatPhysicalDamageMod = 45f,
            PercentLifeStealMod = 0.1f,
            Tags = new[] { "Damage", "LifeSteal" },
            Id = 3181
        };

        #endregion

        #region Entropy

        public static Item Entropy = new Item
        {
            Name = "Entropy",
            GoldBase = 500,
            GoldPrice = 2700,
            GoldSell = 1890,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3044, 1037 },
            InStore = true,
            FlatHPPoolMod = 275f,
            FlatPhysicalDamageMod = 55f,
            Tags = new[] { "Active", "Damage", "Health", "NonbootsMovement", "Slow" },
            Id = 3184
        };

        #endregion

        #region The Lightbringer

        public static Item The_Lightbringer = new Item
        {
            Name = "The Lightbringer",
            GoldBase = 350,
            GoldPrice = 2280,
            GoldSell = 1596,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3122, 1018 },
            InStore = true,
            FlatPhysicalDamageMod = 30f,
            FlatCritChanceMod = 0.3f,
            Tags = new[] { "Active", "CriticalStrike", "Damage", "OnHit", "Stealth", "Vision" },
            Id = 3185
        };

        #endregion

        #region Hextech Sweeper

        public static Item Hextech_Sweeper = new Item
        {
            Name = "Hextech Sweeper",
            GoldBase = 330,
            GoldPrice = 2130,
            GoldSell = 1491,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3024, 3067 },
            InStore = true,
            FlatHPPoolMod = 225f,
            FlatMPPoolMod = 250f,
            FlatArmorMod = 25f,
            Tags = new[] { "Active", "Armor", "CooldownReduction", "Health", "Mana", "Stealth", "Vision" },
            Id = 3187
        };

        #endregion

        #region Locket of the Iron Solari

        public static Item Locket_of_the_Iron_Solari = new Item
        {
            Name = "Locket of the Iron Solari",
            Range = 600f,
            GoldBase = 50,
            GoldPrice = 2800,
            GoldSell = 1960,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3105, 3067 },
            InStore = true,
            FlatHPPoolMod = 400f,
            FlatSpellBlockMod = 20f,
            Tags = new[] { "Active", "Aura", "CooldownReduction", "Health", "HealthRegen", "SpellBlock" },
            Id = 3190
        };

        #endregion

        #region Seeker's Armguard

        public static Item Seekers_Armguard = new Item
        {
            Name = "Seeker's Armguard",
            GoldBase = 465,
            GoldPrice = 1200,
            GoldSell = 840,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1029, 1052 },
            Into = new[] { 3090, 3157, 3170 },
            InStore = true,
            FlatArmorMod = 30f,
            FlatMagicDamageMod = 25f,
            Tags = new[] { "Armor", "SpellDamage" },
            Id = 3191
        };

        #endregion

        #region The Hex Core mk-1

        public static Item The_Hex_Core_mk_1 = new Item
        {
            Name = "The Hex Core mk-1",
            GoldBase = 1000,
            GoldPrice = 1000,
            GoldSell = 700,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 3200 },
            Into = new[] { 3197 },
            InStore = true,
            RequiredChampion = "Viktor",
            FlatMPPoolMod = 150f,
            FlatMagicDamageMod = 20f,
            Tags = new[] { "Mana", "SpellDamage" },
            Id = 3196
        };

        #endregion

        #region The Hex Core mk-2

        public static Item The_Hex_Core_mk_2 = new Item
        {
            Name = "The Hex Core mk-2",
            GoldBase = 1000,
            GoldPrice = 2000,
            GoldSell = 1400,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3196 },
            Into = new[] { 3198 },
            InStore = true,
            RequiredChampion = "Viktor",
            FlatMPPoolMod = 300f,
            FlatMagicDamageMod = 40f,
            Tags = new[] { "Mana", "SpellDamage" },
            Id = 3197
        };

        #endregion

        #region Perfect Hex Core

        public static Item Perfect_Hex_Core = new Item
        {
            Name = "Perfect Hex Core",
            GoldBase = 1000,
            GoldPrice = 3000,
            GoldSell = 2100,
            Purchasable = true,
            Stacks = 1,
            Depth = 4,
            From = new[] { 3197 },
            InStore = true,
            RequiredChampion = "Viktor",
            FlatMPPoolMod = 500f,
            FlatMagicDamageMod = 60f,
            Tags = new[] { "Mana", "SpellDamage" },
            Id = 3198
        };

        #endregion

        #region Prototype Hex Core

        public static Item Prototype_Hex_Core = new Item
        {
            Name = "Prototype Hex Core",
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3196 },
            SpecialRecipe = 1,
            InStore = false,
            RequiredChampion = "Viktor",
            Id = 3200
        };

        #endregion

        #region Spectre's Cowl

        public static Item Spectres_Cowl = new Item
        {
            Name = "Spectre's Cowl",
            GoldBase = 300,
            GoldPrice = 1200,
            GoldSell = 840,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1028, 1033 },
            Into = new[] { 3065, 3102 },
            InStore = true,
            FlatHPPoolMod = 200f,
            FlatSpellBlockMod = 35f,
            Tags = new[] { "Health", "HealthRegen", "SpellBlock" },
            Id = 3211
        };

        #endregion

        #region Mikael's Crucible

        public static Item Mikaels_Crucible = new Item
        {
            Name = "Mikael's Crucible",
            GoldBase = 950,
            GoldPrice = 2450,
            GoldSell = 1715,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3028, 3114 },
            InStore = true,
            FlatSpellBlockMod = 40f,
            Tags = new[] { "Active", "CooldownReduction", "ManaRegen", "SpellBlock" },
            Id = 3222
        };

        #endregion

        #region Berserker's Greaves Enchantment: Homeguard

        public static Item Berserkers_Greaves_Enchantment_Homeguard = new Item
        {
            Name = "Berserker's Greaves Enchantment: Homeguard",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsHomeguard",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3006 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            PercentAttackSpeedMod = 0.25f,
            Id = 3250
        };

        #endregion

        #region Berserker's Greaves Enchantment: Captain

        public static Item Berserkers_Greaves_Enchantment_Captain = new Item
        {
            Name = "Berserker's Greaves Enchantment: Captain",
            GoldBase = 600,
            GoldPrice = 1600,
            GoldSell = 1120,
            Purchasable = true,
            Group = "BootsCaptain",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3006 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            PercentAttackSpeedMod = 0.25f,
            Id = 3251
        };

        #endregion

        #region Berserker's Greaves Enchantment: Furor

        public static Item Berserkers_Greaves_Enchantment_Furor = new Item
        {
            Name = "Berserker's Greaves Enchantment: Furor",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsFuror",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3006 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            PercentAttackSpeedMod = 0.25f,
            Id = 3252
        };

        #endregion

        #region Berserker's Greaves Enchantment: Distortion

        public static Item Berserkers_Greaves_Enchantment_Distortion = new Item
        {
            Name = "Berserker's Greaves Enchantment: Distortion",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsDistortion",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3006 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            PercentAttackSpeedMod = 0.25f,
            Id = 3253
        };

        #endregion

        #region Berserker's Greaves Enchantment: Alacrity

        public static Item Berserkers_Greaves_Enchantment_Alacrity = new Item
        {
            Name = "Berserker's Greaves Enchantment: Alacrity",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsAlacrity",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3006 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            PercentAttackSpeedMod = 0.25f,
            Id = 3254
        };

        #endregion

        #region Sorcerer's Shoes Enchantment: Homeguard

        public static Item Sorcerers_Shoes_Enchantment_Homeguard = new Item
        {
            Name = "Sorcerer's Shoes Enchantment: Homeguard",
            GoldBase = 475,
            GoldPrice = 1575,
            GoldSell = 1103,
            Purchasable = true,
            Group = "BootsHomeguard",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3020 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            Id = 3255
        };

        #endregion

        #region Sorcerer's Shoes Enchantment: Captain

        public static Item Sorcerers_Shoes_Enchantment_Captain = new Item
        {
            Name = "Sorcerer's Shoes Enchantment: Captain",
            GoldBase = 600,
            GoldPrice = 1700,
            GoldSell = 1190,
            Purchasable = true,
            Group = "BootsCaptain",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3020 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            Id = 3256
        };

        #endregion

        #region Sorcerer's Shoes Enchantment: Furor

        public static Item Sorcerers_Shoes_Enchantment_Furor = new Item
        {
            Name = "Sorcerer's Shoes Enchantment: Furor",
            GoldBase = 475,
            GoldPrice = 1575,
            GoldSell = 1103,
            Purchasable = true,
            Group = "BootsFuror",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3020 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            Id = 3257
        };

        #endregion

        #region Sorcerer's Shoes Enchantment: Distortion

        public static Item Sorcerers_Shoes_Enchantment_Distortion = new Item
        {
            Name = "Sorcerer's Shoes Enchantment: Distortion",
            GoldBase = 475,
            GoldPrice = 1575,
            GoldSell = 1103,
            Purchasable = true,
            Group = "BootsDistortion",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3020 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            Id = 3258
        };

        #endregion

        #region Sorcerer's Shoes Enchantment: Alacrity

        public static Item Sorcerers_Shoes_Enchantment_Alacrity = new Item
        {
            Name = "Sorcerer's Shoes Enchantment: Alacrity",
            GoldBase = 475,
            GoldPrice = 1575,
            GoldSell = 1103,
            Purchasable = true,
            Group = "BootsAlacrity",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3020 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            Id = 3259
        };

        #endregion

        #region Ninja Tabi Enchantment: Homeguard

        public static Item Ninja_Tabi_Enchantment_Homeguard = new Item
        {
            Name = "Ninja Tabi Enchantment: Homeguard",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsHomeguard",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3047 },
            InStore = true,
            HideFromAll = true,
            FlatArmorMod = 25f,
            FlatMovementSpeedMod = 45f,
            Id = 3260
        };

        #endregion

        #region Ninja Tabi Enchantment: Captain

        public static Item Ninja_Tabi_Enchantment_Captain = new Item
        {
            Name = "Ninja Tabi Enchantment: Captain",
            GoldBase = 600,
            GoldPrice = 1600,
            GoldSell = 1120,
            Purchasable = true,
            Group = "BootsCaptain",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3047 },
            InStore = true,
            HideFromAll = true,
            FlatArmorMod = 25f,
            FlatMovementSpeedMod = 45f,
            Id = 3261
        };

        #endregion

        #region Ninja Tabi Enchantment: Furor

        public static Item Ninja_Tabi_Enchantment_Furor = new Item
        {
            Name = "Ninja Tabi Enchantment: Furor",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsFuror",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3047 },
            InStore = true,
            HideFromAll = true,
            FlatArmorMod = 25f,
            FlatMovementSpeedMod = 45f,
            Id = 3262
        };

        #endregion

        #region Ninja Tabi Enchantment: Distortion

        public static Item Ninja_Tabi_Enchantment_Distortion = new Item
        {
            Name = "Ninja Tabi Enchantment: Distortion",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsDistortion",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3047 },
            InStore = true,
            HideFromAll = true,
            FlatArmorMod = 25f,
            FlatMovementSpeedMod = 45f,
            Id = 3263
        };

        #endregion

        #region Ninja Tabi Enchantment: Alacrity

        public static Item Ninja_Tabi_Enchantment_Alacrity = new Item
        {
            Name = "Ninja Tabi Enchantment: Alacrity",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsAlacrity",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3047 },
            InStore = true,
            HideFromAll = true,
            FlatArmorMod = 25f,
            FlatMovementSpeedMod = 45f,
            Id = 3264
        };

        #endregion

        #region Mercury's Treads Enchantment: Homeguard

        public static Item Mercurys_Treads_Enchantment_Homeguard = new Item
        {
            Name = "Mercury's Treads Enchantment: Homeguard",
            GoldBase = 475,
            GoldPrice = 1675,
            GoldSell = 1173,
            Purchasable = true,
            Group = "BootsHomeguard",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3111 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            FlatSpellBlockMod = 25f,
            Id = 3265
        };

        #endregion

        #region Mercury's Treads Enchantment: Captain

        public static Item Mercurys_Treads_Enchantment_Captain = new Item
        {
            Name = "Mercury's Treads Enchantment: Captain",
            GoldBase = 600,
            GoldPrice = 1800,
            GoldSell = 1260,
            Purchasable = true,
            Group = "BootsCaptain",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3111 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            FlatSpellBlockMod = 25f,
            Id = 3266
        };

        #endregion

        #region Mercury's Treads Enchantment: Furor

        public static Item Mercurys_Treads_Enchantment_Furor = new Item
        {
            Name = "Mercury's Treads Enchantment: Furor",
            GoldBase = 475,
            GoldPrice = 1675,
            GoldSell = 1173,
            Purchasable = true,
            Group = "BootsFuror",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3111 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            FlatSpellBlockMod = 25f,
            Id = 3267
        };

        #endregion

        #region Mercury's Treads Enchantment: Distortion

        public static Item Mercurys_Treads_Enchantment_Distortion = new Item
        {
            Name = "Mercury's Treads Enchantment: Distortion",
            GoldBase = 475,
            GoldPrice = 1675,
            GoldSell = 1173,
            Purchasable = true,
            Group = "BootsDistortion",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3111 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            FlatSpellBlockMod = 25f,
            Id = 3268
        };

        #endregion

        #region Mercury's Treads Enchantment: Alacrity

        public static Item Mercurys_Treads_Enchantment_Alacrity = new Item
        {
            Name = "Mercury's Treads Enchantment: Alacrity",
            GoldBase = 475,
            GoldPrice = 1675,
            GoldSell = 1173,
            Purchasable = true,
            Group = "BootsAlacrity",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3111 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            FlatSpellBlockMod = 25f,
            Id = 3269
        };

        #endregion

        #region Boots of Mobility Enchantment: Homeguard

        public static Item Boots_of_Mobility_Enchantment_Homeguard = new Item
        {
            Name = "Boots of Mobility Enchantment: Homeguard",
            GoldBase = 475,
            GoldPrice = 1275,
            GoldSell = 893,
            Purchasable = true,
            Group = "BootsHomeguard",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3117 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 105f,
            Id = 3270
        };

        #endregion

        #region Boots of Mobility Enchantment: Captain

        public static Item Boots_of_Mobility_Enchantment_Captain = new Item
        {
            Name = "Boots of Mobility Enchantment: Captain",
            GoldBase = 600,
            GoldPrice = 1400,
            GoldSell = 980,
            Purchasable = true,
            Group = "BootsCaptain",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3117 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 105f,
            Id = 3271
        };

        #endregion

        #region Boots of Mobility Enchantment: Furor

        public static Item Boots_of_Mobility_Enchantment_Furor = new Item
        {
            Name = "Boots of Mobility Enchantment: Furor",
            GoldBase = 475,
            GoldPrice = 1275,
            GoldSell = 893,
            Purchasable = true,
            Group = "BootsFuror",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3117 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 105f,
            Id = 3272
        };

        #endregion

        #region Boots of Mobility Enchantment: Distortion

        public static Item Boots_of_Mobility_Enchantment_Distortion = new Item
        {
            Name = "Boots of Mobility Enchantment: Distortion",
            GoldBase = 475,
            GoldPrice = 1275,
            GoldSell = 893,
            Purchasable = true,
            Group = "BootsDistortion",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3117 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 105f,
            Id = 3273
        };

        #endregion

        #region Boots of Mobility Enchantment: Alacrity

        public static Item Boots_of_Mobility_Enchantment_Alacrity = new Item
        {
            Name = "Boots of Mobility Enchantment: Alacrity",
            GoldBase = 475,
            GoldPrice = 1275,
            GoldSell = 893,
            Purchasable = true,
            Group = "BootsAlacrity",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3117 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 105f,
            Id = 3274
        };

        #endregion

        #region Ionian Boots of Lucidity Enchantment: Homeguard

        public static Item Ionian_Boots_of_Lucidity_Enchantment_Homeguard = new Item
        {
            Name = "Ionian Boots of Lucidity Enchantment: Homeguard",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsHomeguard",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3158 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            Id = 3275
        };

        #endregion

        #region Ionian Boots of Lucidity Enchantment: Captain

        public static Item Ionian_Boots_of_Lucidity_Enchantment_Captain = new Item
        {
            Name = "Ionian Boots of Lucidity Enchantment: Captain",
            GoldBase = 600,
            GoldPrice = 1600,
            GoldSell = 1120,
            Purchasable = true,
            Group = "BootsCaptain",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3158 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            Id = 3276
        };

        #endregion

        #region Ionian Boots of Lucidity Enchantment: Furor

        public static Item Ionian_Boots_of_Lucidity_Enchantment_Furor = new Item
        {
            Name = "Ionian Boots of Lucidity Enchantment: Furor",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsFuror",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3158 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            Id = 3277
        };

        #endregion

        #region Ionian Boots of Lucidity Enchantment: Distortion

        public static Item Ionian_Boots_of_Lucidity_Enchantment_Distortion = new Item
        {
            Name = "Ionian Boots of Lucidity Enchantment: Distortion",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsDistortion",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3158 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            Id = 3278
        };

        #endregion

        #region Ionian Boots of Lucidity Enchantment: Alacrity

        public static Item Ionian_Boots_of_Lucidity_Enchantment_Alacrity = new Item
        {
            Name = "Ionian Boots of Lucidity Enchantment: Alacrity",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsAlacrity",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3158 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 45f,
            Id = 3279
        };

        #endregion

        #region Boots of Swiftness Enchantment: Homeguard

        public static Item Boots_of_Swiftness_Enchantment_Homeguard = new Item
        {
            Name = "Boots of Swiftness Enchantment: Homeguard",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsHomeguard",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3009 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 60f,
            Id = 3280
        };

        #endregion

        #region Boots of Swiftness Enchantment: Captain

        public static Item Boots_of_Swiftness_Enchantment_Captain = new Item
        {
            Name = "Boots of Swiftness Enchantment: Captain",
            GoldBase = 600,
            GoldPrice = 1600,
            GoldSell = 1120,
            Purchasable = true,
            Group = "BootsCaptain",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3009 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 60f,
            Id = 3281
        };

        #endregion

        #region Boots of Swiftness Enchantment: Furor

        public static Item Boots_of_Swiftness_Enchantment_Furor = new Item
        {
            Name = "Boots of Swiftness Enchantment: Furor",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsFuror",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3009 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 60f,
            Id = 3282
        };

        #endregion

        #region Boots of Swiftness Enchantment: Distortion

        public static Item Boots_of_Swiftness_Enchantment_Distortion = new Item
        {
            Name = "Boots of Swiftness Enchantment: Distortion",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsDistortion",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3009 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 60f,
            Id = 3283
        };

        #endregion

        #region Boots of Swiftness Enchantment: Alacrity

        public static Item Boots_of_Swiftness_Enchantment_Alacrity = new Item
        {
            Name = "Boots of Swiftness Enchantment: Alacrity",
            GoldBase = 475,
            GoldPrice = 1475,
            GoldSell = 1033,
            Purchasable = true,
            Group = "BootsAlacrity",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3009 },
            InStore = true,
            HideFromAll = true,
            FlatMovementSpeedMod = 60f,
            Id = 3284
        };

        #endregion

        #region Luden's Echo

        public static Item Ludens_Echo = new Item
        {
            Name = "Luden's Echo",
            GoldBase = 650,
            GoldPrice = 3100,
            GoldSell = 2170,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1058, 3113 },
            InStore = true,
            FlatMagicDamageMod = 120f,
            PercentMovementSpeedMod = 0.07f,
            Tags = new[] { "NonbootsMovement", "OnHit", "SpellDamage" },
            Id = 3285
        };

        #endregion

        #region Luden's Echo

        public static Item Ludens_Echo2 = new Item
        {
            Name = "Luden's Echo",
            GoldBase = 1090,
            GoldPrice = 2800,
            GoldSell = 1960,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1026, 3113 },
            InStore = true,
            FlatMagicDamageMod = 100f,
            PercentMovementSpeedMod = 0.07f,
            Tags = new[] { "NonbootsMovement", "OnHit", "SpellDamage" },
            Id = 3286
        };

        #endregion

        #region Twin Shadows

        public static Item Twin_Shadows2 = new Item
        {
            Name = "Twin Shadows",
            GoldBase = 630,
            GoldPrice = 2300,
            GoldSell = 1610,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3108, 3113 },
            InStore = true,
            FlatMagicDamageMod = 80f,
            PercentMovementSpeedMod = 0.06f,
            Tags = new[] { "Active", "CooldownReduction", "NonbootsMovement", "Slow", "SpellDamage", "Vision" },
            Id = 3290
        };

        #endregion

        #region Ancient Coin

        public static Item Ancient_Coin = new Item
        {
            Name = "Ancient Coin",
            GoldBase = 365,
            GoldPrice = 365,
            GoldSell = 146,
            Purchasable = true,
            Group = "GoldBase",
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3096 },
            InStore = true,
            Tags = new[] { "GoldPer", "Lane", "ManaRegen" },
            Id = 3301
        };

        #endregion

        #region Relic Shield

        public static Item Relic_Shield = new Item
        {
            Name = "Relic Shield",
            GoldBase = 365,
            GoldPrice = 365,
            GoldSell = 146,
            Purchasable = true,
            Group = "GoldBase",
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3097 },
            InStore = true,
            FlatHPPoolMod = 75f,
            Tags = new[] { "Aura", "GoldPer", "Health", "Lane" },
            Id = 3302
        };

        #endregion

        #region Spellthief's Edge

        public static Item Spellthiefs_Edge = new Item
        {
            Name = "Spellthief's Edge",
            GoldBase = 365,
            GoldPrice = 365,
            GoldSell = 146,
            Purchasable = true,
            Group = "GoldBase",
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3098 },
            InStore = true,
            FlatMagicDamageMod = 5f,
            Tags = new[] { "GoldPer", "Lane", "ManaRegen", "SpellDamage" },
            Id = 3303
        };

        #endregion

        #region Warding Totem (Trinket)

        public static Item Warding_Totem_Trinket = new Item
        {
            Name = "Warding Totem (Trinket)",
            Range = 600f,
            Purchasable = true,
            Group = "RelicBase",
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3361, 3362 },
            InStore = true,
            Tags = new[] { "Active", "Jungle", "Lane", "Trinket", "Vision" },
            Id = 3340
        };

        #endregion

        #region Sweeping Lens (Trinket)

        public static Item Sweeping_Lens_Trinket = new Item
        {
            Name = "Sweeping Lens (Trinket)",
            Range = 400f,
            Purchasable = true,
            Group = "RelicBase",
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3364 },
            InStore = true,
            Tags = new[] { "Active", "Jungle", "Trinket", "Vision" },
            Id = 3341
        };

        #endregion

        #region Scrying Orb (Trinket)

        public static Item Scrying_Orb_Trinket = new Item
        {
            Name = "Scrying Orb (Trinket)",
            Range = 2500f,
            Purchasable = true,
            Group = "RelicBase",
            Stacks = 1,
            Depth = 1,
            Into = new[] { 3363 },
            InStore = true,
            Tags = new[] { "Active", "Trinket", "Vision" },
            Id = 3342
        };

        #endregion

        #region Soul Anchor (Trinket)

        public static Item Soul_Anchor_Trinket = new Item
        {
            Name = "Soul Anchor (Trinket)",
            Purchasable = true,
            Group = "RelicBase",
            Stacks = 1,
            Depth = 1,
            InStore = true,
            Tags = new[] { "Active", "Trinket", "Vision" },
            Id = 3345
        };

        #endregion

        #region Greater Stealth Totem (Trinket)

        public static Item Greater_Stealth_Totem_Trinket = new Item
        {
            Name = "Greater Stealth Totem (Trinket)",
            Range = 600f,
            GoldBase = 250,
            GoldPrice = 250,
            GoldSell = 175,
            Purchasable = true,
            Group = "RelicBase",
            Stacks = 1,
            Depth = 2,
            From = new[] { 3340 },
            InStore = true,
            Tags = new[] { "Active", "Trinket", "Vision" },
            Id = 3361
        };

        #endregion

        #region Greater Vision Totem (Trinket)

        public static Item Greater_Vision_Totem_Trinket = new Item
        {
            Name = "Greater Vision Totem (Trinket)",
            Range = 600f,
            GoldBase = 250,
            GoldPrice = 250,
            GoldSell = 175,
            Purchasable = true,
            Group = "RelicBase",
            Stacks = 1,
            Depth = 2,
            From = new[] { 3340 },
            InStore = true,
            Tags = new[] { "Active", "Trinket", "Vision" },
            Id = 3362
        };

        #endregion

        #region Farsight Orb (Trinket)

        public static Item Farsight_Orb_Trinket = new Item
        {
            Name = "Farsight Orb (Trinket)",
            Range = 4000f,
            GoldBase = 250,
            GoldPrice = 250,
            GoldSell = 175,
            Purchasable = true,
            Group = "RelicBase",
            Stacks = 1,
            Depth = 2,
            From = new[] { 3342 },
            InStore = true,
            Tags = new[] { "Active", "Trinket", "Vision" },
            Id = 3363
        };

        #endregion

        #region Oracle's Lens (Trinket)

        public static Item Oracles_Lens_Trinket = new Item
        {
            Name = "Oracle's Lens (Trinket)",
            Range = 600f,
            GoldBase = 250,
            GoldPrice = 250,
            GoldSell = 175,
            Purchasable = true,
            Group = "RelicBase",
            Stacks = 1,
            Depth = 2,
            From = new[] { 3341 },
            InStore = true,
            Tags = new[] { "Active", "Trinket", "Vision" },
            Id = 3364
        };

        #endregion

        #region Face of the Mountain

        public static Item Face_of_the_Mountain = new Item
        {
            Name = "Face of the Mountain",
            Range = 750f,
            GoldBase = 485,
            GoldPrice = 2200,
            GoldSell = 880,
            Purchasable = true,
            Group = "GoldBase",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3097, 3067 },
            InStore = true,
            FlatHPPoolMod = 500f,
            Tags = new[] { "CooldownReduction", "GoldPer", "Health", "HealthRegen" },
            Id = 3401
        };

        #endregion

        #region Golden Transcendence

        public static Item Golden_Transcendence = new Item
        {
            Name = "Golden Transcendence",
            Group = "RelicBase",
            Stacks = 1,
            Depth = 1,
            InStore = false,
            Tags = new[] { "Active", "Trinket" },
            Id = 3460
        };

        #endregion

        #region Ardent Censer

        public static Item Ardent_Censer = new Item
        {
            Name = "Ardent Censer",
            GoldBase = 650,
            GoldPrice = 2100,
            GoldSell = 1470,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3114, 3113 },
            InStore = true,
            FlatMagicDamageMod = 40f,
            Tags = new[] { "CooldownReduction", "ManaRegen", "NonbootsMovement", "SpellDamage" },
            Id = 3504
        };

        #endregion

        #region Essence Reaver

        public static Item Essence_Reaver = new Item
        {
            Name = "Essence Reaver",
            GoldBase = 850,
            GoldPrice = 3200,
            GoldSell = 2240,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 1053, 1038 },
            InStore = true,
            FlatPhysicalDamageMod = 80f,
            PercentLifeStealMod = 0.1f,
            Tags = new[] { "CooldownReduction", "Damage", "LifeSteal", "ManaRegen" },
            Id = 3508
        };

        #endregion

        #region Zz'Rot Portal

        public static Item ZzRot_Portal = new Item
        {
            Name = "Zz'Rot Portal",
            GoldBase = 950,
            GoldPrice = 2800,
            GoldSell = 1960,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 2053, 1057 },
            InStore = true,
            FlatArmorMod = 60f,
            FlatSpellBlockMod = 60f,
            Tags = new[] { "Armor", "HealthRegen", "NonbootsMovement", "SpellBlock" },
            Id = 3512
        };

        #endregion

        #region The Black Spear

        public static Item The_Black_Spear = new Item
        {
            Name = "The Black Spear",
            Purchasable = true,
            Group = "TheBlackSpear",
            Stacks = 1,
            Depth = 1,
            InStore = true,
            RequiredChampion = "Kalista",
            Tags = new[] { "Active" },
            Id = 3599
        };

        #endregion

        #region Stalker's Blade

        public static Item Stalkers_Blade = new Item
        {
            Name = "Stalker's Blade",
            GoldBase = 450,
            GoldPrice = 850,
            GoldSell = 595,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 2,
            From = new[] { 1039 },
            Into = new[] { 3707, 3708, 3709, 3710 },
            InStore = true,
            Tags = new[] { "Damage", "HealthRegen", "Jungle", "ManaRegen", "OnHit" },
            Id = 3706
        };

        #endregion

        #region Stalker's Blade Enchantment: Warrior

        public static Item Stalkers_Blade_Enchantment_Warrior = new Item
        {
            Name = "Stalker's Blade Enchantment: Warrior",
            GoldBase = 63,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3706, 3134 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "ArmorPenetration", "CooldownReduction", "Damage", "Jungle" },
            Id = 3707
        };

        #endregion

        #region Stalker's Blade Enchantment: Magus

        public static Item Stalkers_Blade_Enchantment_Magus = new Item
        {
            Name = "Stalker's Blade Enchantment: Magus",
            GoldBase = 580,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3706, 3108 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "CooldownReduction", "Jungle", "SpellDamage" },
            Id = 3708
        };

        #endregion

        #region Bami's Cinder Stalkers Blade Enchantment Cinderhulk

        public static Item Bamis_Cinder_Stalkers_Blade_Enchantment_Cinderhulk = new Item
        {
            Name = "Bami's Cinder Stalkers Blade Enchantment Cinderhulk",
            GoldBase = 400,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3706, 3751 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "Health", "Jungle" },
            Id = 3709
        };

        #endregion

        #region Stalker's Blade Enchantment: Devourer

        public static Item Stalkers_Blade_Enchantment_Devourer = new Item
        {
            Name = "Stalker's Blade Enchantment: Devourer",
            GoldBase = 500,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3706, 1042, 1042 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "AttackSpeed", "Damage", "Jungle", "OnHit" },
            Id = 3710
        };

        #endregion

        #region Poacher's Knife

        public static Item Poachers_Knife = new Item
        {
            Name = "Poacher's Knife",
            GoldBase = 450,
            GoldPrice = 850,
            GoldSell = 595,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 2,
            From = new[] { 1039 },
            Into = new[] { 3719, 3720, 3721, 3722 },
            InStore = true,
            Tags = new[] { "Damage", "HealthRegen", "Jungle", "ManaRegen", "NonbootsMovement", "OnHit" },
            Id = 3711
        };

        #endregion

        #region Ranger's Trailblazer

        public static Item Rangers_Trailblazer = new Item
        {
            Name = "Ranger's Trailblazer",
            GoldBase = 450,
            GoldPrice = 850,
            GoldSell = 595,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 2,
            From = new[] { 1039 },
            Into = new[] { 3723, 3724, 3725, 3726 },
            InStore = true,
            Tags = new[] { "Damage", "HealthRegen", "Jungle", "ManaRegen", "OnHit" },
            Id = 3713
        };

        #endregion

        #region Skirmisher's Sabre Enchantment: Warrior

        public static Item Skirmishers_Sabre_Enchantment_Warrior = new Item
        {
            Name = "Skirmisher's Sabre Enchantment: Warrior",
            GoldBase = 63,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3715, 3134 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "ArmorPenetration", "CooldownReduction", "Damage", "Jungle" },
            Id = 3714
        };

        #endregion

        #region Skirmisher's Sabre

        public static Item Skirmishers_Sabre = new Item
        {
            Name = "Skirmisher's Sabre",
            GoldBase = 450,
            GoldPrice = 850,
            GoldSell = 595,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 2,
            From = new[] { 1039 },
            Into = new[] { 3714, 3716, 3717, 3718 },
            InStore = true,
            Tags = new[] { "Damage", "HealthRegen", "Jungle", "ManaRegen", "OnHit" },
            Id = 3715
        };

        #endregion

        #region Skirmisher's Sabre Enchantment: Magus

        public static Item Skirmishers_Sabre_Enchantment_Magus = new Item
        {
            Name = "Skirmisher's Sabre Enchantment: Magus",
            GoldBase = 580,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3715, 3108 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "CooldownReduction", "Jungle", "SpellDamage" },
            Id = 3716
        };

        #endregion

        #region Bami's Cinder Skirmishers Sabre Enchantment Cinderhulk

        public static Item Bamis_Cinder_Skirmishers_Sabre_Enchantment_Cinderhulk = new Item
        {
            Name = "Bami's Cinder Skirmishers Sabre Enchantment Cinderhulk",
            GoldBase = 400,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3715, 3751 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "Health", "Jungle" },
            Id = 3717
        };

        #endregion

        #region Skirmisher's Sabre Enchantment: Devourer

        public static Item Skirmishers_Sabre_Enchantment_Devourer = new Item
        {
            Name = "Skirmisher's Sabre Enchantment: Devourer",
            GoldBase = 500,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3715, 1042, 1042 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "AttackSpeed", "Damage", "Jungle", "OnHit" },
            Id = 3718
        };

        #endregion

        #region Poacher's Knife Enchantment: Warrior

        public static Item Poachers_Knife_Enchantment_Warrior = new Item
        {
            Name = "Poacher's Knife Enchantment: Warrior",
            GoldBase = 63,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3711, 3134 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "ArmorPenetration", "CooldownReduction", "Damage", "Jungle" },
            Id = 3719
        };

        #endregion

        #region Poacher's Knife Enchantment: Magus

        public static Item Poachers_Knife_Enchantment_Magus = new Item
        {
            Name = "Poacher's Knife Enchantment: Magus",
            GoldBase = 580,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3711, 3108 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "CooldownReduction", "Jungle", "SpellDamage" },
            Id = 3720
        };

        #endregion

        #region Bami's Cinder Poachers Knife Enchantment Cinderhulk

        public static Item Bamis_Cinder_Poachers_Knife_Enchantment_Cinderhulk = new Item
        {
            Name = "Bami's Cinder Poachers Knife Enchantment Cinderhulk",
            GoldBase = 400,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3711, 3751 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "Health", "Jungle" },
            Id = 3721
        };

        #endregion

        #region Poacher's Knife Enchantment: Devourer

        public static Item Poachers_Knife_Enchantment_Devourer = new Item
        {
            Name = "Poacher's Knife Enchantment: Devourer",
            GoldBase = 500,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3711, 1042, 1042 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "AttackSpeed", "Damage", "Jungle", "OnHit" },
            Id = 3722
        };

        #endregion

        #region Ranger's Trailblazer Enchantment: Warrior

        public static Item Rangers_Trailblazer_Enchantment_Warrior = new Item
        {
            Name = "Ranger's Trailblazer Enchantment: Warrior",
            GoldBase = 63,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3713, 3134 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "ArmorPenetration", "CooldownReduction", "Damage", "Jungle" },
            Id = 3723
        };

        #endregion

        #region Ranger's Trailblazer Enchantment: Magus

        public static Item Rangers_Trailblazer_Enchantment_Magus = new Item
        {
            Name = "Ranger's Trailblazer Enchantment: Magus",
            GoldBase = 580,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3713, 3108 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "CooldownReduction", "Jungle", "SpellDamage" },
            Id = 3724
        };

        #endregion

        #region Bami's Cinder Rangers Trailblazer Enchantment Cinderhulk

        public static Item Bamis_Cinder_Rangers_Trailblazer_Enchantment_Cinderhulk = new Item
        {
            Name = "Bami's Cinder Rangers Trailblazer Enchantment Cinderhulk",
            GoldBase = 400,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3713, 3751 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "Health", "Jungle" },
            Id = 3725
        };

        #endregion

        #region Ranger's Trailblazer Enchantment: Devourer

        public static Item Rangers_Trailblazer_Enchantment_Devourer = new Item
        {
            Name = "Ranger's Trailblazer Enchantment: Devourer",
            GoldBase = 500,
            GoldPrice = 2250,
            GoldSell = 1575,
            Purchasable = true,
            Group = "JungleItems",
            Stacks = 1,
            Depth = 3,
            From = new[] { 3713, 1042, 1042 },
            InStore = true,
            HideFromAll = true,
            Tags = new[] { "AttackSpeed", "Damage", "Jungle", "OnHit" },
            Id = 3726
        };

        #endregion

        #region Bami's Cinder

        public static Item Bamis_Cinder = new Item
        {
            Name = "Bami's Cinder",
            GoldBase = 600,
            GoldPrice = 1000,
            GoldSell = 700,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1028 },
            Into = new[] { 3068, 3709, 3717, 3721, 3725 },
            InStore = true,
            FlatHPPoolMod = 300f,
            Tags = new[] { "Health" },
            Id = 3751
        };

        #endregion

        #region Righteous Glory

        public static Item Righteous_Glory = new Item
        {
            Name = "Righteous Glory",
            GoldBase = 700,
            GoldPrice = 2500,
            GoldSell = 1750,
            Purchasable = true,
            Stacks = 1,
            Depth = 3,
            From = new[] { 3010, 3801 },
            InStore = true,
            FlatHPPoolMod = 650f,
            FlatMPPoolMod = 300f,
            Tags = new[] { "Active", "Health", "HealthRegen", "Mana", "NonbootsMovement", "Slow" },
            Id = 3800
        };

        #endregion

        #region Crystalline Bracer

        public static Item Crystalline_Bracer = new Item
        {
            Name = "Crystalline Bracer",
            GoldBase = 20,
            GoldPrice = 600,
            GoldSell = 420,
            Purchasable = true,
            Stacks = 1,
            Depth = 2,
            From = new[] { 1028, 1006 },
            Into = new[] { 3083, 3800 },
            InStore = true,
            FlatHPPoolMod = 200f,
            Tags = new[] { "Health", "HealthRegen" },
            Id = 3801
        };

        #endregion

        #endregion
    }
}