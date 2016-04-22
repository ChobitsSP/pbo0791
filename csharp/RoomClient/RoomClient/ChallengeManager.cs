namespace PokemonBattle.RoomClient
{
    using PokemonBattle.BattleRoom.Client;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class ChallengeManager
    {
        private RoomBattleSetting _battleSetting;
        private Dictionary<int, ChallengeForm> _challenges = new Dictionary<int, ChallengeForm>();
        private ChallengeForm _myChallenge;
        private PokemonBattle.RoomClient.RoomClient _roomClient;

        public ChallengeManager(PokemonBattle.RoomClient.RoomClient client)
        {
            this._roomClient = client;
        }

        private void _myChallenge_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._myChallenge != null)
            {
                this.CancelChallenge(sender, EventArgs.Empty);
            }
        }

        private void AcceptChallenge(object sender, EventArgs e)
        {
            ChallengeForm form = (sender as Button).FindForm() as ChallengeForm;
            int identity = form.TargetInfo.Identity;
            this._roomClient.AcceptChallenge(identity);
            this._challenges.Remove(identity);
            form.Close();
        }

        private void CancelChallenge(object sender, EventArgs e)
        {
            ChallengeForm form = this._myChallenge;
            this._roomClient.CancelChallenge(this._myChallenge.TargetInfo.Identity);
            this._myChallenge = null;
            form.Close();
        }

        public ChallengeForm Challenge(User target)
        {
            this._myChallenge = new ChallengeForm(target);
            this._myChallenge.OK_Button.Click += new EventHandler(this.ConfirmChallenge);
            this._myChallenge.FormClosing += new FormClosingEventHandler(this._myChallenge_FormClosing);
            if (this._battleSetting.SingleBan)
            {
                this._myChallenge.ModeCombo.SelectedIndex = 1;
                this._myChallenge.ModeCombo.Enabled = false;
            }
            if (this._battleSetting.DoubleBan)
            {
                this._myChallenge.ModeCombo.SelectedIndex = 0;
                this._myChallenge.ModeCombo.Enabled = false;
            }
            if (this._battleSetting.RandomOnly)
            {
                this._myChallenge.RandomCheck.Checked = true;
                this._myChallenge.RandomCheck.Enabled = false;
            }
            List<ChallengeForm> list = new List<ChallengeForm>(this._challenges.Values);
            foreach (ChallengeForm form in list)
            {
                form.Cancel_Button.PerformClick();
            }
            return this._myChallenge;
        }

        public void ChallengeAccepted()
        {
            this._roomClient.StartBattle(this._myChallenge.TargetInfo.Identity, this._myChallenge.ChallengeInfo);
            if (this._myChallenge != null)
            {
                ChallengeForm form = this._myChallenge;
                this._myChallenge = null;
                MethodInvoker methodInvokerDelegate = delegate () { form.Close(); };
                form.Invoke(methodInvokerDelegate);
            }
        }

        public void ChallengeCanceled(int identity)
        {
            if (this._challenges.ContainsKey(identity))
            {
                ChallengeForm challenge = this._challenges[identity];
                this._challenges.Remove(identity);
                MethodInvoker methodInvokerDelegate = delegate () { challenge.Close(); };
                challenge.Invoke(methodInvokerDelegate);
            }
        }

        private void challengeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            int identity = (sender as ChallengeForm).TargetInfo.Identity;
            if (this._challenges.ContainsKey(identity))
            {
                this.RefuseChallenge(sender, EventArgs.Empty);
            }
        }

        public void ChallengeRefused()
        {
            if (this._myChallenge != null)
            {
                ChallengeForm form = this._myChallenge;
                this._myChallenge = null;
                MethodInvoker methodInvokerDelegate = delegate () {
                    form.Close();
                    MessageBox.Show("对方拒绝了你的挑战", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                };
                form.Invoke(methodInvokerDelegate);
            }
        }

        private void ConfirmChallenge(object sender, EventArgs e)
        {
            this._myChallenge.Cancel_Button.Click += new EventHandler(this.CancelChallenge);
            this._roomClient.Challenge(this._myChallenge.TargetInfo.Identity, this._myChallenge.ChallengeInfo);
        }

        public ChallengeForm ReceiveChallenge(User from, ChallengeInfo challenge)
        {
            ChallengeForm form = new ChallengeForm(from, challenge);
            form.OK_Button.Click += new EventHandler(this.AcceptChallenge);
            form.Cancel_Button.Click += new EventHandler(this.RefuseChallenge);
            form.FormClosing += new FormClosingEventHandler(this.challengeForm_FormClosing);
            this._challenges[from.Identity] = form;
            return form;
        }

        private void RefuseChallenge(object sender, EventArgs e)
        {
            ChallengeForm form = (sender as Button).FindForm() as ChallengeForm;
            int identity = form.TargetInfo.Identity;
            this._roomClient.RefuseChallenge(identity);
            this._challenges.Remove(identity);
            form.Close();
        }

        public void SetSetting(RoomBattleSetting setting)
        {
            this._battleSetting = setting;
        }

        public bool UserLogout(int identity)
        {
            if ((this._myChallenge != null) && (this._myChallenge.TargetInfo.Identity == identity))
            {
                this.ChallengeRefused();
                return true;
            }
            if (this._challenges.ContainsKey(identity))
            {
                this.ChallengeCanceled(identity);
            }
            return false;
        }
    }
}

