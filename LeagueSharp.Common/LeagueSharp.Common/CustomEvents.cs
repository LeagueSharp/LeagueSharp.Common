using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LeagueSharp.Common
{
    public static class CustomEvents
    {
        static CustomEvents()
        {
            if (Common.isInitialized == false)
            {
                Common.InitializeCommonLib();
            }
        }
        public class Game
        {
            public delegate void OnGameLoaded(EventArgs args);
            /// <summary>
            /// OnGameLoad is getting called when you get ingame (doesn't matter if started or restarted while game is already running) and when reloading an assembly
            /// </summary>
            public static event OnGameLoaded OnGameLoad;

            static Game()
            {
                LeagueSharp.Game.OnGameProcessPacket += Game_OnGameProcessPacket;
            }

            static void Game_OnGameProcessPacket(GamePacketEventArgs args) // Use Packets as they come exactly when a game starts GG
            {
                if (LeagueSharp.Game.Mode == GameMode.Running)
                {
                    OnGameLoad(new EventArgs());
                    LeagueSharp.Game.OnGameProcessPacket -= Game_OnGameProcessPacket; // delete the event
                }
            }
        }

        public class Unit
        {
            public delegate void OnLeveledUpSpell(Obj_AI_Base sender, OnLevelUpSpellEventArgs args);
            /// <summary>
            /// OnLevelUpSpell gets called after you leveled a spell
            /// </summary>
            public static event OnLeveledUpSpell OnLevelUpSpell;


            static Unit()
            {
                LeagueSharp.Game.OnGameProcessPacket += PacketHandler;
            }

            private class UnitBuff
            {
                public int networkid;
                public BuffInstance[] buffs;
            }

            static void PacketHandler(GamePacketEventArgs args)
            {
                if (OnLevelUpSpell != null)
                {
                    if (args.PacketData[0] == 0x15)
                    {
                        Obj_AI_Base unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(BitConverter.ToInt32(args.PacketData, 1));
                        int id = args.PacketData[5];
                        int lvl = args.PacketData[6];
                        int pts = args.PacketData[7];
                        OnLevelUpSpell(unit, new OnLevelUpSpellEventArgs() { SpellId = id, SpellLevel = lvl, Remainingpoints = pts });
                    }
                }
                if (OnLevelUp != null)
                {
                    if (args.PacketData[0] == 0x3F)
                    {
                        Obj_AI_Base unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(BitConverter.ToInt32(args.PacketData, 1));
                        int newlvl = args.PacketData[5];
                        int pts = args.PacketData[6];
                        OnLevelUp(unit, new OnLevelUpEventArgs() { NewLevel = newlvl, RemainingPoints = pts });
                    }
                }
            }

            public class OnLevelUpSpellEventArgs : EventArgs
            {
                public int SpellId;
                public int SpellLevel;
                public int Remainingpoints;
                internal OnLevelUpSpellEventArgs() { }
            }

            // // // //

            public delegate void OnLeveledUp(Obj_AI_Base sender, OnLevelUpEventArgs args);

            /// <summary>
            /// Gets called when a unit gets a level up
            /// </summary>
            public static event OnLeveledUp OnLevelUp;

            public class OnLevelUpEventArgs : EventArgs
            {
                public int NewLevel;
                public int RemainingPoints;
            }

            // // // //

            public delegate void OnDashed(Obj_AI_Base sender, Dash.DashItem args);
            /// <summary>
            /// OnDash is getting called when a unit dashes.
            /// </summary>
            public static event OnDashed OnDash;

            public static void TriggerOnDash(Obj_AI_Base sender, Dash.DashItem args)
            {
                OnDash(sender, args);
            }
        }
    }
}
