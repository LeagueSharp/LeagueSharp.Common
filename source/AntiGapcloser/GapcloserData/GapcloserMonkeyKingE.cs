// <copyright file="GapcloserMonkeyKingE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The MonkeyKing E gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "monkeykingnimbus")]
    public class GapcloserMonkeyKingE : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserMonkeyKingE" /> class.
        /// </summary>
        public GapcloserMonkeyKingE()
            : base("MonkeyKing", GapcloserType.Targeted, SpellSlot.E, "monkeykingnimbus")
        {
        }

        #endregion
    }
}