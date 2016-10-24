// <copyright file="DamageDuskblade.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;
    using System.Linq;

    /// <summary>
    ///     Duskblade of Draktharr item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.Duskblade)]
    public class DamageDuskblade : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageDuskblade" /> class.
        /// </summary>
        public DamageDuskblade()
        {
            this.ItemId = 3147;
            this.DamageType = Damage.DamageType.Physical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            var item = source.InventoryItems.FirstOrDefault(i => (int)i.Id == this.ItemId);
            if (item == null)
            {
                return 0;
            }

            return source.Spellbook.GetSpell(item.SpellSlot)?.IsReady() ?? false
                       ? 90 + ((target.MaxHealth - target.Health) * .25)
                       : 0;
        }

        #endregion
    }
}