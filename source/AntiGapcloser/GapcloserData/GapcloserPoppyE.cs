// <copyright file="GapcloserPoppyE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Poppy E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "poppyheroiccharge")]
    public class GapcloserPoppyE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserPoppyE" /> class.
        /// </summary>
        public GapcloserPoppyE()
            : base("Poppy", GapcloserType.Targeted, SpellSlot.E, "poppyheroiccharge")
        {
        }

        #endregion
    }
}