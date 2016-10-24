// <copyright file="DamageJhinPassive1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Jhin's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Jhin")]
    public class DamageJhinPassive1 : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return
                (float)
                source.CalcDamage(
                    target,
                    Damage.DamageType.Physical,
                    (Items.HasItem((int)ItemId.Infinity_Edge, source) ? 0.875 : 0.5)
                    * (source.TotalAttackDamage
                       * (1
                          + ((new[] { 2, 3, 4, 5, 6, 7, 8, 10, 12, 14, 16, 18, 20, 24, 28, 32, 36, 40 }[source.Level - 1
                              ] + (Math.Floor(source.Crit * 100 / 10) * 4)
                              + (Math.Floor((source.AttackSpeedMod - 1) * 100 / 10) * 2.5)) / 100))));
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return Math.Abs(source.Crit - 1) < float.Epsilon;
        }

        #endregion
    }
}