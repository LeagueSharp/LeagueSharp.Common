// <copyright file="GapcloserShyvanaR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Shyvana R gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "shyvanatransformcast")]
    public class GapcloserShyvanaR : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserShyvanaR" /> class.
        /// </summary>
        public GapcloserShyvanaR()
            : base("Shyvana", GapcloserType.Skillshot, SpellSlot.R, "shyvanatransformcast")
        {
        }

        #endregion
    }
}