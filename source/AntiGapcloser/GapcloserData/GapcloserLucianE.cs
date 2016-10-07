// <copyright file="GapcloserLucianE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Lucian E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "luciane")]
    public class GapcloserLucianE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserLucianE" /> class.
        /// </summary>
        public GapcloserLucianE()
            : base("Lucian", GapcloserType.Skillshot, SpellSlot.E, "luciane")
        {
        }

        #endregion
    }
}