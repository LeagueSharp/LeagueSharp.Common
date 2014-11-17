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
            CustomEvents.Unit.OnLevelUp += Unit_OnLevelUp;
        }

        public AutoLevel(IEnumerable<SpellSlot> levels)
        {
            order = levels.Select(spell => (int) spell).ToArray();
            CustomEvents.Unit.OnLevelUp += Unit_OnLevelUp;
        }

        public static void Enabled(bool enabled)
        {
            if (enabled)
            {
                CustomEvents.Unit.OnLevelUp += Unit_OnLevelUp;
            }
            else
            {
                CustomEvents.Unit.OnLevelUp -= Unit_OnLevelUp;
            }
        }

        private static void Unit_OnLevelUp(Obj_AI_Base sender, CustomEvents.Unit.OnLevelUpEventArgs args)
        {
            if (!sender.IsValid || !sender.IsMe)
            {
                return;
            }

            if (ObjectManager.Player.Level == 1 && HasLevelOneSpell())
            {
                return;
            }

            ObjectManager.Player.Spellbook.LevelUpSpell((SpellSlot) order[args.NewLevel - 1]);
        }

        private static bool HasLevelOneSpell()
        {
            var name = ObjectManager.Player.ChampionName;
            return name == "Elise" || name == "Jayce" || name == "Karma";
        }
    }
}