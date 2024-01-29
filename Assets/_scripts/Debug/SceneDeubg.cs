using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDeubg : MonoBehaviour
{
    public static SceneDeubg inst { get ; private set; }

    public MenuSceneStates currentState;
    public string id;
    public string PlayerName;
    public string server;
    public bool IsLogedin;
    public bool IsLogedinAsGesut;
    public Users firendList;
    [Header("Debug")]
    public bool DisableAllDebug;
    public bool ShowPhotonDebug;
    public bool showPlayfabDebug;
    public bool showPhotonServerDebug;
    public bool showPlayfabDataDebug;
    public bool showPlayfabErrorDebug;
    public bool showPhotonChatDebug;

    private void Awake()
    {
        inst = this;
    }

    void Update()
    {
        firendList = GameData.FriendList;
        id = GameData.UserID;
        PlayerName = GameData.UserName;
        IsLogedin = GameData.IsLogedin;
        IsLogedinAsGesut = GameData.IsLogedinAsGuest;
        currentState = GlobalEvents.currentState;
        server = GameData.Server;

    }
}
