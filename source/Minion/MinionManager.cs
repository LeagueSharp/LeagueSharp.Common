// <copyright file="MinionManager.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Provides a minion AI manager.
    /// </summary>
    [Export(typeof(MinionManager))]
    public partial class MinionManager
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MinionManager" /> class.
        /// </summary>
        public MinionManager()
        {
            GameObject.OnCreate += this.OnCreate;
            GameObject.OnDelete += this.OnDelete;
            CustomEvents.Game.OnGameLoad += this.OnLoad;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the minion info collection.
        /// </summary>
        public IDictionary<int, MinionInfo> MinionInfos { get; } = new Dictionary<int, MinionInfo>();

        #endregion

        #region Methods

        private void OnCreate(GameObject sender, EventArgs args)
        {
            var minion = sender as Obj_AI_Minion;
            if (minion != null && !this.MinionInfos.ContainsKey(minion.NetworkId))
            {
                this.MinionInfos.Add(minion.NetworkId, new MinionInfo(minion));
            }
        }

        private void OnDelete(GameObject sender, EventArgs args)
        {
            var minion = sender as Obj_AI_Minion;
            if (minion != null && this.MinionInfos.ContainsKey(minion.NetworkId))
            {
                this.MinionInfos.Remove(minion.NetworkId);
            }
        }

        private void OnLoad(EventArgs eventArgs)
        {
            foreach (var obj in ObjectManager.Get<Obj_AI_Minion>())
            {
                if (!this.MinionInfos.ContainsKey(obj.NetworkId))
                {
                    this.MinionInfos.Add(obj.NetworkId, new MinionInfo(obj));
                }
            }
        }

        #endregion
    }
}