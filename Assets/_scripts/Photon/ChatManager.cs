using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System;
using UnityEngine;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    ChatClient chatClient;
    bool isChatConnected;
     void OnEnable()
     {
        GlobalEvents.OnStateChane += OnMenuStatusChaned;
        //  MenuEvents.OnUserFriendsReceived += onUserFriendsRecieved;
        GlobalEvents.OnSendChat += sendChat;
        GlobalEvents.OnFriendAdded += OnNewFriendAdded;

     }


    void OnDisable()
    {
        GlobalEvents.OnStateChane -= OnMenuStatusChaned;
        // MenuEvents.OnUserFriendsReceived -= onUserFriendsRecieved;
        GlobalEvents.OnSendChat -= sendChat;
    }

    private void OnMenuStatusChaned(MenuSceneStates states)
    {
        if (states == MenuSceneStates.Logedin)
        {
            SwitchableDebug.Log("Connecting to chat", debugType.PhotonChat);
            chatClient = new ChatClient(this);
            chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, GameData.AppVersion, new Photon.Chat.AuthenticationValues(GameData.UserID));
            isChatConnected = true;
        }
    }


    private void OnNewFriendAdded(User user)
    {
        onUserFriendsRecieved();
    }

    private void onUserFriendsRecieved()
    {
        var friendIds = GameUtility.GetUserArray(GameData.FriendList);
        if (friendIds == null) return;
        chatClient.AddFriends(friendIds);
    }

    public void OnConnected()
    {
        SwitchableDebug.Log("Connected to chat", debugType.PhotonChat);
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
        if (GameData.IsFriendListReceived)
        {
            onUserFriendsRecieved();
        }
        else
        {
            GlobalEvents.OnUserFriendsReceived += onUserFriendsRecieved;
        }
        //
    }
    public void OnDisconnected()
    {
        SwitchableDebug.Log("Disconnected to chat", debugType.PhotonChat);
        chatClient.SetOnlineStatus(ChatUserStatus.Offline);
    }

    private void Update()
    {
        if (isChatConnected)
        {
            chatClient.Service();
        }
    }

    private void sendChat(chatContainer chatContianer)
    {
        chatClient.SendPrivateMessage(chatContianer.id, chatContianer.msg);
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if(!string.IsNullOrEmpty(message.ToString()))
        {
            string[] names  = channelName.Split(new[] {':'});
            if (!names[0].Equals(sender,StringComparison.OrdinalIgnoreCase))
            {
                GameUtility.ChatReceived(message.ToString());
                SwitchableDebug.Log($"Chat Received: {names[0]}:{message.ToString()}", debugType.PhotonChat);
            }
        }
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        if(GameUtility.FriendStatus.ContainsKey(user))
        {
            GameUtility.FriendStatus[user] = status;
        }
        else
        {
            GameUtility.FriendStatus.Add(user, status);
        }

        SwitchableDebug.Log($"Player status : {user} : {status}", debugType.PhotonChat);
        chatContainer friendStatus = new chatContainer(user, status, (string)message);
        GlobalEvents.OnFriendStatusUpdated?.Invoke(friendStatus);
    }
    public void DebugReturn(DebugLevel level, string message)
    {
       
    }
    public void OnChatStateChange(ChatState state)
    {
       
    }
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
    }

    public void OnUnsubscribed(string[] channels)
    {
    }

    public void OnUserSubscribed(string channel, string user)
    {
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }
}
