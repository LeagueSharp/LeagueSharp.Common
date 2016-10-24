// <copyright file="GapcloserRivenE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Riven E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "rivenfeint")]
    public class GapcloserRivenE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserRivenE" /> class.
        /// </summary>
        public GapcloserRivenE()
            : base("Riven", GapcloserType.Skillshot, SpellSlot.E, "rivenfeint")
        {
        }

        #endregion
    }
}