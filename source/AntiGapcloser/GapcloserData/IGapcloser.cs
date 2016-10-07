// <copyright file="IGapcloser.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    /// <summary>
    ///     The gapcloser interface.
    /// </summary>
    public interface IGapcloser
    {
        #region Public Properties

        /// <summary>
        ///     Gets the champion name.
        /// </summary>
        string ChampionName { get; }

        /// <summary>
        ///     Gets the skill type.
        /// </summary>
        GapcloserType SkillType { get; }

        /// <summary>
        ///     Gets the spell slot.
        /// </summary>
        SpellSlot Slot { get; }

        /// <summary>
        ///     Gets the spell name.
        /// </summary>
        string SpellName { get; }

        #endregion
    }
}