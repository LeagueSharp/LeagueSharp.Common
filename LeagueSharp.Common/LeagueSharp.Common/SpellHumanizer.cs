#region

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
                state = ObjectManager.Player.SummonerSpellbook.CanUseSpell(slot);
            }
            else
            {
                state = ObjectManager.Player.Spellbook.CanUseSpell(slot);
            }

            return state == SpellState.Ready;
        }
    }
}