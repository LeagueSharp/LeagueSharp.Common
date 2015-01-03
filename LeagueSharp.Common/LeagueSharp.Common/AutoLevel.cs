#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
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
        private static int[] order = new int[18];
        private static int offset;
        private static int lastLeveled;
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;
        private static Random RandomNumber;

        public AutoLevel(int[] levels)
        {
            order = levels;
            Utility.DelayAction.Add(500, Initialize);
        }

        public AutoLevel(IEnumerable<SpellSlot> levels)
        {
            order = levels.Select(spell => (int) spell).ToArray();
            Utility.DelayAction.Add(500, Initialize);
        }

        private static void Initialize()
        {
            RandomNumber = new Random(Environment.TickCount);
            var spellbook = Player.Spellbook;
            var spells = new List<SpellSlot> { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R };


            if (HasLevelOneSpell())
            {
                offset = 1;
                spells.Remove(SpellSlot.R);
            }

            //not beginning of game
            if (spells.Any(spell => spellbook.GetSpell(spell).Level != 0))
            {
                lastLeveled = Player.Level;
                Game.OnGameUpdate += Game_OnGameUpdate;
                return;
            }

            for (var i = 0; i < ObjectManager.Player.Level; i++)
            {
                var spell = (SpellSlot) (order[i + offset] - 1);
                Utility.DelayAction.Add(RandomNumber.Next(500), () => { spellbook.LevelSpell(spell); });
            }


            lastLeveled = ObjectManager.Player.Level;
            Game.OnGameUpdate += Game_OnGameUpdate;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Player.Level <= lastLeveled)
            {
                return;
            }

            Utility.DelayAction.Add(
                RandomNumber.Next(500), () =>
                {
                    var spell = (SpellSlot) (order[Player.Level + offset - 1] - 1);
                    Player.Spellbook.LevelSpell(spell);
                });
            lastLeveled = Player.Level;
        }

        public static void Enabled(bool enabled)
        {
            if (enabled)
            {
                Game.OnGameUpdate += Game_OnGameUpdate;
            }
            else
            {
                Game.OnGameUpdate -= Game_OnGameUpdate;
            }
        }

        private static bool HasLevelOneSpell()
        {
            var name = Player.ChampionName;
            var list = new List<string> { "Elise", "Jayce", "Karma", "Nidalee" };
            return list.Contains(name);
        }
    }
}