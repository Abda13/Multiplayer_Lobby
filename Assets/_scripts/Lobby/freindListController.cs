
using PlayFab;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class freindListController : MonoBehaviour
{
   // public sprites sprites;

    //[SerializeField] RectTransform Lists;
   // [SerializeField] GameObject OpenButton;
  //  [SerializeField] GameObject loading;
    [SerializeField] Transform parent;
    [SerializeField] TMP_InputField searchInput;
    [SerializeField] GameObject search;
    [SerializeField] TextMeshProUGUI SearchText;
    [SerializeField] FirendController firendController;
    [SerializeField] FirendController SearchResult;
    [SerializeField] TMP_Dropdown pages;

    Dictionary<string,FirendController> firendControllers = new Dictionary<string, FirendController> ();

    int currentIndex;

    private void OnEnable()
    {
        if(GameData.IsFriendListReceived)
        {
           // loading.SetActive(false);
            CreateFriendList();
        }
        else
        {
            GlobalEvents.OnUserFriendsReceived += friendListRecieved;
          //  loading.SetActive(true);
        }
        GlobalEvents.OnFriendStatusUpdated += OnFriendStatusChaned;
        GlobalEvents.OnFriendAdded += OnFriendAdded;
        GlobalEvents.OnChatReceived += onChatRecived;
        GlobalEvents.OnOpenRightSideMenu += openRightSideMenu;
        pages.onValueChanged.AddListener(OnChangePlayerList);
    }

    private void openRightSideMenu(RightSideMenu menu)
    {
       if(menu == RightSideMenu.FriendList)
        {
            return;
        }
        //Lists.LocalMO(GameUtility.Vector3Zero, 0.3f);
    }

    private void OnDisable()
    {
        GlobalEvents.OnFriendStatusUpdated -= OnFriendStatusChaned;
        GlobalEvents.OnFriendAdded -= OnFriendAdded;
        GlobalEvents.OnChatReceived -= onChatRecived;
    }

    private void onChatRecived(ChatContent content)
    {
        if (content.chatType == ChatType.AcceptRequest)
        {
            var newFriend = new User
            {
                id = content.id,
                name = content.senderName
            };
            GameData.FriendList.users.Add(newFriend);

            GlobalEvents.OnFriendAdded?.Invoke(newFriend);

            OnChangePlayerList(currentIndex);
        }

        if (content.chatType == ChatType.Request)
        {
            var newFriend = new User
            {
                id = content.id,
                name = content.senderName
            };
            //create friend UI
            OnFriendRequest(newFriend);
            //save it locally
            GameData.ReceivedPendingFriendRequest.users.Add(newFriend);
            //update list UI
            OnChangePlayerList(currentIndex);
        }
    }

    public void OnOpenFriendLists()
    {
        GlobalEvents.OnOpenRightSideMenu?.Invoke(RightSideMenu.FriendList);
       //Lists.DOLocalMove(new Vector3(-800,0f,0f), 0.3f);
       // OpenButton.SetActive(false);
    }
    public void OnCloseFriendLists()
    {
     //  Lists.DOLocalMove(GameUtility.Vector3Zero, 0.3f);
       // OpenButton.SetActive(true);
    }
    public void OnChangePlayerList(int index)
    {
        currentIndex = index;
        foreach (var friend in firendControllers)
        {
            if(index ==0)
            {
                friend.Value.gameObject.SetActive(friend.Value.IsOnline && friend.Value.friendType == FriendType.Friend);
            }
            else if(index == 1) 
            {
                friend.Value.gameObject.SetActive(!friend.Value.IsOnline && friend.Value.friendType == FriendType.Friend);
            }
            else if(index == 2)
            {
                friend.Value.gameObject.SetActive(friend.Value.friendType == FriendType.PendingRequest);
            }
            else if(index == 3)
            {
                friend.Value.gameObject.SetActive(friend.Value.friendType == FriendType.LastTeam);
            }
            else 
            {
                friend.Value.gameObject.SetActive(false);
            }
        }
        search.SetActive(index == 4);
        SearchResult.gameObject.SetActive(false);
        searchInput.text = "";
    }
   
    public void OnSearchForUser()
    {
        if(searchInput.text.Length>2f)
        {
            Playfab.OnFindOtherUsers(searchInput.text, searchInput.text, false, OnSeachResult, OnError);
            SearchText.text = "Searching.";
        }
    }

    public void OnSendInvite(User user)
    {
        GameUtility.OnSendGameInvitation(user);
    }
    public void OnAcceptFriendRequest(User user)
    {
        GameUtility.OnAcceptFriendRequest(user);
    }
    public void SendRequest(User user)
    {
        GameUtility.OnSendFriendRequest(user);
    }


    private void OnFriendAdded(User user)
    {
        if (firendControllers.TryGetValue(user.id, out var f))
        {
            f.SetUp(user, FriendType.Friend, 0, this);
        }
        else
        {
            var firendObj = Instantiate(firendController.gameObject, parent);
            FirendController firend = firendObj.GetComponent<FirendController>();
            firend.SetUp(user, FriendType.Friend, 0, this);
            firendControllers.Add(user.id, firend);
        }
    }


    private void OnFriendRequest(User user)
    {
        if (firendControllers.TryGetValue(user.id, out var f))
        {
            f.SetUp(user, FriendType.PendingRequest, 0, this);
        }
        else
        {
            var firendObj = Instantiate(firendController.gameObject, parent);
            FirendController firend = firendObj.GetComponent<FirendController>();
            firend.SetUp(user, FriendType.PendingRequest, 0, this);
            firendControllers.Add(user.id, firend);
        }
    }


    private void OnError(PlayFabError error)
    {
        SearchText.text = $"Error : {error.ErrorMessage}";
    }

    private void OnSeachResult(User user)
    {
        SearchText.text = "";
        SearchResult.gameObject.SetActive(true);
        SearchResult.SetUp(user,FriendType.Search, 0, this);
    }

    private void OnFriendStatusChaned(chatContainer status)
    {
        if(firendControllers.TryGetValue(status.id,out var f))
        {
            f.OnStatusChaned(status.status);
        }
        OnChangePlayerList(currentIndex);
    }
    private void friendListRecieved()
    {
        GlobalEvents.OnUserFriendsReceived -= friendListRecieved;
       // loading.SetActive(false);
        CreateFriendList();
    }

    private void CreateFriendList()
    {
        if (GameData.FriendList != null)
        {
            var frendList = GameData.FriendList;
            for (int i = 0; i < frendList.users.Count; i++)
            {
                if (firendControllers.TryGetValue(frendList.users[i].id, out var f))
                {
                    int status = 0;
                    if (GameUtility.FriendStatus.ContainsKey(frendList.users[i].id))
                    {
                        status = GameUtility.FriendStatus[frendList.users[i].id];
                    }
                    f.SetUp(frendList.users[i], FriendType.Friend, status, this);
                }
                else
                {
                    var firendObj = Instantiate(firendController.gameObject, parent);
                    FirendController firend = firendObj.GetComponent<FirendController>();
                    int status = 0;
                    if (GameUtility.FriendStatus.ContainsKey(frendList.users[i].id))
                    {
                        status = GameUtility.FriendStatus[frendList.users[i].id];
                    }
                    firend.SetUp(frendList.users[i], FriendType.Friend, status, this);
                    firendControllers.Add(frendList.users[i].id, firend);
                }

            }
        }

        if (GameData.ReceivedPendingFriendRequest != null)
        {

            var pendings = GameData.ReceivedPendingFriendRequest;
            for (int i = 0; i < pendings.users.Count; i++)
            {
                if (firendControllers.TryGetValue(pendings.users[i].id, out var f))
                {
                    f.SetUp(pendings.users[i], FriendType.PendingRequest, 0, this);
                }
                else
                {
                    var firendObj = Instantiate(firendController.gameObject, parent);
                    FirendController firend = firendObj.GetComponent<FirendController>();
                    firend.SetUp(pendings.users[i], FriendType.PendingRequest, 0, this);
                    firendControllers.Add(pendings.users[i].id, firend);
                }
            }
        }

        if (GameData.LastTeam != null)
        {
            var last = GameData.LastTeam;
            for (int i = 0; i < last.users.Count; i++)
            {
                if (firendControllers.TryGetValue(last.users[i].id, out var f))
                {
                    f.SetUp(last.users[i], FriendType.LastTeam, 0, this); ;
                }
                else
                {
                    var firendObj = Instantiate(firendController.gameObject, parent);
                    FirendController firend = firendObj.GetComponent<FirendController>();
                    firend.SetUp(last.users[i], FriendType.LastTeam, 0, this);
                    firendControllers.Add(last.users[i].id, firend);
                }
            }
        }
        OnChangePlayerList(currentIndex);
    }
}
