// <copyright file="GapcloserCorkiW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Corki W gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "carpetbomb")]
    public class GapcloserCorkiW : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserCorkiW" /> class.
        /// </summary>
        public GapcloserCorkiW()
            : base("Corki", GapcloserType.Skillshot, SpellSlot.W, "carpetbomb")
        {
        }

        #endregion
    }
}