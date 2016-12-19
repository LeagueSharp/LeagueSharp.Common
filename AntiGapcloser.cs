namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using LeagueSharp.Data.DataTypes;

    using SharpDX;

    /// <summary>
    ///     The delegate for the <see cref="AntiGapcloser.OnEnemyGapcloser" /> event.
    /// </summary>
    /// <param name="gapcloser">The gapcloser.</param>
    public delegate void OnGapcloseH(ActiveGapcloser gapcloser);

    /// <summary>
    ///     The type of gapcloser.
    /// </summary>
    public enum GapcloserType
    {
        /// <summary>
        ///     The gapcloser used a skillshot ability.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        Skillshot,

        /// <summary>
        ///     The gapcloser used a targeted ability.
        /// </summary>
        Targeted
    }

    /// <summary>
    ///     Represents a gapcloser.
    /// </summary>
    public struct Gapcloser
    {
        #region Fields

        /// <summary>
        ///     The champion name
        /// </summary>
        public string ChampionName;

        /// <summary>
        ///     The skill type
        /// </summary>
        public GapcloserType SkillType;

        /// <summary>
        ///     The slot
        /// </summary>
        public SpellSlot Slot;

        /// <summary>
        ///     The spell name
        /// </summary>
        public string SpellName;

        #endregion
    }

    /// <summary>
    ///     Represents an active gapcloser.
    /// </summary>
    public struct ActiveGapcloser
    {
        #region Fields

        /// <summary>
        ///     The end
        /// </summary>
        public Vector3 End;

        /// <summary>
        ///     The sender
        /// </summary>
        public Obj_AI_Hero Sender;

        /// <summary>
        ///     The skill type
        /// </summary>
        public GapcloserType SkillType;

        /// <summary>
        ///     The slot
        /// </summary>
        public SpellSlot Slot;

        /// <summary>
        ///     The start
        /// </summary>
        public Vector3 Start;

        /// <summary>
        ///     The tick count
        /// </summary>
        public int TickCount;

        #endregion
    }

    /// <summary>
    ///     Provides events and information about gapclosers.
    /// </summary>
    public static class AntiGapcloser
    {
        #region Static Fields

        /// <summary>
        ///     The active gapclosers
        /// </summary>
        public static List<ActiveGapcloser> ActiveGapclosers = new List<ActiveGapcloser>();

        /// <summary>
        ///     The gapcloser spells
        /// </summary>
        public static List<Gapcloser> Spells;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="AntiGapcloser" /> class.
        /// </summary>
        static AntiGapcloser()
        {
            Spells =
                LeagueSharp.Data.Data.Get<GapcloserData>()
                    .SpellsList.Select(
                        x =>
                        new Gapcloser
                            {
                                ChampionName = x.Key,
                                SkillType =
                                    x.Value.SkillType == LeagueSharp.Data.Enumerations.GapcloserType.Skillshot
                                        ? GapcloserType.Skillshot
                                        : GapcloserType.Targeted,
                                Slot = x.Value.Slot, SpellName = x.Value.SpellName
                            })
                    .ToList();

            Initialize();
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     Occurs on an incoming enemy gapcloser.
        /// </summary>
        public static event OnGapcloseH OnEnemyGapcloser;

        #endregion

        #region Public Methods and Operators

        public static void Initialize()
        {
            Game.OnUpdate += Game_OnGameUpdate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        public static void Shutdown()
        {
            Game.OnUpdate -= Game_OnGameUpdate;
            Obj_AI_Base.OnProcessSpellCast -= Obj_AI_Base_OnProcessSpellCast;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Fired when the game updates.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void Game_OnGameUpdate(EventArgs args)
        {
            ActiveGapclosers.RemoveAll(entry => Utils.TickCount > entry.TickCount + 900);
            if (OnEnemyGapcloser == null)
            {
                return;
            }

            foreach (var gapcloser in
                ActiveGapclosers.Where(gapcloser => gapcloser.Sender.IsValidTarget())
                    .Where(
                        gapcloser =>
                        gapcloser.SkillType == GapcloserType.Targeted
                        || (gapcloser.SkillType == GapcloserType.Skillshot
                            && ObjectManager.Player.Distance(gapcloser.Sender, true) < 250000))) // 500 * 500
            {
                OnEnemyGapcloser(gapcloser);
            }
        }

        /// <summary>
        ///     Fired when the game processes a spell cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!SpellIsGapcloser(args))
            {
                return;
            }

            ActiveGapclosers.Add(
                new ActiveGapcloser
                    {
                        Start = args.Start, End = args.End, Sender = (Obj_AI_Hero)sender, TickCount = Utils.TickCount,
                        SkillType =
                            (args.Target != null && args.Target.IsMe) ? GapcloserType.Targeted : GapcloserType.Skillshot,
                        Slot = ((Obj_AI_Hero)sender).GetSpellSlot(args.SData.Name)
                    });
        }

        /// <summary>
        ///     Checks if a spell is a gapcloser.
        /// </summary>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        /// <returns></returns>
        private static bool SpellIsGapcloser(GameObjectProcessSpellCastEventArgs args)
        {
            return Spells.Any(spell => spell.SpellName == args.SData.Name.ToLower());
        }

        #endregion
    }
}