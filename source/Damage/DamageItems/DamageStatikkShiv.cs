// <copyright file="DamageStatikkShiv.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Statikk Shiv item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.StatikkShiv)]
    public class DamageStatikkShiv : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageStatikkShiv" /> class.
        /// </summary>
        public DamageStatikkShiv()
        {
            this.ItemId = 3087;
            this.DamageType = Damage.DamageType.Magical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            if (source.GetBuff("ItemStatikShankCharge")?.Count == 100)
            {
                return
                    (target is Obj_AI_Minion
                         ? new double[] { 50, 50, 50, 50, 50, 56, 61, 67, 72, 77, 83, 88, 94, 99, 104, 110, 115, 120 }
                         : new[]
                               {
                                   110, 110, 110, 110, 110, 123.2, 134.2, 147.4, 158.4, 169.4, 182.6, 193.6, 206.8,
                                   217.8, 228.8, 242, 253, 264
                               })[source.Level - 1];
            }

            return 0;
        }

        #endregion
    }
}