namespace LeagueSharp.Common
{
    using System;

    using SharpDX;

    /// <summary>
    ///     The PermaShow class allows you to add important items to permashow easily.
    /// </summary>
    [Obsolete]
    public static class PermaShow
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Adds a menuitem to PermaShow, can be used without any arguements or with if you want to customize. The bool can be
        ///     set to false to remove the item from permashow.
        ///     When removing, you can simply set the bool parameter to false and everything else can be null. The default color is
        ///     White.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="enabled">if set to <c>true</c> the instance will be enabled.</param>
        /// <param name="customdisplayname">The customdisplayname.</param>
        /// <param name="col">The color.</param>
        public static void Permashow(
            this MenuItem item,
            bool enabled = true,
            string customdisplayname = null,
            Color? col = null)
        {
        }

        #endregion
    }
}