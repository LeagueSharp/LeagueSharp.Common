namespace LeagueSharp.Common
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    ///     The windows event message composition, gives indepth information from a <see cref="WndEventArgs" />.
    /// </summary>
    public struct WndEventComposition
    {
        #region Fields

        /// <summary>
        ///     The UTF-8/ANSI character of the current message. (If available)
        /// </summary>
        public readonly char Char;

        /// <summary>
        ///     The key, with a modifier if available.
        /// </summary>
        public readonly Keys FullKey;

        /// <summary>
        ///     The key.
        /// </summary>
        public readonly Keys Key;

        /// <summary>
        ///     The windows message.
        /// </summary>
        public readonly WindowsMessages Msg;

        /// <summary>
        ///     The side button.
        /// </summary>
        public readonly Keys SideButton;

        /// <summary>
        ///     The windows event arguments.
        /// </summary>
        private readonly WndEventArgs wndEventArgs;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="WndEventComposition" /> struct.
        /// </summary>
        /// <param name="wndEventArgs">
        ///     The <see cref="WndEventArgs" />
        /// </param>
        public WndEventComposition(WndEventArgs wndEventArgs)
        {
            this.wndEventArgs = wndEventArgs;

            this.Char = Convert.ToChar((wndEventArgs.WParam <= char.MaxValue) ? wndEventArgs.WParam : 0);
            this.Key = (Keys)((int)wndEventArgs.WParam);
            this.FullKey = (Keys)((int)wndEventArgs.WParam) != Control.ModifierKeys
                               ? (Keys)((int)wndEventArgs.WParam) | Control.ModifierKeys
                               : (Keys)((int)wndEventArgs.WParam);
            this.Msg = (WindowsMessages)wndEventArgs.Msg;

            var bytes = BitConverter.GetBytes(wndEventArgs.WParam);
            this.SideButton = (bytes.Length > 2)
                                  ? bytes[2] == 1 ? Keys.XButton1 : bytes[2] == 2 ? Keys.XButton2 : Keys.None
                                  : Keys.None;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the Windows Event Message LParam.
        /// </summary>
        public int LParam
        {
            get
            {
                return this.wndEventArgs.LParam;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether to process the message.
        /// </summary>
        public bool Process
        {
            get
            {
                return this.wndEventArgs.Process;
            }

            set
            {
                this.wndEventArgs.Process = value;
            }
        }

        /// <summary>
        ///     Gets the Windows Event Message WParam.
        /// </summary>
        public uint WParam
        {
            get
            {
                return this.wndEventArgs.WParam;
            }
        }

        #endregion
    }
}