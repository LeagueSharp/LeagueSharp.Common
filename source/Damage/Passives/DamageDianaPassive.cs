// <copyright file="DamageDianaPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Diana's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Diana")]
    public class DamageDianaPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.CalcDamage(
                target,
                Damage.DamageType.Magical,
                15
                + ((source.Level < 6
                        ? 5
                        : (source.Level < 11 ? 10 : (source.Level < 14 ? 15 : (source.Level < 16 ? 20 : 25))))
                   * source.Level) + (source.TotalMagicalDamage * 0.8));
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("dianaarcready");
        }

        #endregion
    }
}