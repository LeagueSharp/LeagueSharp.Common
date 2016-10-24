// <copyright file="GapcloserEliseE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Elise E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "elisespideredescent")]
    public class GapcloserEliseE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserEliseE" /> class.
        /// </summary>
        public GapcloserEliseE()
            : base("Elise", GapcloserType.Targeted, SpellSlot.E, "elisespideredescent")
        {
        }

        #endregion
    }
}