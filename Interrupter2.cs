using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;

namespace LeagueSharp.Common
{
    public static class Interrupter2
    {
        public delegate void InterruptableTargetHandler(Obj_AI_Hero target, DangerLevel dangerLevel, float endTime);
        public static event InterruptableTargetHandler OnInterruptableTarget;

        public class InterruptableTargetData
        {
            public DangerLevel DangerLevel { get; private set; }
            public float EndTime { get; private set; }
            public bool MovementInterrupts { get; private set; }

            public InterruptableTargetData(DangerLevel dangerLevel, float endTime, bool movementInterrupts)
            {
                DangerLevel = dangerLevel;
                EndTime = endTime;
                MovementInterrupts = movementInterrupts;
            }
        }

        private static Dictionary<string, List<InterruptableSpell>> InterruptableSpells { get; set; }
        // Until jodus improves ObjectManager, we'll use this
        private static List<Obj_AI_Hero> Enemies { get; set; }

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

        private class InterruptableSpell
        {
            public SpellSlot Slot { get; private set; }
            public DangerLevel DangerLevel { get; private set; }
            public bool MovementInterrupts { get; private set; }

            public InterruptableSpell(SpellSlot slot, DangerLevel dangerLevel, bool movementInterrupts = true)
            {
                Slot = slot;
                DangerLevel = dangerLevel;
                MovementInterrupts = movementInterrupts;
            }
        }

        private static void RegisterSpell(string champName, InterruptableSpell spell)
        {
            if (!InterruptableSpells.ContainsKey(champName))
                InterruptableSpells.Add(champName, new List<InterruptableSpell>());

            InterruptableSpells[champName].Add(spell);
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (OnInterruptableTarget != null)
            {
                Enemies.ForEach(enemy =>
                {
                    var newArgs = GetInterruptableTargetData(enemy);
                    if (newArgs != null)
                    {
                        OnInterruptableTarget(enemy, newArgs.DangerLevel, newArgs.EndTime);
                    }
                });
            }
        }

        public enum DangerLevel
        {
            Low,
            Medium,
            High
        }

        public static bool IsCastingInterruptableSpell(this Obj_AI_Hero target, bool checkMovementInterruption = false)
        {
            var data = GetInterruptableTargetData(target);
            return data != null && checkMovementInterruption ? data.MovementInterrupts : true;
        }

        public static InterruptableTargetData GetInterruptableTargetData(Obj_AI_Hero target)
        {
            if (target.IsValid<Obj_AI_Hero>())
            {
                if (target.Spellbook.IsCastingSpell || target.Spellbook.IsChanneling || target.Spellbook.IsCharging)
                {
                    // Check if the target is known to have interruptable spells
                    if (InterruptableSpells.ContainsKey(target.ChampionName))
                    {
                        // Get the interruptable spell
                        var spell = InterruptableSpells[target.ChampionName].Find(s => s.Slot == target.GetSpellSlot(target.LastCastedSpellName()));
                        if (spell != null)
                        {
                            // Return the args with spell end time
                            return new InterruptableTargetData(spell.DangerLevel, target.Spellbook.CastEndTime, spell.MovementInterrupts);
                        }
                    }
                }
            }

            return null;
        }
    }
}
