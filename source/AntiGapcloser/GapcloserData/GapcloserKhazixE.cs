// <copyright file="GapcloserKhazixE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The Khazix E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "khazixelong")]
    public class GapcloserKhazixE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserKhazixE" /> class.
        /// </summary>
        public GapcloserKhazixE()
            : base("Khazix", GapcloserType.Skillshot, SpellSlot.E, "khazixelong")
        {
        }

        #endregion
    }
}