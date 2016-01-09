#region LICENSE
/*
 Copyright 2014 - 2014 LeagueSharp
 AntiGapcloser.cs is part of LeagueSharp.Common.
 
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
    /// The delegate for the <see cref="AntiGapcloser.OnEnemyGapcloser"/> event.
    /// </summary>
    /// <param name="gapcloser">The gapcloser.</param>
    public delegate void OnGapcloseH(ActiveGapcloser gapcloser);

    /// <summary>
    /// The type of gapcloser.
    /// </summary>
    public enum GapcloserType
    {
        /// <summary>
        /// The gapcloser used a skillshot ability.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        Skillshot,

        /// <summary>
        /// The gapcloser used a targeted ability.
        /// </summary>
        Targeted
    }

    /// <summary>
    /// Represents a gapcloser.
    /// </summary>
    public class Gapcloser
    {
        /// <summary>
        /// Wehther the skill is enabled or not
        /// </summary>
        public bool Enabled = true;

        /// <summary>
        /// The champion name
        /// </summary>
        public string ChampionName;

        /// <summary>
        /// The skill type
        /// </summary>
        public GapcloserType SkillType;

        /// <summary>
        /// The slot
        /// </summary>
        public SpellSlot Slot;

        /// <summary>
        /// The spell name
        /// </summary>
        public string SpellName;

        /// <summary>
        /// A function that calculates the gapcloser's range.
        /// </summary>
        public Func<Obj_AI_Base, float> RangeCalc;

        /// <summary>
        /// Whether the gapcloser is immune to crowd control.
        /// </summary>
        public bool CrowdControlImmune;
    }

    /// <summary>
    /// Represents an active gapcloser.
    /// </summary>
    public struct ActiveGapcloser
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
        public GapcloserType SkillType;

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
    }

    /// <summary>
    /// Provides events and information about gapclosers.
    /// </summary>
    public static class AntiGapcloser
    {
        /// <summary>
        /// The gapcloser spells
        /// </summary>
        public static List<Gapcloser> Spells = new List<Gapcloser>();

        /// <summary>
        /// The active gapclosers
        /// </summary>
        public static List<ActiveGapcloser> ActiveGapclosers = new List<ActiveGapcloser>();

        /// <summary>
        /// Initializes static members of the <see cref="AntiGapcloser"/> class. 
        /// </summary>
        static AntiGapcloser()
        {
            #region Aatrox

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Aatrox",
                    Slot = SpellSlot.Q,
                    SpellName = "aatroxq",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 650f
                });

            #endregion

            #region Akali

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Akali",
                    Slot = SpellSlot.R,
                    SpellName = "akalishadowdance",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Alistar

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Alistar",
                    Slot = SpellSlot.W,
                    SpellName = "headbutt",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Corki

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Corki",
                    Slot = SpellSlot.W,
                    SpellName = "carpetbomb",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 600f
                });

            #endregion

            #region Diana

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Diana",
                    Slot = SpellSlot.R,
                    SpellName = "dianateleport",
                    SkillType = GapcloserType.Targeted
                });

            #endregion
            
            #region Ekko
            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Ekko",
                    Slot = SpellSlot.E,
                    SpellName = "ekkoe",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 325f
                });
	
            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Ekko",
                    Slot = SpellSlot.E,
                    SpellName = "ekkoeattack",
                    SkillType = GapcloserType.Targeted
                });
            #endregion

            #region Elise

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Elise",
                    Slot = SpellSlot.Q,
                    SpellName = "elisespiderqcast",
                    SkillType = GapcloserType.Targeted
                });
                
            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Elise",
                    Slot = SpellSlot.E,
                    SpellName = "elisespideredescent",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Fiora

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Fiora",
                    Slot = SpellSlot.Q,
                    SpellName = "fioraq",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region Fizz

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Fizz",
                    Slot = SpellSlot.Q,
                    SpellName = "fizzpiercingstrike",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Gnar

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Gnar",
                    Slot = SpellSlot.E,
                    SpellName = "gnarbige",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 475f
                });

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Gnar",
                    Slot = SpellSlot.E,
                    SpellName = "gnare",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 475f
                });

            #endregion

            #region Gragas

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Gragas",
                    Slot = SpellSlot.E,
                    SpellName = "gragase",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 600f
                });

            #endregion

            #region Graves

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Graves",
                    Slot = SpellSlot.E,
                    SpellName = "gravesmove",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 425f
                });

            #endregion

            #region Hecarim

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Hecarim",
                    Slot = SpellSlot.R,
                    SpellName = "hecarimult",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 1000f,
                    CrowdControlImmune = true
                });

            #endregion
            
            #region Illaoi
            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Illaoi",
                    Slot = SpellSlot.W,
                    SpellName = "illaoiwattack",
                    SkillType = GapcloserType.Targeted
                });
            #endregion
            
            
            #region Irelia

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Irelia",
                    Slot = SpellSlot.Q,
                    SpellName = "ireliagatotsu",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region JarvanIV

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "JarvanIV",
                    Slot = SpellSlot.Q,
                    SpellName = "jarvanivdragonstrike",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            #region Jax

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Jax",
                    Slot = SpellSlot.Q,
                    SpellName = "jaxleapstrike",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Jayce

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Jayce",
                    Slot = SpellSlot.Q,
                    SpellName = "jaycetotheskies",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Kassadin

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Kassadin",
                    Slot = SpellSlot.R,
                    SpellName = "riftwalk",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 500f
                });

            #endregion

            #region Khazix

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Khazix",
                    Slot = SpellSlot.E,
                    SpellName = "khazixe",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 600f
                });

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Khazix",
                    Slot = SpellSlot.E,
                    SpellName = "khazixelong",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 900f
                });

            #endregion

            #region LeBlanc

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "LeBlanc",
                    Slot = SpellSlot.W,
                    SpellName = "leblancslide",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 600f
                });

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "LeBlanc",
                    Slot = SpellSlot.R,
                    SpellName = "leblancslidem",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 600f
                });

            #endregion

            #region LeeSin

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "LeeSin",
                    Slot = SpellSlot.Q,
                    SpellName = "blindmonkqtwo",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Leona

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Leona",
                    Slot = SpellSlot.E,
                    SpellName = "leonazenithblade",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 875f
                });

            #endregion

            #region Lucian

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Lucian",
                    Slot = SpellSlot.E,
                    SpellName = "luciane",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 425f
                });

            #endregion

            #region Malphite

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Malphite",
                    Slot = SpellSlot.R,
                    SpellName = "ufslash",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 1000f,
                    CrowdControlImmune = true
                });

            #endregion

            #region MasterYi

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "MasterYi",
                    Slot = SpellSlot.Q,
                    SpellName = "alphastrike",
                    SkillType = GapcloserType.Targeted,
                    CrowdControlImmune = true
                });

            #endregion

            #region MonkeyKing

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "MonkeyKing",
                    Slot = SpellSlot.E,
                    SpellName = "monkeykingnimbus",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Pantheon

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Pantheon",
                    Slot = SpellSlot.W,
                    SpellName = "pantheon_leapbash",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Poppy

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Poppy",
                    Slot = SpellSlot.E,
                    SpellName = "poppyheroiccharge",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Renekton

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Renekton",
                    Slot = SpellSlot.E,
                    SpellName = "renektonsliceanddice",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 450f
                });

            #endregion

            #region Riven

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Riven",
                    Slot = SpellSlot.Q,
                    SpellName = "riventricleave",
                    SkillType = GapcloserType.Skillshot
                });

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Riven",
                    Slot = SpellSlot.E,
                    SpellName = "rivenfeint",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 325f
                });

            #endregion

            #region Sejuani

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Sejuani",
                    Slot = SpellSlot.Q,
                    SpellName = "sejuaniarcticassault",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 850f
                });

            #endregion

            #region Shen

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Shen",
                    Slot = SpellSlot.E,
                    SpellName = "shenshadowdash",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 600f
                });

            #endregion

            #region Shyvana

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Shyvana",
                    Slot = SpellSlot.R,
                    SpellName = "shyvanatransformcast",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 1000f
                });

            #endregion

            #region Talon

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Talon",
                    Slot = SpellSlot.E,
                    SpellName = "taloncutthroat",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Tristana

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Tristana",
                    Slot = SpellSlot.W,
                    SpellName = "rocketjump",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 900f
                });

            #endregion

            #region Tryndamere

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Tryndamere",
                    Slot = SpellSlot.E,
                    SpellName = "slashcast",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 660f
                });

            #endregion

            #region Vi

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Vi",
                    Slot = SpellSlot.Q,
                    SpellName = "viq",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 725f
                });

            #endregion

            #region XinZhao

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "XinZhao",
                    Slot = SpellSlot.E,
                    SpellName = "xenzhaosweep",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Yasuo

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Yasuo",
                    Slot = SpellSlot.E,
                    SpellName = "yasuodashwrapper",
                    SkillType = GapcloserType.Targeted
                });

            #endregion

            #region Zac

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Zac",
                    Slot = SpellSlot.E,
                    SpellName = "zace",
                    SkillType = GapcloserType.Skillshot,
                    RangeCalc = @base => 1200f + ((@base.Spellbook.GetSpell(SpellSlot.E).Level - 1) * 150f)
                });

            #endregion

            #region Ziggs

            Spells.Add(
                new Gapcloser
                {
                    ChampionName = "Ziggs",
                    Slot = SpellSlot.W,
                    SpellName = "ziggswtoggle",
                    SkillType = GapcloserType.Skillshot
                });

            #endregion

            Game.OnUpdate += Game_OnGameUpdate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        /// <summary>
        /// Occurs on an incoming enemy gapcloser.
        /// </summary>
        public static event OnGapcloseH OnEnemyGapcloser;

        /// <summary>
        /// Initializes the Common menu
        /// </summary>
        public static void Initialize()
        {
            Menu gapCloseMenu = new Menu("Anti Gapcloser", "AntiGaploser");
            foreach (Gapcloser gapcloser in Spells.Where(
                gapcloser => HeroManager.Enemies.Any(
                    hero => hero.Name.Equals(gapcloser.ChampionName))))
            {
                MenuItem item = new MenuItem($"{gapcloser.ChampionName}{gapcloser.SpellName}",
                    $"{gapcloser.ChampionName} {gapcloser.Slot}").SetShared().SetValue(gapcloser.Enabled);

                item.ValueChanged += (sender, args) => {
                    gapcloser.Enabled = args.GetNewValue<bool>();
                };
            }
            CommonMenu.Instance.AddSubMenu(gapCloseMenu);
        }

        /// <summary>
        /// Fired when the game updates.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void Game_OnGameUpdate(EventArgs args)
        {
            ActiveGapclosers.RemoveAll(entry => Utils.TickCount > (entry.TickCount  + 900));
            if (OnEnemyGapcloser == null)
            {
                return;
            }

            foreach (
                var gapcloser in
                    ActiveGapclosers.Where(gapcloser => gapcloser.Sender.IsValidTarget())
                        .Where(
                            gapcloser =>
                                (gapcloser.SkillType == GapcloserType.Targeted) ||
                                ((gapcloser.SkillType == GapcloserType.Skillshot) &&
                                 (ObjectManager.Player.Distance(gapcloser.Sender) < 500)))) // why make it harder for ppl to understand the code for a negligable perofrmance boost?
            {
                OnEnemyGapcloser(gapcloser);
            }
        }

        /// <summary>
        /// Fired when the game processes a spell cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            Gapcloser gapcloser = Spells.FirstOrDefault(
                spell => (spell != null) && (spell.SpellName == args.SData.Name.ToLower()));
            if ((gapcloser == null) || !gapcloser.Enabled || gapcloser.CrowdControlImmune)
            {
                return;
            }

            Vector3 endPos = args.End;
            float range = gapcloser.RangeCalc(sender);

            if ((gapcloser.SkillType == GapcloserType.Skillshot)
                && (range > default(float))
                && (args.Start.Distance(args.End) > range)) {
                endPos = (args.End - args.Start).Normalized() * range;
            }

            ActiveGapclosers.Add(
                new ActiveGapcloser
                {
                    Start = args.Start,
                    End = endPos,
                    Sender = (Obj_AI_Hero)sender,
                    TickCount = Utils.TickCount,
                    SkillType = ((args.Target != null) && args.Target.IsMe) ? GapcloserType.Targeted : GapcloserType.Skillshot,
                    Slot = ((Obj_AI_Hero)sender).GetSpellSlot(args.SData.Name)
                });
        }
    }
}
