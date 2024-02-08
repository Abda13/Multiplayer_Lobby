using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtility
{

    public static Vector3 Vector3Zero
    {
        get
        {
            return new Vector3(0, 0, 0);
        }
    }
    public static class Keys
    {
        public readonly static string StayLoginToggle = "StayLoginToggle";
        public readonly static string UserEmail = "userEmail";
        public readonly static string UserPassword = "pass";
        public readonly static string UserName = "userName";
        public readonly static string lastTeam = "lastTeam";
        public readonly static string Settings = "settings";
        public readonly static string GameStats = "gameData";
    }
    public static Dictionary<string,int> FriendStatus = new Dictionary<string,int>();
    public static string OnCreatePlayerRandomName()
    {
        return "Player_" + UnityEngine.Random.Range(0,10).ToString() + UnityEngine.Random.Range(0, 10).ToString() + UnityEngine.Random.Range(0, 10).ToString();
    }

    public static void InitializeUserData()
    {
        Dictionary<string, string> data = new Dictionary<string, string>
        {
                { Keys.lastTeam, "" },
                { Keys.Settings, "" },
                { Keys.GameStats, "" }
        };
        JsonAsyncUtility.ToJson(new Users(), (c) =>
        {
            data[Keys.lastTeam] = c;

            JsonAsyncUtility.ToJson(new Settings(), (c) =>
            {
                data[Keys.Settings] = c;

                JsonAsyncUtility.ToJson(new GameStatsData(), (c) =>
                {
                    data[Keys.GameStats] = c;
                    Playfab.UpdateUserData(data);
                    SwitchableDebug.Log("User Data is created", debugType.PlayfabData);
                });
                
            });
        });
    }

    public static void ReadUserData(Dictionary<string, UserDataRecord> data)
    {
        JsonAsyncUtility.FromJson<Users>(data[Keys.lastTeam].Value, (c) =>
        {
            GameData.LastTeam = c;
            JsonAsyncUtility.FromJson<Settings>(data[Keys.Settings].Value, (c) =>
            {
                GameData.Settings = c;
                JsonAsyncUtility.FromJson<GameStatsData>(data[Keys.GameStats].Value, (c) =>
                {
                    GameData.GameStats = c;
                    GlobalEvents.OnUserDataReceived?.Invoke();
                    GameData.IsDataReceived = true;
                    SwitchableDebug.Log("All Data Received", debugType.PlayfabData);
                });
            });
        });
    }

    public static string[] GetUserArray(Users users) 
    {
        if(users == null || users.users.Count == 0) return null;

        string[] ids = new string[users.users.Count];

        for (int i = 0; i < users.users.Count; i++)
        {
            ids[i] = users.users[i].id;
        }

        return ids;
    }

    public static void OnSendGameInvitation(User user)
    {
        //make chat data
        ChatContent chat = new ChatContent();
        chat.id = GameData.UserID;
        chat.senderName = GameData.UserName;
        chat.chatType = ChatType.Invite;
        chat.msg = "";
        SendChat(chat, user.id);
    }

    public static void OnAcceptFriendRequest(User user)
    {
        //update local gameData
        GameData.FriendList.users.Add(user);
        GameData.ReceivedPendingFriendRequest.users.Remove(user);
        //update playfab data
        Playfab.AcceptFriendRequest(user);

        //let other user know
        ChatContent chat = new ChatContent();
        chat.id = GameData.UserID;
        chat.senderName = GameData.UserName;
        chat.chatType = ChatType.AcceptRequest;
        chat.msg = "";
        SendChat(chat, user.id);

        //update local UI , update photon chat friend list 
        GlobalEvents.OnFriendAdded?.Invoke(user);
    }

    public static void OnSendFriendRequest(User user)
    {
        //send playfab friend Request
        Playfab.SendFriendRequest(user);

        //send chat request
        ChatContent chat = new ChatContent();
        chat.id = GameData.UserID;
        chat.senderName = GameData.UserName;
        chat.chatType = ChatType.Request;
        chat.msg = "";

        SendChat(chat, user.id);
    }

    public static void ChatReceived(string chat)
    {
       
        JsonAsyncUtility.FromJson<ChatContent>(chat, (c) =>
        {
            GlobalEvents.OnChatReceived?.Invoke(c);
        });
    }

    public static void SendChatToUser(string msg,string id)
    {
        //tbd make sure to save it in both sender and reciever

        ChatContent chat = new ChatContent();
        chat.id = GameData.UserID;
        chat.senderName = GameData.UserName;
        chat.chatType = ChatType.Massage;
        chat.msg = msg;

        SendChat(chat, id);
    }


    static void SendChat(ChatContent chat,string id)
    {
        JsonAsyncUtility.ToJson(chat, (c) =>
        {
            //send chat data
            chatContainer chatContianer = new chatContainer(id, c);
            GlobalEvents.OnSendChat?.Invoke(chatContianer);
            SwitchableDebug.Log($"Send Chat : {c}", debugType.PhotonChat);
        });
    }

  
}








