// <copyright file="HpBarDamageIndicator.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;

    using SharpDX;

    using Color = System.Drawing.Color;

    /// <summary>
    ///     The utility class.
    /// </summary>
    public partial class Utility
    {
        /// <summary>
        ///     The HP Bar damage indicator.
        /// </summary>
        public static class HpBarDamageIndicator
        {
            #region Constants

            private const int Height = 8;

            private const int Width = 103;

            private const int XOffset = 10;

            private const int YOffset = 20;

            #endregion

            #region Static Fields

            private static readonly Render.Text Text = new Render.Text(
                                                           0,
                                                           0,
                                                           string.Empty,
                                                           11,
                                                           new ColorBGRA(255, 0, 0, 255),
                                                           "monospace");

            private static DamageToUnitDelegate damageToUnitBackingField;

            #endregion

            #region Delegates

            /// <summary>
            ///     The damage to unit delegate.
            /// </summary>
            /// <param name="hero">
            ///     The hero.
            /// </param>
            /// <returns>
            ///     The <see cref="float" />.
            /// </returns>
            public delegate float DamageToUnitDelegate(Obj_AI_Hero hero);

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets or sets the color.
            /// </summary>
            public static Color Color { get; set; } = Color.Lime;

            /// <summary>
            ///     Gets or sets the damage to unit delegate.
            /// </summary>
            public static DamageToUnitDelegate DamageToUnit
            {
                get
                {
                    return damageToUnitBackingField;
                }

                set
                {
                    if (damageToUnitBackingField == null)
                    {
                        Drawing.OnDraw += Drawing_OnDraw;
                    }

                    damageToUnitBackingField = value;
                }
            }

            /// <summary>
            ///     Gets or sets a value indicating whether the indicator is enabled.
            /// </summary>
            public static bool Enabled { get; set; } = true;

            #endregion

            #region Methods

            private static void Drawing_OnDraw(EventArgs args)
            {
                if (!Enabled || damageToUnitBackingField == null)
                {
                    return;
                }

                var width = Drawing.Width;
                var height = Drawing.Height;

                foreach (var unit in
                    HeroManager.Enemies.FindAll(h => h.IsValid && h.IsHPBarRendered))
                {
                    var barPos = unit.HPBarPosition;

                    if (barPos.X < -200 || barPos.X > width + 200)
                    {
                        continue;
                    }

                    if (barPos.Y < -200 || barPos.X > height + 200)
                    {
                        continue;
                    }

                    var damage = damageToUnitBackingField(unit);
                    var percentHealthAfterDamage = Math.Max(0, unit.Health - damage) / unit.MaxHealth;
                    var xPos = barPos.X + XOffset + (Width * percentHealthAfterDamage);

                    Text.X = (int)barPos.X + XOffset;
                    Text.Y = (int)barPos.Y + YOffset - 13;
                    Text.Content = ((int)(unit.Health - damage)).ToString();
                    Text.OnEndScene();

                    Drawing.DrawLine(xPos, barPos.Y + YOffset, xPos, barPos.Y + YOffset + Height, 2, Color);
                }
            }

            #endregion
        }
    }
}