// <copyright file="DelayAction.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     The utility class.
    /// </summary>
    public partial class Utility
    {
        /// <summary>
        ///     The delay action class.
        /// </summary>
        public static class DelayAction
        {
            #region Constructors and Destructors

            /// <summary>
            ///     Initializes static members of the <see cref="DelayAction" /> class.
            /// </summary>
            static DelayAction()
            {
                Game.OnUpdate += GameOnOnGameUpdate;
            }

            #endregion

            #region Delegates

            /// <summary>
            ///     The callback.
            /// </summary>
            public delegate void Callback();

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the action list.
            /// </summary>
            public static List<Action> ActionList { get; } = new List<Action>();

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Adds a delay action.
            /// </summary>
            /// <param name="time">
            ///     The time.
            /// </param>
            /// <param name="func">
            ///     The function.
            /// </param>
            public static void Add(int time, Callback func)
            {
                var action = new Action(time, func);
                ActionList.Add(action);
            }

            #endregion

            #region Methods

            private static void GameOnOnGameUpdate(EventArgs args)
            {
                for (var i = ActionList.Count - 1; i >= 0; i--)
                {
                    if (ActionList[i].Time > Utils.GameTimeTickCount)
                    {
                        continue;
                    }

                    try
                    {
                        // Will somehow result in calling ALL non-internal marked classes of the called assembly and causes NullReferenceExceptions.
                        ActionList[i].CallbackObject?.Invoke();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    ActionList.RemoveAt(i);
                }
            }

            #endregion

            /// <summary>
            ///     The action.
            /// </summary>
            public struct Action
            {
                #region Fields

                /// <summary>
                ///     The callback object.
                /// </summary>
                public Callback CallbackObject;

                /// <summary>
                ///     The time to be executed at.
                /// </summary>
                public int Time;

                #endregion

                #region Constructors and Destructors

                /// <summary>
                ///     Initializes a new instance of the <see cref="Action" /> struct.
                /// </summary>
                /// <param name="time">
                ///     The time to be executed at.
                /// </param>
                /// <param name="callback">
                ///     The callback.
                /// </param>
                public Action(int time, Callback callback)
                {
                    this.Time = time + Utils.GameTimeTickCount;
                    this.CallbackObject = callback;
                }

                #endregion
            }
        }
    }
}