// <copyright file="GapcloserJayceQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Jayce Q gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "jaycetotheskies")]
    public class GapcloserJayceQ : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserJayceQ" /> class.
        /// </summary>
        public GapcloserJayceQ()
            : base("Jayce", GapcloserType.Targeted, SpellSlot.Q, "jaycetotheskies")
        {
        }

        #endregion
    }
}