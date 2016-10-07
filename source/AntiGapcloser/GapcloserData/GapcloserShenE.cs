// <copyright file="GapcloserShenE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Shen E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "shene")]
    public class GapcloserShenE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserShenE" /> class.
        /// </summary>
        public GapcloserShenE()
            : base("Shen", GapcloserType.Skillshot, SpellSlot.E, "shene")
        {
        }

        #endregion
    }
}