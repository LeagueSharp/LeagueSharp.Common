// <copyright file="DamageBardPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Bard's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Bard")]
    public class DamageBardPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.CalcDamage(
                target,
                Damage.DamageType.Magical,
                new[] { 30, 55, 80, 110, 140, 175, 210, 245, 280, 315, 345, 375, 400, 425, 445, 465 }[
                    Math.Min(source.GetBuffCount("bardpdisplaychimecount") / 10, 15)]
                + (source.GetBuffCount("bardpdisplaychimecount") > 150
                       ? Math.Truncate((source.GetBuffCount("bardpdisplaychimecount") - 150) / 5d) * 20
                       : 0) + (0.3 * source.TotalMagicalDamage));
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.GetBuffCount("bardpspiritammocount") > 0;
        }

        #endregion
    }
}