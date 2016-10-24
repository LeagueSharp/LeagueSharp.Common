// <copyright file="GapcloserJaxQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Jax Q gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "jaxleapstrike")]
    public class GapcloserJaxQ : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserJaxQ" /> class.
        /// </summary>
        public GapcloserJaxQ()
            : base("Jax", GapcloserType.Targeted, SpellSlot.Q, "jaxleapstrike")
        {
        }

        #endregion
    }
}