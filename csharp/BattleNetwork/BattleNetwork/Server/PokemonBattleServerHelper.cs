namespace PokemonBattle.BattleNetwork.Server
{
    using NetworkLib.Utilities;
    using PokemonBattle.BattleNetwork;
    using System;

    public class PokemonBattleServerHelper
    {
        public static ByteArray Exit(string identity)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(-1158854104);
            array.WriteUTF(identity);
            return array;
        }

        public static bool InterpretMessage(int sessionID, ByteArray byteArray, IPokemonBattleServerService serverService)
        {
            byteArray.BypassHeader();
            switch (byteArray.ReadInt())
            {
                case -1110713650:
                    return OnTimeUp(sessionID, byteArray, serverService);

                case -774376481:
                    return OnRegistObsever(sessionID, byteArray, serverService);

                case -1287328945:
                    return OnReceiveTeam(sessionID, byteArray, serverService);

                case -1158854104:
                    return OnExit(sessionID, byteArray, serverService);

                case 0x823c9f1:
                    return OnReceiveTieMessage(sessionID, byteArray, serverService);

                case 0x49fd3e0f:
                    return OnReceiveBattleSnapshot(sessionID, byteArray, serverService);

                case 0x51a0a8fc:
                    return OnReceiveBattleInfo(sessionID, byteArray, serverService);

                case 0x5bbe67ef:
                    return OnLogon(sessionID, byteArray, serverService);

                case 0x6615466d:
                    return OnReceiveMove(sessionID, byteArray, serverService);
            }
            byteArray.Rewind();
            return false;
        }

        public static ByteArray LogonFail(string message)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(-632492989);
            array.WriteUTF(message);
            return array;
        }

        public static ByteArray LogonSuccess()
        {
            ByteArray array = new ByteArray();
            array.WriteInt(0x7ada52ee);
            return array;
        }

        private static bool OnExit(int sessionID, ByteArray byteArray, IPokemonBattleServerService serverService)
        {
            string identity = byteArray.ReadUTF();
            serverService.OnExit(sessionID, identity);
            return true;
        }

        private static bool OnLogon(int sessionID, ByteArray byteArray, IPokemonBattleServerService serverService)
        {
            string identity = byteArray.ReadUTF();
            BattleMode modeInfo = BattleModeHelper.ReadFromByteArray(byteArray);
            string versionInfo = byteArray.ReadUTF();
            serverService.OnLogon(sessionID, identity, modeInfo, versionInfo);
            return true;
        }

        private static bool OnReceiveBattleInfo(int sessionID, ByteArray byteArray, IPokemonBattleServerService serverService)
        {
            BattleInfo info = new BattleInfo();
            info.ReadFromByteArray(byteArray);
            serverService.OnReceiveBattleInfo(sessionID, info);
            return true;
        }

        private static bool OnReceiveBattleSnapshot(int sessionID, ByteArray byteArray, IPokemonBattleServerService serverService)
        {
            BattleSnapshot snapshot = new BattleSnapshot();
            snapshot.ReadFromByteArray(byteArray);
            serverService.OnReceiveBattleSnapshot(sessionID, snapshot);
            return true;
        }

        private static bool OnReceiveMove(int sessionID, ByteArray byteArray, IPokemonBattleServerService serverService)
        {
            PlayerMove move = new PlayerMove();
            move.ReadFromByteArray(byteArray);
            serverService.OnReceiveMove(sessionID, move);
            return true;
        }

        private static bool OnReceiveTeam(int sessionID, ByteArray byteArray, IPokemonBattleServerService serverService)
        {
            byte position = byteArray.ReadByte();
            string identity = byteArray.ReadUTF();
            ByteSequence team = new ByteSequence();
            team.ReadFromByteArray(byteArray);
            serverService.OnReceiveTeam(sessionID, position, identity, team);
            return true;
        }

        private static bool OnReceiveTieMessage(int sessionID, ByteArray byteArray, IPokemonBattleServerService serverService)
        {
            string identity = byteArray.ReadUTF();
            TieMessage message = TieMessageHelper.ReadFromByteArray(byteArray);
            serverService.OnReceiveTieMessage(sessionID, identity, message);
            return true;
        }

        private static bool OnRegistObsever(int sessionID, ByteArray byteArray, IPokemonBattleServerService serverService)
        {
            int identity = byteArray.ReadInt();
            serverService.OnRegistObsever(sessionID, identity);
            return true;
        }

        private static bool OnTimeUp(int sessionID, ByteArray byteArray, IPokemonBattleServerService serverService)
        {
            string identity = byteArray.ReadUTF();
            serverService.OnTimeUp(sessionID, identity);
            return true;
        }

        public static ByteArray ReceiveBattleInfo(BattleInfo info)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.WriteInt(0x51a0a8fc);
            info.WriteToByteArray(byteArray);
            return byteArray;
        }

        public static ByteArray ReceiveBattleSnapshot(BattleSnapshot snapshot)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.WriteInt(0x49fd3e0f);
            snapshot.WriteToByteArray(byteArray);
            return byteArray;
        }

        public static ByteArray ReceiveMove(PlayerMove move)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.WriteInt(0x6615466d);
            move.WriteToByteArray(byteArray);
            return byteArray;
        }

        public static ByteArray ReceiveRandomSeed(int seed)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(0x18789bab);
            array.WriteInt(seed);
            return array;
        }

        public static ByteArray ReceiveRules(BattleRuleSequence rules)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.WriteInt(-274250105);
            rules.WriteToByteArray(byteArray);
            return byteArray;
        }

        public static ByteArray ReceiveTeam(byte position, string identity, ByteSequence team)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.WriteInt(-1287328945);
            byteArray.WriteByte(position);
            byteArray.WriteUTF(identity);
            team.WriteToByteArray(byteArray);
            return byteArray;
        }

        public static ByteArray ReceiveTieMessage(string identity, TieMessage message)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.WriteInt(0x823c9f1);
            byteArray.WriteUTF(identity);
            TieMessageHelper.WriteToByteArray(byteArray, message);
            return byteArray;
        }

        public static ByteArray RegistObsever(int identity)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(-774376481);
            array.WriteInt(identity);
            return array;
        }

        public static ByteArray TimeUp(string identity)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(-1110713650);
            array.WriteUTF(identity);
            return array;
        }
    }
}

