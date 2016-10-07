// <copyright file="GapcloserGravesE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Graves E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "gravesmove")]
    public class GapcloserGravesE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserGravesE" /> class.
        /// </summary>
        public GapcloserGravesE()
            : base("Graves", GapcloserType.Skillshot, SpellSlot.E, "gravesmove")
        {
        }

        #endregion
    }
}