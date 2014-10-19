#region

using System;
using System.Collections.Generic;

#endregion

namespace LeagueSharp.Common
{
    public static class SpellHumanizer
    {
        private static readonly List<byte> NonSummonerByte = new List<byte>
        {
            0xF3,
            0xE8,
            0xEE,
            0xEC,
            0x7E,
            0xBE,
            0x1A,
            0x12,
            0x6E,
            0x0,
            0x2
        };

        private static readonly List<byte> SummonerByte = new List<byte> { 0xE9, 0xEF, 0x8B, 0xED, 0x63 };

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

            var bit = args.PacketData[5];

            if (Debug && !NonSummonerByte.Contains(bit))
            {
                Utility.DebugMessage("Blocked: " + BitConverter.ToString(new[] { bit }));
            }

            args.Process = false;
        }

        private static bool CanCast(GamePacket p)
        {
            var spellByte = p.ReadByte(5);
            var slot = (SpellSlot) p.ReadByte();
            var state = SummonerByte.Contains(spellByte)
                ? ObjectManager.Player.SummonerSpellbook.CanUseSpell(slot)
                : ObjectManager.Player.Spellbook.CanUseSpell(slot);
            return state == SpellState.Ready;
        }
    }
}