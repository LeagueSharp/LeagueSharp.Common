// <copyright file="DamageNasusQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;
    using System.Linq;

    /// <summary>
    ///     Spell Damage, Nasus Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Nasus", SpellSlot.Q)]
    public class DamageNasusQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageNasusQ" /> class.
        /// </summary>
        public DamageNasusQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return (from buff in ObjectManager.Player.Buffs where buff.Name == "nasusqstacks" select buff.Count).FirstOrDefault() + new double[] { 30, 50, 70, 90, 110 }[level];
        }

        #endregion
    }
}