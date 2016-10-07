// <copyright file="GapcloserGragasE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Gragas E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "gragase")]
    public class GapcloserGragasE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserGragasE" /> class.
        /// </summary>
        public GapcloserGragasE()
            : base("Gragas", GapcloserType.Skillshot, SpellSlot.E, "gragase")
        {
        }

        #endregion
    }
}