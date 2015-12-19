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

using System.Linq;

#endregion

namespace LeagueSharp.Common.Data
{
    public static class MasteryData
    {
        public static Mastery FindMastery(this Obj_AI_Hero @hero, MasteryPage page, int id)
        {
            var mastery = @hero.Masteries.FirstOrDefault(m => m.Page == page && m.Id == id);
            return mastery;
        }

        public static Mastery GetMastery(this Obj_AI_Hero hero, Ferocity ferocity)
        {
            return FindMastery(hero, MasteryPage.Ferocity, (int)ferocity);
        }

        public static Mastery GetMastery(this Obj_AI_Hero hero, Cunning cunning)
        {
            return FindMastery(hero, MasteryPage.Cunning, (int)cunning);
        }

        public static Mastery GetMastery(this Obj_AI_Hero hero, Resolve resolve)
        {
            return FindMastery(hero, MasteryPage.Resolve, (int)resolve);
        }

        public static bool IsActive(this Mastery mastery)
        {
            return mastery.Points >= 1;
        }

        public enum Ferocity
        {
            Fury = 65,
            Sorcery = 68,
            DoubleEdgedSword = 81,
            Vampirism = 97,
            NaturalTalent = 100,
            Feast = 82,
            BountyHunter = 113,
            Oppresor = 114,
            BatteringBlows = 129,
            PiercingThoughts = 132,
            WarlordsBloodlust = 145,
            FervorofBattle = 146,
            DeathFireTouch = 137
        }

        public enum Cunning
        {
            Wanderer = 65,
            Savagery = 66,
            RunicAffinity = 81,
            SecretStash = 82,
            Meditation = 98,
            Merciless = 97,
            Bandit = 114,
            DangerousGame = 115,
            Precision = 129,
            Intelligence = 130,
            StormraidersSurge = 145,
            ThunderlordsDecree = 146,
            WindspeakerBlessing = 147
        }

        public enum Resolve
        {
            Recovery = 65,
            Unyielding = 66,
            Explorer = 81,
            ToughSkin = 82,
            RunicArmor = 97,
            VeteransScar = 98,
            Insight = 113,
            Swiftness = 129,
            LegendaryGuardian = 130,
            GraspoftheUndying = 145,
            StrengthoftheAges = 146,
            BondofStones = 147
        }
    }
}