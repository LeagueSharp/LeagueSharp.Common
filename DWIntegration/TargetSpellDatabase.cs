using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueSharp.Common.DWIntegration
{
    public static class TargetSpellDatabase
    {
        public static List<TargetSpellData> Spells;

        static TargetSpellDatabase()
        {
            Spells = new List<TargetSpellData>
            {
                #region Aatrox
                new TargetSpellData("aatrox", "aatroxq", SpellSlot.Q, SpellType.Skillshot, CcType.Knockup, 650, 500, 20),
                new TargetSpellData("aatrox", "aatroxw", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("aatrox", "aatroxw2", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("aatrox", "aatroxe", SpellSlot.E, SpellType.Skillshot, CcType.Stun, 1000, 500, 1200),
                new TargetSpellData("aatrox", "aatroxr", SpellSlot.R, SpellType.Self, CcType.No, 550, 0, 0),

                #endregion Aatrox

                #region Ahri
                new TargetSpellData("ahri", "ahriorbofdeception", SpellSlot.Q, SpellType.Skillshot, CcType.No, 880, 500, 1100),
                new TargetSpellData("ahri", "ahrifoxfire", SpellSlot.W, SpellType.Self, CcType.No, 800, 0, 1800),
                new TargetSpellData("ahri", "ahriseduce", SpellSlot.E, SpellType.Skillshot, CcType.Charm, 975, 500, 1200),
                new TargetSpellData("ahri", "ahritumble", SpellSlot.R, SpellType.Skillshot, CcType.No, 450, 500, 2200),

                #endregion Ahri

                #region Akali
                new TargetSpellData("akali", "akalimota", SpellSlot.Q, SpellType.Targeted, CcType.No, 600, 650, 1000),
                new TargetSpellData("akali", "akalismokebomb", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 700, 500, 0),
                new TargetSpellData("akali", "akalishadowswipe", SpellSlot.E, SpellType.Self, CcType.No, 325, 0, 0),
                new TargetSpellData("akali", "akalishadowdance", SpellSlot.R, SpellType.Targeted, CcType.No, 800, 0, 2200),

                #endregion Akali

                #region Alistar
                new TargetSpellData("alistar", "pulverize", SpellSlot.Q, SpellType.Self, CcType.Knockup, 365, 500, 20),
                new TargetSpellData("alistar", "headbutt", SpellSlot.W, SpellType.Targeted, CcType.Knockback, 650, 500, 0),
                new TargetSpellData("alistar", "triumphantroar", SpellSlot.E, SpellType.Self, CcType.No, 575, 0, 0),
                new TargetSpellData("alistar", "feroucioushowl", SpellSlot.R, SpellType.Self, CcType.No, 0, 0, 828),

                #endregion Alistar

                #region Amumu
                new TargetSpellData("amumu", "bandagetoss", SpellSlot.Q, SpellType.Skillshot, CcType.Stun, 1100, 500, 2000),
                new TargetSpellData("amumu", "auraofdespair", SpellSlot.W, SpellType.Self, CcType.No, 300, 470, float.MaxValue),
                new TargetSpellData("amumu", "tantrum", SpellSlot.E, SpellType.Self, CcType.No, 350, 500, float.MaxValue),
                new TargetSpellData("amumu", "curseofthesadmummy", SpellSlot.R, SpellType.Self, CcType.Stun, 550, 500, float.MaxValue),

                #endregion Amumu

                #region Anivia
                new TargetSpellData("anivia", "flashfrost", SpellSlot.Q, SpellType.Skillshot, CcType.Stun, 1200, 500, 850),
                new TargetSpellData("anivia", "crystalize", SpellSlot.W, SpellType.Skillshot, CcType.No, 1000, 500, 1600),
                new TargetSpellData("anivia", "frostbite", SpellSlot.E, SpellType.Targeted, CcType.No, 650, 500, 1200),
                new TargetSpellData("anivia", "glacialstorm", SpellSlot.R, SpellType.Skillshot, CcType.Slow, 675, 300, float.MaxValue),

                #endregion Anivia

                #region Annie
                new TargetSpellData("annie", "disintegrate", SpellSlot.Q, SpellType.Targeted, CcType.No, 623, 500, 1400),
                new TargetSpellData("annie", "incinerate", SpellSlot.W, SpellType.Targeted, CcType.No, 623, 500, 0),
                new TargetSpellData("annie", "moltenshield", SpellSlot.E, SpellType.Self, CcType.No, 100, 0, 20),
                new TargetSpellData("annie", "infernalguardian", SpellSlot.R, SpellType.Skillshot, CcType.No, 600, 500, float.MaxValue),

                #endregion Annie

                #region Ashe
                new TargetSpellData("ashe", "frostshot", SpellSlot.Q, SpellType.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("ashe", "frostarrow", SpellSlot.Q, SpellType.Targeted, CcType.Slow, 0, 0, float.MaxValue),
                new TargetSpellData("ashe", "volley", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1200, 500, 902),
                new TargetSpellData("ashe", "ashespiritofthehawk", SpellSlot.E, SpellType.Skillshot, CcType.No, 2500, 500, 1400),
                new TargetSpellData("ashe", "enchantedcrystalarrow", SpellSlot.R, SpellType.Skillshot, CcType.Stun, 50000, 500, 1600),

                #endregion Ashe

                #region Blitzcrank
                new TargetSpellData("blitzcrank", "rocketgrabmissile", SpellSlot.Q, SpellType.Skillshot, CcType.Pull, 925, 220, 1800),
                new TargetSpellData("blitzcrank", "overdrive", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("blitzcrank", "powerfist", SpellSlot.E, SpellType.Self, CcType.Knockup, 0, 0, 0),
                new TargetSpellData("blitzcrank", "staticfield", SpellSlot.R, SpellType.Self, CcType.Silence, 600, 0, 0),

                #endregion Blitzcrank

                #region Brand
                new TargetSpellData("brand", "brandblaze", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1150, 500, 1200),
                new TargetSpellData("brand", "brandfissure", SpellSlot.W, SpellType.Skillshot, CcType.No, 240, 500, 20),
                new TargetSpellData("brand", "brandconflagration", SpellSlot.E, SpellType.Targeted, CcType.No, 0, 0, 1800),
                new TargetSpellData("brand", "brandwildfire", SpellSlot.R, SpellType.Targeted, CcType.No, 0, 0, 1000),

                #endregion Brand

                #region Braum
                new TargetSpellData("braum", "braumq", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 1100, 500, 1200),
                new TargetSpellData("braum", "braumqmissle", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 1100, 500, 1200),
                new TargetSpellData("braum", "braumw", SpellSlot.W, SpellType.Targeted, CcType.No, 650, 500, 1500),
                new TargetSpellData("braum", "braume", SpellSlot.E, SpellType.Skillshot, CcType.No, 250, 0, float.MaxValue),
                new TargetSpellData("braum", "braumr", SpellSlot.R, SpellType.Skillshot, CcType.Knockup, 1250, 0, 1200),

                #endregion Braum

                #region Caitlyn
                new TargetSpellData("caitlyn", "caitlynpiltoverpeacemaker", SpellSlot.Q, SpellType.Skillshot, CcType.No, 2000, 250, 2200),
                new TargetSpellData("caitlyn", "caitlynyordletrap", SpellSlot.W, SpellType.Skillshot, CcType.Snare, 800, 0, 1400),
                new TargetSpellData("caitlyn", "caitlynentrapment", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 950, 250, 2000),
                new TargetSpellData("caitlyn", "caitlynaceinthehole", SpellSlot.R, SpellType.Targeted, CcType.No, 2500, 0, 1500),

                #endregion Caitlyn

                #region Cassiopeia
                new TargetSpellData("cassiopeia", "cassiopeianoxiousblast", SpellSlot.Q, SpellType.Skillshot, CcType.No, 925, 250, float.MaxValue),
                new TargetSpellData("cassiopeia", "cassiopeiamiasma", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 925, 500, 2500),
                new TargetSpellData("cassiopeia", "cassiopeiatwinfang", SpellSlot.E, SpellType.Targeted, CcType.No, 700, 0, 1900),
                new TargetSpellData("cassiopeia", "cassiopeiapetrifyinggaze", SpellSlot.R, SpellType.Skillshot, CcType.Stun, 875, 500, float.MaxValue),

                #endregion Cassiopeia

                #region Chogath
                new TargetSpellData("chogath", "rupture", SpellSlot.Q, SpellType.Skillshot, CcType.Knockup, 1000, 500, float.MaxValue),
                new TargetSpellData("chogath", "feralscream", SpellSlot.W, SpellType.Skillshot, CcType.Silence, 675, 250, float.MaxValue),
                new TargetSpellData("chogath", "vorpalspikes", SpellSlot.E, SpellType.Targeted, CcType.No, 0, 0, 347),
                new TargetSpellData("chogath", "feast", SpellSlot.R, SpellType.Targeted, CcType.No, 230, 0, 500),

                #endregion Chogath

                #region Corki
                new TargetSpellData("corki", "phosphorusbomb", SpellSlot.Q, SpellType.Skillshot, CcType.No, 875, 0, float.MaxValue),
                new TargetSpellData("corki", "carpetbomb", SpellSlot.W, SpellType.Skillshot, CcType.No, 875, 0, 700),
                new TargetSpellData("corki", "ggun", SpellSlot.E, SpellType.Skillshot, CcType.No, 750, 0, 902),
                new TargetSpellData("corki", "missilebarrage", SpellSlot.R, SpellType.Skillshot, CcType.No, 1225, 250, 828),

                #endregion Corki

                #region Darius
                new TargetSpellData("darius", "dariuscleave", SpellSlot.Q, SpellType.Skillshot, CcType.No, 425, 500, 0),
                new TargetSpellData("darius", "dariusnoxiantacticsonh", SpellSlot.W, SpellType.Self, CcType.Slow, 210, 0, 0),
                new TargetSpellData("darius", "dariusaxegrabcone", SpellSlot.E, SpellType.Skillshot, CcType.Pull, 540, 500, 1500),
                new TargetSpellData("darius", "dariusexecute", SpellSlot.R, SpellType.Targeted, CcType.No, 460, 500, 20),

                #endregion Darius

                #region Diana
                new TargetSpellData("diana", "dianaarc", SpellSlot.Q, SpellType.Skillshot, CcType.No, 900, 500, 1500),
                new TargetSpellData("diana", "dianaorbs", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("diana", "dianavortex", SpellSlot.E, SpellType.Self, CcType.Pull, 300, 500, 1500),
                new TargetSpellData("diana", "dianateleport", SpellSlot.R, SpellType.Targeted, CcType.No, 800, 500, 1500),

                #endregion Diana

                #region Draven
                new TargetSpellData("draven", "dravenspinning", SpellSlot.Q, SpellType.Self, CcType.No, 0, float.MaxValue, float.MaxValue),
                new TargetSpellData("draven", "dravenfury", SpellSlot.W, SpellType.Self, CcType.No, 0, float.MaxValue, float.MaxValue),
                new TargetSpellData("draven", "dravendoubleshot", SpellSlot.E, SpellType.Skillshot, CcType.Knockback, 1050, 500, 1600),
                new TargetSpellData("draven", "dravenrcast", SpellSlot.R, SpellType.Skillshot, CcType.No, 20000, 500, 2000),

                #endregion Draven

                #region DrMundo
                new TargetSpellData("drmundo", "infectedcleavermissilecast", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 1000, 500, 1500),
                new TargetSpellData("drmundo", "burningagony", SpellSlot.W, SpellType.Self, CcType.No, 225, float.MaxValue, float.MaxValue),
                new TargetSpellData("drmundo", "masochism", SpellSlot.E, SpellType.Self, CcType.No, 0, float.MaxValue, float.MaxValue),
                new TargetSpellData("drmundo", "sadism", SpellSlot.R, SpellType.Self, CcType.No, 0, float.MaxValue, float.MaxValue),

                #endregion DrMundo

                #region Elise
                new TargetSpellData("elise", "elisehumanq", SpellSlot.Q, SpellType.Targeted, CcType.No, 625, 750, 2200),
                new TargetSpellData("elise", "elisespiderqcast", SpellSlot.Q, SpellType.Targeted, CcType.No, 475, 500, float.MaxValue),
                new TargetSpellData("elise", "elisehumanw", SpellSlot.W, SpellType.Skillshot, CcType.No, 950, 750, 5000),
                new TargetSpellData("elise", "elisespiderw", SpellSlot.W, SpellType.Self, CcType.No, 0, float.MaxValue, float.MaxValue),
                new TargetSpellData("elise", "elisehumane", SpellSlot.E, SpellType.Skillshot, CcType.Stun, 1075, 500, 1450),
                new TargetSpellData("elise", "elisespidereinitial", SpellSlot.E, SpellType.Targeted, CcType.No, 975, float.MaxValue, float.MaxValue),
                new TargetSpellData("elise", "elisespideredescent", SpellSlot.E, SpellType.Targeted, CcType.No, 975, float.MaxValue, float.MaxValue),
                new TargetSpellData("elise", "eliser", SpellSlot.R, SpellType.Self, CcType.No, 0, float.MaxValue, float.MaxValue),
                new TargetSpellData("elise", "elisespiderr", SpellSlot.R, SpellType.Self, CcType.No, 0, float.MaxValue, float.MaxValue),

                #endregion Elise

                #region Evelynn
                new TargetSpellData("evelynn", "evelynnq", SpellSlot.Q, SpellType.Self, CcType.No, 500, 500, float.MaxValue),
                new TargetSpellData("evelynn", "evelynnw", SpellSlot.W, SpellType.Self, CcType.No, 0, float.MaxValue, float.MaxValue),
                new TargetSpellData("evelynn", "evelynne", SpellSlot.E, SpellType.Targeted, CcType.No, 290, 500, 900),
                new TargetSpellData("evelynn", "evelynnr", SpellSlot.R, SpellType.Skillshot, CcType.Slow, 650, 500, 1300),

                #endregion Evelynn

                #region Ezreal
                new TargetSpellData("ezreal", "ezrealmysticshot", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1200, 250, 2000),
                new TargetSpellData("ezreal", "ezrealessenceflux", SpellSlot.W, SpellType.Skillshot, CcType.No, 1050, 250, 1600),
                new TargetSpellData("ezreal", "ezrealessencemissle", SpellSlot.W, SpellType.Skillshot, CcType.No, 1050, 250, 1600),
                new TargetSpellData("ezreal", "ezrealarcaneshift", SpellSlot.E, SpellType.Targeted, CcType.No, 475, 500, float.MaxValue),
                new TargetSpellData("ezreal", "ezrealtruehotbarrage", SpellSlot.R, SpellType.Skillshot, CcType.No, 20000, 1000, 2000),

                #endregion Ezreal

                #region FiddleSticks
                new TargetSpellData("fiddlesticks", "terrify", SpellSlot.Q, SpellType.Targeted, CcType.Fear, 575, 500, float.MaxValue),
                new TargetSpellData("fiddlesticks", "drain", SpellSlot.W, SpellType.Targeted, CcType.No, 575, 500, float.MaxValue),
                new TargetSpellData("fiddlesticks", "fiddlesticksdarkwind", SpellSlot.E, SpellType.Skillshot, CcType.Silence, 750, 500, 1100),
                new TargetSpellData("fiddlesticks", "crowstorm", SpellSlot.R, SpellType.Targeted, CcType.No, 800, 500, float.MaxValue),

                #endregion FiddleSticks

                #region Fiora
                new TargetSpellData("fiora", "fioraq", SpellSlot.Q, SpellType.Targeted, CcType.No, 300, 500, 2200),
                new TargetSpellData("fiora", "fiorariposte", SpellSlot.W, SpellType.Self, CcType.No, 100, 0, 0),
                new TargetSpellData("fiora", "fioraflurry", SpellSlot.E, SpellType.Self, CcType.No, 210, 0, 0),
                new TargetSpellData("fiora", "fioradance", SpellSlot.R, SpellType.Targeted, CcType.No, 210, 500, 0),

                #endregion Fiora

                #region Fizz
                new TargetSpellData("fizz", "fizzpiercingstrike", SpellSlot.Q, SpellType.Targeted, CcType.No, 550, 500, float.MaxValue),
                new TargetSpellData("fizz", "fizzseastonepassive", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, 0),
                new TargetSpellData("fizz", "fizzjump", SpellSlot.E, SpellType.Self, CcType.No, 400, 500, 1300),
                new TargetSpellData("fizz", "fizzjumptwo", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 400, 500, 1300),
                new TargetSpellData("fizz", "fizzmarinerdoom", SpellSlot.R, SpellType.Skillshot, CcType.Knockup, 1275, 500, 1200),

                #endregion Fizz

                #region Galio
                new TargetSpellData("galio", "galioresolutesmite", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 940, 500, 1300),
                new TargetSpellData("galio", "galiobulwark", SpellSlot.W, SpellType.Targeted, CcType.No, 800, 500, float.MaxValue),
                new TargetSpellData("galio", "galiorighteousgust", SpellSlot.E, SpellType.Skillshot, CcType.No, 1180, 500, 1200),
                new TargetSpellData("galio", "galioidolofdurand", SpellSlot.R, SpellType.Self, CcType.Taunt, 560, 500, float.MaxValue),

                #endregion Galio

                #region Gangplank
                new TargetSpellData("gangplank", "parley", SpellSlot.Q, SpellType.Targeted, CcType.No, 625, 500, 2000),
                new TargetSpellData("gangplank", "removescurvy", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("gangplank", "raisemorale", SpellSlot.E, SpellType.Self, CcType.No, 1300, 500, float.MaxValue),
                new TargetSpellData("gangplank", "cannonbarrage", SpellSlot.R, SpellType.Skillshot, CcType.Slow, 20000, 500, 500),

                #endregion Gangplank

                #region Garen
                new TargetSpellData("garen", "garenq", SpellSlot.Q, SpellType.Self, CcType.No, 0, 200, float.MaxValue),
                new TargetSpellData("garen", "garenw", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("garen", "garene", SpellSlot.E, SpellType.Self, CcType.No, 325, 0, 700),
                new TargetSpellData("garen", "garenr", SpellSlot.R, SpellType.Targeted, CcType.No, 400, 120, float.MaxValue),

                #endregion Garen

                #region Gragas
                new TargetSpellData("gragas", "gragasq", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 1100, 300, 1000),
                new TargetSpellData("gragas", "gragasqtoggle", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1100, 300, 1000),
                new TargetSpellData("gragas", "gragasw", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("gragas", "gragase", SpellSlot.E, SpellType.Skillshot, CcType.Knockback, 1100, 300, 1000),
                new TargetSpellData("gragas", "gragasr", SpellSlot.R, SpellType.Skillshot, CcType.Knockback, 1100, 300, 1000),

                #endregion Gragas

                #region Graves
                new TargetSpellData("graves", "gravesclustershot", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1100, 300, 902),
                new TargetSpellData("graves", "gravessmokegrenade", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1100, 300, 1650),
                new TargetSpellData("graves", "gravessmokegrenadeboom", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1100, 300, 1650),
                new TargetSpellData("graves", "gravesmove", SpellSlot.E, SpellType.Skillshot, CcType.No, 425, 300, 1000),
                new TargetSpellData("graves", "graveschargeshot", SpellSlot.R, SpellType.Skillshot, CcType.No, 1000, 500, 1200),

                #endregion Graves

                #region Hecarim
                new TargetSpellData("hecarim", "hecarimrapidslash", SpellSlot.Q, SpellType.Self, CcType.No, 350, 300, 1450),
                new TargetSpellData("hecarim", "hecarimw", SpellSlot.W, SpellType.Self, CcType.No, 525, 120, 828),
                new TargetSpellData("hecarim", "hecarimramp", SpellSlot.E, SpellType.Self, CcType.No, 0, float.MaxValue, float.MaxValue),
                new TargetSpellData("hecarim", "hecarimult", SpellSlot.R, SpellType.Skillshot, CcType.Fear, 1350, 500, 1200),

                #endregion Hecarim

                #region Heimerdinger
                new TargetSpellData("heimerdinger", "heimerdingerq", SpellSlot.Q, SpellType.Skillshot, CcType.No, 350, 500, float.MaxValue),
                new TargetSpellData("heimerdinger", "heimerdingerw", SpellSlot.W, SpellType.Skillshot, CcType.No, 1525, 500, 902),
                new TargetSpellData("heimerdinger", "heimerdingere", SpellSlot.E, SpellType.Skillshot, CcType.Stun, 970, 500, 2500),
                new TargetSpellData("heimerdinger", "heimerdingerr", SpellSlot.R, SpellType.Self, CcType.No, 0, 230, float.MaxValue),
                new TargetSpellData("heimerdinger", "heimerdingereult", SpellSlot.R, SpellType.Skillshot, CcType.Stun, 970, 230, float.MaxValue),

                #endregion Heimerdinger

                #region Irelia
                new TargetSpellData("irelia", "ireliagatotsu", SpellSlot.Q, SpellType.Targeted, CcType.No, 650, 0, 2200),
                new TargetSpellData("irelia", "ireliahitenstyle", SpellSlot.W, SpellType.Self, CcType.No, 0, 230, 347),
                new TargetSpellData("irelia", "ireliaequilibriumstrike", SpellSlot.E, SpellType.Targeted, CcType.Stun, 325, 500, float.MaxValue),
                new TargetSpellData("irelia", "ireliatranscendentblades", SpellSlot.R, SpellType.Skillshot, CcType.No, 1200, 500, 779),

                #endregion Irelia

                #region Janna
                new TargetSpellData("janna", "howlinggale", SpellSlot.Q, SpellType.Skillshot, CcType.Knockup, 1800, 0, float.MaxValue),
                new TargetSpellData("janna", "sowthewind", SpellSlot.W, SpellType.Targeted, CcType.Slow, 600, 500, 1600),
                new TargetSpellData("janna", "eyeofthestorm", SpellSlot.E, SpellType.Targeted, CcType.No, 800, 500, float.MaxValue),
                new TargetSpellData("janna", "reapthewhirlwind", SpellSlot.R, SpellType.Self, CcType.Knockback, 725, 500, 828),

                #endregion Janna

                #region JarvanIV
                new TargetSpellData("jarvaniv", "jarvanivdragonstrike", SpellSlot.Q, SpellType.Skillshot, CcType.No, 700, 500, float.MaxValue),
                new TargetSpellData("jarvaniv", "jarvanivgoldenaegis", SpellSlot.W, SpellType.Self, CcType.Slow, 300, 500, 0),
                new TargetSpellData("jarvaniv", "jarvanivdemacianstandard", SpellSlot.E, SpellType.Skillshot, CcType.No, 830, 500, float.MaxValue),
                new TargetSpellData("jarvaniv", "jarvanivcataclysm", SpellSlot.R, SpellType.Skillshot, CcType.No, 650, 500, 0),

                #endregion JarvanIV

                #region Jax
                new TargetSpellData("jax", "jaxleapstrike", SpellSlot.Q, SpellType.Targeted, CcType.No, 210, 500, 0),
                new TargetSpellData("jax", "jaxempowertwo", SpellSlot.W, SpellType.Targeted, CcType.No, 0, 500, 0),
                new TargetSpellData("jax", "jaxcounterstrike", SpellSlot.E, SpellType.Self, CcType.Stun, 425, 500, 1450),
                new TargetSpellData("jax", "jaxrelentlessasssault", SpellSlot.R, SpellType.Self, CcType.No, 0, 0, 0),

                #endregion Jax

                #region Jayce
                new TargetSpellData("jayce", "jaycetotheskies", SpellSlot.Q, SpellType.Targeted, CcType.Slow, 600, 500, float.MaxValue),
                new TargetSpellData("jayce", "jayceshockblast", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1050, 500, 1200),
                new TargetSpellData("jayce", "jaycestaticfield", SpellSlot.W, SpellType.Self, CcType.No, 285, 500, 1500),
                new TargetSpellData("jayce", "jaycehypercharge", SpellSlot.W, SpellType.Self, CcType.No, 0, 750, float.MaxValue),
                new TargetSpellData("jayce", "jaycethunderingblow", SpellSlot.E, SpellType.Targeted, CcType.Knockback, 300, 0, float.MaxValue),
                new TargetSpellData("jayce", "jayceaccelerationgate", SpellSlot.E, SpellType.Skillshot, CcType.No, 685, 500, 1600),
                new TargetSpellData("jayce", "jaycestancehtg", SpellSlot.R, SpellType.Self, CcType.No, 0, 750, float.MaxValue),
                new TargetSpellData("jayce", "jaycestancegth", SpellSlot.R, SpellType.Self, CcType.No, 0, 750, float.MaxValue),

                #endregion Jayce

                #region Jinx
                new TargetSpellData("jinx", "jinxq", SpellSlot.Q, SpellType.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("jinx", "jinxw", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1550, 500, 1200),
                new TargetSpellData("jinx", "jinxwmissle", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1550, 500, 1200),
                new TargetSpellData("jinx", "jinxe", SpellSlot.E, SpellType.Skillshot, CcType.Snare, 900, 500, 1000),
                new TargetSpellData("jinx", "jinxr", SpellSlot.R, SpellType.Skillshot, CcType.No, 25000, 0, float.MaxValue),
                new TargetSpellData("jinx", "jinxrwrapper", SpellSlot.R, SpellType.Skillshot, CcType.No, 25000, 0, float.MaxValue),

                #endregion Jinx

                #region Karma
                new TargetSpellData("karma", "karmaq", SpellSlot.Q, SpellType.Skillshot, CcType.No, 950, 500, 902),
                new TargetSpellData("karma", "karmaspiritbind", SpellSlot.W, SpellType.Targeted, CcType.Snare, 700, 500, 2000),
                new TargetSpellData("karma", "karmasolkimshield", SpellSlot.E, SpellType.Targeted, CcType.No, 800, 500, float.MaxValue),
                new TargetSpellData("karma", "karmamantra", SpellSlot.R, SpellType.Self, CcType.No, 0, 500, 1300),

                #endregion Karma

                #region Karthus
                new TargetSpellData("karthus", "laywaste", SpellSlot.Q, SpellType.Skillshot, CcType.No, 875, 500, float.MaxValue),
                new TargetSpellData("karthus", "wallofpain", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1090, 500, 1600),
                new TargetSpellData("karthus", "defile", SpellSlot.E, SpellType.Self, CcType.No, 550, 500, 1000),
                new TargetSpellData("karthus", "fallenone", SpellSlot.R, SpellType.Self, CcType.No, 20000, 0, float.MaxValue),

                #endregion Karthus

                #region Kassadin
                new TargetSpellData("kassadin", "nulllance", SpellSlot.Q, SpellType.Targeted, CcType.Silence, 650, 500, 1400),
                new TargetSpellData("kassadin", "netherblade", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("kassadin", "forcepulse", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 700, 500, float.MaxValue),
                new TargetSpellData("kassadin", "riftwalk", SpellSlot.R, SpellType.Skillshot, CcType.No, 675, 500, float.MaxValue),

                #endregion Kassadin

                #region Katarina
                new TargetSpellData("katarina", "katarinaq", SpellSlot.Q, SpellType.Targeted, CcType.No, 675, 500, 1800),
                new TargetSpellData("katarina", "katarinaw", SpellSlot.W, SpellType.Self, CcType.No, 400, 500, 1800),
                new TargetSpellData("katarina", "katarinae", SpellSlot.E, SpellType.Targeted, CcType.No, 700, 500, 0),
                new TargetSpellData("katarina", "katarinar", SpellSlot.R, SpellType.Self, CcType.No, 550, 500, 1450),

                #endregion Katarina

                #region Kayle
                new TargetSpellData("kayle", "judicatorreckoning", SpellSlot.Q, SpellType.Targeted, CcType.Slow, 650, 500, 1500),
                new TargetSpellData("kayle", "judicatordevineblessing", SpellSlot.W, SpellType.Targeted, CcType.No, 900, 220, float.MaxValue),
                new TargetSpellData("kayle", "judicatorrighteousfury", SpellSlot.E, SpellType.Self, CcType.No, 0, 500, 779),
                new TargetSpellData("kayle", "judicatorintervention", SpellSlot.R, SpellType.Targeted, CcType.No, 900, 500, float.MaxValue),

                #endregion Kayle

                #region Kennen
                new TargetSpellData("kennen", "kennenshurikenhurlmissile1", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1000, 690, 1700),
                new TargetSpellData("kennen", "kennenbringthelight", SpellSlot.W, SpellType.Self, CcType.No, 900, 500, float.MaxValue),
                new TargetSpellData("kennen", "kennenlightningrush", SpellSlot.E, SpellType.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("kennen", "kennenshurikenstorm", SpellSlot.R, SpellType.Self, CcType.No, 550, 500, 779),

                #endregion Kennen

                #region Khazix
                new TargetSpellData("khazix", "khazixq", SpellSlot.Q, SpellType.Targeted, CcType.No, 325, 500, float.MaxValue),
                new TargetSpellData("khazix", "khazixqlong", SpellSlot.Q, SpellType.Targeted, CcType.No, 375, 500, float.MaxValue),
                new TargetSpellData("khazix", "khazixw", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1000, 500, 828),
                new TargetSpellData("khazix", "khazixwlong", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1000, 500, 828),
                new TargetSpellData("khazix", "khazixe", SpellSlot.E, SpellType.Skillshot, CcType.No, 600, 500, float.MaxValue),
                new TargetSpellData("khazix", "khazixelong", SpellSlot.E, SpellType.Skillshot, CcType.No, 900, 500, float.MaxValue),
                new TargetSpellData("khazix", "khazixr", SpellSlot.R, SpellType.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("khazix", "khazixrlong", SpellSlot.R, SpellType.Self, CcType.No, 0, 0, float.MaxValue),

                #endregion Khazix

                #region KogMaw
                new TargetSpellData("kogmaw", "kogmawcausticspittle", SpellSlot.Q, SpellType.Targeted, CcType.No, 625, 500, float.MaxValue),
                new TargetSpellData("kogmaw", "kogmawbioarcanbarrage", SpellSlot.W, SpellType.Self, CcType.No, 130, 500, 2000),
                new TargetSpellData("kogmaw", "kogmawvoidooze", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1000, 500, 1200),
                new TargetSpellData("kogmaw", "kogmawlivingartillery", SpellSlot.R, SpellType.Skillshot, CcType.No, 1400, 600, 2000),

                #endregion KogMaw

                #region Leblanc
                new TargetSpellData("leblanc", "leblancchaosorb", SpellSlot.Q, SpellType.Targeted, CcType.No, 700, 500, 2000),
                new TargetSpellData("leblanc", "leblancslide", SpellSlot.W, SpellType.Skillshot, CcType.No, 600, 500, float.MaxValue),
                new TargetSpellData("leblanc", "leblacslidereturn", SpellSlot.W, SpellType.Skillshot, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("leblanc", "leblancsoulshackle", SpellSlot.E, SpellType.Skillshot, CcType.Snare, 925, 500, 1600),
                new TargetSpellData("leblanc", "leblancchaosorbm", SpellSlot.R, SpellType.Targeted, CcType.No, 700, 500, 2000),
                new TargetSpellData("leblanc", "leblancslidem", SpellSlot.R, SpellType.Skillshot, CcType.No, 600, 500, float.MaxValue),
                new TargetSpellData("leblanc", "leblancslidereturnm", SpellSlot.R, SpellType.Skillshot, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("leblanc", "leblancsoulshacklem", SpellSlot.R, SpellType.Skillshot, CcType.No, 925, 500, 1600),

                #endregion Leblanc

                #region LeeSin
                new TargetSpellData("leesin", "blindmonkqone", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1000, 500, 1800),
                new TargetSpellData("leesin", "blindmonkqtwo", SpellSlot.Q, SpellType.Targeted, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("leesin", "blindmonkwone", SpellSlot.W, SpellType.Targeted, CcType.No, 700, 0, 1500),
                new TargetSpellData("leesin", "blindmonkwtwo", SpellSlot.W, SpellType.Self, CcType.No, 700, 0, float.MaxValue),
                new TargetSpellData("leesin", "blindmonkeone", SpellSlot.E, SpellType.Self, CcType.No, 425, 500, float.MaxValue),
                new TargetSpellData("leesin", "blindmonketwo", SpellSlot.E, SpellType.Self, CcType.Slow, 425, 500, float.MaxValue),
                new TargetSpellData("leesin", "blindmonkrkick", SpellSlot.R, SpellType.Targeted, CcType.Knockback, 375, 500, 1500),

                #endregion LeeSin

                #region Leona
                new TargetSpellData("leona", "leonashieldofdaybreak", SpellSlot.Q, SpellType.Self, CcType.Stun, 215, 0, 0),
                new TargetSpellData("leona", "leonasolarbarrier", SpellSlot.W, SpellType.Self, CcType.No, 500, 3000, 0),
                new TargetSpellData("leona", "leonazenithblade", SpellSlot.E, SpellType.Skillshot, CcType.Stun, 900, 0, 2000),
                new TargetSpellData("leona", "leonazenithblademissle", SpellSlot.E, SpellType.Skillshot, CcType.Stun, 900, 0, 2000),
                new TargetSpellData("leona", "leonasolarflare", SpellSlot.R, SpellType.Skillshot, CcType.Stun, 1200, 700, float.MaxValue),

                #endregion Leona

                #region Lissandra
                new TargetSpellData("lissandra", "lissandraq", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 725, 500, 1200),
                new TargetSpellData("lissandra", "lissandraw", SpellSlot.W, SpellType.Self, CcType.Snare, 450, 500, float.MaxValue),
                new TargetSpellData("lissandra", "lissandrae", SpellSlot.E, SpellType.Skillshot, CcType.No, 1050, 500, 850),
                new TargetSpellData("lissandra", "lissandrar", SpellSlot.R, SpellType.Targeted, CcType.Stun, 550, 0, float.MaxValue),

                #endregion Lissandra

                #region Lucian
                new TargetSpellData("lucian", "lucianpassiveshot", SpellSlot.Unknown, SpellType.Targeted, CcType.No, 550, 500, 500),
                new TargetSpellData("lucian", "lucianq", SpellSlot.Q, SpellType.Targeted, CcType.No, 550, 500, 500),
                new TargetSpellData("lucian", "lucianw", SpellSlot.W, SpellType.Skillshot, CcType.No, 1000, 500, 500),
                new TargetSpellData("lucian", "luciane", SpellSlot.E, SpellType.Skillshot, CcType.No, 650, 500, float.MaxValue),
                new TargetSpellData("lucian", "lucianr", SpellSlot.R, SpellType.Targeted, CcType.No, 1400, 500, float.MaxValue),

                #endregion Lucian

                #region Lulu
                new TargetSpellData("lulu", "luluq", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 925, 500, 1400),
                new TargetSpellData("lulu", "luluqmissle", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 925, 500, 1400),
                new TargetSpellData("lulu", "luluw", SpellSlot.W, SpellType.Targeted, CcType.Polymorph, 650, 640, 2000),
                new TargetSpellData("lulu", "lulue", SpellSlot.E, SpellType.Targeted, CcType.No, 650, 640, float.MaxValue),
                new TargetSpellData("lulu", "lulur", SpellSlot.R, SpellType.Targeted, CcType.Knockup, 900, 500, float.MaxValue),

                #endregion Lulu

                #region Lux
                new TargetSpellData("lux", "luxlightbinding", SpellSlot.Q, SpellType.Skillshot, CcType.Snare, 1300, 500, 1200),
                new TargetSpellData("lux", "luxprismaticwave", SpellSlot.W, SpellType.Skillshot, CcType.No, 1075, 500, 1200),
                new TargetSpellData("lux", "luxlightstrikekugel", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1100, 500, 1300),
                new TargetSpellData("lux", "luxlightstriketoggle", SpellSlot.E, SpellType.Skillshot, CcType.No, 1100, 500, 1300),
                new TargetSpellData("lux", "luxmalicecannon", SpellSlot.R, SpellType.Skillshot, CcType.No, 3340, 1750, 3000),
                new TargetSpellData("lux", "luxmalicecannonmis", SpellSlot.R, SpellType.Skillshot, CcType.No, 3340, 1750, 3000),

                #endregion Lux

                #region Malphite
                new TargetSpellData("malphite", "seismicshard", SpellSlot.Q, SpellType.Targeted, CcType.Slow, 625, 500, 1200),
                new TargetSpellData("malphite", "obduracy", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("malphite", "landslide", SpellSlot.E, SpellType.Self, CcType.No, 400, 500, float.MaxValue),
                new TargetSpellData("malphite", "ufslash", SpellSlot.R, SpellType.Skillshot, CcType.Knockup, 1000, 0, 700),

                #endregion Malphite

                #region Malzahar
                new TargetSpellData("malzahar", "alzaharcallofthevoid", SpellSlot.Q, SpellType.Skillshot, CcType.Silence, 900, 500, float.MaxValue),
                new TargetSpellData("malzahar", "alzaharnullzone", SpellSlot.W, SpellType.Skillshot, CcType.No, 800, 500, float.MaxValue),
                new TargetSpellData("malzahar", "alzaharmaleficvisions", SpellSlot.E, SpellType.Targeted, CcType.No, 650, 500, float.MaxValue),
                new TargetSpellData("malzahar", "alzaharnethergrasp", SpellSlot.R, SpellType.Targeted, CcType.Suppression, 700, 500, float.MaxValue),

                #endregion Malzahar

                #region Maokai
                new TargetSpellData("maokai", "maokaitrunkline", SpellSlot.Q, SpellType.Skillshot, CcType.Knockback, 600, 500, 1200),
                new TargetSpellData("maokai", "maokaiunstablegrowth", SpellSlot.W, SpellType.Targeted, CcType.Snare, 650, 500, float.MaxValue),
                new TargetSpellData("maokai", "maokaisapling2", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1100, 500, 1750),
                new TargetSpellData("maokai", "maokaidrain3", SpellSlot.R, SpellType.Targeted, CcType.No, 625, 500, float.MaxValue),

                #endregion Maokai

                #region MasterYi
                new TargetSpellData("masteryi", "alphastrike", SpellSlot.Q, SpellType.Targeted, CcType.No, 600, 500, 4000),
                new TargetSpellData("masteryi", "meditate", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("masteryi", "wujustyle", SpellSlot.E, SpellType.Self, CcType.No, 0, 230, float.MaxValue),
                new TargetSpellData("masteryi", "highlander", SpellSlot.R, SpellType.Self, CcType.No, 0, 370, float.MaxValue),

                #endregion MasterYi

                #region MissFortune
                new TargetSpellData("missfortune", "missfortunericochetshot", SpellSlot.Q, SpellType.Targeted, CcType.No, 650, 500, 1400),
                new TargetSpellData("missfortune", "missfortuneviciousstrikes", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("missfortune", "missfortunescattershot", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1000, 500, 500),
                new TargetSpellData("missfortune", "missfortunebullettime", SpellSlot.R, SpellType.Skillshot, CcType.No, 1400, 500, 775),

                #endregion MissFortune

                #region MonkeyKing
                new TargetSpellData("monkeyking", "monkeykingdoubleattack", SpellSlot.Q, SpellType.Self, CcType.No, 300, 500, 20),
                new TargetSpellData("monkeyking", "monkeykingdecoy", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, 0),
                new TargetSpellData("monkeyking", "monkeykingdecoyswipe", SpellSlot.W, SpellType.Self, CcType.No, 325, 500, 0),
                new TargetSpellData("monkeyking", "monkeykingnimbus", SpellSlot.E, SpellType.Targeted, CcType.No, 625, 0, 2200),
                new TargetSpellData("monkeyking", "monkeykingspintowin", SpellSlot.R, SpellType.Self, CcType.Knockup, 315, 0, 700),
                new TargetSpellData("monkeyking", "monkeykingspintowinleave", SpellSlot.R, SpellType.Self, CcType.No, 0, 0, 700),

                #endregion MonkeyKing

                #region Mordekaiser
                new TargetSpellData("mordekaiser", "mordekaisermaceofspades", SpellSlot.Q, SpellType.Self, CcType.No, 600, 500, 1500),
                new TargetSpellData("mordekaiser", "mordekaisercreepindeathcast", SpellSlot.W, SpellType.Targeted, CcType.No, 750, 500, float.MaxValue),
                new TargetSpellData("mordekaiser", "mordekaisersyphoneofdestruction", SpellSlot.E, SpellType.Skillshot, CcType.No, 700, 500, 1500),
                new TargetSpellData("mordekaiser", "mordekaiserchildrenofthegrave", SpellSlot.R, SpellType.Targeted, CcType.No, 850, 500, 1500),

                #endregion Mordekaiser

                #region Morgana
                new TargetSpellData("morgana", "darkbindingmissile", SpellSlot.Q, SpellType.Skillshot, CcType.Snare, 1300, 500, 1200),
                new TargetSpellData("morgana", "tormentedsoil", SpellSlot.W, SpellType.Skillshot, CcType.No, 1075, 500, float.MaxValue),
                new TargetSpellData("morgana", "blackshield", SpellSlot.E, SpellType.Targeted, CcType.No, 750, 500, float.MaxValue),
                new TargetSpellData("morgana", "soulshackles", SpellSlot.R, SpellType.Self, CcType.Stun, 600, 500, float.MaxValue),

                #endregion Morgana

                #region Nami
                new TargetSpellData("nami", "namiq", SpellSlot.Q, SpellType.Skillshot, CcType.Knockup, 875, 500, 1750),
                new TargetSpellData("nami", "namiw", SpellSlot.W, SpellType.Targeted, CcType.No, 725, 500, 1100),
                new TargetSpellData("nami", "namie", SpellSlot.E, SpellType.Targeted, CcType.Slow, 800, 500, float.MaxValue),
                new TargetSpellData("nami", "namir", SpellSlot.R, SpellType.Skillshot, CcType.Knockup, 2550, 500, 1200),

                #endregion Nami

                #region Nasus
                new TargetSpellData("nasus", "nasusq", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("nasus", "nasusw", SpellSlot.W, SpellType.Targeted, CcType.Slow, 600, 500, float.MaxValue),
                new TargetSpellData("nasus", "nasuse", SpellSlot.E, SpellType.Skillshot, CcType.No, 850, 500, float.MaxValue),
                new TargetSpellData("nasus", "nasusr", SpellSlot.R, SpellType.Skillshot, CcType.No, 1, 500, float.MaxValue),

                #endregion Nasus

                #region Nautilus
                new TargetSpellData("nautilus", "nautilusanchordrag", SpellSlot.Q, SpellType.Skillshot, CcType.Pull, 950, 500, 1200),
                new TargetSpellData("nautilus", "nautiluspiercinggaze", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("nautilus", "nautilussplashzone", SpellSlot.E, SpellType.Self, CcType.Slow, 600, 500, 1300),
                new TargetSpellData("nautilus", "nautilusgandline", SpellSlot.R, SpellType.Targeted, CcType.Knockup, 1500, 500, 1400),

                #endregion Nautilus

                #region Nidalee
                new TargetSpellData("nidalee", "javelintoss", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1500, 500, 1300),
                new TargetSpellData("nidalee", "takedown", SpellSlot.Q, SpellType.Self, CcType.No, 50, 0, 500),
                new TargetSpellData("nidalee", "bushwhack", SpellSlot.W, SpellType.Skillshot, CcType.No, 900, 500, 1450),
                new TargetSpellData("nidalee", "pounce", SpellSlot.W, SpellType.Skillshot, CcType.No, 375, 500, 1500),
                new TargetSpellData("nidalee", "primalsurge", SpellSlot.E, SpellType.Targeted, CcType.No, 600, 0, float.MaxValue),
                new TargetSpellData("nidalee", "swipe", SpellSlot.E, SpellType.Skillshot, CcType.No, 300, 500, float.MaxValue),
                new TargetSpellData("nidalee", "aspectofthecougar", SpellSlot.R, SpellType.Self, CcType.No, 0, 0, float.MaxValue),

                #endregion Nidalee

                #region Nocturne
                new TargetSpellData("nocturne", "nocturneduskbringer", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1125, 500, 1600),
                new TargetSpellData("nocturne", "nocturneshroudofdarkness", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, 500),
                new TargetSpellData("nocturne", "nocturneunspeakablehorror", SpellSlot.E, SpellType.Targeted, CcType.Fear, 500, 500, 0),
                new TargetSpellData("nocturne", "nocturneparanoia", SpellSlot.R, SpellType.Targeted, CcType.No, 2000, 500, 500),

                #endregion Nocturne

                #region Nunu
                new TargetSpellData("nunu", "consume", SpellSlot.Q, SpellType.Targeted, CcType.No, 125, 500, 1400),
                new TargetSpellData("nunu", "bloodboil", SpellSlot.W, SpellType.Targeted, CcType.No, 700, 500, float.MaxValue),
                new TargetSpellData("nunu", "iceblast", SpellSlot.E, SpellType.Targeted, CcType.Slow, 550, 500, 1000),
                new TargetSpellData("nunu", "absolutezero", SpellSlot.R, SpellType.Self, CcType.Slow, 650, 500, float.MaxValue),

                #endregion Nunu

                #region Olaf
                new TargetSpellData("olaf", "olafaxethrowcast", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 1000, 500, 1600),
                new TargetSpellData("olaf", "olaffrenziedstrikes", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("olaf", "olafrecklessstrike", SpellSlot.E, SpellType.Targeted, CcType.No, 325, 500, float.MaxValue),
                new TargetSpellData("olaf", "olafragnarok", SpellSlot.R, SpellType.Self, CcType.No, 0, 500, float.MaxValue),

                #endregion Olaf

                #region Orianna
                new TargetSpellData("orianna", "orianaizunacommand", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1100, 500, 1200),
                new TargetSpellData("orianna", "orianadissonancecommand", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 0, 500, 1200),
                new TargetSpellData("orianna", "orianaredactcommand", SpellSlot.E, SpellType.Targeted, CcType.No, 1095, 500, 1200),
                new TargetSpellData("orianna", "orianadetonatecommand", SpellSlot.R, SpellType.Skillshot, CcType.Pull, 0, 500, 1200),

                #endregion Orianna

                #region Pantheon
                new TargetSpellData("pantheon", "pantheonq", SpellSlot.Q, SpellType.Targeted, CcType.No, 600, 500, 1500),
                new TargetSpellData("pantheon", "pantheonw", SpellSlot.W, SpellType.Targeted, CcType.Stun, 600, 500, float.MaxValue),
                new TargetSpellData("pantheon", "pantheone", SpellSlot.E, SpellType.Skillshot, CcType.No, 600, 500, 775),
                new TargetSpellData("pantheon", "pantheonrjump", SpellSlot.R, SpellType.Skillshot, CcType.No, 5500, 1000, 3000),
                new TargetSpellData("pantheon", "pantheonrfall", SpellSlot.R, SpellType.Skillshot, CcType.Slow, 5500, 1000, 3000),

                #endregion Pantheon

                #region Poppy
                new TargetSpellData("poppy", "poppydevastatingblow", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("poppy", "poppyparagonofdemacia", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("poppy", "poppyheroiccharge", SpellSlot.E, SpellType.Targeted, CcType.Stun, 525, 500, 1450),
                new TargetSpellData("poppy", "poppydiplomaticimmunity", SpellSlot.R, SpellType.Targeted, CcType.No, 900, 500, float.MaxValue),

                #endregion Poppy

                #region Quinn
                new TargetSpellData("quinn", "quinnq", SpellSlot.Q, SpellType.Skillshot, CcType.Blind, 1025, 500, 1200),
                new TargetSpellData("quinn", "quinnw", SpellSlot.W, SpellType.Self, CcType.No, 2100, 0, 0),
                new TargetSpellData("quinn", "quinne", SpellSlot.E, SpellType.Targeted, CcType.Knockback, 700, 500, 775),
                new TargetSpellData("quinn", "quinnr", SpellSlot.R, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("quinn", "quinnrfinale", SpellSlot.R, SpellType.Self, CcType.No, 700, 0, 0),

                #endregion Quinn

                #region Rammus
                new TargetSpellData("rammus", "powerball", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500, 775),
                new TargetSpellData("rammus", "defensiveballcurl", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("rammus", "puncturingtaunt", SpellSlot.E, SpellType.Targeted, CcType.Taunt, 325, 500, float.MaxValue),
                new TargetSpellData("rammus", "tremors2", SpellSlot.R, SpellType.Self, CcType.No, 300, 500, float.MaxValue),

                #endregion Rammus

                #region Renekton
                new TargetSpellData("renekton", "renektoncleave", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1, 500, float.MaxValue),
                new TargetSpellData("renekton", "renektonpreexecute", SpellSlot.W, SpellType.Self, CcType.Stun, 0, 500, float.MaxValue),
                new TargetSpellData("renekton", "renektonsliceanddice", SpellSlot.E, SpellType.Skillshot, CcType.No, 450, 500, 1400),
                new TargetSpellData("renekton", "renektonreignofthetyrant", SpellSlot.R, SpellType.Skillshot, CcType.No, 1, 500, 775),

                #endregion Renekton

                #region Rengar
                new TargetSpellData("rengar", "rengarq", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("rengar", "rengarw", SpellSlot.W, SpellType.Skillshot, CcType.No, 1, 500, float.MaxValue),
                new TargetSpellData("rengar", "rengare", SpellSlot.E, SpellType.Targeted, CcType.Snare, 1000, 500, 1500),
                new TargetSpellData("rengar", "rengarr", SpellSlot.R, SpellType.Self, CcType.No, 0, 500, float.MaxValue),

                #endregion Rengar

                #region Riven
                new TargetSpellData("riven", "riventricleave", SpellSlot.Q, SpellType.Skillshot, CcType.No, 250, 500, 0),
                new TargetSpellData("riven", "riventricleave_03", SpellSlot.Q, SpellType.Skillshot, CcType.Knockup, 250, 500, 0),
                new TargetSpellData("riven", "rivenmartyr", SpellSlot.W, SpellType.Self, CcType.Stun, 260, 250, 1500),
                new TargetSpellData("riven", "rivenfeint", SpellSlot.E, SpellType.Skillshot, CcType.No, 325, 0, 1450),
                new TargetSpellData("riven", "rivenfengshuiengine", SpellSlot.R, SpellType.Self, CcType.No, 0, 500, 1200),
                new TargetSpellData("riven", "rivenizunablade", SpellSlot.R, SpellType.Skillshot, CcType.No, 900, 300, 1450),

                #endregion Riven

                #region Rumble
                new TargetSpellData("rumble", "rumbleflamethrower", SpellSlot.Q, SpellType.Skillshot, CcType.No, 600, 500, float.MaxValue),
                new TargetSpellData("rumble", "rumbleshield", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("rumble", "rumbegrenade", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 850, 500, 1200),
                new TargetSpellData("rumble", "rumblecarpetbomb", SpellSlot.R, SpellType.Skillshot, CcType.Slow, 1700, 500, 1400),

                #endregion Rumble

                #region Ryze
                new TargetSpellData("ryze", "overload", SpellSlot.Q, SpellType.Targeted, CcType.No, 625, 500, 1400),
                new TargetSpellData("ryze", "runeprison", SpellSlot.W, SpellType.Targeted, CcType.Snare, 600, 500, float.MaxValue),
                new TargetSpellData("ryze", "spellflux", SpellSlot.E, SpellType.Targeted, CcType.No, 600, 500, 1000),
                new TargetSpellData("ryze", "desperatepower", SpellSlot.R, SpellType.Targeted, CcType.No, 625, 500, 1400),

                #endregion Ryze

                #region Sejuani
                new TargetSpellData("sejuani", "sejuaniarcticassault", SpellSlot.Q, SpellType.Skillshot, CcType.Knockback, 650, 500, 1450),
                new TargetSpellData("sejuani", "sejuaninorthernwinds", SpellSlot.W, SpellType.Skillshot, CcType.No, 1, 500, 1500),
                new TargetSpellData("sejuani", "sejuaniwintersclaw", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1, 500, 1450),
                new TargetSpellData("sejuani", "sejuaniglacialprisonstart", SpellSlot.R, SpellType.Skillshot, CcType.Stun, 1175, 500, 1400),

                #endregion Sejuani

                #region Shaco
                new TargetSpellData("shaco", "deceive", SpellSlot.Q, SpellType.Skillshot, CcType.No, 400, 500, float.MaxValue),
                new TargetSpellData("shaco", "jackinthebox", SpellSlot.W, SpellType.Skillshot, CcType.Fear, 425, 500, 1450),
                new TargetSpellData("shaco", "twoshivpoisen", SpellSlot.E, SpellType.Targeted, CcType.Slow, 625, 500, 1500),
                new TargetSpellData("shaco", "hallucinatefull", SpellSlot.R, SpellType.Skillshot, CcType.No, 1125, 500, 395),

                #endregion Shaco

                #region Shen
                new TargetSpellData("shen", "shenvorpalstar", SpellSlot.Q, SpellType.Targeted, CcType.No, 475, 500, 1500),
                new TargetSpellData("shen", "shenfeint", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("shen", "shenshadowdash", SpellSlot.E, SpellType.Skillshot, CcType.Taunt, 600, 500, 1000),
                new TargetSpellData("shen", "shenstandunited", SpellSlot.R, SpellType.Targeted, CcType.No, 25000, 500, float.MaxValue),

                #endregion Shen

                #region Shyvana
                new TargetSpellData("shyvana", "shyvanadoubleattack", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("shyvana", "shyvanadoubleattackdragon", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("shyvana", "shyvanaimmolationauraqw", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("shyvana", "shyvanaimmolateddragon", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("shyvana", "shyvanafireball", SpellSlot.E, SpellType.Skillshot, CcType.No, 925, 500, 1200),
                new TargetSpellData("shyvana", "shyvanafireballdragon2", SpellSlot.E, SpellType.Skillshot, CcType.No, 925, 500, 1200),
                new TargetSpellData("shyvana", "shyvanatransformcast", SpellSlot.R, SpellType.Skillshot, CcType.No, 1000, 500, 700),
                new TargetSpellData("shyvana", "shyvanatransformleap", SpellSlot.R, SpellType.Skillshot, CcType.Knockback, 1000, 500, 700),

                #endregion Shyvana

                #region Singed
                new TargetSpellData("singed", "poisentrail", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("singed", "megaadhesive", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1175, 500, 700),
                new TargetSpellData("singed", "fling", SpellSlot.E, SpellType.Targeted, CcType.Pull, 125, 500, float.MaxValue),
                new TargetSpellData("singed", "insanitypotion", SpellSlot.R, SpellType.Self, CcType.No, 0, 500, float.MaxValue),

                #endregion Singed

                #region Sion
                new TargetSpellData("sion", "crypticgaze", SpellSlot.Q, SpellType.Targeted, CcType.Stun, 550, 500, 1600),
                new TargetSpellData("sion", "deathscaressfull", SpellSlot.W, SpellType.Self, CcType.No, 550, 500, float.MaxValue),
                new TargetSpellData("sion", "deathscaress", SpellSlot.W, SpellType.Self, CcType.No, 550, 500, float.MaxValue),
                new TargetSpellData("sion", "enrage", SpellSlot.E, SpellType.Self, CcType.Slow, 0, 500, float.MaxValue),
                new TargetSpellData("sion", "cannibalism", SpellSlot.R, SpellType.Self, CcType.Stun, 0, 500, 500),

                #endregion Sion

                #region Sivir
                new TargetSpellData("sivir", "sivirq", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1165, 500, 1350),
                new TargetSpellData("sivir", "sivirw", SpellSlot.W, SpellType.Targeted, CcType.No, 565, 500, float.MaxValue),
                new TargetSpellData("sivir", "sivire", SpellSlot.E, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("sivir", "sivirr", SpellSlot.R, SpellType.Self, CcType.No, 1000, 500, float.MaxValue),

                #endregion Sivir

                #region Skarner
                new TargetSpellData("skarner", "skarnervirulentslash", SpellSlot.Q, SpellType.Self, CcType.No, 350, 0, float.MaxValue),
                new TargetSpellData("skarner", "skarnerexoskeleton", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("skarner", "skarnerfracture", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1000, 500, 1200),
                new TargetSpellData("skarner", "skarnerfracturemissilespell", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1000, 500, 1200),
                new TargetSpellData("skarner", "skarnerimpale", SpellSlot.R, SpellType.Targeted, CcType.Suppression, 350, 0, float.MaxValue),

                #endregion Skarner

                #region Sona
                new TargetSpellData("sona", "sonahymnofvalor", SpellSlot.Q, SpellType.Self, CcType.No, 700, 500, 1500),
                new TargetSpellData("sona", "sonaariaofperseverance", SpellSlot.W, SpellType.Self, CcType.No, 1000, 500, 1500),
                new TargetSpellData("sona", "sonasongofdiscord", SpellSlot.E, SpellType.Self, CcType.No, 1000, 500, 1500),
                new TargetSpellData("sona", "sonacrescendo", SpellSlot.R, SpellType.Skillshot, CcType.Stun, 900, 500, 2400),

                #endregion Sona

                #region Soraka
                new TargetSpellData("soraka", "starcall", SpellSlot.Q, SpellType.Self, CcType.No, 675, 500, float.MaxValue),
                new TargetSpellData("soraka", "astralblessing", SpellSlot.W, SpellType.Targeted, CcType.No, 750, 500, float.MaxValue),
                new TargetSpellData("soraka", "infusewrapper", SpellSlot.E, SpellType.Targeted, CcType.No, 725, 500, float.MaxValue),
                new TargetSpellData("soraka", "wish", SpellSlot.R, SpellType.Self, CcType.No, 25000, 500, float.MaxValue),

                #endregion Soraka

                #region Swain
                new TargetSpellData("swain", "swaindecrepify", SpellSlot.Q, SpellType.Targeted, CcType.Slow, 625, 500, float.MaxValue),
                new TargetSpellData("swain", "swainshadowgrasp", SpellSlot.W, SpellType.Skillshot, CcType.Snare, 1040, 500, 1250),
                new TargetSpellData("swain", "swaintorment", SpellSlot.E, SpellType.Targeted, CcType.No, 625, 500, 1400),
                new TargetSpellData("swain", "swainmetamorphism", SpellSlot.R, SpellType.Self, CcType.No, 700, 500, 950),

                #endregion Swain

                #region Syndra
                new TargetSpellData("syndra", "syndraq", SpellSlot.Q, SpellType.Skillshot, CcType.No, 800, 250, 1750),
                new TargetSpellData("syndra", "syndraw", SpellSlot.W, SpellType.Targeted, CcType.No, 925, 500, 1450),
                new TargetSpellData("syndra", "syndrawcast", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 950, 500, 1450),
                new TargetSpellData("syndra", "syndrae", SpellSlot.E, SpellType.Skillshot, CcType.Stun, 700, 500, 902),
                new TargetSpellData("syndra", "syndrar", SpellSlot.R, SpellType.Targeted, CcType.No, 675, 500, 1100),

                #endregion Syndra

                #region Talon
                new TargetSpellData("talon", "talonnoxiandiplomacy", SpellSlot.Q, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("talon", "talonrake", SpellSlot.W, SpellType.Skillshot, CcType.No, 750, 500, 1200),
                new TargetSpellData("talon", "taloncutthroat", SpellSlot.E, SpellType.Targeted, CcType.Slow, 750, 0, 1200),
                new TargetSpellData("talon", "talonshadowassault", SpellSlot.R, SpellType.Self, CcType.No, 750, 0, 0),

                #endregion Talon

                #region Taric
                new TargetSpellData("taric", "imbue", SpellSlot.Q, SpellType.Targeted, CcType.No, 750, 500, 1200),
                new TargetSpellData("taric", "shatter", SpellSlot.W, SpellType.Self, CcType.No, 400, 500, float.MaxValue),
                new TargetSpellData("taric", "dazzle", SpellSlot.E, SpellType.Targeted, CcType.Stun, 625, 500, 1400),
                new TargetSpellData("taric", "tarichammersmash", SpellSlot.R, SpellType.Self, CcType.No, 400, 500, float.MaxValue),

                #endregion Taric

                #region Teemo
                new TargetSpellData("teemo", "blindingdart", SpellSlot.Q, SpellType.Targeted, CcType.Blind, 580, 500, 1500),
                new TargetSpellData("teemo", "movequick", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 943),
                new TargetSpellData("teemo", "toxicshot", SpellSlot.E, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("teemo", "bantamtrap", SpellSlot.R, SpellType.Skillshot, CcType.Slow, 230, 0, 1500),

                #endregion Teemo

                #region Thresh
                new TargetSpellData("thresh", "threshq", SpellSlot.Q, SpellType.Skillshot, CcType.Pull, 1175, 500, 1200),
                new TargetSpellData("thresh", "threshw", SpellSlot.W, SpellType.Skillshot, CcType.No, 950, 500, float.MaxValue),
                new TargetSpellData("thresh", "threshe", SpellSlot.E, SpellType.Skillshot, CcType.Knockback, 515, 300, float.MaxValue),
                new TargetSpellData("thresh", "threshrpenta", SpellSlot.R, SpellType.Skillshot, CcType.Slow, 420, 300, float.MaxValue),

                #endregion Thresh

                #region Tristana
                new TargetSpellData("tristana", "rapidfire", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("tristana", "rocketjump", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 900, 500, 1150),
                new TargetSpellData("tristana", "detonatingshot", SpellSlot.E, SpellType.Targeted, CcType.No, 625, 500, 1400),
                new TargetSpellData("tristana", "bustershot", SpellSlot.R, SpellType.Targeted, CcType.Knockback, 700, 500, 1600),

                #endregion Tristana

                #region Trundle
                new TargetSpellData("trundle", "trundletrollsmash", SpellSlot.Q, SpellType.Targeted, CcType.Slow, 0, 500, float.MaxValue),
                new TargetSpellData("trundle", "trundledesecrate", SpellSlot.W, SpellType.Skillshot, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("trundle", "trundlecircle", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1100, 500, 1600),
                new TargetSpellData("trundle", "trundlepain", SpellSlot.R, SpellType.Targeted, CcType.No, 700, 500, 1400),

                #endregion Trundle

                #region Tryndamere
                new TargetSpellData("tryndamere", "bloodlust", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("tryndamere", "mockingshout", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 400, 500, 500),
                new TargetSpellData("tryndamere", "slashcast", SpellSlot.E, SpellType.Skillshot, CcType.No, 660, 500, 700),
                new TargetSpellData("tryndamere", "undyingrage", SpellSlot.R, SpellType.Self, CcType.No, 0, 500, float.MaxValue),

                #endregion Tryndamere

                #region Twich
                new TargetSpellData("twich", "hideinshadows", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("twich", "twitchvenomcask", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 800, 500, 1750),
                new TargetSpellData("twich", "twitchvenomcaskmissle", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 800, 500, 1750),
                new TargetSpellData("twich", "expunge", SpellSlot.E, SpellType.Targeted, CcType.No, 1200, 500, float.MaxValue),
                new TargetSpellData("twich", "fullautomatic", SpellSlot.R, SpellType.Targeted, CcType.No, 850, 500, 500),

                #endregion Twich

                #region TwistedFate
                new TargetSpellData("twistedfate", "wildcards", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1450, 500, 1450),
                new TargetSpellData("twistedfate", "pickacard", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("twistedfate", "goldcardpreattack", SpellSlot.W, SpellType.Targeted, CcType.Stun, 600, 500, float.MaxValue),
                new TargetSpellData("twistedfate", "redcardpreattack", SpellSlot.W, SpellType.Targeted, CcType.Slow, 600, 500, float.MaxValue),
                new TargetSpellData("twistedfate", "bluecardpreattack", SpellSlot.W, SpellType.Targeted, CcType.No, 600, 500, float.MaxValue),
                new TargetSpellData("twistedfate", "cardmasterstack", SpellSlot.E, SpellType.Self, CcType.No, 525, 500, 1200),
                new TargetSpellData("twistedfate", "destiny", SpellSlot.R, SpellType.Skillshot, CcType.No, 5500, 500, float.MaxValue),

                #endregion TwistedFate

                #region Udyr
                new TargetSpellData("udyr", "udyrtigerstance", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("udyr", "udyrturtlestance", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("udyr", "udyrbearstance", SpellSlot.E, SpellType.Self, CcType.Stun, 0, 500, float.MaxValue),
                new TargetSpellData("udyr", "udyrphoenixstance", SpellSlot.R, SpellType.Self, CcType.No, 0, 500, float.MaxValue),

                #endregion Udyr

                #region Urgot
                new TargetSpellData("urgot", "urgotheatseekinglineqqmissile", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1000, 500, 1600),
                new TargetSpellData("urgot", "urgotheatseekingmissile", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1000, 500, 1600),
                new TargetSpellData("urgot", "urgotterrorcapacitoractive2", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("urgot", "urgotplasmagrenade", SpellSlot.E, SpellType.Skillshot, CcType.No, 950, 500, 1750),
                new TargetSpellData("urgot", "urgotplasmagrenadeboom", SpellSlot.E, SpellType.Skillshot, CcType.No, 950, 500, 1750),
                new TargetSpellData("urgot", "urgotswap2", SpellSlot.R, SpellType.Targeted, CcType.Suppression, 850, 500, 1800),

                #endregion Urgot

                #region Varus
                new TargetSpellData("varus", "varusq", SpellSlot.Q, SpellType.Skillshot, CcType.No, 1500, 500, 1500),
                new TargetSpellData("varus", "varusw", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, 0),
                new TargetSpellData("varus", "varuse", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 925, 500, 1500),
                new TargetSpellData("varus", "varusr", SpellSlot.R, SpellType.Skillshot, CcType.Snare, 1300, 500, 1500),

                #endregion Varus

                #region Vayne
                new TargetSpellData("vayne", "vaynetumble", SpellSlot.Q, SpellType.Skillshot, CcType.No, 250, 500, float.MaxValue),
                new TargetSpellData("vayne", "vaynesilverbolts", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("vayne", "vaynecondemm", SpellSlot.E, SpellType.Targeted, CcType.Stun, 450, 500, 1200),
                new TargetSpellData("vayne", "vayneinquisition", SpellSlot.R, SpellType.Self, CcType.No, 0, 500, float.MaxValue),

                #endregion Vayne

                #region Veigar
                new TargetSpellData("veigar", "veigarbalefulstrike", SpellSlot.Q, SpellType.Targeted, CcType.No, 650, 500, 1500),
                new TargetSpellData("veigar", "veigardarkmatter", SpellSlot.W, SpellType.Skillshot, CcType.No, 900, 1200, 1500),
                new TargetSpellData("veigar", "veigareventhorizon", SpellSlot.E, SpellType.Skillshot, CcType.Stun, 650, float.MaxValue, 1500),
                new TargetSpellData("veigar", "veigarprimordialburst", SpellSlot.R, SpellType.Targeted, CcType.No, 650, 500, 1400),

                #endregion Veigar

                #region Velkoz
                new TargetSpellData("velkoz", "velkozq", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 1050, 300, 1200),
                new TargetSpellData("velkoz", "velkozqmissle", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 1050, 0, 1200),
                new TargetSpellData("velkoz", "velkozqplitactive", SpellSlot.Q, SpellType.Skillshot, CcType.Slow, 1050, 800, 1200),
                new TargetSpellData("velkoz", "velkozw", SpellSlot.W, SpellType.Skillshot, CcType.No, 1050, 0, 1200),
                new TargetSpellData("velkoz", "velkoze", SpellSlot.E, SpellType.Targeted, CcType.Knockup, 850, 0, 500),
                new TargetSpellData("velkoz", "velkozr", SpellSlot.R, SpellType.Skillshot, CcType.No, 1575, 0, 1500),

                #endregion Velkoz

                #region Vi
                new TargetSpellData("vi", "viq", SpellSlot.Q, SpellType.Skillshot, CcType.No, 800, 500, 1500),
                new TargetSpellData("vi", "viw", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("vi", "vie", SpellSlot.E, SpellType.Skillshot, CcType.No, 600, 0, 0),
                new TargetSpellData("vi", "vir", SpellSlot.R, SpellType.Targeted, CcType.Stun, 800, 500, 0),

                #endregion Vi

                #region Viktor
                new TargetSpellData("viktor", "viktorpowertransfer", SpellSlot.Q, SpellType.Targeted, CcType.No, 600, 500, 1400),
                new TargetSpellData("viktor", "viktorgravitonfield", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 815, 500, 1750),
                new TargetSpellData("viktor", "viktordeathray", SpellSlot.E, SpellType.Skillshot, CcType.No, 700, 500, 1210),
                new TargetSpellData("viktor", "viktorchaosstorm", SpellSlot.R, SpellType.Skillshot, CcType.Silence, 700, 500, 1210),

                #endregion Viktor

                #region Vladimir
                new TargetSpellData("vladimir", "vladimirtransfusion", SpellSlot.Q, SpellType.Targeted, CcType.No, 600, 500, 1400),
                new TargetSpellData("vladimir", "vladimirsanguinepool", SpellSlot.W, SpellType.Self, CcType.Slow, 350, 500, 1600),
                new TargetSpellData("vladimir", "vladimirtidesofblood", SpellSlot.E, SpellType.Self, CcType.No, 610, 500, 1100),
                new TargetSpellData("vladimir", "vladimirhemoplague", SpellSlot.R, SpellType.Skillshot, CcType.No, 875, 500, 1200),

                #endregion Vladimir

                #region Volibear
                new TargetSpellData("volibear", "volibearq", SpellSlot.Q, SpellType.Self, CcType.No, 300, 500, float.MaxValue),
                new TargetSpellData("volibear", "volibearw", SpellSlot.W, SpellType.Targeted, CcType.No, 400, 500, 1450),
                new TargetSpellData("volibear", "volibeare", SpellSlot.E, SpellType.Self, CcType.Slow, 425, 500, 825),
                new TargetSpellData("volibear", "volibearr", SpellSlot.R, SpellType.Self, CcType.No, 425, 0, 825),

                #endregion Volibear

                #region Warwick
                new TargetSpellData("warwick", "hungeringstrike", SpellSlot.Q, SpellType.Targeted, CcType.No, 400, 0, float.MaxValue),
                new TargetSpellData("warwick", "hunterscall", SpellSlot.W, SpellType.Self, CcType.No, 1000, 0, float.MaxValue),
                new TargetSpellData("warwick", "bloodscent", SpellSlot.E, SpellType.Self, CcType.No, 1500, 0, float.MaxValue),
                new TargetSpellData("warwick", "infiniteduress", SpellSlot.R, SpellType.Targeted, CcType.Suppression, 700, 500, float.MaxValue),

                #endregion Warwick

                #region Xerath
                new TargetSpellData("xerath", "xeratharcanopulsechargeup", SpellSlot.Q, SpellType.Skillshot, CcType.No, 750, 750, 500),
                new TargetSpellData("xerath", "xeratharcanebarrage2", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 1100, 500, 20),
                new TargetSpellData("xerath", "xerathmagespear", SpellSlot.E, SpellType.Skillshot, CcType.Stun, 1050, 500, 1600),
                new TargetSpellData("xerath", "xerathlocusofpower2", SpellSlot.R, SpellType.Skillshot, CcType.No, 5600, 750, 500),

                #endregion Xerath

                #region Xin Zhao
                new TargetSpellData("xin zhao", "xenzhaocombotarget", SpellSlot.Q, SpellType.Self, CcType.No, 200, 0, 2000),
                new TargetSpellData("xin zhao", "xenzhaobattlecry", SpellSlot.W, SpellType.Self, CcType.No, 0, 0, 2000),
                new TargetSpellData("xin zhao", "xenzhaosweep", SpellSlot.E, SpellType.Targeted, CcType.Slow, 600, 500, 1750),
                new TargetSpellData("xin zhao", "xenzhaoparry", SpellSlot.R, SpellType.Self, CcType.Knockback, 375, 0, 1750),

                #endregion Xin Zhao

                #region Yasuo
                new TargetSpellData("yasuo", "yasuoqw", SpellSlot.Q, SpellType.Skillshot, CcType.No, 475, 750, 1500),
                new TargetSpellData("yasuo", "yasuoq2w", SpellSlot.Q, SpellType.Skillshot, CcType.No, 475, 750, 1500),
                new TargetSpellData("yasuo", "yasuoq3w", SpellSlot.Q, SpellType.Skillshot, CcType.Knockup, 1000, 750, 1500),
                new TargetSpellData("yasuo", "yasuowmovingwall", SpellSlot.W, SpellType.Skillshot, CcType.No, 400, 500, 500),
                new TargetSpellData("yasuo", "yasuodashwrapper", SpellSlot.E, SpellType.Targeted, CcType.No, 475, 500, 20),
                new TargetSpellData("yasuo", "yasuorknockupcombow", SpellSlot.R, SpellType.Self, CcType.No, 1200, 500, 20),

                #endregion Yasuo

                #region Yorick
                new TargetSpellData("yorick", "yorickspectral", SpellSlot.Q, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("yorick", "yorickdecayed", SpellSlot.W, SpellType.Skillshot, CcType.Slow, 600, 500, float.MaxValue),
                new TargetSpellData("yorick", "yorickravenous", SpellSlot.E, SpellType.Targeted, CcType.Slow, 550, 500, float.MaxValue),
                new TargetSpellData("yorick", "yorickreviveally", SpellSlot.R, SpellType.Targeted, CcType.No, 900, 500, 1500),

                #endregion Yorick

                #region Zac
                new TargetSpellData("zac", "zacq", SpellSlot.Q, SpellType.Skillshot, CcType.Knockup, 550, 500, 902),
                new TargetSpellData("zac", "zacw", SpellSlot.W, SpellType.Self, CcType.No, 350, 500, 1600),
                new TargetSpellData("zac", "zace", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 1550, 500, 1500),
                new TargetSpellData("zac", "zacr", SpellSlot.R, SpellType.Self, CcType.Knockback, 850, 500, 1800),

                #endregion Zac

                #region Zed
                new TargetSpellData("zed", "zedshuriken", SpellSlot.Q, SpellType.Skillshot, CcType.No, 900, 500, 902),
                new TargetSpellData("zed", "zedshdaowdash", SpellSlot.W, SpellType.Skillshot, CcType.No, 550, 500, 1600),
                new TargetSpellData("zed", "zedpbaoedummy", SpellSlot.E, SpellType.Self, CcType.Slow, 300, 0, 0),
                new TargetSpellData("zed", "zedult", SpellSlot.R, SpellType.Targeted, CcType.No, 850, 500, 0),

                #endregion Zed

                #region Ziggs
                new TargetSpellData("ziggs", "ziggsq", SpellSlot.Q, SpellType.Skillshot, CcType.No, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggsqspell", SpellSlot.Q, SpellType.Skillshot, CcType.No, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggsw", SpellSlot.W, SpellType.Skillshot, CcType.Knockup, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggswtoggle", SpellSlot.W, SpellType.Self, CcType.Knockup, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggse", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggse2", SpellSlot.E, SpellType.Skillshot, CcType.Slow, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggsr", SpellSlot.R, SpellType.Skillshot, CcType.No, 850, 500, 1750),

                #endregion Ziggs

                #region Zilean
                new TargetSpellData("zilean", "timebomb", SpellSlot.Q, SpellType.Targeted, CcType.No, 700, 0, 1100),
                new TargetSpellData("zilean", "rewind", SpellSlot.W, SpellType.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("zilean", "timewarp", SpellSlot.E, SpellType.Targeted, CcType.Slow, 700, 500, 1100),
                new TargetSpellData("zilean", "chronoshift", SpellSlot.R, SpellType.Targeted, CcType.No, 780, 500, float.MaxValue),

                #endregion Zilean

                #region Zyra
                new TargetSpellData("zyra", "zyraqfissure", SpellSlot.Q, SpellType.Skillshot, CcType.No, 800, 500, 1400),
                new TargetSpellData("zyra", "zyraseed", SpellSlot.W, SpellType.Skillshot, CcType.No, 800, 500, 2200),
                new TargetSpellData("zyra", "zyragraspingroots", SpellSlot.E, SpellType.Skillshot, CcType.Snare, 1100, 500, 1400),
                new TargetSpellData("zyra", "zyrabramblezone", SpellSlot.R, SpellType.Skillshot, CcType.Knockup, 700, 500, 20),

                #endregion Zyra
            };
        }

        public static TargetSpellData GetByName(string spellName)
        {
            spellName = spellName.ToLower();
            return Spells.FirstOrDefault(spell => spell.Name == spellName);
        }

        //Data


    }
}
