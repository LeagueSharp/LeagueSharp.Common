namespace LeagueSharp.Common
{
    internal class SpellHumanizer
    {
        static SpellHumanizer()
        {
            Enabled = false;
        }

        public static bool Enabled { get; set; }

        public static bool Check(GamePacket p)
        {
            if (!Enabled)
            {
                return true;
            }

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