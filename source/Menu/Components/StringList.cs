// <copyright file="StringList.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Runtime.Serialization;

    using SharpDX.Menu;

    /// <summary>
    ///     The string list component container.
    /// </summary>
    [DataContract]
    public class StringList : IUpdateableValue<StringList>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringList" /> class.
        /// </summary>
        /// <param name="items">
        ///     The items.
        /// </param>
        /// <param name="defaultIndex">
        ///     The default index.
        /// </param>
        public StringList(string[] items, int defaultIndex = 0)
        {
            this.Items = items;
            this.SelectedIndex = defaultIndex;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the items.
        /// </summary>
        public string[] Items { get; }

        /// <summary>
        ///     Gets or sets the selected index.
        /// </summary>
        [DataMember]
        public int SelectedIndex { get; set; }

        /// <summary>
        ///     Gets the selected value.
        /// </summary>
        public string SelectedValue
            =>
            this.SelectedIndex >= 0 && this.SelectedIndex < this.Items.Length
                ? this.Items[this.SelectedIndex]
                : string.Empty;

        /// <summary>
        ///     Gets the items.
        /// </summary>
        [Obsolete]
        public string[] SList => this.Items;

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public void Update(StringList newValue)
        {
            this.SelectedIndex = newValue.SelectedIndex;
        }

        #endregion
    }
}