using chat.Database;
using Photon.Chat;
using PlayFab.EconomyModels;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



namespace Aba.Chat
{
    public class ChatController : MonoBehaviour
    {
        public ChatTextReference chatTextReference;

        [SerializeField] TMP_InputField ChatInput;

        [SerializeField] ChatUser ChatUser;
        [SerializeField] RectTransform ChatUserParent;
        [SerializeField] RectTransform ChatContentParent;
        [SerializeField] LeftSideMenuController LeftSideMenuController;

        [SerializeField] Scrollbar charScroll;

        Dictionary<string, ChatUser> chatUsers = new Dictionary<string, ChatUser>();
        User opendUserChat;

        private void Awake()
        {
            chatTextReference.CreatePool(ChatContentParent);
        }

        private void OnEnable()
        {
            GlobalEvents.OnChatReceived += chatRecieved;
            GlobalEvents.OnOpenChat += OnOpenUserChatWindow;
        }

        private void OnDisable()
        {
            GlobalEvents.OnChatReceived -= chatRecieved;
            GlobalEvents.OnOpenChat -= OnOpenUserChatWindow;
        }



        private void chatRecieved(ChatContent content)
        {
            if (content.chatType == ChatType.Massage)
            {
                var newChatItem = new chatItem(false, false, content.msg);

                if (chatUsers.TryGetValue(content.id, out ChatUser user))
                {
                    user.AddNewChat(newChatItem);

                    googleDatabase.PostToDatabase(opendUserChat.id, user.OpenChatWindow());

                    if (content.id == opendUserChat.id)
                    {
                        chatTextReference.ShowChatHistory(user.OpenChatWindow());
                    }
                }
                else
                {
                    var chatUser = Instantiate(ChatUser.gameObject, ChatUserParent);
                    ChatUserInfo newChat = new ChatUserInfo(content.senderName, content.id);
                    var newChatuser = chatUser.GetComponent<ChatUser>();
                    newChatuser.SetupChat(newChat, this);
                    newChatuser.AddNewChat(newChatItem);
                    googleDatabase.PostToDatabase(content.id, newChat);
                    if (content.id == opendUserChat.id)
                    {
                        chatTextReference.ShowChatHistory(newChatuser.OpenChatWindow());
                    }
                    chatUsers.Add(content.id, newChatuser);

                   // GameData.ChatData.Add(content.id, newChatuser); save it to data base 
                }
            }
        }

        public void OnOpenUserChatWindow(User user)
        {
            LeftSideMenuController.HideEverything();
            LeftSideMenuController.OnOpenChatUserWindow();
            opendUserChat = user;
            if (chatUsers.TryGetValue(user.id, out ChatUser chatUser))
            {
                chatTextReference.ShowChatHistory(chatUser.OpenChatWindow());
                charScroll.value = 0f;
            }
        }

        public void OpenSpecificChat(ChatUserInfo chatUserInfo)
        {
            opendUserChat.id = chatUserInfo.ID;
            opendUserChat.name = chatUserInfo.Name;

            chatTextReference.ShowChatHistory(chatUserInfo);
            LeftSideMenuController.HideEverything();
            LeftSideMenuController.OnOpenChatUserWindow();
            charScroll.value = 0f;
        }


        public void OnOnSendChat()
        {
            if (ChatInput.text.Length > 0)
            {
                
                var chat = new chatItem(true, false, ChatInput.text);

                if (chatUsers.TryGetValue(opendUserChat.id, out ChatUser user))
                {
                    user.AddNewChat(chat);
                    chatTextReference.ShowChatHistory(user.OpenChatWindow());
                    googleDatabase.PostToDatabase(opendUserChat.id, user.OpenChatWindow());
                }
                else
                {
                    var chatUser = Instantiate(ChatUser.gameObject, ChatUserParent);
                    ChatUserInfo newChat = new ChatUserInfo(opendUserChat.name, opendUserChat.id);
                    var newChatuser = chatUser.GetComponent<ChatUser>();
                    newChatuser.SetupChat(newChat, this);
                    newChatuser.AddNewChat(chat);

                    googleDatabase.PostToDatabase(opendUserChat.id, newChat);
                    chatTextReference.ShowChatHistory(newChatuser.OpenChatWindow());
                    chatUsers.Add(opendUserChat.id, newChatuser);
                  
                }
                charScroll.value = 0f;
                var msg = new Msg(ChatInput.text, true);
                GameUtility.SendChatToUser(ChatInput.text, opendUserChat.id);
                ChatInput.text = "";
            }
        }




        [Serializable]
        public class ChatTextReference
        {
            public TextMeshProUGUI WindowName;
            public TextMeshProUGUI SenderName;
            public List<TextMeshProUGUI> senderNames = new List<TextMeshProUGUI>();
            public TextMeshProUGUI SenderMsg;
            public List<TextMeshProUGUI> senderMsgs = new List<TextMeshProUGUI>();
            public TextMeshProUGUI MyName;
            public List<TextMeshProUGUI> myNames = new List<TextMeshProUGUI>();
            public TextMeshProUGUI MyMsg;
            public List<TextMeshProUGUI> myMsgs = new List<TextMeshProUGUI>();


