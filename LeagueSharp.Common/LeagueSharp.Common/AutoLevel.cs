#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace LeagueSharp.Common
{
    public class AutoLevel
    {
        private static int[] order = new int[18];
        private static int offset = 1;

        public AutoLevel(int[] levels)
        {
            order = levels;
            offset = HasLevelOneSpell(ObjectManager.Player.ChampionName) ? 2 : 1;
            CustomEvents.Unit.OnLevelUp += Unit_OnLevelUp;
        }

        public AutoLevel(IEnumerable<SpellSlot> levels)
        {
            order = levels.Select(spell => (int) spell).ToArray();
            offset = HasLevelOneSpell(ObjectManager.Player.ChampionName) ? 2 : 1;
            CustomEvents.Unit.OnLevelUp += Unit_OnLevelUp;
        }

        private static void Unit_OnLevelUp(Obj_AI_Base sender, CustomEvents.Unit.OnLevelUpEventArgs args)
        {
            if (!sender.IsValid || !sender.IsMe)
            {
                return;
            }


            ObjectManager.Player.Spellbook.LevelUpSpell((SpellSlot) order[args.NewLevel - offset]);
        }

        private static bool HasLevelOneSpell(string name)
        {
            return name == "Elise" || name == "Jayce" || name == "Karma";
        }
    }
}