namespace LeagueSharp.Common
{
    using System;

    /// <summary>
    ///     The event arguments holder when a value is changed.
    /// </summary>
    public class OnValueChangeEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        ///     The new value.
        /// </summary>
        private readonly object newValue;

        /// <summary>
        ///     The old value.
        /// </summary>
        private readonly object oldValue;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="OnValueChangeEventArgs" /> class.
        /// </summary>
        /// <param name="oldValue">
        ///     The old value.
        /// </param>
        /// <param name="newValue">
        ///     The new value.
        /// </param>
        public OnValueChangeEventArgs(object oldValue, object newValue)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.Process = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether the value change should be processed.
        /// </summary>
        public bool Process { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the new value.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the value.
        /// </typeparam>
        /// <returns>
        ///     The new value with the passed type parameter.
        /// </returns>
        public T GetNewValue<T>()
        {
            return (T)this.newValue;
        }

        /// <summary>
        ///     Gets the old value.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the value.
        /// </typeparam>
        /// <returns>
        ///     The old value with the passed type parameter.
        /// </returns>
        public T GetOldValue<T>()
        {
            return (T)this.oldValue;
        }

        #endregion
    }
}