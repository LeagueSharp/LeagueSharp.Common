// <copyright file="GapcloserTalonE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Talon E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "taloncutthroat")]
    public class GapcloserTalonE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserTalonE" /> class.
        /// </summary>
        public GapcloserTalonE()
            : base("Talon", GapcloserType.Targeted, SpellSlot.E, "taloncutthroat")
        {
        }

        #endregion
    }
}