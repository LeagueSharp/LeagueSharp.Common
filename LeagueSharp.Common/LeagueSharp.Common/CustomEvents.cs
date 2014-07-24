#region

using System;

#endregion

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

            static Game()
            {
                LeagueSharp.Game.OnGameProcessPacket += Game_OnGameProcessPacket;
            }

            /// <summary>
            /// OnGameLoad is getting called when you get ingame (doesn't matter if started or restarted while game is already running) and when reloading an assembly
            /// </summary>
            public static event OnGameLoaded OnGameLoad;

            private static void Game_OnGameProcessPacket(GamePacketEventArgs args)
                // Use Packets as they come exactly when a game starts GG
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
            public delegate void OnDashed(Obj_AI_Base sender, Dash.DashItem args);

            public delegate void OnLeveledUp(Obj_AI_Base sender, OnLevelUpEventArgs args);

            public delegate void OnLeveledUpSpell(Obj_AI_Base sender, OnLevelUpSpellEventArgs args);


            static Unit()
            {
                LeagueSharp.Game.OnGameProcessPacket += PacketHandler;

                //Initializes ondash class:
                ObjectManager.Player.IsDashing();
            }

            /// <summary>
            /// OnLevelUpSpell gets called after you leveled a spell
            /// </summary>
            public static event OnLeveledUpSpell OnLevelUpSpell;

            private static void PacketHandler(GamePacketEventArgs args)
            {
                if (OnLevelUpSpell != null)
                {
                    if (args.PacketData[0] == 0x15)
                    {
                        var unit =
                            ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(BitConverter.ToInt32(args.PacketData, 1));
                        int id = args.PacketData[5];
                        int lvl = args.PacketData[6];
                        int pts = args.PacketData[7];
                        OnLevelUpSpell(unit,
                            new OnLevelUpSpellEventArgs { SpellId = id, SpellLevel = lvl, Remainingpoints = pts });
                    }
                }
                if (OnLevelUp != null)
                {
                    if (args.PacketData[0] == 0x3F)
                    {
                        var unit =
                            ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(BitConverter.ToInt32(args.PacketData, 1));
                        int newlvl = args.PacketData[5];
                        int pts = args.PacketData[6];
                        OnLevelUp(unit, new OnLevelUpEventArgs { NewLevel = newlvl, RemainingPoints = pts });
                    }
                }
            }

            /// <summary>
            /// Gets called when a unit gets a level up
            /// </summary>
            public static event OnLeveledUp OnLevelUp;

            /// <summary>
            /// OnDash is getting called when a unit dashes.
            /// </summary>
            public static event OnDashed OnDash;

            public static void TriggerOnDash(Obj_AI_Base sender, Dash.DashItem args)
            {
                OnDash(sender, args);
            }

            public class OnLevelUpEventArgs : EventArgs
            {
                public int NewLevel;
                public int RemainingPoints;
            }

            public class OnLevelUpSpellEventArgs : EventArgs
            {
                public int Remainingpoints;
                public int SpellId;
                public int SpellLevel;

                internal OnLevelUpSpellEventArgs()
                {
                }
            }

            private class UnitBuff
            {
                public BuffInstance[] buffs;
                public int networkid;
            }
        }
    }
}