// <copyright file="Gapcloser.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    /// <summary>
    ///     The gapcloser.
    /// </summary>
    public class Gapcloser : IGapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Gapcloser" /> class.
        /// </summary>
        /// <param name="championName">
        ///     The champion name.
        /// </param>
        /// <param name="skillType">
        ///     The skill type.
        /// </param>
        /// <param name="slot">
        ///     The slot.
        /// </param>
        /// <param name="spellName">
        ///     The spell name.
        /// </param>
        public Gapcloser(string championName, GapcloserType skillType, SpellSlot slot, string spellName)
        {
            this.ChampionName = championName;
            this.SkillType = skillType;
            this.Slot = slot;
            this.SpellName = spellName;
        }

        #endregion

        #region Public Properties

        /// <inheritdoc />
        public string ChampionName { get; }

        /// <inheritdoc />
        public GapcloserType SkillType { get; }

        /// <inheritdoc />
        public SpellSlot Slot { get; }

        /// <inheritdoc />
        public string SpellName { get; }

        #endregion
    }
}