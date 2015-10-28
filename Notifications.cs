#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Notifications.cs is part of LeagueSharp.Common.
 
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

#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#endregion

namespace LeagueSharp.Common
{
    public class Notifications
    {
        /// <summary>
        /// The notifications list
        /// </summary>
        private static readonly ConcurrentDictionary<string, INotification> NotificationsList =
            new ConcurrentDictionary<string, INotification>();

        /// <summary>
        /// Initializes static members of the <see cref="Notifications"/> class.
        /// </summary>
        static Notifications()
        {
            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnPostReset += Drawing_OnPostReset;
            Drawing.OnPreReset += Drawing_OnPreReset;
            Game.OnWndProc += Game_OnWndProc;
            
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }

        /// <summary>
        /// Fired when the game recieves a window event.
        /// </summary>
        /// <param name="args">The <see cref="WndEventArgs"/> instance containing the event data.</param>
        private static void Game_OnWndProc(WndEventArgs args)
        {
            foreach (var notification in NotificationsList)
            {
                notification.Value.OnWndProc(args);
            }
        }

        /// <summary>
        /// Fired when the game is drawn.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void Drawing_OnDraw(EventArgs args)
        {
            foreach (var notification in NotificationsList)
            {
                notification.Value.OnDraw();
            }
        }

        /// <summary>
        /// Fired before the DirectX device is reset.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void Drawing_OnPreReset(EventArgs args)
        {
            foreach (var notification in NotificationsList)
            {
                notification.Value.OnPreReset();
            }
        }

        /// <summary>
        /// Fired when the DirectX device is reset.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void Drawing_OnPostReset(EventArgs args)
        {
            foreach (var notification in NotificationsList)
            {
                notification.Value.OnPostReset();
            }
        }

        /// <summary>
        /// Fired when the game updates.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void Game_OnGameUpdate(EventArgs args)
        {
            foreach (var notification in NotificationsList)
            {
                notification.Value.OnUpdate();
            }
        }

        /// <summary>
        /// Adds a notification to the notification list
        /// </summary>
        /// <param name="notification">Notification Instance</param>
        /// <returns>Boolean</returns>
        public static bool AddNotification(INotification notification)
        {
            return (notification != null) && NotificationsList.TryAdd(notification.GetId(), notification);
        }

        /// <summary>
        /// Adds a simple notification to the notification list
        /// </summary>
        /// <param name="text">Display Text</param>
        /// <param name="duration">Duration (-1 for infinite)</param>
        /// <param name="dispose">Dispose upon ending</param>
        /// <returns>Notification.</returns>
        public static Notification AddNotification(string text, int duration = -0x1, bool dispose = true)
        {
            var notification = new Notification(text, duration, dispose);
            NotificationsList.TryAdd(notification.GetId(), notification);
            return notification;
        }

        /// <summary>
        /// Removes a notification from the notification list
        /// </summary>
        /// <param name="notification">Notification Instance</param>
        /// <returns>Boolean</returns>
        public static bool RemoveNotification(INotification notification)
        {
            INotification dump;
            return NotificationsList.TryRemove(notification.GetId(), out dump);
        }

        /// <summary>
        /// Removes a notification from the notification list
        /// </summary>
        /// <param name="id">Notification GUID</param>
        /// <param name="notification">Notification Instance</param>
        /// <returns>Boolean</returns>
        public static bool RemoveNotification(string id, out INotification notification)
        {
            return NotificationsList.TryRemove(id, out notification);
        }

        /// <summary>
        /// Validates if a notification currently exists inside the list.
        /// </summary>
        /// <param name="notification">Notification Instance</param>
        /// <returns>Boolean</returns>
        public static bool IsValidNotification(INotification notification)
        {
            return NotificationsList.ContainsKey(notification.GetId());
        }

        /// <summary>
        /// Validates if a notification currently exists inside the list.
        /// </summary>
        /// <param name="id">Notification GUID</param>
        /// <returns><c>true</c> if the specified identifier is a valid notification; otherwise, <c>false</c>.</returns>
        public static bool IsValidNotification(string id)
        {
            return NotificationsList.ContainsKey(id);
        }

