#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Interrupter.cs is part of LeagueSharp.Common.
 
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

#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    /// Delegate for the event <see cref="Interrupter.OnPossibleToInterrupt"/>
    /// </summary>
    /// <param name="unit">The unit.</param>
    /// <param name="spell">The spell.</param>
    public delegate void OnPossibleToInterruptH(Obj_AI_Hero unit, InterruptableSpell spell);

    /// <summary>
    /// The danger level.
    /// </summary>
    public enum InterruptableDangerLevel
    {
        /// <summary>
        /// The low
        /// </summary>
        Low,

        /// <summary>
        /// The medium
        /// </summary>
        Medium,

        /// <summary>
        /// The high
        /// </summary>
        High,
    }

    /// <summary>
    /// Represents an interruptable spell.
    /// </summary>
    public struct InterruptableSpell
    {
        /// <summary>
        /// The buff name
        /// </summary>
        public string BuffName;

        /// <summary>
        /// The champion name
        /// </summary>
        public string ChampionName;

        /// <summary>
        /// The danger level
        /// </summary>
        public InterruptableDangerLevel DangerLevel;

        /// <summary>
        /// The extra duration
        /// </summary>
        public int ExtraDuration;

        /// <summary>
        /// The slot
        /// </summary>
        public SpellSlot Slot;

        /// <summary>
        /// The spell name
        /// </summary>
        public string SpellName;
    }

    /// <summary>
    ///     This class allows you to easily interrupt interruptable spells like Katarina's ult.
    /// </summary>
    [Obsolete("Use Interrupter2", false)]
    public static class Interrupter
    {
        /// <summary>
        /// The spells
        /// </summary>
        public static List<InterruptableSpell> Spells = new List<InterruptableSpell>();

        /// <summary>
        /// Initializes static members of the <see cref="Interrupter"/> class. 
        /// </summary>
        static Interrupter()
        {
            #region Varus

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Varus",
                    SpellName = "VarusQ",
                    DangerLevel = InterruptableDangerLevel.Low,
                    Slot = SpellSlot.Q,
                    BuffName = "VarusQ"
                });

            #endregion

            #region Urgot

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Urgot",
                    SpellName = "UrgotSwap2",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "UrgotSwap2"
                });

            #endregion

            #region Caitlyn

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Caitlyn",
                    SpellName = "CaitlynAceintheHole",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "CaitlynAceintheHole",
                    ExtraDuration = 600
                });

            #endregion

            #region Warwick

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Warwick",
                    SpellName = "InfiniteDuress",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "infiniteduresssound"
                });

            #endregion

            #region Shen

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Shen",
                    SpellName = "ShenStandUnited",
                    DangerLevel = InterruptableDangerLevel.Low,
                    Slot = SpellSlot.R,
                    BuffName = "shenstandunitedlock"
                });

            #endregion

            #region Malzahar

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Malzahar",
                    SpellName = "AlZaharNetherGrasp",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "alzaharnethergraspsound",
                    ExtraDuration = 2000
                });

            #endregion

            #region Nunu

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Nunu",
                    SpellName = "AbsoluteZero",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "AbsoluteZero",
                });

            #endregion

            #region Pantheon

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Pantheon",
                    SpellName = "PantheonRJump",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "PantheonRJump"
                });

            #endregion

            #region Karthus

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Karthus",
                    SpellName = "KarthusFallenOne",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "karthusfallenonecastsound"
                });

            #endregion

            #region Velkoz

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Velkoz",
                    SpellName = "VelkozR",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "VelkozR",
                });

            #endregion

            #region Galio

            Spells.Add(
                new InterruptableSpell
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

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "MissFortune",
                    SpellName = "MissFortuneBulletTime",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "missfortunebulletsound",
                });

            #endregion

            #region Fiddlesticks

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "FiddleSticks",
                    SpellName = "Drain",
                    DangerLevel = InterruptableDangerLevel.Medium,
                    Slot = SpellSlot.W,
                    BuffName = "Drain",
                });
                //Max rank Drain had different buff name
            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "FiddleSticks",
                    SpellName = "Drain",
                    DangerLevel = InterruptableDangerLevel.Medium,
                    Slot = SpellSlot.W,
                    BuffName = "fearmonger_marker",
                });
                /*  Crowstorm buffname only appears after finish casting.
            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "FiddleSticks",
                    SpellName = "Crowstorm",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "Crowstorm",
                });*/

            #endregion

            #region Katarina

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Katarina",
                    SpellName = "KatarinaR",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "katarinarsound"
                });

            #endregion

            #region MasterYi

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "MasterYi",
                    SpellName = "Meditate",
                    BuffName = "Meditate",
                    Slot = SpellSlot.W,
                    DangerLevel = InterruptableDangerLevel.Low,
                });

            #endregion

            #region Xerath

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Xerath",
                    SpellName = "XerathLocusOfPower2",
                    BuffName = "XerathLocusOfPower2",
                    Slot = SpellSlot.R,
                    DangerLevel = InterruptableDangerLevel.Low,
                });

            #endregion

            #region Janna

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Janna",
                    SpellName = "ReapTheWhirlwind",
                    BuffName = "ReapTheWhirlwind",
                    Slot = SpellSlot.R,
                    DangerLevel = InterruptableDangerLevel.Low,
                });

            #endregion

            #region Lucian

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Lucian",
                    SpellName = "LucianR",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "LucianR"
                });

            #endregion

            #region TwistedFate

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "TwistedFate",
                    SpellName = "Destiny",
                    DangerLevel = InterruptableDangerLevel.Medium,
                    Slot = SpellSlot.R,
                    BuffName = "Destiny"
                });

            #endregion

            Game.OnUpdate += Game_OnGameUpdate;
        }

        [Obsolete("Use Interrupter2.OnInterruptableTarget", false)]
        public static event OnPossibleToInterruptH OnPossibleToInterrupt;

        /// <summary>
        /// Fires the on interruptable event.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="spell">The spell.</param>
        private static void FireOnInterruptable(Obj_AI_Hero unit, InterruptableSpell spell)
        {
            if (OnPossibleToInterrupt != null)
            {
                OnPossibleToInterrupt(unit, spell);
            }
        }

        /// <summary>
        /// Fired when the game updates.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void Game_OnGameUpdate(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(e => e.IsValidTarget()))
            {
                foreach (var spell in
                    Spells.Where(
                        spell =>
                            (enemy.LastCastedspell() != null &&
                             String.Equals(
                                 enemy.LastCastedspell().Name, spell.SpellName,
                                 StringComparison.CurrentCultureIgnoreCase) &&
                             Utils.TickCount - enemy.LastCastedSpellT() < 350 + spell.ExtraDuration) ||
                            (!string.IsNullOrEmpty(spell.BuffName) && enemy.HasBuff(spell.BuffName))))
                {
                    FireOnInterruptable(enemy, spell);
                }
            }
        }

        /// <summary>
        /// Determines whether the unit is channeling an important spell.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static bool IsChannelingImportantSpell(this Obj_AI_Hero unit)
        {
            return
                Spells.Any(
                    spell =>
                        spell.ChampionName == unit.ChampionName &&
                        ((unit.LastCastedspell() != null &&
                            String.Equals(
                                unit.LastCastedspell().Name, spell.SpellName, StringComparison.CurrentCultureIgnoreCase) &&
                            Utils.TickCount - unit.LastCastedSpellT() < 350 + spell.ExtraDuration) ||
                        (spell.BuffName != null && unit.HasBuff(spell.BuffName, true)) ||
                        (unit.IsMe &&
                            LastCastedSpell.LastCastPacketSent != null &&
                            LastCastedSpell.LastCastPacketSent.Slot == spell.Slot &&
                            Utils.TickCount - LastCastedSpell.LastCastPacketSent.Tick < 150 + Game.Ping)));
        }
    }
}
