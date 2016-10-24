// <copyright file="GapcloserFizzQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Fizz Q gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "fizzpiercingstrike")]
    public class GapcloserFizzQ : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserFizzQ" /> class.
        /// </summary>
        public GapcloserFizzQ()
            : base("Fizz", GapcloserType.Targeted, SpellSlot.Q, "fizzpiercingstrike")
        {
        }

        #endregion
    }
}