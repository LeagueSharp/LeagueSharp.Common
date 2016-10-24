// <copyright file="ActiveGapcloser.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using SharpDX;

    /// <summary>
    ///     The active gapcloser info.
    /// </summary>
    public struct ActiveGapcloser
    {
        #region Fields

        /// <summary>
        ///     The gapcloser ending position.
        /// </summary>
        public Vector3 End;

        /// <summary>
        ///     The sender, gapcloser entity object.
        /// </summary>
        public Obj_AI_Hero Sender;

        /// <summary>
        ///     The skill type.
        /// </summary>
        public GapcloserType SkillType;

        /// <summary>
        ///     The spell slot.
        /// </summary>
        public SpellSlot Slot;

        /// <summary>
        ///     The gapcloser starting position.
        /// </summary>
        public Vector3 Start;

        /// <summary>
        ///     The starting tick count.
        /// </summary>
        public int TickCount;

        #endregion
    }
}