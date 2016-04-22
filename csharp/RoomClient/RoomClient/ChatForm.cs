namespace PokemonBattle.RoomClient
{
    using NetworkLib;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ChatForm : Form
    {
        private PokemonBattle.RoomClient.RoomClient _client;
        private string _myName;
        private int _target;
        private IContainer components;
        private TextBox DisplayText;
        internal Button ExitButton;
        private TextBox MessageText;
        internal Button SendButton;

        public ChatForm(int target, string myName, PokemonBattle.RoomClient.RoomClient client)
        {
            this.InitializeComponent();
            this._target = target;
            this._myName = myName;
            this._client = client;
            this.MessageText.KeyDown += new KeyEventHandler(this.MessageText_KeyDown);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void InitializeComponent()
        {
            this.ExitButton = new Button();
            this.SendButton = new Button();
            this.DisplayText = new TextBox();
            this.MessageText = new TextBox();
            base.SuspendLayout();
            this.ExitButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.ExitButton.Location = new Point(0xf5, 0x120);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new Size(0x3f, 0x17);
            this.ExitButton.TabIndex = 5;
            this.ExitButton.Text = "退出(&E)";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new EventHandler(this.ExitButton_Click);
            this.SendButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.SendButton.Location = new Point(0xb0, 0x120);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new Size(0x3f, 0x17);
            this.SendButton.TabIndex = 4;
            this.SendButton.Text = "发送(&S)";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new EventHandler(this.SendButton_Click);
            this.DisplayText.BackColor = Color.White;
            this.DisplayText.Location = new Point(12, 12);
            this.DisplayText.Multiline = true;
            this.DisplayText.Name = "DisplayText";
            this.DisplayText.ReadOnly = true;
            this.DisplayText.ScrollBars = ScrollBars.Vertical;
            this.DisplayText.Size = new Size(0x128, 0xee);
            this.DisplayText.TabIndex = 6;
            this.MessageText.ImeMode = ImeMode.Alpha;
            this.MessageText.Location = new Point(12, 0x100);
            this.MessageText.MaxLength = 50;
            this.MessageText.Name = "MessageText";
            this.MessageText.Size = new Size(0x128, 0x15);
            this.MessageText.TabIndex = 7;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(320, 0x143);
            base.Controls.Add(this.MessageText);
            base.Controls.Add(this.DisplayText);
            base.Controls.Add(this.ExitButton);
            base.Controls.Add(this.SendButton);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.Name = "ChatForm";
            base.ShowInTaskbar = false;
            this.Text = "私人聊天窗口";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void MessageText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.SendButton.PerformClick();
            }
        }

        public void ReceiveChatMessage(string message)
        {
            this.AppendText(message);
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (!RoomClientForm.IsEmptyString(this.MessageText.Text))
            {
                string message = string.Format("{0} : {1}", this._myName, this.MessageText.Text);
                if (this._client.Chat(this._target, message))
                {
                    this.AppendText(message);
                    this.MessageText.Clear();
                }
                else
                {
                    this.AppendText("对方已经不再房间中.");
                }
            }
        }

        public int Target
        {
            get
            {
                return this._target;
            }
        }
    }
}

