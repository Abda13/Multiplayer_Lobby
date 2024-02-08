using Photon.Chat;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData 
{
    public static string AppVersion { get; set; }

    public static string UserName { get; set; }
    public static string UserID { get; set; }

    public static Users FriendList => firendList;
    public static Users ReceivedPendingFriendRequest => recievedPendingFirendRequest;
    public static Users RoomPlayers => roomPlayers;
    public static Users LastTeam { get; set; }

    public static Settings Settings { get; set; }
    public static GameStatsData GameStats { get; set; }

    public static bool IsLogedin { get; set; }
    public static bool IsLogedinAsGuest { get; set; }
    public static bool IsDataReceived { get; set; }
    public static bool IsFriendListReceived { get; set; }
    public static RoomStatus RoomStatus { get; set; }
    public static string Server { get; set; }
    public static lobbyPlayerController LocalLobbyPlayerController { get; set; }
    public static List<lobbyPlayerController> RemoteLobbyPlayerControllers => remoteLobbyPlayerControllers;


    public static User CurrentChatWindowUser;
    public static bool IsChatWindowOpen;


    static Users firendList = new Users();
    static Users recievedPendingFirendRequest = new Users();
    static Users sentPendingFirendRequest = new Users();

    static Users roomPlayers = new Users();
    static List<lobbyPlayerController> remoteLobbyPlayerControllers = new List<lobbyPlayerController>();

}


[System.Serializable]
public class Users
{
    public List<User> users = new List<User>();
}


[System.Serializable]
public struct User
{
    public string name;
    public string id;
}


[System.Serializable]
public class Settings
{
    public float audio;
    public int status;
}

[System.Serializable]
public class GameStatsData
{
    public int score;
    public int level;
}



[System.Serializable]
public struct ChatContent
{
    public ChatType chatType;
    public string senderName;
    public string id;
    public string msg; 
}


[System.Serializable]
public struct chatContianer
{
    public string id;
    public int status;
    public string msg;

    public chatContianer(string id, int status, string msg)
    {
        this.id = id;
        this.status = status;
        this.msg = msg;
    }

    public chatContianer(string id, string msg)
    {
        this.id = id;
        this.status = 0;
        this.msg = msg;
    }
}


[Serializable]
public class ChatHistory
{
    public string id;
    public string name;
    public bool newChat;
    public bool isMy;
    public List<Msg> msgs = new List<Msg>();
}
[Serializable]
public struct Msg
{
    public string msg;
    public bool isMine;

    public Msg(string msg, bool isMine)
    {
        this.msg = msg;
        this.isMine = isMine;
    }
}


public enum GameMode
{
    RacingSolo,
}

public enum ChatType
{
    Massage,
    Invite,
    Request,
    AcceptRequest
}

public enum RoomStatus
{
    None,
    My,
    Other,
    Game
}

