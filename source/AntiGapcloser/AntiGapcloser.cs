// <copyright file="AntiGapcloser.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    using static GapcloserType;

    /// <summary>
    ///     The anti-gapcloser, provides an event about game gapclose.
    /// </summary>
    [Export(typeof(AntiGapcloser))]
    public partial class AntiGapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AntiGapcloser" /> class.
        /// </summary>
        public AntiGapcloser()
        {
            this.LazySpells = new Lazy<IGapcloser, IGapcloserMetadata>[0];
            this.Activate();

            this.Gapcloser += OnGapcloser;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     The gapcloser event.
        /// </summary>
        public event EventHandler<ActiveGapcloser> Gapcloser;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the active gapclosers list.
        /// </summary>
        public List<ActiveGapcloser> ActiveGapclosersList { get; } = new List<ActiveGapcloser>();

        /// <summary>
        ///     Gets a value indicating whether the anti gapcloser tracking system is active.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        ///     Gets or sets the lazy group of spells.
        /// </summary>
        [ImportMany]
        public IEnumerable<Lazy<IGapcloser, IGapcloserMetadata>> LazySpells { get; protected set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Activates the anti gapcloser tracking.
        /// </summary>
        public void Activate()
        {
            this.IsActive = true;
            Game.OnUpdate += this.OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += this.OnProcessSpellCast;
        }

        /// <summary>
        ///     Deactivates the anti gapcloser tracking.
        /// </summary>
        public void Deactivate()
        {
            this.IsActive = false;
            Game.OnUpdate -= this.OnUpdate;
            Obj_AI_Base.OnProcessSpellCast -= this.OnProcessSpellCast;
        }

        #endregion

        #region Methods

        private void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            const StringComparison ComparisonType = StringComparison.CurrentCultureIgnoreCase;
            if (!this.LazySpells.Any(s => string.Equals(s.Metadata.SpellName, args.SData.Name, ComparisonType)))
            {
                return;
            }

            this.ActiveGapclosersList.Add(
                new ActiveGapcloser
                    {
                        Start = args.Start, End = args.End, Sender = sender as Obj_AI_Hero,
                        TickCount = Utils.GameTimeTickCount,
                        SkillType = (GapcloserType)Convert.ToInt32(args.Target != null && args.Target.IsMe),
                        Slot = args.Slot
                    });
        }

        private void OnUpdate(EventArgs args)
        {
            this.ActiveGapclosersList.RemoveAll(entry => Utils.GameTimeTickCount > entry.TickCount + 900);
            if (OnEnemyGapcloser == null)
            {
                return;
            }

            var player = ObjectManager.Player;
            foreach (var gapcloser in this.ActiveGapclosersList.Where(g => g.Sender.IsValid))
            {
                if (gapcloser.SkillType == Targeted
                    || (gapcloser.SkillType == Skillshot && player.InRange(gapcloser.Sender, 500, true)))
                {
                    this.Gapcloser?.Invoke(this, gapcloser);
                }
            }
        }

        #endregion
    }
}