// <copyright file="GapcloserAlistarW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Alistar W gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "headbutt")]
    public class GapcloserAlistarW : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserAlistarW" /> class.
        /// </summary>
        public GapcloserAlistarW()
            : base("Alistar", GapcloserType.Targeted, SpellSlot.W, "headbutt")
        {
        }

        #endregion
    }
}