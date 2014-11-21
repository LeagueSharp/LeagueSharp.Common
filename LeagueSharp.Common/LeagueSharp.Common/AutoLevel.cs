#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace LeagueSharp.Common
{
    public class AutoLevel
    {
        private static int[] order = new int[18];

        public AutoLevel(int[] levels)
        {
            order = levels;
            Game.OnGameProcessPacket += Game_OnGameProcessPacket;
        }

        public AutoLevel(IEnumerable<SpellSlot> levels)
        {
            order = levels.Select(spell => (int) spell).ToArray();
            Game.OnGameProcessPacket += Game_OnGameProcessPacket;
        }

        public static void Enabled(bool enabled)
        {
            if (enabled)
            {
                Game.OnGameProcessPacket -= Game_OnGameProcessPacket;
            }
            else
            {
                Game.OnGameProcessPacket -= Game_OnGameProcessPacket;
            }
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

            ObjectManager.Player.Spellbook.LevelUpSpell((SpellSlot) order[dp.Level - 1]);
        }


        private static bool HasLevelOneSpell()
        {
            var name = ObjectManager.Player.ChampionName;
            return name == "Elise" || name == "Jayce" || name == "Karma";
        }
    }
}