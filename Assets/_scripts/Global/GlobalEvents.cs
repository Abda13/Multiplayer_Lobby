using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalEvents 
{
    public static MenuSceneStates currentState { get; private set; }
    public static Action<MenuSceneStates> OnStateChane { get; set; }
    public static Action OnUserDataReceived { get; set; }
    public static Action OnUserFriendsReceived { get; set; }
    public static Action<User> OnFriendAdded { get; set; }
    public static Action<User> OnAcceptInvite { get; set; }
    public static Action<chatContianer> OnFriendStatusUpdated { get; set; }
    public static Action<chatContianer> OnSendChat { get; set; }
    public static Action<ChatContent> OnChatReceived { get; set; }
    public static Action<User> OnOpenChat { get; set; }
    public static Action<RightSideMenu> OnOpenRightSideMenu { get; set; }
    public static Action<int> OnVirtualCurrencyReceived { get; set; }
    public static Action<int> OnCoinsReceived { get; set; }
    public static Action<lobbyPlayerController> OnPlayerJoinedRoom { get; set; }
    public static Action<lobbyPlayerController> OnPlayerLeftRoom { get; set; }


    



    public static void ChangedStat(MenuSceneStates newState)
    {
        currentState = newState;
        OnStateChane?.Invoke(newState);
    }

}

public enum MenuSceneStates
{
    StartingScreen,
    LoggingIn,
    Logedin,
    Connecting,
    Connected,
    InMyRoom,
    Ready,
    Starting
}
public enum RightSideMenu
{
    FriendList,
    Chat
}