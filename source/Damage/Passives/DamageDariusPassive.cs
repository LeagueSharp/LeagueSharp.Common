// <copyright file="DamageDariusPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Darius's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Darius")]
    public class DamageDariusPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.CalcDamage(
                target,
                Damage.DamageType.Physical,
                ((9 + source.Level + (source.FlatPhysicalDamageMod * 0.3))
                 * Math.Min(target.GetBuffCount("dariushemo") + 1, 5))
                * (target.Type == GameObjectType.obj_AI_Minion ? 0.25 : 1));
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return true;
        }

        #endregion
    }
}