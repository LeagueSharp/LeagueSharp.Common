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

using System.Collections.Generic;
using System.Linq;

#endregion

namespace LeagueSharp.Common
{
    public class AutoLevel
    {
        private static int[] _order = new int[18];

        public AutoLevel(int[] levels)
        {
            _order = levels;
            Game.OnGameProcessPacket += InitialLevelUp;
        }

        public AutoLevel(IEnumerable<SpellSlot> levels)
        {
            _order = levels.Select(spell => (int) spell).ToArray();
            Game.OnGameProcessPacket += InitialLevelUp;
        }

        public static void Enabled(bool enabled)
        {
            if (enabled)
            {
                Game.OnGameProcessPacket += Game_OnGameProcessPacket;
            }
            else
            {
                Game.OnGameProcessPacket -= Game_OnGameProcessPacket;
            }
        }

        private static void InitialLevelUp(GamePacketEventArgs args)
        {
            if (Game.Time < 20)
            {
                for (var i = 0; i < ObjectManager.Player.Level; i++)
                {
                    var spell = (SpellSlot) (_order[i] - 1);
                    if (ObjectManager.Player.Spellbook.GetSpell(spell).Level < 2)
                    {
                        ObjectManager.Player.Spellbook.LevelUpSpell(spell);
                    }
                }
            }
            Game.OnGameProcessPacket += Game_OnGameProcessPacket;
            Game.OnGameProcessPacket -= InitialLevelUp;
        }

        private static void Game_OnGameProcessPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] != Packet.S2C.LevelUp.Header)
            {
                return;
            }

            var dp = Packet.S2C.LevelUp.Decoded(args.PacketData);

            if (!dp.Unit.IsValid || !dp.Unit.IsMe || (ObjectManager.Player.Level == 1 && HasLevelOneSpell()))
            {
                return;
            }

            var spell = (SpellSlot) (_order[dp.Level - 1] - 1);
            ObjectManager.Player.Spellbook.LevelUpSpell(spell);
        }

        private static bool HasLevelOneSpell()
        {
            var name = ObjectManager.Player.ChampionName;
            return name == "Elise" || name == "Jayce" || name == "Karma";
        }
    }
}
