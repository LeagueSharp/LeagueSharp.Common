namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Provides cached heroes.
    /// </summary>
    public class HeroManager
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="HeroManager" /> class.
        /// </summary>
        static HeroManager()
        {
            if (Game.Mode == GameMode.Running)
            {
                Game_OnStart(new EventArgs());
            }
            Game.OnStart += Game_OnStart;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets all heroes.
        /// </summary>
        /// <value>
        ///     All heroes.
        /// </value>
        public static List<Obj_AI_Hero> AllHeroes { get; private set; }

        /// <summary>
        ///     Gets the allies.
        /// </summary>
        /// <value>
        ///     The allies.
        /// </value>
        public static List<Obj_AI_Hero> Allies { get; private set; }

        /// <summary>
        ///     Gets the enemies.
        /// </summary>
        /// <value>
        ///     The enemies.
        /// </value>
        public static List<Obj_AI_Hero> Enemies { get; private set; }

        /// <summary>
        ///     Gets the Local Player
        /// </summary>
        public static Obj_AI_Hero Player { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Fired when the game starts.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        static void Game_OnStart(EventArgs args)
        {
            AllHeroes = ObjectManager.Get<Obj_AI_Hero>().ToList();
            Allies = AllHeroes.FindAll(o => o.IsAlly);
            Enemies = AllHeroes.FindAll(o => o.IsEnemy);
            Player = AllHeroes.Find(x => x.IsMe);
        }

        #endregion
    }
}