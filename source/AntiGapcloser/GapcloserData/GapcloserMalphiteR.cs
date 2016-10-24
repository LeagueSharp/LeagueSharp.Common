// <copyright file="GapcloserMalphiteR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Malphite R gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "ufslash")]
    public class GapcloserMalphiteR : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserMalphiteR" /> class.
        /// </summary>
        public GapcloserMalphiteR()
            : base("Malphite", GapcloserType.Skillshot, SpellSlot.R, "ufslash")
        {
        }

        #endregion
    }
}