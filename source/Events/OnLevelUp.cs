// <copyright file="OnLevelUp.cs" company="LeagueSharp">
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
            ///     Leveled up delegate.
            /// </summary>
            /// <param name="sender">
            ///     The sender.
            /// </param>
            /// <param name="args">
            ///     The event arguments.
            /// </param>
            public delegate void OnLeveledUp(Obj_AI_Base sender, OnLevelUpEventArgs args);

            #endregion

            #region Public Events

            /// <summary>
            ///     Level up event.
            /// </summary>
            public static event OnLeveledUp OnLevelUp;

            #endregion

            #region Methods

            private static void InternalOnLevelUp(Obj_AI_Base sender, EventArgs args)
            {
                var level = (sender as Obj_AI_Hero)?.Level ?? (sender as Obj_AI_Minion)?.MinionLevel ?? 1;
                var points = (sender as Obj_AI_Hero)?.SpellTrainingPoints ?? 0;
                OnLevelUp?.Invoke(sender, new OnLevelUpEventArgs(level, level - 1, points));
            }

            #endregion

            /// <summary>
            ///     Level up event arguments.
            /// </summary>
            public class OnLevelUpEventArgs : EventArgs
            {
                #region Constructors and Destructors

                /// <summary>
                ///     Initializes a new instance of the <see cref="OnLevelUpEventArgs" /> class.
                /// </summary>
                /// <param name="newLevel">
                ///     The new level.
                /// </param>
                /// <param name="oldLevel">
                ///     The old level.
                /// </param>
                /// <param name="remainingPoints">
                ///     The remaining points.
                /// </param>
                public OnLevelUpEventArgs(int newLevel, int oldLevel, int remainingPoints)
                {
                    this.NewLevel = newLevel;
                    this.OldLevel = oldLevel;
                    this.RemainingPoints = remainingPoints;
                }

                #endregion

                #region Public Properties

                /// <summary>
                ///     Gets the new level.
                /// </summary>
                public int NewLevel { get; }

                /// <summary>
                ///     Gets the old level.
                /// </summary>
                public int OldLevel { get; }

                /// <summary>
                ///     Gets the remaining points.
                /// </summary>
                public int RemainingPoints { get; }

                #endregion
            }
        }
    }
}