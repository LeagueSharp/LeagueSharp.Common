// <copyright file="GapcloserXinZhaoE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The XinZhao E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "xenzhaosweep")]
    public class GapcloserXinZhaoE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserXinZhaoE" /> class.
        /// </summary>
        public GapcloserXinZhaoE()
            : base("XinZhao", GapcloserType.Targeted, SpellSlot.E, "xenzhaosweep")
        {
        }

        #endregion
    }
}