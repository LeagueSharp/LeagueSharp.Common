// <copyright file="DamageJhinPassive.cs" company="LeagueSharp">
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
    public class DamageJhinPassive : IPassiveDamage
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
                    (source.TotalAttackDamage * 0.5f)
                    + ((target.MaxHealth - target.Health)
                       * new float[] { 0.15f, 0.20f, 0.25f }[Math.Min(2, (source.Level - 1) / 5)]));
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("jhinpassiveattackbuff");
        }

        #endregion
    }
}