// <copyright file="DamageZiggsPassive.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Passives
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Ziggs's Damage Passive.
    /// </summary>
    [Export(typeof(IPassiveDamage))]
    [ExportMetadata("ChampionName", "Ziggs")]
    public class DamageZiggsPassive : IPassiveDamage
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
                    ((float)0.3d * source.TotalMagicalDamage)
                    + new float[] { 20, 24, 28, 32, 36, 40, 48, 56, 64, 72, 80, 88, 100, 112, 124, 136, 148, 160 }[
                        source.Level - 1]);
        }

        /// <inheritdoc />
        public bool IsActive(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("ziggsshortfuse");
        }

        #endregion
    }
}