// <copyright file="GapcloserZacE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Zac E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "zace")]
    public class GapcloserZacE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserZacE" /> class.
        /// </summary>
        public GapcloserZacE()
            : base("Zac", GapcloserType.Skillshot, SpellSlot.E, "zace")
        {
        }

        #endregion
    }
}