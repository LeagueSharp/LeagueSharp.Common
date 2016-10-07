// <copyright file="Dash.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.Collections.Generic;
    using System.Linq;

    using SharpDX;

    /// <summary>
    ///     Game dash information parser.
    /// </summary>
    public partial class Dash
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Dash" /> class.
        /// </summary>
        public Dash()
        {
            this.Activate();
        }

        #endregion

        #region Properties

        private IDictionary<int, DashItem> Dashes { get; } = new Dictionary<int, DashItem>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Activates the system.
        /// </summary>
        public void Activate()
        {
            Obj_AI_Base.OnNewPath += this.OnNewPath;
        }

        /// <summary>
        ///     Deactivates the system.
        /// </summary>
        public void Deactivate()
        {
            Obj_AI_Base.OnNewPath -= this.OnNewPath;
        }

        /// <summary>
        ///     Gets the dash info.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="DashItem" />.
        /// </returns>
        public DashItem GetDashInfo(Obj_AI_Base unit)
        {
            DashItem value;
            return this.Dashes.TryGetValue(unit.NetworkId, out value) ? value : new DashItem();
        }

        /// <summary>
        ///     Determines if the unit is dashing.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool IsDashing(Obj_AI_Base unit)
        {
            DashItem value;
            if (this.Dashes.TryGetValue(unit.NetworkId, out value))
            {
                return unit.Path.Length != 0 && value.EndTick != 0;
            }

            return false;
        }

        #endregion

        #region Methods

        private void OnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
        {
            var hero = sender as Obj_AI_Hero;
            if (hero == null)
            {
                return;
            }

            var id = hero.NetworkId;
            if (!this.Dashes.ContainsKey(id))
            {
                this.Dashes[id] = new DashItem();
            }

            if (args.IsDash)
            {
                var path = new List<Vector2> { sender.ServerPosition.To2D() };
                path.AddRange(args.Path.ToList().To2D());

                this.Dashes[id].StartTick = Utils.GameTimeTickCount;
                this.Dashes[id].Speed = args.Speed;
                this.Dashes[id].StartPos = sender.ServerPosition.To2D();
                this.Dashes[id].Unit = sender;
                this.Dashes[id].Path = path;
                this.Dashes[id].EndPos = path.Last();
                this.Dashes[id].EndTick = this.Dashes[id].StartTick
                                          + (int)(1000
                                           * (this.Dashes[id].EndPos.Distance(this.Dashes[id].StartPos)
                                              / this.Dashes[id].Speed));
                this.Dashes[id].Duration = this.Dashes[id].EndTick - this.Dashes[id].StartTick;

                CustomEvents.Unit.TriggerOnDash(sender, this.Dashes[id]);
            }
            else
            {
                this.Dashes[id].EndTick = 0;
            }
        }

        #endregion
    }
}