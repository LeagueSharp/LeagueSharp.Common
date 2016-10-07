// <copyright file="UnitEvents.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Provides custom defined events.
    /// </summary>
    public static partial class CustomEvents
    {
        /// <summary>
        ///     The unit events.
        /// </summary>
        [Export(typeof(Unit))]
        public partial class Unit
        {
            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="Unit" /> class.
            /// </summary>
            public Unit()
            {
                Obj_AI_Base.OnLevelUp += InternalOnLevelUp;
            }

            #endregion
        }
    }
}