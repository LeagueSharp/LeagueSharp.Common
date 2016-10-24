// <copyright file="DamageEkkoPassive1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Ekko's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Ekko")]
    public class DamageEkkoPassive1 : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            var dmg =
                (float)
                source.CalcDamage(
                    target,
                    Damage.DamageType.Magical,
                    (target.MaxHealth - target.Health) * (5 + (Math.Floor(source.TotalMagicalDamage / 100) * 2.2f))
                    / 100);
            if (!(target is Obj_AI_Hero) && dmg > 150f)
            {
                dmg = 150f;
            }

            return dmg;
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return target.HealthPercent < 30;
        }

        #endregion
    }
}