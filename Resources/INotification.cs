using LeagueSharp;

namespace LeagueSharp.Common
{
    public interface INotification
    {
        /// <summary>
        ///     Gets called when Screen->Present(); is called
        /// </summary>
        void OnDraw();

        /// <summary>
        ///     Gets called when Game -> Tick happens and updates the game.
        /// </summary>
        void OnUpdate();

        /// <summary>
        ///     Gets called on a WindowsMessage event.
        /// </summary>
        /// <param name="args">WndEventArgs</param>
        void OnWndProc(WndEventArgs args);

        /// <summary>
        ///     Returns the Notification ID
        /// </summary>
        /// <returns>GUID</returns>
        string GetId();
    }
}