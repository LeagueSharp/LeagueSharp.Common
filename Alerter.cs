#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Alerter.cs is part of LeagueSharp.Common.
 
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
using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    /// Shows text for an amount of time.
    /// </summary>
    public class Alerter : Render.Text
    {
        private readonly float _duration;
        private readonly float _endTime;
        private readonly float _startTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="Alerter"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="text">The text.</param>
        /// <param name="size">The size.</param>
        /// <param name="color">The color.</param>
        /// <param name="faceName">Name of the face.</param>
        /// <param name="duration">The duration.</param>
        public Alerter(int x,
            int y,
            string text,
            int size,
            ColorBGRA color,
            string faceName = "Calibri",
            float duration = 1f) : base(x, y, text, size, color, faceName)
        {
            _duration = duration;
            _startTime = Utils.TickCount;
            _endTime = _startTime + _duration;

            Game.OnUpdate += Game_OnGameUpdate;
        }

        /// <summary>
        /// Gets the end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public float EndTime
        {
            get { return _endTime; }
        }

        /// <summary>
        /// Gets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public float StartTime
        {
            get { return _startTime; }
        }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public float Duration
        {
            get { return _duration; }
        }

        /// <summary>
        /// Removes this instance.
        /// </summary>
        public void Remove()
        {
            Visible = false;
            Dispose();
        }

        /// <summary>
        /// Fired when the game updates.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Game_OnGameUpdate(EventArgs args)
        {
            if (!(Utils.TickCount > EndTime))
            {
                return;
            }

            Visible = false;
            Dispose();
        }
    }
}