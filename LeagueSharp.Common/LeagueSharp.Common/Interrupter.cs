#region

using System;
using System.Collections.Generic;

#endregion

namespace LeagueSharp.Common
{
    public delegate void OnPosibleToInterruptH(Obj_AI_Base unit, InterruptableSpell spell);

    public enum InterruptableDangerLevel
    {
        Low,
        Medium,
        High,
    }

    public struct InterruptableSpell
    {
        public string BuffName;
        public string ChampionName;
        public InterruptableDangerLevel DangerLevel;
        public SpellSlot Slot;
        public string SpellName;
        public int ExtraDuration;
    }

    /// <summary>
    /// This class allows you to easily interrupt interruptable spells like Katarina's ult.
    /// </summary>
    public static class Interrupter
    {
        public static List<InterruptableSpell> Spells = new List<InterruptableSpell>();

        static Interrupter()
        {

            #region Warwick
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Warwick",
                SpellName = "InfiniteDuress",
                DangerLevel = InterruptableDangerLevel.High,
                Slot = SpellSlot.R,
                BuffName = "infiniteduresssound"
            });
            #endregion
            #region Shen
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Shen",
                SpellName = "ShenStandUnited",
                DangerLevel = InterruptableDangerLevel.High,
                Slot = SpellSlot.R,
                BuffName = "NobodyPlaysShen:^)"
            });
            #endregion
            #region Malzahar
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Malzahar",
                SpellName = "AlZaharNetherGrasp",
                DangerLevel = InterruptableDangerLevel.High,
                Slot = SpellSlot.R,
                BuffName = "alzaharnethergraspsound",
            });
            #endregion
            #region Nunu
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Nunu",
                SpellName = "AbsoluteZero",
                DangerLevel = InterruptableDangerLevel.High,
                Slot = SpellSlot.R,
                BuffName = "AbsoluteZero",
            });
            #endregion
            #region Pantheon
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Pantheon",
                SpellName = "PantheonRJump",
                DangerLevel = InterruptableDangerLevel.High,
                Slot = SpellSlot.R,
                BuffName = "PantheonRJump"
            });
            #endregion
            #region Karthus
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Karthus",
                SpellName = "KarthusFallenOne",
                DangerLevel = InterruptableDangerLevel.High,
                Slot = SpellSlot.R,
                BuffName = "karthusfallenonecastsound"
            });
            #endregion
            #region Velkoz
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Velkoz",
                SpellName = "VelkozR",
                DangerLevel = InterruptableDangerLevel.High,
                Slot = SpellSlot.R,
                BuffName = "VelkozR",
            });
            #endregion
            #region Galio
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Galio",
                SpellName = "GalioIdolOfDurand",
                DangerLevel = InterruptableDangerLevel.High,
                Slot = SpellSlot.R,
                BuffName = "GalioIdolOfDurand",
                ExtraDuration = 200,
            });
            #endregion
            #region MissFortune
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "MissFortune",
                SpellName = "MissFortuneBulletTime",
                DangerLevel = InterruptableDangerLevel.High,
                Slot = SpellSlot.R,
                BuffName = "missfortunebulletsound",
            });
            #endregion
            #region Fiddlesticks
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "FiddleSticks",
                SpellName = "Drain",
                DangerLevel = InterruptableDangerLevel.Medium,
                Slot = SpellSlot.W,
                BuffName = "fearmonger",
            });

            Spells.Add(new InterruptableSpell
            {
                ChampionName = "FiddleSticks",
                SpellName = "Crowstorm",
                DangerLevel = InterruptableDangerLevel.High,
                Slot = SpellSlot.R,
                BuffName = "Crowstorm",
            });
            #endregion
            #region Katarina
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Katarina",
                SpellName = "KatarinaR",
                DangerLevel = InterruptableDangerLevel.High,
                Slot = SpellSlot.R,
                BuffName = "katarinarsound"
            });
            #endregion
            #region MasterYi
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "MasterYi",
                SpellName = "Meditate",
                BuffName = "Meditate",
                Slot = SpellSlot.W,
                DangerLevel = InterruptableDangerLevel.Low,
            });
            #endregion
            #region Xerath
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Xerath",
                SpellName = "XerathLocusOfPower2",
                BuffName = "XerathLocusOfPower2",
                Slot = SpellSlot.R,
                DangerLevel = InterruptableDangerLevel.Low,
            });
            #endregion
            #region Janna
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Janna",
                SpellName = "ReapTheWhirlwind",
                BuffName = "ReapTheWhirlwind",
                Slot = SpellSlot.R,
                DangerLevel = InterruptableDangerLevel.Low,
            });
            #endregion

            Game.OnGameUpdate += Game_OnGameUpdate;
        }


        public static event OnPosibleToInterruptH OnPosibleToInterrupt;

        private static void FireOnInterruptable(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (OnPosibleToInterrupt != null)
                OnPosibleToInterrupt(unit, spell);
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (enemy.IsValidTarget())
                {
                    foreach (var spell in Spells)
                    {
                        if ((enemy.LastCastedspell() != null &&
                             enemy.LastCastedspell().Name.ToLower() == spell.SpellName.ToLower() &&
                             Environment.TickCount - enemy.LastCastedSpellT() < 350) ||
                            (spell.BuffName != null && enemy.HasBuff(spell.BuffName, true)))
                        {
                            FireOnInterruptable(enemy, spell);
                        }
                    }
                }
            }
        }

        public static bool IsChannelingImportantSpell(this Obj_AI_Hero unit)
        {
            foreach (var spell in Spells)
                if (spell.ChampionName == unit.ChampionName)
                    if ((unit.LastCastedspell() != null &&
                         unit.LastCastedspell().Name.ToLower() == spell.SpellName.ToLower() &&
                        Environment.TickCount - unit.LastCastedSpellT() < 350 + spell.ExtraDuration) ||
                        (spell.BuffName != null && unit.HasBuff(spell.BuffName, true)) ||
                        (ObjectManager.Player.NetworkId == unit.NetworkId && LastCastedSpell.LastCastPacketSent != null &&
                         LastCastedSpell.LastCastPacketSent.Slot == spell.Slot &&
                         Environment.TickCount - LastCastedSpell.LastCastPacketSent.Tick < 150 + Game.Ping))
                        return true;

            return false;
        }
    }
}