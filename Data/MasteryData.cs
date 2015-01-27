#region LICENSE

/*
 Copyright 2014 - 2015 LeagueSharp
 MasteryData.cs is part of LeagueSharp.Common.Data.
 
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

using System.Diagnostics.CodeAnalysis;

#endregion

namespace LeagueSharp.Common.Data
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class MasteryData
    {
        public struct Mastery
        {
            /// <summary>
            ///     Mastery ByteId
            /// </summary>
            public byte ByteId;

            /// <summary>
            ///     Mastery Id
            /// </summary>
            public int Id;

            /// <summary>
            ///     Mastery Name
            /// </summary>
            public string Name;

            /// <summary>
            ///     Mastery Requirements
            /// </summary>
            public int RequiredId;

            /// <summary>
            ///     Mastery Tree
            /// </summary>
            public MasteryPage Tree;
        }

        #region Masteries

        #region Double-Edged Sword

        public static Mastery Double_Edged_Sword = new Mastery
        {
            Id = 4111,
            Name = "Double-Edged Sword",
            Tree = MasteryPage.Offense,
            RequiredId = 0,
            ByteId = 65
        };

        #endregion

        #region Fury

        public static Mastery Fury = new Mastery
        {
            Id = 4112,
            Name = "Fury",
            Tree = MasteryPage.Offense,
            RequiredId = 0,
            ByteId = 66
        };

        #endregion

        #region Sorcery

        public static Mastery Sorcery = new Mastery
        {
            Id = 4113,
            Name = "Sorcery",
            Tree = MasteryPage.Offense,
            RequiredId = 0,
            ByteId = 67
        };

        #endregion

        #region Butcher

        public static Mastery Butcher = new Mastery
        {
            Id = 4114,
            Name = "Butcher",
            Tree = MasteryPage.Offense,
            RequiredId = 0,
            ByteId = 68
        };

        #endregion

        #region Expose Weakness

        public static Mastery Expose_Weakness = new Mastery
        {
            Id = 4121,
            Name = "Expose Weakness",
            Tree = MasteryPage.Offense,
            RequiredId = 0,
            ByteId = 81
        };

        #endregion

        #region Brute Force

        public static Mastery Brute_Force = new Mastery
        {
            Id = 4122,
            Name = "Brute Force",
            Tree = MasteryPage.Offense,
            RequiredId = 0,
            ByteId = 82
        };

        #endregion

        #region Mental Force

        public static Mastery Mental_Force = new Mastery
        {
            Id = 4123,
            Name = "Mental Force",
            Tree = MasteryPage.Offense,
            RequiredId = 0,
            ByteId = 83
        };

        #endregion

        #region Feast

        public static Mastery Feast = new Mastery
        {
            Id = 4124,
            Name = "Feast",
            Tree = MasteryPage.Offense,
            RequiredId = 4114,
            ByteId = 84
        };

        #endregion

        #region Spell Weaving

        public static Mastery Spell_Weaving = new Mastery
        {
            Id = 4131,
            Name = "Spell Weaving",
            Tree = MasteryPage.Offense,
            RequiredId = 0,
            ByteId = 97
        };

        #endregion

        #region Martial Mastery

        public static Mastery Martial_Mastery = new Mastery
        {
            Id = 4132,
            Name = "Martial Mastery",
            Tree = MasteryPage.Offense,
            RequiredId = 4122,
            ByteId = 98
        };

        #endregion

        #region Arcane Mastery

        public static Mastery Arcane_Mastery = new Mastery
        {
            Id = 4133,
            Name = "Arcane Mastery",
            Tree = MasteryPage.Offense,
            RequiredId = 4123,
            ByteId = 99
        };

        #endregion

        #region Executioner

        public static Mastery Executioner = new Mastery
        {
            Id = 4134,
            Name = "Executioner",
            Tree = MasteryPage.Offense,
            RequiredId = 0,
            ByteId = 100
        };

        #endregion

        #region Blade Weaving

        public static Mastery Blade_Weaving = new Mastery
        {
            Id = 4141,
            Name = "Blade Weaving",
            Tree = MasteryPage.Offense,
            RequiredId = 4131,
            ByteId = 113
        };

        #endregion

        #region Warlord

        public static Mastery Warlord = new Mastery
        {
            Id = 4142,
            Name = "Warlord",
            Tree = MasteryPage.Offense,
            RequiredId = 0,
            ByteId = 114
        };

        #endregion

        #region Archmage

        public static Mastery Archmage = new Mastery
        {
            Id = 4143,
            Name = "Archmage",
            Tree = MasteryPage.Offense,
            RequiredId = 0,
            ByteId = 115
        };

        #endregion

        #region Dangerous Game

        public static Mastery Dangerous_Game = new Mastery
        {
            Id = 4144,
            Name = "Dangerous Game",
            Tree = MasteryPage.Offense,
            RequiredId = 4134,
            ByteId = 116
        };

        #endregion

        #region Frenzy

        public static Mastery Frenzy = new Mastery
        {
            Id = 4151,
            Name = "Frenzy",
            Tree = MasteryPage.Offense,
            RequiredId = 0,
            ByteId = 129
        };

        #endregion

        #region Devastating Strikes

        public static Mastery Devastating_Strikes = new Mastery
        {
            Id = 4152,
            Name = "Devastating Strikes",
            Tree = MasteryPage.Offense,
            RequiredId = 0,
            ByteId = 130
        };

        #endregion

        #region Arcane Blade

        public static Mastery Arcane_Blade = new Mastery
        {
            Id = 4154,
            Name = "Arcane Blade",
            Tree = MasteryPage.Offense,
            RequiredId = 0,
            ByteId = 132
        };

        #endregion

        #region Havoc

        public static Mastery Havoc = new Mastery
        {
            Id = 4162,
            Name = "Havoc",
            Tree = MasteryPage.Offense,
            RequiredId = 0,
            ByteId = 146
        };

        #endregion

        #region Block

        public static Mastery Block = new Mastery
        {
            Id = 4211,
            Name = "Block",
            Tree = MasteryPage.Defense,
            RequiredId = 0,
            ByteId = 65
        };

        #endregion

        #region Recovery

        public static Mastery Recovery = new Mastery
        {
            Id = 4212,
            Name = "Recovery",
            Tree = MasteryPage.Defense,
            RequiredId = 0,
            ByteId = 66
        };

        #endregion

        #region Enchanted Armor

        public static Mastery Enchanted_Armor = new Mastery
        {
            Id = 4213,
            Name = "Enchanted Armor",
            Tree = MasteryPage.Defense,
            RequiredId = 0,
            ByteId = 67
        };

        #endregion

        #region Tough Skin

        public static Mastery Tough_Skin = new Mastery
        {
            Id = 4214,
            Name = "Tough Skin",
            Tree = MasteryPage.Defense,
            RequiredId = 0,
            ByteId = 68
        };

        #endregion

        #region Unyielding

        public static Mastery Unyielding = new Mastery
        {
            Id = 4221,
            Name = "Unyielding",
            Tree = MasteryPage.Defense,
            RequiredId = 4211,
            ByteId = 81
        };

        #endregion

        #region Veteran's Scars

        public static Mastery Veterans_Scars = new Mastery
        {
            Id = 4222,
            Name = "Veteran's Scars",
            Tree = MasteryPage.Defense,
            RequiredId = 0,
            ByteId = 82
        };

        #endregion

        #region Bladed Armor

        public static Mastery Bladed_Armor = new Mastery
        {
            Id = 4224,
            Name = "Bladed Armor",
            Tree = MasteryPage.Defense,
            RequiredId = 4214,
            ByteId = 84
        };

        #endregion

        #region Oppression

        public static Mastery Oppression = new Mastery
        {
            Id = 4231,
            Name = "Oppression",
            Tree = MasteryPage.Defense,
            RequiredId = 0,
            ByteId = 97
        };

        #endregion

        #region Juggernaut

        public static Mastery Juggernaut = new Mastery
        {
            Id = 4232,
            Name = "Juggernaut",
            Tree = MasteryPage.Defense,
            RequiredId = 4222,
            ByteId = 98
        };

        #endregion

        #region Hardiness

        public static Mastery Hardiness = new Mastery
        {
            Id = 4233,
            Name = "Hardiness",
            Tree = MasteryPage.Defense,
            RequiredId = 0,
            ByteId = 99
        };

        #endregion

        #region Resistance

        public static Mastery Resistance = new Mastery
        {
            Id = 4234,
            Name = "Resistance",
            Tree = MasteryPage.Defense,
            RequiredId = 0,
            ByteId = 100
        };

        #endregion

        #region Perseverance

        public static Mastery Perseverance_ = new Mastery
        {
            Id = 4241,
            Name = "Perseverance ",
            Tree = MasteryPage.Defense,
            RequiredId = 0,
            ByteId = 113
        };

        #endregion

        #region Swiftness

        public static Mastery Swiftness = new Mastery
        {
            Id = 4242,
            Name = "Swiftness",
            Tree = MasteryPage.Defense,
            RequiredId = 0,
            ByteId = 114
        };

        #endregion

        #region Reinforced Armor

        public static Mastery Reinforced_Armor = new Mastery
        {
            Id = 4243,
            Name = "Reinforced Armor",
            Tree = MasteryPage.Defense,
            RequiredId = 4233,
            ByteId = 115
        };

        #endregion

        #region Evasive

        public static Mastery Evasive = new Mastery
        {
            Id = 4244,
            Name = "Evasive",
            Tree = MasteryPage.Defense,
            RequiredId = 4234,
            ByteId = 116
        };

        #endregion

        #region Second Wind

        public static Mastery Second_Wind = new Mastery
        {
            Id = 4251,
            Name = "Second Wind",
            Tree = MasteryPage.Defense,
            RequiredId = 4241,
            ByteId = 129
        };

        #endregion

        #region Legendary Guardian

        public static Mastery Legendary_Guardian = new Mastery
        {
            Id = 4252,
            Name = "Legendary Guardian",
            Tree = MasteryPage.Defense,
            RequiredId = 0,
            ByteId = 130
        };

        #endregion

        #region Runic Blessing

        public static Mastery Runic_Blessing = new Mastery
        {
            Id = 4253,
            Name = "Runic Blessing",
            Tree = MasteryPage.Defense,
            RequiredId = 0,
            ByteId = 131
        };

        #endregion

        #region Tenacious

        public static Mastery Tenacious = new Mastery
        {
            Id = 4262,
            Name = "Tenacious",
            Tree = MasteryPage.Defense,
            RequiredId = 0,
            ByteId = 146
        };

        #endregion

        #region Phasewalker

        public static Mastery Phasewalker = new Mastery
        {
            Id = 4311,
            Name = "Phasewalker",
            Tree = MasteryPage.Utility,
            RequiredId = 0,
            ByteId = 65
        };

        #endregion

        #region Fleet of Foot

        public static Mastery Fleet_of_Foot = new Mastery
        {
            Id = 4312,
            Name = "Fleet of Foot",
            Tree = MasteryPage.Utility,
            RequiredId = 0,
            ByteId = 66
        };

        #endregion

        #region Meditation

        public static Mastery Meditation = new Mastery
        {
            Id = 4313,
            Name = "Meditation",
            Tree = MasteryPage.Utility,
            RequiredId = 0,
            ByteId = 67
        };

        #endregion

        #region Scout

        public static Mastery Scout = new Mastery
        {
            Id = 4314,
            Name = "Scout",
            Tree = MasteryPage.Utility,
            RequiredId = 0,
            ByteId = 68
        };

        #endregion

        #region Summoner's Insight

        public static Mastery Summoners_Insight = new Mastery
        {
            Id = 4322,
            Name = "Summoner's Insight",
            Tree = MasteryPage.Utility,
            RequiredId = 0,
            ByteId = 82
        };

        #endregion

        #region Strength of Spirit

        public static Mastery Strength_of_Spirit = new Mastery
        {
            Id = 4323,
            Name = "Strength of Spirit",
            Tree = MasteryPage.Utility,
            RequiredId = 4313,
            ByteId = 83
        };

        #endregion

        #region Alchemist

        public static Mastery Alchemist = new Mastery
        {
            Id = 4324,
            Name = "Alchemist",
            Tree = MasteryPage.Utility,
            RequiredId = 0,
            ByteId = 84
        };

        #endregion

        #region Greed

        public static Mastery Greed = new Mastery
        {
            Id = 4331,
            Name = "Greed",
            Tree = MasteryPage.Utility,
            RequiredId = 0,
            ByteId = 97
        };

        #endregion

        #region Runic Affinity

        public static Mastery Runic_Affinity = new Mastery
        {
            Id = 4332,
            Name = "Runic Affinity",
            Tree = MasteryPage.Utility,
            RequiredId = 0,
            ByteId = 98
        };

        #endregion

        #region Vampirism

        public static Mastery Vampirism = new Mastery
        {
            Id = 4333,
            Name = "Vampirism",
            Tree = MasteryPage.Utility,
            RequiredId = 0,
            ByteId = 99
        };

        #endregion

        #region Culinary Master

        public static Mastery Culinary_Master = new Mastery
        {
            Id = 4334,
            Name = "Culinary Master",
            Tree = MasteryPage.Utility,
            RequiredId = 4324,
            ByteId = 100
        };

        #endregion

        #region Scavenger

        public static Mastery Scavenger = new Mastery
        {
            Id = 4341,
            Name = "Scavenger",
            Tree = MasteryPage.Utility,
            RequiredId = 4331,
            ByteId = 113
        };

        #endregion

        #region Wealth

        public static Mastery Wealth = new Mastery
        {
            Id = 4342,
            Name = "Wealth",
            Tree = MasteryPage.Utility,
            RequiredId = 0,
            ByteId = 114
        };

        #endregion

        #region Expanded Mind

        public static Mastery Expanded_Mind = new Mastery
        {
            Id = 4343,
            Name = "Expanded Mind",
            Tree = MasteryPage.Utility,
            RequiredId = 0,
            ByteId = 115
        };

        #endregion

        #region Inspiration

        public static Mastery Inspiration = new Mastery
        {
            Id = 4344,
            Name = "Inspiration",
            Tree = MasteryPage.Utility,
            RequiredId = 0,
            ByteId = 116
        };

        #endregion

        #region Bandit

        public static Mastery Bandit = new Mastery
        {
            Id = 4352,
            Name = "Bandit",
            Tree = MasteryPage.Utility,
            RequiredId = 4342,
            ByteId = 130
        };

        #endregion

        #region Intelligence

        public static Mastery Intelligence = new Mastery
        {
            Id = 4353,
            Name = "Intelligence",
            Tree = MasteryPage.Utility,
            RequiredId = 0,
            ByteId = 131
        };

        #endregion

        #region Wanderer

        public static Mastery Wanderer = new Mastery
        {
            Id = 4362,
            Name = "Wanderer",
            Tree = MasteryPage.Utility,
            RequiredId = 0,
            ByteId = 146
        };

        #endregion

        #endregion
    }
}