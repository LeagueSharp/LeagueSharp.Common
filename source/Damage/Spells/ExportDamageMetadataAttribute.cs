// <copyright file="ExportDamageMetadataAttribute.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Exports the damage metadata.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    [MetadataAttribute]
    public class ExportDamageMetadataAttribute : ExportAttribute, IDamageSpellMetadata
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExportDamageMetadataAttribute" /> class.
        /// </summary>
        /// <param name="championName">
        ///     The champion name.
        /// </param>
        /// <param name="slot">
        ///     The slot.
        /// </param>
        /// <param name="stage">
        ///     The stage.
        /// </param>
        public ExportDamageMetadataAttribute(string championName, SpellSlot slot, int stage = 0)
            : base(typeof(IDamageSpellMetadata))
        {
            this.ChampionName = championName;
            this.SpellSlot = slot;
            this.Stage = stage;
        }

        #endregion

        #region Public Properties

        /// <inheritdoc />
        public string ChampionName { get; }

        /// <inheritdoc />
        public SpellSlot SpellSlot { get; }

        /// <inheritdoc />
        public int Stage { get; }

        #endregion
    }
}