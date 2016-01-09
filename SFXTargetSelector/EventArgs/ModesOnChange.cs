#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 ModesOnChange.cs is part of SFXTargetSelector.

 SFXTargetSelector is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 SFXTargetSelector is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with SFXTargetSelector. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion License

#region

using System;

#endregion

namespace SFXTargetSelector
{
    public static partial class TargetSelector
    {
        public static partial class Modes
        {
            public class OnChangeArgs : EventArgs
            {
                public OnChangeArgs(Item modeItem)
                {
                    ModeItem = modeItem;
                }

                public Item ModeItem { get; private set; }
            }
        }
    }
}