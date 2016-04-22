namespace PokemonBattle.RoomClient
{
    using NetworkLib;
    using PokemonBattle.BattleNetwork;
    using PokemonBattle.BattleRoom.Client;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;

    public class RoomClientForm : Form
    {
        private BroadcastController _bcController;
        private ChatManager _chatManager;
        private PokemonBattle.RoomClient.RoomClient _client;
        private User _myInfo;
        private RoomBattleSetting _roomSetting;
        private string _serverIP;
        private string _version;
        private ToolStripMenuItem AwayMenuItem;
        private Label BcCounterLabel;
        private Label BroadcastLabel;
        internal ToolStripMenuItem ChallengeMenuItem;
        internal TabPage ChatPage;
        private ToolStripMenuItem ChatsMenuItem;
        internal ToolStripMenuItem ChatToMenuItem;
        private CheckBox CommandCheck;
        private IContainer components;
        private Button CreateButton;
        internal TextBox DisplayText;
        internal Button EnterButton;
        private ToolStripMenuItem ExitMenuItem;
        internal ListView FourPlayerList;
        internal TabPage FourPlayerPage;
        private MenuStrip MainMenu;
        internal TextBox MessageText;
        internal ToolStripMenuItem ObserveMenuItem;
        internal ColumnHeader PlayerCount;
        private ToolStripMenuItem RoomMenuItem;
        internal ColumnHeader RoomNameColumn;
        internal TabControl RoomTab;
        internal Button SendButton;
        internal ColumnHeader StateColumn;
        internal ToolStripSeparator ToolStripSeparator1;
        internal ImageList UserImage;
        private ToolStripMenuItem UserInfoMenuItem;
        internal ListView UserList;
        internal ContextMenuStrip UserMenu;
        internal ColumnHeader UserNameColumn;

        public event BuildAgentFormDelegate OnBuildBattleAgentForm;

        public event BuildClientFormDelegate OnBuildBattleClientForm;

        public event BuildObserverFormDelegate OnBuildBattleObserverForm;

        public event BuildServerFormDelegate OnBuildBattleServerForm;

        public event BuildFourPlayerFormDelegate OnBuildFourPlayerForm;

        public RoomClientForm(User userInfo, string serverAddress, string roomName, string version)
        {
            this.InitializeComponent();
            this._myInfo = userInfo;
            this._myInfo.State = UserState.Free;
            this._serverIP = serverAddress;
            this._version = version;
            this.Text = string.Format("房间窗口 - {0}", roomName);
        }

        private void _bcController_OnCounterChanged()
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                if (this._bcController.Counter >= 10)
                {
                    this.SendButton.Enabled = false;
                    this.BcCounterLabel.Text = "100%";
                }
                else
                {
                    this.SendButton.Enabled = true;
                    this.BcCounterLabel.Text = ((this._bcController.Counter * 10)).ToString() + "%";
                }
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this._bcController_OnCounterChanged();
                    };
                }
                base.Invoke(method);
            }
        }

        private void _chatManager_OnAddChat(int identity)
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                User user = this._client.GetUser(identity);
                if (user != null)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(user.Name);
                    item.Name = identity.ToString();
                    item.Tag = identity;
                    item.Click += new EventHandler(this.chatMenu_Click);
                    this.ChatsMenuItem.DropDownItems.Add(item);
                    if (!this.ChatsMenuItem.Enabled)
                    {
                        this.ChatsMenuItem.Enabled = true;
                    }
                }
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this._chatManager_OnAddChat(identity);
                    };
                }
                base.Invoke(method);
            }
        }

        private void _chatManager_OnRemoveChat(int identity)
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                this.ChatsMenuItem.DropDownItems.RemoveByKey(identity.ToString());
                if (this.ChatsMenuItem.DropDownItems.Count == 0)
                {
                    this.ChatsMenuItem.Enabled = false;
                }
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this._chatManager_OnRemoveChat(identity);
                    };
                }
                base.Invoke(method);
            }
        }

        private void _client_OnAdd4PRoomList(FourPlayerRoomSequence rooms)
        {
            foreach (FourPlayerRoom room in rooms.Elements)
            {
                ListViewItem item = new ListViewItem(new string[] { room.Name, room.PlayerCount.ToString() });
                item.Name = room.Identity.ToString();
                MethodInvoker methodInvokerDelegate = delegate () {
                    this.FourPlayerList.Items.Add(item);
                };
                base.Invoke(methodInvokerDelegate);
            }
        }

        private void _client_OnBroadcast(string message)
        {
            this.AppendText(message);
        }

        private void _client_OnConnectFail(NetworkException error)
        {
            this._client_OnLogonFailed("连接错误.");
        }

        private void _client_OnDisconnected()
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                MessageBox.Show("与房间服务器断开了连接.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                base.Close();
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this._client_OnDisconnected();
                    };
                }
                base.Invoke(method);
            }
        }

        private void _client_OnKicked()
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                MessageBox.Show("你被请出了房间.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                base.Close();
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this._client_OnKicked();
                    };
                }
                base.Invoke(method);
            }
        }

        private void _client_OnLogoned(User info)
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                this._myInfo.Identity = info.Identity;
                this._myInfo.Address = info.Address;
                this.SendButton.Enabled = true;
                this.RoomMenuItem.Enabled = true;
                this.CreateButton.Enabled = true;
                this._chatManager = new ChatManager();
                this._chatManager.OnAddChat += new ChatDelegate(this._chatManager_OnAddChat);
                this._chatManager.OnRemoveChat += new ChatDelegate(this._chatManager_OnRemoveChat);
                this._bcController = new BroadcastController();
                this._bcController.OnCounterChanged += new VoidFunctionDelegate(this._bcController_OnCounterChanged);
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this._client_OnLogoned(info);
                    };
                }
                base.Invoke(method);
            }
        }

        private void _client_OnLogonFailed(string message)
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                MessageBox.Show(string.Format("无法登录房间服务器 : {0}.", message), "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                base.Close();
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this._client_OnLogonFailed(message);
                    };
                }
                base.Invoke(method);
            }
        }

        private void _client_OnObserveBattle(ObserveInfo info)
        {
            if (info.BattleIdentity != -1)
            {
                info.BattleAddress = this._serverIP;
            }
            this.BuildBattleObserverForm(info.BattleIdentity, info.BattleAddress, info.Position);
        }

        private void _client_OnReceiveChallenge(ChallengeForm challenge)
        {
            MethodInvoker methodInvokerDelegate = delegate () {
                challenge.Icon = this.Icon;
                challenge.MdiParent = this.MdiParent;
                challenge.Show();
                challenge.Activate();
            };
            base.Invoke(methodInvokerDelegate);
        }

        private void _client_OnSetting(RoomBattleSetting setting)
        {
            VoidFunctionDelegate method = null;
            this._roomSetting = setting;
            if (!string.IsNullOrEmpty(setting.Version) && (this._version != setting.Version))
            {
                if (method == null)
                {
                    method = delegate {
                        MessageBox.Show(string.Format("当前房间限制用户版本为{0}", setting.Version), "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        this.Close();
                    };
                }
                base.Invoke(method);
            }
        }

        private void AddUserInfo(User userInfo)
        {
            ListViewItem item = new ListViewItem(new string[] { userInfo.Name, this.GetStateString(userInfo.State) });
            item.Name = userInfo.Identity.ToString();
            item.ImageIndex = userInfo.ImageKey;
            MethodInvoker methodInvokerDelegate = delegate () {
                this.UserList.Items.Add(item);
            };
            base.Invoke(methodInvokerDelegate);
        }

        private void AppendText(string text)
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                if (this.DisplayText.TextLength > 0)
                {
                    this.DisplayText.Text = this.DisplayText.Text + "\r\n";
                }
                this.DisplayText.Text = this.DisplayText.Text + string.Format("{0}", text);
                this.DisplayText.SelectionStart = this.DisplayText.Text.Length - 1;
                this.DisplayText.ScrollToCaret();
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this.AppendText(text);
                    };
                }
                base.Invoke(method);
            }
        }

        private void AwayMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (this.AwayMenuItem.Checked)
            {
                this.SetUserState(UserState.Away);
            }
            else
            {
                this.SetUserState(UserState.Free);
            }
        }

        private void BattleFormClosed(object sender, FormClosedEventArgs e)
        {
            this.SetUserState(UserState.Free);
        }

        public void BeginEditTeam()
        {
            if (!this.AwayMenuItem.Checked)
            {
                this.AwayMenuItem.Checked = true;
            }
            this.AwayMenuItem.Enabled = false;
        }

        private void Build4PBattleForm(int identity, byte position)
        {
            this.BuildBattleAgentForm(identity, position, BattleMode.Double_4P);
            this._client.StartFourPlayerBattle(identity, position);
        }

        private void BuildBattleAgentForm(int identity, byte myPosition, BattleMode battleMode)
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                AgentBattleInfo battleInfo = new AgentBattleInfo();
                battleInfo.AgentID = identity;
                battleInfo.BattleMode = battleMode;
                battleInfo.Position = myPosition;
                battleInfo.ServerAddress = this._serverIP;
                battleInfo.UserName = this._myInfo.Name;
                battleInfo.MoveInterval = this._roomSetting.MoveInterval;
                Form form = this.HandleBuildBattleAgentFormEvent(battleInfo);
                if (form != null)
                {
                    this.SetUserState(UserState.Battling);
                    form.FormClosed += new FormClosedEventHandler(this.BattleFormClosed);
                    form.Show();
                }
                else
                {
                    MessageBox.Show("please build battle agent form");
                    this.SetUserState(UserState.Free);
                }
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this.BuildBattleAgentForm(identity, myPosition, battleMode);
                    };
                }
                base.Invoke(method);
            }
        }

        private void BuildBattleClientForm(string serverAddress, BattleMode battleMode)
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                Form form = this.HandleBuildBattleClientFormEvent(serverAddress, 2, this._myInfo.Name, battleMode);
                if (form != null)
                {
                    this.SetUserState(UserState.Battling);
                    form.FormClosed += new FormClosedEventHandler(this.BattleFormClosed);
                    form.Show();
                }
                else
                {
                    MessageBox.Show("please build battle client form");
                    this.SetUserState(UserState.Free);
                }
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this.BuildBattleClientForm(serverAddress, battleMode);
                    };
                }
                base.Invoke(method);
            }
        }

        private void BuildBattleObserverForm(int identity, string serverAddress, byte position)
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                Form form = this.HandleBuildBattleObserverFormEvent(identity, serverAddress, position);
                if (form != null)
                {
                    form.Show();
                }
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this.BuildBattleObserverForm(identity, serverAddress, position);
                    };
                }
                base.Invoke(method);
            }
        }

        private void BuildBattleServerForm(BattleMode battleMode, BattleRuleSequence rules)
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                Form form = this.HandleBuildBattleServerFormEvent(this._myInfo.Name, battleMode, rules.Elements);
                if (form != null)
                {
                    this.SetUserState(UserState.Battling);
                    form.FormClosed += new FormClosedEventHandler(this.BattleFormClosed);
                    form.Show();
                }
                else
                {
                    MessageBox.Show("please build battle server form");
                    this.SetUserState(UserState.Free);
                }
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this.BuildBattleServerForm(battleMode, rules);
                    };
                }
                base.Invoke(method);
            }
        }

        private void ChallengeMenuItem_Click(object sender, EventArgs e)
        {
            if (this._roomSetting.SingleBan && this._roomSetting.DoubleBan)
            {
                this.AppendText("提示 : 当前房间禁止单打与双打");
            }
            else
            {
                int selectedUserIdentity = this.SelectedUserIdentity;
                if ((selectedUserIdentity != -1) && (selectedUserIdentity != this._myInfo.Identity))
                {
                    ChallengeForm challengeForm = this._client.GetChallengeForm(selectedUserIdentity);
                    if (challengeForm != null)
                    {
                        challengeForm.Icon = base.Icon;
                        challengeForm.MdiParent = base.MdiParent;
                        challengeForm.Show();
                        this.SetUserState(UserState.Challenging);
                    }
                }
            }
        }

        private void chatMenu_Click(object sender, EventArgs e)
        {
            int tag = (int) (sender as ToolStripMenuItem).Tag;
            this.ShowChat(tag);
        }

        private void ChatToMenuItem_Click(object sender, EventArgs e)
        {
            int selectedUserIdentity = this.SelectedUserIdentity;
            if ((selectedUserIdentity != -1) && (selectedUserIdentity != this._myInfo.Identity))
            {
                this.ShowChat(selectedUserIdentity);
            }
        }

        private void CloseClient()
        {
            if (this._client != null)
            {
                this._client.Stop();
                this._client = null;
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            if (this._client != null)
            {
                this._client.RegistFourPlayer();
                this.CreateButton.Enabled = false;
                this.EnterButton.Enabled = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void EndEditTeam()
        {
            this.AwayMenuItem.Checked = false;
            this.AwayMenuItem.Enabled = true;
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            if (this.FourPlayerList.SelectedItems.Count == 0)
            {
                this.EnterButton.Enabled = false;
            }
            else
            {
                ListViewItem item = this.FourPlayerList.SelectedItems[0];
                int identity = int.Parse(item.Name);
                Form form = this.HandleBuildFourPlayerFormEvent(identity, this._serverIP, this._myInfo.Name, false, new FourPlayerFormCallback(this.Build4PBattleForm));
                if (form != null)
                {
                    this.CreateButton.Enabled = false;
                    this.EnterButton.Enabled = false;
                    form.Show();
                    form.FormClosed += new FormClosedEventHandler(this.FourPlayerForm_FormClosed);
                }
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void FourPlayerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!this._roomSetting.FourPlayerBan)
            {
                this.CreateButton.Enabled = true;
                this.EnterButton.Enabled = true;
            }
        }

        private void FourPlayerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this._roomSetting.FourPlayerBan && (this._myInfo.State != UserState.Away))
            {
                if (this.FourPlayerList.SelectedItems.Count > 0)
                {
                    this.EnterButton.Enabled = true;
                }
                else
                {
                    this.EnterButton.Enabled = false;
                }
            }
        }

        private string GetStateString(UserState state)
        {
            switch (state)
            {
                case UserState.Battling:
                    return "对战中";

                case UserState.Away:
                    return "离开";

                case UserState.Challenging:
                    return "挑战中";

                case UserState.Free:
                    return "空闲";
            }
            return "";
        }

        private Form HandleBuildBattleAgentFormEvent(AgentBattleInfo battleInfo)
        {
            if (this.OnBuildBattleAgentForm != null)
            {
                return this.OnBuildBattleAgentForm(battleInfo);
            }
            return null;
        }

        private Form HandleBuildBattleClientFormEvent(string serverAddress, byte position, string userName, BattleMode mode)
        {
            if (this.OnBuildBattleClientForm != null)
            {
                return this.OnBuildBattleClientForm(serverAddress, position, userName, mode);
            }
            return null;
        }

        private Form HandleBuildBattleObserverFormEvent(int identity, string serverAddress, byte playerPosition)
        {
            if (this.OnBuildBattleObserverForm != null)
            {
                return this.OnBuildBattleObserverForm(identity, serverAddress, playerPosition);
            }
            return null;
        }

        private Form HandleBuildBattleServerFormEvent(string userName, BattleMode mode, List<BattleRule> rules)
        {
            if (this.OnBuildBattleServerForm != null)
            {
                return this.OnBuildBattleServerForm(userName, mode, rules);
            }
            return null;
        }

        private Form HandleBuildFourPlayerFormEvent(int identity, string serverAddress, string userName, bool asHost, FourPlayerFormCallback callback)
        {
            if (this.OnBuildFourPlayerForm != null)
            {
                return this.OnBuildFourPlayerForm(identity, serverAddress, userName, asHost, callback);
            }
            return null;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(RoomClientForm));
            this.UserImage = new ImageList(this.components);
            this.UserList = new ListView();
            this.UserNameColumn = new ColumnHeader();
            this.StateColumn = new ColumnHeader();
            this.UserMenu = new ContextMenuStrip(this.components);
            this.ChatToMenuItem = new ToolStripMenuItem();
            this.ToolStripSeparator1 = new ToolStripSeparator();
            this.ChallengeMenuItem = new ToolStripMenuItem();
            this.ObserveMenuItem = new ToolStripMenuItem();
            this.RoomTab = new TabControl();
            this.ChatPage = new TabPage();
            this.BcCounterLabel = new Label();
            this.BroadcastLabel = new Label();
            this.CommandCheck = new CheckBox();
            this.SendButton = new Button();
            this.MessageText = new TextBox();
            this.DisplayText = new TextBox();
            this.FourPlayerPage = new TabPage();
            this.CreateButton = new Button();
            this.EnterButton = new Button();
            this.FourPlayerList = new ListView();
            this.RoomNameColumn = new ColumnHeader();
            this.PlayerCount = new ColumnHeader();
            this.MainMenu = new MenuStrip();
            this.RoomMenuItem = new ToolStripMenuItem();
            this.UserInfoMenuItem = new ToolStripMenuItem();
            this.AwayMenuItem = new ToolStripMenuItem();
            this.ExitMenuItem = new ToolStripMenuItem();
            this.ChatsMenuItem = new ToolStripMenuItem();
            this.UserMenu.SuspendLayout();
            this.RoomTab.SuspendLayout();
            this.ChatPage.SuspendLayout();
            this.FourPlayerPage.SuspendLayout();
            this.MainMenu.SuspendLayout();
            base.SuspendLayout();
            this.UserImage.ImageStream = (ImageListStreamer) manager.GetObject("UserImage.ImageStream");
            this.UserImage.TransparentColor = Color.Transparent;
            this.UserImage.Images.SetKeyName(0, "Pokemon1.ico");
            this.UserImage.Images.SetKeyName(1, "Pokemon2.ico");
            this.UserImage.Images.SetKeyName(2, "Pokemon3.ico");
            this.UserImage.Images.SetKeyName(3, "Pokemon4.ico");
            this.UserImage.Images.SetKeyName(4, "Pokemon5.ico");
            this.UserImage.Images.SetKeyName(5, "Pokemon6.ico");
            this.UserImage.Images.SetKeyName(6, "Pokemon7.ico");
            this.UserImage.Images.SetKeyName(7, "Pokemon8.ico");
            this.UserImage.Images.SetKeyName(8, "Pokemon9.ico");
            this.UserImage.Images.SetKeyName(9, "Pokemon10.ico");
            this.UserImage.Images.SetKeyName(10, "Pokemon11.ico");
            this.UserImage.Images.SetKeyName(11, "Pokemon12.ico");
            this.UserImage.Images.SetKeyName(12, "Pokemon13.ico");
            this.UserImage.Images.SetKeyName(13, "Pokemon14.ico");
            this.UserImage.Images.SetKeyName(14, "Pokemon15.ico");
            this.UserList.Alignment = ListViewAlignment.Left;
            this.UserList.Columns.AddRange(new ColumnHeader[] { this.UserNameColumn, this.StateColumn });
            this.UserList.ContextMenuStrip = this.UserMenu;
            this.UserList.FullRowSelect = true;
            this.UserList.Location = new Point(0x1bd, 0x30);
            this.UserList.MultiSelect = false;
            this.UserList.Name = "UserList";
            this.UserList.Size = new Size(0xc4, 410);
            this.UserList.SmallImageList = this.UserImage;
            this.UserList.Sorting = SortOrder.Ascending;
            this.UserList.TabIndex = 4;
            this.UserList.UseCompatibleStateImageBehavior = false;
            this.UserList.View = View.Details;
            this.UserNameColumn.Text = "名称";
            this.UserNameColumn.Width = 0x71;
            this.StateColumn.Text = "状态";
            this.UserMenu.Items.AddRange(new ToolStripItem[] { this.ChatToMenuItem, this.ToolStripSeparator1, this.ChallengeMenuItem, this.ObserveMenuItem });
            this.UserMenu.Name = "mnuRoomList";
            this.UserMenu.ShowImageMargin = false;
            this.UserMenu.Size = new Size(100, 0x4c);
            this.ChatToMenuItem.Name = "ChatToMenuItem";
            this.ChatToMenuItem.Size = new Size(0x63, 0x16);
            this.ChatToMenuItem.Text = "私人聊天";
            this.ChatToMenuItem.Click += new EventHandler(this.ChatToMenuItem_Click);
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new Size(0x60, 6);
            this.ChallengeMenuItem.Name = "ChallengeMenuItem";
            this.ChallengeMenuItem.Size = new Size(0x63, 0x16);
            this.ChallengeMenuItem.Text = "挑战";
            this.ChallengeMenuItem.Click += new EventHandler(this.ChallengeMenuItem_Click);
            this.ObserveMenuItem.Name = "ObserveMenuItem";
            this.ObserveMenuItem.Size = new Size(0x63, 0x16);
            this.ObserveMenuItem.Text = "观战";
            this.ObserveMenuItem.Click += new EventHandler(this.ObserveMenuItem_Click);
            this.RoomTab.Controls.Add(this.ChatPage);
            this.RoomTab.Controls.Add(this.FourPlayerPage);
            this.RoomTab.Location = new Point(0, 0x1b);
            this.RoomTab.Name = "RoomTab";
            this.RoomTab.SelectedIndex = 0;
            this.RoomTab.Size = new Size(0x1b6, 0x1bb);
            this.RoomTab.TabIndex = 5;
            this.ChatPage.Controls.Add(this.BcCounterLabel);
            this.ChatPage.Controls.Add(this.BroadcastLabel);
            this.ChatPage.Controls.Add(this.CommandCheck);
            this.ChatPage.Controls.Add(this.SendButton);
            this.ChatPage.Controls.Add(this.MessageText);
            this.ChatPage.Controls.Add(this.DisplayText);
            this.ChatPage.Location = new Point(4, 0x16);
            this.ChatPage.Name = "ChatPage";
            this.ChatPage.Padding = new Padding(3);
            this.ChatPage.Size = new Size(430, 0x1a1);
            this.ChatPage.TabIndex = 1;
            this.ChatPage.Text = "房间";
            this.ChatPage.UseVisualStyleBackColor = true;
            this.BcCounterLabel.AutoSize = true;
            this.BcCounterLabel.ForeColor = Color.Red;
            this.BcCounterLabel.Location = new Point(80, 0x18c);
            this.BcCounterLabel.Name = "BcCounterLabel";
            this.BcCounterLabel.Size = new Size(0x11, 12);
            this.BcCounterLabel.TabIndex = 5;
            this.BcCounterLabel.Text = "0%";
            this.BroadcastLabel.AutoSize = true;
            this.BroadcastLabel.Location = new Point(9, 0x18c);
            this.BroadcastLabel.Name = "BroadcastLabel";
            this.BroadcastLabel.Size = new Size(0x41, 12);
            this.BroadcastLabel.TabIndex = 4;
            this.BroadcastLabel.Text = "发言限制 :";
            this.CommandCheck.AutoSize = true;
            this.CommandCheck.Location = new Point(280, 0x187);
            this.CommandCheck.Name = "CommandCheck";
            this.CommandCheck.Size = new Size(0x48, 0x10);
            this.CommandCheck.TabIndex = 3;
            this.CommandCheck.Text = "房间命令";
            this.CommandCheck.UseVisualStyleBackColor = true;
            this.SendButton.Enabled = false;
            this.SendButton.Location = new Point(0x166, 0x183);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new Size(0x3f, 0x17);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "发送(&S)";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new EventHandler(this.SendButton_Click);
            this.MessageText.ImeMode = ImeMode.Alpha;
            this.MessageText.Location = new Point(3, 360);
            this.MessageText.MaxLength = 50;
            this.MessageText.Name = "MessageText";
            this.MessageText.ScrollBars = ScrollBars.Vertical;
            this.MessageText.Size = new Size(0x1a2, 0x15);
            this.MessageText.TabIndex = 1;
            this.DisplayText.BackColor = Color.White;
            this.DisplayText.Dock = DockStyle.Top;
            this.DisplayText.Location = new Point(3, 3);
            this.DisplayText.Multiline = true;
            this.DisplayText.Name = "DisplayText";
            this.DisplayText.ReadOnly = true;
            this.DisplayText.ScrollBars = ScrollBars.Vertical;
            this.DisplayText.Size = new Size(0x1a8, 0x15f);
            this.DisplayText.TabIndex = 0;
            this.FourPlayerPage.Controls.Add(this.CreateButton);
            this.FourPlayerPage.Controls.Add(this.EnterButton);
            this.FourPlayerPage.Controls.Add(this.FourPlayerList);
            this.FourPlayerPage.Location = new Point(4, 0x16);
            this.FourPlayerPage.Name = "FourPlayerPage";
            this.FourPlayerPage.Padding = new Padding(3);
            this.FourPlayerPage.Size = new Size(430, 0x1a1);
            this.FourPlayerPage.TabIndex = 2;
            this.FourPlayerPage.Text = "4P对战";
            this.FourPlayerPage.UseVisualStyleBackColor = true;
            this.CreateButton.Enabled = false;
            this.CreateButton.Location = new Point(0x10c, 0x183);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new Size(0x4b, 0x17);
            this.CreateButton.TabIndex = 5;
            this.CreateButton.Text = "创建(&C)";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new EventHandler(this.CreateButton_Click);
            this.EnterButton.Enabled = false;
            this.EnterButton.Location = new Point(0x15d, 0x183);
            this.EnterButton.Name = "EnterButton";
            this.EnterButton.Size = new Size(0x4b, 0x17);
            this.EnterButton.TabIndex = 4;
            this.EnterButton.Text = "进入(&E)";
            this.EnterButton.UseVisualStyleBackColor = true;
            this.EnterButton.Click += new EventHandler(this.EnterButton_Click);
            this.FourPlayerList.Columns.AddRange(new ColumnHeader[] { this.RoomNameColumn, this.PlayerCount });
            this.FourPlayerList.FullRowSelect = true;
            this.FourPlayerList.Location = new Point(0, 0);
            this.FourPlayerList.MultiSelect = false;
            this.FourPlayerList.Name = "FourPlayerList";
            this.FourPlayerList.Size = new Size(430, 0x173);
            this.FourPlayerList.TabIndex = 3;
            this.FourPlayerList.UseCompatibleStateImageBehavior = false;
            this.FourPlayerList.View = View.Details;
            this.FourPlayerList.SelectedIndexChanged += new EventHandler(this.FourPlayerList_SelectedIndexChanged);
            this.RoomNameColumn.Text = "4P房间";
            this.RoomNameColumn.Width = 0xfc;
            this.PlayerCount.Text = "人数";
            this.PlayerCount.Width = 0x60;
            this.MainMenu.AllowMerge = false;
            this.MainMenu.Items.AddRange(new ToolStripItem[] { this.RoomMenuItem, this.ChatsMenuItem });
            this.MainMenu.Location = new Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new Size(0x284, 0x19);
            this.MainMenu.TabIndex = 6;
            this.RoomMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.UserInfoMenuItem, this.AwayMenuItem, this.ExitMenuItem });
            this.RoomMenuItem.Enabled = false;
            this.RoomMenuItem.Name = "RoomMenuItem";
            this.RoomMenuItem.Size = new Size(60, 0x15);
            this.RoomMenuItem.Text = "房间(&R)";
            this.UserInfoMenuItem.Name = "UserInfoMenuItem";
            this.UserInfoMenuItem.Size = new Size(0x8b, 0x16);
            this.UserInfoMenuItem.Text = "用户信息(&S)";
            this.UserInfoMenuItem.Click += new EventHandler(this.UserInfoMenuItem_Click);
            this.AwayMenuItem.CheckOnClick = true;
            this.AwayMenuItem.Name = "AwayMenuItem";
            this.AwayMenuItem.Size = new Size(0x8b, 0x16);
            this.AwayMenuItem.Text = "离开(&A)";
            this.AwayMenuItem.CheckedChanged += new EventHandler(this.AwayMenuItem_CheckedChanged);
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new Size(0x8b, 0x16);
            this.ExitMenuItem.Text = "退出(&X)";
            this.ExitMenuItem.Click += new EventHandler(this.ExitMenuItem_Click);
            this.ChatsMenuItem.Enabled = false;
            this.ChatsMenuItem.Name = "ChatsMenuItem";
            this.ChatsMenuItem.Size = new Size(0x54, 0x15);
            this.ChatsMenuItem.Text = "私人聊天(&C)";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x284, 470);
            base.Controls.Add(this.MainMenu);
            base.Controls.Add(this.RoomTab);
            base.Controls.Add(this.UserList);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.Name = "RoomClientForm";
            this.Text = "房间窗口";
            base.Load += new EventHandler(this.RoomUserForm_Load);
            this.UserMenu.ResumeLayout(false);
            this.RoomTab.ResumeLayout(false);
            this.ChatPage.ResumeLayout(false);
            this.ChatPage.PerformLayout();
            this.FourPlayerPage.ResumeLayout(false);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public static bool IsEmptyString(string str)
        {
            return Regex.IsMatch(str, @"^\s*$");
        }

        private void MessageText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.SendButton.PerformClick();
            }
        }

        private void ObserveMenuItem_Click(object sender, EventArgs e)
        {
            if (this._client != null)
            {
                int selectedUserIdentity = this.SelectedUserIdentity;
                User user = this._client.GetUser(selectedUserIdentity);
                if ((user != null) && (user.State == UserState.Battling))
                {
                    this._client.ObserveBattle(selectedUserIdentity);
                }
            }
        }

        private void OnAdd4PRoom(int identity, string message)
        {
            ListViewItem item = new ListViewItem(new string[] { message, "1" });
            item.Name = identity.ToString();
            MethodInvoker methodInvokerDelegate = delegate () {
                this.FourPlayerList.Items.Add(item);
            };
            base.Invoke(methodInvokerDelegate);
        }

        private void OnChat(int from, string message)
        {
            this._chatManager.PassChatMessage(from, message);
        }

        private void OnRemove4PRoom(int identity)
        {
            MethodInvoker methodInvokerDelegate = delegate () {
                this.FourPlayerList.Items.RemoveByKey(identity.ToString());
            };
            base.Invoke(methodInvokerDelegate);
        }

        private void OnStart4PHost(int identity)
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                Form form = this.HandleBuildFourPlayerFormEvent(identity, this._serverIP, this._myInfo.Name, true, new FourPlayerFormCallback(this.Build4PBattleForm));
                if (form != null)
                {
                    form.Show();
                    form.FormClosed += new FormClosedEventHandler(this.FourPlayerForm_FormClosed);
                }
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this.OnStart4PHost(identity);
                    };
                }
                base.Invoke(method);
            }
        }

        private void OnUpdate4PRoom(int identity, byte count)
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                ListViewItem[] itemArray = this.FourPlayerList.Items.Find(identity.ToString(), false);
                if (itemArray.Length > 0)
                {
                    itemArray[0].SubItems[1].Text = count.ToString();
                }
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this.OnUpdate4PRoom(identity, count);
                    };
                }
                base.Invoke(method);
            }
        }

        private void RemoveUserInfo(int identity)
        {
            MethodInvoker methodInvokerDelegate = delegate () {
                this.UserList.Items.RemoveByKey(identity.ToString());
            };
            base.Invoke(methodInvokerDelegate);
        }

        private void RoomUserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.CloseClient();
            if (this._bcController != null)
            {
                this._bcController.Stop();
            }
        }

        private void RoomUserForm_Load(object sender, EventArgs e)
        {
            this.RunClient();
            base.FormClosed += new FormClosedEventHandler(this.RoomUserForm_FormClosed);
            this.MessageText.KeyDown += new KeyEventHandler(this.MessageText_KeyDown);
            this.UserList.ListViewItemSorter = new ListViewSorter(0);
        }

        private void RunClient()
        {
            this._client = new PokemonBattle.RoomClient.RoomClient(this._serverIP, this._myInfo);
            this._client.OnAddUserInfo += new UserDelegate(this.AddUserInfo);
            this._client.OnUpdateUserInfo += new UserDelegate(this.UpdateUserInfo);
            this._client.OnUpdateUserList += new UserListDelegate(this.UpdateUserList);
            this._client.OnRemoveUserInfo += new PokemonBattle.RoomClient.IdentityDelegate(this.RemoveUserInfo);
            this._client.OnConnectFail += new NetworkErrorDelegate(this._client_OnConnectFail);
            this._client.OnLogonFailed += new PokemonBattle.RoomClient.MessageDelegate(this._client_OnLogonFailed);
            this._client.OnDisconnected += new NetworkEventDelegate(this._client_OnDisconnected);
            this._client.OnKicked += new VoidFunctionDelegate(this._client_OnKicked);
            this._client.OnLogoned += new UserDelegate(this._client_OnLogoned);
            this._client.OnSetting += new SettingDelegate(this._client_OnSetting);
            this._client.OnReceiveChat += new IdentityMessageDelegate(this.OnChat);
            this._client.OnReceiveBroadcast += new PokemonBattle.RoomClient.MessageDelegate(this._client_OnBroadcast);
            this._client.OnReceiveChallenge += new ReceiveChallengeDelegate(this._client_OnReceiveChallenge);
            this._client.OnStartAgentBattle += new AgentBattleDelegate(this.BuildBattleAgentForm);
            this._client.OnStartDirectBattle += new DirectBattleDelegate(this.BuildBattleClientForm);
            this._client.OnBuildBattleServer += new BuildServerDelegate(this.BuildBattleServerForm);
            this._client.OnObserveBattle += new ObserveBattleDelegate(this._client_OnObserveBattle);
            this._client.OnAdd4PRoom += new IdentityMessageDelegate(this.OnAdd4PRoom);
            this._client.OnRemove4PRoom += new PokemonBattle.RoomClient.IdentityDelegate(this.OnRemove4PRoom);
            this._client.OnUpdate4PRoom += new UpdateCountDelegate(this.OnUpdate4PRoom);
            this._client.OnStart4PHost += new PokemonBattle.RoomClient.IdentityDelegate(this.OnStart4PHost);
            this._client.OnAdd4PRoomList += new FourPlayerRoomListDelegate(this._client_OnAdd4PRoomList);
            this._client.Initialize();
            this._client.RunThread();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (!IsEmptyString(this.MessageText.Text))
            {
                if (this.CommandCheck.Checked)
                {
                    this._client.RoomCommand(this.MessageText.Text);
                }
                else
                {
                    this._client.Broadcast(string.Format("{0} : {1}", this._myInfo.Name, this.MessageText.Text));
                    this._bcController.Tick();
                }
                this.MessageText.Clear();
            }
        }

        private void SetUserState(UserState state)
        {
            this._myInfo.State = state;
            if (this._client != null)
            {
                this._client.UpdateInfo();
            }
        }

        private void ShowChat(int target)
        {
            ChatForm chatForm = this._chatManager.GetChatForm(target);
            if (chatForm == null)
            {
                this._chatManager.BuildChatForm(target, this._myInfo.Name, this._client);
                chatForm = this._chatManager.GetChatForm(target);
            }
            chatForm.Icon = base.Icon;
            chatForm.MdiParent = base.MdiParent;
            chatForm.Show();
        }

        public void UpdateCustomData(string name, string hash)
        {
            this._myInfo.CustomDataHash = hash;
            this._myInfo.CustomDataInfo = name;
            if (this._client != null)
            {
                this._client.UpdateInfo();
            }
        }

        private void UpdateUserInfo(User userInfo)
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                ListViewItem[] itemArray = this.UserList.Items.Find(userInfo.Identity.ToString(), false);
                if (itemArray.Length > 0)
                {
                    itemArray[0].SubItems[0].Text = userInfo.Name;
                    itemArray[0].SubItems[1].Text = this.GetStateString(userInfo.State);
                    itemArray[0].ImageIndex = userInfo.ImageKey;
                }
                if (userInfo.Identity == this._myInfo.Identity)
                {
                    this.UpdateUserState();
                }
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this.UpdateUserInfo(userInfo);
                    };
                }
                base.Invoke(method);
            }
        }

        private void UpdateUserList(List<User> users)
        {
            VoidFunctionDelegate method = null;
            if (!base.InvokeRequired)
            {
                this.UserList.BeginUpdate();
                this.UserList.Items.Clear();
                foreach (User user in users)
                {
                    this.AddUserInfo(user);
                }
                this.UserList.EndUpdate();
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this.UpdateUserList(users);
                    };
                }
                base.Invoke(method);
            }
        }

        private void UpdateUserState()
        {
            switch (this._myInfo.State)
            {
                case UserState.Battling:
                    this.AwayMenuItem.Enabled = false;
                    this.ChallengeMenuItem.Enabled = false;
                    this.CreateButton.Enabled = false;
                    this.EnterButton.Enabled = false;
                    break;

                case UserState.Away:
                    this.ChallengeMenuItem.Enabled = false;
                    this.CreateButton.Enabled = false;
                    this.EnterButton.Enabled = false;
                    break;

                case UserState.Challenging:
                    this.AwayMenuItem.Enabled = false;
                    this.ChallengeMenuItem.Enabled = false;
                    this.CreateButton.Enabled = false;
                    this.EnterButton.Enabled = false;
                    return;

                case UserState.Free:
                    this.AwayMenuItem.Enabled = true;
                    this.ChallengeMenuItem.Enabled = true;
                    if (!this._roomSetting.FourPlayerBan)
                    {
                        this.CreateButton.Enabled = true;
                    }
                    return;
            }
        }

        private void UserInfoMenuItem_Click(object sender, EventArgs e)
        {
            UserInfoForm form = new UserInfoForm(this._myInfo);
            form.Icon = base.Icon;
            if (form.ShowDialog() == DialogResult.OK)
            {
                this._client.UpdateInfo();
            }
        }

        private int SelectedUserIdentity
        {
            get
            {
                if (this.UserList.SelectedItems.Count > 0)
                {
                    return int.Parse(this.UserList.SelectedItems[0].Name);
                }
                return -1;
            }
        }
    }
}

