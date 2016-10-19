// <copyright file="DamageLeonaMarkPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Aatrox's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "")]
    public class DamageLeonaMarkPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            var lvl = ((Obj_AI_Hero)target.GetBuff("leonasunlight").Caster).Level - 1;
            if ((lvl / 2) % 1 > 0)
            {
                lvl -= 1;
            }

            return source.CalcDamage(target, Damage.DamageType.Magical, 20 + (15 * lvl / 2));
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return target.HasBuff("leonasunlight")
                   && target.GetBuff("leonasunlight").Caster.NetworkId != source.NetworkId;
        }

        #endregion
    }
}