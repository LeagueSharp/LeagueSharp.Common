// <copyright file="MinionInfo.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Provides information about the minion.
    /// </summary>
    public class MinionInfo
    {
        #region Static Fields

        private static readonly string[] LargeNameRegex =
            {
                "SRU_Murkwolf[0-9.]{1,}", "SRU_Gromp", "SRU_Blue[0-9.]{1,}",
                "SRU_Razorbeak[0-9.]{1,}", "SRU_Red[0-9.]{1,}",
                "SRU_Krug[0-9]{1,}"
            };

        private static readonly string[] LegendaryNameRegex = { "SRU_Dragon", "SRU_Baron", "SRU_RiftHerald" };

        private static readonly string[] SmallNameRegex = { "SRU_[a-zA-Z](.*?)Mini", "Sru_Crab" };

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MinionInfo" /> class.
        /// </summary>
        /// <param name="minion">
        ///     The minion.
        /// </param>
        public MinionInfo(Obj_AI_Minion minion)
        {
            this.Instance = minion;
            if (!minion.IsValid || minion.IsDead)
            {
                return;
            }

            this.IsMinion = minion.Name.Contains("Minion");
            this.IsWard = minion.Name.Contains("Ward");

            var isJungleMob = false;
            if (SmallNameRegex.Any(regex => Regex.IsMatch(minion.Name, regex)))
            {
                this.JungleType = JungleType.Small;
                isJungleMob = true;
            }
            else if (LargeNameRegex.Any(regex => Regex.IsMatch(minion.Name, regex)))
            {
                this.JungleType = JungleType.Large;
                isJungleMob = true;
            }
            else if (LegendaryNameRegex.Any(regex => Regex.IsMatch(minion.Name, regex)))
            {
                this.JungleType = JungleType.Legendary;
                isJungleMob = true;
            }

            this.IsJungleMob = isJungleMob;
            this.IsComponent = !this.IsMinion && !this.IsWard && !this.IsJungleMob;
            this.IsJungleBuff = minion.CharData.BaseSkinName.Equals("SRU_Blue")
                                || minion.CharData.BaseSkinName.Equals("SRU_Red");

            if (this.IsMinion)
            {
                var regex =
                    new Regex(@"Minion_T(?<Team>\d+)+L(?<Lane>\d+)+S(?<Wave>\d+)+N(?<Index>\d+)").Match(minion.Name);
                if (!regex.Success)
                {
                    return;
                }

                this.Lane = (LaneType)Convert.ToInt32(regex.Groups["Lane"]);
                this.Wave = Convert.ToInt32(regex.Groups["Wave"]);
                this.Index = Convert.ToInt32(regex.Groups["Index"]);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the index.
        /// </summary>
        public int Index { get; }

        /// <summary>
        ///     Gets the minion instance.
        /// </summary>
        public Obj_AI_Minion Instance { get; }

        /// <summary>
        ///     Gets a value indicating whether the minion is a component.
        /// </summary>
        public bool IsComponent { get; }

        /// <summary>
        ///     Gets a value indicating whether the jungle mob is a buff carrier.
        /// </summary>
        public bool IsJungleBuff { get; }

        /// <summary>
        ///     Gets a value indicating whether the minion is a jungle mob.
        /// </summary>
        public bool IsJungleMob { get; }

        /// <summary>
        ///     Gets a value indicating whether the minion is a minion AI.
        /// </summary>
        public bool IsMinion { get; }

        /// <summary>
        ///     Gets a value indicating whether the minion is a ward.
        /// </summary>
        public bool IsWard { get; }

        /// <summary>
        ///     Gets the jungle type.
        /// </summary>
        public JungleType JungleType { get; }

        /// <summary>
        ///     Gets the lane.
        /// </summary>
        public LaneType Lane { get; }

        /// <summary>
        ///     Gets the wave.
        /// </summary>
        public int Wave { get; }

        #endregion
    }
}