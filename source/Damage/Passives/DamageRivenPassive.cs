// <copyright file="DamageRivenPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Riven's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Riven")]
    public class DamageRivenPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return
                (float)
                source.CalcDamage(
                    target,
                    Damage.DamageType.Physical,
                    (source.Level < 3
                         ? 0.25
                         : (source.Level < 6
                                ? 0.29167
                                : (source.Level < 9
                                       ? 0.3333
                                       : (source.Level < 12
                                              ? 0.375
                                              : (source.Level < 15 ? 0.4167 : (source.Level < 18 ? 0.4583 : 0.5))))))
                    * source.TotalAttackDamage);
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.GetBuffCount("rivenpassiveaaboost") > 0;
        }

        #endregion
    }
}