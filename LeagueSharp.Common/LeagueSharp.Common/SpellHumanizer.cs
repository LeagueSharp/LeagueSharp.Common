#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 SpellHumanizer.cs is part of LeagueSharp.Common.
 
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

namespace LeagueSharp.Common
{
    public static class SpellHumanizer
    {
        static SpellHumanizer()
        {
            Enabled = false;
            Game.OnGameSendPacket += Game_OnGameSendPacket;
        }


        public static bool Enabled { get; set; }
        public static bool Debug { get; set; }

        public static bool Check(GamePacket p)
        {
            return !Enabled || CanCast(p);
        }

        private static void Game_OnGameSendPacket(GamePacketEventArgs args)
        {
            if (!Enabled || args.PacketData[0] != Packet.C2S.Cast.Header || CanCast(new GamePacket(args.PacketData)))
            {
                return;
            }

            args.Process = false;
        }

        private static bool CanCast(GamePacket p)
        {
            var slot = (SpellSlot) p.ReadByte(6);
            SpellState state;

            if (slot == SpellSlot.Summoner1 || slot == SpellSlot.Summoner2)
            {
                state = ObjectManager.Player.Spellbook.CanUseSpell(slot);
            }
            else
            {
                state = ObjectManager.Player.Spellbook.CanUseSpell(slot);
            }

            return state == SpellState.Ready;
        }
    }
}