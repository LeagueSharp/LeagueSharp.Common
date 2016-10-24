// <copyright file="DamageYasuoPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Yasuo's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Yasuo")]
    public class DamageYasuoPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.CalcDamage(
                target,
                Damage.DamageType.Physical,
                (Items.HasItem((int)ItemId.Infinity_Edge, source) ? 1.25 : 0.8) * source.TotalAttackDamage);
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return Math.Abs(source.Crit - 1) < float.Epsilon;
        }

        #endregion
    }
}