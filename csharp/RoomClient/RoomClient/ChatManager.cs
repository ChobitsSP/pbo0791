namespace PokemonBattle.RoomClient
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Forms;

    public class ChatManager
    {
        private Dictionary<int, ChatForm> _chats = new Dictionary<int, ChatForm>();
        private Dictionary<int, List<string>> _waitChats = new Dictionary<int, List<string>>();

        public event ChatDelegate OnAddChat;

        public event ChatDelegate OnRemoveChat;

        public bool BuildChatForm(int target, string myName, PokemonBattle.RoomClient.RoomClient client)
        {
            if (this._chats.ContainsKey(target))
            {
                return false;
            }
            ChatForm form = new ChatForm(target, myName, client);
            this._chats[target] = form;
            form.FormClosed += new FormClosedEventHandler(this.ChatForm_FormClosed);
            if (this._waitChats.ContainsKey(target))
            {
                List<string> list = this._waitChats[target];
                this._waitChats.Remove(target);
                this.HandleOnRemoveChatEvent(target);
                foreach (string str in list)
                {
                    form.ReceiveChatMessage(str);
                }
            }
            return true;
        }

        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            int target = (sender as ChatForm).Target;
            this._chats.Remove(target);
        }

        public ChatForm GetChatForm(int target)
        {
            if (this._chats.ContainsKey(target))
            {
                return this._chats[target];
            }
            return null;
        }

        private void HandleOnAddChatEvent(int from)
        {
            if (this.OnAddChat != null)
            {
                this.OnAddChat(from);
            }
        }

        private void HandleOnRemoveChatEvent(int target)
        {
            if (this.OnRemoveChat != null)
            {
                this.OnRemoveChat(target);
            }
        }

        public void PassChatMessage(int from, string message)
        {
            if (this._chats.ContainsKey(from))
            {
                this._chats[from].ReceiveChatMessage(message);
            }
            else
            {
                if (!this._waitChats.ContainsKey(from))
                {
                    this._waitChats[from] = new List<string>();
                    this.HandleOnAddChatEvent(from);
                }
                this._waitChats[from].Add(message);
            }
        }
    }
}

