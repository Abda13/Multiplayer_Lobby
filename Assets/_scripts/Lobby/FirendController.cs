using Photon.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirendController : MonoBehaviour
{
    public string id => user.id;
    public bool IsOnline;
    public FriendType friendType;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Button yes;
    [SerializeField] TextMeshProUGUI yesText;
    [SerializeField] Button no;
    [SerializeField] TextMeshProUGUI noText;
    [SerializeField] Button chat;
    [SerializeField] TextMeshProUGUI chatText;

    int onlineStatus;
    User user;

    List<chatContainer> chatsHistory = new List<chatContainer>();
   freindListController freindListController;

    public void SetUp(User user, FriendType friendType, int onlineStatus, freindListController freindListController)
    {
        this.user = user;
        nameText.text = user.name;
        this.friendType = friendType;
        this.onlineStatus = onlineStatus;
        this.freindListController = freindListController;
        setupSprites();
    }

  

    public void OnStatusChaned(int  status)
    {
        if (friendType != FriendType.Friend) return;
        onlineStatus = status;

        if(status == ChatUserStatus.Online)
        {
            IsOnline = true;
        }
        else
        {
            IsOnline = false;
        }
        updateFriendStatus(IsOnline);
        setupSprites();
    }



    public bool isFriendOnline()
    {
        return IsOnline && friendType == FriendType.Friend;
    }

    public void OnAcceptButton()
    {
        switch (friendType)
        {
            case FriendType.Friend:
                if(IsOnline)
                {
                    
                }
                freindListController.OnSendInvite(user);
                break;
            case FriendType.PendingRequest:
                freindListController.OnAcceptFriendRequest(user);
                break;
            case FriendType.LastTeam:
                freindListController.SendRequest(user);
                break;
            case FriendType.Search:
                freindListController.SendRequest(user);
                break;

        }
    }

    public void OnCloseButton()
    {
        
    }

    public void OnSendChatButton()
    {
        GlobalEvents.OnOpenChat?.Invoke(user);
    }


    private void updateFriendStatus(bool isOnline)
    {
        chat.interactable = isOnline;
        yes.interactable = isOnline;
        no.interactable = isOnline;   
    }

    private void setupSprites()
    {
        switch (friendType)
        {
            case FriendType.Friend:
                yesText.text = "Invite";
                yes.interactable = IsOnline;
                chatText.text = "Chat";
                no.gameObject.SetActive(false);

                break;
            case FriendType.PendingRequest:
                yesText.text = "Accept";
                yes.interactable = true;
                noText.text = "Deny";
                chat.gameObject.SetActive(false);
                break;
            case FriendType.LastTeam:

                break;
            case FriendType.Search:
                yesText.text = "Send";
                yes.interactable = true;

                no.gameObject.SetActive(false);
                chat.gameObject.SetActive(false);
                break;

        }
    }

}
public enum FriendType
{
    Friend,
    PendingRequest,
    LastTeam,
    Search
}
