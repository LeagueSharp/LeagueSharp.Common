// <copyright file="GapcloserAatroxQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Aatrox Q gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "aatroxq")]
    public class GapcloserAatroxQ : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserAatroxQ" /> class.
        /// </summary>
        public GapcloserAatroxQ()
            : base("Aatrox", GapcloserType.Skillshot, SpellSlot.Q, "aatroxq")
        {
        }

        #endregion
    }
}