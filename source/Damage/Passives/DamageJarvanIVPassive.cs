// <copyright file="DamageJarvanIVPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     JarvanIV's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "JarvanIV")]
    public class DamageJarvanIVPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.CalcDamage(target, Damage.DamageType.Physical, Math.Min(target.Health * 0.1, 400));
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return !target.HasBuff("jarvanivmartialcadencecheck");
        }

        #endregion
    }
}