#region

using System;
using System.Collections.Generic;
using Leaguesharp.Common;

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
        public string ChampionName;
        public InterruptableDangerLevel DangerLevel;
        public string SpellName;
    }

    /// <summary>
    /// This class allows you to easily interrupt interruptable spells like Katarina's ult.
    /// </summary>
    public static class Interrupter
    {
        public static List<InterruptableSpell> Spells = new List<InterruptableSpell>();

        static Interrupter()
        {
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Warwick",
                SpellName = "InfiniteDuress",
                DangerLevel = InterruptableDangerLevel.High,
            });
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Shen",
                SpellName = "ShenStandUnited",
                DangerLevel = InterruptableDangerLevel.High,
            });
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Malzahar",
                SpellName = "AlZaharNetherGrasp",
                DangerLevel = InterruptableDangerLevel.High,
            });
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Nunu",
                SpellName = "AbsoluteZero",
                DangerLevel = InterruptableDangerLevel.High,
            });
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Pantheon",
                SpellName = "Pantheon_GrandSkyfall_Jump",
                DangerLevel = InterruptableDangerLevel.High,
            });
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Karthus",
                SpellName = "FallenOne",
                DangerLevel = InterruptableDangerLevel.High,
            });
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Velkoz",
                SpellName = "VelkozR",
                DangerLevel = InterruptableDangerLevel.High,
            });
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Galio",
                SpellName = "GalioIdolOfDurand",
                DangerLevel = InterruptableDangerLevel.High,
            });
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "MissFortune",
                SpellName = "MissFortuneBulletTime",
                DangerLevel = InterruptableDangerLevel.High,
            });
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "FiddleSticks",
                SpellName = "Drain",
                DangerLevel = InterruptableDangerLevel.Medium,
            });
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "FiddleSticks",
                SpellName = "Crowstorm",
                DangerLevel = InterruptableDangerLevel.High,
            });
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "Katarina",
                SpellName = "KatarinaR",
                DangerLevel = InterruptableDangerLevel.High,
            });
            Spells.Add(new InterruptableSpell
            {
                ChampionName = "MasterYi",
                SpellName = "Meditate",
                DangerLevel = InterruptableDangerLevel.Low,
            });

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
                if (enemy.IsValidTarget() && enemy.IsChanneling)
                {
                    foreach (var spell in Spells)
                    {
                        if (enemy.LastCastedspell().Name.ToLower() == spell.SpellName.ToLower() &&
                            Environment.TickCount - enemy.LastCastedSpellT() < 3000)
                        {
                            FireOnInterruptable(enemy, spell);
                        }
                    }
                }
            }
        }
    }
}