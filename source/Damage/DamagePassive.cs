// <copyright file="DamagePassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    using LeagueSharp.Common.Passives;

    /// <summary>
    ///     Damage calculations and data.
    /// </summary>
    public partial class Damage
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the spells.
        /// </summary>
        [ImportMany]
        public IEnumerable<Lazy<IPassiveDamage, IPassiveDamageMetadata>> PassiveLazies { get; protected set; }

        #endregion

        #region Methods

        internal static float GetCritMultiplier(Obj_AI_Hero hero, bool checkCrit = false)
        {
            var crit = Items.HasItem((int)ItemId.Infinity_Edge, hero) ? 1.5f : 1;
            return !checkCrit ? crit : (Math.Abs(hero.Crit - 1) < float.Epsilon ? 1 + crit : 1);
        }

        #endregion
    }
}