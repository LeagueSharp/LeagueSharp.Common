// <copyright file="DamageKalistaE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Kalista E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Kalista", SpellSlot.E)]
    public class DamageKalistaE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKalistaE" /> class.
        /// </summary>
        public DamageKalistaE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            var count = target.GetBuffCount("kalistaexpungemarker");
            if (count > 0)
            {
                return (new double[] { 20, 30, 40, 50, 60 }[level]
                        + (0.6 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)))
                       + ((count - 1)
                          * (new double[] { 10, 14, 19, 25, 32 }[level]
                             + (new[] { 0.2, 0.225, 0.25, 0.275, 0.3 }[level]
                                * (source.BaseAttackDamage + source.FlatPhysicalDamageMod))));
            }

            return 0;
        }

        #endregion
    }
}