        /// <summary>
        /// Removes a notification from the notification list
        /// </summary>
        /// <param name="id">Notification GUID</param>
        /// <returns>Boolean</returns>
        public static bool RemoveNotification(string id)
        {
            INotification dump;
            return NotificationsList.TryRemove(id, out dump);
        }

        #region Memory

        /// <summary>
        /// Reserves a location slot for a GUID
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="old">Old Slot</param>
        /// <returns>FileStream Handler</returns>
        public static FileStream Reserve(string id, FileStream old = null)
        {
            var loc = GetLocation();

            if (loc != -0x1)
            {
                try
                {
                    var path = Path + "\\" + loc + ".lock";

                    if (!File.Exists(path))
                    {
                        var stream = File.Create(path, 0x1, FileOptions.DeleteOnClose);

                        if (old != null)
                        {
                            Free(old);
                        }
                        return stream;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Frees a location slot
        /// </summary>
        /// <param name="stream">FileStream Handler</param>
        /// <returns>Boolean</returns>
        public static bool Free(FileStream stream)
        {
            if (stream != null)
            {
                stream.Dispose();
                stream.Close();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the next free location
        /// </summary>
        /// <returns>Location</returns>
        public static int GetLocation()
        {
            var files = Directory.GetFiles(Path, "*.lock", SearchOption.TopDirectoryOnly);

            if (!files.Any())
            {
                return 0x55;
            }

            var array = new List<int>();

            foreach (var i in files)
            {
                try
                {
                    var length = i.IndexOf("Notifications\\", StringComparison.Ordinal) + "Notifications\\".Length;
                    var str = i.Substring(length, i.Length - length);
                    var @int = int.Parse(str.Substring(0x0, str.IndexOf('.')));

                    array.Add(@int);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            array.Sort();

            // Find a free slot if array does not start from the zero-based location (85)
            if (array.Count > 0x0 && array[0] > 0x55)
            {
                for (var i = 0x55; i < array[0]; i += 0x1E)
                {
                    if (File.Exists(Path + "\\" + (i + 0x1E) + ".lock"))
                    {
                        // If slot found, return it as value.
                        return i;
                    }
                }
            }

            // Find a free slot between the current locked locations
            for (var i = 0x0; i < array.Count - 0x1; ++i)
            {
                if (array[i] + 0x1E != array[i + 0x1])
                {
                    // Return free slot which was found between current locked locations
                    return array[i] + 0x1E;
                }
            }

            // Return (last slot + 30) as value
            return array[array.Count - 0x1] + 0x1E;
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>System.Int32.</returns>
        public static int GetLocation(FileStream stream)
        {
            var i = stream.Name;
            var length = i.IndexOf("Notifications\\", StringComparison.Ordinal) + "Notifications\\".Length;
            var str = i.Substring(length, i.Length - length);
            var @int = int.Parse(str.Substring(0x0, str.IndexOf('.')));

            return @int;
        }

        /// <summary>
        /// Validates if current position is first in line
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Boolean</returns>
        public static bool IsFirst(int position)
        {
            if (position == 0x55)
            {
                return true;
            }

            var files = Directory.GetFiles(Path, "*.lock", SearchOption.TopDirectoryOnly);

            if (!files.Any())
            {
                return true;
            }

            var array = new List<int>();

            foreach (var i in files)
            {
                try
                {
                    var length = i.IndexOf("Notifications\\", StringComparison.Ordinal) + "Notifications\\".Length;
                    var str = i.Substring(length, i.Length - length);
                    var @int = int.Parse(str.Substring(0x0, str.IndexOf('.')));

                    array.Add(@int);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            if (array.Count > 0x0)
            {
                for (var i = position - 0x1E; i > GetLocation(); i -= 0x1E)
                {
                    if (array.Contains(i))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        private static string Path
        {
            get
            {
                return System.IO.Path.Combine(Config.AppDataDirectory, "Notifications");
            }
        }

        #endregion
    }
}