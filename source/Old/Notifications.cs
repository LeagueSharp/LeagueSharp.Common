namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Concurrent;

    public class Notifications
    {
        #region Static Fields

        /// <summary>
        ///     The notifications list
        /// </summary>
        private static readonly ConcurrentDictionary<string, INotification> NotificationsList =
            new ConcurrentDictionary<string, INotification>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="Notifications" /> class.
        /// </summary>
        static Notifications()
        {
            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnPostReset += Drawing_OnPostReset;
            Drawing.OnPreReset += Drawing_OnPreReset;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            Game.OnWndProc += Game_OnWndProc;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds a notification to the notification list
        /// </summary>
        /// <param name="notification">Notification Instance</param>
        /// <returns>Boolean</returns>
        public static bool AddNotification(INotification notification)
        {
            return (notification != null) && NotificationsList.TryAdd(notification.GetId(), notification);
        }

        /// <summary>
        ///     Adds a simple notification to the notification list
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
        ///     Returns the next free location
        /// </summary>
        /// <returns>Location</returns>
        public static int GetLocation()
        {
            return 0x55 + 0x1E * NotificationsList.Count;
        }

        /// <summary>
        ///     Returns the location free location
        /// </summary>
        /// <returns>Location</returns>
        public static int GetLocation(INotification notification)
        {
            var i = 0;
            var guid = notification.GetId();

            foreach (var notification_ in NotificationsList)
            {
                if (notification_.Key == guid)
                {
                    return 0x55 + 0x1E * i;
                }
                i++;
            }

            return 0x55 + 0x1E * i;
        }

        /// <summary>
        ///     Validates if a notification currently exists inside the list.
        /// </summary>
        /// <param name="notification">Notification Instance</param>
        /// <returns>Boolean</returns>
        public static bool IsValidNotification(INotification notification)
        {
            return NotificationsList.ContainsKey(notification.GetId());
        }

        /// <summary>
        ///     Validates if a notification currently exists inside the list.
        /// </summary>
        /// <param name="id">Notification GUID</param>
        /// <returns><c>true</c> if the specified identifier is a valid notification; otherwise, <c>false</c>.</returns>
        public static bool IsValidNotification(string id)
        {
            return NotificationsList.ContainsKey(id);
        }

        /// <summary>
        ///     Removes a notification from the notification list
        /// </summary>
        /// <param name="notification">Notification Instance</param>
        /// <returns>Boolean</returns>
        public static bool RemoveNotification(INotification notification)
        {
            INotification dump;
            return NotificationsList.TryRemove(notification.GetId(), out dump);
        }

        /// <summary>
        ///     Removes a notification from the notification list
        /// </summary>
        /// <param name="id">Notification GUID</param>
        /// <param name="notification">Notification Instance</param>
        /// <returns>Boolean</returns>
        public static bool RemoveNotification(string id, out INotification notification)
        {
            return NotificationsList.TryRemove(id, out notification);
        }

        /// <summary>
        ///     Removes a notification from the notification list
        /// </summary>
        /// <param name="id">Notification GUID</param>
        /// <returns>Boolean</returns>
        public static bool RemoveNotification(string id)
        {
            INotification dump;
            return NotificationsList.TryRemove(id, out dump);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Fired when the current domain unloads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            foreach (var notification in NotificationsList)
            {
                notification.Value.OnDomainUnload();
            }
        }

        /// <summary>
        ///     Fired when the game is drawn.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void Drawing_OnDraw(EventArgs args)
        {
            foreach (var notification in NotificationsList)
            {
                notification.Value.OnDraw();
            }
        }

        /// <summary>
        ///     Fired when the DirectX device is reset.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void Drawing_OnPostReset(EventArgs args)
        {
            foreach (var notification in NotificationsList)
            {
                notification.Value.OnPostReset();
            }
        }

        /// <summary>
        ///     Fired before the DirectX device is reset.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void Drawing_OnPreReset(EventArgs args)
        {
            foreach (var notification in NotificationsList)
            {
                notification.Value.OnPreReset();
            }
        }

        /// <summary>
        ///     Fired when the game updates.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void Game_OnGameUpdate(EventArgs args)
        {
            foreach (var notification in NotificationsList)
            {
                notification.Value.OnUpdate();
            }
        }

        /// <summary>
        ///     Fired when the game recieves a window event.
        /// </summary>
        /// <param name="args">The <see cref="WndEventArgs" /> instance containing the event data.</param>
        private static void Game_OnWndProc(WndEventArgs args)
        {
            foreach (var notification in NotificationsList)
            {
                notification.Value.OnWndProc(args);
            }
        }

        #endregion
    }
}