            RectTransform parent;

            public void CreatePool(RectTransform parent)
            {
                this.parent = parent;

                for (int i = 0; i >= 20; i++)
                {
                    createOtherUser(parent);
                    createMyUser(parent);
                }
            }

            public void ShowChatHistory(ChatUserInfo chatUserInfo)
            {
                DisableAll();
                WindowName.text = chatUserInfo.Name;

                foreach(var chat in chatUserInfo.chatHistory)
                {
                    if(chat.isMine)
                    {
                        displayMyChat(chat.msgBody);
                    }
                    else
                    {
                        dispyOtherMsg(chat, chatUserInfo.Name);
                    }
                }

            }


            void dispyOtherMsg(chatItem chat, string name)
            {
                bool itemFound = false;

                foreach (var SenderName in senderNames)
                {
                    if (!SenderName.gameObject.activeInHierarchy)
                    {
                        itemFound = true;
                        SenderName.gameObject.SetActive(true);
                        SenderName.text = name;

                        if(!chat.isSeen) { SenderName.text += " *"; }

                        break;
                    }
                }


                foreach (var SenderMsg in senderMsgs)
                {
                    if (!SenderMsg.gameObject.activeInHierarchy)
                    {
                        itemFound = true;
                        SenderMsg.gameObject.SetActive(true);

                        if (!chat.isSeen) { SenderMsg.text = $" <b> {chat.msgBody} </b>"; chat.isSeen = true; }
                        else SenderMsg.text = chat.msgBody;

                        break;
                    }
                }

                if (!itemFound)
                {
                    var newSenderName = Instantiate(SenderName.gameObject, parent);
                    var newSenderNameText = newSenderName.GetComponent<TextMeshProUGUI>();

                    newSenderNameText.text = name;
                    if (!chat.isSeen) { newSenderNameText.text += " *"; chat.isSeen = true; }
                    newSenderName.gameObject.SetActive(true);
                    senderNames.Add(newSenderNameText);


                    var newSenderMsg = Instantiate(SenderMsg.gameObject, parent);
                    var newnewSenderMsgText = newSenderMsg.GetComponent<TextMeshProUGUI>();

                    if (!chat.isSeen) { newnewSenderMsgText.text = $" <b> {chat.msgBody} </b>"; chat.isSeen = true; }
                    else newnewSenderMsgText.text = chat.msgBody;
                    newSenderMsg.gameObject.SetActive(true);
                    senderMsgs.Add(newnewSenderMsgText);
                }
            }


            void displayMyChat(string body)
            {
                bool itemFound = false;
                foreach (var MyName in myNames)
                {
                    if(!MyName.gameObject.activeInHierarchy)
                    {
                        itemFound = true;
                        MyName.gameObject.SetActive(true);
                        MyName.text = GameData.UserName;
                        break;
                    }
                }


                foreach (var MyMsg in myMsgs)
                {
                    if (!MyMsg.gameObject.activeInHierarchy)
                    {
                        itemFound = true;
                        MyMsg.gameObject.SetActive(true);
                        MyMsg.text =body;
                        break;
                    }
                }

                if(!itemFound)
                {
                    var newMyName = Instantiate(MyName.gameObject, parent);
                    var newNameText = newMyName.GetComponent<TextMeshProUGUI>();
                    newNameText.text = GameData.UserName;
                    newNameText.gameObject.SetActive(true);
                    myNames.Add(newNameText);


                    var newMyMsg = Instantiate(MyMsg.gameObject, parent);
                    var newMsgText = newMyMsg.GetComponent<TextMeshProUGUI>();
                    newMsgText.text = body;
                    newMyMsg.gameObject.SetActive(true);
                    myMsgs.Add(newMsgText);
                }
            }

            void DisableAll()
            {
                foreach (var MyName in myNames)
                {
                    MyName.gameObject.SetActive(false);
                }
                foreach (var MyMsg in myMsgs)
                {
                    MyMsg.gameObject.SetActive(false);
                }
                foreach (var SenderName in senderNames)
                {
                    SenderName.gameObject.SetActive(false);
                }
                foreach (var SenderMsg in senderMsgs)
                {
                    SenderMsg.gameObject.SetActive(false);
                }
            }

            void createOtherUser(Transform Parent)
            {
                var newSenderName = Instantiate(SenderName.gameObject, Parent);
                newSenderName.SetActive(false);
                senderNames.Add(newSenderName.GetComponent<TextMeshProUGUI>());

                var newSenderMsg = Instantiate(SenderMsg.gameObject, Parent);
                newSenderMsg.SetActive(false);
                senderMsgs.Add(newSenderMsg.GetComponent<TextMeshProUGUI>());
            }
            void createMyUser(Transform Parent)
            {
                var newMyName = Instantiate(SenderName.gameObject, Parent);
                newMyName.SetActive(false);
                myNames.Add(newMyName.GetComponent<TextMeshProUGUI>());


                var newMyMsg = Instantiate(MyMsg.gameObject, Parent);
                newMyMsg.SetActive(false);
                myMsgs.Add(newMyMsg.GetComponent<TextMeshProUGUI>());
            }

            


        }

    }




}
