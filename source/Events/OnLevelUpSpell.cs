// <copyright file="OnLevelUpSpell.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;

    /// <summary>
    ///     Provides custom defined events.
    /// </summary>
    public static partial class CustomEvents
    {
        /// <summary>
        ///     The unit events.
        /// </summary>
        public partial class Unit
        {
            #region Delegates

            /// <summary>
            ///     The level up spell delegate.
            /// </summary>
            /// <param name="sender">
            ///     The sender.
            /// </param>
            /// <param name="args">
            ///     The args.
            /// </param>
            public delegate void OnLeveledUpSpell(Obj_AI_Base sender, OnLevelUpSpellEventArgs args);

            #endregion

            #region Public Events

            /// <summary>
            ///     The level up spell event.
            /// </summary>
            public static event OnLeveledUpSpell OnLevelUpSpell;

            #endregion

            /// <summary>
            ///     Level up spell event args.
            /// </summary>
            public class OnLevelUpSpellEventArgs : EventArgs
            {
                #region Fields

                /// <summary>
                ///     The remaining points.
                /// </summary>
                [Obsolete]
                public int Remainingpoints;

                #endregion

                #region Constructors and Destructors

                /// <summary>
                ///     Initializes a new instance of the <see cref="OnLevelUpSpellEventArgs" /> class.
                /// </summary>
                /// <param name="remainingPoints">
                ///     The remaining points.
                /// </param>
                /// <param name="spellId">
                ///     The spell id.
                /// </param>
                /// <param name="spellLevel">
                ///     The spell level.
                /// </param>
                public OnLevelUpSpellEventArgs(int remainingPoints, int spellId, int spellLevel)
                {
                    this.RemainingPoints = remainingPoints;
                    this.SpellId = spellId;
                    this.SpellLevel = spellLevel;
                }

                #endregion

                #region Public Properties

                /// <summary>
                ///     Gets the remaining points.
                /// </summary>
                public int RemainingPoints { get; }

                /// <summary>
                ///     Gets the spell id.
                /// </summary>
                public int SpellId { get; }

                /// <summary>
                ///     Gets the spell level.
                /// </summary>
                public int SpellLevel { get; }

                #endregion
            }
        }
    }
}