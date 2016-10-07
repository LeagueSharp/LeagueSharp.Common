// <copyright file="DamageTwitchE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;
    using System.Linq;

    /// <summary>
    ///     Spell Damage, Twitch E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Twitch", SpellSlot.E)]
    public class DamageTwitchE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTwitchE" /> class.
        /// </summary>
        public DamageTwitchE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return ((from buff in target.Buffs where buff.DisplayName.ToLower() == "twitchdeadlyvenom" select buff.Count).FirstOrDefault() * (new double[] { 15, 20, 25, 30, 35 }[level] + (0.2 * source.TotalMagicalDamage) + (0.25 * source.FlatPhysicalDamageMod))) + new double[] { 20, 35, 50, 65, 80 }[level];
        }

        #endregion
    }
}