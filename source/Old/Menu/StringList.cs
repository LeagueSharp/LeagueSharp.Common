namespace LeagueSharp.Common
{
    using System;

    /// <summary>
    ///     The menu string list.
    /// </summary>
    [Serializable]
    public struct StringList
    {
        #region Fields

        /// <summary>
        ///     The selected index.
        /// </summary>
        public int SelectedIndex;

        /// <summary>
        ///     The string list.
        /// </summary>
        public string[] SList;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringList" /> struct.
        /// </summary>
        /// <param name="stringList">
        ///     The string list.
        /// </param>
        /// <param name="defaultIndex">
        ///     The default index.
        /// </param>
        public StringList(string[] stringList, int defaultIndex = 0)
        {
            this.SList = stringList;
            this.SelectedIndex = defaultIndex;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the selected value.
        /// </summary>
        public string SelectedValue
        {
            get
            {
                return (this.SelectedIndex >= 0 && this.SelectedIndex < this.SList.Length)
                           ? this.SList[this.SelectedIndex]
                           : string.Empty;
            }
        }

        #endregion
    }
}