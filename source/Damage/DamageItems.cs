// <copyright file="DamageItems.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    using JetBrains.Annotations;

    using LeagueSharp.Common.DamageItems;

    /// <summary>
    ///     Damage calculations and data.
    /// </summary>
    public partial class Damage
    {
        #region Enums

        /// <summary>
        ///     Damaging item types.
        /// </summary>
        public enum DamageItems
        {
            /// <summary>
            ///     Hextech Gunblade
            /// </summary>
            Hexgun,

            /// <summary>
            ///     Death Fire Grasp
            /// </summary>
            [Obsolete("Removed from the game.")]
            Dfg,

            /// <summary>
            ///     Blade of the Ruined King
            /// </summary>
            Botrk,

            /// <summary>
            ///     Bilgewater
            /// </summary>
            Bilgewater,

            /// <summary>
            ///     Tiamat
            /// </summary>
            Tiamat,

            /// <summary>
            ///     Hydra
            /// </summary>
            Hydra,

            /// <summary>
            ///     Black Fire Torch
            /// </summary>
            [Obsolete("Removed from the game.")]
            BlackFireTorch,

            /// <summary>
            ///     Oding Veils
            /// </summary>
            [Obsolete("Removed from the game.")]
            OdingVeils,

            /// <summary>
            ///     Frost Queen Claims
            /// </summary>
            [Obsolete("Active changed, no longer deals damage.")]
            FrostQueenClaim,

            /// <summary>
            ///     Liandrys Torment
            /// </summary>
            LiandrysTorment,

            /// <summary>
            ///     Hextech GLP-2000
            /// </summary>
            HextechGLP,

            /// <summary>
            ///     Titanic Hydra
            /// </summary>
            TitanicHydra,

            /// <summary>
            ///     Hextech Protobelt-01
            /// </summary>
            HextechProtobelt,

            /// <summary>
            ///     Wit's End
            /// </summary>
            WitsEnd,

            /// <summary>
            ///     Trinity Force
            /// </summary>
            TrinityForce,

            /// <summary>
            ///     Sunfire Cape
            /// </summary>
            SunfireCape,

            /// <summary>
            ///     Statikk Shiv
            /// </summary>
            StatikkShiv,

            /// <summary>
            ///     Sheen
            /// </summary>
            Sheen,

            /// <summary>
            ///     Serrated Dirk
            /// </summary>
            SerratedDirk,

            /// <summary>
            ///     Runaan's Hurricane
            /// </summary>
            RunaansHurricane,

            /// <summary>
            ///     Recurve Bow
            /// </summary>
            RecurveBow,

            /// <summary>
            ///     Rapid Firecannon
            /// </summary>
            RapidFirecannon,

            /// <summary>
            ///     Muramana
            /// </summary>
            Muramana,

            /// <summary>
            ///     Lord Van Damm's Pillager
            /// </summary>
            LordVanDammsPillager,

            /// <summary>
            ///     Lich Bane
            /// </summary>
            LichBane,

            /// <summary>
            ///     Kirchei's Shard
            /// </summary>
            KircheisShard,

            /// <summary>
            ///     Iceborn Gauntlet
            /// </summary>
            IcebornGauntlet,

            /// <summary>
            ///     Duskblade of Draktharr
            /// </summary>
            Duskblade,

            /// <summary>
            ///     Bami's Cinder
            /// </summary>
            BamisCinder
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the items.
        /// </summary>
        [ImportMany]
        public IEnumerable<Lazy<IDamageItem, IDamageItemMetadata>> ItemLazies { get; protected set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Calculates the item damage.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        /// <param name="item">
        ///     The item.
        /// </param>
        /// <param name="type">
        ///     The raw damage type.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public double GetItemDamage(
            [NotNull] Obj_AI_Hero source,
            [NotNull] Obj_AI_Base target,
            DamageItems item,
            ItemDamageType type = ItemDamageType.Default)
        {
            var value = 0d;
            var damage = this.GetItemDamage(item);

            if (damage != null)
            {
                if (type.HasFlag(ItemDamageType.Default))
                {
                    value += damage.GetDamage(source, target);
                }

                if (type.HasFlag(ItemDamageType.Dot))
                {
                    value += damage.GetDotDamage(source, target);
                }

                if (type.HasFlag(ItemDamageType.Passive))
                {
                    value += damage.GetPassiveDamage(source, target);
                }
            }

            return value;
        }

        /// <summary>
        ///     Retrieves the item damage from the lazy collection.
        /// </summary>
        /// <param name="item">
        ///     The item.
        /// </param>
        /// <returns>
        ///     The <see cref="IDamageItem" />.
        /// </returns>
        [CanBeNull]
        public IDamageItem GetItemDamage(DamageItems item)
            => this.ItemLazies.FirstOrDefault(i => i.Metadata.Item == item)?.Value;

        #endregion
    }
}