// <copyright file="DamageType.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    /// <summary>
    ///     Damage calculations and data.
    /// </summary>
    public partial class Damage
    {
        #region Enums

        /// <summary>
        ///     The type of damage.
        /// </summary>
        public enum DamageType
        {
            /// <summary>
            ///     Physical Damage.
            /// </summary>
            Physical,

            /// <summary>
            ///     Magical Damage.
            /// </summary>
            Magical,

            /// <summary>
            ///     True Damage.
            /// </summary>
            True
        }

        #endregion
    }
}