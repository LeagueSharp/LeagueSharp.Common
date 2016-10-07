// <copyright file="GapcloserGnarE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Gnar E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "gnarbige")]
    public class GapcloserGnarE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserGnarE" /> class.
        /// </summary>
        public GapcloserGnarE()
            : base("Gnar", GapcloserType.Skillshot, SpellSlot.E, "gnarbige")
        {
        }

        #endregion
    }
}