// <copyright file="GapcloserJarvanIVQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     The JarvanIV Q gapcloser information.
    /// </summary>
    [Export(typeof(IGapcloser))]
    [ExportMetadata("SpellName", "jarvanivdragonstrike")]
    public class GapcloserJarvanIVQ : Gapcloser
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GapcloserJarvanIVQ" /> class.
        /// </summary>
        public GapcloserJarvanIVQ()
            : base("JarvanIV", GapcloserType.Skillshot, SpellSlot.Q, "jarvanivdragonstrike")
        {
        }

        #endregion
    }
}