// <copyright file="GapcloserLeBlancR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The LeBlanc R gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "leblancslidem")]
    public class GapcloserLeBlancR : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserLeBlancR" /> class.
        /// </summary>
        public GapcloserLeBlancR()
            : base("LeBlanc", GapcloserType.Skillshot, SpellSlot.R, "leblancslidem")
        {
        }

        #endregion
    }
}