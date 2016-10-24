// <copyright file="GapcloserViQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Vi Q gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "viq")]
    public class GapcloserViQ : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserViQ" /> class.
        /// </summary>
        public GapcloserViQ()
            : base("Vi", GapcloserType.Skillshot, SpellSlot.Q, "viq")
        {
        }

        #endregion
    }
}