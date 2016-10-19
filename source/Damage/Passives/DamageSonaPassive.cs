// <copyright file="DamageSonaPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Sona's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Sona")]
    public class DamageSonaPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.CalcDamage(
                target,
                Damage.DamageType.Magical,
                (6
                 + ((source.Level < 4
                         ? 7
                         : (source.Level < 6 ? 8 : (source.Level < 7 ? 9 : (source.Level < 15 ? 10 : 15))))
                    * source.Level)) + (0.2 * target.TotalMagicalDamage));
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("SonaPassiveReady");
        }

        #endregion
    }
}