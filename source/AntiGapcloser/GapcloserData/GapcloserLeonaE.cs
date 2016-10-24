// <copyright file="GapcloserLeonaE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Leona E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "leonazenithblade")]
    public class GapcloserLeonaE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserLeonaE" /> class.
        /// </summary>
        public GapcloserLeonaE()
            : base("Leona", GapcloserType.Skillshot, SpellSlot.E, "leonazenithblade")
        {
        }

        #endregion
    }
}