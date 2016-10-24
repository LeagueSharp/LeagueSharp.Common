// <copyright file="GapcloserFioraQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Fiora Q gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "fioraq")]
    public class GapcloserFioraQ : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserFioraQ" /> class.
        /// </summary>
        public GapcloserFioraQ()
            : base("Fiora", GapcloserType.Skillshot, SpellSlot.Q, "fioraq")
        {
        }

        #endregion
    }
}