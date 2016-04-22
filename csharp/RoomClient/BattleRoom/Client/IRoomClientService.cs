namespace PokemonBattle.BattleRoom.Client
{
    using PokemonBattle.BattleNetwork;
    using System;

    public interface IRoomClientService
    {
        void OnAcceptChallenge(int from);
        void OnAddFourPlayerRoom(int identity, string host);
        void OnAddFourPlayerRoomList(FourPlayerRoomSequence rooms);
        void OnAddNewUser(User userInfo);
        void OnAddUserList(UserSequence users);
        void OnAgentBattle(int identity, byte playerPosition, BattleMode battleMode);
        void OnBeKicked();
        void OnCancelChallenge(int from);
        void OnChallenge(int from, ChallengeInfo info);
        void OnDirectBattle(int server, BattleMode battleMode);
        void OnLogonFail(string message);
        void OnLogonSuccess(User info);
        void OnReceiveBroadcastMessage(string message);
        void OnReceiveChatMessage(int from, string message);
        void OnReceiveObserveInfo(ObserveInfo info);
        void OnRefuseChallenge(int from);
        void OnRegistFourPlayerSuccess(int identity);
        void OnRemoveFourPlayerRoom(int identity);
        void OnRemoveUser(int identity);
        void OnUpdateFourPlayerRoom(int identity, byte userCount);
        void OnUpdateRoomSetting(RoomBattleSetting setting);
        void OnUpdateUser(User userInfo);
    }
}

