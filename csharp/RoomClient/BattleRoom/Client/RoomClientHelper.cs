namespace PokemonBattle.BattleRoom.Client
{
    using NetworkLib.Utilities;
    using PokemonBattle.BattleNetwork;
    using System;

    public class RoomClientHelper
    {
        public static ByteArray AcceptChallenge(int targetIdentity)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(-1374719948);
            array.WriteInt(targetIdentity);
            return array;
        }

        public static ByteArray CancelChallenge(int targetIdentity)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(-1060517986);
            array.WriteInt(targetIdentity);
            return array;
        }

        public static ByteArray Challenge(int targetIdentity, ChallengeInfo info)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.WriteInt(-255202176);
            byteArray.WriteInt(targetIdentity);
            info.WriteToByteArray(byteArray);
            return byteArray;
        }

        public static ByteArray GetObserveInfo(int identity)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(-935233673);
            array.WriteInt(identity);
            return array;
        }

        public static bool InterpretMessage(int sessionID, ByteArray byteArray, IRoomClientService clientService)
        {
            byteArray.BypassHeader();
            switch (byteArray.ReadInt())
            {
                case -1532911397:
                    return OnBeKicked(byteArray, clientService);

                case -1374719948:
                    return OnAcceptChallenge(byteArray, clientService);

                case -1356060584:
                    return OnRemoveUser(byteArray, clientService);

                case -1990948387:
                    return OnRefuseChallenge(byteArray, clientService);

                case -1971873217:
                    return OnAddNewUser(byteArray, clientService);

                case -1293976651:
                    return OnDirectBattle(byteArray, clientService);

                case -1166474376:
                    return OnAddFourPlayerRoomList(byteArray, clientService);

                case -1060517986:
                    return OnCancelChallenge(byteArray, clientService);

                case -842583856:
                    return OnAgentBattle(byteArray, clientService);

                case -632492989:
                    return OnLogonFail(byteArray, clientService);

                case -547480323:
                    return OnUpdateRoomSetting(byteArray, clientService);

                case 0x19fd643c:
                    return OnAddFourPlayerRoom(byteArray, clientService);

                case 0x22a12498:
                    return OnRemoveFourPlayerRoom(byteArray, clientService);

                case 0x22af2d8b:
                    return OnRegistFourPlayerSuccess(byteArray, clientService);

                case -255202176:
                    return OnChallenge(byteArray, clientService);

                case 0x13ca42b9:
                    return OnReceiveObserveInfo(byteArray, clientService);

                case 0x3e04cac0:
                    return OnReceiveChatMessage(byteArray, clientService);

                case 0x429af0d6:
                    return OnReceiveBroadcastMessage(byteArray, clientService);

                case 0x4680c78b:
                    return OnUpdateUser(byteArray, clientService);

                case 0x72d44af6:
                    return OnAddUserList(byteArray, clientService);

                case 0x73500aff:
                    return OnUpdateFourPlayerRoom(byteArray, clientService);

                case 0x7ada52ee:
                    return OnLogonSuccess(byteArray, clientService);
            }
            byteArray.Rewind();
            return false;
        }

        private static bool OnAcceptChallenge(ByteArray byteArray, IRoomClientService clientService)
        {
            int from = byteArray.ReadInt();
            clientService.OnAcceptChallenge(from);
            return true;
        }

        private static bool OnAddFourPlayerRoom(ByteArray byteArray, IRoomClientService clientService)
        {
            int identity = byteArray.ReadInt();
            string host = byteArray.ReadUTF();
            clientService.OnAddFourPlayerRoom(identity, host);
            return true;
        }

        private static bool OnAddFourPlayerRoomList(ByteArray byteArray, IRoomClientService clientService)
        {
            FourPlayerRoomSequence rooms = new FourPlayerRoomSequence();
            rooms.ReadFromByteArray(byteArray);
            clientService.OnAddFourPlayerRoomList(rooms);
            return true;
        }

        private static bool OnAddNewUser(ByteArray byteArray, IRoomClientService clientService)
        {
            User userInfo = new User();
            userInfo.ReadFromByteArray(byteArray);
            clientService.OnAddNewUser(userInfo);
            return true;
        }

        private static bool OnAddUserList(ByteArray byteArray, IRoomClientService clientService)
        {
            UserSequence users = new UserSequence();
            users.ReadFromByteArray(byteArray);
            clientService.OnAddUserList(users);
            return true;
        }

        private static bool OnAgentBattle(ByteArray byteArray, IRoomClientService clientService)
        {
            int identity = byteArray.ReadInt();
            byte playerPosition = byteArray.ReadByte();
            BattleMode battleMode = BattleModeHelper.ReadFromByteArray(byteArray);
            clientService.OnAgentBattle(identity, playerPosition, battleMode);
            return true;
        }

        private static bool OnBeKicked(ByteArray byteArray, IRoomClientService clientService)
        {
            clientService.OnBeKicked();
            return true;
        }

        private static bool OnCancelChallenge(ByteArray byteArray, IRoomClientService clientService)
        {
            int from = byteArray.ReadInt();
            clientService.OnCancelChallenge(from);
            return true;
        }

        private static bool OnChallenge(ByteArray byteArray, IRoomClientService clientService)
        {
            int from = byteArray.ReadInt();
            ChallengeInfo info = new ChallengeInfo();
            info.ReadFromByteArray(byteArray);
            clientService.OnChallenge(from, info);
            return true;
        }

        private static bool OnDirectBattle(ByteArray byteArray, IRoomClientService clientService)
        {
            int server = byteArray.ReadInt();
            BattleMode battleMode = BattleModeHelper.ReadFromByteArray(byteArray);
            clientService.OnDirectBattle(server, battleMode);
            return true;
        }

        private static bool OnLogonFail(ByteArray byteArray, IRoomClientService clientService)
        {
            string message = byteArray.ReadUTF();
            clientService.OnLogonFail(message);
            return true;
        }

        private static bool OnLogonSuccess(ByteArray byteArray, IRoomClientService clientService)
        {
            User info = new User();
            info.ReadFromByteArray(byteArray);
            clientService.OnLogonSuccess(info);
            return true;
        }

        private static bool OnReceiveBroadcastMessage(ByteArray byteArray, IRoomClientService clientService)
        {
            string message = byteArray.ReadUTF();
            clientService.OnReceiveBroadcastMessage(message);
            return true;
        }

        private static bool OnReceiveChatMessage(ByteArray byteArray, IRoomClientService clientService)
        {
            int from = byteArray.ReadInt();
            string message = byteArray.ReadUTF();
            clientService.OnReceiveChatMessage(from, message);
            return true;
        }

        private static bool OnReceiveObserveInfo(ByteArray byteArray, IRoomClientService clientService)
        {
            ObserveInfo info = new ObserveInfo();
            info.ReadFromByteArray(byteArray);
            clientService.OnReceiveObserveInfo(info);
            return true;
        }

        private static bool OnRefuseChallenge(ByteArray byteArray, IRoomClientService clientService)
        {
            int from = byteArray.ReadInt();
            clientService.OnRefuseChallenge(from);
            return true;
        }

        private static bool OnRegistFourPlayerSuccess(ByteArray byteArray, IRoomClientService clientService)
        {
            int identity = byteArray.ReadInt();
            clientService.OnRegistFourPlayerSuccess(identity);
            return true;
        }

        private static bool OnRemoveFourPlayerRoom(ByteArray byteArray, IRoomClientService clientService)
        {
            int identity = byteArray.ReadInt();
            clientService.OnRemoveFourPlayerRoom(identity);
            return true;
        }

        private static bool OnRemoveUser(ByteArray byteArray, IRoomClientService clientService)
        {
            int identity = byteArray.ReadInt();
            clientService.OnRemoveUser(identity);
            return true;
        }

        private static bool OnUpdateFourPlayerRoom(ByteArray byteArray, IRoomClientService clientService)
        {
            int identity = byteArray.ReadInt();
            byte userCount = byteArray.ReadByte();
            clientService.OnUpdateFourPlayerRoom(identity, userCount);
            return true;
        }

        private static bool OnUpdateRoomSetting(ByteArray byteArray, IRoomClientService clientService)
        {
            RoomBattleSetting setting = new RoomBattleSetting();
            setting.ReadFromByteArray(byteArray);
            clientService.OnUpdateRoomSetting(setting);
            return true;
        }

        private static bool OnUpdateUser(ByteArray byteArray, IRoomClientService clientService)
        {
            User userInfo = new User();
            userInfo.ReadFromByteArray(byteArray);
            clientService.OnUpdateUser(userInfo);
            return true;
        }

        public static ByteArray ReceiveBroadcastMessage(string message)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(0x429af0d6);
            array.WriteUTF(message);
            return array;
        }

        public static ByteArray ReceiveChatMessage(int to, string message)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(0x3e04cac0);
            array.WriteInt(to);
            array.WriteUTF(message);
            return array;
        }

        public static ByteArray ReceiveRoomCommand(string message)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(0xf23f2e0);
            array.WriteUTF(message);
            return array;
        }

        public static ByteArray RefuseChallenge(int targetIdentity)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(-1990948387);
            array.WriteInt(targetIdentity);
            return array;
        }

        public static ByteArray RegistFourPlayer()
        {
            ByteArray array = new ByteArray();
            array.WriteInt(-1248496399);
            return array;
        }

        public static ByteArray StartBattle(int with, ChallengeInfo info)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.WriteInt(-357168322);
            byteArray.WriteInt(with);
            info.WriteToByteArray(byteArray);
            return byteArray;
        }

        public static ByteArray StartFourPlayerBattle(int battleIdentity, byte position)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(0x3eaa61ec);
            array.WriteInt(battleIdentity);
            array.WriteByte(position);
            return array;
        }

        public static ByteArray UpdateUser(User info)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.WriteInt(0x4680c78b);
            info.WriteToByteArray(byteArray);
            return byteArray;
        }

        public static ByteArray UserLogon(User info)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.WriteInt(-394619561);
            info.WriteToByteArray(byteArray);
            return byteArray;
        }

        public static ByteArray UserLogout()
        {
            ByteArray array = new ByteArray();
            array.WriteInt(-389114558);
            return array;
        }
    }
}

