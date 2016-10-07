// <copyright file="GapcloserTristanaW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Tristana W gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "rocketjump")]
    public class GapcloserTristanaW : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserTristanaW" /> class.
        /// </summary>
        public GapcloserTristanaW()
            : base("Tristana", GapcloserType.Skillshot, SpellSlot.W, "rocketjump")
        {
        }

        #endregion
    }
}