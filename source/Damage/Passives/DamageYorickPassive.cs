// <copyright file="DamageYorickPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;
    using System.Linq;

    /// <summary>
    ///     Yorick's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Yorick")]
    public class DamageYorickPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.CalcDamage(
                target,
                Damage.DamageType.Physical,
                (0.05
                 * MinionManager.GetMinions(float.MaxValue)
                     .Count(
                         g =>
                             g.Team == source.Team
                             && (g.Name.Equals("Clyde") || g.Name.Equals("Inky") || g.Name.Equals("Blinky")
                                 || (g.HasBuff("yorickunholysymbiosis")
                                     && g.GetBuff("yorickunholysymbiosis").Caster.NetworkId == source.NetworkId))))
                * source.TotalAttackDamage);
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("YorickUnholySymbiosis");
        }

        #endregion
    }
}