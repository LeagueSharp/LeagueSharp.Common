#region LICENSE
/*
 Copyright 2014 - 2014 LeagueSharp
 CustomizableAntiGapcloser is part of LeagueSharp.Common.
 
 LeagueSharp.Common is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 LeagueSharp.Common is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with LeagueSharp.Common. If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

#region

using System;
using System.Collections.Generic;
using System.Linq;

using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The delegate for the <see cref="CustomizableAntiGapcloser.OnEnemyCustomGapcloser"/> event.
    /// </summary>
    /// <param name="cGapcloser">The CGapcloser.</param>
    public delegate void OnAGapcloseH(CActiveCGapcloser cGapcloser);

    /// <summary>
    /// The type of Customizable AntiGapcloser.
    /// </summary>
    public enum CGapcloserTypes
    {
        /// <summary>
        /// The Customizable AntiGapcloser used a skillshot ability.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        Skillshot,

        /// <summary>
        /// The Customizable AntiGapcloser used a targeted ability.
        /// </summary>
        Targeted
    }

    /// <summary>
    /// Represents a Customizable AntiGapcloser.
    /// </summary>
    public struct CGapcloser
    {
        /// <summary>
        /// The champion name
        /// </summary>
        public string ChampionName;

        /// <summary>
        /// The skill type
        /// </summary>
        public CGapcloserTypes SkillType;

        /// <summary>
        /// The slot
        /// </summary>
        public SpellSlot Slot;

        /// <summary>
        /// The spell name
        /// </summary>
        public string SpellName;

        /// <summary>
        /// The danger level
        /// </summary>
        public int DangerLevel;
    }

    /// <summary>
    /// Represents an active Customizable AntiGapcloser.
    /// </summary>
    public struct CActiveCGapcloser
    {
        /// <summary>
        /// The end
        /// </summary>
        public Vector3 End;

        /// <summary>
        /// The sender
        /// </summary>
        public Obj_AI_Hero Sender;

        /// <summary>
        /// The skill type
        /// </summary>
        public CGapcloserTypes SkillType;

        /// <summary>
        /// The start
        /// </summary>
        public Vector3 Start;

        /// <summary>
        /// The tick count
        /// </summary>
        public int TickCount;

        /// <summary>
        /// The slot
        /// </summary>
        public SpellSlot Slot;

        /// <summary>
        /// The spell name
        /// </summary>
        public string SpellName;
    }

    /// <summary>
    /// Provides events and information about Customizable AntiGapcloser.
    /// </summary>
    public static class CustomizableAntiGapcloser
    {
        /// <summary>
        /// The customizable anti gapcloser menu
        /// </summary>
        public static Menu CustomizableCustomizableAntiGapcloserMenu;

        /// <summary>
        /// The Gapcloser spells
        /// </summary>
        public static List<CGapcloser> Spells = new List<CGapcloser>();

        /// <summary>
        /// The active Gapclosers
        /// </summary>
        public static List<CActiveCGapcloser> CActiveCGapclosers = new List<CActiveCGapcloser>();

        /// <summary>
        /// Initializes static members of the <see cref="CustomizableAntiGapcloser"/> class. 
        /// </summary>
        static CustomizableAntiGapcloser()
        {
                        #region Aatrox

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Aatrox",
                    Slot = SpellSlot.Q,
                    SpellName = "aatroxq",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 1

                });

            #endregion

            #region Akali

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Akali",
                    Slot = SpellSlot.R,
                    SpellName = "akalishadowdance",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 5,
                });

            #endregion

            #region Alistar

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Alistar",
                    Slot = SpellSlot.W,
                    SpellName = "headbutt",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 5,
                });

            #endregion

            #region Corki

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Corki",
                    Slot = SpellSlot.W,
                    SpellName = "carpetbomb",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 1,
                });

            #endregion

            #region Diana

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Diana",
                    Slot = SpellSlot.R,
                    SpellName = "dianateleport",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 4,
                });

            #endregion

            #region Ekko
            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Ekko",
                    Slot = SpellSlot.E,
                    SpellName = "ekkoe",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 4,
                });

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Ekko",
                    Slot = SpellSlot.E,
                    SpellName = "ekkoeattack",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 4,
                });
            #endregion

            #region Elise

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Elise",
                    Slot = SpellSlot.Q,
                    SpellName = "elisespiderqcast",
                    SkillType = CGapcloserTypes.Skillshot
                });

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Elise",
                    Slot = SpellSlot.Q,
                    SpellName = "elisespiderqcast",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 1,
                });

            #endregion

            #region Fiora

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Fiora",
                    Slot = SpellSlot.Q,
                    SpellName = "fioraq",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 1,
                });

            #endregion

            #region Fizz

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Fizz",
                    Slot = SpellSlot.Q,
                    SpellName = "fizzpiercingstrike",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 3,
                });

            #endregion

            #region Gnar

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Gnar",
                    Slot = SpellSlot.E,
                    SpellName = "gnarbige",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 1,
                });

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Gnar",
                    Slot = SpellSlot.E,
                    SpellName = "gnare",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 1,
                });

            #endregion

            #region Gragas

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Gragas",
                    Slot = SpellSlot.E,
                    SpellName = "gragase",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 2,
                });

            #endregion

            #region Graves

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Graves",
                    Slot = SpellSlot.E,
                    SpellName = "gravesmove",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 4,
                });

            #endregion

            #region Hecarim

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Hecarim",
                    Slot = SpellSlot.R,
                    SpellName = "hecarimult",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 5,
                });

            #endregion

            #region Illaoi
            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Illaoi",
                    Slot = SpellSlot.W,
                    SpellName = "illaoiwattack",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 3,
                });
            #endregion


            #region Irelia

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Irelia",
                    Slot = SpellSlot.Q,
                    SpellName = "ireliagatotsu",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 5,
                });

            #endregion

            #region JarvanIV

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "JarvanIV",
                    Slot = SpellSlot.Q,
                    SpellName = "jarvanivdragonstrike",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 2,
                });

            #endregion

            #region Jax

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Jax",
                    Slot = SpellSlot.Q,
                    SpellName = "jaxleapstrike",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 5,
                });

            #endregion

            #region Jayce

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Jayce",
                    Slot = SpellSlot.Q,
                    SpellName = "jaycetotheskies",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 3,
                });

            #endregion

            #region Kassadin

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Kassadin",
                    Slot = SpellSlot.R,
                    SpellName = "riftwalk",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 4
                });

            #endregion

            #region Khazix

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Khazix",
                    Slot = SpellSlot.E,
                    SpellName = "khazixe",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 3,
                });

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Khazix",
                    Slot = SpellSlot.E,
                    SpellName = "khazixelong",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 3,
                });

            #endregion

            #region LeBlanc

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "LeBlanc",
                    Slot = SpellSlot.W,
                    SpellName = "leblancslide",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 5,
                });

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "LeBlanc",
                    Slot = SpellSlot.R,
                    SpellName = "leblancslidem",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 5,
                });

            #endregion

            #region LeeSin

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "LeeSin",
                    Slot = SpellSlot.Q,
                    SpellName = "blindmonkqtwo",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 3,
                });

            #endregion

            #region Leona

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Leona",
                    Slot = SpellSlot.E,
                    SpellName = "leonazenithblade",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 5
                });

            #endregion

            #region Lucian

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Lucian",
                    Slot = SpellSlot.E,
                    SpellName = "luciane",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 4,
                });

            #endregion

            #region Malphite

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Malphite",
                    Slot = SpellSlot.R,
                    SpellName = "ufslash",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 5
                });

            #endregion

            #region MasterYi

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "MasterYi",
                    Slot = SpellSlot.Q,
                    SpellName = "alphastrike",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 4
                });

            #endregion

            #region MonkeyKing

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "MonkeyKing",
                    Slot = SpellSlot.E,
                    SpellName = "monkeykingnimbus",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 4
                });

            #endregion

            #region Pantheon

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Pantheon",
                    Slot = SpellSlot.W,
                    SpellName = "pantheon_leapbash",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 5
                });

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Pantheon",
                    Slot = SpellSlot.R,
                    SpellName = "pantheonrjump",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 4
                });

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Pantheon",
                    Slot = SpellSlot.R,
                    SpellName = "pantheonrfall",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 4
                });

            #endregion

            #region Poppy

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Poppy",
                    Slot = SpellSlot.E,
                    SpellName = "poppyheroiccharge",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 4
                });

            #endregion

            #region Renekton

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Renekton",
                    Slot = SpellSlot.E,
                    SpellName = "renektonsliceanddice",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 4,
                });

            #endregion

            #region Riven

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Riven",
                    Slot = SpellSlot.Q,
                    SpellName = "riventricleave",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 4,
                });

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Riven",
                    Slot = SpellSlot.E,
                    SpellName = "rivenfeint",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 4,
                });

            #endregion

            #region Sejuani

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Sejuani",
                    Slot = SpellSlot.Q,
                    SpellName = "sejuaniarcticassault",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 4,
                });

            #endregion

            #region Shen

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Shen",
                    Slot = SpellSlot.E,
                    SpellName = "shenshadowdash",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 5,
                });

            #endregion

            #region Shyvana

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Shyvana",
                    Slot = SpellSlot.R,
                    SpellName = "shyvanatransformcast",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 5,
                });

            #endregion

            #region Talon

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Talon",
                    Slot = SpellSlot.E,
                    SpellName = "taloncutthroat",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 4
                });

            #endregion

            #region Tristana

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Tristana",
                    Slot = SpellSlot.W,
                    SpellName = "rocketjump",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 4,
                });

            #endregion

            #region Tryndamere

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Tryndamere",
                    Slot = SpellSlot.E,
                    SpellName = "slashcast",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 2,
                });

            #endregion

            #region Vi

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Vi",
                    Slot = SpellSlot.Q,
                    SpellName = "ViQ",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 5,
                });

            #endregion

            #region XinZhao

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "XinZhao",
                    Slot = SpellSlot.E,
                    SpellName = "xenzhaosweep",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 4,
                });

            #endregion

            #region Yasuo

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Yasuo",
                    Slot = SpellSlot.E,
                    SpellName = "yasuodashwrapper",
                    SkillType = CGapcloserTypes.Targeted,
                    DangerLevel = 4,
                });

            #endregion

            #region Zac

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Zac",
                    Slot = SpellSlot.E,
                    SpellName = "zace",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 3,
                });

            #endregion

            #region Ziggs

            Spells.Add(
                new CGapcloser
                {
                    ChampionName = "Ziggs",
                    Slot = SpellSlot.W,
                    SpellName = "ziggswtoggle",
                    SkillType = CGapcloserTypes.Skillshot,
                    DangerLevel = 4,
                });

            #endregion

            Game.OnUpdate += Game_OnGameUpdate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        /// <summary>
        /// Occurs on an incoming enemy Customizable AntiGapcloser.
        /// </summary>
        public static event OnAGapcloseH OnEnemyCustomGapcloser;

        /// <summary>
        /// Customizable menu of the Anti Gapcloser
        /// </summary>
        /// <param name="mainmenu"></param>
        public static void AddToMenu(Menu mainmenu)
        {
            CustomizableCustomizableAntiGapcloserMenu = mainmenu;
            foreach (var gapclose in Spells.Where(x => ObjectManager.Get<Obj_AI_Hero>().Any(y => y.ChampionName == x.ChampionName && y.IsEnemy)))
            {
                mainmenu.AddItem(new MenuItem("gapclose." + gapclose.ChampionName, "Anti-Gapclose: " + gapclose.ChampionName + " - Spell: " + gapclose.Slot).SetValue(true));
                mainmenu.AddItem(new MenuItem("gapclose.slider." + gapclose.SpellName, "" + gapclose.ChampionName + " " + "(" + gapclose.Slot + ") Priority").SetValue(new Slider(gapclose.DangerLevel, 1, 5)));
            }
        }

        /// <summary>
        /// Fired when the game updates.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void Game_OnGameUpdate(EventArgs args)
        {
            CActiveCGapclosers.RemoveAll(entry => Utils.TickCount > entry.TickCount + 900);
            if (OnEnemyCustomGapcloser == null)
            {
                return;
            }

            foreach (
                var cGapcloser in
                    CActiveCGapclosers.Where(cGapcloser => cGapcloser.Sender.IsValidTarget())
                        .Where(
                            cGapcloser =>
                                cGapcloser.SkillType == CGapcloserTypes.Targeted ||
                                (cGapcloser.SkillType == CGapcloserTypes.Skillshot &&
                                 ObjectManager.Player.Distance(cGapcloser.Sender, true) < 250000) // 500 * 500
                                 && CustomizableCustomizableAntiGapcloserMenu.Item("gapclose." + cGapcloser.Sender.ChampionName).GetValue<bool>())
                                 .OrderByDescending(c => CustomizableCustomizableAntiGapcloserMenu.Item("gapclose.slider." + c.SpellName).GetValue<Slider>().Value))
            {
                OnEnemyCustomGapcloser(cGapcloser);
            }
        }

        /// <summary>
        /// Fired when the game processes a spell cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!SpellIsCGapcloser(args))
            {
                return;
            }

            CActiveCGapclosers.Add(
                new CActiveCGapcloser
                {
                    Start = args.Start,
                    End = args.End,
                    Sender = (Obj_AI_Hero)sender,
                    TickCount = Utils.TickCount,
                    SkillType = (args.Target != null && args.Target.IsMe) ? CGapcloserTypes.Targeted : CGapcloserTypes.Skillshot,
                    Slot = ((Obj_AI_Hero)sender).GetSpellSlot(args.SData.Name),
                    SpellName = args.SData.Name
                });
        }

        /// <summary>
        /// Checks if a spell is a Customizable AntiGapcloser.
        /// </summary>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        private static bool SpellIsCGapcloser(GameObjectProcessSpellCastEventArgs args)
        {
            return Spells.Any(spell => spell.SpellName == args.SData.Name.ToLower());
        }
    }
}
