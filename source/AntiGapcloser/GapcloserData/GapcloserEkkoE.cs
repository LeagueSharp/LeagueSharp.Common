// <copyright file="GapcloserEkkoE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Ekko E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "ekkoeattack")]
    public class GapcloserEkkoE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserEkkoE" /> class.
        /// </summary>
        public GapcloserEkkoE()
            : base("Ekko", GapcloserType.Targeted, SpellSlot.E, "ekkoeattack")
        {
        }

        #endregion
    }
}