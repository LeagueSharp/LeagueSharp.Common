// <copyright file="GapcloserSejuaniQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Sejuani Q gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "sejuaniarcticassault")]
    public class GapcloserSejuaniQ : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserSejuaniQ" /> class.
        /// </summary>
        public GapcloserSejuaniQ()
            : base("Sejuani", GapcloserType.Skillshot, SpellSlot.Q, "sejuaniarcticassault")
        {
        }

        #endregion
    }
}