// <copyright file="GapcloserPantheonR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Pantheon R gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "pantheonrfall")]
    public class GapcloserPantheonR : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserPantheonR" /> class.
        /// </summary>
        public GapcloserPantheonR()
            : base("Pantheon", GapcloserType.Skillshot, SpellSlot.R, "pantheonrfall")
        {
        }

        #endregion
    }
}