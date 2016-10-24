// <copyright file="GapcloserKassadinR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Kassadin R gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "riftwalk")]
    public class GapcloserKassadinR : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserKassadinR" /> class.
        /// </summary>
        public GapcloserKassadinR()
            : base("Kassadin", GapcloserType.Skillshot, SpellSlot.R, "riftwalk")
        {
        }

        #endregion
    }
}