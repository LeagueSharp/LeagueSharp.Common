namespace LeagueSharp.Common
{
    public interface INotification
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Returns the Notification ID
        /// </summary>
        /// <returns>GUID</returns>
        string GetId();

        /// <summary>
        ///     Gets called after unloading the current appdomain.
        /// </summary>
        void OnDomainUnload();

        /// <summary>
        ///     Gets called when Screen->Present(); is called
        /// </summary>
        void OnDraw();

        /// <summary>
        ///     Gets called after resetting the device
        /// </summary>
        void OnPostReset();

        /// <summary>
        ///     Gets called before resetting the device
        /// </summary>
        void OnPreReset();

        /// <summary>
        ///     Gets called when Game -> Tick happens and updates the game.
        /// </summary>
        void OnUpdate();

        /// <summary>
        ///     Gets called on a WindowsMessage event.
        /// </summary>
        /// <param name="args">WndEventArgs</param>
        void OnWndProc(WndEventArgs args);

        #endregion
    }
}