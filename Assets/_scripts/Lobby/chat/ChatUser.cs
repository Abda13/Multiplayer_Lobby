using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Aba.Chat
{
    public class ChatUser : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI NameText;

        [SerializeField] ChatUserInfo ChatUserInfo;
        ChatController chatController;

        public void SetupChat(ChatUserInfo ChatUserInfo, ChatController chatController)
        {
            this.ChatUserInfo = ChatUserInfo;
            this.chatController = chatController;
            NameText.text = ChatUserInfo.Name;
        }

        internal void AddNewChat(chatItem chatItem)
        {
            ChatUserInfo.AddChat(chatItem);
        }

        internal ChatUserInfo OpenChatWindow()
        {
            return ChatUserInfo;
        }

        public void OnOpenChatPressed()
        {
            chatController.OpenSpecificChat(ChatUserInfo);
        }
    }

    [Serializable]
    public class ChatUserInfo
    {
        public string Name;
        public string ID;

        public List<chatItem> chatHistory;

        public ChatUserInfo(string name, string iD)
        {
            Name = name;
            ID = iD;
            chatHistory = new List<chatItem>();
        }
        public void AddChat(chatItem item)
        {
            chatHistory.Add(item);
        }
    }
    [Serializable]
    public class chatItem
    {
        public bool isMine;
        public bool isSeen;
        public string msgBody;

        public chatItem(bool isMine, bool isSeen, string msgBody)
        {
            this.isMine = isMine;
            this.isSeen = isSeen;
            this.msgBody = msgBody;
        }
    }
}
