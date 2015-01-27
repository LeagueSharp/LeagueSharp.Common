using System;
using System.Linq;
using SharpDX;

namespace LeagueSharp.Common
{
    static class SummonerSpells
    {

        #region Spells
        public static SummonerSpell Ignite = new SummonerSpell
        {
            Names = new[] { "summonerdot" },
            Range = 600f
        };

        public static SummonerSpell Ghost = new SummonerSpell
        {
            Names = new[] { "summonerhaste" }
        };

        public static SummonerSpell Flash = new SummonerSpell
        {
            Names = new[] { "summonerflash" },
            Range = 400f
        };

        public static SummonerSpell Teleport = new SummonerSpell
        {
            Names = new[] { "summonerteleport" },
            Range = float.MaxValue
        };

        public static SummonerSpell Heal = new SummonerSpell
        {
            Names = new[] { "summonerheal" }
        };

        public static SummonerSpell Barrier = new SummonerSpell
        {
            Names = new[] { "summonerbarrier" }
        };

        public static SummonerSpell Smite = new SummonerSpell
        {
            Names =
                new[] { "s5_summonersmiteplayerganker", "s5_summonersmitequick", "s5_summonersmiteduel", "itemsmiteaoe" },
            Range = 760f
        };

        public static SummonerSpell Revive = new SummonerSpell
        {
            Names = new[] { "SummonerRevive" }
        };

        public static SummonerSpell Clarity = new SummonerSpell
        {
            Names = new[] { "SummonerMana" }
        };

        public static SummonerSpell Clairvoyance = new SummonerSpell
        {
            Names = new[] { "SummonerClairvoyance" },
            Range = float.MaxValue
        };

        public static SummonerSpell Garrison = new SummonerSpell
        {
            Names = new[] { "SummonerOdinGarrison" },
            Range = 1000f
        };

        public static SummonerSpell Exhaust = new SummonerSpell
        {
            Names = new[] { "SummonerExhaust" },
            Range = 650f
        };

        #endregion

        public static bool IsReady(this SummonerSpell spell)
        {
            return spell.Slot != SpellSlot.Unknown &&
                   ObjectManager.Player.Spellbook.CanUseSpell(spell.Slot) == SpellState.Ready;
        }

        public static bool Cast(this SummonerSpell spell)
        {
            return IsReady(spell) && ObjectManager.Player.Spellbook.CastSpell(spell.Slot);
        }

        public static bool Cast(this SummonerSpell spell, Obj_AI_Hero tgHero)
        {
            return IsReady(spell) &&
                (ObjectManager.Player.Distance(tgHero, true) < spell.Range * spell.Range) &&
                ObjectManager.Player.Spellbook.CastSpell(spell.Slot, tgHero ?? ObjectManager.Player);
        }

        public static bool Cast(this SummonerSpell spell, Vector2 position)
        {
            return IsReady(spell) &&
                (ObjectManager.Player.Distance(position, true) < spell.Range * spell.Range) &&
                ObjectManager.Player.Spellbook.CastSpell(spell.Slot, position.To3D());
        }

        public static bool Cast(this SummonerSpell spell, Vector3 position)
        {
            return IsReady(spell) &&
                ObjectManager.Player.Spellbook.CastSpell(spell.Slot, position);
        }
    }

    internal class SummonerSpell
    {
        public String[] Names { get; set; }
        public float Range { get; set; }
        public SpellSlot Slot
        {
            get
            {
                foreach (var name in Names.Where(name => ObjectManager.Player.GetSpellSlot(name) != SpellSlot.Unknown))
                {
                    return ObjectManager.Player.GetSpellSlot(name);
                }
                return SpellSlot.Unknown;
            }
        }
    }
}
