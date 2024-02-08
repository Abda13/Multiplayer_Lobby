
using PlayFab.EconomyModels;
using System;
using System.Collections.Generic;
using TMPro;
using UITween;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;


public class chatWindowController : MonoBehaviour
{
    [Header("Tittle")]
    [SerializeField] TextMeshProUGUI NameText;

    [Header("Sending")]
    [SerializeField] TMP_InputField ChatInput;
 
    [Header("Massage Content")]
    [SerializeField] TextMeshProUGUI SenderName;
    [SerializeField] TextMeshProUGUI SenderMsg;
    [SerializeField] TextMeshProUGUI MyName;
    [SerializeField] TextMeshProUGUI MyMsg;

    [Header("Others")]
    [SerializeField] RectTransform ChatWindow;
    [SerializeField] Transform ListParent;
    [SerializeField] Scrollbar Scroller;
    [SerializeField] LeftSideMenuController LeftSideMenuController;

    Dictionary<string, ChatHistory> chats = new Dictionary<string, ChatHistory>();

    List<TextMeshProUGUI> senderNames = new List<TextMeshProUGUI>();
    List<TextMeshProUGUI> senderMsgs = new List<TextMeshProUGUI>();
    List<TextMeshProUGUI> myNames = new List<TextMeshProUGUI>();
    List<TextMeshProUGUI> myMsgs = new List<TextMeshProUGUI>();
    Vector3 localPos;
    private void OnEnable()
    {
        GlobalEvents.OnChatReceived += chatRecieved;
        GlobalEvents.OnOpenChat += OnOpenUserChatWindow;
        GlobalEvents.OnOpenRightSideMenu += OpenRightSideMenu;
    }

    private void OnDisable()
    {
        GlobalEvents.OnChatReceived -= chatRecieved;
        GlobalEvents.OnOpenChat -= OnOpenUserChatWindow;
        GlobalEvents.OnOpenRightSideMenu -= OpenRightSideMenu;
    }

    private void chatRecieved(ChatContent content)
    {
        if (content.chatType == ChatType.Massage)
        {
            



            if(chats.ContainsKey(content.id))
            {
                chats[content.id].msgs.Add(new Msg(content.msg, false));
                chats[content.id].newChat = true;
            }
            else
            {
                ChatHistory chatHistory = new ChatHistory
                {
                    id = content.id,
                    name = content.senderName,
                    newChat = true,
                };
                chatHistory.msgs.Add(new Msg(content.msg,false));
                chats.Add(content.id, chatHistory);
            }
            if(!string.IsNullOrEmpty(GameData.CurrentChatWindowUser.id))
            {
                if (GameData.CurrentChatWindowUser.id.Equals(content.id, StringComparison.OrdinalIgnoreCase))
                {
                    createChatHistory(chats[content.id]);
                    chats[content.id].newChat = !GameData.IsChatWindowOpen;
                }
            }
        }
    }

    public void OnOpenUserChatWindow(User user)
    {
        GlobalEvents.OnOpenRightSideMenu?.Invoke(RightSideMenu.Chat);

        if (chats.ContainsKey(user.id))
        {
            createChatHistory(chats[user.id]);
        }
        else
        {
            //show no chat history
        }
        NameText.text = user.name;
        ChatWindow.LocalMove(GameUtility.Vector3Zero, 0.3f);
        LeftSideMenuController.HideEverything();
        Scroller.value = 1f;
        GameData.CurrentChatWindowUser = user;
        GameData.IsChatWindowOpen = true;

    }

    public void OnCloseUserChatWindow()
    {
        GameData.IsChatWindowOpen = false;
    }
    public void OnOnSendChat()
    {
        if(ChatInput.text.Length > 0)
        {
             var msg =  new Msg(ChatInput.text, true);

            if (chats.ContainsKey(GameData.CurrentChatWindowUser.id))
            {
                chats[GameData.CurrentChatWindowUser.id].msgs.Add(msg);
            }
            else
            {
                ChatHistory chatHistory = new ChatHistory
                {
                    id = GameData.CurrentChatWindowUser.id,
                    name = GameData.CurrentChatWindowUser.name,
                };
                chatHistory.msgs.Add(msg);
                chats.Add(GameData.CurrentChatWindowUser.id, chatHistory);
            }
            GameUtility.SendChatToUser(ChatInput.text, GameData.CurrentChatWindowUser.id);
            createChatHistory(chats[GameData.CurrentChatWindowUser.id]);
            ChatInput.text = "";
        }
    }



    private void OpenRightSideMenu(RightSideMenu menu)
    {
        if (menu == RightSideMenu.Chat)
        {
            return;
        }
        ChatWindow.LocalMove(localPos, 0.3f);
        GameData.IsChatWindowOpen = false;
    }


    private void createChatHistory(ChatHistory chatHistory)
    {
        foreach(var MyName in myNames)
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


        for (int i = 0; i < chatHistory.msgs.Count; i++)
        {
            var c = chatHistory.msgs[i];

            if (c.isMine)
            {
                if(myNames.Count > i)
                {
                    myNames[i].text = GameData.UserName;
                    myNames[i].gameObject.SetActive(true);
                    myMsgs[i].text = c.msg;
                    myMsgs[i].gameObject.SetActive(true);
                }
                else
                {
                    var myNameObj = Instantiate(MyName.gameObject,ListParent);
                    var myNameText = myNameObj.GetComponent<TextMeshProUGUI>();
                    myNameText.text = GameData.UserName;
                    myNameText.gameObject.SetActive(true);
                    myNames.Add(myNameText);

                    var myMsgObj = Instantiate(MyMsg.gameObject, ListParent);
                    var myMsgText = myMsgObj.GetComponent<TextMeshProUGUI>();
                    myMsgText.text = c.msg;
                    myMsgText.gameObject.SetActive(true);
                    myMsgs.Add(myMsgText);
                }
            }
            else
            {
                if (senderNames.Count > i)
                {
                    senderNames[i].text = chatHistory.name;
                    senderNames[i].gameObject.SetActive(true);
                    senderMsgs[i].text = c.msg;
                    senderMsgs[i].gameObject.SetActive(true);
                }
                else
                {
                    var senderNameObj = Instantiate(SenderName.gameObject, ListParent);
                    var senderNameText = senderNameObj.GetComponent<TextMeshProUGUI>();
                    senderNameText.text = chatHistory.name;
                    senderNameText.gameObject.SetActive(true);
                    senderNames.Add(senderNameText);

                    var senderMsgObj = Instantiate(SenderMsg.gameObject, ListParent);
                    var senderMsgText = senderMsgObj.GetComponent<TextMeshProUGUI>();
                    senderMsgText.text = c.msg;
                    senderMsgText.gameObject.SetActive(true);
                    senderMsgs.Add(senderMsgText);
                }
            }
        }
        chatHistory.newChat = false;
    }
}
