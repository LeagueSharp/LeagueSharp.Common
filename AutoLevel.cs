#region LICENSE

/*
 Copyright 2014 - 2015 LeagueSharp
 AutoLevel.cs is part of LeagueSharp.Common.
 
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
using System.Collections.Generic;
using System.Linq;

#endregion

namespace LeagueSharp.Common
{
    public class AutoLevel
    {
        private static List<SpellSlot> order = new List<SpellSlot>();
        private static float LastLeveled;
        private static float NextDelay;
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;
        private static Random RandomNumber;
        private static bool enabled;
        private static bool init;

        public AutoLevel(IEnumerable<int> levels)
        {
            UpdateSequence(levels);
            Init();
        }

        public AutoLevel(List<SpellSlot> levels)
        {
            UpdateSequence(levels);
            Init();
        }

        private static void Init()
        {
            if (init)
            {
                return;
            }

            init = true;
            RandomNumber = new Random(Utils.TickCount);
            Game.OnUpdate += Game_OnGameUpdate;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (!enabled || Player.SpellTrainingPoints < 1 || Utils.TickCount - LastLeveled < NextDelay)
            {
                return;
            }

            NextDelay = RandomNumber.Next(750);
            LastLeveled = Utils.TickCount;
            var spell = order[GetTotalPoints()];
            Player.Spellbook.LevelSpell(spell);
        }

        private static int GetTotalPoints()
        {
            var spell = Player.Spellbook;
            var q = spell.GetSpell(SpellSlot.Q).Level;
            var w = spell.GetSpell(SpellSlot.W).Level;
            var e = spell.GetSpell(SpellSlot.E).Level;
            var r = spell.GetSpell(SpellSlot.R).Level;

            return q + w + e + r;
        }

        public static void Enable()
        {
            enabled = true;
        }

        public static void Disable()
        {
            enabled = false;
        }

        public static void Enabled(bool b)
        {
            enabled = b;
        }

        public static void UpdateSequence(IEnumerable<int> levels)
        {
            Init();
            order.Clear();
            foreach (var level in levels)
            {
                order.Add((SpellSlot) level);
            }
        }

        public static void UpdateSequence(List<SpellSlot> levels)
        {
            Init();
            order.Clear();
            order = levels;
        }

        public static int[] GetSequence()
        {
            return order.Select(spell => (int) spell).ToArray();
        }

        public static List<SpellSlot> GetSequenceList()
        {
            return order;
        }
    }
}
