namespace PokemonBattle.RoomClient
{
    using NetworkLib;
    using NetworkLib.Tcp;
    using NetworkLib.Utilities;
    using PokemonBattle.BattleNetwork;
    using PokemonBattle.BattleRoom.Client;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class RoomClient : NetworkClient, IProtocolInterpreter, IRoomClientService
    {
        private ChallengeManager _challengeManager;
        private Dictionary<int, User> _removeUsers = new Dictionary<int, User>();
        private User _userInfo;
        private Dictionary<int, User> _users = new Dictionary<int, User>();

        public event IdentityMessageDelegate OnAdd4PRoom;

        public event FourPlayerRoomListDelegate OnAdd4PRoomList;

        public event UserDelegate OnAddUserInfo;

        public event BuildServerDelegate OnBuildBattleServer;

        public event VoidFunctionDelegate OnKicked;

        public event UserDelegate OnLogoned;

        public event PokemonBattle.RoomClient.MessageDelegate OnLogonFailed;

        public event ObserveBattleDelegate OnObserveBattle;

        public event PokemonBattle.RoomClient.MessageDelegate OnReceiveBroadcast;

        public event ReceiveChallengeDelegate OnReceiveChallenge;

        public event IdentityMessageDelegate OnReceiveChat;

        public event PokemonBattle.RoomClient.IdentityDelegate OnRemove4PRoom;

        public event PokemonBattle.RoomClient.IdentityDelegate OnRemoveUserInfo;

        public event SettingDelegate OnSetting;

        public event PokemonBattle.RoomClient.IdentityDelegate OnStart4PHost;

        public event AgentBattleDelegate OnStartAgentBattle;

        public event DirectBattleDelegate OnStartDirectBattle;

        public event UpdateCountDelegate OnUpdate4PRoom;

        public event UserDelegate OnUpdateUserInfo;

        public event UserListDelegate OnUpdateUserList;

        public RoomClient(string serverIP, User info)
        {
            this._userInfo = info;
            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Port = 0x2758;
            strategy.ServerIP = serverIP;
            strategy.Sync = true;
            base.NetworkStrategy = strategy;
            base._interpreter = this;
            base.UpdateInterval = 200;
            base.OnLogicUpdate += new VoidFunctionDelegate(this.RoomClient_OnLogicUpdate);
            base.OnConnected += new NetworkEventDelegate(this.RoomClient_OnConnected);
            this._challengeManager = new ChallengeManager(this);
        }

        public void AcceptChallenge(int target)
        {
            base.Send(RoomClientHelper.AcceptChallenge(target));
        }

        public void Broadcast(string message)
        {
            base.Send(RoomClientHelper.ReceiveBroadcastMessage(message));
        }

        public void CancelChallenge(int target)
        {
            base.Send(RoomClientHelper.CancelChallenge(target));
            this._userInfo.State = UserState.Free;
            this.UpdateInfo();
        }

        public void Challenge(int target, ChallengeInfo info)
        {
            base.Send(RoomClientHelper.Challenge(target, info));
        }

        public bool Chat(int target, string message)
        {
            if (this._users.ContainsKey(target))
            {
                base.Send(RoomClientHelper.ReceiveChatMessage(target, message));
                return true;
            }
            return false;
        }

        public ChallengeForm GetChallengeForm(int target)
        {
            User user = this.GetUser(target);
            if (user != null)
            {
                if (user.State != UserState.Free)
                {
                    this.OnReceiveBroadcast("提示 : 该用户不是空闲状态,无法挑战.");
                    return null;
                }
                if (!(user.CustomDataHash != this._userInfo.CustomDataHash))
                {
                    return this._challengeManager.Challenge(user);
                }
                string customDataInfo = user.CustomDataInfo;
                if (string.IsNullOrEmpty(customDataInfo))
                {
                    customDataInfo = "空";
                }
                this.OnReceiveBroadcast(string.Format("提示 : 该用户的自定义数据为{0},与你不同,无法挑战.", customDataInfo));
            }
            return null;
        }

        public User GetUser(int identity)
        {
            User user;
            if (this._users.TryGetValue(identity, out user))
            {
                return user;
            }
            return null;
        }

        private void HandleAddUserInfoEvent(User userInfo)
        {
            if (this.OnAddUserInfo != null)
            {
                this.OnAddUserInfo(userInfo);
            }
        }

        private void HandleBuildBattleServerEvent(BattleMode mode, BattleRuleSequence rules)
        {
            if (this.OnBuildBattleServer != null)
            {
                this.OnBuildBattleServer(mode, rules);
            }
        }

        private void HandleOnUpdateUserInfoEvent(User userInfo)
        {
            if (this.OnUpdateUserInfo != null)
            {
                this.OnUpdateUserInfo(userInfo);
            }
        }

        private void HandleRemoveUserInfoEvent(int identity)
        {
            if (this.OnRemoveUserInfo != null)
            {
                this.OnRemoveUserInfo(identity);
            }
        }

        private void HandleUpdateUserListEvent(List<User> users)
        {
            if (this.OnUpdateUserList != null)
            {
                this.OnUpdateUserList(users);
            }
        }

        public bool InterpretMessage(int sessionID, ByteArray byteArray)
        {
            return RoomClientHelper.InterpretMessage(sessionID, byteArray, this);
        }

        public void ObserveBattle(int target)
        {
            base.Send(RoomClientHelper.GetObserveInfo(target));
        }

        public void OnAcceptChallenge(int from)
        {
            this._challengeManager.ChallengeAccepted();
        }

        public void OnAddFourPlayerRoom(int identity, string host)
        {
            if (this.OnAdd4PRoom != null)
            {
                this.OnAdd4PRoom(identity, host);
            }
        }

        public void OnAddFourPlayerRoomList(FourPlayerRoomSequence rooms)
        {
            if (this.OnAdd4PRoomList != null)
            {
                this.OnAdd4PRoomList(rooms);
            }
        }

        public void OnAddNewUser(User userInfo)
        {
            this._users[userInfo.Identity] = userInfo;
            this.HandleAddUserInfoEvent(userInfo);
            Logger.LogInfo("New user info, ID : {0}, Name : {1}", new object[] { userInfo.Identity, userInfo.Name });
        }

        public void OnAddUserList(UserSequence users)
        {
            foreach (User user in users.Elements)
            {
                this._users[user.Identity] = user;
            }
            this.HandleUpdateUserListEvent(users.Elements);
            Logger.LogInfo("Get user list , Length : {0} ", new object[] { users.Elements.Count });
        }

        public void OnAgentBattle(int identity, byte playerPosition, BattleMode battleMode)
        {
            if (this.OnStartAgentBattle != null)
            {
                this.OnStartAgentBattle(identity, playerPosition, battleMode);
            }
        }

        public void OnBeKicked()
        {
            if (this.OnKicked != null)
            {
                this.OnKicked();
            }
        }

        public void OnCancelChallenge(int from)
        {
            this._challengeManager.ChallengeCanceled(from);
        }

        public void OnChallenge(int from, ChallengeInfo info)
        {
            User user = this.GetUser(from);
            if (user != null)
            {
                if (this._userInfo.State == UserState.Free)
                {
                    if (this.OnReceiveChallenge != null)
                    {
                        this.OnReceiveChallenge(this._challengeManager.ReceiveChallenge(user, info));
                    }
                }
                else
                {
                    this.RefuseChallenge(from);
                }
            }
        }

        public void OnDirectBattle(int server, BattleMode battleMode)
        {
            User user = this.GetUser(server);
            if ((user != null) && (this.OnStartDirectBattle != null))
            {
                this.OnStartDirectBattle(user.Address, battleMode);
            }
        }

        public void OnLogonFail(string message)
        {
            if (this.OnLogonFailed != null)
            {
                this.OnLogonFailed(message);
            }
        }

        public void OnLogonSuccess(User info)
        {
            if (this.OnLogoned != null)
            {
                this.OnLogoned(info);
            }
        }

        public void OnReceiveBroadcastMessage(string message)
        {
            if (this.OnReceiveBroadcast != null)
            {
                this.OnReceiveBroadcast(message);
            }
        }

        public void OnReceiveChatMessage(int from, string message)
        {
            if (this.OnReceiveChat != null)
            {
                this.OnReceiveChat(from, message);
            }
        }

        public void OnReceiveObserveInfo(ObserveInfo info)
        {
            if (this.OnObserveBattle != null)
            {
                this.OnObserveBattle(info);
            }
        }

        public void OnRefuseChallenge(int from)
        {
            this._challengeManager.ChallengeRefused();
            this._userInfo.State = UserState.Free;
            this.UpdateInfo();
        }

        public void OnRegistFourPlayerSuccess(int identity)
        {
            if (this.OnStart4PHost != null)
            {
                this.OnStart4PHost(identity);
            }
        }

        public void OnRemoveFourPlayerRoom(int identity)
        {
            if (this.OnRemove4PRoom != null)
            {
                this.OnRemove4PRoom(identity);
            }
        }

        public void OnRemoveUser(int identity)
        {
            User user = this.GetUser(identity);
            if (user != null)
            {
                this._removeUsers[identity] = user;
                if (this._challengeManager.UserLogout(identity))
                {
                    this._userInfo.State = UserState.Free;
                    this.UpdateInfo();
                }
                Logger.LogInfo("Remove user , ID : {0} ", new object[] { identity });
            }
        }

        public void OnUpdateFourPlayerRoom(int identity, byte userCount)
        {
            if (this.OnUpdate4PRoom != null)
            {
                this.OnUpdate4PRoom(identity, userCount);
            }
        }

        public void OnUpdateRoomSetting(RoomBattleSetting setting)
        {
            this._challengeManager.SetSetting(setting);
            if (this.OnSetting != null)
            {
                this.OnSetting(setting);
            }
        }

        public void OnUpdateUser(User userInfo)
        {
            this._users[userInfo.Identity] = userInfo;
            this.HandleOnUpdateUserInfoEvent(userInfo);
            Logger.LogInfo("Update user info, ID : {0}", new object[] { userInfo.Identity });
        }

        public void RefuseChallenge(int target)
        {
            base.Send(RoomClientHelper.RefuseChallenge(target));
        }

        public void RegistFourPlayer()
        {
            base.Send(RoomClientHelper.RegistFourPlayer());
        }

        private void RoomClient_OnConnected()
        {
            base.Send(RoomClientHelper.UserLogon(this._userInfo));
        }

        private void RoomClient_OnLogicUpdate()
        {
            Dictionary<int, User> dictionary = new Dictionary<int, User>();
            this._removeUsers = Interlocked.Exchange<Dictionary<int, User>>(ref dictionary, this._removeUsers);
            foreach (int num in dictionary.Keys)
            {
                this._users.Remove(num);
                this.HandleRemoveUserInfoEvent(num);
            }
        }

        public void RoomCommand(string message)
        {
            base.Send(RoomClientHelper.ReceiveRoomCommand(message));
        }

        public void StartBattle(int with, ChallengeInfo info)
        {
            if (info.LinkMode == BattleLinkMode.Direct)
            {
                this.HandleBuildBattleServerEvent(info.BattleMode, info.Rules);
            }
            base.Send(RoomClientHelper.StartBattle(with, info));
        }

        public void StartFourPlayerBattle(int identity, byte position)
        {
            base.Send(RoomClientHelper.StartFourPlayerBattle(identity, position));
        }

        protected override void StopImpl()
        {
            base.Send(RoomClientHelper.UserLogout());
            base.StopImpl();
        }

        public void UpdateInfo()
        {
            base.Send(RoomClientHelper.UpdateUser(this._userInfo));
        }

        public User UserInfo
        {
            get
            {
                return this._userInfo;
            }
        }
    }
}

