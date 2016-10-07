// <copyright file="GapcloserIllaoiW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Illaoi W gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "illaoiwattack")]
    public class GapcloserIllaoiW : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserIllaoiW" /> class.
        /// </summary>
        public GapcloserIllaoiW()
            : base("Illaoi", GapcloserType.Targeted, SpellSlot.W, "illaoiwattack")
        {
        }

        #endregion
    }
}