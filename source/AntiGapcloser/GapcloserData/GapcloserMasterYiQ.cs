// <copyright file="GapcloserMasterYiQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The MasterYi Q gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "alphastrike")]
    public class GapcloserMasterYiQ : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserMasterYiQ" /> class.
        /// </summary>
        public GapcloserMasterYiQ()
            : base("MasterYi", GapcloserType.Targeted, SpellSlot.Q, "alphastrike")
        {
        }

        #endregion
    }
}