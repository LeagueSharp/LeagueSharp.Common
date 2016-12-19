namespace LeagueSharp.Common
{
    using System.Collections.Generic;

    /// <summary>
    ///     Represents a last casted spell.
    /// </summary>
    public class LastCastedSpellEntry
    {
        #region Fields

        /// <summary>
        ///     The name
        /// </summary>
        public string Name;

        /// <summary>
        ///     The target
        /// </summary>
        public Obj_AI_Base Target;

        /// <summary>
        ///     The tick
        /// </summary>
        public int Tick;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LastCastedSpellEntry" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="tick">The tick.</param>
        /// <param name="target">The target.</param>
        public LastCastedSpellEntry(string name, int tick, Obj_AI_Base target)
        {
            this.Name = name;
            this.Tick = tick;
            this.Target = target;
        }

        #endregion
    }

    /// <summary>
    ///     Represents the last cast packet sent.
    /// </summary>
    public class LastCastPacketSentEntry
    {
        #region Fields

        /// <summary>
        ///     The slot
        /// </summary>
        public SpellSlot Slot;

        /// <summary>
        ///     The target network identifier
        /// </summary>
        public int TargetNetworkId;

        /// <summary>
        ///     The tick
        /// </summary>
        public int Tick;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LastCastPacketSentEntry" /> class.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <param name="tick">The tick.</param>
        /// <param name="targetNetworkId">The target network identifier.</param>
        public LastCastPacketSentEntry(SpellSlot slot, int tick, int targetNetworkId)
        {
            this.Slot = slot;
            this.Tick = tick;
            this.TargetNetworkId = targetNetworkId;
        }

        #endregion
    }

    /// <summary>
    ///     Gets the last casted spell of the unit.
    /// </summary>
    public static class LastCastedSpell
    {
        #region Static Fields

        /// <summary>
        ///     The last cast packet sent
        /// </summary>
        public static LastCastPacketSentEntry LastCastPacketSent;

        /// <summary>
        ///     The casted spells
        /// </summary>
        internal static readonly Dictionary<int, LastCastedSpellEntry> CastedSpells =
            new Dictionary<int, LastCastedSpellEntry>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="LastCastedSpell" /> class.
        /// </summary>
        static LastCastedSpell()
        {
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Spellbook.OnCastSpell += SpellbookOnCastSpell;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the last casted spell.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static LastCastedSpellEntry LastCastedspell(this Obj_AI_Hero unit)
        {
            return CastedSpells.ContainsKey(unit.NetworkId) ? CastedSpells[unit.NetworkId] : null;
        }

        /// <summary>
        ///     Gets the last casted spell name.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static string LastCastedSpellName(this Obj_AI_Hero unit)
        {
            return CastedSpells.ContainsKey(unit.NetworkId) ? CastedSpells[unit.NetworkId].Name : string.Empty;
        }

        /// <summary>
        ///     Gets the last casted spell tick.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static int LastCastedSpellT(this Obj_AI_Hero unit)
        {
            return CastedSpells.ContainsKey(unit.NetworkId)
                       ? CastedSpells[unit.NetworkId].Tick
                       : (Utils.TickCount > 0 ? 0 : int.MinValue);
        }

        /// <summary>
        ///     Gets the last casted spell's target.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Obj_AI_Base LastCastedSpellTarget(this Obj_AI_Hero unit)
        {
            return CastedSpells.ContainsKey(unit.NetworkId) ? CastedSpells[unit.NetworkId].Target : null;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Fired when the game processes the spell cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender is Obj_AI_Hero)
            {
                var entry = new LastCastedSpellEntry(args.SData.Name, Utils.TickCount, ObjectManager.Player);
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

        /// <summary>
        ///     Fired then a spell is casted.
        /// </summary>
        /// <param name="spellbook">The spellbook.</param>
        /// <param name="args">The <see cref="SpellbookCastSpellEventArgs" /> instance containing the event data.</param>
        static void SpellbookOnCastSpell(Spellbook spellbook, SpellbookCastSpellEventArgs args)
        {
            if (spellbook.Owner.IsMe)
            {
                LastCastPacketSent = new LastCastPacketSentEntry(
                    args.Slot,
                    Utils.TickCount,
                    (args.Target is Obj_AI_Base) ? args.Target.NetworkId : 0);
            }
        }

        #endregion
    }
}