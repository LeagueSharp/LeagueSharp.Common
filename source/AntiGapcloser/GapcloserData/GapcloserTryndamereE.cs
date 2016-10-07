// <copyright file="GapcloserTryndamereE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Tryndamere E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "slashcast")]
    public class GapcloserTryndamereE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserTryndamereE" /> class.
        /// </summary>
        public GapcloserTryndamereE()
            : base("Tryndamere", GapcloserType.Skillshot, SpellSlot.E, "slashcast")
        {
        }

        #endregion
    }
}