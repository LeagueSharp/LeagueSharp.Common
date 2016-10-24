// <copyright file="Version.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    /// <summary>
    ///     The game version.
    /// </summary>
    public static class Version
    {
        #region Public Properties

        /// <summary>
        ///     Gets the build.
        /// </summary>
        public static int Build => GameVersion.Build;

        /// <summary>
        ///     Gets the major version.
        /// </summary>
        public static int MajorVersion => GameVersion.Major;

        /// <summary>
        ///     Gets the minor version.
        /// </summary>
        public static int MinorVersion => GameVersion.Minor;

        /// <summary>
        ///     Gets the revision.
        /// </summary>
        public static int Revision => GameVersion.Revision;

        #endregion

        #region Properties

        private static System.Version GameVersion { get; } = new System.Version(Game.Version);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     determines whether the versions are equal.
        /// </summary>
        /// <param name="version">
        ///     The verison.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsEqual(string version) => GameVersion == new System.Version(version);

        /// <summary>
        ///     determines whether the version is newer.
        /// </summary>
        /// <param name="version">
        ///     The verison.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsNewer(string version) => GameVersion > new System.Version(version);

        /// <summary>
        ///     determines whether the version is older.
        /// </summary>
        /// <param name="version">
        ///     The verison.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsOlder(string version) => GameVersion < new System.Version(version);

        #endregion
    }
}