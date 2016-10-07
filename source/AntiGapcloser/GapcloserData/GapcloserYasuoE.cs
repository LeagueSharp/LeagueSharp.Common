// <copyright file="GapcloserYasuoE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Yasuo E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "yasuodashwrapper")]
    public class GapcloserYasuoE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserYasuoE" /> class.
        /// </summary>
        public GapcloserYasuoE()
            : base("Yasuo", GapcloserType.Targeted, SpellSlot.E, "yasuodashwrapper")
        {
        }

        #endregion
    }
}