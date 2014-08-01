#region

using System;
using System.Collections.Generic;

#endregion

namespace LeagueSharp.Common
{
    public class LastCastedSpellEntry
    {
        public string Name;
        public Obj_AI_Base Target;
        public int Tick;

        public LastCastedSpellEntry(string name, int tick, Obj_AI_Base target)
        {
            Name = name;
            Tick = tick;
            Target = target;
        }
    }

    public static class LastCastedSpell
    {
        private static readonly Dictionary<int, LastCastedSpellEntry> CastedSpells =
            new Dictionary<int, LastCastedSpellEntry>();

        static LastCastedSpell()
        {
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender is Obj_AI_Hero)
            {
                var entry = new LastCastedSpellEntry(args.SData.Name, Environment.TickCount, ObjectManager.Player);
                if (CastedSpells.ContainsKey(sender.NetworkId))
                {
                    CastedSpells[sender.NetworkId] = entry;
                }
                else
                {
                    CastedSpells.Add(sender.NetworkId, entry);
                }
            }
        }

        public static int LastCastedSpellT(this Obj_AI_Hero unit)
        {
            if (CastedSpells.ContainsKey(unit.NetworkId))
                return CastedSpells[unit.NetworkId].Tick;
            return 0;
        }

        public static string LastCastedSpellName(this Obj_AI_Hero unit)
        {
            if (CastedSpells.ContainsKey(unit.NetworkId))
                return CastedSpells[unit.NetworkId].Name;
            return "";
        }

        public static Obj_AI_Base LastCastedSpellTarget(this Obj_AI_Hero unit)
        {
            if (CastedSpells.ContainsKey(unit.NetworkId))
                return CastedSpells[unit.NetworkId].Target;
            return null;
        }

        public static LastCastedSpellEntry LastCastedspell(this Obj_AI_Hero unit)
        {
            if (CastedSpells.ContainsKey(unit.NetworkId))
                return CastedSpells[unit.NetworkId];
            return null;
        }
    }
}