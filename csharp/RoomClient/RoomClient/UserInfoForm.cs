namespace PokemonBattle.RoomClient
{
    using PokemonBattle.BattleRoom.Client;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class UserInfoForm : Form
    {
        private int _imageIndex;
        private User _userInfo;
        private Button Cancel_Button;
        private IContainer components;
        internal Button ImageDownButton;
        internal Button ImageUpButton;
        internal PictureBox MyImage;
        internal Label NameLabel;
        private TextBox NameText;
        private Button OK_Button;
        internal ImageList UserImage;

        public UserInfoForm(User userInfo)
        {
            this.InitializeComponent();
            this._userInfo = userInfo;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ImageDownButton_Click(object sender, EventArgs e)
        {
            this._imageIndex--;
            this.UpdateImage();
        }

        private void ImageUpButton_Click(object sender, EventArgs e)
        {
            this._imageIndex++;
            this.UpdateImage();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(UserInfoForm));
            this.ImageDownButton = new Button();
            this.ImageUpButton = new Button();
            this.MyImage = new PictureBox();
            this.NameLabel = new Label();
            this.UserImage = new ImageList(this.components);
            this.NameText = new TextBox();
            this.Cancel_Button = new Button();
            this.OK_Button = new Button();
            ((ISupportInitialize) this.MyImage).BeginInit();
            base.SuspendLayout();
            this.ImageDownButton.Enabled = false;
            this.ImageDownButton.FlatAppearance.BorderColor = Color.White;
            this.ImageDownButton.FlatAppearance.MouseDownBackColor = Color.LightSkyBlue;
            this.ImageDownButton.FlatAppearance.MouseOverBackColor = Color.SkyBlue;
            this.ImageDownButton.FlatStyle = FlatStyle.Flat;
            this.ImageDownButton.Font = new Font("宋体", 7f, FontStyle.Bold);
            this.ImageDownButton.Location = new Point(0x4d, 0x18);
            this.ImageDownButton.Name = "ImageDownButton";
            this.ImageDownButton.Size = new Size(20, 20);
            this.ImageDownButton.TabIndex = 2;
            this.ImageDownButton.Text = "<";
            this.ImageDownButton.UseVisualStyleBackColor = true;
            this.ImageDownButton.Click += new EventHandler(this.ImageDownButton_Click);
            this.ImageUpButton.Enabled = false;
            this.ImageUpButton.FlatAppearance.BorderColor = Color.White;
            this.ImageUpButton.FlatAppearance.MouseDownBackColor = Color.LightSkyBlue;
            this.ImageUpButton.FlatAppearance.MouseOverBackColor = Color.SkyBlue;
            this.ImageUpButton.FlatStyle = FlatStyle.Flat;
            this.ImageUpButton.Font = new Font("宋体", 7f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.ImageUpButton.Location = new Point(0x8d, 0x18);
            this.ImageUpButton.Name = "ImageUpButton";
            this.ImageUpButton.Size = new Size(20, 20);
            this.ImageUpButton.TabIndex = 3;
            this.ImageUpButton.Text = ">";
            this.ImageUpButton.UseVisualStyleBackColor = true;
            this.ImageUpButton.Click += new EventHandler(this.ImageUpButton_Click);
            this.MyImage.Location = new Point(0x67, 12);
            this.MyImage.Name = "MyImage";
            this.MyImage.Size = new Size(0x20, 0x20);
            this.MyImage.TabIndex = 10;
            this.MyImage.TabStop = false;
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new Point(0x16, 0x44);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new Size(0x29, 12);
            this.NameLabel.TabIndex = 9;
            this.NameLabel.Text = "名称 :";
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
            this.NameText.Enabled = false;
            this.NameText.Location = new Point(0x45, 0x41);
            this.NameText.MaxLength = 0x10;
            this.NameText.Name = "NameText";
            this.NameText.Size = new Size(100, 0x15);
            this.NameText.TabIndex = 4;
            this.NameText.TextChanged += new EventHandler(this.NameText_TextChanged);
            this.Cancel_Button.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Cancel_Button.DialogResult = DialogResult.Cancel;
            this.Cancel_Button.Location = new Point(0xaf, 0x7b);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new Size(0x4b, 0x17);
            this.Cancel_Button.TabIndex = 1;
            this.Cancel_Button.Text = "取消(&A)";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.OK_Button.DialogResult = DialogResult.OK;
            this.OK_Button.Location = new Point(0x5e, 0x7b);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new Size(0x4b, 0x17);
            this.OK_Button.TabIndex = 0;
            this.OK_Button.Text = "确定(&O)";
            this.OK_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Click += new EventHandler(this.OK_Button_Click);
            base.AcceptButton = this.OK_Button;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.Cancel_Button;
            base.ClientSize = new Size(0x106, 0x9e);
            base.Controls.Add(this.Cancel_Button);
            base.Controls.Add(this.OK_Button);
            base.Controls.Add(this.NameText);
            base.Controls.Add(this.ImageDownButton);
            base.Controls.Add(this.ImageUpButton);
            base.Controls.Add(this.MyImage);
            base.Controls.Add(this.NameLabel);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.Name = "UserInfoForm";
            base.ShowInTaskbar = false;
            this.Text = "用户信息";
            base.Load += new EventHandler(this.UserInfoForm_Load);
            ((ISupportInitialize) this.MyImage).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void NameText_TextChanged(object sender, EventArgs e)
        {
            if (RoomClientForm.IsEmptyString(this.NameText.Text))
            {
                this.OK_Button.Enabled = false;
            }
            else
            {
                this.OK_Button.Enabled = true;
            }
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            this._userInfo.Name = this.NameText.Text;
            this._userInfo.ImageKey = (byte) this._imageIndex;
        }

        private void UpdateImage()
        {
            this.MyImage.Image = this.UserImage.Images[this._imageIndex];
            if (this._imageIndex == 0)
            {
                this.ImageDownButton.Enabled = false;
                this.ImageUpButton.Enabled = true;
            }
            else if (this._imageIndex == (this.UserImage.Images.Count - 1))
            {
                this.ImageDownButton.Enabled = true;
                this.ImageUpButton.Enabled = false;
            }
            else
            {
                this.ImageDownButton.Enabled = true;
                this.ImageUpButton.Enabled = true;
            }
        }

        private void UserInfoForm_Load(object sender, EventArgs e)
        {
            this._imageIndex = this._userInfo.ImageKey;
            this.NameText.Text = this._userInfo.Name;
            this.UpdateImage();
        }
    }
}

