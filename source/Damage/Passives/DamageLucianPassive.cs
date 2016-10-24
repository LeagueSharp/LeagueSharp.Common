// <copyright file="DamageLucianPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Lucian's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Lucian")]
    public class DamageLucianPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.CalcDamage(
                target,
                Damage.DamageType.Physical,
                ((target.Type == GameObjectType.obj_AI_Minion
                      ? 1
                      : (source.Level < 6 ? 0.3 : (source.Level < 11 ? 0.4 : (source.Level < 16 ? 0.5 : 0.6))))
                 * source.TotalAttackDamage) * Damage.GetCritMultiplier(source, true));
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("lucianpassivebuff");
        }

        #endregion
    }
}