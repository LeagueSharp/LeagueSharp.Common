// <copyright file="DamageEkkoPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Ekko's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Ekko")]
    public class DamageEkkoPassive : IPassiveDamage
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
                    10 + (source.Level * 10) + (source.TotalMagicalDamage * 0.8));
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return target.GetBuffCount("EkkoStacks") == 2;
        }

        #endregion
    }
}