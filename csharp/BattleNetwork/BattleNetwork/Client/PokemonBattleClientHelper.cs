namespace PokemonBattle.BattleNetwork.Client
{
    using NetworkLib.Utilities;
    using PokemonBattle.BattleNetwork;
    using System;

    public class PokemonBattleClientHelper
    {
        public static ByteArray Exit(string identity)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(-1158854104);
            array.WriteUTF(identity);
            return array;
        }

        public static bool InterpretMessage(int sessionID, ByteArray byteArray, IPokemonBattleClientService clientService)
        {
            byteArray.BypassHeader();
            switch (byteArray.ReadInt())
            {
                case -774376481:
                    return OnRegistObsever(byteArray, clientService);

                case -632492989:
                    return OnLogonFail(byteArray, clientService);

                case -274250105:
                    return OnReceiveRules(byteArray, clientService);

                case -1287328945:
                    return OnReceiveTeam(byteArray, clientService);

                case -1158854104:
                    return OnExit(byteArray, clientService);

                case -1110713650:
                    return OnTimeUp(byteArray, clientService);

                case 0x823c9f1:
                    return OnReceiveTieMessage(byteArray, clientService);

                case 0x18789bab:
                    return OnReceiveRandomSeed(byteArray, clientService);

                case 0x49fd3e0f:
                    return OnReceiveBattleSnapshot(byteArray, clientService);

                case 0x51a0a8fc:
                    return OnReceiveBattleInfo(byteArray, clientService);

                case 0x6615466d:
                    return OnReceiveMove(byteArray, clientService);

                case 0x7ada52ee:
                    return OnLogonSuccess(byteArray, clientService);
            }
            byteArray.Rewind();
            return false;
        }

        public static ByteArray Logon(string identity, BattleMode modeInfo, string versionInfo)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.WriteInt(0x5bbe67ef);
            byteArray.WriteUTF(identity);
            BattleModeHelper.WriteToByteArray(byteArray, modeInfo);
            byteArray.WriteUTF(versionInfo);
            return byteArray;
        }

        private static bool OnExit(ByteArray byteArray, IPokemonBattleClientService clientService)
        {
            string identity = byteArray.ReadUTF();
            clientService.OnExit(identity);
            return true;
        }

        private static bool OnLogonFail(ByteArray byteArray, IPokemonBattleClientService clientService)
        {
            string message = byteArray.ReadUTF();
            clientService.OnLogonFail(message);
            return true;
        }

        private static bool OnLogonSuccess(ByteArray byteArray, IPokemonBattleClientService clientService)
        {
            clientService.OnLogonSuccess();
            return true;
        }

        private static bool OnReceiveBattleInfo(ByteArray byteArray, IPokemonBattleClientService clientService)
        {
            BattleInfo info = new BattleInfo();
            info.ReadFromByteArray(byteArray);
            clientService.OnReceiveBattleInfo(info);
            return true;
        }

        private static bool OnReceiveBattleSnapshot(ByteArray byteArray, IPokemonBattleClientService clientService)
        {
            BattleSnapshot snapshot = new BattleSnapshot();
            snapshot.ReadFromByteArray(byteArray);
            clientService.OnReceiveBattleSnapshot(snapshot);
            return true;
        }

        private static bool OnReceiveMove(ByteArray byteArray, IPokemonBattleClientService clientService)
        {
            PlayerMove move = new PlayerMove();
            move.ReadFromByteArray(byteArray);
            clientService.OnReceiveMove(move);
            return true;
        }

        private static bool OnReceiveRandomSeed(ByteArray byteArray, IPokemonBattleClientService clientService)
        {
            int seed = byteArray.ReadInt();
            clientService.OnReceiveRandomSeed(seed);
            return true;
        }

        private static bool OnReceiveRules(ByteArray byteArray, IPokemonBattleClientService clientService)
        {
            BattleRuleSequence rules = new BattleRuleSequence();
            rules.ReadFromByteArray(byteArray);
            clientService.OnReceiveRules(rules);
            return true;
        }

        private static bool OnReceiveTeam(ByteArray byteArray, IPokemonBattleClientService clientService)
        {
            byte position = byteArray.ReadByte();
            string identity = byteArray.ReadUTF();
            ByteSequence team = new ByteSequence();
            team.ReadFromByteArray(byteArray);
            clientService.OnReceiveTeam(position, identity, team);
            return true;
        }

        private static bool OnReceiveTieMessage(ByteArray byteArray, IPokemonBattleClientService clientService)
        {
            string identity = byteArray.ReadUTF();
            TieMessage message = TieMessageHelper.ReadFromByteArray(byteArray);
            clientService.OnReceiveTieMessage(identity, message);
            return true;
        }

        private static bool OnRegistObsever(ByteArray byteArray, IPokemonBattleClientService clientService)
        {
            int identity = byteArray.ReadInt();
            clientService.OnRegistObsever(identity);
            return true;
        }

        private static bool OnTimeUp(ByteArray byteArray, IPokemonBattleClientService clientService)
        {
            string identity = byteArray.ReadUTF();
            clientService.OnTimeUp(identity);
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

