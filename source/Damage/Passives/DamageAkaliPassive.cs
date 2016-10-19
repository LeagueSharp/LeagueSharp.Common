// <copyright file="DamageAkaliPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Akali's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Akali")]
    public class DamageAkaliPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return
                (float)
                source.CalcDamage(
                    target,
                    Damage.DamageType.Magical,
                    (0.06 + (Math.Abs(source.TotalMagicalDamage / 100) * 0.16667)) * source.TotalAttackDamage);
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return true;
        }

        #endregion
    }
}