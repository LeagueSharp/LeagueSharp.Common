// <copyright file="GapcloserHecarimR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Hecarim R gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "hecarimult")]
    public class GapcloserHecarimR : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserHecarimR" /> class.
        /// </summary>
        public GapcloserHecarimR()
            : base("Hecarim", GapcloserType.Skillshot, SpellSlot.R, "hecarimult")
        {
        }

        #endregion
    }
}