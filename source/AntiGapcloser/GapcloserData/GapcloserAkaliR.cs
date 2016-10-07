// <copyright file="GapcloserAkaliR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Akali R gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "akalishadowdance")]
    public class GapcloserAkaliR : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserAkaliR" /> class.
        /// </summary>
        public GapcloserAkaliR()
            : base("Akali", GapcloserType.Targeted, SpellSlot.R, "akalishadowdance")
        {
        }

        #endregion
    }
}