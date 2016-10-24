// <copyright file="GapcloserRenektonE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Renekton E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "renektonsliceanddice")]
    public class GapcloserRenektonE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserRenektonE" /> class.
        /// </summary>
        public GapcloserRenektonE()
            : base("Renekton", GapcloserType.Skillshot, SpellSlot.E, "renektonsliceanddice")
        {
        }

        #endregion
    }
}