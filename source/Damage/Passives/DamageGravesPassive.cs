// <copyright file="DamageGravesPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Graves's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Graves")]
    public class DamageGravesPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return
                (float)
                ((((72 + (3 * source.Level)) / 100f)
                  * source.CalcDamage(target, Damage.DamageType.Physical, source.TotalAttackDamage))
                 - source.CalcDamage(target, Damage.DamageType.Physical, source.TotalAttackDamage));
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return true;
        }

        #endregion
    }
}