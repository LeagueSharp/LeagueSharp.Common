// <copyright file="GapcloserIreliaQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Irelia Q gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "ireliagatotsu")]
    public class GapcloserIreliaQ : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserIreliaQ" /> class.
        /// </summary>
        public GapcloserIreliaQ()
            : base("Irelia", GapcloserType.Targeted, SpellSlot.Q, "ireliagatotsu")
        {
        }

        #endregion
    }
}