// <copyright file="GapcloserLeeSinQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The LeeSin Q gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "blindmonkqtwo")]
    public class GapcloserLeeSinQ : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserLeeSinQ" /> class.
        /// </summary>
        public GapcloserLeeSinQ()
            : base("LeeSin", GapcloserType.Targeted, SpellSlot.Q, "blindmonkqtwo")
        {
        }

        #endregion
    }
}