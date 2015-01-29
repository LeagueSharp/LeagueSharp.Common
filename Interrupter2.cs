#region LICENSE

/*
 Copyright 2014 - 2015 LeagueSharp
 Interrupter2.cs is part of LeagueSharp.Common.
 
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

#endregion

namespace LeagueSharp.Common
{
    public static class Interrupter2
    {
        public delegate void InterruptableTargetHandler(Obj_AI_Hero sender, InterruptableTargetEventArgs args);

        public enum DangerLevel
        {
            Low,
            Medium,
            High
        }

        static Interrupter2()
        {
            // Initialize Properties
            InterruptableSpells = new Dictionary<string, List<InterruptableSpell>>();
            Enemies = ObjectManager.Get<Obj_AI_Hero>().FindAll(o => o.IsEnemy);

            InitializeSpells();

            // Trigger LastCastedSpell
            ObjectManager.Player.LastCastedspell();

            // Listen to required events
            Game.OnGameUpdate += Game_OnGameUpdate;
        }

        private static Dictionary<string, List<InterruptableSpell>> InterruptableSpells { get; set; }
        // Until jodus improves ObjectManager, we'll use this
        private static List<Obj_AI_Hero> Enemies { get; set; }
        public static event InterruptableTargetHandler OnInterruptableTarget;

        private static void InitializeSpells()
        {
            #region Spells

            RegisterSpell("Caitlyn", new InterruptableSpell(SpellSlot.R, DangerLevel.High));
            RegisterSpell("FiddleSticks", new InterruptableSpell(SpellSlot.W, DangerLevel.Medium));
            RegisterSpell("FiddleSticks", new InterruptableSpell(SpellSlot.R, DangerLevel.High));
            RegisterSpell("Galio", new InterruptableSpell(SpellSlot.R, DangerLevel.High));
            RegisterSpell("Janna", new InterruptableSpell(SpellSlot.R, DangerLevel.Low));
            RegisterSpell("Karthus", new InterruptableSpell(SpellSlot.R, DangerLevel.High));
            RegisterSpell("Katarina", new InterruptableSpell(SpellSlot.R, DangerLevel.High));
            RegisterSpell("Lucian", new InterruptableSpell(SpellSlot.R, DangerLevel.High, false));
            RegisterSpell("Malzahar", new InterruptableSpell(SpellSlot.R, DangerLevel.High));
            RegisterSpell("MasterYi", new InterruptableSpell(SpellSlot.W, DangerLevel.Low));
            RegisterSpell("MissFortune", new InterruptableSpell(SpellSlot.R, DangerLevel.High));
            RegisterSpell("Nunu", new InterruptableSpell(SpellSlot.R, DangerLevel.High));
            RegisterSpell("Pantheon", new InterruptableSpell(SpellSlot.R, DangerLevel.High));
            RegisterSpell("Shen", new InterruptableSpell(SpellSlot.R, DangerLevel.Low));
            RegisterSpell("TwistedFate", new InterruptableSpell(SpellSlot.R, DangerLevel.Medium));
            RegisterSpell("Urgot", new InterruptableSpell(SpellSlot.R, DangerLevel.High));
            RegisterSpell("Velkoz", new InterruptableSpell(SpellSlot.R, DangerLevel.High));
            RegisterSpell("Warwick", new InterruptableSpell(SpellSlot.R, DangerLevel.High));
            RegisterSpell("Xerath", new InterruptableSpell(SpellSlot.R, DangerLevel.High));
            RegisterSpell("Varus", new InterruptableSpell(SpellSlot.Q, DangerLevel.Low, false));

            #endregion
        }

        private static void RegisterSpell(string champName, InterruptableSpell spell)
        {
            if (!InterruptableSpells.ContainsKey(champName))
            {
                InterruptableSpells.Add(champName, new List<InterruptableSpell>());
            }

            InterruptableSpells[champName].Add(spell);
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (OnInterruptableTarget != null)
            {
                Enemies.ForEach(
                    enemy =>
                    {
                        var newArgs = GetInterruptableTargetData(enemy);
                        if (newArgs != null)
                        {
                            OnInterruptableTarget(enemy, newArgs);
                        }
                    });
            }
        }

        public static bool IsCastingInterruptableSpell(this Obj_AI_Hero target, bool checkMovementInterruption = false)
        {
            var data = GetInterruptableTargetData(target);
            return data != null && (!checkMovementInterruption || data.MovementInterrupts);
        }

        public static InterruptableTargetEventArgs GetInterruptableTargetData(Obj_AI_Hero target)
        {
            if (target.IsValid<Obj_AI_Hero>())
            {
                if (target.Spellbook.IsCastingSpell || target.Spellbook.IsChanneling || target.Spellbook.IsCharging)
                {
                    // Check if the target is known to have interruptable spells
                    if (InterruptableSpells.ContainsKey(target.ChampionName))
                    {
                        // Get the interruptable spell
                        var spell =
                            InterruptableSpells[target.ChampionName].Find(
                                s => s.Slot == target.GetSpellSlot(target.LastCastedSpellName()));
                        if (spell != null)
                        {
                            // Return the args with spell end time
                            return new InterruptableTargetEventArgs(
                                spell.DangerLevel, target.Spellbook.CastEndTime, spell.MovementInterrupts);
                        }
                    }
                }
            }

            return null;
        }

        public class InterruptableTargetEventArgs
        {
            public InterruptableTargetEventArgs(DangerLevel dangerLevel, float endTime, bool movementInterrupts)
            {
                DangerLevel = dangerLevel;
                EndTime = endTime;
                MovementInterrupts = movementInterrupts;
            }

            private DangerLevel DangerLevel { get; set; }
            private float EndTime { get; set; }
            public bool MovementInterrupts { get; private set; }
        }

        private class InterruptableSpell
        {
            public InterruptableSpell(SpellSlot slot, DangerLevel dangerLevel, bool movementInterrupts = true)
            {
                Slot = slot;
                DangerLevel = dangerLevel;
                MovementInterrupts = movementInterrupts;
            }

            public SpellSlot Slot { get; private set; }
            private DangerLevel DangerLevel { get; set; }
            private bool MovementInterrupts { get; set; }
        }
    }
}