// <copyright file="DamageKalistaMarkPassive.cs" company="LeagueSharp">
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
    public class DamageKalistaMarkPassive : IPassiveDamage
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return ((Obj_AI_Hero)target.GetBuff("kalistacoopstrikemarkbuff").Caster).GetSpellDamage(target, SpellSlot.W);
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return target.HasBuff("kalistacoopstrikemarkbuff") && source.HasBuff("kalistacoopstrikeally");
        }

        #endregion
    }
}