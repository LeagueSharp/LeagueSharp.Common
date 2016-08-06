namespace LeagueSharp.Common.Data
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ItemData
    {
        #region Static Fields

        public static Item Abyssal_Scepter = new Item
                                                 {
                                                     Name = "Abyssal Scepter", Range = 700f, GoldBase = 780,
                                                     GoldPrice = 2350, GoldSell = 1645, Purchasable = true, Stacks = 1,
                                                     Depth = 3, From = new[] { 1026, 1057 }, InStore = true,
                                                     FlatMagicDamageMod = 70f, FlatSpellBlockMod = 50f,
                                                     Tags = new[] { "Aura", "SpellBlock", "SpellDamage" }, Id = 3001
                                                 };

        public static Item Aegis_of_the_Legion = new Item
                                                     {
                                                         Name = "Aegis of the Legion", GoldBase = 820, GoldPrice = 1900,
                                                         GoldSell = 1330, Purchasable = true, Stacks = 1, Depth = 2,
                                                         From = new[] { 1028, 1033, 1006 }, Into = new[] { 3190, 3060 },
                                                         InStore = true, FlatHPPoolMod = 200f, FlatSpellBlockMod = 20f,
                                                         Tags = new[] { "Aura", "Health", "HealthRegen", "SpellBlock" },
                                                         Id = 3105
                                                     };

        public static Item Aether_Wisp = new Item
                                             {
                                                 Name = "Aether Wisp", GoldBase = 415, GoldPrice = 850, GoldSell = 595,
                                                 Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1052 },
                                                 Into = new[] { 3023, 3290, 3100, 3285, 3286, 3504 }, InStore = true,
                                                 FlatMagicDamageMod = 30f,
                                                 Tags = new[] { "NonbootsMovement", "SpellDamage" }, Id = 3113
                                             };

        public static Item Amplifying_Tome = new Item
                                                 {
                                                     Name = "Amplifying Tome", GoldBase = 435, GoldPrice = 435,
                                                     GoldSell = 305, Purchasable = true, Stacks = 1, Depth = 1,
                                                     Into =
                                                         new[]
                                                             {
                                                                 3108, 3191, 3057, 3136, 3135, 3145, 3113, 3090, 3116, 3151,
                                                                 3041, 3089
                                                             },
                                                     InStore = true, FlatMagicDamageMod = 20f,
                                                     Tags = new[] { "SpellDamage" }, Id = 1052
                                                 };

        public static Item Ancient_Coin = new Item
                                              {
                                                  Name = "Ancient Coin", GoldBase = 365, GoldPrice = 365, GoldSell = 146,
                                                  Purchasable = true, Group = "GoldBase", Stacks = 1, Depth = 1,
                                                  Into = new[] { 3096 }, InStore = true,
                                                  Tags = new[] { "GoldPer", "Lane", "ManaRegen" }, Id = 3301
                                              };

        public static Item Archangels_Staff = new Item
                                                  {
                                                      Name = "Archangel's Staff", GoldBase = 1100, GoldPrice = 3100,
                                                      GoldSell = 2170, Purchasable = true, Stacks = 1, Depth = 3,
                                                      From = new[] { 3070, 1058 }, Into = new[] { 3040 }, InStore = true,
                                                      FlatMPPoolMod = 250f, FlatMPRegenMod = 0.5f,
                                                      FlatMagicDamageMod = 80f,
                                                      Tags = new[] { "Mana", "ManaRegen", "SpellDamage" }, Id = 3003
                                                  };

        public static Item Archangels_Staff_Crystal_Scar = new Item
                                                               {
                                                                   Name = "Archangel's Staff (Crystal Scar)",
                                                                   GoldBase = 1100, GoldPrice = 3100, GoldSell = 2170,
                                                                   Purchasable = true, Stacks = 1, Depth = 3,
                                                                   From = new[] { 3070, 1058 }, Into = new[] { 3040 },
                                                                   InStore = true, FlatMPPoolMod = 250f,
                                                                   FlatMPRegenMod = 0.5f, FlatMagicDamageMod = 80f,
                                                                   Tags = new[] { "Mana", "ManaRegen", "SpellDamage" },
                                                                   Id = 3007
                                                               };

        public static Item Ardent_Censer = new Item
                                               {
                                                   Name = "Ardent Censer", GoldBase = 650, GoldPrice = 2100,
                                                   GoldSell = 1470, Purchasable = true, Stacks = 1, Depth = 3,
                                                   From = new[] { 3114, 3113 }, InStore = true, FlatMagicDamageMod = 40f,
                                                   Tags =
                                                       new[]
                                                           {
                                                               "CooldownReduction", "ManaRegen", "NonbootsMovement",
                                                               "SpellDamage"
                                                           },
                                                   Id = 3504
                                               };

        public static Item Athenes_Unholy_Grail = new Item
                                                      {
                                                          Name = "Athene's Unholy Grail", GoldBase = 545, GoldPrice = 2700,
                                                          GoldSell = 1890, Purchasable = true, Stacks = 1, Depth = 3,
                                                          From = new[] { 3108, 3028, 1052 }, InStore = true,
                                                          FlatMagicDamageMod = 60f, FlatSpellBlockMod = 25f,
                                                          Tags =
                                                              new[]
                                                                  {
                                                                      "CooldownReduction", "ManaRegen", "SpellBlock",
                                                                      "SpellDamage"
                                                                  },
                                                          Id = 3174
                                                      };

        public static Item B_F_Sword = new Item
                                           {
                                               Name = "B. F. Sword", GoldBase = 1300, GoldPrice = 1300, GoldSell = 910,
                                               Purchasable = true, Stacks = 1, Depth = 1,
                                               Into = new[] { 3031, 3072, 3139, 3508 }, InStore = true,
                                               FlatPhysicalDamageMod = 40f, Tags = new[] { "Damage" }, Id = 1038
                                           };

        public static Item Bamis_Cinder = new Item
                                              {
                                                  Name = "Bami's Cinder", GoldBase = 600, GoldPrice = 1000, GoldSell = 700,
                                                  Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1028 },
                                                  Into = new[] { 3068, 3709, 3717, 3721, 3725 }, InStore = true,
                                                  FlatHPPoolMod = 300f, Tags = new[] { "Health" }, Id = 3751
                                              };

        public static Item Banner_of_Command = new Item
                                                   {
                                                       Name = "Banner of Command", GoldBase = 280, GoldPrice = 3000,
                                                       GoldSell = 2100, Purchasable = true, Stacks = 1, Depth = 3,
                                                       From = new[] { 3105, 3108 }, InStore = true, FlatHPPoolMod = 200f,
                                                       FlatMagicDamageMod = 60f, FlatSpellBlockMod = 20f,
                                                       Tags =
                                                           new[]
                                                               {
                                                                   "Active", "Aura", "CooldownReduction", "Health",
                                                                   "HealthRegen", "SpellBlock", "SpellDamage"
                                                               },
                                                       Id = 3060
                                                   };

        public static Item Banshees_Veil = new Item
                                               {
                                                   Name = "Banshee's Veil", GoldBase = 1150, GoldPrice = 2750,
                                                   GoldSell = 1925, Purchasable = true, Stacks = 1, Depth = 3,
                                                   From = new[] { 3211, 1028 }, InStore = true, FlatHPPoolMod = 450f,
                                                   FlatSpellBlockMod = 55f,
                                                   Tags = new[] { "Health", "HealthRegen", "SpellBlock" }, Id = 3102
                                               };

        public static Item Berserkers_Greaves = new Item
                                                    {
                                                        Name = "Berserker's Greaves", GoldBase = 500, GoldPrice = 1100,
                                                        GoldSell = 770, Purchasable = true, Stacks = 1, Depth = 2,
                                                        From = new[] { 1001, 1042 },
                                                        Into = new[] { 3254, 3253, 3252, 3251, 3250 }, InStore = true,
                                                        FlatMovementSpeedMod = 45f, PercentAttackSpeedMod = 0.3f,
                                                        Tags = new[] { "AttackSpeed", "Boots" }, Id = 3006
                                                    };

        public static Item Bilgewater_Cutlass = new Item
                                                    {
                                                        Name = "Bilgewater Cutlass", Range = 550f, GoldBase = 240,
                                                        GoldPrice = 1400, GoldSell = 980, Purchasable = true, Stacks = 1,
                                                        Depth = 3, From = new[] { 1036, 1053 },
                                                        Into = new[] { 3146, 3153 }, InStore = true,
                                                        FlatPhysicalDamageMod = 25f, PercentLifeStealMod = 0.08f,
                                                        Tags = new[] { "Active", "Damage", "LifeSteal", "Slow" },
                                                        Id = 3144
                                                    };

        public static Item Blade_of_the_Ruined_King = new Item
                                                          {
                                                              Name = "Blade of the Ruined King", Range = 550f,
                                                              GoldBase = 750, GoldPrice = 3400, GoldSell = 2380,
                                                              Purchasable = true, Stacks = 1, Depth = 4,
                                                              From = new[] { 3144, 1043 }, InStore = true,
                                                              FlatPhysicalDamageMod = 25f, PercentAttackSpeedMod = 0.4f,
                                                              PercentLifeStealMod = 0.1f,
                                                              Tags =
                                                                  new[]
                                                                      {
                                                                          "Active", "AttackSpeed", "Damage", "LifeSteal",
                                                                          "NonbootsMovement", "OnHit"
                                                                      },
                                                              Id = 3153
                                                          };

        public static Item Blasting_Wand = new Item
                                               {
                                                   Name = "Blasting Wand", GoldBase = 850, GoldPrice = 850, GoldSell = 595,
                                                   Purchasable = true, Stacks = 1, Depth = 1,
                                                   Into =
                                                       new[]
                                                           {
                                                               3001, 3135, 3027, 3029, 3089, 3116, 3124, 3090, 3003, 3007,
                                                               3286
                                                           },
                                                   InStore = true, FlatMagicDamageMod = 40f,
                                                   Tags = new[] { "SpellDamage" }, Id = 1026
                                               };

        public static Item Boots_of_Mobility = new Item
                                                   {
                                                       Name = "Boots of Mobility", GoldBase = 500, GoldPrice = 800,
                                                       GoldSell = 560, Purchasable = true, Stacks = 1, Depth = 2,
                                                       From = new[] { 1001 },
                                                       Into = new[] { 3274, 3273, 3272, 3271, 3270 }, InStore = true,
                                                       FlatMovementSpeedMod = 105f, Tags = new[] { "Boots" }, Id = 3117
                                                   };

        public static Item Boots_of_Speed = new Item
                                                {
                                                    Name = "Boots of Speed", GoldBase = 300, GoldPrice = 300,
                                                    GoldSell = 210, Purchasable = true, Group = "BootsNormal", Stacks = 1,
                                                    Depth = 1, Into = new[] { 3006, 3047, 3020, 3158, 3111, 3117, 3009 },
                                                    InStore = true, FlatMovementSpeedMod = 25f, Tags = new[] { "Boots" },
                                                    Id = 1001
                                                };

        public static Item Boots_of_Swiftness = new Item
                                                    {
                                                        Name = "Boots of Swiftness", GoldBase = 500, GoldPrice = 800,
                                                        GoldSell = 560, Purchasable = true, Stacks = 1, Depth = 2,
                                                        From = new[] { 1001 },
                                                        Into = new[] { 3284, 3283, 3282, 3281, 3280 }, InStore = true,
                                                        FlatMovementSpeedMod = 65f, Tags = new[] { "Boots" }, Id = 3009
                                                    };

        public static Item Brawlers_Gloves = new Item
                                                 {
                                                     Name = "Brawler's Gloves", GoldBase = 400, GoldPrice = 400,
                                                     GoldSell = 280, Purchasable = true, Stacks = 1, Depth = 1,
                                                     Into = new[] { 3086, 3122 }, InStore = true, FlatCritChanceMod = 0.1f,
                                                     Tags = new[] { "CriticalStrike" }, Id = 1051
                                                 };

        public static Item Catalyst_the_Protector = new Item
                                                        {
                                                            Name = "Catalyst the Protector", GoldBase = 400,
                                                            GoldPrice = 1200, GoldSell = 840, Purchasable = true,
                                                            Stacks = 1, Depth = 2, From = new[] { 1028, 1027 },
                                                            Into = new[] { 3027, 3029, 3180, 3800 }, InStore = true,
                                                            FlatHPPoolMod = 225f, FlatMPPoolMod = 300f,
                                                            Tags = new[] { "Health", "HealthRegen", "Mana", "ManaRegen" },
                                                            Id = 3010
                                                        };

        public static Item Caufield_Hammer = new Item
                                                 {
                                                     Name = "Caufield Hammer", GoldBase = 345, GoldPrice = 345,
                                                     GoldSell = 138, Purchasable = true, Stacks = 1, Depth = 1,
                                                     InStore = true, Id = 3133
                                                 };

        public static Item Chain_Vest = new Item
                                            {
                                                Name = "Chain Vest", GoldBase = 500, GoldPrice = 800, GoldSell = 560,
                                                Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1029 },
                                                Into = new[] { 3075, 3068, 3026 }, InStore = true, FlatArmorMod = 40f,
                                                Tags = new[] { "Armor" }, Id = 1031
                                            };

        public static Item Chalice_of_Harmony = new Item
                                                    {
                                                        Name = "Chalice of Harmony", GoldBase = 40, GoldPrice = 900,
                                                        GoldSell = 630, Purchasable = true, Stacks = 1, Depth = 2,
                                                        From = new[] { 1004, 1033, 1004 }, Into = new[] { 3174, 3222 },
                                                        InStore = true, FlatSpellBlockMod = 25f,
                                                        Tags = new[] { "ManaRegen", "SpellBlock" }, Id = 3028
                                                    };

        public static Item Cloak_of_Agility = new Item
                                                  {
                                                      Name = "Cloak of Agility", GoldBase = 800, GoldPrice = 800,
                                                      GoldSell = 560, Purchasable = true, Stacks = 1, Depth = 1,
                                                      Into = new[] { 3046, 3031, 3104, 3185 }, InStore = true,
                                                      FlatCritChanceMod = 0.20f, Tags = new[] { "CriticalStrike" },
                                                      Id = 1018
                                                  };

        public static Item Cloth_Armor = new Item
                                             {
                                                 Name = "Cloth Armor", GoldBase = 300, GoldPrice = 300, GoldSell = 210,
                                                 Purchasable = true, Stacks = 1, Depth = 1,
                                                 Into = new[] { 3047, 1031, 3191, 3024, 3082, 3075, 2053 }, InStore = true,
                                                 FlatArmorMod = 15f, Tags = new[] { "Armor" }, Id = 1029
                                             };

        public static Item Corrupting_Potion = new Item
                                                   {
                                                       Name = "Corrupting Potion", GoldBase = 345, GoldPrice = 345,
                                                       GoldSell = 138, Purchasable = true, Stacks = 1, Depth = 1,
                                                       InStore = true,
                                                       Tags =
                                                           new[]
                                                               {
                                                                   "Active", "Consumable", "HealthRegen", "Lane",
                                                                   "ManaRegen"
                                                               },
                                                       Id = 2033
                                                   };

        public static Item Crystalline_Bracer = new Item
                                                    {
                                                        Name = "Crystalline Bracer", GoldBase = 20, GoldPrice = 600,
                                                        GoldSell = 420, Purchasable = true, Stacks = 1, Depth = 2,
                                                        From = new[] { 1028, 1006 }, Into = new[] { 3083, 3800 },
                                                        InStore = true, FlatHPPoolMod = 200f,
                                                        Tags = new[] { "Health", "HealthRegen" }, Id = 3801
                                                    };

        public static Item Cull = new Item
                                      {
                                          Name = "Cull", GoldBase = 450, GoldPrice = 450, GoldSell = 180,
                                          Purchasable = true, Group = "S6Items", Consumed = false, Stacks = 1, Depth = 1,
                                          InStore = true, FlatPhysicalDamageMod = 5f,
                                          Tags = new[] { "Lane", "Damage", "HealthRegen", "OnHit" }, Id = 1083
                                      };

        public static Item Dagger = new Item
                                        {
                                            Name = "Dagger", GoldBase = 300, GoldPrice = 300, GoldSell = 210,
                                            Purchasable = true, Stacks = 1, Depth = 1,
                                            Into = new[] { 1043, 3006, 2015, 3046, 3086, 3091, 3101, 3106, 3154, 3159 },
                                            InStore = true, PercentAttackSpeedMod = 0.12f, Tags = new[] { "AttackSpeed" },
                                            Id = 1042
                                        };

        public static Item Dead_Mans_Plate = new Item
                                                 {
                                                     Name = "Dead Man's Plate", GoldBase = 1000, GoldPrice = 2800,
                                                     GoldSell = 1960, Purchasable = true, Stacks = 1, Depth = 2,
                                                     From = new[] { 1031, 1011 }, InStore = true, FlatHPPoolMod = 500f,
                                                     FlatArmorMod = 50f,
                                                     Tags = new[] { "Health", "Armor", "Slow", "Damage", "OnHit" },
                                                     Id = 3742
                                                 };

        public static Item Deaths_Dance = new Item
                                              {
                                                  Name = "Death's Dance", GoldBase = 525, GoldPrice = 3400,
                                                  GoldSell = 2380, Purchasable = true, Group = "S6Items", Stacks = 1,
                                                  Depth = 3, From = new[] { 3133, 1037, 1053 }, InStore = true,
                                                  FlatPhysicalDamageMod = 65f,
                                                  Tags = new[] { "Damage", "LifeSteal", "CooldownReduction" }, Id = 3812
                                              };

        public static Item Dervish_Blade = new Item
                                               {
                                                   Name = "Dervish Blade", GoldBase = 200, GoldPrice = 2700,
                                                   GoldSell = 1890, Purchasable = true, Stacks = 1, Depth = 3,
                                                   From = new[] { 3140, 3101 }, InStore = true,
                                                   PercentAttackSpeedMod = 0.5f, FlatSpellBlockMod = 45f,
                                                   Tags =
                                                       new[]
                                                           {
                                                               "Active", "AttackSpeed", "CooldownReduction",
                                                               "NonbootsMovement", "SpellBlock"
                                                           },
                                                   Id = 3137
                                               };

        public static Item Diet_Poro_Snax = new Item
                                                {
                                                    Name = "Diet Poro-Snax", Group = "RelicBase", Consumed = true,
                                                    Stacks = 1, Depth = 1, InStore = false, Id = 2054
                                                };

        public static Item Dorans_Blade = new Item
                                              {
                                                  Name = "Doran's Blade", GoldBase = 450, GoldPrice = 450, GoldSell = 180,
                                                  Purchasable = true, Stacks = 1, Depth = 1, InStore = true,
                                                  FlatHPPoolMod = 80f, FlatPhysicalDamageMod = 8f,
                                                  PercentLifeStealMod = 0.03f,
                                                  Tags = new[] { "Lane", "Health", "Damage", "LifeSteal" }, Id = 1055
                                              };

        public static Item Dorans_Blade_Showdown = new Item
                                                       {
                                                           Name = "Doran's Blade (Showdown)", GoldBase = 450,
                                                           GoldPrice = 450, GoldSell = 180, Purchasable = true,
                                                           Group = "DoransShowdown", Stacks = 1, Depth = 1, InStore = true,
                                                           FlatHPPoolMod = 80f, FlatPhysicalDamageMod = 8f,
                                                           PercentLifeStealMod = 0.03f,
                                                           Tags = new[] { "Health", "Damage", "LifeSteal" }, Id = 1075
                                                       };

        public static Item Dorans_Ring = new Item
                                             {
                                                 Name = "Doran's Ring", GoldBase = 400, GoldPrice = 400, GoldSell = 160,
                                                 Purchasable = true, Stacks = 1, Depth = 1, InStore = true,
                                                 FlatHPPoolMod = 60f, FlatMPRegenMod = 0.5f, FlatMagicDamageMod = 15f,
                                                 Tags = new[] { "Lane", "Health", "ManaRegen", "SpellDamage" }, Id = 1056
                                             };

        public static Item Dorans_Ring_Showdown = new Item
                                                      {
                                                          Name = "Doran's Ring (Showdown)", GoldBase = 400,
                                                          GoldPrice = 400, GoldSell = 160, Purchasable = true,
                                                          Group = "DoransShowdown", Stacks = 1, Depth = 1, InStore = true,
                                                          FlatHPPoolMod = 60f, FlatMPRegenMod = 0.5f,
                                                          FlatMagicDamageMod = 15f,
                                                          Tags = new[] { "Health", "ManaRegen", "SpellDamage" }, Id = 1076
                                                      };

        public static Item Dorans_Shield = new Item
                                               {
                                                   Name = "Doran's Shield", GoldBase = 450, GoldPrice = 450,
                                                   GoldSell = 180, Purchasable = true, Stacks = 1, Depth = 1,
                                                   InStore = true, FlatHPPoolMod = 80f, FlatBlockMod = 8f,
                                                   Tags = new[] { "Health", "HealthRegen", "Lane" }, Id = 1054
                                               };

        public static Item Dorans_Shield_Showdown = new Item
                                                        {
                                                            Name = "Doran's Shield (Showdown)", GoldBase = 450,
                                                            GoldPrice = 450, GoldSell = 180, Purchasable = true,
                                                            Group = "DoransShowdown", Stacks = 1, Depth = 1,
                                                            InStore = true, FlatHPPoolMod = 80f, FlatBlockMod = 8f,
                                                            Tags = new[] { "Health", "HealthRegen" }, Id = 1074
                                                        };

        public static Item Elixir_of_Iron = new Item
                                                {
                                                    Name = "Elixir of Iron", GoldBase = 400, GoldPrice = 400,
                                                    GoldSell = 160, Purchasable = true, Group = "Flasks", Consumed = true,
                                                    Stacks = 1, Depth = 1, ConsumeOnFull = true, InStore = true,
                                                    Tags = new[] { "Consumable", "NonbootsMovement", "Tenacity" },
                                                    Id = 2138
                                                };

        public static Item Elixir_of_Sorcery = new Item
                                                   {
                                                       Name = "Elixir of Sorcery", GoldBase = 400, GoldPrice = 400,
                                                       GoldSell = 160, Purchasable = true, Group = "Flasks",
                                                       Consumed = true, Stacks = 1, Depth = 1, ConsumeOnFull = true,
                                                       InStore = true,
                                                       Tags = new[] { "Consumable", "ManaRegen", "SpellDamage" },
                                                       Id = 2139
                                                   };

        public static Item Elixir_of_Wrath = new Item
                                                 {
                                                     Name = "Elixir of Wrath", GoldBase = 400, GoldPrice = 400,
                                                     GoldSell = 160, Purchasable = true, Group = "Flasks", Consumed = true,
                                                     Stacks = 1, Depth = 1, ConsumeOnFull = true, InStore = true,
                                                     Tags = new[] { "Consumable", "Damage", "LifeSteal", "SpellVamp" },
                                                     Id = 2140
                                                 };

        public static Item Entropy = new Item
                                         {
                                             Name = "Entropy", GoldBase = 500, GoldPrice = 2700, GoldSell = 1890,
                                             Purchasable = true, Stacks = 1, Depth = 3, From = new[] { 3044, 1037 },
                                             InStore = true, FlatHPPoolMod = 275f, FlatPhysicalDamageMod = 55f,
                                             Tags = new[] { "Active", "Damage", "Health", "NonbootsMovement", "Slow" },
                                             Id = 3184
                                         };

        public static Item Essence_Reaver = new Item
                                                {
                                                    Name = "Essence Reaver", GoldBase = 400, GoldPrice = 3600,
                                                    GoldSell = 2520, Purchasable = true, Stacks = 1, Depth = 3,
                                                    From = new[] { 1038, 3133, 1018 }, InStore = true,
                                                    FlatPhysicalDamageMod = 65f,
                                                    Tags =
                                                        new[]
                                                            {
                                                                "CooldownReduction", "Damage", "CriticalStrike", "ManaRegen"
                                                            },
                                                    Id = 3508
                                                };

        public static Item Executioners_Calling = new Item
                                                      {
                                                          Name = "Executioner's Calling", GoldBase = 345, GoldPrice = 345,
                                                          GoldSell = 138, Purchasable = true, Stacks = 1, Depth = 1,
                                                          InStore = true, Id = 3123
                                                      };

        public static Item Explorers_Ward = new Item
                                                {
                                                    Name = "Explorer's Ward", Stacks = 1, Depth = 1, InStore = false,
                                                    Tags = new[] { "Consumable" }, Id = 2050
                                                };

        public static Item Eye_of_the_Equinox = new Item
                                                    {
                                                        Name = "Eye of the Equinox", Range = 850f, GoldBase = 550,
                                                        GoldPrice = 2200, GoldSell = 880, Purchasable = true, Stacks = 1,
                                                        Depth = 3, From = new[] { 3094, 2049 }, InStore = true,
                                                        FlatHPPoolMod = 500f, FlatHPRegenMod = 1f, rFlatGoldPer10Mod = 2f,
                                                        Tags =
                                                            new[]
                                                                { "Active", "Vision", "Health", "HealthRegen", "GoldPer" },
                                                        Id = 2303
                                                    };

        public static Item Eye_of_the_Oasis = new Item
                                                  {
                                                      Name = "Eye of the Oasis", GoldBase = 550, GoldPrice = 2200,
                                                      GoldSell = 880, Purchasable = true, Stacks = 1, Depth = 3,
                                                      From = new[] { 3096, 2049 }, InStore = true, FlatHPPoolMod = 200f,
                                                      FlatMPRegenMod = 1f, FlatHPRegenMod = 1.5f,
                                                      Tags =
                                                          new[]
                                                              {
                                                                  "Active", "Vision", "Health", "ManaRegen", "HealthRegen",
                                                                  "CooldownReduction"
                                                              },
                                                      Id = 2302
                                                  };

        public static Item Eye_of_the_Watchers = new Item
                                                     {
                                                         Name = "Eye of the Watchers", GoldBase = 550, GoldPrice = 2200,
                                                         GoldSell = 880, Purchasable = true, Stacks = 1, Depth = 3,
                                                         From = new[] { 3098, 2049 }, InStore = true, FlatHPPoolMod = 200f,
                                                         FlatMPRegenMod = 1f, FlatMagicDamageMod = 25f,
                                                         rFlatGoldPer10Mod = 2f,
                                                         Tags =
                                                             new[]
                                                                 {
                                                                     "Active", "Vision", "Health", "ManaRegen",
                                                                     "SpellDamage", "CooldownReduction", "GoldPer"
                                                                 },
                                                         Id = 2301
                                                     };

        public static Item Face_of_the_Mountain = new Item
                                                      {
                                                          Name = "Face of the Mountain", Range = 750f, GoldBase = 485,
                                                          GoldPrice = 2200, GoldSell = 880, Purchasable = true,
                                                          Group = "GoldBase", Stacks = 1, Depth = 3,
                                                          From = new[] { 3097, 3067 }, InStore = true,
                                                          FlatHPPoolMod = 500f,
                                                          Tags =
                                                              new[]
                                                                  {
                                                                      "Active", "CooldownReduction", "GoldPer", "Health",
                                                                      "HealthRegen"
                                                                  },
                                                          Id = 3401
                                                      };

        public static Item Faerie_Charm = new Item
                                              {
                                                  Name = "Faerie Charm", GoldBase = 180, GoldPrice = 180, GoldSell = 126,
                                                  Purchasable = true, Stacks = 1, Depth = 1,
                                                  Into = new[] { 3028, 3070, 3073, 3114 }, InStore = true,
                                                  FlatMPRegenMod = 0.25f, Tags = new[] { "ManaRegen" }, Id = 1004
                                              };

        public static Item Farsight_Alteration = new Item
                                                     {
                                                         Name = "Farsight Alteration", Range = 4000f, GoldBase = 250,
                                                         GoldPrice = 250, GoldSell = 175, Purchasable = true,
                                                         Group = "RelicBase", Stacks = 1, Depth = 2, From = new[] { 3342 },
                                                         InStore = true, Tags = new[] { "Active", "Trinket", "Vision" },
                                                         Id = 3363
                                                     };

        public static Item Fiendish_Codex = new Item
                                                {
                                                    Name = "Fiendish Codex", GoldBase = 385, GoldPrice = 820,
                                                    GoldSell = 574, Purchasable = true, Stacks = 1, Depth = 2,
                                                    From = new[] { 1052 },
                                                    Into =
                                                        new[]
                                                            {
                                                                3174, 3092, 3115, 3290, 3165, 3152, 3060, 3023, 3708, 3716,
                                                                3720, 3724
                                                            },
                                                    InStore = true, FlatMagicDamageMod = 30f,
                                                    Tags = new[] { "CooldownReduction", "SpellDamage" }, Id = 3108
                                                };

        public static Item Forbidden_Idol = new Item
                                                {
                                                    Name = "Forbidden Idol", GoldBase = 240, GoldPrice = 600,
                                                    GoldSell = 420, Purchasable = true, Stacks = 1, Depth = 2,
                                                    From = new[] { 1004, 1004 }, Into = new[] { 3069, 3165, 3222, 3504 },
                                                    InStore = true, Tags = new[] { "CooldownReduction", "ManaRegen" },
                                                    Id = 3114
                                                };

        public static Item Frost_Queens_Claim = new Item
                                                    {
                                                        Name = "Frost Queen's Claim", Range = 850f, GoldBase = 515,
                                                        GoldPrice = 2200, GoldSell = 880, Purchasable = true,
                                                        Group = "GoldBase", Stacks = 1, Depth = 3,
                                                        From = new[] { 3098, 3108 }, InStore = true,
                                                        FlatMagicDamageMod = 50f,
                                                        Tags =
                                                            new[]
                                                                {
                                                                    "Active", "CooldownReduction", "GoldPer", "ManaRegen",
                                                                    "Slow", "SpellDamage"
                                                                },
                                                        Id = 3092
                                                    };

        public static Item Frostfang = new Item
                                           {
                                               Name = "Frostfang", GoldBase = 500, GoldPrice = 865, GoldSell = 346,
                                               Purchasable = true, Group = "GoldBase", Stacks = 1, Depth = 2,
                                               From = new[] { 3303 }, Into = new[] { 3092 }, InStore = true,
                                               FlatMagicDamageMod = 10f,
                                               Tags = new[] { "GoldPer", "ManaRegen", "SpellDamage" }, Id = 3098
                                           };

        public static Item Frozen_Heart = new Item
                                              {
                                                  Name = "Frozen Heart", GoldBase = 700, GoldPrice = 2800, GoldSell = 1960,
                                                  Purchasable = true, Stacks = 1, Depth = 3, From = new[] { 3082, 3024 },
                                                  InStore = true, FlatMPPoolMod = 400f, FlatArmorMod = 90f,
                                                  Tags = new[] { "Armor", "Aura", "CooldownReduction", "Mana" }, Id = 3110
                                              };

        public static Item Frozen_Mallet = new Item
                                               {
                                                   Name = "Frozen Mallet", GoldBase = 625, GoldPrice = 3100,
                                                   GoldSell = 2170, Purchasable = true, Stacks = 1, Depth = 3,
                                                   From = new[] { 1028, 3052, 1037 }, InStore = true, FlatHPPoolMod = 650f,
                                                   FlatPhysicalDamageMod = 40f,
                                                   Tags = new[] { "Damage", "Health", "OnHit", "Slow" }, Id = 3022
                                               };

        public static Item Giant_Slayer = new Item
                                              {
                                                  Name = "Giant Slayer", GoldBase = 345, GoldPrice = 345, GoldSell = 138,
                                                  Purchasable = true, Stacks = 1, Depth = 1, InStore = true, Id = 3034
                                              };

        public static Item Giants_Belt = new Item
                                             {
                                                 Name = "Giant's Belt", GoldBase = 600, GoldPrice = 1000, GoldSell = 700,
                                                 Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1028 },
                                                 Into = new[] { 3083, 3143, 3116, 3022, 3084 }, InStore = true,
                                                 FlatHPPoolMod = 380f, Tags = new[] { "Health" }, Id = 1011
                                             };

        public static Item Glacial_Shroud = new Item
                                                {
                                                    Name = "Glacial Shroud", GoldBase = 350, GoldPrice = 1000,
                                                    GoldSell = 700, Purchasable = true, Stacks = 1, Depth = 2,
                                                    From = new[] { 1027, 1029 }, Into = new[] { 3050, 3110, 3025, 3187 },
                                                    InStore = true, FlatMPPoolMod = 250f, FlatArmorMod = 20f,
                                                    Tags = new[] { "Armor", "CooldownReduction", "Mana" }, Id = 3024
                                                };

        public static Item Golden_Transcendence = new Item
                                                      {
                                                          Name = "Golden Transcendence", Group = "RelicBase", Stacks = 1,
                                                          Depth = 1, InStore = false, Tags = new[] { "Active", "Trinket" },
                                                          Id = 3460
                                                      };

        public static Item Greater_Stealth_Totem_Trinket = new Item
                                                               {
                                                                   Name = "Greater Stealth Totem (Trinket)", Range = 600f,
                                                                   GoldBase = 250, GoldPrice = 250, GoldSell = 175,
                                                                   Purchasable = false, Group = "RelicBase", Stacks = 1,
                                                                   Depth = 2, From = new[] { 3340 }, InStore = true,
                                                                   Tags = new[] { "Active", "Trinket", "Vision" },
                                                                   Id = 3361
                                                               };

        public static Item Greater_Vision_Totem_Trinket = new Item
                                                              {
                                                                  Name = "Greater Vision Totem (Trinket)", Range = 600f,
                                                                  GoldBase = 250, GoldPrice = 250, GoldSell = 175,
                                                                  Purchasable = true, Group = "RelicBase", Stacks = 1,
                                                                  Depth = 2, From = new[] { 3340 }, InStore = true,
                                                                  Tags = new[] { "Active", "Trinket", "Vision" },
                                                                  Id = 3362
                                                              };

        public static Item Grezs_Spectral_Lantern = new Item
                                                        {
                                                            Name = "Grez's Spectral Lantern", GoldBase = 180,
                                                            GoldPrice = 1740, GoldSell = 696, Purchasable = true,
                                                            Group = "GoldBase", Stacks = 1, Depth = 3,
                                                            From = new[] { 3106, 1036, 1042 }, InStore = true,
                                                            FlatPhysicalDamageMod = 15f, PercentAttackSpeedMod = 0.3f,
                                                            Tags =
                                                                new[]
                                                                    {
                                                                        "Active", "AttackSpeed", "Damage", "GoldPer",
                                                                        "OnHit", "Stealth", "Vision"
                                                                    },
                                                            Id = 3159
                                                        };

        public static Item Guardian_Angel = new Item
                                                {
                                                    Name = "Guardian Angel", GoldBase = 1200, GoldPrice = 2800,
                                                    GoldSell = 1120, Purchasable = true, Stacks = 1, Depth = 3,
                                                    From = new[] { 1057, 1031 }, InStore = true, FlatArmorMod = 50f,
                                                    FlatSpellBlockMod = 50f, Tags = new[] { "Armor", "SpellBlock" },
                                                    Id = 3026
                                                };

        public static Item Guardians_Horn = new Item
                                                {
                                                    Name = "Guardian's Horn", GoldBase = 435, GoldPrice = 1015,
                                                    GoldSell = 711, Purchasable = true, Stacks = 1, Depth = 2,
                                                    From = new[] { 1006, 1028 }, InStore = true, FlatHPPoolMod = 200f,
                                                    FlatHPRegenMod = 1.25f,
                                                    Tags =
                                                        new[]
                                                            {
                                                                "Active", "Health", "HealthRegen", "NonbootsMovement",
                                                                "Armor", "SpellBlock"
                                                            },
                                                    Id = 2051
                                                };

        public static Item Guinsoos_Rageblade = new Item
                                                    {
                                                        Name = "Guinsoo's Rageblade", GoldBase = 1075, GoldPrice = 2800,
                                                        GoldSell = 1960, Purchasable = true, Stacks = 1, Depth = 2,
                                                        From = new[] { 1026, 1037 }, InStore = true,
                                                        FlatPhysicalDamageMod = 30f, FlatMagicDamageMod = 40f,
                                                        Tags = new[] { "AttackSpeed", "Damage", "OnHit", "SpellDamage" },
                                                        Id = 3124
                                                    };

        public static Item Haunting_Guise = new Item
                                                {
                                                    Name = "Haunting Guise", GoldBase = 665, GoldPrice = 1500,
                                                    GoldSell = 1050, Purchasable = true, Stacks = 1, Depth = 2,
                                                    From = new[] { 1028, 1052 }, Into = new[] { 3151 }, InStore = true,
                                                    FlatHPPoolMod = 200f, FlatMagicDamageMod = 25f,
                                                    Tags = new[] { "Health", "MagicPenetration", "SpellDamage" },
                                                    Id = 3136
                                                };

        public static Item Health_Potion = new Item
                                               {
                                                   Name = "Health Potion", GoldBase = 35, GoldPrice = 35, GoldSell = 14,
                                                   Purchasable = true, Group = "HealthPotion", Consumed = true, Stacks = 5,
                                                   Depth = 1, InStore = true,
                                                   Tags = new[] { "Consumable", "Jungle", "Lane" }, Id = 2003
                                               };

        public static Item Hexdrinker = new Item
                                            {
                                                Name = "Hexdrinker", GoldBase = 590, GoldPrice = 1450, GoldSell = 1015,
                                                Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1036, 1033 },
                                                Into = new[] { 3156 }, InStore = true, FlatPhysicalDamageMod = 25f,
                                                FlatSpellBlockMod = 30f, Tags = new[] { "Damage", "SpellBlock" },
                                                Id = 3155
                                            };

        public static Item Hextech_Gunblade = new Item
                                                  {
                                                      Name = "Hextech Gunblade", Range = 700f, GoldBase = 800,
                                                      GoldPrice = 3400, GoldSell = 2380, Purchasable = true, Stacks = 1,
                                                      Depth = 4, From = new[] { 3144, 3145 }, InStore = true,
                                                      FlatPhysicalDamageMod = 40f, FlatMagicDamageMod = 80f,
                                                      PercentLifeStealMod = 0.1f,
                                                      Tags =
                                                          new[]
                                                              {
                                                                  "Active", "Damage", "LifeSteal", "Slow", "SpellDamage",
                                                                  "SpellVamp"
                                                              },
                                                      Id = 3146
                                                  };

        public static Item Hextech_Revolver = new Item
                                                  {
                                                      Name = "Hextech Revolver", GoldBase = 330, GoldPrice = 1200,
                                                      GoldSell = 840, Purchasable = true, Stacks = 1, Depth = 2,
                                                      From = new[] { 1052, 1052 }, Into = new[] { 3146, 3152 },
                                                      InStore = true, FlatMagicDamageMod = 40f,
                                                      Tags = new[] { "SpellDamage", "SpellVamp" }, Id = 3145
                                                  };

        public static Item Hextech_Sweeper = new Item
                                                 {
                                                     Name = "Hextech Sweeper", GoldBase = 330, GoldPrice = 2130,
                                                     GoldSell = 1491, Purchasable = true, Stacks = 1, Depth = 3,
                                                     From = new[] { 3024, 3067 }, InStore = true, FlatHPPoolMod = 225f,
                                                     FlatMPPoolMod = 250f, FlatArmorMod = 25f,
                                                     Tags =
                                                         new[]
                                                             {
                                                                 "Active", "Armor", "CooldownReduction", "Health", "Mana",
                                                                 "Stealth", "Vision"
                                                             },
                                                     Id = 3187
                                                 };

        public static Item Hunters_Machete = new Item
                                                 {
                                                     Name = "Hunter's Machete", GoldBase = 350, GoldPrice = 350,
                                                     GoldSell = 245, Purchasable = true, Group = "JungleItems", Stacks = 1,
                                                     Depth = 1, Into = new[] { 3706, 3711, 3715 }, InStore = true,
                                                     FlatEXPBonus = 15f, PercentLifeStealMod = 0.08f,
                                                     Tags =
                                                         new[] { "Jungle", "Damage", "HealthRegen", "LifeSteal", "OnHit" },
                                                     Id = 1041
                                                 };

        public static Item Hunters_Potion = new Item
                                                {
                                                    Name = "Hunter's Potion", GoldBase = 250, GoldPrice = 400,
                                                    GoldSell = 160, Purchasable = true, Stacks = 1, Depth = 1,
                                                    InStore = true,
                                                    Tags =
                                                        new[]
                                                            { "Active", "Consumable", "HealthRegen", "Lane", "ManaRegen" },
                                                    Id = 2032
                                                };

        public static Item Hunters_Talisman = new Item
                                                  {
                                                      Name = "Hunter's Talisman", GoldBase = 350, GoldPrice = 350,
                                                      GoldSell = 245, Purchasable = true, Group = "JungleItems",
                                                      Stacks = 1, Depth = 1, Into = new[] { 3706, 3711, 3715 },
                                                      InStore = true, FlatEXPBonus = 15f, FlatMPRegenMod = 1.8f,
                                                      Tags =
                                                          new[]
                                                              {
                                                                  "Jungle", "SpellDamage", "HealthRegen", "ManaRegen",
                                                                  "OnHit"
                                                              },
                                                      Id = 1039
                                                  };

        public static Item Iceborn_Gauntlet = new Item
                                                  {
                                                      Name = "Iceborn Gauntlet", GoldBase = 750, GoldPrice = 2900,
                                                      GoldSell = 2030, Purchasable = true, Stacks = 1, Depth = 3,
                                                      From = new[] { 3057, 3024 }, InStore = true, FlatMPPoolMod = 500f,
                                                      FlatArmorMod = 60f, FlatMagicDamageMod = 30f,
                                                      Tags =
                                                          new[]
                                                              {
                                                                  "Armor", "CooldownReduction", "Mana", "Slow",
                                                                  "SpellDamage"
                                                              },
                                                      Id = 3025
                                                  };

        public static Item Infinity_Edge = new Item
                                               {
                                                   Name = "Infinity Edge", GoldBase = 625, GoldPrice = 3600,
                                                   GoldSell = 2520, Purchasable = true, Stacks = 1, Depth = 2,
                                                   From = new[] { 1038, 1037, 1018 }, InStore = true,
                                                   FlatPhysicalDamageMod = 65f, FlatCritChanceMod = 0.2f,
                                                   Tags = new[] { "CriticalStrike", "Damage" }, Id = 3031
                                               };

        public static Item Ionian_Boots_of_Lucidity = new Item
                                                          {
                                                              Name = "Ionian Boots of Lucidity", GoldBase = 500,
                                                              GoldPrice = 800, GoldSell = 560, Purchasable = true,
                                                              Stacks = 1, Depth = 2, From = new[] { 1001 },
                                                              Into = new[] { 3279, 3278, 3277, 3276, 3275 },
                                                              InStore = true, FlatMovementSpeedMod = 45f,
                                                              Tags = new[] { "Boots", "CooldownReduction" }, Id = 3158
                                                          };

        public static Item Jaurims_Fist = new Item
                                              {
                                                  Name = "Jaurim's Fist", GoldBase = 345, GoldPrice = 345, GoldSell = 138,
                                                  Purchasable = true, Stacks = 1, Depth = 1, InStore = true, Id = 3052
                                              };

        public static Item Kindlegem = new Item
                                           {
                                               Name = "Kindlegem", GoldBase = 450, GoldPrice = 850, GoldSell = 595,
                                               Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1028 },
                                               Into = new[] { 3187, 3190, 3401, 3065, 3050, 3056 }, InStore = true,
                                               FlatHPPoolMod = 200f, Tags = new[] { "CooldownReduction", "Health" },
                                               Id = 3067
                                           };

        public static Item Kircheis_Shard = new Item
                                                {
                                                    Name = "Kircheis_Shard", GoldBase = 450, GoldPrice = 750,
                                                    GoldSell = 525, Purchasable = true, Group = "S6Items",
                                                    Consumed = false, Stacks = 1, Depth = 1, From = new[] { 1042, },
                                                    Into = new[] { 3094, 3087 }, InStore = true,
                                                    PercentAttackSpeedMod = 0.15f, Tags = new[] { "AttackSpeed", "OnHit" },
                                                    Id = 2015
                                                };

        public static Item Last_Whisper = new Item
                                              {
                                                  Name = "Last Whisper", GoldBase = 1065, GoldPrice = 2300,
                                                  GoldSell = 1610, Purchasable = true, Stacks = 1, Depth = 2,
                                                  From = new[] { 1037, 1036 }, InStore = true, FlatPhysicalDamageMod = 40f,
                                                  Tags = new[] { "ArmorPenetration", "Damage" }, Id = 3035
                                              };

        public static Item Liandrys_Torment = new Item
                                                  {
                                                      Name = "Liandry's Torment", GoldBase = 650, GoldPrice = 3000,
                                                      GoldSell = 2100, Purchasable = true, Stacks = 1, Depth = 3,
                                                      From = new[] { 3136, 1026 }, InStore = true, FlatHPPoolMod = 300f,
                                                      FlatMagicDamageMod = 80f,
                                                      Tags = new[] { "Health", "MagicPenetration", "SpellDamage" },
                                                      Id = 3151
                                                  };

        public static Item Lich_Bane = new Item
                                           {
                                               Name = "Lich Bane", GoldBase = 950, GoldPrice = 3000, GoldSell = 2100,
                                               Purchasable = true, Stacks = 1, Depth = 3, From = new[] { 3057, 3113 },
                                               InStore = true, FlatMPPoolMod = 250f, FlatMagicDamageMod = 80f,
                                               PercentMovementSpeedMod = 0.05f,
                                               Tags = new[] { "Mana", "NonbootsMovement", "SpellDamage" }, Id = 3100
                                           };

        public static Item Locket_of_the_Iron_Solari = new Item
                                                           {
                                                               Name = "Locket of the Iron Solari", Range = 600f,
                                                               GoldBase = 50, GoldPrice = 2800, GoldSell = 1960,
                                                               Purchasable = true, Stacks = 1, Depth = 3,
                                                               From = new[] { 3105, 3067 }, InStore = true,
                                                               FlatHPPoolMod = 400f, FlatSpellBlockMod = 20f,
                                                               Tags =
                                                                   new[]
                                                                       {
                                                                           "Active", "Aura", "CooldownReduction", "Health",
                                                                           "HealthRegen", "SpellBlock"
                                                                       },
                                                               Id = 3190
                                                           };

        public static Item Long_Sword = new Item
                                            {
                                                Name = "Long Sword", GoldBase = 350, GoldPrice = 350, GoldSell = 245,
                                                Purchasable = true, Stacks = 1, Depth = 1,
                                                Into =
                                                    new[]
                                                        {
                                                            1053, 3044, 3134, 3155, 3077, 3035, 3154, 3141, 3144, 3122, 3159
                                                        },
                                                InStore = true, FlatPhysicalDamageMod = 10f,
                                                Tags = new[] { "Damage" }, Id = 1036
                                            };

        public static Item Lord_Dominiks_Regards = new Item
                                                       {
                                                           Name = "Lord Dominik's Regards", GoldBase = 345,
                                                           GoldPrice = 345, GoldSell = 138, Purchasable = true, Stacks = 1,
                                                           Depth = 1, InStore = true, Id = 3306
                                                       };

        public static Item Lord_Van_Damms_Pillager = new Item
                                                         {
                                                             Name = "Lord Van Damm's Pillager", GoldBase = 995,
                                                             GoldPrice = 3800, GoldSell = 2660, Purchasable = true,
                                                             Stacks = 1, Depth = 3, From = new[] { 3122, 1037, 1018 },
                                                             InStore = true, FlatPhysicalDamageMod = 80f,
                                                             FlatCritChanceMod = 0.25f,
                                                             Tags = new[] { "CriticalStrike", "Damage" }, Id = 3104
                                                         };

        public static Item Ludens_Echo = new Item
                                             {
                                                 Name = "Luden's Echo", GoldBase = 650, GoldPrice = 3000, GoldSell = 2100,
                                                 Purchasable = true, Stacks = 1, Depth = 3, From = new[] { 1058, 3113 },
                                                 InStore = true, FlatMagicDamageMod = 100f, PercentMovementSpeedMod = 0.1f,
                                                 Tags = new[] { "NonbootsMovement", "OnHit", "SpellDamage" }, Id = 3285
                                             };

        public static Item Madreds_Razors = new Item
                                                {
                                                    Name = "Madred's Razors", GoldBase = 300, GoldPrice = 750,
                                                    GoldSell = 525, Purchasable = true, Stacks = 1, Depth = 2,
                                                    From = new[] { 1042 }, Into = new[] { 3154, 3159 }, InStore = true,
                                                    PercentAttackSpeedMod = 0.15f, Tags = new[] { "AttackSpeed", "OnHit" },
                                                    Id = 3106
                                                };

        public static Item Manamune = new Item
                                          {
                                              Name = "Manamune", GoldBase = 775, GoldPrice = 2400, GoldSell = 1680,
                                              Purchasable = true, Stacks = 1, Depth = 3, From = new[] { 3070, 1037 },
                                              Into = new[] { 3042 }, InStore = true, FlatMPPoolMod = 250f,
                                              FlatMPRegenMod = 0.25f, FlatPhysicalDamageMod = 25f,
                                              Tags = new[] { "Damage", "Mana", "ManaRegen", "OnHit" }, Id = 3004
                                          };

        public static Item Manamune_Crystal_Scar = new Item
                                                       {
                                                           Name = "Manamune (Crystal Scar)", GoldBase = 775,
                                                           GoldPrice = 2400, GoldSell = 1680, Purchasable = true,
                                                           Stacks = 1, Depth = 3, From = new[] { 3070, 1037 },
                                                           Into = new[] { 3042 }, InStore = true, FlatMPPoolMod = 250f,
                                                           FlatMPRegenMod = 0.25f, FlatPhysicalDamageMod = 25f,
                                                           Tags = new[] { "Damage", "Mana", "ManaRegen", "OnHit" },
                                                           Id = 3008
                                                       };

        public static Item Maw_of_Malmortius = new Item
                                                   {
                                                       Name = "Maw of Malmortius", GoldBase = 875, GoldPrice = 3200,
                                                       GoldSell = 2240, Purchasable = true, Stacks = 1, Depth = 3,
                                                       From = new[] { 3155, 1037 }, InStore = true,
                                                       FlatPhysicalDamageMod = 60f, FlatSpellBlockMod = 40f,
                                                       Tags = new[] { "Damage", "SpellBlock" }, Id = 3156
                                                   };

        public static Item Mejais_Soulstealer = new Item
                                                    {
                                                        Name = "Mejai's Soulstealer", GoldBase = 965, GoldPrice = 1400,
                                                        GoldSell = 980, Purchasable = true, Stacks = 1, Depth = 2,
                                                        From = new[] { 1082 }, InStore = true, FlatMagicDamageMod = 20f,
                                                        Tags = new[] { "SpellDamage" }, Id = 3041
                                                    };

        public static Item Mercurial_Scimitar = new Item
                                                    {
                                                        Name = "Mercurial Scimitar", GoldBase = 900, GoldPrice = 3700,
                                                        GoldSell = 2590, Purchasable = true, Stacks = 1, Depth = 3,
                                                        From = new[] { 1038, 3140 }, InStore = true,
                                                        FlatPhysicalDamageMod = 80f, FlatSpellBlockMod = 35f,
                                                        Tags =
                                                            new[] { "Active", "Damage", "NonbootsMovement", "SpellBlock" },
                                                        Id = 3139
                                                    };

        public static Item Mercurys_Treads = new Item
                                                 {
                                                     Name = "Mercury's Treads", GoldBase = 350, GoldPrice = 1100,
                                                     GoldSell = 770, Purchasable = true, Stacks = 1, Depth = 2,
                                                     From = new[] { 1001, 1033 },
                                                     Into = new[] { 3269, 3268, 3267, 3266, 3265 }, InStore = true,
                                                     FlatMovementSpeedMod = 45f, FlatSpellBlockMod = 25f,
                                                     Tags = new[] { "Boots", "SpellBlock", "Tenacity" }, Id = 3111
                                                 };

        public static Item Mikaels_Crucible = new Item
                                                  {
                                                      Name = "Mikael's Crucible", GoldBase = 950, GoldPrice = 2450,
                                                      GoldSell = 1715, Purchasable = true, Stacks = 1, Depth = 3,
                                                      From = new[] { 3028, 3114 }, InStore = true, FlatSpellBlockMod = 40f,
                                                      Tags =
                                                          new[]
                                                              { "Active", "CooldownReduction", "ManaRegen", "SpellBlock" },
                                                      Id = 3222
                                                  };

        public static Item Moonflair_Spellblade = new Item
                                                      {
                                                          Name = "Moonflair Spellblade", GoldBase = 920, GoldPrice = 2620,
                                                          GoldSell = 1834, Purchasable = true, Stacks = 1, Depth = 3,
                                                          From = new[] { 3191, 1033 }, InStore = true, FlatArmorMod = 50f,
                                                          FlatMagicDamageMod = 50f, FlatSpellBlockMod = 50f,
                                                          Tags =
                                                              new[] { "Armor", "SpellBlock", "SpellDamage", "Tenacity" },
                                                          Id = 3170
                                                      };

        public static Item Morellonomicon = new Item
                                                {
                                                    Name = "Morellonomicon", GoldBase = 445, GoldPrice = 2300,
                                                    GoldSell = 1610, Purchasable = true, Stacks = 1, Depth = 3,
                                                    From = new[] { 3108, 3114, 1052 }, InStore = true,
                                                    FlatMagicDamageMod = 80f,
                                                    Tags = new[] { "CooldownReduction", "ManaRegen", "SpellDamage" },
                                                    Id = 3165
                                                };

        public static Item Mortal_Reminder = new Item
                                                 {
                                                     Name = "Mortal_Reminder", GoldBase = 345, GoldPrice = 345,
                                                     GoldSell = 138, Purchasable = true, Stacks = 1, Depth = 1,
                                                     InStore = true, Id = 3033
                                                 };

        public static Item Muramana = new Item
                                          {
                                              Name = "Muramana", GoldBase = 2200, GoldPrice = 2200, GoldSell = 3080,
                                              Stacks = 1, Depth = 4, From = new[] { 3004 }, SpecialRecipe = 3004,
                                              InStore = false, FlatMPPoolMod = 1000f, FlatPhysicalDamageMod = 25f,
                                              Tags = new[] { "OnHit" }, Id = 3042
                                          };

        public static Item Muramana2 = new Item
                                           {
                                               Name = "Muramana", GoldBase = 2200, GoldPrice = 2200, GoldSell = 3080,
                                               Stacks = 1, Depth = 4, From = new[] { 3008 }, SpecialRecipe = 3008,
                                               InStore = false, FlatMPPoolMod = 1000f, FlatPhysicalDamageMod = 25f,
                                               Tags = new[] { "OnHit" }, Id = 3043
                                           };

        public static Item Nashors_Tooth = new Item
                                               {
                                                   Name = "Nashor's Tooth", GoldBase = 930, GoldPrice = 3000,
                                                   GoldSell = 2100, Purchasable = true, Stacks = 1, Depth = 3,
                                                   From = new[] { 3101, 3108 }, InStore = true, FlatMagicDamageMod = 80f,
                                                   PercentAttackSpeedMod = 0.4f,
                                                   Tags =
                                                       new[]
                                                           { "AttackSpeed", "CooldownReduction", "OnHit", "SpellDamage" },
                                                   Id = 3115
                                               };

        public static Item Needlessly_Large_Rod = new Item
                                                      {
                                                          Name = "Needlessly Large Rod", GoldBase = 1250, GoldPrice = 1250,
                                                          GoldSell = 875, Purchasable = true, Stacks = 1, Depth = 1,
                                                          Into = new[] { 3089, 3157, 3003, 3007, 3090, 3116, 3285 },
                                                          InStore = true, FlatMagicDamageMod = 60f,
                                                          Tags = new[] { "SpellDamage" }, Id = 1058
                                                      };

        public static Item Negatron_Cloak = new Item
                                                {
                                                    Name = "Negatron Cloak", GoldBase = 270, GoldPrice = 720,
                                                    GoldSell = 504, Purchasable = true, Stacks = 1, Depth = 2,
                                                    From = new[] { 1033 },
                                                    Into = new[] { 3001, 3026, 3112, 3170, 3180, 3512 }, InStore = true,
                                                    FlatSpellBlockMod = 40f, Tags = new[] { "SpellBlock" }, Id = 1057
                                                };

        public static Item Ninja_Tabi = new Item
                                            {
                                                Name = "Ninja Tabi", GoldBase = 500, GoldPrice = 1100, GoldSell = 770,
                                                Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1001, 1029 },
                                                Into = new[] { 3264, 3263, 3262, 3261, 3260 }, InStore = true,
                                                FlatArmorMod = 25f, FlatMovementSpeedMod = 45f,
                                                Tags = new[] { "Armor", "Boots" }, Id = 3047
                                            };

        public static Item Nomads_Medallion = new Item
                                                  {
                                                      Name = "Nomad's Medallion", GoldBase = 500, GoldPrice = 865,
                                                      GoldSell = 346, Purchasable = true, Group = "GoldBase", Stacks = 1,
                                                      Depth = 2, From = new[] { 3301 }, Into = new[] { 3069 },
                                                      InStore = true,
                                                      Tags =
                                                          new[]
                                                              {
                                                                  "GoldPer", "HealthRegen", "ManaRegen", "NonbootsMovement"
                                                              },
                                                      Id = 3096
                                                  };

        public static Item Null_Magic_Mantle = new Item
                                                   {
                                                       Name = "Null-Magic Mantle", GoldBase = 450, GoldPrice = 450,
                                                       GoldSell = 315, Purchasable = true, Stacks = 1, Depth = 1,
                                                       Into =
                                                           new[]
                                                               {
                                                                   3111, 3211, 1057, 3028, 3112, 3140, 3155, 3105, 3091,
                                                                   3170, 3180
                                                               },
                                                       InStore = true, FlatSpellBlockMod = 25f,
                                                       Tags = new[] { "SpellBlock" }, Id = 1033
                                                   };

        public static Item Odyns_Veil = new Item
                                            {
                                                Name = "Odyn's Veil", Range = 525, GoldBase = 800, GoldPrice = 2500,
                                                GoldSell = 1750, Purchasable = true, Stacks = 1, Depth = 3,
                                                From = new[] { 1033, 3010 }, InStore = true, FlatHPPoolMod = 350f,
                                                FlatMPPoolMod = 350f, FlatSpellBlockMod = 50f,
                                                Tags = new[] { "Active", "Health", "Mana", "SpellBlock" }, Id = 3180
                                            };

        public static Item Ohmwrecker = new Item
                                            {
                                                Name = "Ohmwrecker", GoldBase = 750, GoldPrice = 2600, GoldSell = 1820,
                                                Purchasable = true, Stacks = 1, Depth = 3, From = new[] { 2053, 3067 },
                                                InStore = true, FlatHPPoolMod = 300f, FlatArmorMod = 50f,
                                                Tags =
                                                    new[]
                                                        {
                                                            "Active", "Armor", "CooldownReduction", "Health", "HealthRegen",
                                                            "NonbootsMovement"
                                                        },
                                                Id = 3056
                                            };

        public static Item Oracle_Alteration = new Item
                                                   {
                                                       Name = "Oracle Alteration", Range = 600f, GoldBase = 250,
                                                       GoldPrice = 250, GoldSell = 175, Purchasable = true,
                                                       Group = "RelicBase", Stacks = 1, Depth = 2, From = new[] { 3341 },
                                                       InStore = true, Tags = new[] { "Active", "Trinket", "Vision" },
                                                       Id = 3364
                                                   };

        public static Item Oracles_Extract = new Item
                                                 {
                                                     Name = "Oracle's Extract", GoldBase = 250, GoldPrice = 250,
                                                     GoldSell = 100, Purchasable = true, Consumed = true, Stacks = 1,
                                                     Depth = 1, ConsumeOnFull = true, InStore = true,
                                                     Tags = new[] { "Consumable", "Stealth", "Vision" }, Id = 2047
                                                 };

        public static Item Orb_of_Winter = new Item
                                               {
                                                   Name = "Orb of Winter", GoldBase = 990, GoldPrice = 2200,
                                                   GoldSell = 1540, Purchasable = true, Stacks = 1, Depth = 3,
                                                   From = new[] { 1006, 1006, 1057 }, InStore = true,
                                                   FlatSpellBlockMod = 70f, Tags = new[] { "HealthRegen", "SpellBlock" },
                                                   Id = 3112
                                               };

        public static Item Overlords_Bloodmail = new Item
                                                     {
                                                         Name = "Overlord's Bloodmail", GoldBase = 1055, GoldPrice = 2455,
                                                         GoldSell = 1719, Purchasable = true, Stacks = 1, Depth = 3,
                                                         From = new[] { 1011, 1028 }, InStore = true, FlatHPPoolMod = 850f,
                                                         Tags = new[] { "Health", "HealthRegen" }, Id = 3084
                                                     };

        public static Item Perfect_Hex_Core = new Item
                                                  {
                                                      Name = "Perfect Hex Core", GoldBase = 1000, GoldPrice = 3000,
                                                      GoldSell = 2100, Purchasable = true, Stacks = 1, Depth = 4,
                                                      From = new[] { 3197 }, InStore = true, RequiredChampion = "Viktor",
                                                      FlatMPPoolMod = 500f, FlatMagicDamageMod = 60f,
                                                      Tags = new[] { "Mana", "SpellDamage" }, Id = 3198
                                                  };

        public static Item Phage = new Item
                                       {
                                           Name = "Phage", GoldBase = 565, GoldPrice = 1325, GoldSell = 927,
                                           Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1028, 1036 },
                                           Into = new[] { 3078, 3184 }, InStore = true, FlatHPPoolMod = 200f,
                                           FlatPhysicalDamageMod = 20f,
                                           Tags = new[] { "Damage", "Health", "NonbootsMovement", "OnHit" }, Id = 3044
                                       };

        public static Item Phantom_Dancer = new Item
                                                {
                                                    Name = "Phantom Dancer", GoldBase = 900, GoldPrice = 2700,
                                                    GoldSell = 1890, Purchasable = true, Stacks = 1, Depth = 3,
                                                    From = new[] { 1018, 3086, 1042 }, InStore = true,
                                                    PercentMovementSpeedMod = 0.12f, PercentAttackSpeedMod = 0.45f,
                                                    FlatCritChanceMod = 0.30f,
                                                    Tags = new[] { "AttackSpeed", "CriticalStrike", "NonbootsMovement" },
                                                    Id = 3046
                                                };

        public static Item Pickaxe = new Item
                                         {
                                             Name = "Pickaxe", GoldBase = 875, GoldPrice = 875, GoldSell = 613,
                                             Purchasable = true, Stacks = 1, Depth = 1,
                                             Into =
                                                 new[]
                                                     {
                                                         3035, 3124, 3031, 3156, 3077, 3104, 3184, 3004, 3008, 3022, 3172,
                                                         3181
                                                     },
                                             InStore = true, FlatPhysicalDamageMod = 25f,
                                             Tags = new[] { "Damage" }, Id = 1037
                                         };

        public static Item Poro_Snax = new Item
                                           {
                                               Name = "Poro-Snax", Group = "RelicBase", Consumed = true, Stacks = 1,
                                               Depth = 1, InStore = false, Id = 2052
                                           };

        public static Item Prospectors_Blade = new Item
                                                   {
                                                       Name = "Prospector's Blade", GoldBase = 950, GoldPrice = 950,
                                                       GoldSell = 380, Purchasable = true, Stacks = 1, Depth = 1,
                                                       InStore = true, PercentAttackSpeedMod = 0.15f,
                                                       FlatPhysicalDamageMod = 16f, FlatHPPoolMod = 150f,
                                                       Tags = new[] { "AttackSpeed", "Damage", "Health" }, Id = 1062
                                                   };

        public static Item Prospectors_Ring = new Item
                                                  {
                                                      Name = "Prospector's Ring", GoldBase = 950, GoldPrice = 950,
                                                      GoldSell = 380, Purchasable = true, Stacks = 1, Depth = 1,
                                                      InStore = true, FlatMagicDamageMod = 35f, FlatMPRegenMod = 1.4f,
                                                      FlatHPPoolMod = 150f,
                                                      Tags = new[] { "SpellDamage", "ManaRegen", "Health" }, Id = 1063
                                                  };

        public static Item Prototype_Hex_Core = new Item
                                                    {
                                                        Name = "Prototype Hex Core", Stacks = 1, Depth = 1,
                                                        Into = new[] { 3196 }, SpecialRecipe = 1, InStore = false,
                                                        RequiredChampion = "Viktor", Id = 3200
                                                    };

        public static Item Quicksilver_Sash = new Item
                                                  {
                                                      Name = "Quicksilver Sash", GoldBase = 750, GoldPrice = 1250,
                                                      GoldSell = 875, Purchasable = true, Stacks = 1, Depth = 2,
                                                      From = new[] { 1033 }, Into = new[] { 3139, 3137 }, InStore = true,
                                                      FlatSpellBlockMod = 30f, Tags = new[] { "Active", "SpellBlock" },
                                                      Id = 3140
                                                  };

        public static Item Rabadons_Deathcap = new Item
                                                   {
                                                       Name = "Rabadon's Deathcap", GoldBase = 935, GoldPrice = 3500,
                                                       GoldSell = 1450, Purchasable = true, Stacks = 1, Depth = 2,
                                                       From = new[] { 1026, 1058, 1052 }, InStore = true,
                                                       FlatMagicDamageMod = 120f, Tags = new[] { "SpellDamage" },
                                                       Id = 3089
                                                   };

        public static Item Randuins_Omen = new Item
                                               {
                                                   Name = "Randuin's Omen", Range = 500f, GoldBase = 800, GoldPrice = 2850,
                                                   GoldSell = 1995, Purchasable = true, Stacks = 1, Depth = 3,
                                                   From = new[] { 3082, 1011 }, InStore = true, FlatHPPoolMod = 500f,
                                                   FlatArmorMod = 70f,
                                                   Tags = new[] { "Active", "Armor", "Health", "Slow" }, Id = 3143
                                               };

        public static Item Rapid_Firecannon = new Item
                                                  {
                                                      Name = "Rapid Firecannon", GoldBase = 550, GoldPrice = 2500,
                                                      GoldSell = 1750, Purchasable = true, Stacks = 1, Depth = 3,
                                                      From = new[] { 3086, 2015 }, Into = new int[] { }, InStore = true,
                                                      Tags =
                                                          new[]
                                                              {
                                                                  "AttackSpeed", "CriticalStrike", "NonbootsMovement",
                                                                  "OnHit"
                                                              },
                                                      PercentCritChanceMod = 0.3f,
                                                      PercentMovementSpeedMod = 0.08f, PercentAttackSpeedMod = 0.3f,
                                                      Id = 3094
                                                  };

        public static Item Raptor_Cloak = new Item
                                              {
                                                  Name = "Raptor Cloak", GoldBase = 520, GoldPrice = 1000, GoldSell = 700,
                                                  Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1006, 1029 },
                                                  Into = new[] { 3056, 3512 }, InStore = true, FlatArmorMod = 40f,
                                                  FlatHPRegenMod = 1.25f,
                                                  Tags = new[] { "Armor", "HealthRegen", "NonbootsMovement" }, Id = 2053
                                              };

        public static Item Ravenous_Hydra_Melee_Only = new Item
                                                           {
                                                               Name = "Ravenous Hydra (Melee Only)", Range = 400f,
                                                               GoldBase = 600, GoldPrice = 3300, GoldSell = 2310,
                                                               Purchasable = true, Stacks = 1, Depth = 3,
                                                               From = new[] { 3077, 1053 }, InStore = true,
                                                               FlatPhysicalDamageMod = 75f, PercentLifeStealMod = 0.12f,
                                                               Tags =
                                                                   new[]
                                                                       {
                                                                           "Active", "Damage", "HealthRegen", "LifeSteal",
                                                                           "OnHit"
                                                                       },
                                                               Id = 3074
                                                           };

        public static Item Recurve_Bow = new Item
                                             {
                                                 Name = "Recurve Bow", GoldBase = 400, GoldPrice = 1000, GoldSell = 700,
                                                 Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1042, 1042 },
                                                 Into = new[] { 3091, 3153, 3085, 3718, 3722, 1403, 1411, 1415, 3674 },
                                                 InStore = true, PercentAttackSpeedMod = 0.25f,
                                                 PercentPhysicalDamageMod = 15f,
                                                 Tags = new[] { "AttackSpeed", "Damage", "OnHit" }, Id = 1043
                                             };

        public static Item Refillable_Potion = new Item
                                                   {
                                                       Name = "Refillable Potion", GoldBase = 150, GoldPrice = 150,
                                                       GoldSell = 60, Purchasable = true, Stacks = 1, Depth = 1,
                                                       InStore = true,
                                                       Tags =
                                                           new[]
                                                               {
                                                                   "Active", "Consumable", "HealthRegen", "Lane",
                                                                   "ManaRegen"
                                                               },
                                                       Id = 2031
                                                   };

        public static Item Rejuvenation_Bead = new Item
                                                   {
                                                       Name = "Rejuvenation Bead", GoldBase = 150, GoldPrice = 150,
                                                       GoldSell = 105, Purchasable = true, Stacks = 1, Depth = 1,
                                                       Into = new[] { 3077, 3112, 2051, 2053, 3105, 3801 }, InStore = true,
                                                       FlatHPRegenMod = 0.5f, Tags = new[] { "HealthRegen" }, Id = 1006
                                                   };

        public static Item Relic_Shield = new Item
                                              {
                                                  Name = "Relic Shield", GoldBase = 365, GoldPrice = 365, GoldSell = 146,
                                                  Purchasable = true, Group = "GoldBase", Stacks = 1, Depth = 1,
                                                  Into = new[] { 3097 }, InStore = true, FlatHPPoolMod = 75f,
                                                  Tags = new[] { "Aura", "GoldPer", "Health", "Lane" }, Id = 3302
                                              };

        public static Item Righteous_Glory = new Item
                                                 {
                                                     Name = "Righteous Glory", GoldBase = 750, GoldPrice = 2600,
                                                     GoldSell = 1820, Purchasable = true, Stacks = 1, Depth = 3,
                                                     From = new[] { 3010, 3801 }, InStore = true, FlatHPPoolMod = 500f,
                                                     FlatMPPoolMod = 300f,
                                                     Tags =
                                                         new[]
                                                             {
                                                                 "Active", "Health", "HealthRegen", "Mana",
                                                                 "NonbootsMovement", "Slow"
                                                             },
                                                     Id = 3800
                                                 };

        public static Item Rod_of_Ages = new Item
                                             {
                                                 Name = "Rod of Ages", GoldBase = 740, GoldPrice = 2700, GoldSell = 1890,
                                                 Purchasable = true, Stacks = 1, Depth = 3, From = new[] { 3010, 1026 },
                                                 InStore = true, FlatHPPoolMod = 300f, FlatMPPoolMod = 400f,
                                                 FlatMagicDamageMod = 60f,
                                                 Tags =
                                                     new[] { "Health", "HealthRegen", "Mana", "ManaRegen", "SpellDamage" },
                                                 Id = 3027
                                             };

        public static Item Rod_of_Ages_Crystal_Scar = new Item
                                                          {
                                                              Name = "Rod of Ages (Crystal Scar)", GoldBase = 740,
                                                              GoldPrice = 2800, GoldSell = 1960, Purchasable = true,
                                                              Stacks = 1, Depth = 3, From = new[] { 3010, 1026 },
                                                              InStore = true, FlatHPPoolMod = 450f, FlatMPPoolMod = 450f,
                                                              FlatMagicDamageMod = 60f,
                                                              Tags =
                                                                  new[]
                                                                      {
                                                                          "Health", "HealthRegen", "Mana", "ManaRegen",
                                                                          "SpellDamage"
                                                                      },
                                                              Id = 3029
                                                          };

        public static Item Ruby_Crystal = new Item
                                              {
                                                  Name = "Ruby Crystal", GoldBase = 400, GoldPrice = 400, GoldSell = 280,
                                                  Purchasable = true, Stacks = 1, Depth = 1,
                                                  Into =
                                                      new[]
                                                          {
                                                              1011, 2049, 2045, 2051, 3010, 3022, 3044, 3067, 3105, 3211,
                                                              3751, 3071, 3801, 3084, 3102, 3136
                                                          },
                                                  InStore = true,
                                                  FlatHPPoolMod = 150f, Tags = new[] { "Health" }, Id = 1028
                                              };

        public static Item Ruby_Sightstone = new Item
                                                 {
                                                     Name = "Ruby Sightstone", GoldBase = 600, GoldPrice = 1800,
                                                     GoldSell = 720, Purchasable = true, Stacks = 1, Depth = 3,
                                                     From = new[] { 2049, 1028 }, InStore = true, FlatHPPoolMod = 500f,
                                                     Tags = new[] { "Active", "Health", "Vision" }, Id = 2045
                                                 };

        public static Item Runaans_Hurricane_Ranged_Only = new Item
                                                               {
                                                                   Name = "Runaan's Hurricane (Ranged Only)",
                                                                   GoldBase = 300, GoldPrice = 2600, GoldSell = 1750,
                                                                   Purchasable = true, Stacks = 1, Depth = 3,
                                                                   From = new[] { 3086, 1043 }, InStore = true,
                                                                   PercentAttackSpeedMod = 0.4f, FlatCritChanceMod = 0.3f,
                                                                   PercentMovementSpeedMod = 0.05f,
                                                                   Tags =
                                                                       new[]
                                                                           {
                                                                               "AttackSpeed", "OnHit", "CriticalStrike",
                                                                               "NonbootsMovement"
                                                                           },
                                                                   Id = 3085
                                                               };

        public static Item Rylais_Crystal_Scepter = new Item
                                                        {
                                                            Name = "Rylai's Crystal Scepter", GoldBase = 315,
                                                            GoldPrice = 3000, GoldSell = 2100, Purchasable = true,
                                                            Stacks = 1, Depth = 3, From = new[] { 1058, 1052, 1011 },
                                                            InStore = true, FlatHPPoolMod = 400f,
                                                            FlatMagicDamageMod = 100f,
                                                            Tags = new[] { "Health", "Slow", "SpellDamage" }, Id = 3116
                                                        };

        public static Item Sanguine_Blade = new Item
                                                {
                                                    Name = "Sanguine Blade", GoldBase = 600, GoldPrice = 2275,
                                                    GoldSell = 1593, Purchasable = true, Stacks = 1, Depth = 3,
                                                    From = new[] { 1037, 1053 }, InStore = true,
                                                    FlatPhysicalDamageMod = 45f, PercentLifeStealMod = 0.1f,
                                                    Tags = new[] { "Damage", "LifeSteal" }, Id = 3181
                                                };

        public static Item Sapphire_Crystal = new Item
                                                  {
                                                      Name = "Sapphire Crystal", GoldBase = 350, GoldPrice = 350,
                                                      GoldSell = 245, Purchasable = true, Stacks = 1, Depth = 1,
                                                      Into = new[] { 3057, 3070, 3073, 3010, 3024 }, InStore = true,
                                                      FlatMPPoolMod = 250f, Tags = new[] { "Mana" }, Id = 1027
                                                  };

        public static Item Seekers_Armguard = new Item
                                                  {
                                                      Name = "Seeker's Armguard", GoldBase = 465, GoldPrice = 1200,
                                                      GoldSell = 840, Purchasable = true, Stacks = 1, Depth = 2,
                                                      From = new[] { 1029, 1052 }, Into = new[] { 3090, 3157, 3170 },
                                                      InStore = true, FlatArmorMod = 30f, FlatMagicDamageMod = 25f,
                                                      Tags = new[] { "Armor", "SpellDamage" }, Id = 3191
                                                  };

        public static Item Seraphs_Embrace = new Item
                                                 {
                                                     Name = "Seraph's Embrace", GoldBase = 2700, GoldPrice = 2700,
                                                     GoldSell = 3780, Stacks = 1, Depth = 4, From = new[] { 3003 },
                                                     SpecialRecipe = 3003, InStore = false, FlatMPPoolMod = 1000f,
                                                     FlatMagicDamageMod = 80f, Tags = new[] { "Active" }, Id = 3040
                                                 };

        public static Item Seraphs_Embrace2 = new Item
                                                  {
                                                      Name = "Seraph's Embrace", GoldBase = 2700, GoldPrice = 2700,
                                                      GoldSell = 3780, Stacks = 1, Depth = 4, From = new[] { 3007 },
                                                      SpecialRecipe = 3007, InStore = false, FlatMPPoolMod = 1000f,
                                                      FlatMagicDamageMod = 60f, Tags = new[] { "Active" }, Id = 3048
                                                  };

        public static Item Serrated_Dirk = new Item
                                               {
                                                   Name = "Serrated Dirk", GoldBase = 617, GoldPrice = 1337,
                                                   GoldSell = 936, Purchasable = true, Stacks = 1, Depth = 2,
                                                   From = new int[] { }, Into = new int[] { }, InStore = true,
                                                   FlatPhysicalDamageMod = 25f,
                                                   Tags = new[] { "ArmorPenetration", "CooldownReduction", "Damage" },
                                                   Id = 3134
                                               };

        public static Item Sheen = new Item
                                       {
                                           Name = "Sheen", GoldBase = 365, GoldPrice = 1200, GoldSell = 840,
                                           Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1027, 1052 },
                                           Into = new[] { 3078, 3100, 3025 }, InStore = true, FlatMPPoolMod = 200f,
                                           FlatMagicDamageMod = 25f, Tags = new[] { "Mana", "SpellDamage" }, Id = 3057
                                       };

        public static Item Sightstone = new Item
                                            {
                                                Name = "Sightstone", GoldBase = 400, GoldPrice = 800, GoldSell = 320,
                                                Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1028 },
                                                Into = new[] { 2045 }, InStore = true, FlatHPPoolMod = 150f,
                                                Tags = new[] { "Active", "Health", "Vision" }, Id = 2049
                                            };

        public static Item Sorcerers_Shoes = new Item
                                                 {
                                                     Name = "Sorcerer's Shoes", GoldBase = 800, GoldPrice = 1100,
                                                     GoldSell = 770, Purchasable = true, Stacks = 1, Depth = 2,
                                                     From = new[] { 1001 }, Into = new[] { 3259, 3258, 3257, 3256, 3255 },
                                                     InStore = true, FlatMovementSpeedMod = 45f,
                                                     rFlatMagicPenetrationMod = 15f,
                                                     Tags = new[] { "Boots", "MagicPenetration" }, Id = 3020
                                                 };

        public static Item Soul_Anchor_Trinket = new Item
                                                     {
                                                         Name = "Soul Anchor (Trinket)", Purchasable = true,
                                                         Group = "RelicBase", Stacks = 1, Depth = 1, InStore = true,
                                                         Tags = new[] { "Active", "Trinket", "Vision" }, Id = 3345
                                                     };

        public static Item Spectres_Cowl = new Item
                                               {
                                                   Name = "Spectre's Cowl", GoldBase = 300, GoldPrice = 1200,
                                                   GoldSell = 840, Purchasable = true, Stacks = 1, Depth = 2,
                                                   From = new[] { 1028, 1033 }, Into = new[] { 3065, 3102 },
                                                   InStore = true, FlatHPPoolMod = 200f, FlatSpellBlockMod = 35f,
                                                   Tags = new[] { "Health", "HealthRegen", "SpellBlock" }, Id = 3211
                                               };

        public static Item Spellthiefs_Edge = new Item
                                                  {
                                                      Name = "Spellthief's Edge", GoldBase = 365, GoldPrice = 365,
                                                      GoldSell = 146, Purchasable = true, Group = "GoldBase", Stacks = 1,
                                                      Depth = 1, Into = new[] { 3098 }, InStore = true,
                                                      FlatMagicDamageMod = 5f,
                                                      Tags = new[] { "GoldPer", "Lane", "ManaRegen", "SpellDamage" },
                                                      Id = 3303
                                                  };

        public static Item Spirit_Visage = new Item
                                               {
                                                   Name = "Spirit Visage", GoldBase = 700, GoldPrice = 2750,
                                                   GoldSell = 1925, Purchasable = true, Stacks = 1, Depth = 3,
                                                   From = new[] { 3211, 3067 }, InStore = true, FlatHPPoolMod = 400f,
                                                   FlatSpellBlockMod = 55f,
                                                   Tags =
                                                       new[]
                                                           { "CooldownReduction", "Health", "HealthRegen", "SpellBlock" },
                                                   Id = 3065
                                               };

        public static Item Stalkers_Blade = new Item
                                                {
                                                    Name = "Stalker's Blade", GoldBase = 450, GoldPrice = 850,
                                                    GoldSell = 595, Purchasable = true, Group = "JungleItems", Stacks = 1,
                                                    Depth = 2, From = new[] { 1039 },
                                                    Into = new[] { 3707, 3708, 3709, 3710 }, InStore = true,
                                                    Tags =
                                                        new[] { "Damage", "HealthRegen", "Jungle", "ManaRegen", "OnHit" },
                                                    Id = 3706
                                                };

        public static Item Statikk_Shiv = new Item
                                              {
                                                  Name = "Statikk Shiv", GoldBase = 550, GoldPrice = 2600, GoldSell = 1750,
                                                  Purchasable = true, Stacks = 1, Depth = 3, From = new[] { 3086, 2015 },
                                                  InStore = true, PercentMovementSpeedMod = 0.05f,
                                                  PercentAttackSpeedMod = 0.35f, FlatCritChanceMod = 0.3f,
                                                  Tags =
                                                      new[]
                                                          { "AttackSpeed", "CriticalStrike", "NonbootsMovement", "OnHit" },
                                                  Id = 3087
                                              };

        public static Item Steraks_Gage = new Item
                                              {
                                                  Name = "Sterak's Gage", GoldBase = 345, GoldPrice = 345, GoldSell = 138,
                                                  Purchasable = true, Stacks = 1, Depth = 1, InStore = true, Id = 3053
                                              };

        public static Item Stinger = new Item
                                         {
                                             Name = "Stinger", GoldBase = 350, GoldPrice = 1250, GoldSell = 875,
                                             Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1042, 1042 },
                                             Into = new[] { 3115, 3172, 3137 }, InStore = true,
                                             PercentAttackSpeedMod = 0.4f,
                                             Tags = new[] { "AttackSpeed", "CooldownReduction" }, Id = 3101
                                         };

        public static Item Sunfire_Cape = new Item
                                              {
                                                  Name = "Sunfire Cape", GoldBase = 850, GoldPrice = 2600, GoldSell = 1820,
                                                  Purchasable = true, Stacks = 1, Depth = 3, From = new[] { 1031, 3751 },
                                                  InStore = true, FlatHPPoolMod = 450f, FlatArmorMod = 45f,
                                                  Tags = new[] { "Armor", "Health" }, Id = 3068
                                              };

        public static Item Sweeping_Lens_Trinket = new Item
                                                       {
                                                           Name = "Sweeping Lens (Trinket)", Range = 400f,
                                                           Purchasable = true, Group = "RelicBase", Stacks = 1, Depth = 1,
                                                           Into = new[] { 3364 }, InStore = true,
                                                           Tags = new[] { "Active", "Jungle", "Trinket", "Vision" },
                                                           Id = 3341
                                                       };

        public static Item Talisman_of_Ascension = new Item
                                                       {
                                                           Name = "Talisman of Ascension", GoldBase = 635,
                                                           GoldPrice = 2100, GoldSell = 840, Purchasable = true,
                                                           Group = "GoldBase", Stacks = 1, Depth = 3,
                                                           From = new[] { 3096, 3114 }, InStore = true,
                                                           Tags =
                                                               new[]
                                                                   {
                                                                       "Active", "CooldownReduction", "GoldPer",
                                                                       "HealthRegen", "ManaRegen", "NonbootsMovement"
                                                                   },
                                                           Id = 3069
                                                       };

        public static Item Targons_Brace = new Item
                                               {
                                                   Name = "Targon's Brace", GoldBase = 500, GoldPrice = 865,
                                                   GoldSell = 346, Purchasable = true, Group = "GoldBase", Stacks = 1,
                                                   Depth = 2, From = new[] { 3302 }, Into = new[] { 3401 }, InStore = true,
                                                   FlatHPPoolMod = 175f,
                                                   Tags = new[] { "Aura", "GoldPer", "Health", "HealthRegen" }, Id = 3097
                                               };

        public static Item Tear_of_the_Goddess = new Item
                                                     {
                                                         Name = "Tear of the Goddess", GoldBase = 140, GoldPrice = 720,
                                                         GoldSell = 504, Purchasable = true, Stacks = 1, Depth = 2,
                                                         From = new[] { 1027, 1004 }, Into = new[] { 3003, 3004 },
                                                         InStore = true, FlatMPPoolMod = 250f,
                                                         Tags = new[] { "Mana", "ManaRegen" }, Id = 3070
                                                     };

        public static Item Tear_of_the_Goddess_Crystal_Scar = new Item
                                                                  {
                                                                      Name = "Tear of the Goddess (Crystal Scar)",
                                                                      GoldBase = 140, GoldPrice = 720, GoldSell = 504,
                                                                      Purchasable = true, Stacks = 1, Depth = 2,
                                                                      From = new[] { 1027, 1004 },
                                                                      Into = new[] { 3007, 3008 }, InStore = true,
                                                                      FlatMPPoolMod = 250f,
                                                                      Tags = new[] { "Mana", "ManaRegen" }, Id = 3073
                                                                  };

        public static Item The_Black_Cleaver = new Item
                                                   {
                                                       Name = "The Black Cleaver", GoldBase = 1263, GoldPrice = 3000,
                                                       GoldSell = 2100, Purchasable = true, Stacks = 1, Depth = 3,
                                                       From = new[] { 3134, 1028 }, InStore = true, FlatHPPoolMod = 200f,
                                                       FlatPhysicalDamageMod = 50f,
                                                       Tags =
                                                           new[]
                                                               {
                                                                   "ArmorPenetration", "CooldownReduction", "Damage",
                                                                   "Health", "OnHit"
                                                               },
                                                       Id = 3071
                                                   };

        public static Item The_Black_Spear = new Item
                                                 {
                                                     Name = "The Black Spear", Purchasable = true, Group = "TheBlackSpear",
                                                     Stacks = 1, Depth = 1, InStore = true, RequiredChampion = "Kalista",
                                                     Tags = new[] { "Active" }, Id = 3599
                                                 };

        public static Item The_Bloodthirster = new Item
                                                   {
                                                       Name = "The Bloodthirster", GoldBase = 1150, GoldPrice = 3700,
                                                       GoldSell = 2590, Purchasable = true, Stacks = 1, Depth = 3,
                                                       From = new[] { 1053, 1038, 1036 }, InStore = true,
                                                       FlatPhysicalDamageMod = 75f, Tags = new[] { "Damage", "LifeSteal" },
                                                       Id = 3072
                                                   };

        public static Item The_Dark_Seal = new Item
                                               {
                                                   Name = "The Dark Seal", GoldBase = 400, GoldPrice = 400, GoldSell = 280,
                                                   Purchasable = true, Group = "S6Items", Consumed = false, Stacks = 1,
                                                   Depth = 1, InStore = true, FlatMPPoolMod = 100f,
                                                   FlatMagicDamageMod = 15f,
                                                   Tags = new[] { "Lane", "HealthRegen", "ManaRegen", "SpellDamage" },
                                                   Id = 1082
                                               };

        public static Item The_Hex_Core_mk_1 = new Item
                                                   {
                                                       Name = "The Hex Core mk-1", GoldBase = 1000, GoldPrice = 1000,
                                                       GoldSell = 700, Purchasable = true, Stacks = 1, Depth = 2,
                                                       From = new[] { 3200 }, Into = new[] { 3197 }, InStore = true,
                                                       RequiredChampion = "Viktor", FlatMPPoolMod = 150f,
                                                       FlatMagicDamageMod = 20f, Tags = new[] { "Mana", "SpellDamage" },
                                                       Id = 3196
                                                   };

        public static Item The_Hex_Core_mk_2 = new Item
                                                   {
                                                       Name = "The Hex Core mk-2", GoldBase = 1000, GoldPrice = 2000,
                                                       GoldSell = 1400, Purchasable = true, Stacks = 1, Depth = 3,
                                                       From = new[] { 3196 }, Into = new[] { 3198 }, InStore = true,
                                                       RequiredChampion = "Viktor", FlatMPPoolMod = 300f,
                                                       FlatMagicDamageMod = 40f, Tags = new[] { "Mana", "SpellDamage" },
                                                       Id = 3197
                                                   };

        public static Item The_Lightbringer = new Item
                                                  {
                                                      Name = "The Lightbringer", GoldBase = 350, GoldPrice = 2280,
                                                      GoldSell = 1596, Purchasable = true, Stacks = 1, Depth = 3,
                                                      From = new[] { 3122, 1018 }, InStore = true,
                                                      FlatPhysicalDamageMod = 30f, FlatCritChanceMod = 0.3f,
                                                      Tags =
                                                          new[]
                                                              {
                                                                  "Active", "CriticalStrike", "Damage", "OnHit", "Stealth",
                                                                  "Vision"
                                                              },
                                                      Id = 3185
                                                  };

        public static Item Thornmail = new Item
                                           {
                                               Name = "Thornmail", GoldBase = 1050, GoldPrice = 2100, GoldSell = 1470,
                                               Purchasable = true, Stacks = 1, Depth = 3, From = new[] { 1029, 1031 },
                                               InStore = true, FlatArmorMod = 100f, Tags = new[] { "Armor" }, Id = 3075
                                           };

        public static Item Tiamat_Melee_Only = new Item
                                                   {
                                                       Name = "Tiamat (Melee Only)", Range = 400f, GoldBase = 305,
                                                       GoldPrice = 1900, GoldSell = 1330, Purchasable = true, Stacks = 1,
                                                       Depth = 2, From = new[] { 1037, 1036, 1006, 1006 },
                                                       Into = new[] { 3074 }, InStore = true, FlatPhysicalDamageMod = 40f,
                                                       Tags = new[] { "Active", "Damage", "HealthRegen", "OnHit" },
                                                       Id = 3077
                                                   };

        public static Item Titanic_Hydra_Melee_Only = new Item
                                                          {
                                                              Name = "Titanic Hydra (Melee Only)", Range = 385f,
                                                              GoldBase = 750, GoldPrice = 3600, GoldSell = 2520,
                                                              Purchasable = true, Stacks = 1, Depth = 3,
                                                              From = new[] { 3077, 1028, 3052 }, InStore = true,
                                                              FlatHPPoolMod = 450f, FlatPhysicalDamageMod = 50f,
                                                              Tags =
                                                                  new[]
                                                                      {
                                                                          "Active", "Damage", "Health", "HealthRegen",
                                                                          "OnHit"
                                                                      },
                                                              Id = 3748
                                                          };

        public static Item Total_Biscuit_of_Rejuvenation = new Item
                                                               {
                                                                   Name = "Total Biscuit of Rejuvenation", Consumed = true,
                                                                   Stacks = 1, Depth = 1, InStore = false, Id = 2009
                                                               };

        public static Item Total_Biscuit_of_Rejuvenation2 = new Item
                                                                {
                                                                    Name = "Total Biscuit of Rejuvenation", GoldBase = 35,
                                                                    GoldPrice = 35, GoldSell = 14, Group = "HealthPotion",
                                                                    Consumed = true, Stacks = 5, Depth = 1,
                                                                    InStore = false, Id = 2010
                                                                };

        public static Item Trackers_Knife = new Item
                                                {
                                                    Name = "Tracker's Knife", GoldBase = 450, GoldPrice = 850,
                                                    GoldSell = 595, Purchasable = true, Group = "JungleItems", Stacks = 1,
                                                    Depth = 2, From = new int[] { }, Into = new int[] { }, InStore = true,
                                                    Tags =
                                                        new[] { "Damage", "HealthRegen", "Jungle", "ManaRegen", "OnHit" },
                                                    Id = 3711
                                                };

        public static Item Trinity_Force = new Item
                                               {
                                                   Name = "Trinity Force", GoldBase = 200, GoldPrice = 3800,
                                                   GoldSell = 2660, Purchasable = true, Stacks = 1, Depth = 3,
                                                   From = new[] { 3086, 3057, 3044 }, InStore = true, FlatHPPoolMod = 250f,
                                                   FlatMPPoolMod = 250f, FlatPhysicalDamageMod = 25f,
                                                   PercentMovementSpeedMod = 0.05f, PercentAttackSpeedMod = 0.15f,
                                                   FlatCritChanceMod = 0.2f,
                                                   Tags =
                                                       new[]
                                                           {
                                                               "AttackSpeed", "CriticalStrike", "Damage", "Health", "Mana",
                                                               "NonbootsMovement", "OnHit", "CooldownReduction"
                                                           },
                                                   Id = 3078
                                               };

        public static Item Twin_Shadows2 = new Item
                                               {
                                                   Name = "Twin Shadows", GoldBase = 630, GoldPrice = 2300,
                                                   GoldSell = 1610, Purchasable = true, Stacks = 1, Depth = 3,
                                                   From = new[] { 3108, 3113 }, InStore = true, FlatMagicDamageMod = 80f,
                                                   PercentMovementSpeedMod = 0.06f,
                                                   Tags =
                                                       new[]
                                                           {
                                                               "Active", "CooldownReduction", "NonbootsMovement", "Slow",
                                                               "SpellDamage", "Vision"
                                                           },
                                                   Id = 3290
                                               };

        public static Item Vampiric_Scepter = new Item
                                                  {
                                                      Name = "Vampiric Scepter", GoldBase = 550, GoldPrice = 900,
                                                      GoldSell = 630, Purchasable = true, Stacks = 1, Depth = 2,
                                                      From = new[] { 1036 },
                                                      Into = new[] { 3072, 3074, 3139, 3144, 3181, 3812 }, InStore = true,
                                                      FlatPhysicalDamageMod = 15f, PercentLifeStealMod = 0.1f,
                                                      Tags = new[] { "Damage", "LifeSteal" }, Id = 1053
                                                  };

        public static Item Vision_Ward = new Item
                                             {
                                                 Name = "Vision Ward", GoldBase = 100, GoldPrice = 100, GoldSell = 40,
                                                 Purchasable = true, Group = "PinkWards", Consumed = true, Stacks = 2,
                                                 Depth = 1, InStore = true,
                                                 Tags = new[] { "Consumable", "Lane", "Stealth", "Vision" }, Id = 2043
                                             };

        public static Item Void_Staff = new Item
                                            {
                                                Name = "Void Staff", GoldBase = 1215, GoldPrice = 2500, GoldSell = 1607,
                                                Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1026, 1052 },
                                                InStore = true, FlatMagicDamageMod = 80f,
                                                Tags = new[] { "MagicPenetration", "SpellDamage" }, Id = 3135
                                            };

        public static Item Wardens_Mail = new Item
                                              {
                                                  Name = "Warden's Mail", GoldBase = 450, GoldPrice = 1050, GoldSell = 735,
                                                  Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1029, 1029 },
                                                  Into = new[] { 3110, 3143 }, InStore = true, FlatArmorMod = 45f,
                                                  Tags = new[] { "Armor", "Slow" }, Id = 3082
                                              };

        public static Item Warding_Totem_Trinket = new Item
                                                       {
                                                           Name = "Warding Totem (Trinket)", Range = 600f,
                                                           Purchasable = true, Group = "RelicBase", Stacks = 1, Depth = 1,
                                                           Into = new[] { 3361, 3362 }, InStore = true,
                                                           Tags =
                                                               new[] { "Active", "Jungle", "Lane", "Trinket", "Vision" },
                                                           Id = 3340
                                                       };

        public static Item Warmogs_Armor = new Item
                                               {
                                                   Name = "Warmog's Armor", GoldBase = 300, GoldPrice = 2500,
                                                   GoldSell = 1750, Purchasable = true, Stacks = 1, Depth = 3,
                                                   From = new[] { 3801, 1011, 3801 }, InStore = true, FlatHPPoolMod = 800f,
                                                   Tags = new[] { "Health", "HealthRegen" }, Id = 3083
                                               };

        public static Item Wicked_Hatchet = new Item
                                                {
                                                    Name = "Wicked Hatchet", GoldBase = 440, GoldPrice = 1200,
                                                    GoldSell = 840, Purchasable = true, Stacks = 1, Depth = 2,
                                                    From = new[] { 1051, 1036 }, Into = new[] { 3104, 3185 },
                                                    InStore = true, FlatPhysicalDamageMod = 20f, FlatCritChanceMod = 0.1f,
                                                    Tags = new[] { "CriticalStrike", "Damage", "OnHit" }, Id = 3122
                                                };

        public static Item Will_of_the_Ancients = new Item
                                                      {
                                                          Name = "Will of the Ancients", GoldBase = 480, GoldPrice = 2500,
                                                          GoldSell = 1750, Purchasable = true, Stacks = 1, Depth = 3,
                                                          From = new[] { 3145, 3108 }, InStore = true,
                                                          FlatMagicDamageMod = 80f,
                                                          Tags = new[] { "CooldownReduction", "SpellDamage", "SpellVamp" },
                                                          Id = 3152
                                                      };

        public static Item Wits_End = new Item
                                          {
                                              Name = "Wit's End", GoldBase = 750, GoldPrice = 2600, GoldSell = 1820,
                                              Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1043, 1033, 1042 },
                                              InStore = true, PercentAttackSpeedMod = 0.5f, FlatSpellBlockMod = 30f,
                                              Tags = new[] { "AttackSpeed", "OnHit", "SpellBlock" }, Id = 3091
                                          };

        public static Item Wooglets_Witchcap = new Item
                                                   {
                                                       Name = "Wooglet's Witchcap", GoldBase = 1050, GoldPrice = 3500,
                                                       GoldSell = 2450, Purchasable = true, Stacks = 1, Depth = 3,
                                                       From = new[] { 3191, 1058 }, InStore = true, FlatArmorMod = 45f,
                                                       FlatMagicDamageMod = 100f,
                                                       Tags = new[] { "Active", "Armor", "SpellDamage" }, Id = 3090
                                                   };

        public static Item Wriggles_Lantern = new Item
                                                  {
                                                      Name = "Wriggle's Lantern", Range = 600f, GoldBase = 215,
                                                      GoldPrice = 1775, GoldSell = 710, Purchasable = true,
                                                      Group = "GoldBase", Stacks = 1, Depth = 3,
                                                      From = new[] { 3106, 1036, 1042 }, InStore = true,
                                                      FlatPhysicalDamageMod = 12f, PercentAttackSpeedMod = 0.3f,
                                                      Tags =
                                                          new[]
                                                              {
                                                                  "Active", "AttackSpeed", "Damage", "GoldPer", "OnHit",
                                                                  "Vision"
                                                              },
                                                      Id = 3154
                                                  };

        public static Item Youmuus_Ghostblade = new Item
                                                    {
                                                        Name = "Youmuu's Ghostblade", GoldBase = 563, GoldPrice = 2700,
                                                        GoldSell = 1890, Purchasable = true, Stacks = 1, Depth = 3,
                                                        From = new[] { 3093, 3134 }, InStore = true,
                                                        FlatPhysicalDamageMod = 30f, FlatCritChanceMod = 0.15f,
                                                        Tags =
                                                            new[]
                                                                {
                                                                    "Active", "ArmorPenetration", "AttackSpeed",
                                                                    "CooldownReduction", "CriticalStrike", "Damage",
                                                                    "NonbootsMovement"
                                                                },
                                                        Id = 3142
                                                    };

        public static Item Zeal = new Item
                                      {
                                          Name = "Zeal", GoldBase = 600, GoldPrice = 1300, GoldSell = 770,
                                          Purchasable = true, Stacks = 1, Depth = 2, From = new[] { 1051, 1042 },
                                          Into = new[] { 3046, 3078, 3087 }, InStore = true,
                                          PercentMovementSpeedMod = 0.05f, PercentAttackSpeedMod = 0.2f,
                                          FlatCritChanceMod = 0.1f,
                                          Tags = new[] { "AttackSpeed", "CriticalStrike", "NonbootsMovement" }, Id = 3086
                                      };

        public static Item Zekes_Herald = new Item
                                              {
                                                  Name = "Zeke's Herald", GoldBase = 480, GoldPrice = 2300,
                                                  GoldSell = 1610, Purchasable = true, Stacks = 1, Depth = 3,
                                                  From = new[] { 1052, 3024, 1052 }, InStore = true, FlatMPPoolMod = 250f,
                                                  FlatMagicDamageMod = 50f, FlatArmorMod = 35f,
                                                  Tags = new[] { "Aura", "CooldownReduction", "Damage", "Health" },
                                                  Id = 3050
                                              };

        public static Item Zephyr = new Item
                                        {
                                            Name = "Zephyr", GoldBase = 725, GoldPrice = 2850, GoldSell = 1995,
                                            Purchasable = true, Stacks = 1, Depth = 3, From = new[] { 3101, 1037 },
                                            InStore = true, FlatPhysicalDamageMod = 25f, PercentMovementSpeedMod = 0.1f,
                                            PercentAttackSpeedMod = 0.5f,
                                            Tags =
                                                new[]
                                                    {
                                                        "AttackSpeed", "CooldownReduction", "Damage", "NonbootsMovement",
                                                        "Tenacity"
                                                    },
                                            Id = 3172
                                        };

        public static Item Zhonyas_Hourglass = new Item
                                                   {
                                                       Name = "Zhonya's Hourglass", GoldBase = 500, GoldPrice = 3000,
                                                       GoldSell = 2100, Purchasable = true, Stacks = 1, Depth = 3,
                                                       From = new[] { 3191, 1058 }, InStore = true, FlatArmorMod = 50f,
                                                       FlatMagicDamageMod = 100f,
                                                       Tags = new[] { "Active", "Armor", "SpellDamage" }, Id = 3157
                                                   };

        public static Item ZzRot_Portal = new Item
                                              {
                                                  Name = "Zz'Rot Portal", GoldBase = 950, GoldPrice = 2800,
                                                  GoldSell = 1960, Purchasable = true, Stacks = 1, Depth = 3,
                                                  From = new[] { 2053, 1057 }, InStore = true, FlatArmorMod = 60f,
                                                  FlatSpellBlockMod = 60f,
                                                  Tags =
                                                      new[] { "Armor", "HealthRegen", "NonbootsMovement", "SpellBlock" },
                                                  Id = 3512
                                              };

        #endregion

        public struct Item
        {
            #region Fields

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

            public float FlatArmorMod;

            public float FlatAttackSpeedMod;

            public float FlatBlockMod;

            public float FlatCritChanceMod;

            public float FlatCritDamageMod;

            public float FlatEnergyPoolMod;

            public float FlatEnergyRegenMod;

            public float FlatEXPBonus;

            public float FlatHPPoolMod;

            public float FlatHPRegenMod;

            public float FlatMagicDamageMod;

            public float FlatMovementSpeedMod;

            public float FlatMPPoolMod;

            public float FlatMPRegenMod;

            public float FlatPhysicalDamageMod;

            public float FlatSpellBlockMod;

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

            public float PercentArmorMod;

            public float PercentAttackSpeedMod;

            public float PercentBlockMod;

            public float PercentCritChanceMod;

            public float PercentCritDamageMod;

            public float PercentDodgeMod;

            public float PercentEXPBonus;

            public float PercentHPPoolMod;

            public float PercentHPRegenMod;

            public float PercentLifeStealMod;

            public float PercentMagicDamageMod;

            public float PercentMovementSpeedMod;

            public float PercentMPPoolMod;

            public float PercentMPRegenMod;

            public float PercentPhysicalDamageMod;

            public float PercentSpellBlockMod;

            public float PercentSpellVampMod;

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

            public float rFlatArmorModPerLevel;

            public float rFlatArmorPenetrationMod;

            public float rFlatArmorPenetrationModPerLevel;

            public float rFlatCritChanceModPerLevel;

            public float rFlatCritDamageModPerLevel;

            public float rFlatDodgeMod;

            public float rFlatDodgeModPerLevel;

            public float rFlatEnergyModPerLevel;

            public float rFlatEnergyRegenModPerLevel;

            public float rFlatGoldPer10Mod;

            public float rFlatHPModPerLevel;

            public float rFlatHPRegenModPerLevel;

            public float rFlatMagicDamageModPerLevel;

            public float rFlatMagicPenetrationMod;

            public float rFlatMagicPenetrationModPerLevel;

            public float rFlatMovementSpeedModPerLevel;

            public float rFlatMPModPerLevel;

            public float rFlatMPRegenModPerLevel;

            public float rFlatPhysicalDamageModPerLevel;

            public float rFlatSpellBlockModPerLevel;

            public float rFlatTimeDeadMod;

            public float rFlatTimeDeadModPerLevel;

            public float rPercentArmorPenetrationMod;

            public float rPercentArmorPenetrationModPerLevel;

            public float rPercentAttackSpeedModPerLevel;

            public float rPercentCooldownMod;

            public float rPercentCooldownModPerLevel;

            public float rPercentMagicPenetrationMod;

            public float rPercentMagicPenetrationModPerLevel;

            public float rPercentMovementSpeedModPerLevel;

            public float rPercentTimeDeadMod;

            public float rPercentTimeDeadModPerLevel;

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

            #endregion

            #region Public Methods and Operators

            public Items.Item GetItem()
            {
                return new Items.Item(this.Id, this.Range);
            }

            #endregion
        }
    }
}