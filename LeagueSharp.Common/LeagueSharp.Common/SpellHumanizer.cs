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

        public static bool Check(GamePacket p)
        {
            return !Enabled || CanCast(p);
        }

        private static void Game_OnGameSendPacket(GamePacketEventArgs args)
        {
            if (Enabled && args.PacketData[0] == Packet.C2S.Cast.Header && !CanCast(new GamePacket(args.PacketData)))
            {
                args.Process = false;
            }
        }

        private static bool CanCast(GamePacket p)
        {
            var slot = (SpellSlot) p.ReadByte(6);
            var state = IsSummonerSpell(p.ReadByte(5))
                ? ObjectManager.Player.SummonerSpellbook.CanUseSpell(slot)
                : ObjectManager.Player.Spellbook.CanUseSpell(slot);
            return state == SpellState.Ready;
        }

        private static bool IsSummonerSpell(byte spellByte)
        {
            return spellByte == 0xE9 || spellByte == 0xEF || spellByte == 0x8B || spellByte == 0xED || spellByte == 0x63;
        }
    }
}