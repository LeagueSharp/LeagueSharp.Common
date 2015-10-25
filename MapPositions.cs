#region LICENSE

/*
 Copyright 2014 - 2015 LeagueSharp
 Geometry.cs is part of LeagueSharp.Common.
 
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

using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    /// Map information for Summoner's Rift
    /// </summary>
    public static class SummonersRift
    {
        /// <summary>
        /// Gets the name of the map.
        /// </summary>
        /// <value>The name of the map.</value>
        public static string Name
        {
            get { return "Summoner's Rift"; }
        }

        /// <summary>
        /// Gets the short name of the map.
        /// </summary>
        /// <value>The short name of the map.</value>
        public static string ShortName
        {
            get { return "summonerRift"; }
        }

        /// <summary>
        /// Gets the map type.
        /// </summary>
        /// <value>The map type.</value>
        public static Utility.Map.MapType Type
        {
            get { return Utility.Map.MapType.SummonersRift; }
        }

        /// <summary>
        /// Gets the grid.
        /// </summary>
        /// <value>The grid.</value>
        public static Vector2 Grid
        {
            get { return new Vector2(13982, 14446); }
        }

        /// <summary>
        /// Gets the starting level.
        /// </summary>
        /// <value>The starting level.</value>
        public static int StartingLevel
        {
            get { return 1; }
        }

        /// <summary>
        /// Contains data about the top lane.
        /// </summary>
        public static class TopLane
        {
            /// <summary>
            /// Gets the top zone.
            /// </summary>
            /// <value>The top zone.</value>
            public static Geometry.Polygon Top_Zone
            {
                get
                {
                    var topLane = new Geometry.Polygon();
                    topLane.Add(new Vector2(600, 5000));
                    topLane.Add(new Vector2(600, 11000));
                    topLane.Add(new Vector2(675, 10700));
                    topLane.Add(new Vector2(750, 11200));
                    topLane.Add(new Vector2(830, 11900));
                    topLane.Add(new Vector2(980, 12450));
                    topLane.Add(new Vector2(1150, 12750));
                    topLane.Add(new Vector2(1360, 13060));
                    topLane.Add(new Vector2(1590, 13360));
                    topLane.Add(new Vector2(1890, 13550));
                    topLane.Add(new Vector2(2260, 13770));
                    topLane.Add(new Vector2(2680, 13900));
                    topLane.Add(new Vector2(3050, 14040));
                    topLane.Add(new Vector2(3450, 14150));
                    topLane.Add(new Vector2(3950, 14250));
                    topLane.Add(new Vector2(4450, 14300));
                    topLane.Add(new Vector2(4920, 14300));
                    topLane.Add(new Vector2(4900, 14300));
                    topLane.Add(new Vector2(8280, 14300));
                    topLane.Add(new Vector2(9850, 14300));
                    topLane.Add(new Vector2(9850, 13050));
                    topLane.Add(new Vector2(8320, 13050));
                    topLane.Add(new Vector2(4850, 13050));
                    topLane.Add(new Vector2(4280, 13050));
                    topLane.Add(new Vector2(3470, 12810));
                    topLane.Add(new Vector2(3285, 12710));
                    topLane.Add(new Vector2(3070, 12490));
                    topLane.Add(new Vector2(3050, 12490));
                    topLane.Add(new Vector2(3220, 12050));
                    topLane.Add(new Vector2(2500, 11480));
                    topLane.Add(new Vector2(2270, 11660));
                    topLane.Add(new Vector2(2190, 11585));
                    topLane.Add(new Vector2(2020, 11360));
                    topLane.Add(new Vector2(1910, 11125));
                    topLane.Add(new Vector2(1850, 9625));
                    topLane.Add(new Vector2(1850, 5000));
                    return topLane;
                }
            }

            /// <summary>
            /// Gets the blue zone.
            /// </summary>
            /// <value>The blue zone.</value>
            public static Geometry.Polygon Blue_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(1250, 3900), new Vector2(1250, 9880), 775);
                }
            }

            /// <summary>
            /// Gets the blue side inhibitor zone.
            /// </summary>
            /// <value>The blue inhibitor zone.</value>
            public static Geometry.Polygon Blue_Inhib_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(1250, 3900), new Vector2(1250, 6100), 775);
                }
            }

            /// <summary>
            /// Gets the blue side inner zone.
            /// </summary>
            /// <value>The blue inner zone.</value>
            public static Geometry.Polygon Blue_Inner_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(1250, 6100), new Vector2(1250, 9880), 775);

                }
            }

            /// <summary>
            /// Gets the contest zone.
            /// </summary>
            /// <value>The contest zone.</value>
            public static Geometry.Polygon Contest_Zone
            {
                get
                {
                    var contestZone = new Geometry.Polygon();
                    contestZone.Add(new Vector2(600, 9880));
                    contestZone.Add(new Vector2(675, 10700));
                    contestZone.Add(new Vector2(750, 11200));
                    contestZone.Add(new Vector2(830, 11900));
                    contestZone.Add(new Vector2(980, 12450));
                    contestZone.Add(new Vector2(1150, 12750));
                    contestZone.Add(new Vector2(1360, 13060));
                    contestZone.Add(new Vector2(1590, 13360));
                    contestZone.Add(new Vector2(1890, 13550));
                    contestZone.Add(new Vector2(2260, 13770));
                    contestZone.Add(new Vector2(2680, 13900));
                    contestZone.Add(new Vector2(3050, 14040));
                    contestZone.Add(new Vector2(3450, 14150));
                    contestZone.Add(new Vector2(3950, 14250));
                    contestZone.Add(new Vector2(4450, 14300));
                    contestZone.Add(new Vector2(4880, 14300));
                    contestZone.Add(new Vector2(4880, 13050));
                    contestZone.Add(new Vector2(4280, 13050));
                    contestZone.Add(new Vector2(3470, 12810));
                    contestZone.Add(new Vector2(3285, 12710));
                    contestZone.Add(new Vector2(3070, 12490));
                    contestZone.Add(new Vector2(3220, 12050));
                    contestZone.Add(new Vector2(2500, 11480));
                    contestZone.Add(new Vector2(2270, 11660));
                    contestZone.Add(new Vector2(2190, 11585));
                    contestZone.Add(new Vector2(2020, 11360));
                    contestZone.Add(new Vector2(1910, 11125));
                    contestZone.Add(new Vector2(1850, 9880));
                    return contestZone;
                }
            }

            /// <summary>
            /// Gets the red zone.
            /// </summary>
            /// <value>The red zone.</value>
            public static Geometry.Polygon Red_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(10900, 13675), new Vector2(4880, 13675), 775);

                }
            }

            /// <summary>
            /// Gets the red side inhibitor zone.
            /// </summary>
            /// <value>The red inhibitor zone.</value>
            public static Geometry.Polygon Red_Inhib_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(10900, 13675), new Vector2(8400, 13675), 775);
                }
            }

            /// <summary>
            /// Gets the red side inner zone.
            /// </summary>
            /// <value>The red side inner zone.</value>
            public static Geometry.Polygon Red_Inner_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(8400, 13675), new Vector2(4880, 13675), 775);
                }
            }

            /// <summary>
            /// Gets the blue side outer turret.
            /// </summary>
            /// <value>The blue outer turret.</value>
            public static Vector2 Blue_Outer_Turret
            {
                get { return new Vector2(981, 10441); }
            }

            /// <summary>
            /// Gets the blue side inner turret.
            /// </summary>
            /// <value>The blue inner turret.</value>
            public static Vector2 Blue_Inner_Turret
            {
                get { return new Vector2(1512, 6700); }
            }

            /// <summary>
            /// Gets the red side outer turret.
            /// </summary>
            /// <value>The red outer turret.</value>
            public static Vector2 Red_Outer_Turret
            {
                get { return new Vector2(4318, 13876); }
            }

            /// <summary>
            /// Gets the redside inner turret.
            /// </summary>
            /// <value>The red inner turret.</value>
            public static Vector2 Red_Inner_Turret
            {
                get { return new Vector2(7943, 13412); }
            }
        }

        /// <summary>
        /// Contains data about the middle lane.
        /// </summary>
        public static class MidLane
        {
            /// <summary>
            /// Gets the middle lane zone.
            /// </summary>
            /// <value>The middle lane zone.</value>
            public static Geometry.Polygon Mid_Zone
            {
                get
                {
                    var midLane = new Geometry.Polygon();
                    midLane.Add(new Vector2(3510, 4470));
                    midLane.Add(new Vector2(5820, 6860));
                    midLane.Add(new Vector2(5400, 7350));
                    midLane.Add(new Vector2(7500, 9150));
                    midLane.Add(new Vector2(7960, 8850));
                    midLane.Add(new Vector2(10360, 11250));
                    midLane.Add(new Vector2(11200, 10440));
                    midLane.Add(new Vector2(9040, 7980));
                    midLane.Add(new Vector2(9650, 7595));
                    midLane.Add(new Vector2(7430, 5680));
                    midLane.Add(new Vector2(6790, 6130));
                    midLane.Add(new Vector2(4430, 3715));
                    return midLane;
                }
            }

            /// <summary>
            /// Gets the blue side zone.
            /// </summary>
            /// <value>The blue side zone.</value>
            public static Geometry.Polygon Blue_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(3490, 3500), new Vector2(5760, 5950), 775);
                }
            }

            /// <summary>
            /// Gets the blue side inhibitor zone.
            /// </summary>
            /// <value>The blue side inhibitor zone.</value>
            public static Geometry.Polygon Blue_Inhib_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(3490, 3500), new Vector2(4560, 4700), 775);
                }
            }

            /// <summary>
            /// Gets the blue side inner zone.
            /// </summary>
            /// <value>The blue side inner zone.</value>
            public static Geometry.Polygon Blue_Inner_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(4560, 4700), new Vector2(5760, 5950), 775);

                }
            }

            /// <summary>
            /// Gets the contest zone.
            /// </summary>
            /// <value>The contest zone.</value>
            public static Geometry.Polygon Contest_Zone
            {
                get
                {
                    var contestZone = new Geometry.Polygon();
                    contestZone.Add(new Vector2(6805, 5795));
                    contestZone.Add(new Vector2(7350, 5391));
                    contestZone.Add(new Vector2(9451, 7471));
                    contestZone.Add(new Vector2(9017, 7921));
                    contestZone.Add(new Vector2(9491, 8338));
                    contestZone.Add(new Vector2(8468, 9501));
                    contestZone.Add(new Vector2(7968, 9061));
                    contestZone.Add(new Vector2(7508, 9400));
                    contestZone.Add(new Vector2(5348, 7408));
                    contestZone.Add(new Vector2(5779, 6960));
                    contestZone.Add(new Vector2(5274, 6554));
                    contestZone.Add(new Vector2(6245, 5345));
                    return contestZone;
                }
            }

            /// <summary>
            /// Gets the red side zone.
            /// </summary>
            /// <value>The red side zone.</value>
            public static Geometry.Polygon Red_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(11330, 11400), new Vector2(8980, 8920), 775);

                }
            }

            /// <summary>
            /// Gets the red side inhibitor zone.
            /// </summary>
            /// <value>The red inhibitor zone.</value>
            public static Geometry.Polygon Red_Inhib_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(11330, 11400), new Vector2(10310, 10210), 775);
                }
            }

            /// <summary>
            /// Gets the red side inner zone.
            /// </summary>
            /// <value>The red inner zone.</value>
            public static Geometry.Polygon Red_Inner_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(10310, 10210), new Vector2(8980, 8920), 775);
                }
            }

            /// <summary>
            /// Gets the blue side outer turret position.
            /// </summary>
            /// <value>The blue side outer turret position.</value>
            public static Vector2 Blue_Outer_Turret
            {
                get { return new Vector2(5846, 6396); }
            }

            /// <summary>
            /// Gets the blue side inner turret position.
            /// </summary>
            /// <value>The blue side inner turret position.</value>
            public static Vector2 Blue_Inner_Turret
            {
                get { return new Vector2(5048, 4813); }
            }

            /// <summary>
            /// Gets the red side outer turret position.
            /// </summary>
            /// <value>The red side outer turret position.</value>
            public static Vector2 Red_Outer_Turret
            {
                get { return new Vector2(8955, 8510); }
            }

            /// <summary>
            /// Gets the red side inner turret position.
            /// </summary>
            /// <value>The red side inner turret position.</value>
            public static Vector2 Red_Inner_Turret
            {
                get { return new Vector2(9768, 10114); }
            }
        }

        /// <summary>
        /// Contrains data about the bottom lane.
        /// </summary>
        public static class BottomLane
        {
            /// <summary>
            /// Gets the bottom lane zone.
            /// </summary>
            /// <value>The bottom lane zone.</value>
            public static Geometry.Polygon Bottom_Zone
            {
                get
                {
                    var bottomLane = new Geometry.Polygon();
                    bottomLane.Add(new Vector2(11000, 573));
                    bottomLane.Add(new Vector2(11045, 575));
                    bottomLane.Add(new Vector2(11305, 635));
                    bottomLane.Add(new Vector2(11575, 655));
                    bottomLane.Add(new Vector2(11825, 765));
                    bottomLane.Add(new Vector2(12145, 835));
                    bottomLane.Add(new Vector2(12395, 950));
                    bottomLane.Add(new Vector2(12645, 1085));
                    bottomLane.Add(new Vector2(12785, 1245));
                    bottomLane.Add(new Vector2(13045, 1430));
                    bottomLane.Add(new Vector2(13240, 1635));
                    bottomLane.Add(new Vector2(13425, 1860));
                    bottomLane.Add(new Vector2(13700, 2126));
                    bottomLane.Add(new Vector2(13805, 2395));
                    bottomLane.Add(new Vector2(13935, 2630));
                    bottomLane.Add(new Vector2(14065, 2895));
                    bottomLane.Add(new Vector2(14140, 3060));
                    bottomLane.Add(new Vector2(14300, 3310));
                    bottomLane.Add(new Vector2(14360, 3540));
                    bottomLane.Add(new Vector2(14450, 3750));
                    bottomLane.Add(new Vector2(14550, 4115));
                    bottomLane.Add(new Vector2(14575, 4400));
                    bottomLane.Add(new Vector2(14575, 4710));
                    bottomLane.Add(new Vector2(14535, 4800));
                    bottomLane.Add(new Vector2(14450, 4798));
                    bottomLane.Add(new Vector2(14450, 10000));
                    bottomLane.Add(new Vector2(12900, 10000));
                    bottomLane.Add(new Vector2(12900, 4762));
                    bottomLane.Add(new Vector2(12825, 4760));
                    bottomLane.Add(new Vector2(11825, 4155));
                    bottomLane.Add(new Vector2(10370, 3115));
                    bottomLane.Add(new Vector2(10149, 2025));
                    bottomLane.Add(new Vector2(5000, 2025));
                    bottomLane.Add(new Vector2(5000, 475));
                    bottomLane.Add(new Vector2(11000, 475));
                    return bottomLane;
                }
            }

            /// <summary>
            /// Gets the blue zone.
            /// </summary>
            /// <value>The blue zone.</value>
            public static Geometry.Polygon Blue_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(3900, 1250), new Vector2(9880, 1250), 775);
                }
            }

            /// <summary>
            /// Gets the blue inhib zone.
            /// </summary>
            /// <value>The blue inhib zone.</value>
            public static Geometry.Polygon Blue_Inhib_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(3900, 1250), new Vector2(6100, 1250), 775);
                }
            }

            /// <summary>
            /// Gets the blue inner zone.
            /// </summary>
            /// <value>The blue inner zone.</value>
            public static Geometry.Polygon Blue_Inner_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(6100, 1250), new Vector2(9880, 1250), 775);

                }
            }

            /// <summary>
            /// Gets the contest zone.
            /// </summary>
            /// <value>The contest zone.</value>
            public static Geometry.Polygon Contest_Zone
            {
                get
                {
                    var contestZone = new Geometry.Polygon();
                    contestZone.Add(new Vector2(10180, 550));
                    contestZone.Add(new Vector2(10500, 560));
                    contestZone.Add(new Vector2(10770, 565));
                    contestZone.Add(new Vector2(11045, 575));
                    contestZone.Add(new Vector2(11305, 635));
                    contestZone.Add(new Vector2(11575, 655));
                    contestZone.Add(new Vector2(11825, 765));
                    contestZone.Add(new Vector2(12145, 835));
                    contestZone.Add(new Vector2(12395, 950));
                    contestZone.Add(new Vector2(12645, 1085));
                    contestZone.Add(new Vector2(12785, 1245));
                    contestZone.Add(new Vector2(13045, 1430));
                    contestZone.Add(new Vector2(13240, 1635));
                    contestZone.Add(new Vector2(13425, 1860));
                    contestZone.Add(new Vector2(13700, 2126));
                    contestZone.Add(new Vector2(13805, 2395));
                    contestZone.Add(new Vector2(13935, 2630));
                    contestZone.Add(new Vector2(14065, 2895));
                    contestZone.Add(new Vector2(14140, 3060));
                    contestZone.Add(new Vector2(14300, 3310));
                    contestZone.Add(new Vector2(14360, 3540));
                    contestZone.Add(new Vector2(14450, 3750));
                    contestZone.Add(new Vector2(14550, 4115));
                    contestZone.Add(new Vector2(14575, 4400));
                    contestZone.Add(new Vector2(14575, 4710));
                    contestZone.Add(new Vector2(14535, 4800));
                    contestZone.Add(new Vector2(12825, 4760));
                    contestZone.Add(new Vector2(11825, 4155));
                    contestZone.Add(new Vector2(10370, 3115));
                    contestZone.Add(new Vector2(10130, 1930));
                    return contestZone;
                }
            }

            /// <summary>
            /// Gets the red zone.
            /// </summary>
            /// <value>The red zone.</value>
            public static Geometry.Polygon Red_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(13675, 10900), new Vector2(13675, 4880), 775);

                }
            }

            /// <summary>
            /// Gets the red inhib zone.
            /// </summary>
            /// <value>The red inhib zone.</value>
            public static Geometry.Polygon Red_Inhib_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(13675, 10900), new Vector2(13675, 8400), 775);
                }
            }

            /// <summary>
            /// Gets the red inner zone.
            /// </summary>
            /// <value>The red inner zone.</value>
            public static Geometry.Polygon Red_Inner_Zone
            {
                get
                {
                    return new Geometry.Polygon.Rectangle(new Vector2(13675, 8400), new Vector2(13675, 4880), 775);
                }
            }

            /// <summary>
            /// Gets the blue outer turret.
            /// </summary>
            /// <value>The blue outer turret.</value>
            public static Vector2 Blue_Outer_Turret
            {
                get { return new Vector2(10504, 1030); }
            }

            /// <summary>
            /// Gets the blue inner turret.
            /// </summary>
            /// <value>The blue inner turret.</value>
            public static Vector2 Blue_Inner_Turret
            {
                get { return new Vector2(6919, 1484); }
            }

            /// <summary>
            /// Gets the red outer turret.
            /// </summary>
            /// <value>The red outer turret.</value>
            public static Vector2 Red_Outer_Turret
            {
                get { return new Vector2(13866, 4505); }
            }

            /// <summary>
            /// Gets the red inner turret.
            /// </summary>
            /// <value>The red inner turret.</value>
            public static Vector2 Red_Inner_Turret
            {
                get { return new Vector2(13328, 8226); }
            }
        }

        /// <summary>
        /// Contains data about the bases.
        /// </summary>
        public static class Base
        {
            /// <summary>
            /// Gets the blue base zone.
            /// </summary>
            /// <value>The blue base zone.</value>
            public static Geometry.Polygon Blue_Base_Zone
            {
                get
                {
                    var blueBase = new Geometry.Polygon();
                    blueBase.Add(new Vector2(300, 300));
                    blueBase.Add(new Vector2(5000, 300));
                    blueBase.Add(
                        new Geometry.Polygon.Arc(
                            new Vector2(1700, 1700), new Vector2(4000, 4000), Geometry.DegreeToRadian(90), 3200));
                    blueBase.Add(new Vector2(300, 5000));
                    return blueBase;
                }
            }

            /// <summary>
            /// Gets the red base zone.
            /// </summary>
            /// <value>The red base zone.</value>
            public static Geometry.Polygon Red_Base_Zone
            {
                get
                {
                    var redBase = new Geometry.Polygon();
                    redBase.Add(new Vector2(14535, 10006));
                    redBase.Add(new Vector2(14573, 14724));
                    redBase.Add(new Vector2(9734, 14699));
                    redBase.Add(new Vector2(9836, 13173));
                    redBase.Add(new Vector2(9834, 13069));
                    redBase.Add(new Vector2(9867, 12806));
                    redBase.Add(new Vector2(9901, 12471));
                    redBase.Add(new Vector2(9974, 12188));
                    redBase.Add(new Vector2(10020, 11903));
                    redBase.Add(new Vector2(10135, 11653));
                    redBase.Add(new Vector2(10227, 11363));
                    redBase.Add(new Vector2(10396, 10946));
                    redBase.Add(new Vector2(10497, 10743));
                    redBase.Add(new Vector2(10673, 10579));
                    redBase.Add(new Vector2(11066, 10427));
                    redBase.Add(new Vector2(11132, 10406));
                    redBase.Add(new Vector2(11409, 10295));
                    redBase.Add(new Vector2(11690, 10189));
                    redBase.Add(new Vector2(11897, 10136));
                    redBase.Add(new Vector2(12187, 10060));
                    redBase.Add(new Vector2(12394, 10037));
                    redBase.Add(new Vector2(12656, 10011));
                    redBase.Add(new Vector2(12981, 9990));
                    return redBase;
                }
            }

            /// <summary>
            /// Gets the blue top turret.
            /// </summary>
            /// <value>the blue top turret.</value>
            public static Vector2 Blue_Top_Turret
            {
                get { return new Vector2(1170, 4288); }
            }

            /// <summary>
            /// Gets the blue mid turret.
            /// </summary>
            /// <value>the blue mid turret.</value>
            public static Vector2 Blue_Mid_Turret
            {
                get { return new Vector2(3652, 3696); }
            }

            /// <summary>
            /// Gets the blue bottom turret.
            /// </summary>
            /// <value>the blue bottom turret.</value>
            public static Vector2 Blue_Bottom_Turret
            {
                get { return new Vector2(4282, 1254); }
            }

            /// <summary>
            /// Gets the blue top inhibitor.
            /// </summary>
            /// <value>the blue top inhibitor.</value>
            public static Vector2 Blue_Top_Inhibitor
            {
                get { return new Vector2(1170, 3570); }
            }

            /// <summary>
            /// Gets the blue mid inhibitor.
            /// </summary>
            /// <value>the blue mid inhibitor.</value>
            public static Vector2 Blue_Mid_Inhibitor
            {
                get { return new Vector2(3204, 3204); }
            }

            /// <summary>
            /// Gets the blue bottom inhibitor.
            /// </summary>
            /// <value>the blue bottom inhibitor.</value>
            public static Vector2 Blue_Bottom_Inhibitor
            {
                get { return new Vector2(3453, 1237); }
            }

            /// <summary>
            /// Gets the blue top nexus turret.
            /// </summary>
            /// <value>the blue top nexus turret.</value>
            public static Vector2 Blue_Top_Nexus_Turret
            {
                get { return new Vector2(1748, 2270); }
            }

            /// <summary>
            /// Gets the blue bottom nexus turret.
            /// </summary>
            /// <value>the blue bottom nexus turret.</value>
            public static Vector2 Blue_Bottom_Nexus_Turret
            {
                get { return new Vector2(2178, 1808); }
            }

            /// <summary>
            /// Gets the blue nexus.
            /// </summary>
            /// <value>the blue nexus.</value>
            public static Vector2 Blue_Nexus
            {
                get { return new Vector2(1550, 1660); }
            }

            /// <summary>
            /// Gets the blue spawn.
            /// </summary>
            /// <value>the blue spawn.</value>
            public static Geometry.Polygon Blue_Spawn
            {
                get { return new Geometry.Polygon.Circle(new Vector2(450, 500), 800); }
            }

            /// <summary>
            /// Gets the red top turret.
            /// </summary>
            /// <value>the red top turret.</value>
            public static Vector2 Red_Top_Turret
            {
                get { return new Vector2(10481, 13650); }
            }

            /// <summary>
            /// Gets the red mid turret.
            /// </summary>
            /// <value>the red mid turret.</value>
            public static Vector2 Red_Mid_Turret
            {
                get { return new Vector2(11135, 11208); }
            }

            /// <summary>
            /// Gets the red bottom turret.
            /// </summary>
            /// <value>the red bottom turret.</value>
            public static Vector2 Red_Bottom_Turret
            {
                get { return new Vector2(13625, 10573); }
            }

            /// <summary>
            /// Gets the red top inhibitor.
            /// </summary>
            /// <value>the red top inhibitor.</value>
            public static Vector2 Red_Top_Inhibitor
            {
                get { return new Vector2(11262, 13677); }
            }

            /// <summary>
            /// Gets the red mid inhibitor.
            /// </summary>
            /// <value>the red mid inhibitor.</value>
            public static Vector2 Red_Mid_Inhibitor
            {
                get { return new Vector2(11598, 11668); }
            }

            /// <summary>
            /// Gets the red bottom inhibitor.
            /// </summary>
            /// <value>the red bottom inhibitor.</value>
            public static Vector2 Red_Bottom_Inhibitor
            {
                get { return new Vector2(13605, 11316); }
            }

            /// <summary>
            /// Gets the red top nexus turret.
            /// </summary>
            /// <value>the red top nexus turret.</value>
            public static Vector2 Red_Top_Nexus_Turret
            {
                get { return new Vector2(12612, 13084); }
            }

            /// <summary>
            /// Gets the red bottom nexus turret.
            /// </summary>
            /// <value>the red bottom nexus turret.</value>
            public static Vector2 Red_Bottom_Nexus_Turret
            {
                get { return new Vector2(13053, 12612); }
            }

            /// <summary>
            /// Gets the red nexus.
            /// </summary>
            /// <value>the red nexus.</value>
            public static Vector2 Red_Nexus
            {
                get { return new Vector2(13243, 13235); }
            }

            /// <summary>
            /// Gets the red spawn.
            /// </summary>
            /// <value>the red spawn.</value>
            public static Geometry.Polygon Red_Spawn
            {
                get { return new Geometry.Polygon.Circle(new Vector2(14300, 14400), 800); }
            }
        }

        /// <summary>
        /// Gets data about the jungle.
        /// </summary>
        public static class Jungle
        {
            /// <summary>
            /// Gets the blue top jungle.
            /// </summary>
            /// <value>the blue top jungle.</value>
            public static Geometry.Polygon Blue_Top_Jungle
            {
                get
                {
                    var jungle = new Geometry.Polygon();
                    jungle.Add(new Vector2(5841, 6866));
                    jungle.Add(new Vector2(5183, 7505));
                    jungle.Add(new Vector2(5058, 8122));
                    jungle.Add(new Vector2(3806, 9045));
                    jungle.Add(new Vector2(3388, 9568));
                    jungle.Add(new Vector2(2939, 10882));
                    jungle.Add(new Vector2(2058, 10857));
                    jungle.Add(new Vector2(1904, 10578));
                    jungle.Add(new Vector2(1890, 9901));
                    jungle.Add(new Vector2(1896, 8235));
                    jungle.Add(new Vector2(1873, 7324));
                    jungle.Add(new Vector2(1813, 6685));
                    jungle.Add(new Vector2(1839, 6087));
                    jungle.Add(new Vector2(1849, 4874));
                    jungle.Add(new Vector2(2458, 4870));
                    jungle.Add(new Vector2(2978, 4793));
                    jungle.Add(new Vector2(3476, 4492));
                    return jungle;
                }
            }

            /// <summary>
            /// Gets the blue bottom jungle.
            /// </summary>
            /// <value>the blue bottom jungle.</value>
            public static Geometry.Polygon Blue_Bottom_Jungle
            {
                get
                {
                    var jungle = new Geometry.Polygon();
                    jungle.Add(new Vector2(10462, 1880));
                    jungle.Add(new Vector2(10426, 3110));
                    jungle.Add(new Vector2(10266, 3549));
                    jungle.Add(new Vector2(9874, 3538));
                    jungle.Add(new Vector2(9522, 3701));
                    jungle.Add(new Vector2(9291, 3969));
                    jungle.Add(new Vector2(9104, 4365));
                    jungle.Add(new Vector2(9114, 4576));
                    jungle.Add(new Vector2(9167, 4763));
                    jungle.Add(new Vector2(9153, 4912));
                    jungle.Add(new Vector2(9236, 4995));
                    jungle.Add(new Vector2(8760, 5243));
                    jungle.Add(new Vector2(7557, 5462));
                    jungle.Add(new Vector2(6957, 5943));
                    jungle.Add(new Vector2(6841, 6030));
                    jungle.Add(new Vector2(6356, 5668));
                    jungle.Add(new Vector2(6031, 5328));
                    jungle.Add(new Vector2(4971, 4267));
                    jungle.Add(new Vector2(4441, 3682));
                    jungle.Add(new Vector2(4780, 3082));
                    jungle.Add(new Vector2(4895, 2543));
                    jungle.Add(new Vector2(4888, 1864));
                    return jungle;
                }
            }

            /// <summary>
            /// Gets the red top jungle.
            /// </summary>
            /// <value>the red top jungle.</value>
            public static Geometry.Polygon Red_Top_Jungle
            {
                get
                {
                    var jungle = new Geometry.Polygon();
                    jungle.Add(new Vector2(8458, 9450));
                    jungle.Add(new Vector2(9287, 10244));
                    jungle.Add(new Vector2(10281, 11281));
                    jungle.Add(new Vector2(10045, 11867));
                    jungle.Add(new Vector2(9925, 12427));
                    jungle.Add(new Vector2(9845, 13030));
                    jungle.Add(new Vector2(9279, 13024));
                    jungle.Add(new Vector2(8343, 13050));
                    jungle.Add(new Vector2(7694, 13118));
                    jungle.Add(new Vector2(7097, 13141));
                    jungle.Add(new Vector2(6553, 13056));
                    jungle.Add(new Vector2(5987, 13016));
                    jungle.Add(new Vector2(5202, 12826));
                    jungle.Add(new Vector2(4354, 12895));
                    jungle.Add(new Vector2(4596, 11401));
                    jungle.Add(new Vector2(4873, 11346));
                    jungle.Add(new Vector2(5243, 11180));
                    jungle.Add(new Vector2(5567, 10971));
                    jungle.Add(new Vector2(5703, 10644));
                    jungle.Add(new Vector2(5700, 10238));
                    jungle.Add(new Vector2(5587, 9929));
                    jungle.Add(new Vector2(5978, 9608));
                    jungle.Add(new Vector2(7260, 9406));
                    jungle.Add(new Vector2(7944, 9001));
                    return jungle;
                }
            }

            /// <summary>
            /// Gets the red bottom jungle.
            /// </summary>
            /// <value>the red bottom jungle.</value>
            public static Geometry.Polygon Red_Bottom_Jungle
            {
                get
                {
                    var jungle = new Geometry.Polygon();
                    jungle.Add(new Vector2(12907, 4891));
                    jungle.Add(new Vector2(12919, 5602));
                    jungle.Add(new Vector2(12962, 5865));
                    jungle.Add(new Vector2(12946, 6566));
                    jungle.Add(new Vector2(12896, 7473));
                    jungle.Add(new Vector2(12960, 8012));
                    jungle.Add(new Vector2(12975, 8755));
                    jungle.Add(new Vector2(12958, 9991));
                    jungle.Add(new Vector2(12358, 10023));
                    jungle.Add(new Vector2(11790, 10166));
                    jungle.Add(new Vector2(11205, 10389));
                    jungle.Add(new Vector2(10657, 9691));
                    jungle.Add(new Vector2(10201, 9215));
                    jungle.Add(new Vector2(9532, 8585));
                    jungle.Add(new Vector2(9361, 8325));
                    jungle.Add(new Vector2(9563, 7590));
                    jungle.Add(new Vector2(9650, 6545));
                    jungle.Add(new Vector2(10276, 6136));
                    jungle.Add(new Vector2(10863, 5745));
                    jungle.Add(new Vector2(11461, 5157));
                    jungle.Add(new Vector2(11790, 4704));
                    jungle.Add(new Vector2(12163, 4245));
                    jungle.Add(new Vector2(12853, 4178));
                    return jungle;
                }
            }

            /// <summary>
            /// Gets the blue blue buff.
            /// </summary>
            /// <value>the blue blue buff.</value>
            public static Vector2 Blue_BlueBuff
            {
                get { return new Vector2(3910, 7920); }
            }

            /// <summary>
            /// Gets the red blue buff.
            /// </summary>
            /// <value>the red blue buff.</value>
            public static Vector2 Red_BlueBuff
            {
                get { return new Vector2(10830, 7000); }
            }

            /// <summary>
            /// Gets the blue red buff.
            /// </summary>
            /// <value>the blue red buff.</value>
            public static Vector2 Blue_RedBuff
            {
                get { return new Vector2(7890, 4120); }
            }

            /// <summary>
            /// Gets the red red buff.
            /// </summary>
            /// <value>the red red buff.</value>
            public static Vector2 Red_RedBuff
            {
                get { return new Vector2(6980, 10760); }
            }

            /// <summary>
            /// Gets the blue razor beak.
            /// </summary>
            /// <value>the blue razor beak.</value>
            public static Vector2 Blue_RazorBeak
            {
                get { return new Vector2(6830, 5458); }
            }

            /// <summary>
            /// Gets the blue gromp.
            /// </summary>
            /// <value>the blue gromp.</value>
            public static Vector2 Blue_Gromp
            {
                get { return new Vector2(2090, 8428); }
            }

            /// <summary>
            /// Gets the blue krug.
            /// </summary>
            /// <value>the blue krug.</value>
            public static Vector2 Blue_Krug
            {
                get { return new Vector2(8400, 2740); }
            }

            /// <summary>
            /// Gets the blue wolf.
            /// </summary>
            /// <value>the blue wolf.</value>
            public static Vector2 Blue_Wolf
            {
                get { return new Vector2(3781, 6444); }
            }

            /// <summary>
            /// Gets the red razor beak.
            /// </summary>
            /// <value>the red razor beak.</value>
            public static Vector2 Red_RazorBeak
            {
                get { return new Vector2(7800, 9460); }
            }

            /// <summary>
            /// Gets the red gromp.
            /// </summary>
            /// <value>the red gromp.</value>
            public static Vector2 Red_Gromp
            {
                get { return new Vector2(12704, 6444); }
            }

            /// <summary>
            /// Gets the red krug.
            /// </summary>
            /// <value>the red krug.</value>
            public static Vector2 Red_Krug
            {
                get { return new Vector2(6400, 12150); }
            }

            /// <summary>
            /// Gets the red wolf.
            /// </summary>
            /// <value>the red wolf.</value>
            public static Vector2 Red_Wolf
            {
                get { return new Vector2(11000, 8380); }
            }
        }

        /// <summary>
        /// Class River.
        /// </summary>
        public static class River
        {
            /// <summary>
            /// Gets the top.
            /// </summary>
            /// <value>The top.</value>
            public static Geometry.Polygon Top
            {
                get
                {
                    var river = new Geometry.Polygon();
                    river.Add(new Vector2(6458, 7414));
                    river.Add(new Vector2(7413, 8197));
                    river.Add(new Vector2(8002, 8915));
                    river.Add(new Vector2(7261, 9395));
                    river.Add(new Vector2(5979, 9622));
                    river.Add(new Vector2(5468, 9981));
                    river.Add(new Vector2(5690, 10307));
                    river.Add(new Vector2(5616, 10697));
                    river.Add(new Vector2(5284, 11078));
                    river.Add(new Vector2(4760, 11184));
                    river.Add(new Vector2(4464, 11867));
                    river.Add(new Vector2(4313, 12918));
                    river.Add(new Vector2(3246, 12423));
                    river.Add(new Vector2(2241, 11594));
                    river.Add(new Vector2(2638, 11283));
                    river.Add(new Vector2(2842, 10879));
                    river.Add(new Vector2(3052, 10351));
                    river.Add(new Vector2(3178, 10071));
                    river.Add(new Vector2(3260, 9906));
                    river.Add(new Vector2(3434, 9516));
                    river.Add(new Vector2(3698, 9230));
                    river.Add(new Vector2(4077, 9056));
                    river.Add(new Vector2(4272, 8931));
                    river.Add(new Vector2(4534, 8726));
                    river.Add(new Vector2(4204, 8446));
                    river.Add(new Vector2(4268, 8092));
                    river.Add(new Vector2(4405, 7815));
                    river.Add(new Vector2(4526, 7665));
                    river.Add(new Vector2(5163, 7419));
                    river.Add(new Vector2(5505, 7223));
                    river.Add(new Vector2(5806, 6875));
                    return river;
                }
            }

            /// <summary>
            /// Gets the bottom.
            /// </summary>
            /// <value>The bottom.</value>
            public static Geometry.Polygon Bottom
            {
                get
                {
                    var river = new Geometry.Polygon();
                    river.Add(new Vector2(12532, 3329));
                    river.Add(new Vector2(12301, 3580));
                    river.Add(new Vector2(12151, 4180));
                    river.Add(new Vector2(12071, 4357));
                    river.Add(new Vector2(11914, 4590));
                    river.Add(new Vector2(11770, 4779));
                    river.Add(new Vector2(11527, 5026));
                    river.Add(new Vector2(11473, 5362));
                    river.Add(new Vector2(10904, 5759));
                    river.Add(new Vector2(10418, 6133));
                    river.Add(new Vector2(10568, 6549));
                    river.Add(new Vector2(10550, 6881));
                    river.Add(new Vector2(10426, 7173));
                    river.Add(new Vector2(9750, 7518));
                    river.Add(new Vector2(9082, 8039));
                    river.Add(new Vector2(8443, 7298));
                    river.Add(new Vector2(7428, 6596));
                    river.Add(new Vector2(6775, 6001));
                    river.Add(new Vector2(7513, 5425));
                    river.Add(new Vector2(8808, 5229));
                    river.Add(new Vector2(9320, 4965));
                    river.Add(new Vector2(9207, 4679));
                    river.Add(new Vector2(9110, 4414));
                    river.Add(new Vector2(9138, 4129));
                    river.Add(new Vector2(9323, 3837));
                    river.Add(new Vector2(9616, 3581));
                    river.Add(new Vector2(9994, 3427));
                    river.Add(new Vector2(10381, 3059));
                    river.Add(new Vector2(10704, 3067));
                    river.Add(new Vector2(11032, 3116));
                    river.Add(new Vector2(11637, 2694));
                    return river;
                }
            }

            /// <summary>
            /// Gets the dragon.
            /// </summary>
            /// <value>The dragon.</value>
            public static Vector2 Dragon
            {
                get { return new Vector2(9860, 4410); }
            }

            /// <summary>
            /// Gets the dragon pit.
            /// </summary>
            /// <value>the dragon pit.</value>
            public static Geometry.Polygon Dragon_Pit
            {
                get { return new Geometry.Polygon.Circle(new Vector2(9900, 4400), 650); }
            }

            /// <summary>
            /// Gets the dragon contest zone.
            /// </summary>
            /// <value>the dragon contest_ zone.</value>
            public static Geometry.Polygon Dragon_Contest_Zone
            {
                get
                {
                    var dragon = new Geometry.Polygon();
                    dragon.Add(new Vector2(10743, 3073));
                    dragon.Add(new Vector2(12150, 3651));
                    dragon.Add(new Vector2(12037, 4128));
                    dragon.Add(new Vector2(11821, 4660));
                    dragon.Add(new Vector2(11483, 5052));
                    dragon.Add(new Vector2(11514, 5498));
                    dragon.Add(new Vector2(11094, 5850));
                    dragon.Add(new Vector2(10317, 6166));
                    dragon.Add(new Vector2(9616, 6451));
                    dragon.Add(new Vector2(8644, 5624));
                    dragon.Add(new Vector2(8321, 5263));
                    dragon.Add(new Vector2(8376, 4651));
                    dragon.Add(new Vector2(8454, 4517));
                    dragon.Add(new Vector2(8407, 3914));
                    dragon.Add(new Vector2(8981, 3277));
                    dragon.Add(new Vector2(10031, 2859));
                    return dragon;
                }
            }

            /// <summary>
            /// Gets the baron.
            /// </summary>
            /// <value>The baron.</value>
            public static Vector2 Baron
            {
                get { return new Vector2(4950, 10400); }
            }

            /// <summary>
            /// Gets the baron pit.
            /// </summary>
            /// <value>the baron pit.</value>
            public static Geometry.Polygon Baron_Pit
            {
                get { return new Geometry.Polygon.Circle(new Vector2(4950, 10400), 650); }
            }

            /// <summary>
            /// Gets the baron contest zone.
            /// </summary>
            /// <value>the baron contest zone.</value>
            public static Geometry.Polygon Baron_Contest_Zone
            {
                get
                {
                    var baron = new Geometry.Polygon();
                    baron.Add(new Vector2(6640, 8974));
                    baron.Add(new Vector2(6623, 10325));
                    baron.Add(new Vector2(6631, 11153));
                    baron.Add(new Vector2(5977, 11575));
                    baron.Add(new Vector2(5582, 11789));
                    baron.Add(new Vector2(4737, 12101));
                    baron.Add(new Vector2(3827, 11739));
                    baron.Add(new Vector2(2830, 10905));
                    baron.Add(new Vector2(3053, 10260));
                    baron.Add(new Vector2(3336, 9367));
                    baron.Add(new Vector2(3883, 9042));
                    baron.Add(new Vector2(4388, 8722));
                    baron.Add(new Vector2(5214, 8318));
                    return baron;
                }
            }
        }

        /// <summary>
        /// Class Bushes.
        /// </summary>
        public static class Bushes
        {
            /// <summary>
            /// Gets the top lane blue inner bush.
            /// </summary>
            /// <value>The top lane blue inner bush.</value>
            public static Geometry.Polygon TopLane_BlueInnerBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(807, 7836));
                    bush.Add(new Vector2(807, 7886));
                    bush.Add(new Vector2(907, 7886));
                    bush.Add(new Vector2(907, 7936));
                    bush.Add(new Vector2(957, 7936));
                    bush.Add(new Vector2(957, 7986));
                    bush.Add(new Vector2(907, 7986));
                    bush.Add(new Vector2(907, 8386));
                    bush.Add(new Vector2(857, 8386));
                    bush.Add(new Vector2(857, 8436));
                    bush.Add(new Vector2(757, 8436));
                    bush.Add(new Vector2(757, 8486));
                    bush.Add(new Vector2(702, 8486));
                    bush.Add(new Vector2(702, 7836));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top lane contest blue bush.
            /// </summary>
            /// <value>The top lane contest blue bush.</value>
            public static Geometry.Polygon TopLane_ContestBlueBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(1100, 12532));
                    bush.Add(new Vector2(1100, 12432));
                    bush.Add(new Vector2(1050, 12432));
                    bush.Add(new Vector2(1050, 12282));
                    bush.Add(new Vector2(1000, 12282));
                    bush.Add(new Vector2(1000, 12082));
                    bush.Add(new Vector2(1050, 12082));
                    bush.Add(new Vector2(1050, 12032));
                    bush.Add(new Vector2(1255, 12032));
                    bush.Add(new Vector2(1255, 12132));
                    bush.Add(new Vector2(1305, 12132));
                    bush.Add(new Vector2(1305, 12232));
                    bush.Add(new Vector2(1355, 12232));
                    bush.Add(new Vector2(1355, 12382));
                    bush.Add(new Vector2(1405, 12382));
                    bush.Add(new Vector2(1405, 12482));
                    bush.Add(new Vector2(1355, 12482));
                    bush.Add(new Vector2(1355, 12532));
                    bush.Add(new Vector2(1305, 12532));
                    bush.Add(new Vector2(1305, 12582));
                    bush.Add(new Vector2(1150, 12582));
                    bush.Add(new Vector2(1150, 12532));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top lane contest middle bush.
            /// </summary>
            /// <value>The top lane contest middle bush.</value>
            public static Geometry.Polygon TopLane_ContestMiddleBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(1648, 13234));
                    bush.Add(new Vector2(1648, 13184));
                    bush.Add(new Vector2(1598, 13184));
                    bush.Add(new Vector2(1598, 13134));
                    bush.Add(new Vector2(1548, 13134));
                    bush.Add(new Vector2(1548, 13084));
                    bush.Add(new Vector2(1498, 13084));
                    bush.Add(new Vector2(1498, 13034));
                    bush.Add(new Vector2(1448, 13034));
                    bush.Add(new Vector2(1448, 12984));
                    bush.Add(new Vector2(1398, 12984));
                    bush.Add(new Vector2(1398, 12934));
                    bush.Add(new Vector2(1448, 12934));
                    bush.Add(new Vector2(1448, 12884));
                    bush.Add(new Vector2(1498, 12884));
                    bush.Add(new Vector2(1498, 12834));
                    bush.Add(new Vector2(1548, 12834));
                    bush.Add(new Vector2(1548, 12784));
                    bush.Add(new Vector2(1598, 12784));
                    bush.Add(new Vector2(1598, 12734));
                    bush.Add(new Vector2(1653, 12734));
                    bush.Add(new Vector2(1653, 12784));
                    bush.Add(new Vector2(1703, 12784));
                    bush.Add(new Vector2(1703, 12834));
                    bush.Add(new Vector2(1753, 12834));
                    bush.Add(new Vector2(1753, 12884));
                    bush.Add(new Vector2(1803, 12884));
                    bush.Add(new Vector2(1803, 12934));
                    bush.Add(new Vector2(1853, 12934));
                    bush.Add(new Vector2(1853, 12984));
                    bush.Add(new Vector2(1903, 12984));
                    bush.Add(new Vector2(1903, 13034));
                    bush.Add(new Vector2(1953, 13034));
                    bush.Add(new Vector2(1953, 13084));
                    bush.Add(new Vector2(1903, 13084));
                    bush.Add(new Vector2(1903, 13134));
                    bush.Add(new Vector2(1853, 13134));
                    bush.Add(new Vector2(1853, 13184));
                    bush.Add(new Vector2(1803, 13184));
                    bush.Add(new Vector2(1803, 13234));
                    bush.Add(new Vector2(1753, 13234));
                    bush.Add(new Vector2(1753, 13284));
                    bush.Add(new Vector2(1698, 13284));
                    bush.Add(new Vector2(1698, 13234));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top lane contest red bush.
            /// </summary>
            /// <value>The top lane contest red bush.</value>
            public static Geometry.Polygon TopLane_ContestRedBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(2399, 13737));
                    bush.Add(new Vector2(2399, 13687));
                    bush.Add(new Vector2(2299, 13687));
                    bush.Add(new Vector2(2299, 13637));
                    bush.Add(new Vector2(2199, 13637));
                    bush.Add(new Vector2(2199, 13587));
                    bush.Add(new Vector2(2149, 13587));
                    bush.Add(new Vector2(2149, 13537));
                    bush.Add(new Vector2(2049, 13537));
                    bush.Add(new Vector2(2049, 13487));
                    bush.Add(new Vector2(2099, 13487));
                    bush.Add(new Vector2(2099, 13387));
                    bush.Add(new Vector2(2149, 13387));
                    bush.Add(new Vector2(2149, 13337));
                    bush.Add(new Vector2(2199, 13337));
                    bush.Add(new Vector2(2199, 13287));
                    bush.Add(new Vector2(2249, 13287));
                    bush.Add(new Vector2(2249, 13237));
                    bush.Add(new Vector2(2304, 13237));
                    bush.Add(new Vector2(2304, 13287));
                    bush.Add(new Vector2(2354, 13287));
                    bush.Add(new Vector2(2354, 13337));
                    bush.Add(new Vector2(2404, 13337));
                    bush.Add(new Vector2(2404, 13387));
                    bush.Add(new Vector2(2504, 13387));
                    bush.Add(new Vector2(2504, 13437));
                    bush.Add(new Vector2(2604, 13437));
                    bush.Add(new Vector2(2604, 13487));
                    bush.Add(new Vector2(2654, 13487));
                    bush.Add(new Vector2(2654, 13537));
                    bush.Add(new Vector2(2704, 13537));
                    bush.Add(new Vector2(2704, 13637));
                    bush.Add(new Vector2(2654, 13637));
                    bush.Add(new Vector2(2654, 13737));
                    bush.Add(new Vector2(2604, 13737));
                    bush.Add(new Vector2(2604, 13787));
                    bush.Add(new Vector2(2499, 13787));
                    bush.Add(new Vector2(2499, 13737));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top lane red inner bush.
            /// </summary>
            /// <value>The top lane red inner bush.</value>
            public static Geometry.Polygon TopLane_RedInnerBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(7101, 14034));
                    bush.Add(new Vector2(7101, 13984));
                    bush.Add(new Vector2(7256, 13984));
                    bush.Add(new Vector2(7256, 14034));
                    bush.Add(new Vector2(7356, 14034));
                    bush.Add(new Vector2(7356, 14084));
                    bush.Add(new Vector2(7406, 14084));
                    bush.Add(new Vector2(7406, 14134));
                    bush.Add(new Vector2(6951, 14134));
                    bush.Add(new Vector2(6951, 14034));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom lane blue inner bush.
            /// </summary>
            /// <value>The bottom lane blue inner bush.</value>
            public static Geometry.Polygon BottomLane_BlueInnerBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(7601, 837));
                    bush.Add(new Vector2(7601, 737));
                    bush.Add(new Vector2(8006, 737));
                    bush.Add(new Vector2(8006, 887));
                    bush.Add(new Vector2(7651, 887));
                    bush.Add(new Vector2(7651, 837));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom lane contest blue bush.
            /// </summary>
            /// <value>The bottom lane contest blue bush.</value>
            public static Geometry.Polygon BottomLane_ContestBlueBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(12652, 1833));
                    bush.Add(new Vector2(12652, 1783));
                    bush.Add(new Vector2(12602, 1783));
                    bush.Add(new Vector2(12602, 1733));
                    bush.Add(new Vector2(12552, 1733));
                    bush.Add(new Vector2(12552, 1683));
                    bush.Add(new Vector2(12502, 1683));
                    bush.Add(new Vector2(12502, 1633));
                    bush.Add(new Vector2(12452, 1633));
                    bush.Add(new Vector2(12452, 1583));
                    bush.Add(new Vector2(12402, 1583));
                    bush.Add(new Vector2(12402, 1533));
                    bush.Add(new Vector2(12302, 1533));
                    bush.Add(new Vector2(12302, 1483));
                    bush.Add(new Vector2(12252, 1483));
                    bush.Add(new Vector2(12252, 1433));
                    bush.Add(new Vector2(12202, 1433));
                    bush.Add(new Vector2(12202, 1383));
                    bush.Add(new Vector2(12152, 1383));
                    bush.Add(new Vector2(12152, 1333));
                    bush.Add(new Vector2(12102, 1333));
                    bush.Add(new Vector2(12102, 1283));
                    bush.Add(new Vector2(12152, 1283));
                    bush.Add(new Vector2(12152, 1233));
                    bush.Add(new Vector2(12202, 1233));
                    bush.Add(new Vector2(12202, 1183));
                    bush.Add(new Vector2(12252, 1183));
                    bush.Add(new Vector2(12252, 1133));
                    bush.Add(new Vector2(12307, 1133));
                    bush.Add(new Vector2(12307, 1183));
                    bush.Add(new Vector2(12357, 1183));
                    bush.Add(new Vector2(12357, 1233));
                    bush.Add(new Vector2(12457, 1233));
                    bush.Add(new Vector2(12457, 1283));
                    bush.Add(new Vector2(12507, 1283));
                    bush.Add(new Vector2(12507, 1333));
                    bush.Add(new Vector2(12607, 1333));
                    bush.Add(new Vector2(12607, 1383));
                    bush.Add(new Vector2(12657, 1383));
                    bush.Add(new Vector2(12657, 1433));
                    bush.Add(new Vector2(12707, 1433));
                    bush.Add(new Vector2(12707, 1483));
                    bush.Add(new Vector2(12757, 1483));
                    bush.Add(new Vector2(12757, 1533));
                    bush.Add(new Vector2(12807, 1533));
                    bush.Add(new Vector2(12807, 1583));
                    bush.Add(new Vector2(12857, 1583));
                    bush.Add(new Vector2(12857, 1633));
                    bush.Add(new Vector2(12907, 1633));
                    bush.Add(new Vector2(12907, 1683));
                    bush.Add(new Vector2(12957, 1683));
                    bush.Add(new Vector2(12957, 1733));
                    bush.Add(new Vector2(12907, 1733));
                    bush.Add(new Vector2(12907, 1783));
                    bush.Add(new Vector2(12857, 1783));
                    bush.Add(new Vector2(12857, 1833));
                    bush.Add(new Vector2(12807, 1833));
                    bush.Add(new Vector2(12807, 1883));
                    bush.Add(new Vector2(12702, 1883));
                    bush.Add(new Vector2(12702, 1833));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom lane contest red bush.
            /// </summary>
            /// <value>The bottom lane contest red bush.</value>
            public static Geometry.Polygon BottomLane_ContestRedBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(13453, 2835));
                    bush.Add(new Vector2(13453, 2785));
                    bush.Add(new Vector2(13403, 2785));
                    bush.Add(new Vector2(13403, 2685));
                    bush.Add(new Vector2(13353, 2685));
                    bush.Add(new Vector2(13353, 2585));
                    bush.Add(new Vector2(13303, 2585));
                    bush.Add(new Vector2(13303, 2535));
                    bush.Add(new Vector2(13253, 2535));
                    bush.Add(new Vector2(13253, 2435));
                    bush.Add(new Vector2(13203, 2435));
                    bush.Add(new Vector2(13203, 2385));
                    bush.Add(new Vector2(13153, 2385));
                    bush.Add(new Vector2(13153, 2335));
                    bush.Add(new Vector2(13103, 2335));
                    bush.Add(new Vector2(13103, 2285));
                    bush.Add(new Vector2(13053, 2285));
                    bush.Add(new Vector2(13053, 2235));
                    bush.Add(new Vector2(13003, 2235));
                    bush.Add(new Vector2(13003, 2185));
                    bush.Add(new Vector2(13053, 2185));
                    bush.Add(new Vector2(13053, 2135));
                    bush.Add(new Vector2(13103, 2135));
                    bush.Add(new Vector2(13103, 2085));
                    bush.Add(new Vector2(13153, 2085));
                    bush.Add(new Vector2(13153, 2035));
                    bush.Add(new Vector2(13258, 2035));
                    bush.Add(new Vector2(13258, 2085));
                    bush.Add(new Vector2(13308, 2085));
                    bush.Add(new Vector2(13308, 2135));
                    bush.Add(new Vector2(13358, 2135));
                    bush.Add(new Vector2(13358, 2185));
                    bush.Add(new Vector2(13408, 2185));
                    bush.Add(new Vector2(13408, 2285));
                    bush.Add(new Vector2(13458, 2285));
                    bush.Add(new Vector2(13458, 2335));
                    bush.Add(new Vector2(13508, 2335));
                    bush.Add(new Vector2(13508, 2435));
                    bush.Add(new Vector2(13558, 2435));
                    bush.Add(new Vector2(13558, 2485));
                    bush.Add(new Vector2(13608, 2485));
                    bush.Add(new Vector2(13608, 2585));
                    bush.Add(new Vector2(13658, 2585));
                    bush.Add(new Vector2(13658, 2685));
                    bush.Add(new Vector2(13708, 2685));
                    bush.Add(new Vector2(13708, 2735));
                    bush.Add(new Vector2(13658, 2735));
                    bush.Add(new Vector2(13658, 2785));
                    bush.Add(new Vector2(13608, 2785));
                    bush.Add(new Vector2(13608, 2835));
                    bush.Add(new Vector2(13558, 2835));
                    bush.Add(new Vector2(13558, 2885));
                    bush.Add(new Vector2(13503, 2885));
                    bush.Add(new Vector2(13503, 2835));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom lane red inner bush.
            /// </summary>
            /// <value>The bottom lane red inner bush.</value>
            public static Geometry.Polygon BottomLane_RedInnerBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(14053, 7183));
                    bush.Add(new Vector2(14053, 7133));
                    bush.Add(new Vector2(14003, 7133));
                    bush.Add(new Vector2(14003, 6783));
                    bush.Add(new Vector2(14103, 6783));
                    bush.Add(new Vector2(14103, 6733));
                    bush.Add(new Vector2(14158, 6733));
                    bush.Add(new Vector2(14158, 7233));
                    bush.Add(new Vector2(14103, 7233));
                    bush.Add(new Vector2(14103, 7183));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top river top bush.
            /// </summary>
            /// <value>The top river top bush.</value>
            public static Geometry.Polygon TopRiver_TopBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(2851, 11435));
                    bush.Add(new Vector2(2851, 11385));
                    bush.Add(new Vector2(2801, 11385));
                    bush.Add(new Vector2(2801, 11335));
                    bush.Add(new Vector2(2751, 11335));
                    bush.Add(new Vector2(2751, 11285));
                    bush.Add(new Vector2(2801, 11285));
                    bush.Add(new Vector2(2801, 11185));
                    bush.Add(new Vector2(2851, 11185));
                    bush.Add(new Vector2(2851, 11085));
                    bush.Add(new Vector2(2901, 11085));
                    bush.Add(new Vector2(2901, 10985));
                    bush.Add(new Vector2(2951, 10985));
                    bush.Add(new Vector2(2951, 10885));
                    bush.Add(new Vector2(3001, 10885));
                    bush.Add(new Vector2(3001, 10785));
                    bush.Add(new Vector2(3051, 10785));
                    bush.Add(new Vector2(3051, 10685));
                    bush.Add(new Vector2(3106, 10685));
                    bush.Add(new Vector2(3106, 10735));
                    bush.Add(new Vector2(3206, 10735));
                    bush.Add(new Vector2(3206, 10835));
                    bush.Add(new Vector2(3156, 10835));
                    bush.Add(new Vector2(3156, 10985));
                    bush.Add(new Vector2(3106, 10985));
                    bush.Add(new Vector2(3106, 11085));
                    bush.Add(new Vector2(3056, 11085));
                    bush.Add(new Vector2(3056, 11285));
                    bush.Add(new Vector2(3006, 11285));
                    bush.Add(new Vector2(3006, 11435));
                    bush.Add(new Vector2(2956, 11435));
                    bush.Add(new Vector2(2956, 11485));
                    bush.Add(new Vector2(2901, 11485));
                    bush.Add(new Vector2(2901, 11435));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top river middle bush.
            /// </summary>
            /// <value>The top river middle bush.</value>
            public static Geometry.Polygon TopRiver_MiddleBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(5152, 9035));
                    bush.Add(new Vector2(5152, 8985));
                    bush.Add(new Vector2(5307, 8985));
                    bush.Add(new Vector2(5307, 9035));
                    bush.Add(new Vector2(5357, 9035));
                    bush.Add(new Vector2(5357, 9085));
                    bush.Add(new Vector2(5407, 9085));
                    bush.Add(new Vector2(5407, 9235));
                    bush.Add(new Vector2(5357, 9235));
                    bush.Add(new Vector2(5357, 9285));
                    bush.Add(new Vector2(5102, 9285));
                    bush.Add(new Vector2(5102, 9035));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top river bottom bush.
            /// </summary>
            /// <value>The top river bottom bush.</value>
            public static Geometry.Polygon TopRiver_BottomBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(6703, 8635));
                    bush.Add(new Vector2(6703, 8585));
                    bush.Add(new Vector2(6653, 8585));
                    bush.Add(new Vector2(6653, 8535));
                    bush.Add(new Vector2(6553, 8535));
                    bush.Add(new Vector2(6553, 8485));
                    bush.Add(new Vector2(6503, 8485));
                    bush.Add(new Vector2(6503, 8435));
                    bush.Add(new Vector2(6453, 8435));
                    bush.Add(new Vector2(6453, 8385));
                    bush.Add(new Vector2(6403, 8385));
                    bush.Add(new Vector2(6403, 8335));
                    bush.Add(new Vector2(6353, 8335));
                    bush.Add(new Vector2(6353, 8285));
                    bush.Add(new Vector2(6253, 8285));
                    bush.Add(new Vector2(6253, 8235));
                    bush.Add(new Vector2(6203, 8235));
                    bush.Add(new Vector2(6203, 8185));
                    bush.Add(new Vector2(6103, 8185));
                    bush.Add(new Vector2(6103, 8135));
                    bush.Add(new Vector2(6153, 8135));
                    bush.Add(new Vector2(6153, 8085));
                    bush.Add(new Vector2(6253, 8085));
                    bush.Add(new Vector2(6253, 8035));
                    bush.Add(new Vector2(6408, 8035));
                    bush.Add(new Vector2(6408, 8085));
                    bush.Add(new Vector2(6458, 8085));
                    bush.Add(new Vector2(6458, 8135));
                    bush.Add(new Vector2(6508, 8135));
                    bush.Add(new Vector2(6508, 8185));
                    bush.Add(new Vector2(6608, 8185));
                    bush.Add(new Vector2(6608, 8235));
                    bush.Add(new Vector2(6658, 8235));
                    bush.Add(new Vector2(6658, 8285));
                    bush.Add(new Vector2(6708, 8285));
                    bush.Add(new Vector2(6708, 8335));
                    bush.Add(new Vector2(6758, 8335));
                    bush.Add(new Vector2(6758, 8385));
                    bush.Add(new Vector2(6808, 8385));
                    bush.Add(new Vector2(6808, 8435));
                    bush.Add(new Vector2(6858, 8435));
                    bush.Add(new Vector2(6858, 8485));
                    bush.Add(new Vector2(6908, 8485));
                    bush.Add(new Vector2(6908, 8535));
                    bush.Add(new Vector2(6958, 8535));
                    bush.Add(new Vector2(6958, 8585));
                    bush.Add(new Vector2(6908, 8585));
                    bush.Add(new Vector2(6908, 8635));
                    bush.Add(new Vector2(6808, 8635));
                    bush.Add(new Vector2(6808, 8685));
                    bush.Add(new Vector2(6753, 8685));
                    bush.Add(new Vector2(6753, 8635));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom river top bush.
            /// </summary>
            /// <value>The bottom river top bush.</value>
            public static Geometry.Polygon BottomRiver_TopBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(8551, 6733));
                    bush.Add(new Vector2(8551, 6683));
                    bush.Add(new Vector2(8501, 6683));
                    bush.Add(new Vector2(8501, 6633));
                    bush.Add(new Vector2(8451, 6633));
                    bush.Add(new Vector2(8451, 6583));
                    bush.Add(new Vector2(8401, 6583));
                    bush.Add(new Vector2(8401, 6533));
                    bush.Add(new Vector2(8301, 6533));
                    bush.Add(new Vector2(8301, 6483));
                    bush.Add(new Vector2(8251, 6483));
                    bush.Add(new Vector2(8251, 6433));
                    bush.Add(new Vector2(8151, 6433));
                    bush.Add(new Vector2(8151, 6383));
                    bush.Add(new Vector2(8101, 6383));
                    bush.Add(new Vector2(8101, 6333));
                    bush.Add(new Vector2(8051, 6333));
                    bush.Add(new Vector2(8051, 6283));
                    bush.Add(new Vector2(8001, 6283));
                    bush.Add(new Vector2(8001, 6233));
                    bush.Add(new Vector2(7951, 6233));
                    bush.Add(new Vector2(7951, 6183));
                    bush.Add(new Vector2(8001, 6183));
                    bush.Add(new Vector2(8001, 6133));
                    bush.Add(new Vector2(8101, 6133));
                    bush.Add(new Vector2(8101, 6083));
                    bush.Add(new Vector2(8206, 6083));
                    bush.Add(new Vector2(8206, 6133));
                    bush.Add(new Vector2(8256, 6133));
                    bush.Add(new Vector2(8256, 6233));
                    bush.Add(new Vector2(8306, 6233));
                    bush.Add(new Vector2(8306, 6283));
                    bush.Add(new Vector2(8356, 6283));
                    bush.Add(new Vector2(8356, 6333));
                    bush.Add(new Vector2(8456, 6333));
                    bush.Add(new Vector2(8456, 6383));
                    bush.Add(new Vector2(8556, 6383));
                    bush.Add(new Vector2(8556, 6433));
                    bush.Add(new Vector2(8606, 6433));
                    bush.Add(new Vector2(8606, 6483));
                    bush.Add(new Vector2(8656, 6483));
                    bush.Add(new Vector2(8656, 6533));
                    bush.Add(new Vector2(8706, 6533));
                    bush.Add(new Vector2(8706, 6583));
                    bush.Add(new Vector2(8756, 6583));
                    bush.Add(new Vector2(8756, 6633));
                    bush.Add(new Vector2(8806, 6633));
                    bush.Add(new Vector2(8806, 6733));
                    bush.Add(new Vector2(8706, 6733));
                    bush.Add(new Vector2(8706, 6783));
                    bush.Add(new Vector2(8601, 6783));
                    bush.Add(new Vector2(8601, 6733));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom river middle bush.
            /// </summary>
            /// <value>The bottom river middle bush.</value>
            public static Geometry.Polygon BottomRiver_MiddleBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(9299, 5736));
                    bush.Add(new Vector2(9299, 5586));
                    bush.Add(new Vector2(9349, 5586));
                    bush.Add(new Vector2(9349, 5536));
                    bush.Add(new Vector2(9399, 5536));
                    bush.Add(new Vector2(9399, 5486));
                    bush.Add(new Vector2(9554, 5486));
                    bush.Add(new Vector2(9554, 5536));
                    bush.Add(new Vector2(9604, 5536));
                    bush.Add(new Vector2(9604, 5636));
                    bush.Add(new Vector2(9554, 5636));
                    bush.Add(new Vector2(9554, 5736));
                    bush.Add(new Vector2(9504, 5736));
                    bush.Add(new Vector2(9504, 5786));
                    bush.Add(new Vector2(9349, 5786));
                    bush.Add(new Vector2(9349, 5736));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom river bottom bush.
            /// </summary>
            /// <value>The bottom river bottom bush.</value>
            public static Geometry.Polygon BottomRiver_BottomBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(11751, 4134));
                    bush.Add(new Vector2(11751, 4084));
                    bush.Add(new Vector2(11701, 4084));
                    bush.Add(new Vector2(11701, 3884));
                    bush.Add(new Vector2(11751, 3884));
                    bush.Add(new Vector2(11751, 3684));
                    bush.Add(new Vector2(11801, 3684));
                    bush.Add(new Vector2(11801, 3584));
                    bush.Add(new Vector2(12006, 3584));
                    bush.Add(new Vector2(12006, 3634));
                    bush.Add(new Vector2(12056, 3634));
                    bush.Add(new Vector2(12056, 3784));
                    bush.Add(new Vector2(12006, 3784));
                    bush.Add(new Vector2(12006, 3934));
                    bush.Add(new Vector2(11956, 3934));
                    bush.Add(new Vector2(11956, 4184));
                    bush.Add(new Vector2(11901, 4184));
                    bush.Add(new Vector2(11901, 4134));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top blue jungle tri bush.
            /// </summary>
            /// <value>The top blue jungle tri bush.</value>
            public static Geometry.Polygon TopBlueJungle_TriBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(2149, 10086));
                    bush.Add(new Vector2(2149, 9486));
                    bush.Add(new Vector2(2199, 9486));
                    bush.Add(new Vector2(2199, 9436));
                    bush.Add(new Vector2(2304, 9436));
                    bush.Add(new Vector2(2304, 9486));
                    bush.Add(new Vector2(2354, 9486));
                    bush.Add(new Vector2(2354, 9586));
                    bush.Add(new Vector2(2454, 9586));
                    bush.Add(new Vector2(2454, 9636));
                    bush.Add(new Vector2(2604, 9636));
                    bush.Add(new Vector2(2604, 9836));
                    bush.Add(new Vector2(2404, 9836));
                    bush.Add(new Vector2(2404, 9886));
                    bush.Add(new Vector2(2354, 9886));
                    bush.Add(new Vector2(2354, 10136));
                    bush.Add(new Vector2(2199, 10136));
                    bush.Add(new Vector2(2199, 10086));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top blue jungle blue buff bush.
            /// </summary>
            /// <value>The top blue jungle blue buff bush.</value>
            public static Geometry.Polygon TopBlueJungle_BlueBuffBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(3252, 7785));
                    bush.Add(new Vector2(3252, 7685));
                    bush.Add(new Vector2(3607, 7685));
                    bush.Add(new Vector2(3607, 7735));
                    bush.Add(new Vector2(3557, 7735));
                    bush.Add(new Vector2(3557, 7885));
                    bush.Add(new Vector2(3202, 7885));
                    bush.Add(new Vector2(3202, 7785));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top blue jungle river entrance bush.
            /// </summary>
            /// <value>The top blue jungle river entrance bush.</value>
            public static Geometry.Polygon TopBlueJungle_RiverEntranceBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(4902, 8537));
                    bush.Add(new Vector2(4902, 8487));
                    bush.Add(new Vector2(4852, 8487));
                    bush.Add(new Vector2(4852, 8437));
                    bush.Add(new Vector2(4802, 8437));
                    bush.Add(new Vector2(4802, 8287));
                    bush.Add(new Vector2(4957, 8287));
                    bush.Add(new Vector2(4957, 8337));
                    bush.Add(new Vector2(5007, 8337));
                    bush.Add(new Vector2(5007, 8387));
                    bush.Add(new Vector2(5057, 8387));
                    bush.Add(new Vector2(5057, 8437));
                    bush.Add(new Vector2(5157, 8437));
                    bush.Add(new Vector2(5157, 8487));
                    bush.Add(new Vector2(5307, 8487));
                    bush.Add(new Vector2(5307, 8587));
                    bush.Add(new Vector2(5002, 8587));
                    bush.Add(new Vector2(5002, 8537));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top blue jungle wolf bush.
            /// </summary>
            /// <value>The top blue jungle wolf bush.</value>
            public static Geometry.Polygon TopBlueJungle_WolfBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(4700, 7086));
                    bush.Add(new Vector2(4700, 7036));
                    bush.Add(new Vector2(4750, 7036));
                    bush.Add(new Vector2(4750, 6986));
                    bush.Add(new Vector2(4800, 6986));
                    bush.Add(new Vector2(4800, 6936));
                    bush.Add(new Vector2(4955, 6936));
                    bush.Add(new Vector2(4955, 7136));
                    bush.Add(new Vector2(4905, 7136));
                    bush.Add(new Vector2(4905, 7186));
                    bush.Add(new Vector2(4855, 7186));
                    bush.Add(new Vector2(4855, 7236));
                    bush.Add(new Vector2(4805, 7236));
                    bush.Add(new Vector2(4805, 7286));
                    bush.Add(new Vector2(4650, 7286));
                    bush.Add(new Vector2(4650, 7086));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle raptor bush.
            /// </summary>
            /// <value>The bottom blue jungle raptor bush.</value>
            public static Geometry.Polygon BottomBlueJungle_RaptorBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(6403, 4783));
                    bush.Add(new Vector2(6403, 4433));
                    bush.Add(new Vector2(6458, 4433));
                    bush.Add(new Vector2(6458, 4483));
                    bush.Add(new Vector2(6658, 4483));
                    bush.Add(new Vector2(6658, 4633));
                    bush.Add(new Vector2(6708, 4633));
                    bush.Add(new Vector2(6708, 4833));
                    bush.Add(new Vector2(6558, 4833));
                    bush.Add(new Vector2(6558, 4883));
                    bush.Add(new Vector2(6453, 4883));
                    bush.Add(new Vector2(6453, 4783));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle near base bush.
            /// </summary>
            /// <value>The bottom blue jungle near base bush.</value>
            public static Geometry.Polygon BottomBlueJungle_NearBaseBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(5557, 3237));
                    bush.Add(new Vector2(5557, 3487));
                    bush.Add(new Vector2(5857, 3487));
                    bush.Add(new Vector2(5857, 3587));
                    bush.Add(new Vector2(5402, 3587));
                    bush.Add(new Vector2(5402, 3237));

                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle red bush1.
            /// </summary>
            /// <value>The bottom blue jungle red bush1.</value>
            public static Geometry.Polygon BottomBlueJungle_RedBush1
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(6799, 3034));
                    bush.Add(new Vector2(6799, 2984));
                    bush.Add(new Vector2(7104, 2984));
                    bush.Add(new Vector2(7104, 3034));
                    bush.Add(new Vector2(7154, 3034));
                    bush.Add(new Vector2(7154, 3184));
                    bush.Add(new Vector2(7049, 3184));
                    bush.Add(new Vector2(7049, 3134));
                    bush.Add(new Vector2(6904, 3134));
                    bush.Add(new Vector2(6904, 3184));
                    bush.Add(new Vector2(6549, 3184));
                    bush.Add(new Vector2(6549, 3034));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle red bush.
            /// </summary>
            /// <value>The bottom blue jungle red bush.</value>
            public static Geometry.Polygon BottomBlueJungle_RedBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(7902, 3436));
                    bush.Add(new Vector2(7902, 3386));
                    bush.Add(new Vector2(8207, 3386));
                    bush.Add(new Vector2(8207, 3436));
                    bush.Add(new Vector2(8307, 3436));
                    bush.Add(new Vector2(8307, 3486));
                    bush.Add(new Vector2(8357, 3486));
                    bush.Add(new Vector2(8357, 3636));
                    bush.Add(new Vector2(8252, 3636));
                    bush.Add(new Vector2(8252, 3586));
                    bush.Add(new Vector2(8202, 3586));
                    bush.Add(new Vector2(8202, 3536));
                    bush.Add(new Vector2(7957, 3536));
                    bush.Add(new Vector2(7957, 3586));
                    bush.Add(new Vector2(7907, 3586));
                    bush.Add(new Vector2(7907, 3636));
                    bush.Add(new Vector2(7802, 3636));
                    bush.Add(new Vector2(7802, 3436));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle bihinde dragon.
            /// </summary>
            /// <value>The bottom blue jungle bihinde dragon.</value>
            public static Geometry.Polygon BottomBlueJungle_BihindeDragon
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(8500, 4734));
                    bush.Add(new Vector2(8500, 4684));
                    bush.Add(new Vector2(8550, 4684));
                    bush.Add(new Vector2(8550, 4534));
                    bush.Add(new Vector2(8600, 4534));
                    bush.Add(new Vector2(8600, 4484));
                    bush.Add(new Vector2(8705, 4484));
                    bush.Add(new Vector2(8705, 4734));
                    bush.Add(new Vector2(8655, 4734));
                    bush.Add(new Vector2(8655, 4884));
                    bush.Add(new Vector2(8605, 4884));
                    bush.Add(new Vector2(8605, 4934));
                    bush.Add(new Vector2(8450, 4934));
                    bush.Add(new Vector2(8450, 4734));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle tri bush.
            /// </summary>
            /// <value>The bottom blue jungle tri bush.</value>
            public static Geometry.Polygon BottomBlueJungle_TriBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(10150, 3083));
                    bush.Add(new Vector2(10150, 2883));
                    bush.Add(new Vector2(10200, 2883));
                    bush.Add(new Vector2(10200, 2833));
                    bush.Add(new Vector2(10355, 2833));
                    bush.Add(new Vector2(10355, 2883));
                    bush.Add(new Vector2(10405, 2883));
                    bush.Add(new Vector2(10405, 2933));
                    bush.Add(new Vector2(10655, 2933));
                    bush.Add(new Vector2(10655, 3133));
                    bush.Add(new Vector2(10455, 3133));
                    bush.Add(new Vector2(10455, 3233));
                    bush.Add(new Vector2(10405, 3233));
                    bush.Add(new Vector2(10405, 3283));
                    bush.Add(new Vector2(10250, 3283));
                    bush.Add(new Vector2(10250, 3083));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle krug bush.
            /// </summary>
            /// <value>The bottom blue jungle krug bush.</value>
            public static Geometry.Polygon BottomBlueJungle_KrugBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(9151, 2284));
                    bush.Add(new Vector2(9151, 2234));
                    bush.Add(new Vector2(8951, 2234));
                    bush.Add(new Vector2(8951, 2084));
                    bush.Add(new Vector2(9001, 2084));
                    bush.Add(new Vector2(9001, 1984));
                    bush.Add(new Vector2(9206, 1984));
                    bush.Add(new Vector2(9206, 2034));
                    bush.Add(new Vector2(9356, 2034));
                    bush.Add(new Vector2(9356, 2084));
                    bush.Add(new Vector2(9506, 2084));
                    bush.Add(new Vector2(9506, 2184));
                    bush.Add(new Vector2(9456, 2184));
                    bush.Add(new Vector2(9456, 2284));
                    bush.Add(new Vector2(9406, 2284));
                    bush.Add(new Vector2(9406, 2334));
                    bush.Add(new Vector2(9301, 2334));
                    bush.Add(new Vector2(9301, 2284));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top red jungle tri bush.
            /// </summary>
            /// <value>The top red jungle tri bush.</value>
            public static Geometry.Polygon TopRedJungle_TriBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(4449, 12034));
                    bush.Add(new Vector2(4449, 11984));
                    bush.Add(new Vector2(4149, 11984));
                    bush.Add(new Vector2(4149, 11734));
                    bush.Add(new Vector2(4349, 11734));
                    bush.Add(new Vector2(4349, 11684));
                    bush.Add(new Vector2(4399, 11684));
                    bush.Add(new Vector2(4399, 11484));
                    bush.Add(new Vector2(4449, 11484));
                    bush.Add(new Vector2(4449, 11434));
                    bush.Add(new Vector2(4504, 11434));
                    bush.Add(new Vector2(4504, 11484));
                    bush.Add(new Vector2(4554, 11484));
                    bush.Add(new Vector2(4554, 11534));
                    bush.Add(new Vector2(4604, 11534));
                    bush.Add(new Vector2(4604, 11834));
                    bush.Add(new Vector2(4654, 11834));
                    bush.Add(new Vector2(4654, 11884));
                    bush.Add(new Vector2(4704, 11884));
                    bush.Add(new Vector2(4704, 12034));
                    bush.Add(new Vector2(4604, 12034));
                    bush.Add(new Vector2(4604, 12084));
                    bush.Add(new Vector2(4499, 12084));
                    bush.Add(new Vector2(4499, 12034));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top red jungle krug bush.
            /// </summary>
            /// <value>The top red jungle krug bush.</value>
            public static Geometry.Polygon TopRedJungle_KrugBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(5701, 12884));
                    bush.Add(new Vector2(5701, 12834));
                    bush.Add(new Vector2(5601, 12834));
                    bush.Add(new Vector2(5601, 12784));
                    bush.Add(new Vector2(5401, 12784));
                    bush.Add(new Vector2(5401, 12584));
                    bush.Add(new Vector2(5606, 12584));
                    bush.Add(new Vector2(5606, 12634));
                    bush.Add(new Vector2(5856, 12634));
                    bush.Add(new Vector2(5856, 12684));
                    bush.Add(new Vector2(6006, 12684));
                    bush.Add(new Vector2(6006, 12784));
                    bush.Add(new Vector2(5956, 12784));
                    bush.Add(new Vector2(5956, 12834));
                    bush.Add(new Vector2(5906, 12834));
                    bush.Add(new Vector2(5906, 12884));
                    bush.Add(new Vector2(5856, 12884));
                    bush.Add(new Vector2(5856, 12934));
                    bush.Add(new Vector2(5751, 12934));
                    bush.Add(new Vector2(5751, 12884));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top red jungle behind baron bush.
            /// </summary>
            /// <value>The top red jungle behind baron bush.</value>
            public static Geometry.Polygon TopRedJungle_BehindBaronBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(6149, 10383));
                    bush.Add(new Vector2(6149, 10133));
                    bush.Add(new Vector2(6199, 10133));
                    bush.Add(new Vector2(6199, 10083));
                    bush.Add(new Vector2(6249, 10083));
                    bush.Add(new Vector2(6249, 10033));
                    bush.Add(new Vector2(6354, 10033));
                    bush.Add(new Vector2(6354, 10183));
                    bush.Add(new Vector2(6304, 10183));
                    bush.Add(new Vector2(6304, 10333));
                    bush.Add(new Vector2(6254, 10333));
                    bush.Add(new Vector2(6254, 10533));
                    bush.Add(new Vector2(6099, 10533));
                    bush.Add(new Vector2(6099, 10383));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top red jungle red bush.
            /// </summary>
            /// <value>The top red jungle red bush.</value>
            public static Geometry.Polygon TopRedJungle_RedBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(6499, 11483));
                    bush.Add(new Vector2(6499, 11433));
                    bush.Add(new Vector2(6449, 11433));
                    bush.Add(new Vector2(6449, 11383));
                    bush.Add(new Vector2(6399, 11383));
                    bush.Add(new Vector2(6399, 11333));
                    bush.Add(new Vector2(6449, 11333));
                    bush.Add(new Vector2(6449, 11283));
                    bush.Add(new Vector2(6499, 11283));
                    bush.Add(new Vector2(6499, 11233));
                    bush.Add(new Vector2(6554, 11233));
                    bush.Add(new Vector2(6554, 11283));
                    bush.Add(new Vector2(6604, 11283));
                    bush.Add(new Vector2(6604, 11333));
                    bush.Add(new Vector2(6704, 11333));
                    bush.Add(new Vector2(6704, 11383));
                    bush.Add(new Vector2(6899, 11383));
                    bush.Add(new Vector2(6899, 11333));
                    bush.Add(new Vector2(6949, 11333));
                    bush.Add(new Vector2(6949, 11283));
                    bush.Add(new Vector2(7054, 11283));
                    bush.Add(new Vector2(7054, 11333));
                    bush.Add(new Vector2(7104, 11333));
                    bush.Add(new Vector2(7104, 11433));
                    bush.Add(new Vector2(7054, 11433));
                    bush.Add(new Vector2(7054, 11483));
                    bush.Add(new Vector2(7004, 11483));
                    bush.Add(new Vector2(7004, 11533));
                    bush.Add(new Vector2(6549, 11533));
                    bush.Add(new Vector2(6549, 11483));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top red jungle red bush1.
            /// </summary>
            /// <value>The top red jungle red bush1.</value>
            public static Geometry.Polygon TopRedJungle_RedBush1
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(7751, 11886));
                    bush.Add(new Vector2(7751, 11836));
                    bush.Add(new Vector2(7701, 11836));
                    bush.Add(new Vector2(7701, 11686));
                    bush.Add(new Vector2(7756, 11686));
                    bush.Add(new Vector2(7756, 11736));
                    bush.Add(new Vector2(8151, 11736));
                    bush.Add(new Vector2(8151, 11686));
                    bush.Add(new Vector2(8306, 11686));
                    bush.Add(new Vector2(8306, 11886));
                    bush.Add(new Vector2(8156, 11886));
                    bush.Add(new Vector2(8156, 11936));
                    bush.Add(new Vector2(7851, 11936));
                    bush.Add(new Vector2(7851, 11886));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top red jungle near base bush.
            /// </summary>
            /// <value>The top red jungle near base bush.</value>
            public static Geometry.Polygon TopRedJungle_NearBaseBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(8949, 11433));
                    bush.Add(new Vector2(8949, 11333));
                    bush.Add(new Vector2(9454, 11333));
                    bush.Add(new Vector2(9454, 11633));
                    bush.Add(new Vector2(9299, 11633));
                    bush.Add(new Vector2(9299, 11433));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the top red jungle raptor bush.
            /// </summary>
            /// <value>The top red jungle raptor bush.</value>
            public static Geometry.Polygon TopRedJungle_RaptorBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(8202, 10384));
                    bush.Add(new Vector2(8202, 10284));
                    bush.Add(new Vector2(8152, 10284));
                    bush.Add(new Vector2(8152, 10084));
                    bush.Add(new Vector2(8407, 10084));
                    bush.Add(new Vector2(8407, 10434));
                    bush.Add(new Vector2(8302, 10434));
                    bush.Add(new Vector2(8302, 10384));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom red jungle wolf bush.
            /// </summary>
            /// <value>The bottom red jungle wolf bush.</value>
            public static Geometry.Polygon BottomRedJungle_WolfBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(9853, 8035));
                    bush.Add(new Vector2(9853, 7935));
                    bush.Add(new Vector2(9903, 7935));
                    bush.Add(new Vector2(9903, 7785));
                    bush.Add(new Vector2(9953, 7785));
                    bush.Add(new Vector2(9953, 7685));
                    bush.Add(new Vector2(10158, 7685));
                    bush.Add(new Vector2(10158, 7785));
                    bush.Add(new Vector2(10108, 7785));
                    bush.Add(new Vector2(10108, 7935));
                    bush.Add(new Vector2(10058, 7935));
                    bush.Add(new Vector2(10058, 8085));
                    bush.Add(new Vector2(9903, 8085));
                    bush.Add(new Vector2(9903, 8035));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom red jungle blue buff bush.
            /// </summary>
            /// <value>The bottom red jungle blue buff bush.</value>
            public static Geometry.Polygon BottomRedJungle_BlueBuffBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(11350, 7135));
                    bush.Add(new Vector2(11350, 7035));
                    bush.Add(new Vector2(11655, 7035));
                    bush.Add(new Vector2(11655, 7235));
                    bush.Add(new Vector2(11300, 7235));
                    bush.Add(new Vector2(11300, 7135));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom red jungle tri bush.
            /// </summary>
            /// <value>The bottom red jungle tri bush.</value>
            public static Geometry.Polygon BottomRedJungle_TriBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(12498, 5383));
                    bush.Add(new Vector2(12498, 5283));
                    bush.Add(new Vector2(12248, 5283));
                    bush.Add(new Vector2(12248, 5033));
                    bush.Add(new Vector2(12498, 5033));
                    bush.Add(new Vector2(12498, 4883));
                    bush.Add(new Vector2(12548, 4883));
                    bush.Add(new Vector2(12548, 4833));
                    bush.Add(new Vector2(12653, 4833));
                    bush.Add(new Vector2(12653, 4883));
                    bush.Add(new Vector2(12703, 4883));
                    bush.Add(new Vector2(12703, 5333));
                    bush.Add(new Vector2(12753, 5333));
                    bush.Add(new Vector2(12753, 5433));
                    bush.Add(new Vector2(12703, 5433));
                    bush.Add(new Vector2(12703, 5483));
                    bush.Add(new Vector2(12653, 5483));
                    bush.Add(new Vector2(12653, 5533));
                    bush.Add(new Vector2(12548, 5533));
                    bush.Add(new Vector2(12548, 5383));
                    return bush;
                }
            }

            /// <summary>
            /// Gets the bottom red jungle river entrance bush.
            /// </summary>
            /// <value>The bottom red jungle river entrance bush.</value>
            public static Geometry.Polygon BottomRedJungle_RiverEntranceBush
            {
                get
                {
                    var bush = new Geometry.Polygon();
                    bush.Add(new Vector2(9853, 6584));
                    bush.Add(new Vector2(9853, 6534));
                    bush.Add(new Vector2(9803, 6534));
                    bush.Add(new Vector2(9803, 6484));
                    bush.Add(new Vector2(9753, 6484));
                    bush.Add(new Vector2(9753, 6434));
                    bush.Add(new Vector2(9653, 6434));
                    bush.Add(new Vector2(9653, 6384));
                    bush.Add(new Vector2(9603, 6384));
                    bush.Add(new Vector2(9603, 6334));
                    bush.Add(new Vector2(9653, 6334));
                    bush.Add(new Vector2(9653, 6284));
                    bush.Add(new Vector2(9808, 6284));
                    bush.Add(new Vector2(9808, 6334));
                    bush.Add(new Vector2(9858, 6334));
                    bush.Add(new Vector2(9858, 6384));
                    bush.Add(new Vector2(9908, 6384));
                    bush.Add(new Vector2(9908, 6434));
                    bush.Add(new Vector2(10008, 6434));
                    bush.Add(new Vector2(10008, 6534));
                    bush.Add(new Vector2(10058, 6534));
                    bush.Add(new Vector2(10058, 6634));
                    bush.Add(new Vector2(9903, 6634));
                    bush.Add(new Vector2(9903, 6584));
                    return bush;
                }
            }
        }

        /// <summary>
        /// Class Walls.
        /// </summary>
        public static class Walls
        {
            /// <summary>
            /// Gets the dragon wall.
            /// </summary>
            /// <value>The dragon wall.</value>
            public static Geometry.Polygon DragonWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(10405, 3284));
                    wall.Add(new Vector2(10408, 3284));
                    wall.Add(new Vector2(10408, 3532));
                    wall.Add(new Vector2(10457, 3532));
                    wall.Add(new Vector2(10457, 3534));
                    wall.Add(new Vector2(10458, 3534));
                    wall.Add(new Vector2(10458, 3632));
                    wall.Add(new Vector2(10507, 3632));
                    wall.Add(new Vector2(10507, 3634));
                    wall.Add(new Vector2(10508, 3634));
                    wall.Add(new Vector2(10508, 3682));
                    wall.Add(new Vector2(10702, 3682));
                    wall.Add(new Vector2(10702, 3632));
                    wall.Add(new Vector2(10808, 3632));
                    wall.Add(new Vector2(10808, 3682));
                    wall.Add(new Vector2(10908, 3682));
                    wall.Add(new Vector2(10908, 3732));
                    wall.Add(new Vector2(10958, 3732));
                    wall.Add(new Vector2(10958, 3782));
                    wall.Add(new Vector2(11008, 3782));
                    wall.Add(new Vector2(11008, 4035));
                    wall.Add(new Vector2(10958, 4035));
                    wall.Add(new Vector2(10958, 4185));
                    wall.Add(new Vector2(10908, 4185));
                    wall.Add(new Vector2(10908, 4335));
                    wall.Add(new Vector2(10858, 4335));
                    wall.Add(new Vector2(10858, 4385));
                    wall.Add(new Vector2(10808, 4385));
                    wall.Add(new Vector2(10808, 4435));
                    wall.Add(new Vector2(10784, 4435));
                    wall.Add(new Vector2(10784, 4437));
                    wall.Add(new Vector2(10708, 4437));
                    wall.Add(new Vector2(10708, 4485));
                    wall.Add(new Vector2(10704, 4485));
                    wall.Add(new Vector2(10704, 4487));
                    wall.Add(new Vector2(10658, 4487));
                    wall.Add(new Vector2(10658, 4535));
                    wall.Add(new Vector2(10608, 4535));
                    wall.Add(new Vector2(10608, 4545));
                    wall.Add(new Vector2(10605, 4545));
                    wall.Add(new Vector2(10605, 4583));
                    wall.Add(new Vector2(10555, 4583));
                    wall.Add(new Vector2(10555, 4633));
                    wall.Add(new Vector2(10505, 4633));
                    wall.Add(new Vector2(10505, 4683));
                    wall.Add(new Vector2(10455, 4683));
                    wall.Add(new Vector2(10455, 4733));
                    wall.Add(new Vector2(10350, 4733));
                    wall.Add(new Vector2(10350, 4683));
                    wall.Add(new Vector2(10300, 4683));
                    wall.Add(new Vector2(10300, 4583));
                    wall.Add(new Vector2(10350, 4583));
                    wall.Add(new Vector2(10350, 4533));
                    wall.Add(new Vector2(10400, 4533));
                    wall.Add(new Vector2(10400, 4502));
                    wall.Add(new Vector2(10399, 4502));
                    wall.Add(new Vector2(10399, 4487));
                    wall.Add(new Vector2(10400, 4487));
                    wall.Add(new Vector2(10400, 4483));
                    wall.Add(new Vector2(10449, 4483));
                    wall.Add(new Vector2(10449, 4437));
                    wall.Add(new Vector2(10450, 4437));
                    wall.Add(new Vector2(10450, 4433));
                    wall.Add(new Vector2(10499, 4433));
                    wall.Add(new Vector2(10499, 4287));
                    wall.Add(new Vector2(10449, 4287));
                    wall.Add(new Vector2(10449, 4237));
                    wall.Add(new Vector2(10399, 4237));
                    wall.Add(new Vector2(10399, 4187));
                    wall.Add(new Vector2(10349, 4187));
                    wall.Add(new Vector2(10349, 4137));
                    wall.Add(new Vector2(10299, 4137));
                    wall.Add(new Vector2(10299, 4087));
                    wall.Add(new Vector2(10249, 4087));
                    wall.Add(new Vector2(10249, 4037));
                    wall.Add(new Vector2(10199, 4037));
                    wall.Add(new Vector2(10199, 3987));
                    wall.Add(new Vector2(10149, 3987));
                    wall.Add(new Vector2(10149, 3936));
                    wall.Add(new Vector2(10100, 3936));
                    wall.Add(new Vector2(10100, 3886));
                    wall.Add(new Vector2(9950, 3886));
                    wall.Add(new Vector2(9950, 3836));
                    wall.Add(new Vector2(9948, 3836));
                    wall.Add(new Vector2(9948, 3837));
                    wall.Add(new Vector2(9758, 3837));
                    wall.Add(new Vector2(9758, 3887));
                    wall.Add(new Vector2(9708, 3887));
                    wall.Add(new Vector2(9708, 3937));
                    wall.Add(new Vector2(9658, 3937));
                    wall.Add(new Vector2(9658, 3987));
                    wall.Add(new Vector2(9608, 3987));
                    wall.Add(new Vector2(9608, 4034));
                    wall.Add(new Vector2(9605, 4034));
                    wall.Add(new Vector2(9605, 4036));
                    wall.Add(new Vector2(9558, 4036));
                    wall.Add(new Vector2(9558, 4084));
                    wall.Add(new Vector2(9555, 4084));
                    wall.Add(new Vector2(9555, 4086));
                    wall.Add(new Vector2(9508, 4086));
                    wall.Add(new Vector2(9508, 4134));
                    wall.Add(new Vector2(9505, 4134));
                    wall.Add(new Vector2(9505, 4136));
                    wall.Add(new Vector2(9408, 4136));
                    wall.Add(new Vector2(9408, 4159));
                    wall.Add(new Vector2(9405, 4159));
                    wall.Add(new Vector2(9405, 4186));
                    wall.Add(new Vector2(9355, 4186));
                    wall.Add(new Vector2(9355, 4286));
                    wall.Add(new Vector2(9305, 4286));
                    wall.Add(new Vector2(9305, 4371));
                    wall.Add(new Vector2(9304, 4371));
                    wall.Add(new Vector2(9304, 4511));
                    wall.Add(new Vector2(9307, 4511));
                    wall.Add(new Vector2(9307, 4533));
                    wall.Add(new Vector2(9354, 4533));
                    wall.Add(new Vector2(9354, 4536));
                    wall.Add(new Vector2(9357, 4536));
                    wall.Add(new Vector2(9357, 4683));
                    wall.Add(new Vector2(9404, 4683));
                    wall.Add(new Vector2(9404, 4686));
                    wall.Add(new Vector2(9407, 4686));
                    wall.Add(new Vector2(9407, 4783));
                    wall.Add(new Vector2(9454, 4783));
                    wall.Add(new Vector2(9454, 4786));
                    wall.Add(new Vector2(9457, 4786));
                    wall.Add(new Vector2(9457, 4833));
                    wall.Add(new Vector2(9504, 4833));
                    wall.Add(new Vector2(9504, 4836));
                    wall.Add(new Vector2(9507, 4836));
                    wall.Add(new Vector2(9507, 4883));
                    wall.Add(new Vector2(9554, 4883));
                    wall.Add(new Vector2(9554, 4886));
                    wall.Add(new Vector2(9557, 4886));
                    wall.Add(new Vector2(9557, 4933));
                    wall.Add(new Vector2(9589, 4933));
                    wall.Add(new Vector2(9589, 4935));
                    wall.Add(new Vector2(10004, 4935));
                    wall.Add(new Vector2(10004, 5085));
                    wall.Add(new Vector2(9904, 5085));
                    wall.Add(new Vector2(9904, 5135));
                    wall.Add(new Vector2(9872, 5135));
                    wall.Add(new Vector2(9872, 5136));
                    wall.Add(new Vector2(9807, 5136));
                    wall.Add(new Vector2(9807, 5141));
                    wall.Add(new Vector2(9804, 5141));
                    wall.Add(new Vector2(9804, 5185));
                    wall.Add(new Vector2(9704, 5185));
                    wall.Add(new Vector2(9704, 5235));
                    wall.Add(new Vector2(9625, 5235));
                    wall.Add(new Vector2(9625, 5237));
                    wall.Add(new Vector2(9605, 5237));
                    wall.Add(new Vector2(9605, 5287));
                    wall.Add(new Vector2(9455, 5287));
                    wall.Add(new Vector2(9455, 5337));
                    wall.Add(new Vector2(9255, 5337));
                    wall.Add(new Vector2(9255, 5387));
                    wall.Add(new Vector2(9058, 5387));
                    wall.Add(new Vector2(9058, 5436));
                    wall.Add(new Vector2(9055, 5436));
                    wall.Add(new Vector2(9055, 5437));
                    wall.Add(new Vector2(8990, 5437));
                    wall.Add(new Vector2(8990, 5436));
                    wall.Add(new Vector2(8753, 5436));
                    wall.Add(new Vector2(8753, 5386));
                    wall.Add(new Vector2(8703, 5386));
                    wall.Add(new Vector2(8703, 5136));
                    wall.Add(new Vector2(8753, 5136));
                    wall.Add(new Vector2(8753, 5086));
                    wall.Add(new Vector2(8853, 5086));
                    wall.Add(new Vector2(8853, 5036));
                    wall.Add(new Vector2(8903, 5036));
                    wall.Add(new Vector2(8903, 4986));
                    wall.Add(new Vector2(8953, 4986));
                    wall.Add(new Vector2(8953, 4936));
                    wall.Add(new Vector2(8954, 4936));
                    wall.Add(new Vector2(8954, 4933));
                    wall.Add(new Vector2(8999, 4933));
                    wall.Add(new Vector2(8999, 4883));
                    wall.Add(new Vector2(9049, 4883));
                    wall.Add(new Vector2(9049, 4783));
                    wall.Add(new Vector2(9099, 4783));
                    wall.Add(new Vector2(9099, 4535));
                    wall.Add(new Vector2(9049, 4535));
                    wall.Add(new Vector2(9049, 4285));
                    wall.Add(new Vector2(9099, 4285));
                    wall.Add(new Vector2(9099, 4085));
                    wall.Add(new Vector2(9149, 4085));
                    wall.Add(new Vector2(9149, 4015));
                    wall.Add(new Vector2(9150, 4015));
                    wall.Add(new Vector2(9150, 3936));
                    wall.Add(new Vector2(9200, 3936));
                    wall.Add(new Vector2(9200, 3886));
                    wall.Add(new Vector2(9250, 3886));
                    wall.Add(new Vector2(9250, 3836));
                    wall.Add(new Vector2(9278, 3836));
                    wall.Add(new Vector2(9278, 3834));
                    wall.Add(new Vector2(9300, 3834));
                    wall.Add(new Vector2(9300, 3786));
                    wall.Add(new Vector2(9303, 3786));
                    wall.Add(new Vector2(9303, 3784));
                    wall.Add(new Vector2(9350, 3784));
                    wall.Add(new Vector2(9350, 3741));
                    wall.Add(new Vector2(9353, 3741));
                    wall.Add(new Vector2(9353, 3734));
                    wall.Add(new Vector2(9403, 3734));
                    wall.Add(new Vector2(9403, 3684));
                    wall.Add(new Vector2(9453, 3684));
                    wall.Add(new Vector2(9453, 3634));
                    wall.Add(new Vector2(9503, 3634));
                    wall.Add(new Vector2(9503, 3584));
                    wall.Add(new Vector2(9603, 3584));
                    wall.Add(new Vector2(9603, 3534));
                    wall.Add(new Vector2(9703, 3534));
                    wall.Add(new Vector2(9703, 3487));
                    wall.Add(new Vector2(9800, 3487));
                    wall.Add(new Vector2(9800, 3432));
                    wall.Add(new Vector2(9900, 3432));
                    wall.Add(new Vector2(9900, 3382));
                    wall.Add(new Vector2(10000, 3382));
                    wall.Add(new Vector2(10000, 3332));
                    wall.Add(new Vector2(10100, 3332));
                    wall.Add(new Vector2(10100, 3282));
                    wall.Add(new Vector2(10405, 3282));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the baron wall.
            /// </summary>
            /// <value>The baron wall.</value>
            public static Geometry.Polygon BaronWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(5775, 9486));
                    wall.Add(new Vector2(5804, 9486));
                    wall.Add(new Vector2(5804, 9484));
                    wall.Add(new Vector2(5831, 9484));
                    wall.Add(new Vector2(5831, 9483));
                    wall.Add(new Vector2(6056, 9483));
                    wall.Add(new Vector2(6056, 9499));
                    wall.Add(new Vector2(6058, 9499));
                    wall.Add(new Vector2(6058, 9533));
                    wall.Add(new Vector2(6156, 9533));
                    wall.Add(new Vector2(6156, 9534));
                    wall.Add(new Vector2(6158, 9534));
                    wall.Add(new Vector2(6158, 9684));
                    wall.Add(new Vector2(6108, 9684));
                    wall.Add(new Vector2(6108, 9734));
                    wall.Add(new Vector2(6058, 9734));
                    wall.Add(new Vector2(6058, 9784));
                    wall.Add(new Vector2(6008, 9784));
                    wall.Add(new Vector2(6008, 9834));
                    wall.Add(new Vector2(5958, 9834));
                    wall.Add(new Vector2(5958, 9859));
                    wall.Add(new Vector2(5956, 9859));
                    wall.Add(new Vector2(5956, 9882));
                    wall.Add(new Vector2(5954, 9882));
                    wall.Add(new Vector2(5954, 9884));
                    wall.Add(new Vector2(5906, 9884));
                    wall.Add(new Vector2(5906, 9932));
                    wall.Add(new Vector2(5908, 9932));
                    wall.Add(new Vector2(5908, 9937));
                    wall.Add(new Vector2(5858, 9937));
                    wall.Add(new Vector2(5858, 9987));
                    wall.Add(new Vector2(5808, 9987));
                    wall.Add(new Vector2(5808, 10452));
                    wall.Add(new Vector2(5806, 10452));
                    wall.Add(new Vector2(5806, 10637));
                    wall.Add(new Vector2(5756, 10637));
                    wall.Add(new Vector2(5756, 10787));
                    wall.Add(new Vector2(5706, 10787));
                    wall.Add(new Vector2(5706, 10936));
                    wall.Add(new Vector2(5656, 10936));
                    wall.Add(new Vector2(5656, 10976));
                    wall.Add(new Vector2(5655, 10976));
                    wall.Add(new Vector2(5655, 10983));
                    wall.Add(new Vector2(5605, 10983));
                    wall.Add(new Vector2(5605, 11033));
                    wall.Add(new Vector2(5555, 11033));
                    wall.Add(new Vector2(5555, 11083));
                    wall.Add(new Vector2(5533, 11083));
                    wall.Add(new Vector2(5533, 11084));
                    wall.Add(new Vector2(5508, 11084));
                    wall.Add(new Vector2(5508, 11134));
                    wall.Add(new Vector2(5483, 11134));
                    wall.Add(new Vector2(5483, 11135));
                    wall.Add(new Vector2(5458, 11135));
                    wall.Add(new Vector2(5458, 11185));
                    wall.Add(new Vector2(5408, 11185));
                    wall.Add(new Vector2(5408, 11285));
                    wall.Add(new Vector2(5308, 11285));
                    wall.Add(new Vector2(5308, 11335));
                    wall.Add(new Vector2(5208, 11335));
                    wall.Add(new Vector2(5208, 11385));
                    wall.Add(new Vector2(5191, 11385));
                    wall.Add(new Vector2(5191, 11386));
                    wall.Add(new Vector2(5058, 11386));
                    wall.Add(new Vector2(5058, 11435));
                    wall.Add(new Vector2(5056, 11435));
                    wall.Add(new Vector2(5056, 11436));
                    wall.Add(new Vector2(4957, 11436));
                    wall.Add(new Vector2(4957, 11486));
                    wall.Add(new Vector2(4807, 11486));
                    wall.Add(new Vector2(4807, 11536));
                    wall.Add(new Vector2(4708, 11536));
                    wall.Add(new Vector2(4708, 11583));
                    wall.Add(new Vector2(4707, 11583));
                    wall.Add(new Vector2(4707, 11586));
                    wall.Add(new Vector2(4602, 11586));
                    wall.Add(new Vector2(4602, 11536));
                    wall.Add(new Vector2(4552, 11536));
                    wall.Add(new Vector2(4552, 11487));
                    wall.Add(new Vector2(4503, 11487));
                    wall.Add(new Vector2(4503, 11486));
                    wall.Add(new Vector2(4502, 11486));
                    wall.Add(new Vector2(4502, 11437));
                    wall.Add(new Vector2(4453, 11437));
                    wall.Add(new Vector2(4453, 11340));
                    wall.Add(new Vector2(4449, 11340));
                    wall.Add(new Vector2(4449, 11287));
                    wall.Add(new Vector2(4403, 11287));
                    wall.Add(new Vector2(4403, 11285));
                    wall.Add(new Vector2(4399, 11285));
                    wall.Add(new Vector2(4399, 11237));
                    wall.Add(new Vector2(4188, 11237));
                    wall.Add(new Vector2(4188, 11236));
                    wall.Add(new Vector2(4001, 11236));
                    wall.Add(new Vector2(4001, 11235));
                    wall.Add(new Vector2(4000, 11235));
                    wall.Add(new Vector2(4000, 11186));
                    wall.Add(new Vector2(3901, 11186));
                    wall.Add(new Vector2(3901, 11185));
                    wall.Add(new Vector2(3900, 11185));
                    wall.Add(new Vector2(3900, 11136));
                    wall.Add(new Vector2(3851, 11136));
                    wall.Add(new Vector2(3851, 11135));
                    wall.Add(new Vector2(3850, 11135));
                    wall.Add(new Vector2(3850, 11036));
                    wall.Add(new Vector2(3801, 11036));
                    wall.Add(new Vector2(3801, 11035));
                    wall.Add(new Vector2(3800, 11035));
                    wall.Add(new Vector2(3800, 10885));
                    wall.Add(new Vector2(3803, 10885));
                    wall.Add(new Vector2(3803, 10882));
                    wall.Add(new Vector2(3850, 10882));
                    wall.Add(new Vector2(3850, 10875));
                    wall.Add(new Vector2(3851, 10875));
                    wall.Add(new Vector2(3851, 10826));
                    wall.Add(new Vector2(3852, 10826));
                    wall.Add(new Vector2(3852, 10686));
                    wall.Add(new Vector2(3853, 10686));
                    wall.Add(new Vector2(3853, 10682));
                    wall.Add(new Vector2(3902, 10682));
                    wall.Add(new Vector2(3902, 10662));
                    wall.Add(new Vector2(3900, 10662));
                    wall.Add(new Vector2(3900, 10587));
                    wall.Add(new Vector2(3902, 10587));
                    wall.Add(new Vector2(3902, 10586));
                    wall.Add(new Vector2(3903, 10586));
                    wall.Add(new Vector2(3903, 10582));
                    wall.Add(new Vector2(3950, 10582));
                    wall.Add(new Vector2(3950, 10537));
                    wall.Add(new Vector2(3953, 10537));
                    wall.Add(new Vector2(3953, 10532));
                    wall.Add(new Vector2(4000, 10532));
                    wall.Add(new Vector2(4000, 10487));
                    wall.Add(new Vector2(4003, 10487));
                    wall.Add(new Vector2(4003, 10482));
                    wall.Add(new Vector2(4050, 10482));
                    wall.Add(new Vector2(4050, 10437));
                    wall.Add(new Vector2(4100, 10437));
                    wall.Add(new Vector2(4100, 10387));
                    wall.Add(new Vector2(4131, 10387));
                    wall.Add(new Vector2(4131, 10383));
                    wall.Add(new Vector2(4150, 10383));
                    wall.Add(new Vector2(4150, 10337));
                    wall.Add(new Vector2(4151, 10337));
                    wall.Add(new Vector2(4151, 10333));
                    wall.Add(new Vector2(4191, 10333));
                    wall.Add(new Vector2(4191, 10332));
                    wall.Add(new Vector2(4249, 10332));
                    wall.Add(new Vector2(4249, 10286));
                    wall.Add(new Vector2(4251, 10286));
                    wall.Add(new Vector2(4251, 10282));
                    wall.Add(new Vector2(4299, 10282));
                    wall.Add(new Vector2(4299, 10236));
                    wall.Add(new Vector2(4301, 10236));
                    wall.Add(new Vector2(4301, 10232));
                    wall.Add(new Vector2(4349, 10232));
                    wall.Add(new Vector2(4349, 10186));
                    wall.Add(new Vector2(4351, 10186));
                    wall.Add(new Vector2(4351, 10182));
                    wall.Add(new Vector2(4506, 10182));
                    wall.Add(new Vector2(4506, 10185));
                    wall.Add(new Vector2(4507, 10185));
                    wall.Add(new Vector2(4507, 10232));
                    wall.Add(new Vector2(4556, 10232));
                    wall.Add(new Vector2(4556, 10235));
                    wall.Add(new Vector2(4557, 10235));
                    wall.Add(new Vector2(4557, 10335));
                    wall.Add(new Vector2(4554, 10335));
                    wall.Add(new Vector2(4554, 10336));
                    wall.Add(new Vector2(4507, 10336));
                    wall.Add(new Vector2(4507, 10385));
                    wall.Add(new Vector2(4504, 10385));
                    wall.Add(new Vector2(4504, 10386));
                    wall.Add(new Vector2(4457, 10386));
                    wall.Add(new Vector2(4457, 10590));
                    wall.Add(new Vector2(4456, 10590));
                    wall.Add(new Vector2(4456, 10633));
                    wall.Add(new Vector2(4506, 10633));
                    wall.Add(new Vector2(4506, 10653));
                    wall.Add(new Vector2(4505, 10653));
                    wall.Add(new Vector2(4505, 10684));
                    wall.Add(new Vector2(4555, 10684));
                    wall.Add(new Vector2(4555, 10734));
                    wall.Add(new Vector2(4605, 10734));
                    wall.Add(new Vector2(4605, 10735));
                    wall.Add(new Vector2(4606, 10735));
                    wall.Add(new Vector2(4606, 10784));
                    wall.Add(new Vector2(4655, 10784));
                    wall.Add(new Vector2(4655, 10785));
                    wall.Add(new Vector2(4656, 10785));
                    wall.Add(new Vector2(4656, 10834));
                    wall.Add(new Vector2(4705, 10834));
                    wall.Add(new Vector2(4705, 10835));
                    wall.Add(new Vector2(4706, 10835));
                    wall.Add(new Vector2(4706, 10884));
                    wall.Add(new Vector2(4715, 10884));
                    wall.Add(new Vector2(4715, 10885));
                    wall.Add(new Vector2(4806, 10885));
                    wall.Add(new Vector2(4806, 10935));
                    wall.Add(new Vector2(5006, 10935));
                    wall.Add(new Vector2(5006, 10936));
                    wall.Add(new Vector2(5007, 10936));
                    wall.Add(new Vector2(5007, 10955));
                    wall.Add(new Vector2(5008, 10955));
                    wall.Add(new Vector2(5008, 10984));
                    wall.Add(new Vector2(5151, 10984));
                    wall.Add(new Vector2(5151, 10956));
                    wall.Add(new Vector2(5152, 10956));
                    wall.Add(new Vector2(5152, 10936));
                    wall.Add(new Vector2(5153, 10936));
                    wall.Add(new Vector2(5153, 10934));
                    wall.Add(new Vector2(5252, 10934));
                    wall.Add(new Vector2(5252, 10886));
                    wall.Add(new Vector2(5253, 10886));
                    wall.Add(new Vector2(5253, 10884));
                    wall.Add(new Vector2(5295, 10884));
                    wall.Add(new Vector2(5295, 10883));
                    wall.Add(new Vector2(5300, 10883));
                    wall.Add(new Vector2(5300, 10833));
                    wall.Add(new Vector2(5350, 10833));
                    wall.Add(new Vector2(5350, 10783));
                    wall.Add(new Vector2(5450, 10783));
                    wall.Add(new Vector2(5450, 10683));
                    wall.Add(new Vector2(5500, 10683));
                    wall.Add(new Vector2(5500, 10593));
                    wall.Add(new Vector2(5501, 10593));
                    wall.Add(new Vector2(5501, 10337));
                    wall.Add(new Vector2(5451, 10337));
                    wall.Add(new Vector2(5451, 10287));
                    wall.Add(new Vector2(5408, 10287));
                    wall.Add(new Vector2(5408, 10284));
                    wall.Add(new Vector2(5401, 10284));
                    wall.Add(new Vector2(5401, 10184));
                    wall.Add(new Vector2(5351, 10184));
                    wall.Add(new Vector2(5351, 10149));
                    wall.Add(new Vector2(5350, 10149));
                    wall.Add(new Vector2(5350, 10134));
                    wall.Add(new Vector2(5299, 10134));
                    wall.Add(new Vector2(5299, 10084));
                    wall.Add(new Vector2(5249, 10084));
                    wall.Add(new Vector2(5249, 9986));
                    wall.Add(new Vector2(4902, 9986));
                    wall.Add(new Vector2(4902, 9982));
                    wall.Add(new Vector2(4901, 9982));
                    wall.Add(new Vector2(4901, 9936));
                    wall.Add(new Vector2(4892, 9936));
                    wall.Add(new Vector2(4892, 9932));
                    wall.Add(new Vector2(4851, 9932));
                    wall.Add(new Vector2(4851, 9832));
                    wall.Add(new Vector2(4951, 9832));
                    wall.Add(new Vector2(4951, 9782));
                    wall.Add(new Vector2(5051, 9782));
                    wall.Add(new Vector2(5051, 9732));
                    wall.Add(new Vector2(5136, 9732));
                    wall.Add(new Vector2(5136, 9736));
                    wall.Add(new Vector2(5151, 9736));
                    wall.Add(new Vector2(5151, 9685));
                    wall.Add(new Vector2(5251, 9685));
                    wall.Add(new Vector2(5251, 9635));
                    wall.Add(new Vector2(5351, 9635));
                    wall.Add(new Vector2(5351, 9585));
                    wall.Add(new Vector2(5410, 9585));
                    wall.Add(new Vector2(5410, 9582));
                    wall.Add(new Vector2(5500, 9582));
                    wall.Add(new Vector2(5500, 9532));
                    wall.Add(new Vector2(5749, 9532));
                    wall.Add(new Vector2(5749, 9486));
                    wall.Add(new Vector2(5750, 9486));
                    wall.Add(new Vector2(5750, 9482));
                    wall.Add(new Vector2(5775, 9482));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the blue base top wall.
            /// </summary>
            /// <value>The blue base top wall.</value>
            public static Geometry.Polygon BlueBaseTopWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(3705, 4384));
                    wall.Add(new Vector2(3755, 4384));
                    wall.Add(new Vector2(3755, 4585));
                    wall.Add(new Vector2(3705, 4585));
                    wall.Add(new Vector2(3705, 4635));
                    wall.Add(new Vector2(3605, 4635));
                    wall.Add(new Vector2(3605, 4685));
                    wall.Add(new Vector2(3505, 4685));
                    wall.Add(new Vector2(3505, 4735));
                    wall.Add(new Vector2(3408, 4735));
                    wall.Add(new Vector2(3408, 4783));
                    wall.Add(new Vector2(3405, 4783));
                    wall.Add(new Vector2(3405, 4785));
                    wall.Add(new Vector2(3308, 4785));
                    wall.Add(new Vector2(3308, 4833));
                    wall.Add(new Vector2(3305, 4833));
                    wall.Add(new Vector2(3305, 4835));
                    wall.Add(new Vector2(3215, 4835));
                    wall.Add(new Vector2(3215, 4833));
                    wall.Add(new Vector2(3158, 4833));
                    wall.Add(new Vector2(3158, 4883));
                    wall.Add(new Vector2(2958, 4883));
                    wall.Add(new Vector2(2958, 4933));
                    wall.Add(new Vector2(2754, 4933));
                    wall.Add(new Vector2(2754, 4983));
                    wall.Add(new Vector2(2582, 4983));
                    wall.Add(new Vector2(2582, 4984));
                    wall.Add(new Vector2(2457, 4984));
                    wall.Add(new Vector2(2457, 5034));
                    wall.Add(new Vector2(2156, 5034));
                    wall.Add(new Vector2(2156, 5037));
                    wall.Add(new Vector2(1701, 5037));
                    wall.Add(new Vector2(1701, 4937));
                    wall.Add(new Vector2(1651, 4937));
                    wall.Add(new Vector2(1651, 4787));
                    wall.Add(new Vector2(1701, 4787));
                    wall.Add(new Vector2(1701, 4737));
                    wall.Add(new Vector2(1927, 4737));
                    wall.Add(new Vector2(1927, 4734));
                    wall.Add(new Vector2(2394, 4734));
                    wall.Add(new Vector2(2394, 4733));
                    wall.Add(new Vector2(2449, 4733));
                    wall.Add(new Vector2(2449, 4683));
                    wall.Add(new Vector2(2749, 4683));
                    wall.Add(new Vector2(2749, 4633));
                    wall.Add(new Vector2(2949, 4633));
                    wall.Add(new Vector2(2949, 4583));
                    wall.Add(new Vector2(3103, 4583));
                    wall.Add(new Vector2(3103, 4533));
                    wall.Add(new Vector2(3250, 4533));
                    wall.Add(new Vector2(3250, 4485));
                    wall.Add(new Vector2(3253, 4485));
                    wall.Add(new Vector2(3253, 4483));
                    wall.Add(new Vector2(3350, 4483));
                    wall.Add(new Vector2(3350, 4434));
                    wall.Add(new Vector2(3353, 4434));
                    wall.Add(new Vector2(3353, 4433));
                    wall.Add(new Vector2(3400, 4433));
                    wall.Add(new Vector2(3400, 4384));
                    wall.Add(new Vector2(3403, 4384));
                    wall.Add(new Vector2(3403, 4383));
                    wall.Add(new Vector2(3498, 4383));
                    wall.Add(new Vector2(3498, 4384));
                    wall.Add(new Vector2(3500, 4384));
                    wall.Add(new Vector2(3500, 4334));
                    wall.Add(new Vector2(3705, 4334));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the blue base bottom wall.
            /// </summary>
            /// <value>The blue base bottom wall.</value>
            public static Geometry.Polygon BlueBaseBottomWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(5005, 1686));
                    wall.Add(new Vector2(5055, 1686));
                    wall.Add(new Vector2(5055, 2136));
                    wall.Add(new Vector2(5005, 2136));
                    wall.Add(new Vector2(5005, 2321));
                    wall.Add(new Vector2(5004, 2321));
                    wall.Add(new Vector2(5004, 2757));
                    wall.Add(new Vector2(5006, 2757));
                    wall.Add(new Vector2(5006, 2787));
                    wall.Add(new Vector2(4956, 2787));
                    wall.Add(new Vector2(4956, 3037));
                    wall.Add(new Vector2(4906, 3037));
                    wall.Add(new Vector2(4906, 3237));
                    wall.Add(new Vector2(4856, 3237));
                    wall.Add(new Vector2(4856, 3287));
                    wall.Add(new Vector2(4806, 3287));
                    wall.Add(new Vector2(4806, 3387));
                    wall.Add(new Vector2(4756, 3387));
                    wall.Add(new Vector2(4756, 3487));
                    wall.Add(new Vector2(4706, 3487));
                    wall.Add(new Vector2(4706, 3585));
                    wall.Add(new Vector2(4656, 3585));
                    wall.Add(new Vector2(4656, 3685));
                    wall.Add(new Vector2(4606, 3685));
                    wall.Add(new Vector2(4606, 3785));
                    wall.Add(new Vector2(4556, 3785));
                    wall.Add(new Vector2(4556, 3835));
                    wall.Add(new Vector2(4506, 3835));
                    wall.Add(new Vector2(4506, 3885));
                    wall.Add(new Vector2(4401, 3885));
                    wall.Add(new Vector2(4401, 3835));
                    wall.Add(new Vector2(4351, 3835));
                    wall.Add(new Vector2(4351, 3785));
                    wall.Add(new Vector2(4301, 3785));
                    wall.Add(new Vector2(4301, 3735));
                    wall.Add(new Vector2(4251, 3735));
                    wall.Add(new Vector2(4251, 3635));
                    wall.Add(new Vector2(4301, 3635));
                    wall.Add(new Vector2(4301, 3535));
                    wall.Add(new Vector2(4351, 3535));
                    wall.Add(new Vector2(4351, 3435));
                    wall.Add(new Vector2(4401, 3435));
                    wall.Add(new Vector2(4401, 3385));
                    wall.Add(new Vector2(4451, 3385));
                    wall.Add(new Vector2(4451, 3335));
                    wall.Add(new Vector2(4501, 3335));
                    wall.Add(new Vector2(4501, 3287));
                    wall.Add(new Vector2(4551, 3287));
                    wall.Add(new Vector2(4551, 3187));
                    wall.Add(new Vector2(4601, 3187));
                    wall.Add(new Vector2(4601, 3037));
                    wall.Add(new Vector2(4651, 3037));
                    wall.Add(new Vector2(4651, 2828));
                    wall.Add(new Vector2(4649, 2828));
                    wall.Add(new Vector2(4649, 2783));
                    wall.Add(new Vector2(4699, 2783));
                    wall.Add(new Vector2(4699, 2083));
                    wall.Add(new Vector2(4749, 2083));
                    wall.Add(new Vector2(4749, 2068));
                    wall.Add(new Vector2(4750, 2068));
                    wall.Add(new Vector2(4750, 1686));
                    wall.Add(new Vector2(4800, 1686));
                    wall.Add(new Vector2(4800, 1636));
                    wall.Add(new Vector2(5005, 1636));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the red base top wall.
            /// </summary>
            /// <value>The red base top wall.</value>
            public static Geometry.Polygon RedBaseTopWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(10456, 11136));
                    wall.Add(new Vector2(10506, 11136));
                    wall.Add(new Vector2(10506, 11336));
                    wall.Add(new Vector2(10456, 11336));
                    wall.Add(new Vector2(10456, 11436));
                    wall.Add(new Vector2(10406, 11436));
                    wall.Add(new Vector2(10406, 11486));
                    wall.Add(new Vector2(10356, 11486));
                    wall.Add(new Vector2(10356, 11586));
                    wall.Add(new Vector2(10306, 11586));
                    wall.Add(new Vector2(10306, 11784));
                    wall.Add(new Vector2(10256, 11784));
                    wall.Add(new Vector2(10256, 11984));
                    wall.Add(new Vector2(10206, 11984));
                    wall.Add(new Vector2(10206, 12134));
                    wall.Add(new Vector2(10156, 12134));
                    wall.Add(new Vector2(10156, 12274));
                    wall.Add(new Vector2(10155, 12274));
                    wall.Add(new Vector2(10155, 12384));
                    wall.Add(new Vector2(10105, 12384));
                    wall.Add(new Vector2(10105, 12713));
                    wall.Add(new Vector2(10106, 12713));
                    wall.Add(new Vector2(10106, 13183));
                    wall.Add(new Vector2(10056, 13183));
                    wall.Add(new Vector2(10056, 13233));
                    wall.Add(new Vector2(9851, 13233));
                    wall.Add(new Vector2(9851, 13183));
                    wall.Add(new Vector2(9801, 13183));
                    wall.Add(new Vector2(9801, 12899));
                    wall.Add(new Vector2(9800, 12899));
                    wall.Add(new Vector2(9800, 12384));
                    wall.Add(new Vector2(9850, 12384));
                    wall.Add(new Vector2(9850, 12134));
                    wall.Add(new Vector2(9900, 12134));
                    wall.Add(new Vector2(9900, 12119));
                    wall.Add(new Vector2(9901, 12119));
                    wall.Add(new Vector2(9901, 11984));
                    wall.Add(new Vector2(9951, 11984));
                    wall.Add(new Vector2(9951, 11784));
                    wall.Add(new Vector2(10001, 11784));
                    wall.Add(new Vector2(10001, 11584));
                    wall.Add(new Vector2(10051, 11584));
                    wall.Add(new Vector2(10051, 11486));
                    wall.Add(new Vector2(10101, 11486));
                    wall.Add(new Vector2(10101, 11336));
                    wall.Add(new Vector2(10151, 11336));
                    wall.Add(new Vector2(10151, 11236));
                    wall.Add(new Vector2(10201, 11236));
                    wall.Add(new Vector2(10201, 11136));
                    wall.Add(new Vector2(10251, 11136));
                    wall.Add(new Vector2(10251, 11086));
                    wall.Add(new Vector2(10456, 11086));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the red base bottom wall.
            /// </summary>
            /// <value>The red base bottom wall.</value>
            public static Geometry.Polygon RedBaseBottomWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(12998, 9886));
                    wall.Add(new Vector2(13156, 9886));
                    wall.Add(new Vector2(13156, 9936));
                    wall.Add(new Vector2(13206, 9936));
                    wall.Add(new Vector2(13206, 10136));
                    wall.Add(new Vector2(13156, 10136));
                    wall.Add(new Vector2(13156, 10186));
                    wall.Add(new Vector2(12708, 10186));
                    wall.Add(new Vector2(12708, 10233));
                    wall.Add(new Vector2(12706, 10233));
                    wall.Add(new Vector2(12706, 10236));
                    wall.Add(new Vector2(12606, 10236));
                    wall.Add(new Vector2(12606, 10233));
                    wall.Add(new Vector2(12590, 10233));
                    wall.Add(new Vector2(12590, 10234));
                    wall.Add(new Vector2(12105, 10234));
                    wall.Add(new Vector2(12105, 10284));
                    wall.Add(new Vector2(11957, 10284));
                    wall.Add(new Vector2(11957, 10333));
                    wall.Add(new Vector2(11955, 10333));
                    wall.Add(new Vector2(11955, 10334));
                    wall.Add(new Vector2(11835, 10334));
                    wall.Add(new Vector2(11835, 10333));
                    wall.Add(new Vector2(11807, 10333));
                    wall.Add(new Vector2(11807, 10383));
                    wall.Add(new Vector2(11657, 10383));
                    wall.Add(new Vector2(11657, 10433));
                    wall.Add(new Vector2(11557, 10433));
                    wall.Add(new Vector2(11557, 10483));
                    wall.Add(new Vector2(11527, 10483));
                    wall.Add(new Vector2(11527, 10486));
                    wall.Add(new Vector2(11457, 10486));
                    wall.Add(new Vector2(11457, 10536));
                    wall.Add(new Vector2(11357, 10536));
                    wall.Add(new Vector2(11357, 10586));
                    wall.Add(new Vector2(11307, 10586));
                    wall.Add(new Vector2(11307, 10636));
                    wall.Add(new Vector2(11152, 10636));
                    wall.Add(new Vector2(11152, 10586));
                    wall.Add(new Vector2(11102, 10586));
                    wall.Add(new Vector2(11102, 10536));
                    wall.Add(new Vector2(11052, 10536));
                    wall.Add(new Vector2(11052, 10486));
                    wall.Add(new Vector2(11002, 10486));
                    wall.Add(new Vector2(11002, 10336));
                    wall.Add(new Vector2(11052, 10336));
                    wall.Add(new Vector2(11052, 10286));
                    wall.Add(new Vector2(11102, 10286));
                    wall.Add(new Vector2(11102, 10236));
                    wall.Add(new Vector2(11202, 10236));
                    wall.Add(new Vector2(11202, 10186));
                    wall.Add(new Vector2(11252, 10186));
                    wall.Add(new Vector2(11252, 10183));
                    wall.Add(new Vector2(11502, 10183));
                    wall.Add(new Vector2(11502, 10133));
                    wall.Add(new Vector2(11652, 10133));
                    wall.Add(new Vector2(11652, 10083));
                    wall.Add(new Vector2(11802, 10083));
                    wall.Add(new Vector2(11802, 10033));
                    wall.Add(new Vector2(11950, 10033));
                    wall.Add(new Vector2(11950, 9984));
                    wall.Add(new Vector2(11952, 9984));
                    wall.Add(new Vector2(11952, 9983));
                    wall.Add(new Vector2(12007, 9983));
                    wall.Add(new Vector2(12007, 9984));
                    wall.Add(new Vector2(12100, 9984));
                    wall.Add(new Vector2(12100, 9934));
                    wall.Add(new Vector2(12243, 9934));
                    wall.Add(new Vector2(12243, 9933));
                    wall.Add(new Vector2(12701, 9933));
                    wall.Add(new Vector2(12701, 9886));
                    wall.Add(new Vector2(12703, 9886));
                    wall.Add(new Vector2(12703, 9883));
                    wall.Add(new Vector2(12998, 9883));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top blue jungle wall 1.
            /// </summary>
            /// <value>The top blue jungle wall 1.</value>
            public static Geometry.Polygon TopBlueJungle_Wall_1
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(3354, 9334));
                    wall.Add(new Vector2(3404, 9334));
                    wall.Add(new Vector2(3404, 9384));
                    wall.Add(new Vector2(3454, 9384));
                    wall.Add(new Vector2(3454, 9584));
                    wall.Add(new Vector2(3504, 9584));
                    wall.Add(new Vector2(3504, 9595));
                    wall.Add(new Vector2(3505, 9595));
                    wall.Add(new Vector2(3505, 9785));
                    wall.Add(new Vector2(3455, 9785));
                    wall.Add(new Vector2(3455, 9935));
                    wall.Add(new Vector2(3405, 9935));
                    wall.Add(new Vector2(3405, 10035));
                    wall.Add(new Vector2(3355, 10035));
                    wall.Add(new Vector2(3355, 10135));
                    wall.Add(new Vector2(3305, 10135));
                    wall.Add(new Vector2(3305, 10185));
                    wall.Add(new Vector2(3304, 10185));
                    wall.Add(new Vector2(3304, 10283));
                    wall.Add(new Vector2(3254, 10283));
                    wall.Add(new Vector2(3254, 10383));
                    wall.Add(new Vector2(3204, 10383));
                    wall.Add(new Vector2(3204, 10483));
                    wall.Add(new Vector2(3154, 10483));
                    wall.Add(new Vector2(3154, 10583));
                    wall.Add(new Vector2(3110, 10583));
                    wall.Add(new Vector2(3110, 10587));
                    wall.Add(new Vector2(3105, 10587));
                    wall.Add(new Vector2(3105, 10687));
                    wall.Add(new Vector2(3055, 10687));
                    wall.Add(new Vector2(3055, 10787));
                    wall.Add(new Vector2(3005, 10787));
                    wall.Add(new Vector2(3005, 10887));
                    wall.Add(new Vector2(2955, 10887));
                    wall.Add(new Vector2(2955, 10950));
                    wall.Add(new Vector2(2958, 10950));
                    wall.Add(new Vector2(2958, 10985));
                    wall.Add(new Vector2(2955, 10985));
                    wall.Add(new Vector2(2955, 10987));
                    wall.Add(new Vector2(2908, 10987));
                    wall.Add(new Vector2(2908, 11085));
                    wall.Add(new Vector2(2858, 11085));
                    wall.Add(new Vector2(2858, 11185));
                    wall.Add(new Vector2(2808, 11185));
                    wall.Add(new Vector2(2808, 11285));
                    wall.Add(new Vector2(2758, 11285));
                    wall.Add(new Vector2(2758, 11335));
                    wall.Add(new Vector2(2717, 11335));
                    wall.Add(new Vector2(2717, 11337));
                    wall.Add(new Vector2(2708, 11337));
                    wall.Add(new Vector2(2708, 11385));
                    wall.Add(new Vector2(2707, 11385));
                    wall.Add(new Vector2(2707, 11387));
                    wall.Add(new Vector2(2658, 11387));
                    wall.Add(new Vector2(2658, 11485));
                    wall.Add(new Vector2(2657, 11485));
                    wall.Add(new Vector2(2657, 11487));
                    wall.Add(new Vector2(2608, 11487));
                    wall.Add(new Vector2(2608, 11535));
                    wall.Add(new Vector2(2607, 11535));
                    wall.Add(new Vector2(2607, 11537));
                    wall.Add(new Vector2(2558, 11537));
                    wall.Add(new Vector2(2558, 11540));
                    wall.Add(new Vector2(2557, 11540));
                    wall.Add(new Vector2(2557, 11587));
                    wall.Add(new Vector2(2507, 11587));
                    wall.Add(new Vector2(2507, 11637));
                    wall.Add(new Vector2(2407, 11637));
                    wall.Add(new Vector2(2407, 11687));
                    wall.Add(new Vector2(2152, 11687));
                    wall.Add(new Vector2(2152, 11683));
                    wall.Add(new Vector2(2149, 11683));
                    wall.Add(new Vector2(2149, 11637));
                    wall.Add(new Vector2(2102, 11637));
                    wall.Add(new Vector2(2102, 11633));
                    wall.Add(new Vector2(2099, 11633));
                    wall.Add(new Vector2(2099, 11587));
                    wall.Add(new Vector2(2052, 11587));
                    wall.Add(new Vector2(2052, 11583));
                    wall.Add(new Vector2(2049, 11583));
                    wall.Add(new Vector2(2049, 11537));
                    wall.Add(new Vector2(2002, 11537));
                    wall.Add(new Vector2(2002, 11533));
                    wall.Add(new Vector2(1999, 11533));
                    wall.Add(new Vector2(1999, 11437));
                    wall.Add(new Vector2(1952, 11437));
                    wall.Add(new Vector2(1952, 11434));
                    wall.Add(new Vector2(1951, 11434));
                    wall.Add(new Vector2(1951, 11433));
                    wall.Add(new Vector2(1949, 11433));
                    wall.Add(new Vector2(1949, 11387));
                    wall.Add(new Vector2(1902, 11387));
                    wall.Add(new Vector2(1902, 11384));
                    wall.Add(new Vector2(1901, 11384));
                    wall.Add(new Vector2(1901, 11383));
                    wall.Add(new Vector2(1899, 11383));
                    wall.Add(new Vector2(1899, 11237));
                    wall.Add(new Vector2(1852, 11237));
                    wall.Add(new Vector2(1852, 11234));
                    wall.Add(new Vector2(1851, 11234));
                    wall.Add(new Vector2(1851, 11233));
                    wall.Add(new Vector2(1849, 11233));
                    wall.Add(new Vector2(1849, 11198));
                    wall.Add(new Vector2(1851, 11198));
                    wall.Add(new Vector2(1851, 11086));
                    wall.Add(new Vector2(1803, 11086));
                    wall.Add(new Vector2(1803, 11084));
                    wall.Add(new Vector2(1801, 11084));
                    wall.Add(new Vector2(1801, 10449));
                    wall.Add(new Vector2(1802, 10449));
                    wall.Add(new Vector2(1802, 9834));
                    wall.Add(new Vector2(1957, 9834));
                    wall.Add(new Vector2(1957, 9837));
                    wall.Add(new Vector2(1958, 9837));
                    wall.Add(new Vector2(1958, 9884));
                    wall.Add(new Vector2(2057, 9884));
                    wall.Add(new Vector2(2057, 9887));
                    wall.Add(new Vector2(2058, 9887));
                    wall.Add(new Vector2(2058, 9934));
                    wall.Add(new Vector2(2107, 9934));
                    wall.Add(new Vector2(2107, 9937));
                    wall.Add(new Vector2(2108, 9937));
                    wall.Add(new Vector2(2108, 9984));
                    wall.Add(new Vector2(2157, 9984));
                    wall.Add(new Vector2(2157, 9987));
                    wall.Add(new Vector2(2158, 9987));
                    wall.Add(new Vector2(2158, 10084));
                    wall.Add(new Vector2(2207, 10084));
                    wall.Add(new Vector2(2207, 10087));
                    wall.Add(new Vector2(2208, 10087));
                    wall.Add(new Vector2(2208, 10134));
                    wall.Add(new Vector2(2257, 10134));
                    wall.Add(new Vector2(2257, 10234));
                    wall.Add(new Vector2(2289, 10234));
                    wall.Add(new Vector2(2289, 10233));
                    wall.Add(new Vector2(2304, 10233));
                    wall.Add(new Vector2(2304, 10234));
                    wall.Add(new Vector2(2307, 10234));
                    wall.Add(new Vector2(2307, 10333));
                    wall.Add(new Vector2(2354, 10333));
                    wall.Add(new Vector2(2354, 10334));
                    wall.Add(new Vector2(2357, 10334));
                    wall.Add(new Vector2(2357, 10383));
                    wall.Add(new Vector2(2404, 10383));
                    wall.Add(new Vector2(2404, 10384));
                    wall.Add(new Vector2(2407, 10384));
                    wall.Add(new Vector2(2407, 10433));
                    wall.Add(new Vector2(2799, 10433));
                    wall.Add(new Vector2(2799, 10383));
                    wall.Add(new Vector2(2849, 10383));
                    wall.Add(new Vector2(2849, 10333));
                    wall.Add(new Vector2(2899, 10333));
                    wall.Add(new Vector2(2899, 10283));
                    wall.Add(new Vector2(2949, 10283));
                    wall.Add(new Vector2(2949, 10183));
                    wall.Add(new Vector2(2999, 10183));
                    wall.Add(new Vector2(2999, 10133));
                    wall.Add(new Vector2(3049, 10133));
                    wall.Add(new Vector2(3049, 10083));
                    wall.Add(new Vector2(3099, 10083));
                    wall.Add(new Vector2(3099, 10033));
                    wall.Add(new Vector2(3100, 10033));
                    wall.Add(new Vector2(3100, 9985));
                    wall.Add(new Vector2(3150, 9985));
                    wall.Add(new Vector2(3150, 9975));
                    wall.Add(new Vector2(3149, 9975));
                    wall.Add(new Vector2(3149, 9885));
                    wall.Add(new Vector2(3199, 9885));
                    wall.Add(new Vector2(3199, 9785));
                    wall.Add(new Vector2(3249, 9785));
                    wall.Add(new Vector2(3249, 9234));
                    wall.Add(new Vector2(3354, 9234));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top blue jungle wall 2.
            /// </summary>
            /// <value>The top blue jungle wall 2.</value>
            public static Geometry.Polygon TopBlueJungle_Wall_2
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(3204, 8386));
                    wall.Add(new Vector2(3205, 8386));
                    wall.Add(new Vector2(3205, 8433));
                    wall.Add(new Vector2(3254, 8433));
                    wall.Add(new Vector2(3254, 8436));
                    wall.Add(new Vector2(3255, 8436));
                    wall.Add(new Vector2(3255, 8483));
                    wall.Add(new Vector2(3354, 8483));
                    wall.Add(new Vector2(3354, 8583));
                    wall.Add(new Vector2(3330, 8583));
                    wall.Add(new Vector2(3330, 8585));
                    wall.Add(new Vector2(3305, 8585));
                    wall.Add(new Vector2(3305, 8635));
                    wall.Add(new Vector2(3255, 8635));
                    wall.Add(new Vector2(3255, 8685));
                    wall.Add(new Vector2(3205, 8685));
                    wall.Add(new Vector2(3205, 8735));
                    wall.Add(new Vector2(3184, 8735));
                    wall.Add(new Vector2(3184, 8737));
                    wall.Add(new Vector2(3155, 8737));
                    wall.Add(new Vector2(3155, 8785));
                    wall.Add(new Vector2(3154, 8785));
                    wall.Add(new Vector2(3154, 8787));
                    wall.Add(new Vector2(3105, 8787));
                    wall.Add(new Vector2(3105, 8836));
                    wall.Add(new Vector2(3104, 8836));
                    wall.Add(new Vector2(3104, 8837));
                    wall.Add(new Vector2(3055, 8837));
                    wall.Add(new Vector2(3055, 8846));
                    wall.Add(new Vector2(3058, 8846));
                    wall.Add(new Vector2(3058, 8886));
                    wall.Add(new Vector2(3054, 8886));
                    wall.Add(new Vector2(3054, 8887));
                    wall.Add(new Vector2(3008, 8887));
                    wall.Add(new Vector2(3008, 8936));
                    wall.Add(new Vector2(3004, 8936));
                    wall.Add(new Vector2(3004, 8937));
                    wall.Add(new Vector2(2958, 8937));
                    wall.Add(new Vector2(2958, 8986));
                    wall.Add(new Vector2(2954, 8986));
                    wall.Add(new Vector2(2954, 8987));
                    wall.Add(new Vector2(2908, 8987));
                    wall.Add(new Vector2(2908, 9036));
                    wall.Add(new Vector2(2858, 9036));
                    wall.Add(new Vector2(2858, 9206));
                    wall.Add(new Vector2(2857, 9206));
                    wall.Add(new Vector2(2857, 9386));
                    wall.Add(new Vector2(2850, 9386));
                    wall.Add(new Vector2(2850, 9387));
                    wall.Add(new Vector2(2807, 9387));
                    wall.Add(new Vector2(2807, 9411));
                    wall.Add(new Vector2(2805, 9411));
                    wall.Add(new Vector2(2805, 9507));
                    wall.Add(new Vector2(2808, 9507));
                    wall.Add(new Vector2(2808, 9532));
                    wall.Add(new Vector2(2805, 9532));
                    wall.Add(new Vector2(2805, 9537));
                    wall.Add(new Vector2(2758, 9537));
                    wall.Add(new Vector2(2758, 9684));
                    wall.Add(new Vector2(2755, 9684));
                    wall.Add(new Vector2(2755, 9687));
                    wall.Add(new Vector2(2708, 9687));
                    wall.Add(new Vector2(2708, 9884));
                    wall.Add(new Vector2(2705, 9884));
                    wall.Add(new Vector2(2705, 9887));
                    wall.Add(new Vector2(2600, 9887));
                    wall.Add(new Vector2(2600, 9644));
                    wall.Add(new Vector2(2599, 9644));
                    wall.Add(new Vector2(2599, 9284));
                    wall.Add(new Vector2(2600, 9284));
                    wall.Add(new Vector2(2600, 9187));
                    wall.Add(new Vector2(2550, 9187));
                    wall.Add(new Vector2(2550, 9152));
                    wall.Add(new Vector2(2552, 9152));
                    wall.Add(new Vector2(2552, 8985));
                    wall.Add(new Vector2(2502, 8985));
                    wall.Add(new Vector2(2502, 8940));
                    wall.Add(new Vector2(2503, 8940));
                    wall.Add(new Vector2(2503, 8875));
                    wall.Add(new Vector2(2502, 8875));
                    wall.Add(new Vector2(2502, 8685));
                    wall.Add(new Vector2(2552, 8685));
                    wall.Add(new Vector2(2552, 8535));
                    wall.Add(new Vector2(2652, 8535));
                    wall.Add(new Vector2(2652, 8515));
                    wall.Add(new Vector2(2680, 8515));
                    wall.Add(new Vector2(2680, 8485));
                    wall.Add(new Vector2(2750, 8485));
                    wall.Add(new Vector2(2750, 8435));
                    wall.Add(new Vector2(2900, 8435));
                    wall.Add(new Vector2(2900, 8385));
                    wall.Add(new Vector2(3045, 8385));
                    wall.Add(new Vector2(3045, 8386));
                    wall.Add(new Vector2(3109, 8386));
                    wall.Add(new Vector2(3109, 8383));
                    wall.Add(new Vector2(3204, 8383));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top blue jungle gromp wall.
            /// </summary>
            /// <value>The top blue jungle gromp wall.</value>
            public static Geometry.Polygon TopBlueJungle_GrompWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(2704, 7684));
                    wall.Add(new Vector2(2754, 7684));
                    wall.Add(new Vector2(2754, 7712));
                    wall.Add(new Vector2(2755, 7712));
                    wall.Add(new Vector2(2755, 7987));
                    wall.Add(new Vector2(2705, 7987));
                    wall.Add(new Vector2(2705, 8037));
                    wall.Add(new Vector2(2605, 8037));
                    wall.Add(new Vector2(2605, 8087));
                    wall.Add(new Vector2(2505, 8087));
                    wall.Add(new Vector2(2505, 8137));
                    wall.Add(new Vector2(2407, 8137));
                    wall.Add(new Vector2(2407, 8186));
                    wall.Add(new Vector2(2405, 8186));
                    wall.Add(new Vector2(2405, 8187));
                    wall.Add(new Vector2(2280, 8187));
                    wall.Add(new Vector2(2280, 8186));
                    wall.Add(new Vector2(2257, 8186));
                    wall.Add(new Vector2(2257, 8236));
                    wall.Add(new Vector2(2157, 8236));
                    wall.Add(new Vector2(2157, 8287));
                    wall.Add(new Vector2(2107, 8287));
                    wall.Add(new Vector2(2107, 8337));
                    wall.Add(new Vector2(2057, 8337));
                    wall.Add(new Vector2(2057, 8387));
                    wall.Add(new Vector2(2007, 8387));
                    wall.Add(new Vector2(2007, 8785));
                    wall.Add(new Vector2(2057, 8785));
                    wall.Add(new Vector2(2057, 8885));
                    wall.Add(new Vector2(2107, 8885));
                    wall.Add(new Vector2(2107, 8985));
                    wall.Add(new Vector2(2157, 8985));
                    wall.Add(new Vector2(2157, 9135));
                    wall.Add(new Vector2(2207, 9135));
                    wall.Add(new Vector2(2207, 9190));
                    wall.Add(new Vector2(2206, 9190));
                    wall.Add(new Vector2(2206, 9335));
                    wall.Add(new Vector2(2256, 9335));
                    wall.Add(new Vector2(2256, 9435));
                    wall.Add(new Vector2(2254, 9435));
                    wall.Add(new Vector2(2254, 9437));
                    wall.Add(new Vector2(2206, 9437));
                    wall.Add(new Vector2(2206, 9485));
                    wall.Add(new Vector2(2204, 9485));
                    wall.Add(new Vector2(2204, 9487));
                    wall.Add(new Vector2(2099, 9487));
                    wall.Add(new Vector2(2099, 9437));
                    wall.Add(new Vector2(1999, 9437));
                    wall.Add(new Vector2(1999, 9387));
                    wall.Add(new Vector2(1849, 9387));
                    wall.Add(new Vector2(1849, 9337));
                    wall.Add(new Vector2(1809, 9337));
                    wall.Add(new Vector2(1809, 9335));
                    wall.Add(new Vector2(1781, 9335));
                    wall.Add(new Vector2(1781, 9333));
                    wall.Add(new Vector2(1752, 9333));
                    wall.Add(new Vector2(1752, 9156));
                    wall.Add(new Vector2(1749, 9156));
                    wall.Add(new Vector2(1749, 9036));
                    wall.Add(new Vector2(1699, 9036));
                    wall.Add(new Vector2(1699, 8606));
                    wall.Add(new Vector2(1701, 8606));
                    wall.Add(new Vector2(1701, 8364));
                    wall.Add(new Vector2(1700, 8364));
                    wall.Add(new Vector2(1700, 8184));
                    wall.Add(new Vector2(1701, 8184));
                    wall.Add(new Vector2(1701, 8183));
                    wall.Add(new Vector2(1750, 8183));
                    wall.Add(new Vector2(1750, 8034));
                    wall.Add(new Vector2(1850, 8034));
                    wall.Add(new Vector2(1850, 7984));
                    wall.Add(new Vector2(1950, 7984));
                    wall.Add(new Vector2(1950, 7934));
                    wall.Add(new Vector2(2050, 7934));
                    wall.Add(new Vector2(2050, 7884));
                    wall.Add(new Vector2(2150, 7884));
                    wall.Add(new Vector2(2150, 7834));
                    wall.Add(new Vector2(2160, 7834));
                    wall.Add(new Vector2(2160, 7836));
                    wall.Add(new Vector2(2200, 7836));
                    wall.Add(new Vector2(2200, 7786));
                    wall.Add(new Vector2(2300, 7786));
                    wall.Add(new Vector2(2300, 7736));
                    wall.Add(new Vector2(2350, 7736));
                    wall.Add(new Vector2(2350, 7686));
                    wall.Add(new Vector2(2374, 7686));
                    wall.Add(new Vector2(2374, 7684));
                    wall.Add(new Vector2(2449, 7684));
                    wall.Add(new Vector2(2449, 7634));
                    wall.Add(new Vector2(2704, 7634));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top blue jungle wall 3.
            /// </summary>
            /// <value>The top blue jungle wall 3.</value>
            public static Geometry.Polygon TopBlueJungle_Wall_3
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(5207, 6137));
                    wall.Add(new Vector2(5208, 6137));
                    wall.Add(new Vector2(5208, 6183));
                    wall.Add(new Vector2(5307, 6183));
                    wall.Add(new Vector2(5307, 6187));
                    wall.Add(new Vector2(5308, 6187));
                    wall.Add(new Vector2(5308, 6233));
                    wall.Add(new Vector2(5317, 6233));
                    wall.Add(new Vector2(5317, 6237));
                    wall.Add(new Vector2(5358, 6237));
                    wall.Add(new Vector2(5358, 6287));
                    wall.Add(new Vector2(5408, 6287));
                    wall.Add(new Vector2(5408, 6337));
                    wall.Add(new Vector2(5458, 6337));
                    wall.Add(new Vector2(5458, 6387));
                    wall.Add(new Vector2(5508, 6387));
                    wall.Add(new Vector2(5508, 6435));
                    wall.Add(new Vector2(5555, 6435));
                    wall.Add(new Vector2(5555, 6437));
                    wall.Add(new Vector2(5558, 6437));
                    wall.Add(new Vector2(5558, 6485));
                    wall.Add(new Vector2(5605, 6485));
                    wall.Add(new Vector2(5605, 6486));
                    wall.Add(new Vector2(5656, 6486));
                    wall.Add(new Vector2(5656, 6533));
                    wall.Add(new Vector2(5704, 6533));
                    wall.Add(new Vector2(5704, 6564));
                    wall.Add(new Vector2(5708, 6564));
                    wall.Add(new Vector2(5708, 6583));
                    wall.Add(new Vector2(5729, 6583));
                    wall.Add(new Vector2(5729, 6584));
                    wall.Add(new Vector2(5758, 6584));
                    wall.Add(new Vector2(5758, 6634));
                    wall.Add(new Vector2(5808, 6634));
                    wall.Add(new Vector2(5808, 6684));
                    wall.Add(new Vector2(5828, 6684));
                    wall.Add(new Vector2(5828, 6686));
                    wall.Add(new Vector2(5855, 6686));
                    wall.Add(new Vector2(5855, 6704));
                    wall.Add(new Vector2(5857, 6704));
                    wall.Add(new Vector2(5857, 6834));
                    wall.Add(new Vector2(5907, 6834));
                    wall.Add(new Vector2(5907, 6934));
                    wall.Add(new Vector2(5905, 6934));
                    wall.Add(new Vector2(5905, 6937));
                    wall.Add(new Vector2(5857, 6937));
                    wall.Add(new Vector2(5857, 7054));
                    wall.Add(new Vector2(5855, 7054));
                    wall.Add(new Vector2(5855, 7137));
                    wall.Add(new Vector2(5805, 7137));
                    wall.Add(new Vector2(5805, 7187));
                    wall.Add(new Vector2(5756, 7187));
                    wall.Add(new Vector2(5756, 7283));
                    wall.Add(new Vector2(5755, 7283));
                    wall.Add(new Vector2(5755, 7287));
                    wall.Add(new Vector2(5706, 7287));
                    wall.Add(new Vector2(5706, 7333));
                    wall.Add(new Vector2(5705, 7333));
                    wall.Add(new Vector2(5705, 7337));
                    wall.Add(new Vector2(5656, 7337));
                    wall.Add(new Vector2(5656, 7383));
                    wall.Add(new Vector2(5654, 7383));
                    wall.Add(new Vector2(5654, 7387));
                    wall.Add(new Vector2(5556, 7387));
                    wall.Add(new Vector2(5556, 7433));
                    wall.Add(new Vector2(5554, 7433));
                    wall.Add(new Vector2(5554, 7437));
                    wall.Add(new Vector2(5506, 7437));
                    wall.Add(new Vector2(5506, 7483));
                    wall.Add(new Vector2(5487, 7483));
                    wall.Add(new Vector2(5487, 7484));
                    wall.Add(new Vector2(5308, 7484));
                    wall.Add(new Vector2(5308, 7533));
                    wall.Add(new Vector2(5307, 7533));
                    wall.Add(new Vector2(5307, 7534));
                    wall.Add(new Vector2(5132, 7534));
                    wall.Add(new Vector2(5132, 7533));
                    wall.Add(new Vector2(5003, 7533));
                    wall.Add(new Vector2(5003, 7532));
                    wall.Add(new Vector2(5001, 7532));
                    wall.Add(new Vector2(5001, 7483));
                    wall.Add(new Vector2(4953, 7483));
                    wall.Add(new Vector2(4953, 7482));
                    wall.Add(new Vector2(4951, 7482));
                    wall.Add(new Vector2(4951, 7433));
                    wall.Add(new Vector2(4903, 7433));
                    wall.Add(new Vector2(4903, 7432));
                    wall.Add(new Vector2(4901, 7432));
                    wall.Add(new Vector2(4901, 7383));
                    wall.Add(new Vector2(4853, 7383));
                    wall.Add(new Vector2(4853, 7382));
                    wall.Add(new Vector2(4851, 7382));
                    wall.Add(new Vector2(4851, 7333));
                    wall.Add(new Vector2(4803, 7333));
                    wall.Add(new Vector2(4803, 7248));
                    wall.Add(new Vector2(4802, 7248));
                    wall.Add(new Vector2(4802, 7233));
                    wall.Add(new Vector2(4852, 7233));
                    wall.Add(new Vector2(4852, 7183));
                    wall.Add(new Vector2(4902, 7183));
                    wall.Add(new Vector2(4902, 7133));
                    wall.Add(new Vector2(4952, 7133));
                    wall.Add(new Vector2(4952, 7027));
                    wall.Add(new Vector2(4950, 7027));
                    wall.Add(new Vector2(4950, 6637));
                    wall.Add(new Vector2(4903, 6637));
                    wall.Add(new Vector2(4903, 6635));
                    wall.Add(new Vector2(4901, 6635));
                    wall.Add(new Vector2(4901, 6495));
                    wall.Add(new Vector2(4902, 6495));
                    wall.Add(new Vector2(4902, 6387));
                    wall.Add(new Vector2(4898, 6387));
                    wall.Add(new Vector2(4898, 6384));
                    wall.Add(new Vector2(4853, 6384));
                    wall.Add(new Vector2(4853, 6383));
                    wall.Add(new Vector2(4852, 6383));
                    wall.Add(new Vector2(4852, 6284));
                    wall.Add(new Vector2(4803, 6284));
                    wall.Add(new Vector2(4803, 6283));
                    wall.Add(new Vector2(4802, 6283));
                    wall.Add(new Vector2(4802, 6183));
                    wall.Add(new Vector2(4952, 6183));
                    wall.Add(new Vector2(4952, 6133));
                    wall.Add(new Vector2(5207, 6133));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top blue jungle wolf wall.
            /// </summary>
            /// <value>The top blue jungle wolf wall.</value>
            public static Geometry.Polygon TopBlueJungle_WolfWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(4208, 6186));
                    wall.Add(new Vector2(4258, 6186));
                    wall.Add(new Vector2(4258, 6234));
                    wall.Add(new Vector2(4304, 6234));
                    wall.Add(new Vector2(4304, 6236));
                    wall.Add(new Vector2(4308, 6236));
                    wall.Add(new Vector2(4308, 6284));
                    wall.Add(new Vector2(4314, 6284));
                    wall.Add(new Vector2(4314, 6286));
                    wall.Add(new Vector2(4363, 6286));
                    wall.Add(new Vector2(4363, 6287));
                    wall.Add(new Vector2(4407, 6287));
                    wall.Add(new Vector2(4407, 6337));
                    wall.Add(new Vector2(4458, 6337));
                    wall.Add(new Vector2(4458, 6482));
                    wall.Add(new Vector2(4508, 6482));
                    wall.Add(new Vector2(4508, 6632));
                    wall.Add(new Vector2(4538, 6632));
                    wall.Add(new Vector2(4538, 6636));
                    wall.Add(new Vector2(4558, 6636));
                    wall.Add(new Vector2(4558, 6886));
                    wall.Add(new Vector2(4550, 6886));
                    wall.Add(new Vector2(4550, 6887));
                    wall.Add(new Vector2(4508, 6887));
                    wall.Add(new Vector2(4508, 6956));
                    wall.Add(new Vector2(4505, 6956));
                    wall.Add(new Vector2(4505, 6987));
                    wall.Add(new Vector2(4405, 6987));
                    wall.Add(new Vector2(4405, 7037));
                    wall.Add(new Vector2(4305, 7037));
                    wall.Add(new Vector2(4305, 7087));
                    wall.Add(new Vector2(4058, 7087));
                    wall.Add(new Vector2(4058, 7136));
                    wall.Add(new Vector2(3849, 7136));
                    wall.Add(new Vector2(3849, 7086));
                    wall.Add(new Vector2(3699, 7086));
                    wall.Add(new Vector2(3699, 7036));
                    wall.Add(new Vector2(3599, 7036));
                    wall.Add(new Vector2(3599, 6986));
                    wall.Add(new Vector2(3549, 6986));
                    wall.Add(new Vector2(3549, 6936));
                    wall.Add(new Vector2(3499, 6936));
                    wall.Add(new Vector2(3499, 6886));
                    wall.Add(new Vector2(3494, 6886));
                    wall.Add(new Vector2(3494, 6885));
                    wall.Add(new Vector2(3451, 6885));
                    wall.Add(new Vector2(3451, 6785));
                    wall.Add(new Vector2(3501, 6785));
                    wall.Add(new Vector2(3501, 6735));
                    wall.Add(new Vector2(3899, 6735));
                    wall.Add(new Vector2(3899, 6687));
                    wall.Add(new Vector2(3902, 6687));
                    wall.Add(new Vector2(3902, 6685));
                    wall.Add(new Vector2(3949, 6685));
                    wall.Add(new Vector2(3949, 6634));
                    wall.Add(new Vector2(3999, 6634));
                    wall.Add(new Vector2(3999, 6584));
                    wall.Add(new Vector2(4043, 6584));
                    wall.Add(new Vector2(4043, 6582));
                    wall.Add(new Vector2(4049, 6582));
                    wall.Add(new Vector2(4049, 6534));
                    wall.Add(new Vector2(4053, 6534));
                    wall.Add(new Vector2(4053, 6532));
                    wall.Add(new Vector2(4099, 6532));
                    wall.Add(new Vector2(4099, 6437));
                    wall.Add(new Vector2(4072, 6437));
                    wall.Add(new Vector2(4072, 6436));
                    wall.Add(new Vector2(4052, 6436));
                    wall.Add(new Vector2(4052, 6434));
                    wall.Add(new Vector2(4049, 6434));
                    wall.Add(new Vector2(4049, 6336));
                    wall.Add(new Vector2(4002, 6336));
                    wall.Add(new Vector2(4002, 6334));
                    wall.Add(new Vector2(3999, 6334));
                    wall.Add(new Vector2(3999, 6286));
                    wall.Add(new Vector2(3952, 6286));
                    wall.Add(new Vector2(3952, 6284));
                    wall.Add(new Vector2(3949, 6284));
                    wall.Add(new Vector2(3949, 6189));
                    wall.Add(new Vector2(3952, 6189));
                    wall.Add(new Vector2(3952, 6186));
                    wall.Add(new Vector2(4002, 6186));
                    wall.Add(new Vector2(4002, 6136));
                    wall.Add(new Vector2(4208, 6136));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top blue jungle wall 4.
            /// </summary>
            /// <value>The top blue jungle wall 4.</value>
            public static Geometry.Polygon TopBlueJungle_Wall_4
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(4206, 5133));
                    wall.Add(new Vector2(4256, 5133));
                    wall.Add(new Vector2(4256, 5153));
                    wall.Add(new Vector2(4257, 5153));
                    wall.Add(new Vector2(4257, 5183));
                    wall.Add(new Vector2(4307, 5183));
                    wall.Add(new Vector2(4307, 5233));
                    wall.Add(new Vector2(4357, 5233));
                    wall.Add(new Vector2(4357, 5283));
                    wall.Add(new Vector2(4407, 5283));
                    wall.Add(new Vector2(4407, 5333));
                    wall.Add(new Vector2(4457, 5333));
                    wall.Add(new Vector2(4457, 5383));
                    wall.Add(new Vector2(4507, 5383));
                    wall.Add(new Vector2(4507, 5433));
                    wall.Add(new Vector2(4557, 5433));
                    wall.Add(new Vector2(4557, 5483));
                    wall.Add(new Vector2(4606, 5483));
                    wall.Add(new Vector2(4606, 5485));
                    wall.Add(new Vector2(4607, 5485));
                    wall.Add(new Vector2(4607, 5533));
                    wall.Add(new Vector2(4646, 5533));
                    wall.Add(new Vector2(4646, 5535));
                    wall.Add(new Vector2(4657, 5535));
                    wall.Add(new Vector2(4657, 5585));
                    wall.Add(new Vector2(4707, 5585));
                    wall.Add(new Vector2(4707, 5635));
                    wall.Add(new Vector2(4757, 5635));
                    wall.Add(new Vector2(4757, 5685));
                    wall.Add(new Vector2(4807, 5685));
                    wall.Add(new Vector2(4807, 5735));
                    wall.Add(new Vector2(4854, 5735));
                    wall.Add(new Vector2(4854, 5835));
                    wall.Add(new Vector2(4557, 5835));
                    wall.Add(new Vector2(4557, 5885));
                    wall.Add(new Vector2(4507, 5885));
                    wall.Add(new Vector2(4507, 5934));
                    wall.Add(new Vector2(4504, 5934));
                    wall.Add(new Vector2(4504, 5935));
                    wall.Add(new Vector2(4454, 5935));
                    wall.Add(new Vector2(4454, 5934));
                    wall.Add(new Vector2(4402, 5934));
                    wall.Add(new Vector2(4402, 5884));
                    wall.Add(new Vector2(4352, 5884));
                    wall.Add(new Vector2(4352, 5834));
                    wall.Add(new Vector2(4252, 5834));
                    wall.Add(new Vector2(4252, 5823));
                    wall.Add(new Vector2(4251, 5823));
                    wall.Add(new Vector2(4251, 5784));
                    wall.Add(new Vector2(3932, 5784));
                    wall.Add(new Vector2(3932, 5783));
                    wall.Add(new Vector2(3804, 5783));
                    wall.Add(new Vector2(3804, 5833));
                    wall.Add(new Vector2(3678, 5833));
                    wall.Add(new Vector2(3678, 5835));
                    wall.Add(new Vector2(3658, 5835));
                    wall.Add(new Vector2(3658, 5885));
                    wall.Add(new Vector2(3558, 5885));
                    wall.Add(new Vector2(3558, 5910));
                    wall.Add(new Vector2(3556, 5910));
                    wall.Add(new Vector2(3556, 5934));
                    wall.Add(new Vector2(3456, 5934));
                    wall.Add(new Vector2(3456, 5984));
                    wall.Add(new Vector2(3406, 5984));
                    wall.Add(new Vector2(3406, 6084));
                    wall.Add(new Vector2(3356, 6084));
                    wall.Add(new Vector2(3356, 6134));
                    wall.Add(new Vector2(3351, 6134));
                    wall.Add(new Vector2(3351, 6135));
                    wall.Add(new Vector2(3306, 6135));
                    wall.Add(new Vector2(3306, 6185));
                    wall.Add(new Vector2(3256, 6185));
                    wall.Add(new Vector2(3256, 6235));
                    wall.Add(new Vector2(3206, 6235));
                    wall.Add(new Vector2(3206, 6335));
                    wall.Add(new Vector2(3156, 6335));
                    wall.Add(new Vector2(3156, 6385));
                    wall.Add(new Vector2(3106, 6385));
                    wall.Add(new Vector2(3106, 6485));
                    wall.Add(new Vector2(3056, 6485));
                    wall.Add(new Vector2(3056, 6674));
                    wall.Add(new Vector2(3057, 6674));
                    wall.Add(new Vector2(3057, 7034));
                    wall.Add(new Vector2(3007, 7034));
                    wall.Add(new Vector2(3007, 7084));
                    wall.Add(new Vector2(2902, 7084));
                    wall.Add(new Vector2(2902, 7034));
                    wall.Add(new Vector2(2852, 7034));
                    wall.Add(new Vector2(2852, 6955));
                    wall.Add(new Vector2(2851, 6955));
                    wall.Add(new Vector2(2851, 6685));
                    wall.Add(new Vector2(2801, 6685));
                    wall.Add(new Vector2(2801, 6335));
                    wall.Add(new Vector2(2851, 6335));
                    wall.Add(new Vector2(2851, 6285));
                    wall.Add(new Vector2(2901, 6285));
                    wall.Add(new Vector2(2901, 6235));
                    wall.Add(new Vector2(2951, 6235));
                    wall.Add(new Vector2(2951, 6185));
                    wall.Add(new Vector2(3001, 6185));
                    wall.Add(new Vector2(3001, 6135));
                    wall.Add(new Vector2(3051, 6135));
                    wall.Add(new Vector2(3051, 6035));
                    wall.Add(new Vector2(3081, 6035));
                    wall.Add(new Vector2(3081, 6034));
                    wall.Add(new Vector2(3101, 6034));
                    wall.Add(new Vector2(3101, 5984));
                    wall.Add(new Vector2(3151, 5984));
                    wall.Add(new Vector2(3151, 5699));
                    wall.Add(new Vector2(3153, 5699));
                    wall.Add(new Vector2(3153, 5685));
                    wall.Add(new Vector2(3103, 5685));
                    wall.Add(new Vector2(3103, 5648));
                    wall.Add(new Vector2(3102, 5648));
                    wall.Add(new Vector2(3102, 5585));
                    wall.Add(new Vector2(3053, 5585));
                    wall.Add(new Vector2(3053, 5583));
                    wall.Add(new Vector2(3052, 5583));
                    wall.Add(new Vector2(3052, 5535));
                    wall.Add(new Vector2(3003, 5535));
                    wall.Add(new Vector2(3003, 5435));
                    wall.Add(new Vector2(3052, 5435));
                    wall.Add(new Vector2(3052, 5383));
                    wall.Add(new Vector2(3302, 5383));
                    wall.Add(new Vector2(3302, 5333));
                    wall.Add(new Vector2(3452, 5333));
                    wall.Add(new Vector2(3452, 5283));
                    wall.Add(new Vector2(3467, 5283));
                    wall.Add(new Vector2(3467, 5284));
                    wall.Add(new Vector2(3602, 5284));
                    wall.Add(new Vector2(3602, 5234));
                    wall.Add(new Vector2(3751, 5234));
                    wall.Add(new Vector2(3751, 5186));
                    wall.Add(new Vector2(3752, 5186));
                    wall.Add(new Vector2(3752, 5184));
                    wall.Add(new Vector2(3787, 5184));
                    wall.Add(new Vector2(3787, 5186));
                    wall.Add(new Vector2(3861, 5186));
                    wall.Add(new Vector2(3861, 5183));
                    wall.Add(new Vector2(3901, 5183));
                    wall.Add(new Vector2(3901, 5133));
                    wall.Add(new Vector2(4001, 5133));
                    wall.Add(new Vector2(4001, 5083));
                    wall.Add(new Vector2(4206, 5083));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top blue jungle wall 5.
            /// </summary>
            /// <value>The top blue jungle wall 5.</value>
            public static Geometry.Polygon TopBlueJungle_Wall_5
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(2219, 5536));
                    wall.Add(new Vector2(2407, 5536));
                    wall.Add(new Vector2(2407, 5586));
                    wall.Add(new Vector2(2457, 5586));
                    wall.Add(new Vector2(2457, 5636));
                    wall.Add(new Vector2(2557, 5636));
                    wall.Add(new Vector2(2557, 5686));
                    wall.Add(new Vector2(2607, 5686));
                    wall.Add(new Vector2(2607, 5936));
                    wall.Add(new Vector2(2507, 5936));
                    wall.Add(new Vector2(2507, 5986));
                    wall.Add(new Vector2(2457, 5986));
                    wall.Add(new Vector2(2457, 6033));
                    wall.Add(new Vector2(2407, 6033));
                    wall.Add(new Vector2(2407, 6363));
                    wall.Add(new Vector2(2406, 6363));
                    wall.Add(new Vector2(2406, 6533));
                    wall.Add(new Vector2(2456, 6533));
                    wall.Add(new Vector2(2456, 6763));
                    wall.Add(new Vector2(2454, 6763));
                    wall.Add(new Vector2(2454, 7184));
                    wall.Add(new Vector2(2453, 7184));
                    wall.Add(new Vector2(2453, 7186));
                    wall.Add(new Vector2(2354, 7186));
                    wall.Add(new Vector2(2354, 7194));
                    wall.Add(new Vector2(2353, 7194));
                    wall.Add(new Vector2(2353, 7236));
                    wall.Add(new Vector2(2303, 7236));
                    wall.Add(new Vector2(2303, 7286));
                    wall.Add(new Vector2(2256, 7286));
                    wall.Add(new Vector2(2256, 7334));
                    wall.Add(new Vector2(2253, 7334));
                    wall.Add(new Vector2(2253, 7336));
                    wall.Add(new Vector2(2156, 7336));
                    wall.Add(new Vector2(2156, 7386));
                    wall.Add(new Vector2(2106, 7386));
                    wall.Add(new Vector2(2106, 7436));
                    wall.Add(new Vector2(2006, 7436));
                    wall.Add(new Vector2(2006, 7486));
                    wall.Add(new Vector2(1956, 7486));
                    wall.Add(new Vector2(1956, 7536));
                    wall.Add(new Vector2(1801, 7536));
                    wall.Add(new Vector2(1801, 7535));
                    wall.Add(new Vector2(1800, 7535));
                    wall.Add(new Vector2(1800, 7205));
                    wall.Add(new Vector2(1799, 7205));
                    wall.Add(new Vector2(1799, 6785));
                    wall.Add(new Vector2(1749, 6785));
                    wall.Add(new Vector2(1749, 6355));
                    wall.Add(new Vector2(1752, 6355));
                    wall.Add(new Vector2(1752, 5874));
                    wall.Add(new Vector2(1749, 5874));
                    wall.Add(new Vector2(1749, 5584));
                    wall.Add(new Vector2(2099, 5584));
                    wall.Add(new Vector2(2099, 5534));
                    wall.Add(new Vector2(2219, 5534));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top blue jungle blue buff wall.
            /// </summary>
            /// <value>The top blue jungle blue buff wall.</value>
            public static Geometry.Polygon TopBlueJungle_BlueBuffWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(3449, 7436));
                    wall.Add(new Vector2(3562, 7436));
                    wall.Add(new Vector2(3562, 7435));
                    wall.Add(new Vector2(4207, 7435));
                    wall.Add(new Vector2(4207, 7466));
                    wall.Add(new Vector2(4208, 7466));
                    wall.Add(new Vector2(4208, 7485));
                    wall.Add(new Vector2(4455, 7485));
                    wall.Add(new Vector2(4455, 7486));
                    wall.Add(new Vector2(4458, 7486));
                    wall.Add(new Vector2(4458, 7535));
                    wall.Add(new Vector2(4505, 7535));
                    wall.Add(new Vector2(4505, 7536));
                    wall.Add(new Vector2(4508, 7536));
                    wall.Add(new Vector2(4508, 7585));
                    wall.Add(new Vector2(4555, 7585));
                    wall.Add(new Vector2(4555, 7586));
                    wall.Add(new Vector2(4558, 7586));
                    wall.Add(new Vector2(4558, 7635));
                    wall.Add(new Vector2(4605, 7635));
                    wall.Add(new Vector2(4605, 7636));
                    wall.Add(new Vector2(4608, 7636));
                    wall.Add(new Vector2(4608, 7736));
                    wall.Add(new Vector2(4558, 7736));
                    wall.Add(new Vector2(4558, 7886));
                    wall.Add(new Vector2(4508, 7886));
                    wall.Add(new Vector2(4508, 7986));
                    wall.Add(new Vector2(4480, 7986));
                    wall.Add(new Vector2(4480, 7987));
                    wall.Add(new Vector2(4458, 7987));
                    wall.Add(new Vector2(4458, 8036));
                    wall.Add(new Vector2(4455, 8036));
                    wall.Add(new Vector2(4455, 8037));
                    wall.Add(new Vector2(4408, 8037));
                    wall.Add(new Vector2(4408, 8136));
                    wall.Add(new Vector2(4405, 8136));
                    wall.Add(new Vector2(4405, 8137));
                    wall.Add(new Vector2(4358, 8137));
                    wall.Add(new Vector2(4358, 8231));
                    wall.Add(new Vector2(4357, 8231));
                    wall.Add(new Vector2(4357, 8233));
                    wall.Add(new Vector2(4356, 8233));
                    wall.Add(new Vector2(4356, 8237));
                    wall.Add(new Vector2(4307, 8237));
                    wall.Add(new Vector2(4307, 8333));
                    wall.Add(new Vector2(4357, 8333));
                    wall.Add(new Vector2(4357, 8433));
                    wall.Add(new Vector2(4407, 8433));
                    wall.Add(new Vector2(4407, 8483));
                    wall.Add(new Vector2(4455, 8483));
                    wall.Add(new Vector2(4455, 8484));
                    wall.Add(new Vector2(4456, 8484));
                    wall.Add(new Vector2(4456, 8533));
                    wall.Add(new Vector2(4505, 8533));
                    wall.Add(new Vector2(4505, 8534));
                    wall.Add(new Vector2(4506, 8534));
                    wall.Add(new Vector2(4506, 8583));
                    wall.Add(new Vector2(4555, 8583));
                    wall.Add(new Vector2(4555, 8584));
                    wall.Add(new Vector2(4556, 8584));
                    wall.Add(new Vector2(4556, 8586));
                    wall.Add(new Vector2(4557, 8586));
                    wall.Add(new Vector2(4557, 8633));
                    wall.Add(new Vector2(4655, 8633));
                    wall.Add(new Vector2(4655, 8634));
                    wall.Add(new Vector2(4656, 8634));
                    wall.Add(new Vector2(4656, 8683));
                    wall.Add(new Vector2(4705, 8683));
                    wall.Add(new Vector2(4705, 8684));
                    wall.Add(new Vector2(4706, 8684));
                    wall.Add(new Vector2(4706, 8734));
                    wall.Add(new Vector2(4806, 8734));
                    wall.Add(new Vector2(4806, 8834));
                    wall.Add(new Vector2(4756, 8834));
                    wall.Add(new Vector2(4756, 8884));
                    wall.Add(new Vector2(4656, 8884));
                    wall.Add(new Vector2(4656, 8934));
                    wall.Add(new Vector2(4602, 8934));
                    wall.Add(new Vector2(4602, 8936));
                    wall.Add(new Vector2(4557, 8936));
                    wall.Add(new Vector2(4557, 8986));
                    wall.Add(new Vector2(4407, 8986));
                    wall.Add(new Vector2(4407, 9036));
                    wall.Add(new Vector2(4357, 9036));
                    wall.Add(new Vector2(4357, 9086));
                    wall.Add(new Vector2(4257, 9086));
                    wall.Add(new Vector2(4257, 9126));
                    wall.Add(new Vector2(4254, 9126));
                    wall.Add(new Vector2(4254, 9133));
                    wall.Add(new Vector2(4219, 9133));
                    wall.Add(new Vector2(4219, 9135));
                    wall.Add(new Vector2(4204, 9135));
                    wall.Add(new Vector2(4204, 9185));
                    wall.Add(new Vector2(4107, 9185));
                    wall.Add(new Vector2(4107, 9235));
                    wall.Add(new Vector2(4057, 9235));
                    wall.Add(new Vector2(4057, 9245));
                    wall.Add(new Vector2(4054, 9245));
                    wall.Add(new Vector2(4054, 9285));
                    wall.Add(new Vector2(4004, 9285));
                    wall.Add(new Vector2(4004, 9335));
                    wall.Add(new Vector2(3954, 9335));
                    wall.Add(new Vector2(3954, 9383));
                    wall.Add(new Vector2(3849, 9383));
                    wall.Add(new Vector2(3849, 9235));
                    wall.Add(new Vector2(3799, 9235));
                    wall.Add(new Vector2(3799, 9147));
                    wall.Add(new Vector2(3798, 9147));
                    wall.Add(new Vector2(3798, 9087));
                    wall.Add(new Vector2(3748, 9087));
                    wall.Add(new Vector2(3748, 9037));
                    wall.Add(new Vector2(3698, 9037));
                    wall.Add(new Vector2(3698, 8937));
                    wall.Add(new Vector2(3648, 8937));
                    wall.Add(new Vector2(3648, 8837));
                    wall.Add(new Vector2(3652, 8837));
                    wall.Add(new Vector2(3652, 8835));
                    wall.Add(new Vector2(3748, 8835));
                    wall.Add(new Vector2(3748, 8787));
                    wall.Add(new Vector2(3752, 8787));
                    wall.Add(new Vector2(3752, 8785));
                    wall.Add(new Vector2(3798, 8785));
                    wall.Add(new Vector2(3798, 8607));
                    wall.Add(new Vector2(3799, 8607));
                    wall.Add(new Vector2(3799, 8238));
                    wall.Add(new Vector2(3802, 8238));
                    wall.Add(new Vector2(3802, 8137));
                    wall.Add(new Vector2(3900, 8137));
                    wall.Add(new Vector2(3900, 8087));
                    wall.Add(new Vector2(3902, 8087));
                    wall.Add(new Vector2(3902, 8086));
                    wall.Add(new Vector2(4007, 8086));
                    wall.Add(new Vector2(4007, 8083));
                    wall.Add(new Vector2(4050, 8083));
                    wall.Add(new Vector2(4050, 8037));
                    wall.Add(new Vector2(4052, 8037));
                    wall.Add(new Vector2(4052, 8033));
                    wall.Add(new Vector2(4150, 8033));
                    wall.Add(new Vector2(4150, 7780));
                    wall.Add(new Vector2(4149, 7780));
                    wall.Add(new Vector2(4149, 7737));
                    wall.Add(new Vector2(4100, 7737));
                    wall.Add(new Vector2(4100, 7735));
                    wall.Add(new Vector2(4099, 7735));
                    wall.Add(new Vector2(4099, 7687));
                    wall.Add(new Vector2(3935, 7687));
                    wall.Add(new Vector2(3935, 7686));
                    wall.Add(new Vector2(3202, 7686));
                    wall.Add(new Vector2(3202, 7685));
                    wall.Add(new Vector2(3199, 7685));
                    wall.Add(new Vector2(3199, 7636));
                    wall.Add(new Vector2(3157, 7636));
                    wall.Add(new Vector2(3157, 7635));
                    wall.Add(new Vector2(3149, 7635));
                    wall.Add(new Vector2(3149, 7485));
                    wall.Add(new Vector2(3199, 7485));
                    wall.Add(new Vector2(3199, 7435));
                    wall.Add(new Vector2(3449, 7435));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the red mid wall 1.
            /// </summary>
            /// <value>The red mid wall 1.</value>
            public static Geometry.Polygon RedMid_Wall_1
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(7505, 8037));
                    wall.Add(new Vector2(7506, 8037));
                    wall.Add(new Vector2(7506, 8086));
                    wall.Add(new Vector2(7555, 8086));
                    wall.Add(new Vector2(7555, 8087));
                    wall.Add(new Vector2(7556, 8087));
                    wall.Add(new Vector2(7556, 8287));
                    wall.Add(new Vector2(7506, 8287));
                    wall.Add(new Vector2(7506, 8337));
                    wall.Add(new Vector2(7456, 8337));
                    wall.Add(new Vector2(7456, 8387));
                    wall.Add(new Vector2(7407, 8387));
                    wall.Add(new Vector2(7407, 8436));
                    wall.Add(new Vector2(7406, 8436));
                    wall.Add(new Vector2(7406, 8437));
                    wall.Add(new Vector2(7307, 8437));
                    wall.Add(new Vector2(7307, 8486));
                    wall.Add(new Vector2(7282, 8486));
                    wall.Add(new Vector2(7282, 8487));
                    wall.Add(new Vector2(7257, 8487));
                    wall.Add(new Vector2(7257, 8537));
                    wall.Add(new Vector2(7207, 8537));
                    wall.Add(new Vector2(7207, 8587));
                    wall.Add(new Vector2(7157, 8587));
                    wall.Add(new Vector2(7157, 8637));
                    wall.Add(new Vector2(7107, 8637));
                    wall.Add(new Vector2(7107, 8687));
                    wall.Add(new Vector2(7057, 8687));
                    wall.Add(new Vector2(7057, 8737));
                    wall.Add(new Vector2(7007, 8737));
                    wall.Add(new Vector2(7007, 8787));
                    wall.Add(new Vector2(6957, 8787));
                    wall.Add(new Vector2(6957, 8837));
                    wall.Add(new Vector2(6857, 8837));
                    wall.Add(new Vector2(6857, 8885));
                    wall.Add(new Vector2(6854, 8885));
                    wall.Add(new Vector2(6854, 8887));
                    wall.Add(new Vector2(6807, 8887));
                    wall.Add(new Vector2(6807, 8935));
                    wall.Add(new Vector2(6757, 8935));
                    wall.Add(new Vector2(6757, 8985));
                    wall.Add(new Vector2(6722, 8985));
                    wall.Add(new Vector2(6722, 8986));
                    wall.Add(new Vector2(6657, 8986));
                    wall.Add(new Vector2(6657, 9036));
                    wall.Add(new Vector2(6507, 9036));
                    wall.Add(new Vector2(6507, 9086));
                    wall.Add(new Vector2(6301, 9086));
                    wall.Add(new Vector2(6301, 8986));
                    wall.Add(new Vector2(6401, 8986));
                    wall.Add(new Vector2(6401, 8936));
                    wall.Add(new Vector2(6425, 8936));
                    wall.Add(new Vector2(6425, 8935));
                    wall.Add(new Vector2(6450, 8935));
                    wall.Add(new Vector2(6450, 8885));
                    wall.Add(new Vector2(6550, 8885));
                    wall.Add(new Vector2(6550, 8835));
                    wall.Add(new Vector2(6600, 8835));
                    wall.Add(new Vector2(6600, 8785));
                    wall.Add(new Vector2(6699, 8785));
                    wall.Add(new Vector2(6699, 8737));
                    wall.Add(new Vector2(6700, 8737));
                    wall.Add(new Vector2(6700, 8735));
                    wall.Add(new Vector2(6749, 8735));
                    wall.Add(new Vector2(6749, 8687));
                    wall.Add(new Vector2(6750, 8687));
                    wall.Add(new Vector2(6750, 8685));
                    wall.Add(new Vector2(6799, 8685));
                    wall.Add(new Vector2(6799, 8637));
                    wall.Add(new Vector2(6801, 8637));
                    wall.Add(new Vector2(6801, 8636));
                    wall.Add(new Vector2(6802, 8636));
                    wall.Add(new Vector2(6802, 8635));
                    wall.Add(new Vector2(6899, 8635));
                    wall.Add(new Vector2(6899, 8587));
                    wall.Add(new Vector2(6901, 8587));
                    wall.Add(new Vector2(6901, 8586));
                    wall.Add(new Vector2(6902, 8586));
                    wall.Add(new Vector2(6902, 8585));
                    wall.Add(new Vector2(6949, 8585));
                    wall.Add(new Vector2(6949, 8487));
                    wall.Add(new Vector2(6950, 8487));
                    wall.Add(new Vector2(6950, 8486));
                    wall.Add(new Vector2(6952, 8486));
                    wall.Add(new Vector2(6952, 8485));
                    wall.Add(new Vector2(6999, 8485));
                    wall.Add(new Vector2(6999, 8437));
                    wall.Add(new Vector2(7000, 8437));
                    wall.Add(new Vector2(7000, 8436));
                    wall.Add(new Vector2(7049, 8436));
                    wall.Add(new Vector2(7049, 8402));
                    wall.Add(new Vector2(7050, 8402));
                    wall.Add(new Vector2(7050, 8386));
                    wall.Add(new Vector2(7100, 8386));
                    wall.Add(new Vector2(7100, 8336));
                    wall.Add(new Vector2(7150, 8336));
                    wall.Add(new Vector2(7150, 8286));
                    wall.Add(new Vector2(7200, 8286));
                    wall.Add(new Vector2(7200, 8186));
                    wall.Add(new Vector2(7250, 8186));
                    wall.Add(new Vector2(7250, 8136));
                    wall.Add(new Vector2(7300, 8136));
                    wall.Add(new Vector2(7300, 8086));
                    wall.Add(new Vector2(7350, 8086));
                    wall.Add(new Vector2(7350, 8036));
                    wall.Add(new Vector2(7505, 8036));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the red mid wall 2.
            /// </summary>
            /// <value>The red mid wall 2.</value>
            public static Geometry.Polygon RedMid_Wall_2
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(9658, 6434));
                    wall.Add(new Vector2(9758, 6434));
                    wall.Add(new Vector2(9758, 6484));
                    wall.Add(new Vector2(9808, 6484));
                    wall.Add(new Vector2(9808, 6534));
                    wall.Add(new Vector2(9858, 6534));
                    wall.Add(new Vector2(9858, 6584));
                    wall.Add(new Vector2(9908, 6584));
                    wall.Add(new Vector2(9908, 6634));
                    wall.Add(new Vector2(9958, 6634));
                    wall.Add(new Vector2(9958, 6835));
                    wall.Add(new Vector2(9908, 6835));
                    wall.Add(new Vector2(9908, 6885));
                    wall.Add(new Vector2(9658, 6885));
                    wall.Add(new Vector2(9658, 6935));
                    wall.Add(new Vector2(9627, 6935));
                    wall.Add(new Vector2(9627, 6936));
                    wall.Add(new Vector2(9507, 6936));
                    wall.Add(new Vector2(9507, 6986));
                    wall.Add(new Vector2(9307, 6986));
                    wall.Add(new Vector2(9307, 7036));
                    wall.Add(new Vector2(9158, 7036));
                    wall.Add(new Vector2(9158, 7085));
                    wall.Add(new Vector2(9157, 7085));
                    wall.Add(new Vector2(9157, 7086));
                    wall.Add(new Vector2(9122, 7086));
                    wall.Add(new Vector2(9122, 7085));
                    wall.Add(new Vector2(9108, 7085));
                    wall.Add(new Vector2(9108, 7135));
                    wall.Add(new Vector2(9058, 7135));
                    wall.Add(new Vector2(9058, 7185));
                    wall.Add(new Vector2(8958, 7185));
                    wall.Add(new Vector2(8958, 7235));
                    wall.Add(new Vector2(8908, 7235));
                    wall.Add(new Vector2(8908, 7285));
                    wall.Add(new Vector2(8858, 7285));
                    wall.Add(new Vector2(8858, 7335));
                    wall.Add(new Vector2(8837, 7335));
                    wall.Add(new Vector2(8837, 7337));
                    wall.Add(new Vector2(8808, 7337));
                    wall.Add(new Vector2(8808, 7385));
                    wall.Add(new Vector2(8807, 7385));
                    wall.Add(new Vector2(8807, 7387));
                    wall.Add(new Vector2(8758, 7387));
                    wall.Add(new Vector2(8758, 7435));
                    wall.Add(new Vector2(8757, 7435));
                    wall.Add(new Vector2(8757, 7437));
                    wall.Add(new Vector2(8657, 7437));
                    wall.Add(new Vector2(8657, 7487));
                    wall.Add(new Vector2(8452, 7487));
                    wall.Add(new Vector2(8452, 7484));
                    wall.Add(new Vector2(8450, 7484));
                    wall.Add(new Vector2(8450, 7437));
                    wall.Add(new Vector2(8400, 7437));
                    wall.Add(new Vector2(8400, 7387));
                    wall.Add(new Vector2(8350, 7387));
                    wall.Add(new Vector2(8350, 7287));
                    wall.Add(new Vector2(8300, 7287));
                    wall.Add(new Vector2(8300, 7087));
                    wall.Add(new Vector2(8350, 7087));
                    wall.Add(new Vector2(8350, 7037));
                    wall.Add(new Vector2(8360, 7037));
                    wall.Add(new Vector2(8360, 7035));
                    wall.Add(new Vector2(8400, 7035));
                    wall.Add(new Vector2(8400, 6935));
                    wall.Add(new Vector2(8500, 6935));
                    wall.Add(new Vector2(8500, 6885));
                    wall.Add(new Vector2(8550, 6885));
                    wall.Add(new Vector2(8550, 6835));
                    wall.Add(new Vector2(8600, 6835));
                    wall.Add(new Vector2(8600, 6785));
                    wall.Add(new Vector2(8666, 6785));
                    wall.Add(new Vector2(8666, 6784));
                    wall.Add(new Vector2(8700, 6784));
                    wall.Add(new Vector2(8700, 6735));
                    wall.Add(new Vector2(8701, 6735));
                    wall.Add(new Vector2(8701, 6734));
                    wall.Add(new Vector2(8800, 6734));
                    wall.Add(new Vector2(8800, 6685));
                    wall.Add(new Vector2(8801, 6685));
                    wall.Add(new Vector2(8801, 6684));
                    wall.Add(new Vector2(8850, 6684));
                    wall.Add(new Vector2(8850, 6675));
                    wall.Add(new Vector2(8851, 6675));
                    wall.Add(new Vector2(8851, 6634));
                    wall.Add(new Vector2(8951, 6634));
                    wall.Add(new Vector2(8951, 6584));
                    wall.Add(new Vector2(9049, 6584));
                    wall.Add(new Vector2(9049, 6536));
                    wall.Add(new Vector2(9051, 6536));
                    wall.Add(new Vector2(9051, 6534));
                    wall.Add(new Vector2(9149, 6534));
                    wall.Add(new Vector2(9149, 6486));
                    wall.Add(new Vector2(9151, 6486));
                    wall.Add(new Vector2(9151, 6484));
                    wall.Add(new Vector2(9171, 6484));
                    wall.Add(new Vector2(9171, 6486));
                    wall.Add(new Vector2(9199, 6486));
                    wall.Add(new Vector2(9199, 6436));
                    wall.Add(new Vector2(9223, 6436));
                    wall.Add(new Vector2(9223, 6434));
                    wall.Add(new Vector2(9299, 6434));
                    wall.Add(new Vector2(9299, 6386));
                    wall.Add(new Vector2(9303, 6386));
                    wall.Add(new Vector2(9303, 6384));
                    wall.Add(new Vector2(9658, 6384));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the blue mid wall 1.
            /// </summary>
            /// <value>The blue mid wall 1.</value>
            public static Geometry.Polygon BlueMid_Wall_1
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(6506, 7283));
                    wall.Add(new Vector2(6606, 7283));
                    wall.Add(new Vector2(6606, 7583));
                    wall.Add(new Vector2(6556, 7583));
                    wall.Add(new Vector2(6556, 7633));
                    wall.Add(new Vector2(6506, 7633));
                    wall.Add(new Vector2(6506, 7713));
                    wall.Add(new Vector2(6505, 7713));
                    wall.Add(new Vector2(6505, 7733));
                    wall.Add(new Vector2(6455, 7733));
                    wall.Add(new Vector2(6455, 7833));
                    wall.Add(new Vector2(6405, 7833));
                    wall.Add(new Vector2(6405, 7928));
                    wall.Add(new Vector2(6404, 7928));
                    wall.Add(new Vector2(6404, 7932));
                    wall.Add(new Vector2(6357, 7932));
                    wall.Add(new Vector2(6357, 8033));
                    wall.Add(new Vector2(6310, 8033));
                    wall.Add(new Vector2(6310, 8037));
                    wall.Add(new Vector2(6257, 8037));
                    wall.Add(new Vector2(6257, 8083));
                    wall.Add(new Vector2(6255, 8083));
                    wall.Add(new Vector2(6255, 8087));
                    wall.Add(new Vector2(6157, 8087));
                    wall.Add(new Vector2(6157, 8133));
                    wall.Add(new Vector2(6155, 8133));
                    wall.Add(new Vector2(6155, 8137));
                    wall.Add(new Vector2(6107, 8137));
                    wall.Add(new Vector2(6107, 8183));
                    wall.Add(new Vector2(6078, 8183));
                    wall.Add(new Vector2(6078, 8185));
                    wall.Add(new Vector2(6008, 8185));
                    wall.Add(new Vector2(6008, 8235));
                    wall.Add(new Vector2(5958, 8235));
                    wall.Add(new Vector2(5958, 8285));
                    wall.Add(new Vector2(5889, 8285));
                    wall.Add(new Vector2(5889, 8287));
                    wall.Add(new Vector2(5858, 8287));
                    wall.Add(new Vector2(5858, 8335));
                    wall.Add(new Vector2(5854, 8335));
                    wall.Add(new Vector2(5854, 8337));
                    wall.Add(new Vector2(5808, 8337));
                    wall.Add(new Vector2(5808, 8385));
                    wall.Add(new Vector2(5804, 8385));
                    wall.Add(new Vector2(5804, 8387));
                    wall.Add(new Vector2(5708, 8387));
                    wall.Add(new Vector2(5708, 8435));
                    wall.Add(new Vector2(5704, 8435));
                    wall.Add(new Vector2(5704, 8437));
                    wall.Add(new Vector2(5608, 8437));
                    wall.Add(new Vector2(5608, 8485));
                    wall.Add(new Vector2(5604, 8485));
                    wall.Add(new Vector2(5604, 8487));
                    wall.Add(new Vector2(5505, 8487));
                    wall.Add(new Vector2(5505, 8537));
                    wall.Add(new Vector2(5300, 8537));
                    wall.Add(new Vector2(5300, 8533));
                    wall.Add(new Vector2(5299, 8533));
                    wall.Add(new Vector2(5299, 8487));
                    wall.Add(new Vector2(5150, 8487));
                    wall.Add(new Vector2(5150, 8483));
                    wall.Add(new Vector2(5149, 8483));
                    wall.Add(new Vector2(5149, 8437));
                    wall.Add(new Vector2(5050, 8437));
                    wall.Add(new Vector2(5050, 8433));
                    wall.Add(new Vector2(5049, 8433));
                    wall.Add(new Vector2(5049, 8387));
                    wall.Add(new Vector2(5000, 8387));
                    wall.Add(new Vector2(5000, 8383));
                    wall.Add(new Vector2(4999, 8383));
                    wall.Add(new Vector2(4999, 8337));
                    wall.Add(new Vector2(4960, 8337));
                    wall.Add(new Vector2(4960, 8335));
                    wall.Add(new Vector2(4950, 8335));
                    wall.Add(new Vector2(4950, 8333));
                    wall.Add(new Vector2(4949, 8333));
                    wall.Add(new Vector2(4949, 8285));
                    wall.Add(new Vector2(4900, 8285));
                    wall.Add(new Vector2(4900, 8235));
                    wall.Add(new Vector2(4850, 8235));
                    wall.Add(new Vector2(4850, 8035));
                    wall.Add(new Vector2(4900, 8035));
                    wall.Add(new Vector2(4900, 7985));
                    wall.Add(new Vector2(5019, 7985));
                    wall.Add(new Vector2(5019, 7984));
                    wall.Add(new Vector2(5299, 7984));
                    wall.Add(new Vector2(5299, 7934));
                    wall.Add(new Vector2(5464, 7934));
                    wall.Add(new Vector2(5464, 7933));
                    wall.Add(new Vector2(5499, 7933));
                    wall.Add(new Vector2(5499, 7883));
                    wall.Add(new Vector2(5649, 7883));
                    wall.Add(new Vector2(5649, 7833));
                    wall.Add(new Vector2(5749, 7833));
                    wall.Add(new Vector2(5749, 7823));
                    wall.Add(new Vector2(5751, 7823));
                    wall.Add(new Vector2(5751, 7784));
                    wall.Add(new Vector2(5800, 7784));
                    wall.Add(new Vector2(5800, 7737));
                    wall.Add(new Vector2(5801, 7737));
                    wall.Add(new Vector2(5801, 7734));
                    wall.Add(new Vector2(5850, 7734));
                    wall.Add(new Vector2(5850, 7687));
                    wall.Add(new Vector2(5851, 7687));
                    wall.Add(new Vector2(5851, 7684));
                    wall.Add(new Vector2(5950, 7684));
                    wall.Add(new Vector2(5950, 7642));
                    wall.Add(new Vector2(5951, 7642));
                    wall.Add(new Vector2(5951, 7634));
                    wall.Add(new Vector2(5970, 7634));
                    wall.Add(new Vector2(5970, 7633));
                    wall.Add(new Vector2(6000, 7633));
                    wall.Add(new Vector2(6000, 7533));
                    wall.Add(new Vector2(6050, 7533));
                    wall.Add(new Vector2(6050, 7483));
                    wall.Add(new Vector2(6100, 7483));
                    wall.Add(new Vector2(6100, 7433));
                    wall.Add(new Vector2(6150, 7433));
                    wall.Add(new Vector2(6150, 7388));
                    wall.Add(new Vector2(6152, 7388));
                    wall.Add(new Vector2(6152, 7336));
                    wall.Add(new Vector2(6184, 7336));
                    wall.Add(new Vector2(6184, 7333));
                    wall.Add(new Vector2(6199, 7333));
                    wall.Add(new Vector2(6199, 7283));
                    wall.Add(new Vector2(6249, 7283));
                    wall.Add(new Vector2(6249, 7233));
                    wall.Add(new Vector2(6506, 7233));

                    return wall;
                }
            }

            /// <summary>
            /// Gets the blue mid wall 2.
            /// </summary>
            /// <value>The blue mid wall 2.</value>
            public static Geometry.Polygon BlueMid_Wall_2
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(8506, 5784));
                    wall.Add(new Vector2(8508, 5784));
                    wall.Add(new Vector2(8508, 5884));
                    wall.Add(new Vector2(8458, 5884));
                    wall.Add(new Vector2(8458, 5934));
                    wall.Add(new Vector2(8358, 5934));
                    wall.Add(new Vector2(8358, 5984));
                    wall.Add(new Vector2(8333, 5984));
                    wall.Add(new Vector2(8333, 5985));
                    wall.Add(new Vector2(8308, 5985));
                    wall.Add(new Vector2(8308, 5986));
                    wall.Add(new Vector2(8258, 5986));
                    wall.Add(new Vector2(8258, 6036));
                    wall.Add(new Vector2(8208, 6036));
                    wall.Add(new Vector2(8208, 6086));
                    wall.Add(new Vector2(8108, 6086));
                    wall.Add(new Vector2(8108, 6136));
                    wall.Add(new Vector2(8008, 6136));
                    wall.Add(new Vector2(8008, 6186));
                    wall.Add(new Vector2(7958, 6186));
                    wall.Add(new Vector2(7958, 6236));
                    wall.Add(new Vector2(7908, 6236));
                    wall.Add(new Vector2(7908, 6281));
                    wall.Add(new Vector2(7905, 6281));
                    wall.Add(new Vector2(7905, 6286));
                    wall.Add(new Vector2(7860, 6286));
                    wall.Add(new Vector2(7860, 6287));
                    wall.Add(new Vector2(7805, 6287));
                    wall.Add(new Vector2(7805, 6337));
                    wall.Add(new Vector2(7755, 6337));
                    wall.Add(new Vector2(7755, 6387));
                    wall.Add(new Vector2(7705, 6387));
                    wall.Add(new Vector2(7705, 6437));
                    wall.Add(new Vector2(7655, 6437));
                    wall.Add(new Vector2(7655, 6537));
                    wall.Add(new Vector2(7606, 6537));
                    wall.Add(new Vector2(7606, 6586));
                    wall.Add(new Vector2(7556, 6586));
                    wall.Add(new Vector2(7556, 6636));
                    wall.Add(new Vector2(7506, 6636));
                    wall.Add(new Vector2(7506, 6686));
                    wall.Add(new Vector2(7351, 6686));
                    wall.Add(new Vector2(7351, 6683));
                    wall.Add(new Vector2(7349, 6683));
                    wall.Add(new Vector2(7349, 6636));
                    wall.Add(new Vector2(7301, 6636));
                    wall.Add(new Vector2(7301, 6432));
                    wall.Add(new Vector2(7349, 6432));
                    wall.Add(new Vector2(7349, 6383));
                    wall.Add(new Vector2(7351, 6383));
                    wall.Add(new Vector2(7351, 6382));
                    wall.Add(new Vector2(7399, 6382));
                    wall.Add(new Vector2(7399, 6333));
                    wall.Add(new Vector2(7449, 6333));
                    wall.Add(new Vector2(7449, 6283));
                    wall.Add(new Vector2(7499, 6283));
                    wall.Add(new Vector2(7499, 6233));
                    wall.Add(new Vector2(7550, 6233));
                    wall.Add(new Vector2(7550, 6185));
                    wall.Add(new Vector2(7650, 6185));
                    wall.Add(new Vector2(7650, 6135));
                    wall.Add(new Vector2(7700, 6135));
                    wall.Add(new Vector2(7700, 6085));
                    wall.Add(new Vector2(7749, 6085));
                    wall.Add(new Vector2(7749, 6083));
                    wall.Add(new Vector2(7799, 6083));
                    wall.Add(new Vector2(7799, 6033));
                    wall.Add(new Vector2(7899, 6033));
                    wall.Add(new Vector2(7899, 5983));
                    wall.Add(new Vector2(7949, 5983));
                    wall.Add(new Vector2(7949, 5933));
                    wall.Add(new Vector2(8049, 5933));
                    wall.Add(new Vector2(8049, 5883));
                    wall.Add(new Vector2(8199, 5883));
                    wall.Add(new Vector2(8199, 5833));
                    wall.Add(new Vector2(8351, 5833));
                    wall.Add(new Vector2(8351, 5783));
                    wall.Add(new Vector2(8506, 5783));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle wall 1.
            /// </summary>
            /// <value>The bottom blue jungle wall 1.</value>
            public static Geometry.Polygon BottomBlueJungle_Wall_1
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(11408, 1982));
                    wall.Add(new Vector2(11508, 1982));
                    wall.Add(new Vector2(11508, 2032));
                    wall.Add(new Vector2(11558, 2032));
                    wall.Add(new Vector2(11558, 2132));
                    wall.Add(new Vector2(11608, 2132));
                    wall.Add(new Vector2(11608, 2232));
                    wall.Add(new Vector2(11658, 2232));
                    wall.Add(new Vector2(11658, 2307));
                    wall.Add(new Vector2(11657, 2307));
                    wall.Add(new Vector2(11657, 2386));
                    wall.Add(new Vector2(11707, 2386));
                    wall.Add(new Vector2(11707, 2486));
                    wall.Add(new Vector2(11757, 2486));
                    wall.Add(new Vector2(11757, 2519));
                    wall.Add(new Vector2(11758, 2519));
                    wall.Add(new Vector2(11758, 2784));
                    wall.Add(new Vector2(11708, 2784));
                    wall.Add(new Vector2(11708, 2834));
                    wall.Add(new Vector2(11658, 2834));
                    wall.Add(new Vector2(11658, 2884));
                    wall.Add(new Vector2(11608, 2884));
                    wall.Add(new Vector2(11608, 2934));
                    wall.Add(new Vector2(11558, 2934));
                    wall.Add(new Vector2(11558, 2984));
                    wall.Add(new Vector2(11546, 2984));
                    wall.Add(new Vector2(11546, 2986));
                    wall.Add(new Vector2(11508, 2986));
                    wall.Add(new Vector2(11508, 2989));
                    wall.Add(new Vector2(11506, 2989));
                    wall.Add(new Vector2(11506, 3036));
                    wall.Add(new Vector2(11438, 3036));
                    wall.Add(new Vector2(11438, 3037));
                    wall.Add(new Vector2(11408, 3037));
                    wall.Add(new Vector2(11408, 3087));
                    wall.Add(new Vector2(11358, 3087));
                    wall.Add(new Vector2(11358, 3137));
                    wall.Add(new Vector2(11308, 3137));
                    wall.Add(new Vector2(11308, 3187));
                    wall.Add(new Vector2(11208, 3187));
                    wall.Add(new Vector2(11208, 3237));
                    wall.Add(new Vector2(11158, 3237));
                    wall.Add(new Vector2(11158, 3287));
                    wall.Add(new Vector2(10963, 3287));
                    wall.Add(new Vector2(10963, 3284));
                    wall.Add(new Vector2(10946, 3284));
                    wall.Add(new Vector2(10946, 3285));
                    wall.Add(new Vector2(10851, 3285));
                    wall.Add(new Vector2(10851, 3235));
                    wall.Add(new Vector2(10751, 3235));
                    wall.Add(new Vector2(10751, 3185));
                    wall.Add(new Vector2(10701, 3185));
                    wall.Add(new Vector2(10701, 3135));
                    wall.Add(new Vector2(10651, 3135));
                    wall.Add(new Vector2(10651, 2935));
                    wall.Add(new Vector2(10701, 2935));
                    wall.Add(new Vector2(10701, 2885));
                    wall.Add(new Vector2(10750, 2885));
                    wall.Add(new Vector2(10750, 2786));
                    wall.Add(new Vector2(10753, 2786));
                    wall.Add(new Vector2(10753, 2784));
                    wall.Add(new Vector2(10800, 2784));
                    wall.Add(new Vector2(10800, 2736));
                    wall.Add(new Vector2(10803, 2736));
                    wall.Add(new Vector2(10803, 2734));
                    wall.Add(new Vector2(10850, 2734));
                    wall.Add(new Vector2(10850, 2686));
                    wall.Add(new Vector2(10853, 2686));
                    wall.Add(new Vector2(10853, 2684));
                    wall.Add(new Vector2(10900, 2684));
                    wall.Add(new Vector2(10900, 2586));
                    wall.Add(new Vector2(10903, 2586));
                    wall.Add(new Vector2(10903, 2584));
                    wall.Add(new Vector2(10950, 2584));
                    wall.Add(new Vector2(10950, 2536));
                    wall.Add(new Vector2(10952, 2536));
                    wall.Add(new Vector2(10952, 2534));
                    wall.Add(new Vector2(10967, 2534));
                    wall.Add(new Vector2(10967, 2533));
                    wall.Add(new Vector2(11000, 2533));
                    wall.Add(new Vector2(11000, 2481));
                    wall.Add(new Vector2(11002, 2481));
                    wall.Add(new Vector2(11002, 2433));
                    wall.Add(new Vector2(11052, 2433));
                    wall.Add(new Vector2(11052, 2307));
                    wall.Add(new Vector2(11050, 2307));
                    wall.Add(new Vector2(11050, 2182));
                    wall.Add(new Vector2(11100, 2182));
                    wall.Add(new Vector2(11100, 2082));
                    wall.Add(new Vector2(11150, 2082));
                    wall.Add(new Vector2(11150, 1982));
                    wall.Add(new Vector2(11199, 1982));
                    wall.Add(new Vector2(11199, 1937));
                    wall.Add(new Vector2(11200, 1937));
                    wall.Add(new Vector2(11200, 1932));
                    wall.Add(new Vector2(11408, 1932));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle wall 2.
            /// </summary>
            /// <value>The bottom blue jungle wall 2.</value>
            public static Geometry.Polygon BottomBlueJungle_Wall_2
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(10554, 1786));
                    wall.Add(new Vector2(10557, 1786));
                    wall.Add(new Vector2(10557, 1834));
                    wall.Add(new Vector2(10654, 1834));
                    wall.Add(new Vector2(10654, 1836));
                    wall.Add(new Vector2(10657, 1836));
                    wall.Add(new Vector2(10657, 1884));
                    wall.Add(new Vector2(10704, 1884));
                    wall.Add(new Vector2(10704, 1886));
                    wall.Add(new Vector2(10707, 1886));
                    wall.Add(new Vector2(10707, 2086));
                    wall.Add(new Vector2(10657, 2086));
                    wall.Add(new Vector2(10657, 2136));
                    wall.Add(new Vector2(10608, 2136));
                    wall.Add(new Vector2(10608, 2185));
                    wall.Add(new Vector2(10604, 2185));
                    wall.Add(new Vector2(10604, 2186));
                    wall.Add(new Vector2(10208, 2186));
                    wall.Add(new Vector2(10208, 2235));
                    wall.Add(new Vector2(10204, 2235));
                    wall.Add(new Vector2(10204, 2236));
                    wall.Add(new Vector2(10139, 2236));
                    wall.Add(new Vector2(10139, 2235));
                    wall.Add(new Vector2(10118, 2235));
                    wall.Add(new Vector2(10118, 2234));
                    wall.Add(new Vector2(10107, 2234));
                    wall.Add(new Vector2(10107, 2283));
                    wall.Add(new Vector2(10106, 2283));
                    wall.Add(new Vector2(10106, 2284));
                    wall.Add(new Vector2(10007, 2284));
                    wall.Add(new Vector2(10007, 2333));
                    wall.Add(new Vector2(10006, 2333));
                    wall.Add(new Vector2(10006, 2334));
                    wall.Add(new Vector2(9911, 2334));
                    wall.Add(new Vector2(9911, 2336));
                    wall.Add(new Vector2(9907, 2336));
                    wall.Add(new Vector2(9907, 2383));
                    wall.Add(new Vector2(9906, 2383));
                    wall.Add(new Vector2(9906, 2386));
                    wall.Add(new Vector2(9416, 2386));
                    wall.Add(new Vector2(9416, 2385));
                    wall.Add(new Vector2(9402, 2385));
                    wall.Add(new Vector2(9402, 2284));
                    wall.Add(new Vector2(9451, 2284));
                    wall.Add(new Vector2(9451, 2186));
                    wall.Add(new Vector2(9452, 2186));
                    wall.Add(new Vector2(9452, 2184));
                    wall.Add(new Vector2(9501, 2184));
                    wall.Add(new Vector2(9501, 1985));
                    wall.Add(new Vector2(9551, 1985));
                    wall.Add(new Vector2(9551, 1835));
                    wall.Add(new Vector2(9601, 1835));
                    wall.Add(new Vector2(9601, 1785));
                    wall.Add(new Vector2(9806, 1785));
                    wall.Add(new Vector2(9806, 1786));
                    wall.Add(new Vector2(10054, 1786));
                    wall.Add(new Vector2(10054, 1784));
                    wall.Add(new Vector2(10554, 1784));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle wall 3.
            /// </summary>
            /// <value>The bottom blue jungle wall 3.</value>
            public static Geometry.Polygon BottomBlueJungle_Wall_3
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(9156, 1833));
                    wall.Add(new Vector2(9154, 1833));
                    wall.Add(new Vector2(9154, 1835));
                    wall.Add(new Vector2(9120, 1835));
                    wall.Add(new Vector2(9120, 1837));
                    wall.Add(new Vector2(9106, 1837));
                    wall.Add(new Vector2(9106, 1933));
                    wall.Add(new Vector2(9105, 1933));
                    wall.Add(new Vector2(9105, 1937));
                    wall.Add(new Vector2(9056, 1937));
                    wall.Add(new Vector2(9056, 1983));
                    wall.Add(new Vector2(9055, 1983));
                    wall.Add(new Vector2(9055, 1987));
                    wall.Add(new Vector2(9006, 1987));
                    wall.Add(new Vector2(9006, 2083));
                    wall.Add(new Vector2(9005, 2083));
                    wall.Add(new Vector2(9005, 2087));
                    wall.Add(new Vector2(8956, 2087));
                    wall.Add(new Vector2(8956, 2103));
                    wall.Add(new Vector2(8955, 2103));
                    wall.Add(new Vector2(8955, 2187));
                    wall.Add(new Vector2(8800, 2187));
                    wall.Add(new Vector2(8800, 2185));
                    wall.Add(new Vector2(8799, 2185));
                    wall.Add(new Vector2(8799, 2137));
                    wall.Add(new Vector2(8600, 2137));
                    wall.Add(new Vector2(8600, 2087));
                    wall.Add(new Vector2(8403, 2087));
                    wall.Add(new Vector2(8403, 2086));
                    wall.Add(new Vector2(8402, 2086));
                    wall.Add(new Vector2(8402, 2037));
                    wall.Add(new Vector2(8203, 2037));
                    wall.Add(new Vector2(8203, 2036));
                    wall.Add(new Vector2(8202, 2036));
                    wall.Add(new Vector2(8202, 1736));
                    wall.Add(new Vector2(8626, 1736));
                    wall.Add(new Vector2(8626, 1733));
                    wall.Add(new Vector2(9156, 1733));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle wall 4.
            /// </summary>
            /// <value>The bottom blue jungle wall 4.</value>
            public static Geometry.Polygon BottomBlueJungle_Wall_4
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(7656, 1735));
                    wall.Add(new Vector2(7907, 1735));
                    wall.Add(new Vector2(7907, 2035));
                    wall.Add(new Vector2(7905, 2035));
                    wall.Add(new Vector2(7905, 2036));
                    wall.Add(new Vector2(7707, 2036));
                    wall.Add(new Vector2(7707, 2070));
                    wall.Add(new Vector2(7706, 2070));
                    wall.Add(new Vector2(7706, 2084));
                    wall.Add(new Vector2(7705, 2084));
                    wall.Add(new Vector2(7705, 2086));
                    wall.Add(new Vector2(7606, 2086));
                    wall.Add(new Vector2(7606, 2134));
                    wall.Add(new Vector2(7605, 2134));
                    wall.Add(new Vector2(7605, 2136));
                    wall.Add(new Vector2(7557, 2136));
                    wall.Add(new Vector2(7557, 2184));
                    wall.Add(new Vector2(7507, 2184));
                    wall.Add(new Vector2(7507, 2234));
                    wall.Add(new Vector2(7457, 2234));
                    wall.Add(new Vector2(7457, 2284));
                    wall.Add(new Vector2(7407, 2284));
                    wall.Add(new Vector2(7407, 2336));
                    wall.Add(new Vector2(7357, 2336));
                    wall.Add(new Vector2(7357, 2384));
                    wall.Add(new Vector2(7307, 2384));
                    wall.Add(new Vector2(7307, 2434));
                    wall.Add(new Vector2(7257, 2434));
                    wall.Add(new Vector2(7257, 2484));
                    wall.Add(new Vector2(7207, 2484));
                    wall.Add(new Vector2(7207, 2534));
                    wall.Add(new Vector2(7194, 2534));
                    wall.Add(new Vector2(7194, 2535));
                    wall.Add(new Vector2(7110, 2535));
                    wall.Add(new Vector2(7110, 2537));
                    wall.Add(new Vector2(6750, 2537));
                    wall.Add(new Vector2(6750, 2535));
                    wall.Add(new Vector2(6749, 2535));
                    wall.Add(new Vector2(6749, 2487));
                    wall.Add(new Vector2(6615, 2487));
                    wall.Add(new Vector2(6615, 2483));
                    wall.Add(new Vector2(6603, 2483));
                    wall.Add(new Vector2(6603, 2471));
                    wall.Add(new Vector2(6600, 2471));
                    wall.Add(new Vector2(6600, 2436));
                    wall.Add(new Vector2(6520, 2436));
                    wall.Add(new Vector2(6520, 2433));
                    wall.Add(new Vector2(6503, 2433));
                    wall.Add(new Vector2(6503, 2383));
                    wall.Add(new Vector2(6403, 2383));
                    wall.Add(new Vector2(6403, 2333));
                    wall.Add(new Vector2(6318, 2333));
                    wall.Add(new Vector2(6318, 2233));
                    wall.Add(new Vector2(6301, 2233));
                    wall.Add(new Vector2(6301, 1983));
                    wall.Add(new Vector2(6351, 1983));
                    wall.Add(new Vector2(6351, 1933));
                    wall.Add(new Vector2(6401, 1933));
                    wall.Add(new Vector2(6401, 1833));
                    wall.Add(new Vector2(6451, 1833));
                    wall.Add(new Vector2(6451, 1743));
                    wall.Add(new Vector2(6452, 1743));
                    wall.Add(new Vector2(6452, 1736));
                    wall.Add(new Vector2(6725, 1736));
                    wall.Add(new Vector2(6725, 1734));
                    wall.Add(new Vector2(7161, 1734));
                    wall.Add(new Vector2(7161, 1733));
                    wall.Add(new Vector2(7656, 1733));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle wall 5.
            /// </summary>
            /// <value>The bottom blue jungle wall 5.</value>
            public static Geometry.Polygon BottomBlueJungle_Wall_5
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(5958, 1883));
                    wall.Add(new Vector2(5956, 1883));
                    wall.Add(new Vector2(5956, 1885));
                    wall.Add(new Vector2(5954, 1885));
                    wall.Add(new Vector2(5954, 1887));
                    wall.Add(new Vector2(5908, 1887));
                    wall.Add(new Vector2(5908, 1983));
                    wall.Add(new Vector2(5907, 1983));
                    wall.Add(new Vector2(5907, 1985));
                    wall.Add(new Vector2(5904, 1985));
                    wall.Add(new Vector2(5904, 1987));
                    wall.Add(new Vector2(5858, 1987));
                    wall.Add(new Vector2(5858, 1998));
                    wall.Add(new Vector2(5857, 1998));
                    wall.Add(new Vector2(5857, 2535));
                    wall.Add(new Vector2(5907, 2535));
                    wall.Add(new Vector2(5907, 2585));
                    wall.Add(new Vector2(5957, 2585));
                    wall.Add(new Vector2(5957, 2635));
                    wall.Add(new Vector2(6057, 2635));
                    wall.Add(new Vector2(6057, 2663));
                    wall.Add(new Vector2(6058, 2663));
                    wall.Add(new Vector2(6058, 2683));
                    wall.Add(new Vector2(6158, 2683));
                    wall.Add(new Vector2(6158, 2733));
                    wall.Add(new Vector2(6258, 2733));
                    wall.Add(new Vector2(6258, 2883));
                    wall.Add(new Vector2(6256, 2883));
                    wall.Add(new Vector2(6256, 2885));
                    wall.Add(new Vector2(6208, 2885));
                    wall.Add(new Vector2(6208, 2983));
                    wall.Add(new Vector2(6158, 2983));
                    wall.Add(new Vector2(6158, 3033));
                    wall.Add(new Vector2(6133, 3033));
                    wall.Add(new Vector2(6133, 3036));
                    wall.Add(new Vector2(6108, 3036));
                    wall.Add(new Vector2(6108, 3043));
                    wall.Add(new Vector2(6105, 3043));
                    wall.Add(new Vector2(6105, 3133));
                    wall.Add(new Vector2(6103, 3133));
                    wall.Add(new Vector2(6103, 3136));
                    wall.Add(new Vector2(6055, 3136));
                    wall.Add(new Vector2(6055, 3236));
                    wall.Add(new Vector2(6005, 3236));
                    wall.Add(new Vector2(6005, 3286));
                    wall.Add(new Vector2(5956, 3286));
                    wall.Add(new Vector2(5956, 3384));
                    wall.Add(new Vector2(5955, 3384));
                    wall.Add(new Vector2(5955, 3386));
                    wall.Add(new Vector2(5906, 3386));
                    wall.Add(new Vector2(5906, 3409));
                    wall.Add(new Vector2(5905, 3409));
                    wall.Add(new Vector2(5905, 3486));
                    wall.Add(new Vector2(5811, 3486));
                    wall.Add(new Vector2(5811, 3487));
                    wall.Add(new Vector2(5551, 3487));
                    wall.Add(new Vector2(5551, 3362));
                    wall.Add(new Vector2(5549, 3362));
                    wall.Add(new Vector2(5549, 3233));
                    wall.Add(new Vector2(5452, 3233));
                    wall.Add(new Vector2(5452, 3232));
                    wall.Add(new Vector2(5449, 3232));
                    wall.Add(new Vector2(5449, 2982));
                    wall.Add(new Vector2(5450, 2982));
                    wall.Add(new Vector2(5450, 2629));
                    wall.Add(new Vector2(5452, 2629));
                    wall.Add(new Vector2(5452, 2369));
                    wall.Add(new Vector2(5450, 2369));
                    wall.Add(new Vector2(5450, 1989));
                    wall.Add(new Vector2(5451, 1989));
                    wall.Add(new Vector2(5451, 1786));
                    wall.Add(new Vector2(5606, 1786));
                    wall.Add(new Vector2(5606, 1785));
                    wall.Add(new Vector2(5723, 1785));
                    wall.Add(new Vector2(5723, 1783));
                    wall.Add(new Vector2(5958, 1783));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle wall 6.
            /// </summary>
            /// <value>The bottom blue jungle wall 6.</value>
            public static Geometry.Polygon BottomBlueJungle_Wall_6
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(5658, 3936));
                    wall.Add(new Vector2(5707, 3936));
                    wall.Add(new Vector2(5707, 4086));
                    wall.Add(new Vector2(5757, 4086));
                    wall.Add(new Vector2(5757, 4233));
                    wall.Add(new Vector2(5804, 4233));
                    wall.Add(new Vector2(5804, 4236));
                    wall.Add(new Vector2(5807, 4236));
                    wall.Add(new Vector2(5807, 4286));
                    wall.Add(new Vector2(5805, 4286));
                    wall.Add(new Vector2(5805, 4333));
                    wall.Add(new Vector2(5854, 4333));
                    wall.Add(new Vector2(5854, 4337));
                    wall.Add(new Vector2(5855, 4337));
                    wall.Add(new Vector2(5855, 4402));
                    wall.Add(new Vector2(5854, 4402));
                    wall.Add(new Vector2(5854, 4432));
                    wall.Add(new Vector2(5856, 4432));
                    wall.Add(new Vector2(5856, 4433));
                    wall.Add(new Vector2(5904, 4433));
                    wall.Add(new Vector2(5904, 4437));
                    wall.Add(new Vector2(5906, 4437));
                    wall.Add(new Vector2(5906, 4533));
                    wall.Add(new Vector2(5954, 4533));
                    wall.Add(new Vector2(5954, 4537));
                    wall.Add(new Vector2(5956, 4537));
                    wall.Add(new Vector2(5956, 4633));
                    wall.Add(new Vector2(6004, 4633));
                    wall.Add(new Vector2(6004, 4637));
                    wall.Add(new Vector2(6006, 4637));
                    wall.Add(new Vector2(6006, 4683));
                    wall.Add(new Vector2(6054, 4683));
                    wall.Add(new Vector2(6054, 4687));
                    wall.Add(new Vector2(6056, 4687));
                    wall.Add(new Vector2(6056, 4737));
                    wall.Add(new Vector2(6106, 4737));
                    wall.Add(new Vector2(6106, 4887));
                    wall.Add(new Vector2(5906, 4887));
                    wall.Add(new Vector2(5906, 4937));
                    wall.Add(new Vector2(5676, 4937));
                    wall.Add(new Vector2(5676, 4932));
                    wall.Add(new Vector2(5549, 4932));
                    wall.Add(new Vector2(5549, 4884));
                    wall.Add(new Vector2(5451, 4884));
                    wall.Add(new Vector2(5451, 4882));
                    wall.Add(new Vector2(5449, 4882));
                    wall.Add(new Vector2(5449, 4834));
                    wall.Add(new Vector2(5351, 4834));
                    wall.Add(new Vector2(5351, 4784));
                    wall.Add(new Vector2(5301, 4784));
                    wall.Add(new Vector2(5301, 4734));
                    wall.Add(new Vector2(5251, 4734));
                    wall.Add(new Vector2(5251, 4719));
                    wall.Add(new Vector2(5250, 4719));
                    wall.Add(new Vector2(5250, 4687));
                    wall.Add(new Vector2(5202, 4687));
                    wall.Add(new Vector2(5202, 4684));
                    wall.Add(new Vector2(5200, 4684));
                    wall.Add(new Vector2(5200, 4637));
                    wall.Add(new Vector2(5152, 4637));
                    wall.Add(new Vector2(5152, 4635));
                    wall.Add(new Vector2(5148, 4635));
                    wall.Add(new Vector2(5148, 4587));
                    wall.Add(new Vector2(5142, 4587));
                    wall.Add(new Vector2(5142, 4585));
                    wall.Add(new Vector2(5098, 4585));
                    wall.Add(new Vector2(5098, 4535));
                    wall.Add(new Vector2(5088, 4535));
                    wall.Add(new Vector2(5088, 4533));
                    wall.Add(new Vector2(5049, 4533));
                    wall.Add(new Vector2(5049, 4484));
                    wall.Add(new Vector2(5003, 4484));
                    wall.Add(new Vector2(5003, 4483));
                    wall.Add(new Vector2(4999, 4483));
                    wall.Add(new Vector2(4999, 4434));
                    wall.Add(new Vector2(4953, 4434));
                    wall.Add(new Vector2(4953, 4384));
                    wall.Add(new Vector2(4903, 4384));
                    wall.Add(new Vector2(4903, 4284));
                    wall.Add(new Vector2(4853, 4284));
                    wall.Add(new Vector2(4853, 4184));
                    wall.Add(new Vector2(4903, 4184));
                    wall.Add(new Vector2(4903, 4134));
                    wall.Add(new Vector2(5049, 4134));
                    wall.Add(new Vector2(5049, 4037));
                    wall.Add(new Vector2(5053, 4037));
                    wall.Add(new Vector2(5053, 4034));
                    wall.Add(new Vector2(5099, 4034));
                    wall.Add(new Vector2(5099, 3937));
                    wall.Add(new Vector2(5148, 3937));
                    wall.Add(new Vector2(5148, 3936));
                    wall.Add(new Vector2(5149, 3936));
                    wall.Add(new Vector2(5149, 3887));
                    wall.Add(new Vector2(5153, 3887));
                    wall.Add(new Vector2(5153, 3886));
                    wall.Add(new Vector2(5199, 3886));
                    wall.Add(new Vector2(5199, 3787));
                    wall.Add(new Vector2(5203, 3787));
                    wall.Add(new Vector2(5203, 3786));
                    wall.Add(new Vector2(5658, 3786));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle raptor wall.
            /// </summary>
            /// <value>The bottom blue jungle raptor wall.</value>
            public static Geometry.Polygon BottomBlueJungle_RaptorWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(6705, 4937));
                    wall.Add(new Vector2(6755, 4937));
                    wall.Add(new Vector2(6755, 5085));
                    wall.Add(new Vector2(6756, 5085));
                    wall.Add(new Vector2(6756, 5117));
                    wall.Add(new Vector2(6757, 5117));
                    wall.Add(new Vector2(6757, 5232));
                    wall.Add(new Vector2(6807, 5232));
                    wall.Add(new Vector2(6807, 5633));
                    wall.Add(new Vector2(7092, 5633));
                    wall.Add(new Vector2(7092, 5634));
                    wall.Add(new Vector2(7200, 5634));
                    wall.Add(new Vector2(7200, 5592));
                    wall.Add(new Vector2(7199, 5592));
                    wall.Add(new Vector2(7199, 5232));
                    wall.Add(new Vector2(7629, 5232));
                    wall.Add(new Vector2(7629, 5235));
                    wall.Add(new Vector2(7708, 5235));
                    wall.Add(new Vector2(7708, 5585));
                    wall.Add(new Vector2(7707, 5585));
                    wall.Add(new Vector2(7707, 5586));
                    wall.Add(new Vector2(7658, 5586));
                    wall.Add(new Vector2(7658, 5634));
                    wall.Add(new Vector2(7608, 5634));
                    wall.Add(new Vector2(7608, 5684));
                    wall.Add(new Vector2(7586, 5684));
                    wall.Add(new Vector2(7586, 5687));
                    wall.Add(new Vector2(7558, 5687));
                    wall.Add(new Vector2(7558, 5734));
                    wall.Add(new Vector2(7556, 5734));
                    wall.Add(new Vector2(7556, 5737));
                    wall.Add(new Vector2(7458, 5737));
                    wall.Add(new Vector2(7458, 5784));
                    wall.Add(new Vector2(7456, 5784));
                    wall.Add(new Vector2(7456, 5787));
                    wall.Add(new Vector2(7408, 5787));
                    wall.Add(new Vector2(7408, 5804));
                    wall.Add(new Vector2(7406, 5804));
                    wall.Add(new Vector2(7406, 5837));
                    wall.Add(new Vector2(7356, 5837));
                    wall.Add(new Vector2(7356, 5887));
                    wall.Add(new Vector2(7256, 5887));
                    wall.Add(new Vector2(7256, 5937));
                    wall.Add(new Vector2(7206, 5937));
                    wall.Add(new Vector2(7206, 5982));
                    wall.Add(new Vector2(7204, 5982));
                    wall.Add(new Vector2(7204, 5984));
                    wall.Add(new Vector2(7154, 5984));
                    wall.Add(new Vector2(7154, 6034));
                    wall.Add(new Vector2(7104, 6034));
                    wall.Add(new Vector2(7104, 6084));
                    wall.Add(new Vector2(7005, 6084));
                    wall.Add(new Vector2(7005, 6134));
                    wall.Add(new Vector2(6955, 6134));
                    wall.Add(new Vector2(6955, 6184));
                    wall.Add(new Vector2(6650, 6184));
                    wall.Add(new Vector2(6650, 6134));
                    wall.Add(new Vector2(6600, 6134));
                    wall.Add(new Vector2(6600, 6086));
                    wall.Add(new Vector2(6552, 6086));
                    wall.Add(new Vector2(6552, 6084));
                    wall.Add(new Vector2(6550, 6084));
                    wall.Add(new Vector2(6550, 5986));
                    wall.Add(new Vector2(6502, 5986));
                    wall.Add(new Vector2(6502, 5982));
                    wall.Add(new Vector2(6500, 5982));
                    wall.Add(new Vector2(6500, 5936));
                    wall.Add(new Vector2(6452, 5936));
                    wall.Add(new Vector2(6452, 5932));
                    wall.Add(new Vector2(6450, 5932));
                    wall.Add(new Vector2(6450, 5886));
                    wall.Add(new Vector2(6402, 5886));
                    wall.Add(new Vector2(6402, 5841));
                    wall.Add(new Vector2(6399, 5841));
                    wall.Add(new Vector2(6399, 5836));
                    wall.Add(new Vector2(6349, 5836));
                    wall.Add(new Vector2(6349, 5786));
                    wall.Add(new Vector2(6299, 5786));
                    wall.Add(new Vector2(6299, 5736));
                    wall.Add(new Vector2(6249, 5736));
                    wall.Add(new Vector2(6249, 5686));
                    wall.Add(new Vector2(6199, 5686));
                    wall.Add(new Vector2(6199, 5636));
                    wall.Add(new Vector2(6149, 5636));
                    wall.Add(new Vector2(6149, 5586));
                    wall.Add(new Vector2(6099, 5586));
                    wall.Add(new Vector2(6099, 5536));
                    wall.Add(new Vector2(6059, 5536));
                    wall.Add(new Vector2(6059, 5535));
                    wall.Add(new Vector2(6049, 5535));
                    wall.Add(new Vector2(6049, 5485));
                    wall.Add(new Vector2(6009, 5485));
                    wall.Add(new Vector2(6009, 5483));
                    wall.Add(new Vector2(6000, 5483));
                    wall.Add(new Vector2(6000, 5433));
                    wall.Add(new Vector2(5950, 5433));
                    wall.Add(new Vector2(5950, 5383));
                    wall.Add(new Vector2(5900, 5383));
                    wall.Add(new Vector2(5900, 5233));
                    wall.Add(new Vector2(6105, 5233));
                    wall.Add(new Vector2(6105, 5333));
                    wall.Add(new Vector2(6255, 5333));
                    wall.Add(new Vector2(6255, 5335));
                    wall.Add(new Vector2(6327, 5335));
                    wall.Add(new Vector2(6327, 5332));
                    wall.Add(new Vector2(6449, 5332));
                    wall.Add(new Vector2(6449, 5110));
                    wall.Add(new Vector2(6450, 5110));
                    wall.Add(new Vector2(6450, 4887));
                    wall.Add(new Vector2(6550, 4887));
                    wall.Add(new Vector2(6550, 4837));
                    wall.Add(new Vector2(6705, 4837));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle red buff wall.
            /// </summary>
            /// <value>The bottom blue jungle red buff wall.</value>
            public static Geometry.Polygon BottomBlueJungle_RedBuffWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(8206, 3583));
                    wall.Add(new Vector2(8256, 3583));
                    wall.Add(new Vector2(8256, 3633));
                    wall.Add(new Vector2(8356, 3633));
                    wall.Add(new Vector2(8356, 3683));
                    wall.Add(new Vector2(8406, 3683));
                    wall.Add(new Vector2(8406, 3783));
                    wall.Add(new Vector2(8456, 3783));
                    wall.Add(new Vector2(8456, 3833));
                    wall.Add(new Vector2(8506, 3833));
                    wall.Add(new Vector2(8506, 3883));
                    wall.Add(new Vector2(8551, 3883));
                    wall.Add(new Vector2(8551, 3887));
                    wall.Add(new Vector2(8556, 3887));
                    wall.Add(new Vector2(8556, 3937));
                    wall.Add(new Vector2(8606, 3937));
                    wall.Add(new Vector2(8606, 4092));
                    wall.Add(new Vector2(8607, 4092));
                    wall.Add(new Vector2(8607, 4537));
                    wall.Add(new Vector2(8557, 4537));
                    wall.Add(new Vector2(8557, 4662));
                    wall.Add(new Vector2(8555, 4662));
                    wall.Add(new Vector2(8555, 4686));
                    wall.Add(new Vector2(8505, 4686));
                    wall.Add(new Vector2(8505, 4736));
                    wall.Add(new Vector2(7702, 4736));
                    wall.Add(new Vector2(7702, 4734));
                    wall.Add(new Vector2(7701, 4734));
                    wall.Add(new Vector2(7701, 4686));
                    wall.Add(new Vector2(7557, 4686));
                    wall.Add(new Vector2(7557, 4685));
                    wall.Add(new Vector2(7525, 4685));
                    wall.Add(new Vector2(7525, 4686));
                    wall.Add(new Vector2(7500, 4686));
                    wall.Add(new Vector2(7500, 4636));
                    wall.Add(new Vector2(7350, 4636));
                    wall.Add(new Vector2(7350, 4586));
                    wall.Add(new Vector2(7250, 4586));
                    wall.Add(new Vector2(7250, 4536));
                    wall.Add(new Vector2(7100, 4536));
                    wall.Add(new Vector2(7100, 4116));
                    wall.Add(new Vector2(7102, 4116));
                    wall.Add(new Vector2(7102, 4085));
                    wall.Add(new Vector2(7103, 4085));
                    wall.Add(new Vector2(7103, 4083));
                    wall.Add(new Vector2(7152, 4083));
                    wall.Add(new Vector2(7152, 3985));
                    wall.Add(new Vector2(7153, 3985));
                    wall.Add(new Vector2(7153, 3983));
                    wall.Add(new Vector2(7458, 3983));
                    wall.Add(new Vector2(7458, 4183));
                    wall.Add(new Vector2(7558, 4183));
                    wall.Add(new Vector2(7558, 4233));
                    wall.Add(new Vector2(7583, 4233));
                    wall.Add(new Vector2(7583, 4234));
                    wall.Add(new Vector2(7606, 4234));
                    wall.Add(new Vector2(7606, 4241));
                    wall.Add(new Vector2(7607, 4241));
                    wall.Add(new Vector2(7607, 4284));
                    wall.Add(new Vector2(7756, 4284));
                    wall.Add(new Vector2(7756, 4285));
                    wall.Add(new Vector2(7757, 4285));
                    wall.Add(new Vector2(7757, 4334));
                    wall.Add(new Vector2(7826, 4334));
                    wall.Add(new Vector2(7826, 4335));
                    wall.Add(new Vector2(7852, 4335));
                    wall.Add(new Vector2(7852, 4336));
                    wall.Add(new Vector2(7902, 4336));
                    wall.Add(new Vector2(7902, 4286));
                    wall.Add(new Vector2(8050, 4286));
                    wall.Add(new Vector2(8050, 4246));
                    wall.Add(new Vector2(8052, 4246));
                    wall.Add(new Vector2(8052, 4241));
                    wall.Add(new Vector2(8053, 4241));
                    wall.Add(new Vector2(8053, 4222));
                    wall.Add(new Vector2(8051, 4222));
                    wall.Add(new Vector2(8051, 3888));
                    wall.Add(new Vector2(8050, 3888));
                    wall.Add(new Vector2(8050, 3887));
                    wall.Add(new Vector2(8001, 3887));
                    wall.Add(new Vector2(8001, 3883));
                    wall.Add(new Vector2(8000, 3883));
                    wall.Add(new Vector2(8000, 3837));
                    wall.Add(new Vector2(7901, 3837));
                    wall.Add(new Vector2(7901, 3833));
                    wall.Add(new Vector2(7900, 3833));
                    wall.Add(new Vector2(7900, 3787));
                    wall.Add(new Vector2(7851, 3787));
                    wall.Add(new Vector2(7851, 3783));
                    wall.Add(new Vector2(7850, 3783));
                    wall.Add(new Vector2(7850, 3633));
                    wall.Add(new Vector2(7900, 3633));
                    wall.Add(new Vector2(7900, 3583));
                    wall.Add(new Vector2(7950, 3583));
                    wall.Add(new Vector2(7950, 3533));
                    wall.Add(new Vector2(8206, 3533));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom blue jungle krug wall.
            /// </summary>
            /// <value>The bottom blue jungle krug wall.</value>
            public static Geometry.Polygon BottomBlueJungle_KrugWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(8056, 2387));
                    wall.Add(new Vector2(8057, 2387));
                    wall.Add(new Vector2(8057, 2433));
                    wall.Add(new Vector2(8081, 2433));
                    wall.Add(new Vector2(8081, 2437));
                    wall.Add(new Vector2(8107, 2437));
                    wall.Add(new Vector2(8107, 2487));
                    wall.Add(new Vector2(8157, 2487));
                    wall.Add(new Vector2(8157, 2584));
                    wall.Add(new Vector2(8205, 2584));
                    wall.Add(new Vector2(8205, 2587));
                    wall.Add(new Vector2(8207, 2587));
                    wall.Add(new Vector2(8207, 2603));
                    wall.Add(new Vector2(8208, 2603));
                    wall.Add(new Vector2(8208, 2783));
                    wall.Add(new Vector2(8258, 2783));
                    wall.Add(new Vector2(8258, 2933));
                    wall.Add(new Vector2(8408, 2933));
                    wall.Add(new Vector2(8408, 2934));
                    wall.Add(new Vector2(8599, 2934));
                    wall.Add(new Vector2(8599, 2796));
                    wall.Add(new Vector2(8598, 2796));
                    wall.Add(new Vector2(8598, 2786));
                    wall.Add(new Vector2(8599, 2786));
                    wall.Add(new Vector2(8599, 2784));
                    wall.Add(new Vector2(8648, 2784));
                    wall.Add(new Vector2(8648, 2586));
                    wall.Add(new Vector2(8853, 2586));
                    wall.Add(new Vector2(8853, 2635));
                    wall.Add(new Vector2(8954, 2635));
                    wall.Add(new Vector2(8954, 2682));
                    wall.Add(new Vector2(8957, 2682));
                    wall.Add(new Vector2(8957, 2685));
                    wall.Add(new Vector2(9106, 2685));
                    wall.Add(new Vector2(9106, 2735));
                    wall.Add(new Vector2(9206, 2735));
                    wall.Add(new Vector2(9206, 2785));
                    wall.Add(new Vector2(9306, 2785));
                    wall.Add(new Vector2(9306, 2834));
                    wall.Add(new Vector2(9749, 2834));
                    wall.Add(new Vector2(9749, 2786));
                    wall.Add(new Vector2(9750, 2786));
                    wall.Add(new Vector2(9750, 2784));
                    wall.Add(new Vector2(9780, 2784));
                    wall.Add(new Vector2(9780, 2785));
                    wall.Add(new Vector2(9849, 2785));
                    wall.Add(new Vector2(9849, 2736));
                    wall.Add(new Vector2(9850, 2736));
                    wall.Add(new Vector2(9850, 2735));
                    wall.Add(new Vector2(9870, 2735));
                    wall.Add(new Vector2(9870, 2733));
                    wall.Add(new Vector2(9949, 2733));
                    wall.Add(new Vector2(9949, 2701));
                    wall.Add(new Vector2(9950, 2701));
                    wall.Add(new Vector2(9950, 2683));
                    wall.Add(new Vector2(10050, 2683));
                    wall.Add(new Vector2(10050, 2633));
                    wall.Add(new Vector2(10255, 2633));
                    wall.Add(new Vector2(10255, 2634));
                    wall.Add(new Vector2(10258, 2634));
                    wall.Add(new Vector2(10258, 2733));
                    wall.Add(new Vector2(10285, 2733));
                    wall.Add(new Vector2(10285, 2734));
                    wall.Add(new Vector2(10308, 2734));
                    wall.Add(new Vector2(10308, 2834));
                    wall.Add(new Vector2(10306, 2834));
                    wall.Add(new Vector2(10306, 2836));
                    wall.Add(new Vector2(10208, 2836));
                    wall.Add(new Vector2(10208, 2884));
                    wall.Add(new Vector2(10206, 2884));
                    wall.Add(new Vector2(10206, 2886));
                    wall.Add(new Vector2(10158, 2886));
                    wall.Add(new Vector2(10158, 2934));
                    wall.Add(new Vector2(10156, 2934));
                    wall.Add(new Vector2(10156, 2936));
                    wall.Add(new Vector2(9956, 2936));
                    wall.Add(new Vector2(9956, 2986));
                    wall.Add(new Vector2(9858, 2986));
                    wall.Add(new Vector2(9858, 3036));
                    wall.Add(new Vector2(9658, 3036));
                    wall.Add(new Vector2(9658, 3086));
                    wall.Add(new Vector2(9538, 3086));
                    wall.Add(new Vector2(9538, 3084));
                    wall.Add(new Vector2(9505, 3084));
                    wall.Add(new Vector2(9505, 3134));
                    wall.Add(new Vector2(9403, 3134));
                    wall.Add(new Vector2(9403, 3135));
                    wall.Add(new Vector2(9308, 3135));
                    wall.Add(new Vector2(9308, 3185));
                    wall.Add(new Vector2(9258, 3185));
                    wall.Add(new Vector2(9258, 3235));
                    wall.Add(new Vector2(9208, 3235));
                    wall.Add(new Vector2(9208, 3285));
                    wall.Add(new Vector2(9158, 3285));
                    wall.Add(new Vector2(9158, 3335));
                    wall.Add(new Vector2(9108, 3335));
                    wall.Add(new Vector2(9108, 3370));
                    wall.Add(new Vector2(9106, 3370));
                    wall.Add(new Vector2(9106, 3383));
                    wall.Add(new Vector2(9056, 3383));
                    wall.Add(new Vector2(9056, 3408));
                    wall.Add(new Vector2(8901, 3408));
                    wall.Add(new Vector2(8901, 3383));
                    wall.Add(new Vector2(8801, 3383));
                    wall.Add(new Vector2(8801, 3333));
                    wall.Add(new Vector2(8751, 3333));
                    wall.Add(new Vector2(8751, 3283));
                    wall.Add(new Vector2(8651, 3283));
                    wall.Add(new Vector2(8651, 3237));
                    wall.Add(new Vector2(8518, 3237));
                    wall.Add(new Vector2(8518, 3200));
                    wall.Add(new Vector2(8303, 3200));
                    wall.Add(new Vector2(8303, 3195));
                    wall.Add(new Vector2(8299, 3195));
                    wall.Add(new Vector2(8299, 3185));
                    wall.Add(new Vector2(8249, 3185));
                    wall.Add(new Vector2(8249, 3137));
                    wall.Add(new Vector2(7907, 3137));
                    wall.Add(new Vector2(7907, 3187));
                    wall.Add(new Vector2(7807, 3187));
                    wall.Add(new Vector2(7807, 3237));
                    wall.Add(new Vector2(7652, 3237));
                    wall.Add(new Vector2(7652, 3236));
                    wall.Add(new Vector2(7650, 3236));
                    wall.Add(new Vector2(7650, 3211));
                    wall.Add(new Vector2(7649, 3211));
                    wall.Add(new Vector2(7649, 2725));
                    wall.Add(new Vector2(7651, 2725));
                    wall.Add(new Vector2(7651, 2594));
                    wall.Add(new Vector2(7666, 2594));
                    wall.Add(new Vector2(7666, 2583));
                    wall.Add(new Vector2(7701, 2583));
                    wall.Add(new Vector2(7701, 2533));
                    wall.Add(new Vector2(7751, 2533));
                    wall.Add(new Vector2(7751, 2483));
                    wall.Add(new Vector2(7801, 2483));
                    wall.Add(new Vector2(7801, 2433));
                    wall.Add(new Vector2(7851, 2433));
                    wall.Add(new Vector2(7851, 2383));
                    wall.Add(new Vector2(8056, 2383));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top red jungle wall 1.
            /// </summary>
            /// <value>The top red jungle wall 1.</value>
            public static Geometry.Polygon TopRedJungle_Wall_1
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(3955, 11633));
                    wall.Add(new Vector2(4055, 11633));
                    wall.Add(new Vector2(4055, 11674));
                    wall.Add(new Vector2(4056, 11674));
                    wall.Add(new Vector2(4056, 11683));
                    wall.Add(new Vector2(4105, 11683));
                    wall.Add(new Vector2(4105, 11684));
                    wall.Add(new Vector2(4106, 11684));
                    wall.Add(new Vector2(4106, 11733));
                    wall.Add(new Vector2(4155, 11733));
                    wall.Add(new Vector2(4155, 11734));
                    wall.Add(new Vector2(4156, 11734));
                    wall.Add(new Vector2(4156, 12034));
                    wall.Add(new Vector2(4154, 12034));
                    wall.Add(new Vector2(4154, 12036));
                    wall.Add(new Vector2(4106, 12036));
                    wall.Add(new Vector2(4106, 12084));
                    wall.Add(new Vector2(4104, 12084));
                    wall.Add(new Vector2(4104, 12086));
                    wall.Add(new Vector2(4056, 12086));
                    wall.Add(new Vector2(4056, 12154));
                    wall.Add(new Vector2(4054, 12154));
                    wall.Add(new Vector2(4054, 12186));
                    wall.Add(new Vector2(4004, 12186));
                    wall.Add(new Vector2(4004, 12236));
                    wall.Add(new Vector2(3956, 12236));
                    wall.Add(new Vector2(3956, 12283));
                    wall.Add(new Vector2(3954, 12283));
                    wall.Add(new Vector2(3954, 12286));
                    wall.Add(new Vector2(3906, 12286));
                    wall.Add(new Vector2(3906, 12383));
                    wall.Add(new Vector2(3904, 12383));
                    wall.Add(new Vector2(3904, 12386));
                    wall.Add(new Vector2(3856, 12386));
                    wall.Add(new Vector2(3856, 12433));
                    wall.Add(new Vector2(3806, 12433));
                    wall.Add(new Vector2(3806, 12533));
                    wall.Add(new Vector2(3799, 12533));
                    wall.Add(new Vector2(3799, 12534));
                    wall.Add(new Vector2(3756, 12534));
                    wall.Add(new Vector2(3756, 12683));
                    wall.Add(new Vector2(3754, 12683));
                    wall.Add(new Vector2(3754, 12684));
                    wall.Add(new Vector2(3706, 12684));
                    wall.Add(new Vector2(3706, 12708));
                    wall.Add(new Vector2(3704, 12708));
                    wall.Add(new Vector2(3704, 12784));
                    wall.Add(new Vector2(3654, 12784));
                    wall.Add(new Vector2(3654, 12884));
                    wall.Add(new Vector2(3449, 12884));
                    wall.Add(new Vector2(3449, 12834));
                    wall.Add(new Vector2(3314, 12834));
                    wall.Add(new Vector2(3314, 12832));
                    wall.Add(new Vector2(3302, 12832));
                    wall.Add(new Vector2(3302, 12782));
                    wall.Add(new Vector2(3202, 12782));
                    wall.Add(new Vector2(3202, 12732));
                    wall.Add(new Vector2(3152, 12732));
                    wall.Add(new Vector2(3152, 12683));
                    wall.Add(new Vector2(3102, 12683));
                    wall.Add(new Vector2(3102, 12633));
                    wall.Add(new Vector2(3052, 12633));
                    wall.Add(new Vector2(3052, 12433));
                    wall.Add(new Vector2(3002, 12433));
                    wall.Add(new Vector2(3002, 12379));
                    wall.Add(new Vector2(2999, 12379));
                    wall.Add(new Vector2(2999, 12184));
                    wall.Add(new Vector2(3049, 12184));
                    wall.Add(new Vector2(3049, 12134));
                    wall.Add(new Vector2(3099, 12134));
                    wall.Add(new Vector2(3099, 12084));
                    wall.Add(new Vector2(3149, 12084));
                    wall.Add(new Vector2(3149, 12034));
                    wall.Add(new Vector2(3199, 12034));
                    wall.Add(new Vector2(3199, 11934));
                    wall.Add(new Vector2(3326, 11934));
                    wall.Add(new Vector2(3326, 11933));
                    wall.Add(new Vector2(3351, 11933));
                    wall.Add(new Vector2(3351, 11883));
                    wall.Add(new Vector2(3401, 11883));
                    wall.Add(new Vector2(3401, 11835));
                    wall.Add(new Vector2(3445, 11835));
                    wall.Add(new Vector2(3445, 11833));
                    wall.Add(new Vector2(3450, 11833));
                    wall.Add(new Vector2(3450, 11783));
                    wall.Add(new Vector2(3500, 11783));
                    wall.Add(new Vector2(3500, 11733));
                    wall.Add(new Vector2(3550, 11733));
                    wall.Add(new Vector2(3550, 11683));
                    wall.Add(new Vector2(3600, 11683));
                    wall.Add(new Vector2(3600, 11643));
                    wall.Add(new Vector2(3601, 11643));
                    wall.Add(new Vector2(3601, 11636));
                    wall.Add(new Vector2(3685, 11636));
                    wall.Add(new Vector2(3685, 11633));
                    wall.Add(new Vector2(3800, 11633));
                    wall.Add(new Vector2(3800, 11583));
                    wall.Add(new Vector2(3955, 11583));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top red jungle wall 2.
            /// </summary>
            /// <value>The top red jungle wall 2.</value>
            public static Geometry.Polygon TopRedJungle_Wall_2
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(5212, 12485));
                    wall.Add(new Vector2(5258, 12485));
                    wall.Add(new Vector2(5258, 12535));
                    wall.Add(new Vector2(5408, 12535));
                    wall.Add(new Vector2(5408, 12785));
                    wall.Add(new Vector2(5404, 12785));
                    wall.Add(new Vector2(5404, 12787));
                    wall.Add(new Vector2(5358, 12787));
                    wall.Add(new Vector2(5358, 12930));
                    wall.Add(new Vector2(5357, 12930));
                    wall.Add(new Vector2(5357, 12934));
                    wall.Add(new Vector2(5307, 12934));
                    wall.Add(new Vector2(5307, 13084));
                    wall.Add(new Vector2(5257, 13084));
                    wall.Add(new Vector2(5257, 13134));
                    wall.Add(new Vector2(5056, 13134));
                    wall.Add(new Vector2(5056, 13136));
                    wall.Add(new Vector2(4311, 13136));
                    wall.Add(new Vector2(4311, 13135));
                    wall.Add(new Vector2(4301, 13135));
                    wall.Add(new Vector2(4301, 13085));
                    wall.Add(new Vector2(4251, 13085));
                    wall.Add(new Vector2(4251, 13035));
                    wall.Add(new Vector2(4151, 13035));
                    wall.Add(new Vector2(4151, 12885));
                    wall.Add(new Vector2(4152, 12885));
                    wall.Add(new Vector2(4152, 12884));
                    wall.Add(new Vector2(4201, 12884));
                    wall.Add(new Vector2(4201, 12785));
                    wall.Add(new Vector2(4202, 12785));
                    wall.Add(new Vector2(4202, 12784));
                    wall.Add(new Vector2(4251, 12784));
                    wall.Add(new Vector2(4251, 12735));
                    wall.Add(new Vector2(4252, 12735));
                    wall.Add(new Vector2(4252, 12734));
                    wall.Add(new Vector2(4301, 12734));
                    wall.Add(new Vector2(4301, 12685));
                    wall.Add(new Vector2(4302, 12685));
                    wall.Add(new Vector2(4302, 12684));
                    wall.Add(new Vector2(4537, 12684));
                    wall.Add(new Vector2(4537, 12685));
                    wall.Add(new Vector2(4637, 12685));
                    wall.Add(new Vector2(4637, 12684));
                    wall.Add(new Vector2(4951, 12684));
                    wall.Add(new Vector2(4951, 12586));
                    wall.Add(new Vector2(4952, 12586));
                    wall.Add(new Vector2(4952, 12584));
                    wall.Add(new Vector2(4999, 12584));
                    wall.Add(new Vector2(4999, 12537));
                    wall.Add(new Vector2(5002, 12537));
                    wall.Add(new Vector2(5002, 12534));
                    wall.Add(new Vector2(5049, 12534));
                    wall.Add(new Vector2(5049, 12487));
                    wall.Add(new Vector2(5052, 12487));
                    wall.Add(new Vector2(5052, 12484));
                    wall.Add(new Vector2(5212, 12484));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top red jungle wall 3.
            /// </summary>
            /// <value>The top red jungle wall 3.</value>
            public static Geometry.Polygon TopRedJungle_Wall_3
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(6208, 12786));
                    wall.Add(new Vector2(6406, 12786));
                    wall.Add(new Vector2(6406, 12787));
                    wall.Add(new Vector2(6408, 12787));
                    wall.Add(new Vector2(6408, 12833));
                    wall.Add(new Vector2(6556, 12833));
                    wall.Add(new Vector2(6556, 12837));
                    wall.Add(new Vector2(6558, 12837));
                    wall.Add(new Vector2(6558, 12883));
                    wall.Add(new Vector2(6656, 12883));
                    wall.Add(new Vector2(6656, 12885));
                    wall.Add(new Vector2(6657, 12885));
                    wall.Add(new Vector2(6657, 13185));
                    wall.Add(new Vector2(6302, 13185));
                    wall.Add(new Vector2(6302, 13184));
                    wall.Add(new Vector2(6068, 13184));
                    wall.Add(new Vector2(6068, 13186));
                    wall.Add(new Vector2(5753, 13186));
                    wall.Add(new Vector2(5753, 13184));
                    wall.Add(new Vector2(5751, 13184));
                    wall.Add(new Vector2(5751, 13084));
                    wall.Add(new Vector2(5801, 13084));
                    wall.Add(new Vector2(5801, 12984));
                    wall.Add(new Vector2(5851, 12984));
                    wall.Add(new Vector2(5851, 12884));
                    wall.Add(new Vector2(5901, 12884));
                    wall.Add(new Vector2(5901, 12834));
                    wall.Add(new Vector2(5953, 12834));
                    wall.Add(new Vector2(5953, 12787));
                    wall.Add(new Vector2(6003, 12787));
                    wall.Add(new Vector2(6003, 12737));
                    wall.Add(new Vector2(6208, 12737));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top red jungle wall 4.
            /// </summary>
            /// <value>The top red jungle wall 4.</value>
            public static Geometry.Polygon TopRedJungle_Wall_4
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(8151, 12384));
                    wall.Add(new Vector2(8156, 12384));
                    wall.Add(new Vector2(8156, 12433));
                    wall.Add(new Vector2(8304, 12433));
                    wall.Add(new Vector2(8304, 12434));
                    wall.Add(new Vector2(8306, 12434));
                    wall.Add(new Vector2(8306, 12483));
                    wall.Add(new Vector2(8404, 12483));
                    wall.Add(new Vector2(8404, 12484));
                    wall.Add(new Vector2(8406, 12484));
                    wall.Add(new Vector2(8406, 12533));
                    wall.Add(new Vector2(8504, 12533));
                    wall.Add(new Vector2(8504, 12556));
                    wall.Add(new Vector2(8506, 12556));
                    wall.Add(new Vector2(8506, 12583));
                    wall.Add(new Vector2(8524, 12583));
                    wall.Add(new Vector2(8524, 12586));
                    wall.Add(new Vector2(8556, 12586));
                    wall.Add(new Vector2(8556, 12886));
                    wall.Add(new Vector2(8555, 12886));
                    wall.Add(new Vector2(8555, 12887));
                    wall.Add(new Vector2(8506, 12887));
                    wall.Add(new Vector2(8506, 12986));
                    wall.Add(new Vector2(8505, 12986));
                    wall.Add(new Vector2(8505, 12987));
                    wall.Add(new Vector2(8456, 12987));
                    wall.Add(new Vector2(8456, 13036));
                    wall.Add(new Vector2(8455, 13036));
                    wall.Add(new Vector2(8455, 13037));
                    wall.Add(new Vector2(8406, 13037));
                    wall.Add(new Vector2(8406, 13096));
                    wall.Add(new Vector2(8405, 13096));
                    wall.Add(new Vector2(8405, 13137));
                    wall.Add(new Vector2(8355, 13137));
                    wall.Add(new Vector2(8355, 13187));
                    wall.Add(new Vector2(8015, 13187));
                    wall.Add(new Vector2(8015, 13185));
                    wall.Add(new Vector2(7834, 13185));
                    wall.Add(new Vector2(7834, 13186));
                    wall.Add(new Vector2(7289, 13186));
                    wall.Add(new Vector2(7289, 13185));
                    wall.Add(new Vector2(6953, 13185));
                    wall.Add(new Vector2(6953, 13184));
                    wall.Add(new Vector2(6951, 13184));
                    wall.Add(new Vector2(6951, 13157));
                    wall.Add(new Vector2(6949, 13157));
                    wall.Add(new Vector2(6949, 12887));
                    wall.Add(new Vector2(6953, 12887));
                    wall.Add(new Vector2(6953, 12885));
                    wall.Add(new Vector2(6999, 12885));
                    wall.Add(new Vector2(6999, 12837));
                    wall.Add(new Vector2(7003, 12837));
                    wall.Add(new Vector2(7003, 12835));
                    wall.Add(new Vector2(7016, 12835));
                    wall.Add(new Vector2(7016, 12833));
                    wall.Add(new Vector2(7149, 12833));
                    wall.Add(new Vector2(7149, 12787));
                    wall.Add(new Vector2(7151, 12787));
                    wall.Add(new Vector2(7151, 12783));
                    wall.Add(new Vector2(7249, 12783));
                    wall.Add(new Vector2(7249, 12737));
                    wall.Add(new Vector2(7251, 12737));
                    wall.Add(new Vector2(7251, 12733));
                    wall.Add(new Vector2(7349, 12733));
                    wall.Add(new Vector2(7349, 12687));
                    wall.Add(new Vector2(7351, 12687));
                    wall.Add(new Vector2(7351, 12683));
                    wall.Add(new Vector2(7399, 12683));
                    wall.Add(new Vector2(7399, 12637));
                    wall.Add(new Vector2(7401, 12637));
                    wall.Add(new Vector2(7401, 12633));
                    wall.Add(new Vector2(7451, 12633));
                    wall.Add(new Vector2(7451, 12583));
                    wall.Add(new Vector2(7501, 12583));
                    wall.Add(new Vector2(7501, 12543));
                    wall.Add(new Vector2(7503, 12543));
                    wall.Add(new Vector2(7503, 12537));
                    wall.Add(new Vector2(7553, 12537));
                    wall.Add(new Vector2(7553, 12487));
                    wall.Add(new Vector2(7603, 12487));
                    wall.Add(new Vector2(7603, 12437));
                    wall.Add(new Vector2(7606, 12437));
                    wall.Add(new Vector2(7606, 12433));
                    wall.Add(new Vector2(7650, 12433));
                    wall.Add(new Vector2(7650, 12384));
                    wall.Add(new Vector2(7651, 12384));
                    wall.Add(new Vector2(7651, 12383));
                    wall.Add(new Vector2(8151, 12383));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top red jungle wall 5.
            /// </summary>
            /// <value>The top red jungle wall 5.</value>
            public static Geometry.Polygon TopRedJungle_Wall_5
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(9307, 11634));
                    wall.Add(new Vector2(9407, 11634));
                    wall.Add(new Vector2(9407, 12533));
                    wall.Add(new Vector2(9404, 12533));
                    wall.Add(new Vector2(9404, 12784));
                    wall.Add(new Vector2(9403, 12784));
                    wall.Add(new Vector2(9403, 12786));
                    wall.Add(new Vector2(9354, 12786));
                    wall.Add(new Vector2(9354, 12819));
                    wall.Add(new Vector2(9353, 12819));
                    wall.Add(new Vector2(9353, 13036));
                    wall.Add(new Vector2(9303, 13036));
                    wall.Add(new Vector2(9303, 13086));
                    wall.Add(new Vector2(9253, 13086));
                    wall.Add(new Vector2(9253, 13136));
                    wall.Add(new Vector2(9048, 13136));
                    wall.Add(new Vector2(9048, 13086));
                    wall.Add(new Vector2(8949, 13086));
                    wall.Add(new Vector2(8949, 12796));
                    wall.Add(new Vector2(8951, 12796));
                    wall.Add(new Vector2(8951, 12785));
                    wall.Add(new Vector2(9001, 12785));
                    wall.Add(new Vector2(9001, 12499));
                    wall.Add(new Vector2(8999, 12499));
                    wall.Add(new Vector2(8999, 12334));
                    wall.Add(new Vector2(8925, 12334));
                    wall.Add(new Vector2(8925, 12335));
                    wall.Add(new Vector2(8900, 12335));
                    wall.Add(new Vector2(8900, 12334));
                    wall.Add(new Vector2(8899, 12334));
                    wall.Add(new Vector2(8899, 12285));
                    wall.Add(new Vector2(8850, 12285));
                    wall.Add(new Vector2(8850, 12284));
                    wall.Add(new Vector2(8849, 12284));
                    wall.Add(new Vector2(8849, 12235));
                    wall.Add(new Vector2(8750, 12235));
                    wall.Add(new Vector2(8750, 12185));
                    wall.Add(new Vector2(8650, 12185));
                    wall.Add(new Vector2(8650, 12135));
                    wall.Add(new Vector2(8600, 12135));
                    wall.Add(new Vector2(8600, 12034));
                    wall.Add(new Vector2(8650, 12034));
                    wall.Add(new Vector2(8650, 11969));
                    wall.Add(new Vector2(8649, 11969));
                    wall.Add(new Vector2(8649, 11934));
                    wall.Add(new Vector2(8699, 11934));
                    wall.Add(new Vector2(8699, 11834));
                    wall.Add(new Vector2(8749, 11834));
                    wall.Add(new Vector2(8749, 11784));
                    wall.Add(new Vector2(8799, 11784));
                    wall.Add(new Vector2(8799, 11684));
                    wall.Add(new Vector2(8849, 11684));
                    wall.Add(new Vector2(8849, 11584));
                    wall.Add(new Vector2(8899, 11584));
                    wall.Add(new Vector2(8899, 11534));
                    wall.Add(new Vector2(8949, 11534));
                    wall.Add(new Vector2(8949, 11509));
                    wall.Add(new Vector2(8950, 11509));
                    wall.Add(new Vector2(8950, 11435));
                    wall.Add(new Vector2(9087, 11435));
                    wall.Add(new Vector2(9087, 11434));
                    wall.Add(new Vector2(9307, 11434));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top red jungle wall 6.
            /// </summary>
            /// <value>The top red jungle wall 6.</value>
            public static Geometry.Polygon TopRedJungle_Wall_6
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(9058, 10033));
                    wall.Add(new Vector2(9208, 10033));
                    wall.Add(new Vector2(9208, 10083));
                    wall.Add(new Vector2(9399, 10083));
                    wall.Add(new Vector2(9399, 10086));
                    wall.Add(new Vector2(9404, 10086));
                    wall.Add(new Vector2(9404, 10136));
                    wall.Add(new Vector2(9504, 10136));
                    wall.Add(new Vector2(9504, 10187));
                    wall.Add(new Vector2(9554, 10187));
                    wall.Add(new Vector2(9554, 10201));
                    wall.Add(new Vector2(9557, 10201));
                    wall.Add(new Vector2(9557, 10236));
                    wall.Add(new Vector2(9607, 10236));
                    wall.Add(new Vector2(9607, 10286));
                    wall.Add(new Vector2(9657, 10286));
                    wall.Add(new Vector2(9657, 10333));
                    wall.Add(new Vector2(9704, 10333));
                    wall.Add(new Vector2(9704, 10348));
                    wall.Add(new Vector2(9708, 10348));
                    wall.Add(new Vector2(9708, 10383));
                    wall.Add(new Vector2(9758, 10383));
                    wall.Add(new Vector2(9758, 10433));
                    wall.Add(new Vector2(9858, 10433));
                    wall.Add(new Vector2(9858, 10483));
                    wall.Add(new Vector2(9905, 10483));
                    wall.Add(new Vector2(9905, 10533));
                    wall.Add(new Vector2(9955, 10533));
                    wall.Add(new Vector2(9955, 10583));
                    wall.Add(new Vector2(10005, 10583));
                    wall.Add(new Vector2(10005, 10737));
                    wall.Add(new Vector2(9858, 10737));
                    wall.Add(new Vector2(9858, 10783));
                    wall.Add(new Vector2(9855, 10783));
                    wall.Add(new Vector2(9855, 10787));
                    wall.Add(new Vector2(9808, 10787));
                    wall.Add(new Vector2(9808, 10883));
                    wall.Add(new Vector2(9805, 10883));
                    wall.Add(new Vector2(9805, 10887));
                    wall.Add(new Vector2(9758, 10887));
                    wall.Add(new Vector2(9758, 10893));
                    wall.Add(new Vector2(9756, 10893));
                    wall.Add(new Vector2(9756, 10933));
                    wall.Add(new Vector2(9755, 10933));
                    wall.Add(new Vector2(9755, 10937));
                    wall.Add(new Vector2(9707, 10937));
                    wall.Add(new Vector2(9707, 11032));
                    wall.Add(new Vector2(9706, 11032));
                    wall.Add(new Vector2(9706, 11033));
                    wall.Add(new Vector2(9704, 11033));
                    wall.Add(new Vector2(9704, 11034));
                    wall.Add(new Vector2(9657, 11034));
                    wall.Add(new Vector2(9657, 11082));
                    wall.Add(new Vector2(9656, 11082));
                    wall.Add(new Vector2(9656, 11083));
                    wall.Add(new Vector2(9607, 11083));
                    wall.Add(new Vector2(9607, 11132));
                    wall.Add(new Vector2(9606, 11132));
                    wall.Add(new Vector2(9606, 11133));
                    wall.Add(new Vector2(9446, 11133));
                    wall.Add(new Vector2(9446, 11135));
                    wall.Add(new Vector2(9151, 11135));
                    wall.Add(new Vector2(9151, 11133));
                    wall.Add(new Vector2(9150, 11133));
                    wall.Add(new Vector2(9150, 11085));
                    wall.Add(new Vector2(9101, 11085));
                    wall.Add(new Vector2(9101, 10825));
                    wall.Add(new Vector2(9100, 10825));
                    wall.Add(new Vector2(9100, 10735));
                    wall.Add(new Vector2(9050, 10735));
                    wall.Add(new Vector2(9050, 10635));
                    wall.Add(new Vector2(9000, 10635));
                    wall.Add(new Vector2(9000, 10535));
                    wall.Add(new Vector2(8950, 10535));
                    wall.Add(new Vector2(8950, 10518));
                    wall.Add(new Vector2(8949, 10518));
                    wall.Add(new Vector2(8949, 10435));
                    wall.Add(new Vector2(8900, 10435));
                    wall.Add(new Vector2(8900, 10385));
                    wall.Add(new Vector2(8850, 10385));
                    wall.Add(new Vector2(8850, 10284));
                    wall.Add(new Vector2(8801, 10284));
                    wall.Add(new Vector2(8801, 10283));
                    wall.Add(new Vector2(8800, 10283));
                    wall.Add(new Vector2(8800, 10184));
                    wall.Add(new Vector2(8751, 10184));
                    wall.Add(new Vector2(8751, 10183));
                    wall.Add(new Vector2(8750, 10183));
                    wall.Add(new Vector2(8750, 10033));
                    wall.Add(new Vector2(8900, 10033));
                    wall.Add(new Vector2(8900, 10003));
                    wall.Add(new Vector2(8901, 10003));
                    wall.Add(new Vector2(8901, 9983));
                    wall.Add(new Vector2(9058, 9983));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top red jungle wall 7.
            /// </summary>
            /// <value>The top red jungle wall 7.</value>
            public static Geometry.Polygon TopRedJungle_Wall_7
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(8307, 10436));
                    wall.Add(new Vector2(8407, 10436));
                    wall.Add(new Vector2(8407, 10483));
                    wall.Add(new Vector2(8454, 10483));
                    wall.Add(new Vector2(8454, 10486));
                    wall.Add(new Vector2(8457, 10486));
                    wall.Add(new Vector2(8457, 10533));
                    wall.Add(new Vector2(8504, 10533));
                    wall.Add(new Vector2(8504, 10536));
                    wall.Add(new Vector2(8507, 10536));
                    wall.Add(new Vector2(8507, 10583));
                    wall.Add(new Vector2(8554, 10583));
                    wall.Add(new Vector2(8554, 10586));
                    wall.Add(new Vector2(8557, 10586));
                    wall.Add(new Vector2(8557, 10683));
                    wall.Add(new Vector2(8604, 10683));
                    wall.Add(new Vector2(8604, 10686));
                    wall.Add(new Vector2(8607, 10686));
                    wall.Add(new Vector2(8607, 10706));
                    wall.Add(new Vector2(8604, 10706));
                    wall.Add(new Vector2(8604, 10756));
                    wall.Add(new Vector2(8606, 10756));
                    wall.Add(new Vector2(8606, 10783));
                    wall.Add(new Vector2(8654, 10783));
                    wall.Add(new Vector2(8654, 10786));
                    wall.Add(new Vector2(8656, 10786));
                    wall.Add(new Vector2(8656, 11136));
                    wall.Add(new Vector2(8606, 11136));
                    wall.Add(new Vector2(8606, 11186));
                    wall.Add(new Vector2(8556, 11186));
                    wall.Add(new Vector2(8556, 11266));
                    wall.Add(new Vector2(8554, 11266));
                    wall.Add(new Vector2(8554, 11283));
                    wall.Add(new Vector2(8504, 11283));
                    wall.Add(new Vector2(8504, 11383));
                    wall.Add(new Vector2(8475, 11383));
                    wall.Add(new Vector2(8475, 11386));
                    wall.Add(new Vector2(8457, 11386));
                    wall.Add(new Vector2(8457, 11485));
                    wall.Add(new Vector2(8455, 11485));
                    wall.Add(new Vector2(8455, 11486));
                    wall.Add(new Vector2(8407, 11486));
                    wall.Add(new Vector2(8407, 11585));
                    wall.Add(new Vector2(8357, 11585));
                    wall.Add(new Vector2(8357, 11635));
                    wall.Add(new Vector2(8307, 11635));
                    wall.Add(new Vector2(8307, 11685));
                    wall.Add(new Vector2(8157, 11685));
                    wall.Add(new Vector2(8157, 11735));
                    wall.Add(new Vector2(7993, 11735));
                    wall.Add(new Vector2(7993, 11736));
                    wall.Add(new Vector2(7753, 11736));
                    wall.Add(new Vector2(7753, 11686));
                    wall.Add(new Vector2(7703, 11686));
                    wall.Add(new Vector2(7703, 11639));
                    wall.Add(new Vector2(7700, 11639));
                    wall.Add(new Vector2(7700, 11384));
                    wall.Add(new Vector2(7750, 11384));
                    wall.Add(new Vector2(7750, 11334));
                    wall.Add(new Vector2(7800, 11334));
                    wall.Add(new Vector2(7800, 11284));
                    wall.Add(new Vector2(7900, 11284));
                    wall.Add(new Vector2(7900, 11234));
                    wall.Add(new Vector2(8050, 11234));
                    wall.Add(new Vector2(8050, 11184));
                    wall.Add(new Vector2(8145, 11184));
                    wall.Add(new Vector2(8145, 11186));
                    wall.Add(new Vector2(8150, 11186));
                    wall.Add(new Vector2(8150, 11136));
                    wall.Add(new Vector2(8184, 11136));
                    wall.Add(new Vector2(8184, 11133));
                    wall.Add(new Vector2(8199, 11133));
                    wall.Add(new Vector2(8199, 11033));
                    wall.Add(new Vector2(8203, 11033));
                    wall.Add(new Vector2(8203, 10752));
                    wall.Add(new Vector2(8199, 10752));
                    wall.Add(new Vector2(8199, 10387));
                    wall.Add(new Vector2(8202, 10387));
                    wall.Add(new Vector2(8202, 10386));
                    wall.Add(new Vector2(8307, 10386));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top red jungle krug wall.
            /// </summary>
            /// <value>The top red jungle krug wall.</value>
            public static Geometry.Polygon TopRedJungle_KrugWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(5904, 11433));
                    wall.Add(new Vector2(5908, 11433));
                    wall.Add(new Vector2(5908, 11482));
                    wall.Add(new Vector2(6004, 11482));
                    wall.Add(new Vector2(6004, 11483));
                    wall.Add(new Vector2(6008, 11483));
                    wall.Add(new Vector2(6008, 11532));
                    wall.Add(new Vector2(6104, 11532));
                    wall.Add(new Vector2(6104, 11533));
                    wall.Add(new Vector2(6108, 11533));
                    wall.Add(new Vector2(6108, 11582));
                    wall.Add(new Vector2(6154, 11582));
                    wall.Add(new Vector2(6154, 11584));
                    wall.Add(new Vector2(6155, 11584));
                    wall.Add(new Vector2(6155, 11586));
                    wall.Add(new Vector2(6156, 11586));
                    wall.Add(new Vector2(6156, 11608));
                    wall.Add(new Vector2(6157, 11608));
                    wall.Add(new Vector2(6157, 11632));
                    wall.Add(new Vector2(6179, 11632));
                    wall.Add(new Vector2(6179, 11633));
                    wall.Add(new Vector2(6257, 11633));
                    wall.Add(new Vector2(6257, 11683));
                    wall.Add(new Vector2(6557, 11683));
                    wall.Add(new Vector2(6557, 11733));
                    wall.Add(new Vector2(6606, 11733));
                    wall.Add(new Vector2(6606, 11736));
                    wall.Add(new Vector2(6608, 11736));
                    wall.Add(new Vector2(6608, 11783));
                    wall.Add(new Vector2(6884, 11783));
                    wall.Add(new Vector2(6884, 11785));
                    wall.Add(new Vector2(6899, 11785));
                    wall.Add(new Vector2(6899, 11775));
                    wall.Add(new Vector2(6901, 11775));
                    wall.Add(new Vector2(6901, 11737));
                    wall.Add(new Vector2(6903, 11737));
                    wall.Add(new Vector2(6903, 11735));
                    wall.Add(new Vector2(7001, 11735));
                    wall.Add(new Vector2(7001, 11687));
                    wall.Add(new Vector2(7003, 11687));
                    wall.Add(new Vector2(7003, 11685));
                    wall.Add(new Vector2(7208, 11685));
                    wall.Add(new Vector2(7208, 12125));
                    wall.Add(new Vector2(7206, 12125));
                    wall.Add(new Vector2(7206, 12285));
                    wall.Add(new Vector2(7156, 12285));
                    wall.Add(new Vector2(7156, 12385));
                    wall.Add(new Vector2(7106, 12385));
                    wall.Add(new Vector2(7106, 12435));
                    wall.Add(new Vector2(7096, 12435));
                    wall.Add(new Vector2(7096, 12437));
                    wall.Add(new Vector2(7056, 12437));
                    wall.Add(new Vector2(7056, 12487));
                    wall.Add(new Vector2(6749, 12487));
                    wall.Add(new Vector2(6749, 12437));
                    wall.Add(new Vector2(6699, 12437));
                    wall.Add(new Vector2(6699, 12337));
                    wall.Add(new Vector2(6649, 12337));
                    wall.Add(new Vector2(6649, 12137));
                    wall.Add(new Vector2(6599, 12137));
                    wall.Add(new Vector2(6599, 11986));
                    wall.Add(new Vector2(6558, 11986));
                    wall.Add(new Vector2(6558, 11983));
                    wall.Add(new Vector2(6546, 11983));
                    wall.Add(new Vector2(6546, 11984));
                    wall.Add(new Vector2(6475, 11984));
                    wall.Add(new Vector2(6475, 11985));
                    wall.Add(new Vector2(6466, 11985));
                    wall.Add(new Vector2(6466, 11986));
                    wall.Add(new Vector2(6288, 11986));
                    wall.Add(new Vector2(6288, 11987));
                    wall.Add(new Vector2(6258, 11987));
                    wall.Add(new Vector2(6258, 12087));
                    wall.Add(new Vector2(6208, 12087));
                    wall.Add(new Vector2(6208, 12287));
                    wall.Add(new Vector2(6158, 12287));
                    wall.Add(new Vector2(6158, 12337));
                    wall.Add(new Vector2(6049, 12337));
                    wall.Add(new Vector2(6049, 12287));
                    wall.Add(new Vector2(5899, 12287));
                    wall.Add(new Vector2(5899, 12237));
                    wall.Add(new Vector2(5799, 12237));
                    wall.Add(new Vector2(5799, 12187));
                    wall.Add(new Vector2(5699, 12187));
                    wall.Add(new Vector2(5699, 12137));
                    wall.Add(new Vector2(5644, 12137));
                    wall.Add(new Vector2(5644, 12136));
                    wall.Add(new Vector2(5601, 12136));
                    wall.Add(new Vector2(5601, 12135));
                    wall.Add(new Vector2(5599, 12135));
                    wall.Add(new Vector2(5599, 12086));
                    wall.Add(new Vector2(5471, 12086));
                    wall.Add(new Vector2(5471, 12085));
                    wall.Add(new Vector2(5144, 12085));
                    wall.Add(new Vector2(5144, 12084));
                    wall.Add(new Vector2(5038, 12084));
                    wall.Add(new Vector2(5038, 12087));
                    wall.Add(new Vector2(4958, 12087));
                    wall.Add(new Vector2(4958, 12137));
                    wall.Add(new Vector2(4858, 12137));
                    wall.Add(new Vector2(4858, 12167));
                    wall.Add(new Vector2(4855, 12167));
                    wall.Add(new Vector2(4855, 12183));
                    wall.Add(new Vector2(4854, 12183));
                    wall.Add(new Vector2(4854, 12184));
                    wall.Add(new Vector2(4755, 12184));
                    wall.Add(new Vector2(4755, 12232));
                    wall.Add(new Vector2(4754, 12232));
                    wall.Add(new Vector2(4754, 12234));
                    wall.Add(new Vector2(4599, 12234));
                    wall.Add(new Vector2(4599, 12184));
                    wall.Add(new Vector2(4549, 12184));
                    wall.Add(new Vector2(4549, 12084));
                    wall.Add(new Vector2(4550, 12084));
                    wall.Add(new Vector2(4550, 12082));
                    wall.Add(new Vector2(4599, 12082));
                    wall.Add(new Vector2(4599, 12034));
                    wall.Add(new Vector2(4600, 12034));
                    wall.Add(new Vector2(4600, 12032));
                    wall.Add(new Vector2(4699, 12032));
                    wall.Add(new Vector2(4699, 11984));
                    wall.Add(new Vector2(4700, 11984));
                    wall.Add(new Vector2(4700, 11982));
                    wall.Add(new Vector2(4849, 11982));
                    wall.Add(new Vector2(4849, 11934));
                    wall.Add(new Vector2(4850, 11934));
                    wall.Add(new Vector2(4850, 11933));
                    wall.Add(new Vector2(4949, 11933));
                    wall.Add(new Vector2(4949, 11883));
                    wall.Add(new Vector2(5099, 11883));
                    wall.Add(new Vector2(5099, 11833));
                    wall.Add(new Vector2(5299, 11833));
                    wall.Add(new Vector2(5299, 11783));
                    wall.Add(new Vector2(5389, 11783));
                    wall.Add(new Vector2(5389, 11784));
                    wall.Add(new Vector2(5499, 11784));
                    wall.Add(new Vector2(5499, 11735));
                    wall.Add(new Vector2(5549, 11735));
                    wall.Add(new Vector2(5549, 11685));
                    wall.Add(new Vector2(5599, 11685));
                    wall.Add(new Vector2(5599, 11680));
                    wall.Add(new Vector2(5600, 11680));
                    wall.Add(new Vector2(5600, 11635));
                    wall.Add(new Vector2(5650, 11635));
                    wall.Add(new Vector2(5650, 11585));
                    wall.Add(new Vector2(5668, 11585));
                    wall.Add(new Vector2(5668, 11583));
                    wall.Add(new Vector2(5694, 11583));
                    wall.Add(new Vector2(5694, 11582));
                    wall.Add(new Vector2(5699, 11582));
                    wall.Add(new Vector2(5699, 11532));
                    wall.Add(new Vector2(5749, 11532));
                    wall.Add(new Vector2(5749, 11482));
                    wall.Add(new Vector2(5799, 11482));
                    wall.Add(new Vector2(5799, 11432));
                    wall.Add(new Vector2(5904, 11432));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top red jungle red buff wall.
            /// </summary>
            /// <value>The top red jungle red buff wall.</value>
            public static Geometry.Polygon TopRedJungle_RedBuffWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(6954, 10135));
                    wall.Add(new Vector2(6956, 10135));
                    wall.Add(new Vector2(6956, 10183));
                    wall.Add(new Vector2(7204, 10183));
                    wall.Add(new Vector2(7204, 10233));
                    wall.Add(new Vector2(7354, 10233));
                    wall.Add(new Vector2(7354, 10283));
                    wall.Add(new Vector2(7379, 10283));
                    wall.Add(new Vector2(7379, 10284));
                    wall.Add(new Vector2(7504, 10284));
                    wall.Add(new Vector2(7504, 10286));
                    wall.Add(new Vector2(7505, 10286));
                    wall.Add(new Vector2(7505, 10334));
                    wall.Add(new Vector2(7604, 10334));
                    wall.Add(new Vector2(7604, 10336));
                    wall.Add(new Vector2(7605, 10336));
                    wall.Add(new Vector2(7605, 10341));
                    wall.Add(new Vector2(7608, 10341));
                    wall.Add(new Vector2(7608, 10383));
                    wall.Add(new Vector2(7757, 10383));
                    wall.Add(new Vector2(7757, 10386));
                    wall.Add(new Vector2(7758, 10386));
                    wall.Add(new Vector2(7758, 10883));
                    wall.Add(new Vector2(7730, 10883));
                    wall.Add(new Vector2(7730, 10887));
                    wall.Add(new Vector2(7708, 10887));
                    wall.Add(new Vector2(7708, 10933));
                    wall.Add(new Vector2(7705, 10933));
                    wall.Add(new Vector2(7705, 10937));
                    wall.Add(new Vector2(7400, 10937));
                    wall.Add(new Vector2(7400, 10936));
                    wall.Add(new Vector2(7399, 10936));
                    wall.Add(new Vector2(7399, 10737));
                    wall.Add(new Vector2(7350, 10737));
                    wall.Add(new Vector2(7350, 10736));
                    wall.Add(new Vector2(7349, 10736));
                    wall.Add(new Vector2(7349, 10687));
                    wall.Add(new Vector2(7265, 10687));
                    wall.Add(new Vector2(7265, 10686));
                    wall.Add(new Vector2(7253, 10686));
                    wall.Add(new Vector2(7253, 10685));
                    wall.Add(new Vector2(7250, 10685));
                    wall.Add(new Vector2(7250, 10636));
                    wall.Add(new Vector2(7159, 10636));
                    wall.Add(new Vector2(7159, 10637));
                    wall.Add(new Vector2(7099, 10637));
                    wall.Add(new Vector2(7099, 10587));
                    wall.Add(new Vector2(6908, 10587));
                    wall.Add(new Vector2(6908, 10636));
                    wall.Add(new Vector2(6904, 10636));
                    wall.Add(new Vector2(6904, 10637));
                    wall.Add(new Vector2(6804, 10637));
                    wall.Add(new Vector2(6804, 10731));
                    wall.Add(new Vector2(6805, 10731));
                    wall.Add(new Vector2(6805, 11036));
                    wall.Add(new Vector2(6855, 11036));
                    wall.Add(new Vector2(6855, 11051));
                    wall.Add(new Vector2(6858, 11051));
                    wall.Add(new Vector2(6858, 11086));
                    wall.Add(new Vector2(6958, 11086));
                    wall.Add(new Vector2(6958, 11136));
                    wall.Add(new Vector2(7008, 11136));
                    wall.Add(new Vector2(7008, 11286));
                    wall.Add(new Vector2(6958, 11286));
                    wall.Add(new Vector2(6958, 11336));
                    wall.Add(new Vector2(6908, 11336));
                    wall.Add(new Vector2(6908, 11386));
                    wall.Add(new Vector2(6713, 11386));
                    wall.Add(new Vector2(6713, 11385));
                    wall.Add(new Vector2(6702, 11385));
                    wall.Add(new Vector2(6702, 11339));
                    wall.Add(new Vector2(6701, 11339));
                    wall.Add(new Vector2(6701, 11335));
                    wall.Add(new Vector2(6602, 11335));
                    wall.Add(new Vector2(6602, 11334));
                    wall.Add(new Vector2(6601, 11334));
                    wall.Add(new Vector2(6601, 11285));
                    wall.Add(new Vector2(6552, 11285));
                    wall.Add(new Vector2(6552, 11284));
                    wall.Add(new Vector2(6551, 11284));
                    wall.Add(new Vector2(6551, 11235));
                    wall.Add(new Vector2(6502, 11235));
                    wall.Add(new Vector2(6502, 11234));
                    wall.Add(new Vector2(6501, 11234));
                    wall.Add(new Vector2(6501, 11185));
                    wall.Add(new Vector2(6457, 11185));
                    wall.Add(new Vector2(6457, 11184));
                    wall.Add(new Vector2(6451, 11184));
                    wall.Add(new Vector2(6451, 11134));
                    wall.Add(new Vector2(6401, 11134));
                    wall.Add(new Vector2(6401, 11084));
                    wall.Add(new Vector2(6351, 11084));
                    wall.Add(new Vector2(6351, 11062));
                    wall.Add(new Vector2(6348, 11062));
                    wall.Add(new Vector2(6348, 10987));
                    wall.Add(new Vector2(6298, 10987));
                    wall.Add(new Vector2(6298, 10937));
                    wall.Add(new Vector2(6248, 10937));
                    wall.Add(new Vector2(6248, 10602));
                    wall.Add(new Vector2(6249, 10602));
                    wall.Add(new Vector2(6249, 10333));
                    wall.Add(new Vector2(6299, 10333));
                    wall.Add(new Vector2(6299, 10268));
                    wall.Add(new Vector2(6300, 10268));
                    wall.Add(new Vector2(6300, 10185));
                    wall.Add(new Vector2(6350, 10185));
                    wall.Add(new Vector2(6350, 10135));
                    wall.Add(new Vector2(6914, 10135));
                    wall.Add(new Vector2(6914, 10133));
                    wall.Add(new Vector2(6954, 10133));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the top red jungle raptor wall.
            /// </summary>
            /// <value>The top red jungle raptor wall.</value>
            public static Geometry.Polygon TopRedJungle_RaptorWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(8054, 8735));
                    wall.Add(new Vector2(8055, 8735));
                    wall.Add(new Vector2(8055, 8783));
                    wall.Add(new Vector2(8154, 8783));
                    wall.Add(new Vector2(8154, 8785));
                    wall.Add(new Vector2(8155, 8785));
                    wall.Add(new Vector2(8155, 8812));
                    wall.Add(new Vector2(8157, 8812));
                    wall.Add(new Vector2(8157, 8832));
                    wall.Add(new Vector2(8207, 8832));
                    wall.Add(new Vector2(8207, 8882));
                    wall.Add(new Vector2(8257, 8882));
                    wall.Add(new Vector2(8257, 8932));
                    wall.Add(new Vector2(8307, 8932));
                    wall.Add(new Vector2(8307, 8982));
                    wall.Add(new Vector2(8357, 8982));
                    wall.Add(new Vector2(8357, 9032));
                    wall.Add(new Vector2(8407, 9032));
                    wall.Add(new Vector2(8407, 9082));
                    wall.Add(new Vector2(8507, 9082));
                    wall.Add(new Vector2(8507, 9132));
                    wall.Add(new Vector2(8537, 9132));
                    wall.Add(new Vector2(8537, 9135));
                    wall.Add(new Vector2(8554, 9135));
                    wall.Add(new Vector2(8554, 9185));
                    wall.Add(new Vector2(8604, 9185));
                    wall.Add(new Vector2(8604, 9235));
                    wall.Add(new Vector2(8654, 9235));
                    wall.Add(new Vector2(8654, 9285));
                    wall.Add(new Vector2(8704, 9285));
                    wall.Add(new Vector2(8704, 9286));
                    wall.Add(new Vector2(8705, 9286));
                    wall.Add(new Vector2(8705, 9311));
                    wall.Add(new Vector2(8708, 9311));
                    wall.Add(new Vector2(8708, 9335));
                    wall.Add(new Vector2(8754, 9335));
                    wall.Add(new Vector2(8754, 9336));
                    wall.Add(new Vector2(8758, 9336));
                    wall.Add(new Vector2(8758, 9385));
                    wall.Add(new Vector2(8764, 9385));
                    wall.Add(new Vector2(8764, 9386));
                    wall.Add(new Vector2(8808, 9386));
                    wall.Add(new Vector2(8808, 9436));
                    wall.Add(new Vector2(8858, 9436));
                    wall.Add(new Vector2(8858, 9486));
                    wall.Add(new Vector2(8908, 9486));
                    wall.Add(new Vector2(8908, 9536));
                    wall.Add(new Vector2(8958, 9536));
                    wall.Add(new Vector2(8958, 9636));
                    wall.Add(new Vector2(8750, 9636));
                    wall.Add(new Vector2(8750, 9586));
                    wall.Add(new Vector2(8667, 9586));
                    wall.Add(new Vector2(8667, 9587));
                    wall.Add(new Vector2(8358, 9587));
                    wall.Add(new Vector2(8358, 9907));
                    wall.Add(new Vector2(8357, 9907));
                    wall.Add(new Vector2(8357, 10083));
                    wall.Add(new Vector2(8354, 10083));
                    wall.Add(new Vector2(8354, 10085));
                    wall.Add(new Vector2(8149, 10085));
                    wall.Add(new Vector2(8149, 9935));
                    wall.Add(new Vector2(8099, 9935));
                    wall.Add(new Vector2(8099, 9810));
                    wall.Add(new Vector2(8102, 9810));
                    wall.Add(new Vector2(8102, 9733));
                    wall.Add(new Vector2(8103, 9733));
                    wall.Add(new Vector2(8103, 9632));
                    wall.Add(new Vector2(8053, 9632));
                    wall.Add(new Vector2(8053, 9603));
                    wall.Add(new Vector2(8048, 9603));
                    wall.Add(new Vector2(8048, 9286));
                    wall.Add(new Vector2(7835, 9286));
                    wall.Add(new Vector2(7835, 9287));
                    wall.Add(new Vector2(7658, 9287));
                    wall.Add(new Vector2(7658, 9634));
                    wall.Add(new Vector2(7479, 9634));
                    wall.Add(new Vector2(7479, 9637));
                    wall.Add(new Vector2(7299, 9637));
                    wall.Add(new Vector2(7299, 9587));
                    wall.Add(new Vector2(7149, 9587));
                    wall.Add(new Vector2(7149, 9287));
                    wall.Add(new Vector2(7151, 9287));
                    wall.Add(new Vector2(7151, 9284));
                    wall.Add(new Vector2(7199, 9284));
                    wall.Add(new Vector2(7199, 9262));
                    wall.Add(new Vector2(7200, 9262));
                    wall.Add(new Vector2(7200, 9236));
                    wall.Add(new Vector2(7201, 9236));
                    wall.Add(new Vector2(7201, 9234));
                    wall.Add(new Vector2(7213, 9234));
                    wall.Add(new Vector2(7213, 9233));
                    wall.Add(new Vector2(7300, 9233));
                    wall.Add(new Vector2(7300, 9186));
                    wall.Add(new Vector2(7301, 9186));
                    wall.Add(new Vector2(7301, 9184));
                    wall.Add(new Vector2(7303, 9184));
                    wall.Add(new Vector2(7303, 9183));
                    wall.Add(new Vector2(7350, 9183));
                    wall.Add(new Vector2(7350, 9136));
                    wall.Add(new Vector2(7353, 9136));
                    wall.Add(new Vector2(7353, 9133));
                    wall.Add(new Vector2(7400, 9133));
                    wall.Add(new Vector2(7400, 9086));
                    wall.Add(new Vector2(7403, 9086));
                    wall.Add(new Vector2(7403, 9083));
                    wall.Add(new Vector2(7450, 9083));
                    wall.Add(new Vector2(7450, 9036));
                    wall.Add(new Vector2(7453, 9036));
                    wall.Add(new Vector2(7453, 9033));
                    wall.Add(new Vector2(7550, 9033));
                    wall.Add(new Vector2(7550, 8986));
                    wall.Add(new Vector2(7553, 8986));
                    wall.Add(new Vector2(7553, 8983));
                    wall.Add(new Vector2(7600, 8983));
                    wall.Add(new Vector2(7600, 8936));
                    wall.Add(new Vector2(7603, 8936));
                    wall.Add(new Vector2(7603, 8933));
                    wall.Add(new Vector2(7688, 8933));
                    wall.Add(new Vector2(7688, 8937));
                    wall.Add(new Vector2(7700, 8937));
                    wall.Add(new Vector2(7700, 8887));
                    wall.Add(new Vector2(7705, 8887));
                    wall.Add(new Vector2(7705, 8885));
                    wall.Add(new Vector2(7750, 8885));
                    wall.Add(new Vector2(7750, 8835));
                    wall.Add(new Vector2(7800, 8835));
                    wall.Add(new Vector2(7800, 8785));
                    wall.Add(new Vector2(7850, 8785));
                    wall.Add(new Vector2(7850, 8735));
                    wall.Add(new Vector2(7929, 8735));
                    wall.Add(new Vector2(7929, 8733));
                    wall.Add(new Vector2(8054, 8733));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom red jungle wall 1.
            /// </summary>
            /// <value>The bottom red jungle wall 1.</value>
            public static Geometry.Polygon BottomRedJungle_Wall_1
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(12552, 3233));
                    wall.Add(new Vector2(12708, 3233));
                    wall.Add(new Vector2(12708, 3283));
                    wall.Add(new Vector2(12758, 3283));
                    wall.Add(new Vector2(12758, 3333));
                    wall.Add(new Vector2(12807, 3333));
                    wall.Add(new Vector2(12807, 3383));
                    wall.Add(new Vector2(12857, 3383));
                    wall.Add(new Vector2(12857, 3433));
                    wall.Add(new Vector2(12907, 3433));
                    wall.Add(new Vector2(12907, 3533));
                    wall.Add(new Vector2(12957, 3533));
                    wall.Add(new Vector2(12957, 3683));
                    wall.Add(new Vector2(13007, 3683));
                    wall.Add(new Vector2(13007, 3835));
                    wall.Add(new Vector2(13057, 3835));
                    wall.Add(new Vector2(13057, 4378));
                    wall.Add(new Vector2(13056, 4378));
                    wall.Add(new Vector2(13056, 4577));
                    wall.Add(new Vector2(13057, 4577));
                    wall.Add(new Vector2(13057, 5047));
                    wall.Add(new Vector2(13055, 5047));
                    wall.Add(new Vector2(13055, 5084));
                    wall.Add(new Vector2(12950, 5084));
                    wall.Add(new Vector2(12950, 5037));
                    wall.Add(new Vector2(12852, 5037));
                    wall.Add(new Vector2(12852, 5034));
                    wall.Add(new Vector2(12850, 5034));
                    wall.Add(new Vector2(12850, 4987));
                    wall.Add(new Vector2(12802, 4987));
                    wall.Add(new Vector2(12802, 4984));
                    wall.Add(new Vector2(12800, 4984));
                    wall.Add(new Vector2(12800, 4937));
                    wall.Add(new Vector2(12702, 4937));
                    wall.Add(new Vector2(12702, 4934));
                    wall.Add(new Vector2(12700, 4934));
                    wall.Add(new Vector2(12700, 4884));
                    wall.Add(new Vector2(12650, 4884));
                    wall.Add(new Vector2(12650, 4834));
                    wall.Add(new Vector2(12600, 4834));
                    wall.Add(new Vector2(12600, 4636));
                    wall.Add(new Vector2(12550, 4636));
                    wall.Add(new Vector2(12550, 4586));
                    wall.Add(new Vector2(12500, 4586));
                    wall.Add(new Vector2(12500, 4487));
                    wall.Add(new Vector2(12358, 4487));
                    wall.Add(new Vector2(12358, 4537));
                    wall.Add(new Vector2(12058, 4537));
                    wall.Add(new Vector2(12058, 4634));
                    wall.Add(new Vector2(12055, 4634));
                    wall.Add(new Vector2(12055, 4637));
                    wall.Add(new Vector2(12008, 4637));
                    wall.Add(new Vector2(12008, 4684));
                    wall.Add(new Vector2(12007, 4684));
                    wall.Add(new Vector2(12007, 4687));
                    wall.Add(new Vector2(11958, 4687));
                    wall.Add(new Vector2(11958, 4699));
                    wall.Add(new Vector2(11957, 4699));
                    wall.Add(new Vector2(11957, 4737));
                    wall.Add(new Vector2(11907, 4737));
                    wall.Add(new Vector2(11907, 4787));
                    wall.Add(new Vector2(11857, 4787));
                    wall.Add(new Vector2(11857, 4887));
                    wall.Add(new Vector2(11807, 4887));
                    wall.Add(new Vector2(11807, 4937));
                    wall.Add(new Vector2(11757, 4937));
                    wall.Add(new Vector2(11757, 4987));
                    wall.Add(new Vector2(11707, 4987));
                    wall.Add(new Vector2(11707, 5027));
                    wall.Add(new Vector2(11705, 5027));
                    wall.Add(new Vector2(11705, 5083));
                    wall.Add(new Vector2(11704, 5083));
                    wall.Add(new Vector2(11704, 5085));
                    wall.Add(new Vector2(11655, 5085));
                    wall.Add(new Vector2(11655, 5133));
                    wall.Add(new Vector2(11654, 5133));
                    wall.Add(new Vector2(11654, 5135));
                    wall.Add(new Vector2(11605, 5135));
                    wall.Add(new Vector2(11605, 5183));
                    wall.Add(new Vector2(11604, 5183));
                    wall.Add(new Vector2(11604, 5185));
                    wall.Add(new Vector2(11555, 5185));
                    wall.Add(new Vector2(11555, 5328));
                    wall.Add(new Vector2(11556, 5328));
                    wall.Add(new Vector2(11556, 5383));
                    wall.Add(new Vector2(11606, 5383));
                    wall.Add(new Vector2(11606, 5633));
                    wall.Add(new Vector2(11605, 5633));
                    wall.Add(new Vector2(11605, 5636));
                    wall.Add(new Vector2(11500, 5636));
                    wall.Add(new Vector2(11500, 5586));
                    wall.Add(new Vector2(11450, 5586));
                    wall.Add(new Vector2(11450, 5563));
                    wall.Add(new Vector2(11448, 5563));
                    wall.Add(new Vector2(11448, 5486));
                    wall.Add(new Vector2(11400, 5486));
                    wall.Add(new Vector2(11400, 5483));
                    wall.Add(new Vector2(11398, 5483));
                    wall.Add(new Vector2(11398, 5336));
                    wall.Add(new Vector2(11350, 5336));
                    wall.Add(new Vector2(11350, 5335));
                    wall.Add(new Vector2(11349, 5335));
                    wall.Add(new Vector2(11349, 5333));
                    wall.Add(new Vector2(11348, 5333));
                    wall.Add(new Vector2(11348, 5133));
                    wall.Add(new Vector2(11398, 5133));
                    wall.Add(new Vector2(11398, 5093));
                    wall.Add(new Vector2(11399, 5093));
                    wall.Add(new Vector2(11399, 5085));
                    wall.Add(new Vector2(11400, 5085));
                    wall.Add(new Vector2(11400, 5083));
                    wall.Add(new Vector2(11449, 5083));
                    wall.Add(new Vector2(11449, 5035));
                    wall.Add(new Vector2(11450, 5035));
                    wall.Add(new Vector2(11450, 5033));
                    wall.Add(new Vector2(11499, 5033));
                    wall.Add(new Vector2(11499, 4935));
                    wall.Add(new Vector2(11500, 4935));
                    wall.Add(new Vector2(11500, 4933));
                    wall.Add(new Vector2(11549, 4933));
                    wall.Add(new Vector2(11549, 4885));
                    wall.Add(new Vector2(11550, 4885));
                    wall.Add(new Vector2(11550, 4883));
                    wall.Add(new Vector2(11599, 4883));
                    wall.Add(new Vector2(11599, 4835));
                    wall.Add(new Vector2(11649, 4835));
                    wall.Add(new Vector2(11649, 4735));
                    wall.Add(new Vector2(11699, 4735));
                    wall.Add(new Vector2(11699, 4715));
                    wall.Add(new Vector2(11700, 4715));
                    wall.Add(new Vector2(11700, 4687));
                    wall.Add(new Vector2(11709, 4687));
                    wall.Add(new Vector2(11709, 4684));
                    wall.Add(new Vector2(11749, 4684));
                    wall.Add(new Vector2(11749, 4634));
                    wall.Add(new Vector2(11799, 4634));
                    wall.Add(new Vector2(11799, 4534));
                    wall.Add(new Vector2(11849, 4534));
                    wall.Add(new Vector2(11849, 4384));
                    wall.Add(new Vector2(11850, 4384));
                    wall.Add(new Vector2(11850, 4383));
                    wall.Add(new Vector2(11899, 4383));
                    wall.Add(new Vector2(11899, 4324));
                    wall.Add(new Vector2(11900, 4324));
                    wall.Add(new Vector2(11900, 4183));
                    wall.Add(new Vector2(11950, 4183));
                    wall.Add(new Vector2(11950, 4103));
                    wall.Add(new Vector2(11949, 4103));
                    wall.Add(new Vector2(11949, 3933));
                    wall.Add(new Vector2(11999, 3933));
                    wall.Add(new Vector2(11999, 3783));
                    wall.Add(new Vector2(12049, 3783));
                    wall.Add(new Vector2(12049, 3633));
                    wall.Add(new Vector2(12077, 3633));
                    wall.Add(new Vector2(12077, 3632));
                    wall.Add(new Vector2(12102, 3632));
                    wall.Add(new Vector2(12102, 3532));
                    wall.Add(new Vector2(12152, 3532));
                    wall.Add(new Vector2(12152, 3482));
                    wall.Add(new Vector2(12198, 3482));
                    wall.Add(new Vector2(12198, 3437));
                    wall.Add(new Vector2(12202, 3437));
                    wall.Add(new Vector2(12202, 3432));
                    wall.Add(new Vector2(12248, 3432));
                    wall.Add(new Vector2(12248, 3387));
                    wall.Add(new Vector2(12252, 3387));
                    wall.Add(new Vector2(12252, 3382));
                    wall.Add(new Vector2(12298, 3382));
                    wall.Add(new Vector2(12298, 3337));
                    wall.Add(new Vector2(12302, 3337));
                    wall.Add(new Vector2(12302, 3332));
                    wall.Add(new Vector2(12348, 3332));
                    wall.Add(new Vector2(12348, 3287));
                    wall.Add(new Vector2(12352, 3287));
                    wall.Add(new Vector2(12352, 3282));
                    wall.Add(new Vector2(12498, 3282));
                    wall.Add(new Vector2(12498, 3237));
                    wall.Add(new Vector2(12502, 3237));
                    wall.Add(new Vector2(12502, 3232));
                    wall.Add(new Vector2(12552, 3232));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom red jungle wall 2.
            /// </summary>
            /// <value>The bottom red jungle wall 2.</value>
            public static Geometry.Polygon BottomRedJungle_Wall_2
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(12255, 5036));
                    wall.Add(new Vector2(12256, 5036));
                    wall.Add(new Vector2(12256, 5050));
                    wall.Add(new Vector2(12258, 5050));
                    wall.Add(new Vector2(12258, 5583));
                    wall.Add(new Vector2(12308, 5583));
                    wall.Add(new Vector2(12308, 5768));
                    wall.Add(new Vector2(12306, 5768));
                    wall.Add(new Vector2(12306, 5936));
                    wall.Add(new Vector2(12356, 5936));
                    wall.Add(new Vector2(12356, 5948));
                    wall.Add(new Vector2(12357, 5948));
                    wall.Add(new Vector2(12357, 6183));
                    wall.Add(new Vector2(12355, 6183));
                    wall.Add(new Vector2(12355, 6186));
                    wall.Add(new Vector2(12307, 6186));
                    wall.Add(new Vector2(12307, 6333));
                    wall.Add(new Vector2(12304, 6333));
                    wall.Add(new Vector2(12304, 6334));
                    wall.Add(new Vector2(12257, 6334));
                    wall.Add(new Vector2(12257, 6383));
                    wall.Add(new Vector2(12254, 6383));
                    wall.Add(new Vector2(12254, 6384));
                    wall.Add(new Vector2(12171, 6384));
                    wall.Add(new Vector2(12171, 6387));
                    wall.Add(new Vector2(12157, 6387));
                    wall.Add(new Vector2(12157, 6433));
                    wall.Add(new Vector2(12156, 6433));
                    wall.Add(new Vector2(12156, 6437));
                    wall.Add(new Vector2(12006, 6437));
                    wall.Add(new Vector2(12006, 6487));
                    wall.Add(new Vector2(11906, 6487));
                    wall.Add(new Vector2(11906, 6537));
                    wall.Add(new Vector2(11816, 6537));
                    wall.Add(new Vector2(11816, 6535));
                    wall.Add(new Vector2(11703, 6535));
                    wall.Add(new Vector2(11703, 6533));
                    wall.Add(new Vector2(11701, 6533));
                    wall.Add(new Vector2(11701, 6485));
                    wall.Add(new Vector2(11603, 6485));
                    wall.Add(new Vector2(11603, 6483));
                    wall.Add(new Vector2(11601, 6483));
                    wall.Add(new Vector2(11601, 6435));
                    wall.Add(new Vector2(11518, 6435));
                    wall.Add(new Vector2(11518, 6433));
                    wall.Add(new Vector2(11501, 6433));
                    wall.Add(new Vector2(11501, 6333));
                    wall.Add(new Vector2(11551, 6333));
                    wall.Add(new Vector2(11551, 6283));
                    wall.Add(new Vector2(11601, 6283));
                    wall.Add(new Vector2(11601, 6233));
                    wall.Add(new Vector2(11651, 6233));
                    wall.Add(new Vector2(11651, 6188));
                    wall.Add(new Vector2(11653, 6188));
                    wall.Add(new Vector2(11653, 6185));
                    wall.Add(new Vector2(11700, 6185));
                    wall.Add(new Vector2(11700, 6136));
                    wall.Add(new Vector2(11703, 6136));
                    wall.Add(new Vector2(11703, 6135));
                    wall.Add(new Vector2(11750, 6135));
                    wall.Add(new Vector2(11750, 6086));
                    wall.Add(new Vector2(11753, 6086));
                    wall.Add(new Vector2(11753, 6085));
                    wall.Add(new Vector2(11800, 6085));
                    wall.Add(new Vector2(11800, 6066));
                    wall.Add(new Vector2(11802, 6066));
                    wall.Add(new Vector2(11802, 6036));
                    wall.Add(new Vector2(11803, 6036));
                    wall.Add(new Vector2(11803, 6035));
                    wall.Add(new Vector2(11852, 6035));
                    wall.Add(new Vector2(11852, 5986));
                    wall.Add(new Vector2(11853, 5986));
                    wall.Add(new Vector2(11853, 5985));
                    wall.Add(new Vector2(11868, 5985));
                    wall.Add(new Vector2(11868, 5986));
                    wall.Add(new Vector2(11902, 5986));
                    wall.Add(new Vector2(11902, 5936));
                    wall.Add(new Vector2(11949, 5936));
                    wall.Add(new Vector2(11949, 5901));
                    wall.Add(new Vector2(11951, 5901));
                    wall.Add(new Vector2(11951, 5886));
                    wall.Add(new Vector2(12001, 5886));
                    wall.Add(new Vector2(12001, 5834));
                    wall.Add(new Vector2(12000, 5834));
                    wall.Add(new Vector2(12000, 5741));
                    wall.Add(new Vector2(11999, 5741));
                    wall.Add(new Vector2(11999, 5484));
                    wall.Add(new Vector2(12023, 5484));
                    wall.Add(new Vector2(12023, 5483));
                    wall.Add(new Vector2(12049, 5483));
                    wall.Add(new Vector2(12049, 5334));
                    wall.Add(new Vector2(12099, 5334));
                    wall.Add(new Vector2(12099, 5244));
                    wall.Add(new Vector2(12101, 5244));
                    wall.Add(new Vector2(12101, 5236));
                    wall.Add(new Vector2(12103, 5236));
                    wall.Add(new Vector2(12103, 5235));
                    wall.Add(new Vector2(12120, 5235));
                    wall.Add(new Vector2(12120, 5232));
                    wall.Add(new Vector2(12150, 5232));
                    wall.Add(new Vector2(12150, 5032));
                    wall.Add(new Vector2(12255, 5032));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom red jungle wall 3.
            /// </summary>
            /// <value>The bottom red jungle wall 3.</value>
            public static Geometry.Polygon BottomRedJungle_Wall_3
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(9820, 7387));
                    wall.Add(new Vector2(9908, 7387));
                    wall.Add(new Vector2(9908, 7487));
                    wall.Add(new Vector2(9958, 7487));
                    wall.Add(new Vector2(9958, 7672));
                    wall.Add(new Vector2(9957, 7672));
                    wall.Add(new Vector2(9957, 7786));
                    wall.Add(new Vector2(9907, 7786));
                    wall.Add(new Vector2(9907, 7853));
                    wall.Add(new Vector2(9908, 7853));
                    wall.Add(new Vector2(9908, 7933));
                    wall.Add(new Vector2(9907, 7933));
                    wall.Add(new Vector2(9907, 7936));
                    wall.Add(new Vector2(9858, 7936));
                    wall.Add(new Vector2(9858, 8283));
                    wall.Add(new Vector2(9908, 8283));
                    wall.Add(new Vector2(9908, 8435));
                    wall.Add(new Vector2(9958, 8435));
                    wall.Add(new Vector2(9958, 8535));
                    wall.Add(new Vector2(10008, 8535));
                    wall.Add(new Vector2(10008, 8585));
                    wall.Add(new Vector2(10058, 8585));
                    wall.Add(new Vector2(10058, 8685));
                    wall.Add(new Vector2(10054, 8685));
                    wall.Add(new Vector2(10054, 8687));
                    wall.Add(new Vector2(10008, 8687));
                    wall.Add(new Vector2(10008, 8735));
                    wall.Add(new Vector2(10004, 8735));
                    wall.Add(new Vector2(10004, 8737));
                    wall.Add(new Vector2(9719, 8737));
                    wall.Add(new Vector2(9719, 8736));
                    wall.Add(new Vector2(9658, 8736));
                    wall.Add(new Vector2(9658, 8784));
                    wall.Add(new Vector2(9656, 8784));
                    wall.Add(new Vector2(9656, 8786));
                    wall.Add(new Vector2(9551, 8786));
                    wall.Add(new Vector2(9551, 8736));
                    wall.Add(new Vector2(9501, 8736));
                    wall.Add(new Vector2(9501, 8686));
                    wall.Add(new Vector2(9481, 8686));
                    wall.Add(new Vector2(9481, 8684));
                    wall.Add(new Vector2(9453, 8684));
                    wall.Add(new Vector2(9453, 8634));
                    wall.Add(new Vector2(9403, 8634));
                    wall.Add(new Vector2(9403, 8587));
                    wall.Add(new Vector2(9353, 8587));
                    wall.Add(new Vector2(9353, 8537));
                    wall.Add(new Vector2(9300, 8537));
                    wall.Add(new Vector2(9300, 8487));
                    wall.Add(new Vector2(9250, 8487));
                    wall.Add(new Vector2(9250, 8464));
                    wall.Add(new Vector2(9249, 8464));
                    wall.Add(new Vector2(9249, 8437));
                    wall.Add(new Vector2(9245, 8437));
                    wall.Add(new Vector2(9245, 8436));
                    wall.Add(new Vector2(9201, 8436));
                    wall.Add(new Vector2(9201, 8434));
                    wall.Add(new Vector2(9199, 8434));
                    wall.Add(new Vector2(9199, 8386));
                    wall.Add(new Vector2(9151, 8386));
                    wall.Add(new Vector2(9151, 8363));
                    wall.Add(new Vector2(9150, 8363));
                    wall.Add(new Vector2(9150, 8336));
                    wall.Add(new Vector2(9121, 8336));
                    wall.Add(new Vector2(9121, 8333));
                    wall.Add(new Vector2(9100, 8333));
                    wall.Add(new Vector2(9100, 8286));
                    wall.Add(new Vector2(9051, 8286));
                    wall.Add(new Vector2(9051, 8237));
                    wall.Add(new Vector2(9002, 8237));
                    wall.Add(new Vector2(9002, 8187));
                    wall.Add(new Vector2(8952, 8187));
                    wall.Add(new Vector2(8952, 7833));
                    wall.Add(new Vector2(9002, 7833));
                    wall.Add(new Vector2(9002, 7733));
                    wall.Add(new Vector2(9052, 7733));
                    wall.Add(new Vector2(9052, 7683));
                    wall.Add(new Vector2(9102, 7683));
                    wall.Add(new Vector2(9102, 7633));
                    wall.Add(new Vector2(9152, 7633));
                    wall.Add(new Vector2(9152, 7583));
                    wall.Add(new Vector2(9202, 7583));
                    wall.Add(new Vector2(9202, 7543));
                    wall.Add(new Vector2(9203, 7543));
                    wall.Add(new Vector2(9203, 7536));
                    wall.Add(new Vector2(9253, 7536));
                    wall.Add(new Vector2(9253, 7486));
                    wall.Add(new Vector2(9353, 7486));
                    wall.Add(new Vector2(9353, 7436));
                    wall.Add(new Vector2(9365, 7436));
                    wall.Add(new Vector2(9365, 7435));
                    wall.Add(new Vector2(9450, 7435));
                    wall.Add(new Vector2(9450, 7385));
                    wall.Add(new Vector2(9820, 7385));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom red jungle wall 4.
            /// </summary>
            /// <value>The bottom red jungle wall 4.</value>
            public static Geometry.Polygon BottomRedJungle_Wall_4
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(12058, 8587));
                    wall.Add(new Vector2(12008, 8587));
                    wall.Add(new Vector2(12008, 8637));
                    wall.Add(new Vector2(11958, 8637));
                    wall.Add(new Vector2(11958, 8687));
                    wall.Add(new Vector2(11908, 8687));
                    wall.Add(new Vector2(11908, 8737));
                    wall.Add(new Vector2(11858, 8737));
                    wall.Add(new Vector2(11858, 8787));
                    wall.Add(new Vector2(11808, 8787));
                    wall.Add(new Vector2(11808, 8837));
                    wall.Add(new Vector2(11758, 8837));
                    wall.Add(new Vector2(11758, 8897));
                    wall.Add(new Vector2(11754, 8897));
                    wall.Add(new Vector2(11754, 8934));
                    wall.Add(new Vector2(11704, 8934));
                    wall.Add(new Vector2(11704, 8969));
                    wall.Add(new Vector2(11703, 8969));
                    wall.Add(new Vector2(11703, 9063));
                    wall.Add(new Vector2(11707, 9063));
                    wall.Add(new Vector2(11707, 9233));
                    wall.Add(new Vector2(11757, 9233));
                    wall.Add(new Vector2(11757, 9333));
                    wall.Add(new Vector2(11807, 9333));
                    wall.Add(new Vector2(11807, 9433));
                    wall.Add(new Vector2(11757, 9433));
                    wall.Add(new Vector2(11757, 9483));
                    wall.Add(new Vector2(11707, 9483));
                    wall.Add(new Vector2(11707, 9533));
                    wall.Add(new Vector2(11706, 9533));
                    wall.Add(new Vector2(11706, 9536));
                    wall.Add(new Vector2(11506, 9536));
                    wall.Add(new Vector2(11506, 9586));
                    wall.Add(new Vector2(11426, 9586));
                    wall.Add(new Vector2(11426, 9587));
                    wall.Add(new Vector2(11356, 9587));
                    wall.Add(new Vector2(11356, 9637));
                    wall.Add(new Vector2(11206, 9637));
                    wall.Add(new Vector2(11206, 9687));
                    wall.Add(new Vector2(11056, 9687));
                    wall.Add(new Vector2(11056, 9737));
                    wall.Add(new Vector2(10956, 9737));
                    wall.Add(new Vector2(10956, 9786));
                    wall.Add(new Vector2(10807, 9786));
                    wall.Add(new Vector2(10807, 9835));
                    wall.Add(new Vector2(10806, 9835));
                    wall.Add(new Vector2(10806, 9836));
                    wall.Add(new Vector2(10656, 9836));
                    wall.Add(new Vector2(10656, 9835));
                    wall.Add(new Vector2(10652, 9835));
                    wall.Add(new Vector2(10652, 9785));
                    wall.Add(new Vector2(10602, 9785));
                    wall.Add(new Vector2(10602, 9735));
                    wall.Add(new Vector2(10600, 9735));
                    wall.Add(new Vector2(10600, 9737));
                    wall.Add(new Vector2(10500, 9737));
                    wall.Add(new Vector2(10500, 9687));
                    wall.Add(new Vector2(10450, 9687));
                    wall.Add(new Vector2(10450, 9637));
                    wall.Add(new Vector2(10350, 9637));
                    wall.Add(new Vector2(10350, 9587));
                    wall.Add(new Vector2(10300, 9587));
                    wall.Add(new Vector2(10300, 9537));
                    wall.Add(new Vector2(10250, 9537));
                    wall.Add(new Vector2(10250, 9487));
                    wall.Add(new Vector2(10200, 9487));
                    wall.Add(new Vector2(10200, 9437));
                    wall.Add(new Vector2(10150, 9437));
                    wall.Add(new Vector2(10150, 9384));
                    wall.Add(new Vector2(10148, 9384));
                    wall.Add(new Vector2(10148, 9337));
                    wall.Add(new Vector2(10125, 9337));
                    wall.Add(new Vector2(10125, 9336));
                    wall.Add(new Vector2(10100, 9336));
                    wall.Add(new Vector2(10100, 9334));
                    wall.Add(new Vector2(10098, 9334));
                    wall.Add(new Vector2(10098, 9286));
                    wall.Add(new Vector2(10050, 9286));
                    wall.Add(new Vector2(10050, 9284));
                    wall.Add(new Vector2(10048, 9284));
                    wall.Add(new Vector2(10048, 9236));
                    wall.Add(new Vector2(10000, 9236));
                    wall.Add(new Vector2(10000, 9234));
                    wall.Add(new Vector2(9998, 9234));
                    wall.Add(new Vector2(9998, 9084));
                    wall.Add(new Vector2(10455, 9084));
                    wall.Add(new Vector2(10455, 9134));
                    wall.Add(new Vector2(10505, 9134));
                    wall.Add(new Vector2(10505, 9184));
                    wall.Add(new Vector2(10607, 9184));
                    wall.Add(new Vector2(10607, 9234));
                    wall.Add(new Vector2(10749, 9234));
                    wall.Add(new Vector2(10749, 9185));
                    wall.Add(new Vector2(10752, 9185));
                    wall.Add(new Vector2(10752, 9184));
                    wall.Add(new Vector2(11001, 9184));
                    wall.Add(new Vector2(11001, 9136));
                    wall.Add(new Vector2(11002, 9136));
                    wall.Add(new Vector2(11002, 9134));
                    wall.Add(new Vector2(11037, 9134));
                    wall.Add(new Vector2(11037, 9136));
                    wall.Add(new Vector2(11101, 9136));
                    wall.Add(new Vector2(11101, 9086));
                    wall.Add(new Vector2(11153, 9086));
                    wall.Add(new Vector2(11153, 9084));
                    wall.Add(new Vector2(11166, 9084));
                    wall.Add(new Vector2(11166, 9082));
                    wall.Add(new Vector2(11201, 9082));
                    wall.Add(new Vector2(11201, 9032));
                    wall.Add(new Vector2(11301, 9032));
                    wall.Add(new Vector2(11301, 8982));
                    wall.Add(new Vector2(11401, 8982));
                    wall.Add(new Vector2(11401, 8969));
                    wall.Add(new Vector2(11399, 8969));
                    wall.Add(new Vector2(11399, 8884));
                    wall.Add(new Vector2(11401, 8884));
                    wall.Add(new Vector2(11401, 8882));
                    wall.Add(new Vector2(11449, 8882));
                    wall.Add(new Vector2(11449, 8834));
                    wall.Add(new Vector2(11451, 8834));
                    wall.Add(new Vector2(11451, 8832));
                    wall.Add(new Vector2(11499, 8832));
                    wall.Add(new Vector2(11499, 8784));
                    wall.Add(new Vector2(11501, 8784));
                    wall.Add(new Vector2(11501, 8782));
                    wall.Add(new Vector2(11549, 8782));
                    wall.Add(new Vector2(11549, 8734));
                    wall.Add(new Vector2(11599, 8734));
                    wall.Add(new Vector2(11599, 8684));
                    wall.Add(new Vector2(11649, 8684));
                    wall.Add(new Vector2(11649, 8584));
                    wall.Add(new Vector2(11699, 8584));
                    wall.Add(new Vector2(11699, 8534));
                    wall.Add(new Vector2(11749, 8534));
                    wall.Add(new Vector2(11749, 8305));
                    wall.Add(new Vector2(11751, 8305));
                    wall.Add(new Vector2(11751, 8233));
                    wall.Add(new Vector2(11801, 8233));
                    wall.Add(new Vector2(11801, 8057));
                    wall.Add(new Vector2(11799, 8057));
                    wall.Add(new Vector2(11799, 7837));
                    wall.Add(new Vector2(11803, 7837));
                    wall.Add(new Vector2(11803, 7834));
                    wall.Add(new Vector2(11849, 7834));
                    wall.Add(new Vector2(11849, 7787));
                    wall.Add(new Vector2(11852, 7787));
                    wall.Add(new Vector2(11852, 7786));
                    wall.Add(new Vector2(11853, 7786));
                    wall.Add(new Vector2(11853, 7784));
                    wall.Add(new Vector2(12058, 7784));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom red jungle wall 5.
            /// </summary>
            /// <value>The bottom red jungle wall 5.</value>
            public static Geometry.Polygon BottomRedJungle_Wall_5
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(12877, 7387));
                    wall.Add(new Vector2(13057, 7387));
                    wall.Add(new Vector2(13057, 7419));
                    wall.Add(new Vector2(13058, 7419));
                    wall.Add(new Vector2(13058, 7984));
                    wall.Add(new Vector2(13108, 7984));
                    wall.Add(new Vector2(13108, 8004));
                    wall.Add(new Vector2(13104, 8004));
                    wall.Add(new Vector2(13104, 8229));
                    wall.Add(new Vector2(13103, 8229));
                    wall.Add(new Vector2(13103, 8402));
                    wall.Add(new Vector2(13105, 8402));
                    wall.Add(new Vector2(13105, 8404));
                    wall.Add(new Vector2(13108, 8404));
                    wall.Add(new Vector2(13108, 8774));
                    wall.Add(new Vector2(13106, 8774));
                    wall.Add(new Vector2(13106, 9137));
                    wall.Add(new Vector2(13056, 9137));
                    wall.Add(new Vector2(13056, 9287));
                    wall.Add(new Vector2(13006, 9287));
                    wall.Add(new Vector2(13006, 9337));
                    wall.Add(new Vector2(12896, 9337));
                    wall.Add(new Vector2(12896, 9335));
                    wall.Add(new Vector2(12746, 9335));
                    wall.Add(new Vector2(12746, 9333));
                    wall.Add(new Vector2(12656, 9333));
                    wall.Add(new Vector2(12656, 9383));
                    wall.Add(new Vector2(12584, 9383));
                    wall.Add(new Vector2(12584, 9387));
                    wall.Add(new Vector2(12449, 9387));
                    wall.Add(new Vector2(12449, 9337));
                    wall.Add(new Vector2(12399, 9337));
                    wall.Add(new Vector2(12399, 9287));
                    wall.Add(new Vector2(12349, 9287));
                    wall.Add(new Vector2(12349, 9237));
                    wall.Add(new Vector2(12249, 9237));
                    wall.Add(new Vector2(12249, 9167));
                    wall.Add(new Vector2(12252, 9167));
                    wall.Add(new Vector2(12252, 8986));
                    wall.Add(new Vector2(12302, 8986));
                    wall.Add(new Vector2(12302, 8941));
                    wall.Add(new Vector2(12303, 8941));
                    wall.Add(new Vector2(12303, 8936));
                    wall.Add(new Vector2(12403, 8936));
                    wall.Add(new Vector2(12403, 8908));
                    wall.Add(new Vector2(12399, 8908));
                    wall.Add(new Vector2(12399, 8783));
                    wall.Add(new Vector2(12449, 8783));
                    wall.Add(new Vector2(12449, 8538));
                    wall.Add(new Vector2(12451, 8538));
                    wall.Add(new Vector2(12451, 8433));
                    wall.Add(new Vector2(12401, 8433));
                    wall.Add(new Vector2(12401, 8268));
                    wall.Add(new Vector2(12402, 8268));
                    wall.Add(new Vector2(12402, 7970));
                    wall.Add(new Vector2(12400, 7970));
                    wall.Add(new Vector2(12400, 7735));
                    wall.Add(new Vector2(12450, 7735));
                    wall.Add(new Vector2(12450, 7685));
                    wall.Add(new Vector2(12502, 7685));
                    wall.Add(new Vector2(12502, 7683));
                    wall.Add(new Vector2(12550, 7683));
                    wall.Add(new Vector2(12550, 7635));
                    wall.Add(new Vector2(12552, 7635));
                    wall.Add(new Vector2(12552, 7633));
                    wall.Add(new Vector2(12600, 7633));
                    wall.Add(new Vector2(12600, 7600));
                    wall.Add(new Vector2(12602, 7600));
                    wall.Add(new Vector2(12602, 7583));
                    wall.Add(new Vector2(12652, 7583));
                    wall.Add(new Vector2(12652, 7533));
                    wall.Add(new Vector2(12752, 7533));
                    wall.Add(new Vector2(12752, 7483));
                    wall.Add(new Vector2(12802, 7483));
                    wall.Add(new Vector2(12802, 7433));
                    wall.Add(new Vector2(12851, 7433));
                    wall.Add(new Vector2(12851, 7387));
                    wall.Add(new Vector2(12852, 7387));
                    wall.Add(new Vector2(12852, 7383));
                    wall.Add(new Vector2(12877, 7383));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom red jungle grom wall.
            /// </summary>
            /// <value>The bottom red jungle grom wall.</value>
            public static Geometry.Polygon BottomRedJungle_GromWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(12807, 5434));
                    wall.Add(new Vector2(12808, 5434));
                    wall.Add(new Vector2(12808, 5483));
                    wall.Add(new Vector2(12907, 5483));
                    wall.Add(new Vector2(12907, 5484));
                    wall.Add(new Vector2(12908, 5484));
                    wall.Add(new Vector2(12908, 5533));
                    wall.Add(new Vector2(13057, 5533));
                    wall.Add(new Vector2(13057, 5534));
                    wall.Add(new Vector2(13058, 5534));
                    wall.Add(new Vector2(13058, 5583));
                    wall.Add(new Vector2(13107, 5583));
                    wall.Add(new Vector2(13107, 5733));
                    wall.Add(new Vector2(13157, 5733));
                    wall.Add(new Vector2(13157, 5863));
                    wall.Add(new Vector2(13154, 5863));
                    wall.Add(new Vector2(13154, 6058));
                    wall.Add(new Vector2(13158, 6058));
                    wall.Add(new Vector2(13158, 6448));
                    wall.Add(new Vector2(13157, 6448));
                    wall.Add(new Vector2(13157, 6634));
                    wall.Add(new Vector2(13155, 6634));
                    wall.Add(new Vector2(13155, 6635));
                    wall.Add(new Vector2(13107, 6635));
                    wall.Add(new Vector2(13107, 6749));
                    wall.Add(new Vector2(13106, 6749));
                    wall.Add(new Vector2(13106, 6834));
                    wall.Add(new Vector2(13105, 6834));
                    wall.Add(new Vector2(13105, 6835));
                    wall.Add(new Vector2(13079, 6835));
                    wall.Add(new Vector2(13079, 6836));
                    wall.Add(new Vector2(13056, 6836));
                    wall.Add(new Vector2(13056, 6864));
                    wall.Add(new Vector2(13055, 6864));
                    wall.Add(new Vector2(13055, 6885));
                    wall.Add(new Vector2(13034, 6885));
                    wall.Add(new Vector2(13034, 6887));
                    wall.Add(new Vector2(12905, 6887));
                    wall.Add(new Vector2(12905, 6935));
                    wall.Add(new Vector2(12904, 6935));
                    wall.Add(new Vector2(12904, 6937));
                    wall.Add(new Vector2(12807, 6937));
                    wall.Add(new Vector2(12807, 6987));
                    wall.Add(new Vector2(12757, 6987));
                    wall.Add(new Vector2(12757, 7037));
                    wall.Add(new Vector2(12658, 7037));
                    wall.Add(new Vector2(12658, 7086));
                    wall.Add(new Vector2(12657, 7086));
                    wall.Add(new Vector2(12657, 7087));
                    wall.Add(new Vector2(12558, 7087));
                    wall.Add(new Vector2(12558, 7136));
                    wall.Add(new Vector2(12557, 7136));
                    wall.Add(new Vector2(12557, 7137));
                    wall.Add(new Vector2(12508, 7137));
                    wall.Add(new Vector2(12508, 7187));
                    wall.Add(new Vector2(12458, 7187));
                    wall.Add(new Vector2(12458, 7237));
                    wall.Add(new Vector2(12153, 7237));
                    wall.Add(new Vector2(12153, 7234));
                    wall.Add(new Vector2(12149, 7234));
                    wall.Add(new Vector2(12149, 7187));
                    wall.Add(new Vector2(12118, 7187));
                    wall.Add(new Vector2(12118, 7186));
                    wall.Add(new Vector2(12099, 7186));
                    wall.Add(new Vector2(12099, 6886));
                    wall.Add(new Vector2(12103, 6886));
                    wall.Add(new Vector2(12103, 6885));
                    wall.Add(new Vector2(12199, 6885));
                    wall.Add(new Vector2(12199, 6841));
                    wall.Add(new Vector2(12202, 6841));
                    wall.Add(new Vector2(12202, 6836));
                    wall.Add(new Vector2(12203, 6836));
                    wall.Add(new Vector2(12203, 6835));
                    wall.Add(new Vector2(12273, 6835));
                    wall.Add(new Vector2(12273, 6836));
                    wall.Add(new Vector2(12302, 6836));
                    wall.Add(new Vector2(12302, 6786));
                    wall.Add(new Vector2(12308, 6786));
                    wall.Add(new Vector2(12308, 6783));
                    wall.Add(new Vector2(12352, 6783));
                    wall.Add(new Vector2(12352, 6736));
                    wall.Add(new Vector2(12353, 6736));
                    wall.Add(new Vector2(12353, 6733));
                    wall.Add(new Vector2(12501, 6733));
                    wall.Add(new Vector2(12501, 6684));
                    wall.Add(new Vector2(12503, 6684));
                    wall.Add(new Vector2(12503, 6683));
                    wall.Add(new Vector2(12649, 6683));
                    wall.Add(new Vector2(12649, 6637));
                    wall.Add(new Vector2(12651, 6637));
                    wall.Add(new Vector2(12651, 6634));
                    wall.Add(new Vector2(12653, 6634));
                    wall.Add(new Vector2(12653, 6633));
                    wall.Add(new Vector2(12699, 6633));
                    wall.Add(new Vector2(12699, 6586));
                    wall.Add(new Vector2(12701, 6586));
                    wall.Add(new Vector2(12701, 6584));
                    wall.Add(new Vector2(12749, 6584));
                    wall.Add(new Vector2(12749, 6536));
                    wall.Add(new Vector2(12751, 6536));
                    wall.Add(new Vector2(12751, 6534));
                    wall.Add(new Vector2(12799, 6534));
                    wall.Add(new Vector2(12799, 6456));
                    wall.Add(new Vector2(12802, 6456));
                    wall.Add(new Vector2(12802, 6396));
                    wall.Add(new Vector2(12799, 6396));
                    wall.Add(new Vector2(12799, 6087));
                    wall.Add(new Vector2(12752, 6087));
                    wall.Add(new Vector2(12752, 6086));
                    wall.Add(new Vector2(12749, 6086));
                    wall.Add(new Vector2(12749, 6006));
                    wall.Add(new Vector2(12751, 6006));
                    wall.Add(new Vector2(12751, 5886));
                    wall.Add(new Vector2(12701, 5886));
                    wall.Add(new Vector2(12701, 5811));
                    wall.Add(new Vector2(12700, 5811));
                    wall.Add(new Vector2(12700, 5636));
                    wall.Add(new Vector2(12650, 5636));
                    wall.Add(new Vector2(12650, 5486));
                    wall.Add(new Vector2(12652, 5486));
                    wall.Add(new Vector2(12652, 5483));
                    wall.Add(new Vector2(12700, 5483));
                    wall.Add(new Vector2(12700, 5436));
                    wall.Add(new Vector2(12702, 5436));
                    wall.Add(new Vector2(12702, 5433));
                    wall.Add(new Vector2(12807, 5433));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom red jungle blue buff wall.
            /// </summary>
            /// <value>The bottom red jungle blue buff wall.</value>
            public static Geometry.Polygon BottomRedJungle_BlueBuffWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(11006, 5537));
                    wall.Add(new Vector2(11007, 5537));
                    wall.Add(new Vector2(11007, 5685));
                    wall.Add(new Vector2(11011, 5685));
                    wall.Add(new Vector2(11011, 5687));
                    wall.Add(new Vector2(11057, 5687));
                    wall.Add(new Vector2(11057, 5784));
                    wall.Add(new Vector2(11107, 5784));
                    wall.Add(new Vector2(11107, 5834));
                    wall.Add(new Vector2(11157, 5834));
                    wall.Add(new Vector2(11157, 5934));
                    wall.Add(new Vector2(11207, 5934));
                    wall.Add(new Vector2(11207, 6034));
                    wall.Add(new Vector2(11193, 6034));
                    wall.Add(new Vector2(11193, 6036));
                    wall.Add(new Vector2(11158, 6036));
                    wall.Add(new Vector2(11158, 6086));
                    wall.Add(new Vector2(11108, 6086));
                    wall.Add(new Vector2(11108, 6136));
                    wall.Add(new Vector2(11058, 6136));
                    wall.Add(new Vector2(11058, 6336));
                    wall.Add(new Vector2(11054, 6336));
                    wall.Add(new Vector2(11054, 6337));
                    wall.Add(new Vector2(11008, 6337));
                    wall.Add(new Vector2(11008, 6356));
                    wall.Add(new Vector2(11004, 6356));
                    wall.Add(new Vector2(11004, 6587));
                    wall.Add(new Vector2(11054, 6587));
                    wall.Add(new Vector2(11054, 6736));
                    wall.Add(new Vector2(11004, 6736));
                    wall.Add(new Vector2(11004, 6786));
                    wall.Add(new Vector2(10954, 6786));
                    wall.Add(new Vector2(10954, 6836));
                    wall.Add(new Vector2(10858, 6836));
                    wall.Add(new Vector2(10858, 6848));
                    wall.Add(new Vector2(10855, 6848));
                    wall.Add(new Vector2(10855, 6886));
                    wall.Add(new Vector2(10727, 6886));
                    wall.Add(new Vector2(10727, 6887));
                    wall.Add(new Vector2(10707, 6887));
                    wall.Add(new Vector2(10707, 7137));
                    wall.Add(new Vector2(10706, 7137));
                    wall.Add(new Vector2(10706, 7185));
                    wall.Add(new Vector2(10731, 7185));
                    wall.Add(new Vector2(10731, 7186));
                    wall.Add(new Vector2(10756, 7186));
                    wall.Add(new Vector2(10756, 7227));
                    wall.Add(new Vector2(10758, 7227));
                    wall.Add(new Vector2(10758, 7236));
                    wall.Add(new Vector2(10951, 7236));
                    wall.Add(new Vector2(10951, 7237));
                    wall.Add(new Vector2(11129, 7237));
                    wall.Add(new Vector2(11129, 7235));
                    wall.Add(new Vector2(11357, 7235));
                    wall.Add(new Vector2(11357, 7234));
                    wall.Add(new Vector2(11657, 7234));
                    wall.Add(new Vector2(11657, 7284));
                    wall.Add(new Vector2(11702, 7284));
                    wall.Add(new Vector2(11702, 7286));
                    wall.Add(new Vector2(11705, 7286));
                    wall.Add(new Vector2(11705, 7386));
                    wall.Add(new Vector2(11657, 7386));
                    wall.Add(new Vector2(11657, 7484));
                    wall.Add(new Vector2(11655, 7484));
                    wall.Add(new Vector2(11655, 7486));
                    wall.Add(new Vector2(11495, 7486));
                    wall.Add(new Vector2(11495, 7484));
                    wall.Add(new Vector2(11262, 7484));
                    wall.Add(new Vector2(11262, 7483));
                    wall.Add(new Vector2(10943, 7483));
                    wall.Add(new Vector2(10943, 7487));
                    wall.Add(new Vector2(10853, 7487));
                    wall.Add(new Vector2(10853, 7437));
                    wall.Add(new Vector2(10598, 7437));
                    wall.Add(new Vector2(10598, 7436));
                    wall.Add(new Vector2(10477, 7436));
                    wall.Add(new Vector2(10477, 7437));
                    wall.Add(new Vector2(10452, 7437));
                    wall.Add(new Vector2(10452, 7387));
                    wall.Add(new Vector2(10402, 7387));
                    wall.Add(new Vector2(10402, 7337));
                    wall.Add(new Vector2(10302, 7337));
                    wall.Add(new Vector2(10302, 7310));
                    wall.Add(new Vector2(10299, 7310));
                    wall.Add(new Vector2(10299, 7287));
                    wall.Add(new Vector2(10252, 7287));
                    wall.Add(new Vector2(10252, 7285));
                    wall.Add(new Vector2(10249, 7285));
                    wall.Add(new Vector2(10249, 7084));
                    wall.Add(new Vector2(10299, 7084));
                    wall.Add(new Vector2(10299, 6984));
                    wall.Add(new Vector2(10349, 6984));
                    wall.Add(new Vector2(10349, 6884));
                    wall.Add(new Vector2(10399, 6884));
                    wall.Add(new Vector2(10399, 6794));
                    wall.Add(new Vector2(10401, 6794));
                    wall.Add(new Vector2(10401, 6783));
                    wall.Add(new Vector2(10451, 6783));
                    wall.Add(new Vector2(10451, 6437));
                    wall.Add(new Vector2(10403, 6437));
                    wall.Add(new Vector2(10403, 6434));
                    wall.Add(new Vector2(10401, 6434));
                    wall.Add(new Vector2(10401, 6387));
                    wall.Add(new Vector2(10398, 6387));
                    wall.Add(new Vector2(10398, 6384));
                    wall.Add(new Vector2(10351, 6384));
                    wall.Add(new Vector2(10351, 6334));
                    wall.Add(new Vector2(10301, 6334));
                    wall.Add(new Vector2(10301, 6284));
                    wall.Add(new Vector2(10251, 6284));
                    wall.Add(new Vector2(10251, 6234));
                    wall.Add(new Vector2(10201, 6234));
                    wall.Add(new Vector2(10201, 6204));
                    wall.Add(new Vector2(10200, 6204));
                    wall.Add(new Vector2(10200, 6184));
                    wall.Add(new Vector2(10100, 6184));
                    wall.Add(new Vector2(10100, 6134));
                    wall.Add(new Vector2(10050, 6134));
                    wall.Add(new Vector2(10050, 6034));
                    wall.Add(new Vector2(10100, 6034));
                    wall.Add(new Vector2(10100, 5984));
                    wall.Add(new Vector2(10210, 5984));
                    wall.Add(new Vector2(10210, 5982));
                    wall.Add(new Vector2(10250, 5982));
                    wall.Add(new Vector2(10250, 5932));
                    wall.Add(new Vector2(10400, 5932));
                    wall.Add(new Vector2(10400, 5882));
                    wall.Add(new Vector2(10500, 5882));
                    wall.Add(new Vector2(10500, 5832));
                    wall.Add(new Vector2(10555, 5832));
                    wall.Add(new Vector2(10555, 5834));
                    wall.Add(new Vector2(10591, 5834));
                    wall.Add(new Vector2(10591, 5833));
                    wall.Add(new Vector2(10600, 5833));
                    wall.Add(new Vector2(10600, 5784));
                    wall.Add(new Vector2(10601, 5784));
                    wall.Add(new Vector2(10601, 5783));
                    wall.Add(new Vector2(10650, 5783));
                    wall.Add(new Vector2(10650, 5734));
                    wall.Add(new Vector2(10651, 5734));
                    wall.Add(new Vector2(10651, 5733));
                    wall.Add(new Vector2(10700, 5733));
                    wall.Add(new Vector2(10700, 5684));
                    wall.Add(new Vector2(10701, 5684));
                    wall.Add(new Vector2(10701, 5683));
                    wall.Add(new Vector2(10750, 5683));
                    wall.Add(new Vector2(10750, 5635));
                    wall.Add(new Vector2(10800, 5635));
                    wall.Add(new Vector2(10800, 5585));
                    wall.Add(new Vector2(10901, 5585));
                    wall.Add(new Vector2(10901, 5535));
                    wall.Add(new Vector2(11006, 5535));
                    return wall;
                }
            }

            /// <summary>
            /// Gets the bottom red jungle wolf wall.
            /// </summary>
            /// <value>The bottom red jungle wolf wall.</value>
            public static Geometry.Polygon BottomRedJungle_WolfWall
            {
                get
                {
                    var wall = new Geometry.Polygon();
                    wall.Add(new Vector2(10635, 7783));
                    wall.Add(new Vector2(11055, 7783));
                    wall.Add(new Vector2(11055, 7785));
                    wall.Add(new Vector2(11104, 7785));
                    wall.Add(new Vector2(11104, 7787));
                    wall.Add(new Vector2(11108, 7787));
                    wall.Add(new Vector2(11108, 7835));
                    wall.Add(new Vector2(11134, 7835));
                    wall.Add(new Vector2(11134, 7836));
                    wall.Add(new Vector2(11204, 7836));
                    wall.Add(new Vector2(11204, 7837));
                    wall.Add(new Vector2(11208, 7837));
                    wall.Add(new Vector2(11208, 7886));
                    wall.Add(new Vector2(11254, 7886));
                    wall.Add(new Vector2(11254, 7936));
                    wall.Add(new Vector2(11289, 7936));
                    wall.Add(new Vector2(11289, 7937));
                    wall.Add(new Vector2(11304, 7937));
                    wall.Add(new Vector2(11304, 7956));
                    wall.Add(new Vector2(11306, 7956));
                    wall.Add(new Vector2(11306, 7986));
                    wall.Add(new Vector2(11406, 7986));
                    wall.Add(new Vector2(11406, 8186));
                    wall.Add(new Vector2(11356, 8186));
                    wall.Add(new Vector2(11356, 8287));
                    wall.Add(new Vector2(11251, 8287));
                    wall.Add(new Vector2(11251, 8283));
                    wall.Add(new Vector2(11249, 8283));
                    wall.Add(new Vector2(11249, 8237));
                    wall.Add(new Vector2(11201, 8237));
                    wall.Add(new Vector2(11201, 8236));
                    wall.Add(new Vector2(11199, 8236));
                    wall.Add(new Vector2(11199, 8187));
                    wall.Add(new Vector2(11151, 8187));
                    wall.Add(new Vector2(11151, 8186));
                    wall.Add(new Vector2(11149, 8186));
                    wall.Add(new Vector2(11149, 8137));
                    wall.Add(new Vector2(11031, 8137));
                    wall.Add(new Vector2(11031, 8136));
                    wall.Add(new Vector2(10908, 8136));
                    wall.Add(new Vector2(10908, 8187));
                    wall.Add(new Vector2(10858, 8187));
                    wall.Add(new Vector2(10858, 8237));
                    wall.Add(new Vector2(10808, 8237));
                    wall.Add(new Vector2(10808, 8287));
                    wall.Add(new Vector2(10758, 8287));
                    wall.Add(new Vector2(10758, 8532));
                    wall.Add(new Vector2(10808, 8532));
                    wall.Add(new Vector2(10808, 8686));
                    wall.Add(new Vector2(10758, 8686));
                    wall.Add(new Vector2(10758, 8736));
                    wall.Add(new Vector2(10651, 8736));
                    wall.Add(new Vector2(10651, 8719));
                    wall.Add(new Vector2(10649, 8719));
                    wall.Add(new Vector2(10649, 8686));
                    wall.Add(new Vector2(10551, 8686));
                    wall.Add(new Vector2(10551, 8681));
                    wall.Add(new Vector2(10549, 8681));
                    wall.Add(new Vector2(10549, 8636));
                    wall.Add(new Vector2(10499, 8636));
                    wall.Add(new Vector2(10499, 8586));
                    wall.Add(new Vector2(10449, 8586));
                    wall.Add(new Vector2(10449, 8536));
                    wall.Add(new Vector2(10419, 8536));
                    wall.Add(new Vector2(10419, 8535));
                    wall.Add(new Vector2(10399, 8535));
                    wall.Add(new Vector2(10399, 8435));
                    wall.Add(new Vector2(10349, 8435));
                    wall.Add(new Vector2(10349, 8285));
                    wall.Add(new Vector2(10299, 8285));
                    wall.Add(new Vector2(10299, 8225));
                    wall.Add(new Vector2(10300, 8225));
                    wall.Add(new Vector2(10300, 8033));
                    wall.Add(new Vector2(10310, 8033));
                    wall.Add(new Vector2(10310, 8032));
                    wall.Add(new Vector2(10350, 8032));
                    wall.Add(new Vector2(10350, 7932));
                    wall.Add(new Vector2(10400, 7932));
                    wall.Add(new Vector2(10400, 7882));
                    wall.Add(new Vector2(10450, 7882));
                    wall.Add(new Vector2(10450, 7832));
                    wall.Add(new Vector2(10550, 7832));
                    wall.Add(new Vector2(10550, 7782));
                    wall.Add(new Vector2(10635, 7782));
                    return wall;
                }
            }
        }
    }
}
