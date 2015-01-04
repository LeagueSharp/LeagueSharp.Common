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
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
        }

        public static bool Enabled { get; set; }
        public static bool Debug { get; set; }

        private static void Spellbook_OnCastSpell(GameObject sender, SpellbookCastSpellEventArgs args)
        {
            if (!Enabled || sender == null || !sender.IsValid || !sender.IsMe)
            {
                return;
            }

            if (ObjectManager.Player.Spellbook.GetSpell(args.Slot).State == SpellState.Cooldown)
            {
                args.Process = false;
            }
        }

        /*private static bool CanCast(GamePacket p)
        {
            var slot = (SpellSlot) p.ReadByte(6);
            return ObjectManager.Player.Spellbook.CanUseSpell(slot) == SpellState.Ready;
        }*/
    }
}