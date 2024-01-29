using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    User invitingUser;
    public override void OnEnable()
    {
        base.OnEnable();
        GameData.RoomStatus = RoomStatus.None;
        GlobalEvents.OnStateChane += OnMenuStatusChaned;
        GlobalEvents.OnAcceptInvite += onAcceptInvite;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        GlobalEvents.OnStateChane -= OnMenuStatusChaned;
        GlobalEvents.OnAcceptInvite -= onAcceptInvite;
    }

    private void OnMenuStatusChaned(MenuSceneStates states)
    {
        if(states == MenuSceneStates.Logedin)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            GameData.Server = PhotonNetwork.CloudRegion;
            PhotonNetwork.NickName = GameData.UserName;
            AuthenticationValues values = new AuthenticationValues(GameData.UserID);
            PhotonNetwork.AuthValues = values;
            PhotonNetwork.ConnectUsingSettings();
            GlobalEvents.ChangedStat(MenuSceneStates.Connecting);
        }
    }


    private void onAcceptInvite(User user)
    {
        if(PhotonNetwork.InRoom)
        {
            GameData.RoomStatus = RoomStatus.Other;
            PhotonNetwork.LeaveRoom();
        }
        invitingUser = user;
    }


    public override void OnConnectedToMaster()
    {
        SwitchableDebug.Log("Server host: " + PhotonNetwork.CloudRegion +
                  ",  ping:" + PhotonNetwork.GetPing(),debugType.PhotonServer);

        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        switch(GameData.RoomStatus)
        {
            case RoomStatus.None:
                GlobalEvents.ChangedStat(MenuSceneStates.Connected);
                var options = new RoomOptions
                {
                    PublishUserId = true,
                    IsVisible = false,
                    MaxPlayers = 3,
                };

               /* options.CustomRoomPropertiesForLobby = new string[4] { "OwnerName", "Map", "mode", "isPrivateRoom" };
                options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
                options.CustomRoomProperties.Add("OwnerName", PhotonNetwork.NickName);
                options.CustomRoomProperties.Add("Map", currentMap);
                options.CustomRoomProperties.Add("mode", mode);
                options.CustomRoomProperties.Add("isPrivateRoom", isprivite);*/

                PhotonNetwork.CreateRoom(GameData.UserID, options);
                break;

            case RoomStatus.Other:
                PhotonNetwork.JoinRoom(invitingUser.id);
                break;
        }
        SwitchableDebug.Log("Joined Lobby", debugType.PhotonServer);
    }
    public override void OnCreatedRoom()
    {
        GameData.RoomStatus = RoomStatus.My;
        SwitchableDebug.Log($"Created room : {PhotonNetwork.CurrentRoom.Name}", debugType.PhotonServer);
    }
    public override void OnJoinedRoom()
    {
        SwitchableDebug.Log($"Joined room : {PhotonNetwork.CurrentRoom.Name}", debugType.PhotonServer);
        PhotonNetwork.LocalPlayer.NickName = GameData.UserName;
        var players = PhotonNetwork.PlayerList;

        foreach (var p in players)
        {
            User u = new User
            {
                id = p.UserId,
                name = p.NickName
            };
            GameData.RoomPlayers.users.Add(u);
        }
        PhotonNetwork.Instantiate(Path.Combine("Photon", "LobbyPlayer"), Vector3.zero, Quaternion.identity);
    }
    public override void OnLeftRoom()
    {
        Destroy(GameData.LocalLobbyPlayerController);
    }
    public override void OnPlayerEnteredRoom(Player _newPlayer)
    {
      
    }
    public override void OnPlayerLeftRoom(Player player)
    {
       
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable hangedProps)
    {
        
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(newMasterClient.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            
        }
    }

}
