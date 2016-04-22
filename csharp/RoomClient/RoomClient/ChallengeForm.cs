namespace PokemonBattle.RoomClient
{
    using PokemonBattle.BattleNetwork;
    using PokemonBattle.BattleRoom.Client;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ChallengeForm : Form
    {
        private PokemonBattle.BattleRoom.Client.ChallengeInfo _challengeInfo;
        private bool _myChallenge;
        private User _targetInfo;
        internal Button Cancel_Button;
        private IContainer components;
        internal Label CustomLabel;
        internal ComboBox LinkCombo;
        internal Label LinkLabel;
        internal ComboBox ModeCombo;
        internal Label ModeLabel;
        internal Label NameLabel;
        internal Button OK_Button;
        internal CheckBox PPUpCheck;
        internal CheckBox RandomCheck;
        internal GroupBox RuleGroup;

        public ChallengeForm(User target)
        {
            this.InitializeComponent();
            this._targetInfo = target;
            this._challengeInfo = new PokemonBattle.BattleRoom.Client.ChallengeInfo();
            this._challengeInfo.Rules = new BattleRuleSequence();
            this._myChallenge = true;
            this.OK_Button.Text = "确定(&O)";
            this.Cancel_Button.Text = "取消(&A)";
            this.NameLabel.Text = string.Format("对象 : {0}", this._targetInfo.Name);
            this.CustomLabel.Text = string.Format("自定数据 : {0}", this._targetInfo.CustomDataInfo);
            this.ModeCombo.SelectedIndex = 0;
            this.LinkCombo.SelectedIndex = 0;
            this.Cancel_Button.Click += new EventHandler(this.Cancel_Button_Click);
        }

        public ChallengeForm(User from, PokemonBattle.BattleRoom.Client.ChallengeInfo challenge)
        {
            this.InitializeComponent();
            this._targetInfo = from;
            this._challengeInfo = challenge;
            this._myChallenge = false;
            this.LinkCombo.Enabled = false;
            switch (challenge.LinkMode)
            {
                case BattleLinkMode.Agent:
                    this.LinkCombo.SelectedIndex = 1;
                    break;

                case BattleLinkMode.Direct:
                    this.LinkCombo.SelectedIndex = 0;
                    break;
            }
            this.ModeCombo.Enabled = false;
            BattleMode battleMode = challenge.BattleMode;
            if (battleMode == BattleMode.Single)
            {
                this.ModeCombo.SelectedIndex = 0;
            }
            else if (battleMode == BattleMode.Double)
            {
                this.ModeCombo.SelectedIndex = 1;
            }
            this.PPUpCheck.Enabled = false;
            if (challenge.Rules.Elements.Contains(BattleRule.PPUp))
            {
                this.PPUpCheck.Checked = true;
            }
            this.RandomCheck.Enabled = false;
            if (challenge.Rules.Elements.Contains(BattleRule.Random))
            {
                this.RandomCheck.Checked = true;
            }
            this.OK_Button.Text = "接受(&A)";
            this.Cancel_Button.Text = "拒绝(&R)";
            this.NameLabel.Text = string.Format("来自 : {0}", this._targetInfo.Name);
            this.CustomLabel.Text = string.Format("自定数据 : {0}", this._targetInfo.CustomDataInfo);
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.RuleGroup = new GroupBox();
            this.RandomCheck = new CheckBox();
            this.PPUpCheck = new CheckBox();
            this.CustomLabel = new Label();
            this.ModeLabel = new Label();
            this.ModeCombo = new ComboBox();
            this.LinkCombo = new ComboBox();
            this.LinkLabel = new Label();
            this.Cancel_Button = new Button();
            this.OK_Button = new Button();
            this.NameLabel = new Label();
            this.RuleGroup.SuspendLayout();
            base.SuspendLayout();
            this.RuleGroup.Controls.Add(this.RandomCheck);
            this.RuleGroup.Controls.Add(this.PPUpCheck);
            this.RuleGroup.Location = new Point(13, 0x95);
            this.RuleGroup.Name = "RuleGroup";
            this.RuleGroup.Size = new Size(0x10b, 0x3f);
            this.RuleGroup.TabIndex = 0x19;
            this.RuleGroup.TabStop = false;
            this.RuleGroup.Text = "可选规则";
            this.RandomCheck.AutoSize = true;
            this.RandomCheck.Location = new Point(0xb1, 20);
            this.RandomCheck.Name = "RandomCheck";
            this.RandomCheck.Size = new Size(0x48, 0x10);
            this.RandomCheck.TabIndex = 1;
            this.RandomCheck.Text = "随机队伍";
            this.RandomCheck.UseVisualStyleBackColor = true;
            this.PPUpCheck.AutoSize = true;
            this.PPUpCheck.Checked = true;
            this.PPUpCheck.CheckState = CheckState.Checked;
            this.PPUpCheck.Location = new Point(14, 20);
            this.PPUpCheck.Name = "PPUpCheck";
            this.PPUpCheck.Size = new Size(0x54, 0x10);
            this.PPUpCheck.TabIndex = 0;
            this.PPUpCheck.Text = "PP上限提升";
            this.PPUpCheck.UseVisualStyleBackColor = true;
            this.CustomLabel.AutoSize = true;
            this.CustomLabel.Location = new Point(0x38, 0x84);
            this.CustomLabel.Name = "CustomLabel";
            this.CustomLabel.Size = new Size(0, 12);
            this.CustomLabel.TabIndex = 0x18;
            this.CustomLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.ModeLabel.AutoSize = true;
            this.ModeLabel.Location = new Point(0x35, 0x6a);
            this.ModeLabel.Name = "ModeLabel";
            this.ModeLabel.Size = new Size(0x41, 12);
            this.ModeLabel.TabIndex = 0x17;
            this.ModeLabel.Text = "对战模式 :";
            this.ModeLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.ModeCombo.FormattingEnabled = true;
            this.ModeCombo.Items.AddRange(new object[] { "1V1", "2V2" });
            this.ModeCombo.Location = new Point(0x7c, 0x67);
            this.ModeCombo.Name = "ModeCombo";
            this.ModeCombo.Size = new Size(0x5e, 20);
            this.ModeCombo.TabIndex = 0x16;
            this.LinkCombo.FormattingEnabled = true;
            this.LinkCombo.Items.AddRange(new object[] { "直连", "服务器中转" });
            this.LinkCombo.Location = new Point(0x7c, 0x4d);
            this.LinkCombo.Name = "LinkCombo";
            this.LinkCombo.Size = new Size(0x5e, 20);
            this.LinkCombo.TabIndex = 0x15;
            this.LinkLabel.AutoSize = true;
            this.LinkLabel.Location = new Point(0x35, 80);
            this.LinkLabel.Name = "LinkLabel";
            this.LinkLabel.Size = new Size(0x41, 12);
            this.LinkLabel.TabIndex = 20;
            this.LinkLabel.Text = "连接方式 :";
            this.LinkLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Cancel_Button.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.Cancel_Button.Location = new Point(0xb0, 0xda);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new Size(0x53, 0x1f);
            this.Cancel_Button.TabIndex = 0x12;
            this.Cancel_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.OK_Button.Location = new Point(0x23, 0xda);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new Size(0x53, 0x1f);
            this.OK_Button.TabIndex = 0x11;
            this.OK_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Click += new EventHandler(this.OK_Button_Click);
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new Font("宋体", 19f);
            this.NameLabel.Location = new Point(0x2f, 15);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new Size(0, 0x1a);
            this.NameLabel.TabIndex = 0x13;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x124, 0x10c);
            base.Controls.Add(this.RuleGroup);
            base.Controls.Add(this.CustomLabel);
            base.Controls.Add(this.ModeLabel);
            base.Controls.Add(this.ModeCombo);
            base.Controls.Add(this.LinkCombo);
            base.Controls.Add(this.LinkLabel);
            base.Controls.Add(this.Cancel_Button);
            base.Controls.Add(this.OK_Button);
            base.Controls.Add(this.NameLabel);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ChallengeForm";
            base.ShowInTaskbar = false;
            this.Text = "挑战窗口";
            this.RuleGroup.ResumeLayout(false);
            this.RuleGroup.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            if (this._myChallenge)
            {
                this._challengeInfo.BattleMode = new BattleMode[] { BattleMode.Single, BattleMode.Double }[this.ModeCombo.SelectedIndex];
                this._challengeInfo.LinkMode = new BattleLinkMode[] { BattleLinkMode.Direct, BattleLinkMode.Agent }[this.LinkCombo.SelectedIndex];
                if (this.PPUpCheck.Checked)
                {
                    this._challengeInfo.Rules.Elements.Add(BattleRule.PPUp);
                }
                if (this.RandomCheck.Checked)
                {
                    this._challengeInfo.Rules.Elements.Add(BattleRule.Random);
                }
                this.OK_Button.Enabled = false;
                this.LinkCombo.Enabled = false;
                this.ModeCombo.Enabled = false;
                this.PPUpCheck.Enabled = false;
                this.RandomCheck.Enabled = false;
                this.Cancel_Button.Click -= new EventHandler(this.Cancel_Button_Click);
            }
        }

        public PokemonBattle.BattleRoom.Client.ChallengeInfo ChallengeInfo
        {
            get
            {
                return this._challengeInfo;
            }
        }

        public User TargetInfo
        {
            get
            {
                return this._targetInfo;
            }
        }
    }
}

