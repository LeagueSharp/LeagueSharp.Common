// <copyright file="GapcloserDianaR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Diana R gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "dianateleport")]
    public class GapcloserDianaR : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserDianaR" /> class.
        /// </summary>
        public GapcloserDianaR()
            : base("Diana", GapcloserType.Targeted, SpellSlot.R, "dianateleport")
        {
        }

        #endregion
    }
}