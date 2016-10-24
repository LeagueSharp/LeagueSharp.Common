// <copyright file="GapcloserZedR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Zed R gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "zedr")]
    public class GapcloserZedR : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserZedR" /> class.
        /// </summary>
        public GapcloserZedR()
            : base("Zed", GapcloserType.Targeted, SpellSlot.R, "zedr")
        {
        }

        #endregion
    }
}