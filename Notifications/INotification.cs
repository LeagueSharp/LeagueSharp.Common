#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 INotification.cs is part of LeagueSharp.Common.
 
 LeagueSharp.Common is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 LeagueSharp.Common is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with LeagueSharp.Common. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

namespace LeagueSharp.Common
{
    public interface INotification
    {
        /// <summary>
        ///     Gets called when Screen->Present(); is called
        /// </summary>
        void OnDraw();

        /// <summary>
        ///    Gets called before resetting the device
        /// </summary>
        void OnPreReset();

        /// <summary>
        ///     Gets called after resetting the device
        /// </summary>
        void OnPostReset();

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