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
        private static List<SpellSlot> _order = new List<SpellSlot>();
        private static float _lastLeveled;
        private static float _nextDelay;
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;
        private static Random _randomNumber;
        private static bool _enabled;
        private static bool _init;

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
            if (_init)
            {
                return;
            }

            _init = true;
            _randomNumber = new Random(Utils.TickCount);
            Game.OnUpdate += Game_OnGameUpdate;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (!_enabled || Player.SpellTrainingPoints < 1 || Utils.TickCount - _lastLeveled < _nextDelay)
            {
                return;
            }

            _nextDelay = _randomNumber.Next(750);
            _lastLeveled = Utils.TickCount;
            var spell = _order[GetTotalPoints()];
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
            _enabled = true;
        }

        public static void Disable()
        {
            _enabled = false;
        }

        public static void Enabled(bool b)
        {
            _enabled = b;
        }

        public static void UpdateSequence(IEnumerable<int> levels)
        {
            Init();
            _order.Clear();
            foreach (var level in levels)
            {
                _order.Add((SpellSlot) level);
            }
        }

        public static void UpdateSequence(List<SpellSlot> levels)
        {
            Init();
            _order.Clear();
            _order = levels;
        }

        public static int[] GetSequence()
        {
            return _order.Select(spell => (int) spell).ToArray();
        }

        public static List<SpellSlot> GetSequenceList()
        {
            return _order;
        }
    }
}