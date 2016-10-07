namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;

    using LeagueSharp.Data.DataTypes;

    /// <summary>
    ///     Provides information an API regarding interruptable spells.
    /// </summary>
    public static class Interrupter2
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="Interrupter2" /> class.
        /// </summary>
        static Interrupter2()
        {
            Initialize();
        }

        #endregion

        #region Delegates

        /// <summary>
        ///     The delegate for <see cref="Interrupter2.OnInterruptableTarget" />.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="InterruptableTargetEventArgs" /> instance containing the event data.</param>
        public delegate void InterruptableTargetHandler(Obj_AI_Hero sender, InterruptableTargetEventArgs args);

        #endregion

        #region Public Events

        /// <summary>
        ///     Occurs on an interruptable target.
        /// </summary>
        public static event InterruptableTargetHandler OnInterruptableTarget;

        #endregion

        #region Enums

        /// <summary>
        ///     The danger level of the interruptable spell.
        /// </summary>
        public enum DangerLevel
        {
            /// <summary>
            ///     The low
            /// </summary>
            Low,

            /// <summary>
            ///     The medium
            /// </summary>
            Medium,

            /// <summary>
            ///     The high
            /// </summary>
            High
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the casting interruptable spell.
        /// </summary>
        /// <value>
        ///     The casting interruptable spell.
        /// </value>
        private static Dictionary<int, InterruptableSpell> CastingInterruptableSpell { get; set; }

        /// <summary>
        ///     Gets or sets the interruptable spells.
        /// </summary>
        /// <value>
        ///     The interruptable spells.
        /// </value>
        private static Dictionary<string, List<InterruptableSpell>> InterruptableSpells { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the interruptable target data.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static InterruptableTargetEventArgs GetInterruptableTargetData(Obj_AI_Hero target)
        {
            if (target.IsValid<Obj_AI_Hero>())
            {
                if (CastingInterruptableSpell.ContainsKey(target.NetworkId))
                {
                    // Return the args with spell end time
                    return new InterruptableTargetEventArgs(
                        CastingInterruptableSpell[target.NetworkId].DangerLevel,
                        target.Spellbook.CastEndTime,
                        CastingInterruptableSpell[target.NetworkId].MovementInterrupts);
                }
            }

            return null;
        }

        public static void Initialize()
        {
            // Initialize Properties
            InterruptableSpells = new Dictionary<string, List<InterruptableSpell>>();
            CastingInterruptableSpell = new Dictionary<int, InterruptableSpell>();

            InitializeSpells();

            // Trigger LastCastedSpell
            ObjectManager.Player.LastCastedspell();

            // Listen to required events
            Game.OnUpdate += Game_OnGameUpdate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Spellbook.OnStopCast += Spellbook_OnStopCast;
        }

        /// <summary>
        ///     Determines whether the target is casting an interruptable spell.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="checkMovementInterruption">if set to <c>true</c> checks if movement interrupts.</param>
        /// <returns></returns>
        public static bool IsCastingInterruptableSpell(this Obj_AI_Hero target, bool checkMovementInterruption = false)
        {
            var data = GetInterruptableTargetData(target);
            return data != null && (!checkMovementInterruption || data.MovementInterrupts);
        }

        public static void Shutdown()
        {
            Game.OnUpdate -= Game_OnGameUpdate;
            Obj_AI_Base.OnProcessSpellCast -= Obj_AI_Base_OnProcessSpellCast;
            Spellbook.OnStopCast -= Spellbook_OnStopCast;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Fired when the game updates.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void Game_OnGameUpdate(EventArgs args)
        {
            // Remove heros that have finished casting their interruptable spell
            HeroManager.AllHeroes.ForEach(
                hero =>
                    {
                        if (CastingInterruptableSpell.ContainsKey(hero.NetworkId) && !hero.Spellbook.IsCastingSpell
                            && !hero.Spellbook.IsChanneling && !hero.Spellbook.IsCharging)
                        {
                            CastingInterruptableSpell.Remove(hero.NetworkId);
                        }
                    });

            // Trigger OnInterruptableTarget event if needed
            if (OnInterruptableTarget != null)
            {
                HeroManager.Enemies.ForEach(
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

        /// <summary>
        ///     Initializes the spells.
        /// </summary>
        private static void InitializeSpells()
        {
            foreach (var keyValuePair in LeagueSharp.Data.Data.Get<InterruptableSpellData>().InterruptableSpells)
            {
                foreach (var spell in keyValuePair.Value)
                {
                    RegisterSpell(
                        keyValuePair.Key,
                        new InterruptableSpell(
                            spell.Slot,
                            (DangerLevel)Enum.Parse(typeof(DangerLevel), spell.DangerLevel.ToString()),
                            spell.MovementInterrupts));
                }
            }
        }

        /// <summary>
        ///     Fired when the game processes spell casts.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var target = sender as Obj_AI_Hero;
            if (target != null && !CastingInterruptableSpell.ContainsKey(target.NetworkId))
            {
                // Check if the target is known to have interruptable spells
                if (InterruptableSpells.ContainsKey(target.ChampionName))
                {
                    // Get the interruptable spell
                    var spell =
                        InterruptableSpells[target.ChampionName].Find(
                            s => s.Slot == target.GetSpellSlot(args.SData.Name));
                    if (spell != null)
                    {
                        // Mark champ as casting interruptable spell
                        CastingInterruptableSpell.Add(target.NetworkId, spell);
                    }
                }
            }
        }

        /// <summary>
        ///     Registers the spell.
        /// </summary>
        /// <param name="champName">Name of the champ.</param>
        /// <param name="spell">The spell.</param>
        private static void RegisterSpell(string champName, InterruptableSpell spell)
        {
            if (!InterruptableSpells.ContainsKey(champName))
            {
                InterruptableSpells.Add(champName, new List<InterruptableSpell>());
            }

            InterruptableSpells[champName].Add(spell);
        }

        /// <summary>
        ///     Fired when the spellbook stops a cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SpellbookStopCastEventArgs" /> instance containing the event data.</param>
        private static void Spellbook_OnStopCast(Spellbook sender, SpellbookStopCastEventArgs args)
        {
            var target = sender.Owner as Obj_AI_Hero;
            if (target != null)
            {
                // Check if the spell itself stopped casting (interrupted)
                if (!target.Spellbook.IsCastingSpell && !target.Spellbook.IsChanneling && !target.Spellbook.IsCharging)
                {
                    CastingInterruptableSpell.Remove(target.NetworkId);
                }
            }
        }

        #endregion

        /// <summary>
        ///     Prevents event arguments for the <see cref="Interrupter2.OnInterruptableTarget" />
        /// </summary>
        public class InterruptableTargetEventArgs : EventArgs
        {
            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="InterruptableTargetEventArgs" /> class.
            /// </summary>
            /// <param name="dangerLevel">The danger level.</param>
            /// <param name="endTime">The end time.</param>
            /// <param name="movementInterrupts">if set to <c>true</c> [movement interrupts].</param>
            public InterruptableTargetEventArgs(DangerLevel dangerLevel, float endTime, bool movementInterrupts)
            {
                this.DangerLevel = dangerLevel;
                this.EndTime = endTime;
                this.MovementInterrupts = movementInterrupts;
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the danger level.
            /// </summary>
            /// <value>
            ///     The danger level.
            /// </value>
            public DangerLevel DangerLevel { get; private set; }

            /// <summary>
            ///     Gets the end time.
            /// </summary>
            /// <value>
            ///     The end time.
            /// </value>
            public float EndTime { get; private set; }

            /// <summary>
            ///     Gets a value indicating whether movement interrupts the channel.
            /// </summary>
            /// <value>
            ///     <c>true</c> if movement interrupts the channel; otherwise, <c>false</c>.
            /// </value>
            public bool MovementInterrupts { get; private set; }

            #endregion
        }

        /// <summary>
        ///     Represents an interruptable spell.
        /// </summary>
        private class InterruptableSpell
        {
            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="InterruptableSpell" /> class.
            /// </summary>
            /// <param name="slot">The slot.</param>
            /// <param name="dangerLevel">The danger level.</param>
            /// <param name="movementInterrupts">if set to <c>true</c> [movement interrupts].</param>
            public InterruptableSpell(SpellSlot slot, DangerLevel dangerLevel, bool movementInterrupts = true)
            {
                this.Slot = slot;
                this.DangerLevel = dangerLevel;
                this.MovementInterrupts = movementInterrupts;
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets or sets the danger level.
            /// </summary>
            /// <value>
            ///     The danger level.
            /// </value>
            public DangerLevel DangerLevel { get; private set; }

            /// <summary>
            ///     Gets or sets a value indicating whether movement interrupts the channel.
            /// </summary>
            /// <value>
            ///     <c>true</c> if movement interrupts the channel; otherwise, <c>false</c>.
            /// </value>
            public bool MovementInterrupts { get; private set; }

            /// <summary>
            ///     Gets or sets the slot.
            /// </summary>
            /// <value>
            ///     The slot.
            /// </value>
            public SpellSlot Slot { get; private set; }

            #endregion
        }
    }
}