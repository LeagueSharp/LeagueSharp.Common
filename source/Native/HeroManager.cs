// <copyright file="HeroManager.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    /// <summary>
    ///     Provides cached heroes.
    /// </summary>
    [Export(typeof(HeroManager))]
    public class HeroManager
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HeroManager" /> class.
        /// </summary>
        public HeroManager()
        {
            this.All = ObjectManager.Get<Obj_AI_Hero>().ToList();
            this.Ally = this.All.FindAll(o => o.IsAlly);
            this.Enemy = this.All.FindAll(o => o.IsEnemy);
            this.LocalPlayer = this.All.Find(x => x.IsMe);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets all heroes.
        /// </summary>
        /// <value>
        ///     All heroes.
        /// </value>
        public static List<Obj_AI_Hero> AllHeroes => Instance?.All ?? new List<Obj_AI_Hero>();

        /// <summary>
        ///     Gets the allies.
        /// </summary>
        /// <value>
        ///     The allies.
        /// </value>
        public static List<Obj_AI_Hero> Allies => Instance?.Ally ?? new List<Obj_AI_Hero>();

        /// <summary>
        ///     Gets the enemies.
        /// </summary>
        /// <value>
        ///     The enemies.
        /// </value>
        public static List<Obj_AI_Hero> Enemies => Instance?.Enemy ?? new List<Obj_AI_Hero>();

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static HeroManager Instance => Instances.HeroManager;

        /// <summary>
        ///     Gets the Local Player
        /// </summary>
        public static Obj_AI_Hero Player => Instance.LocalPlayer;

        /// <summary>
        ///     Gets all of the heroes.
        /// </summary>
        public List<Obj_AI_Hero> All { get; }

        /// <summary>
        ///     Gets all of the ally heroes.
        /// </summary>
        public List<Obj_AI_Hero> Ally { get; }

        /// <summary>
        ///     Gets all of the enemies.
        /// </summary>
        public List<Obj_AI_Hero> Enemy { get; }

        /// <summary>
        ///     Gets the local player
        /// </summary>
        public Obj_AI_Hero LocalPlayer { get; }

        #endregion
    }
}