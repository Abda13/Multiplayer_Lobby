using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject[] Cameras;
    private void OnEnable()
    {
        GlobalEvents.OnStateChane += OnMenuStatusChaned;
        GlobalEvents.OnPlayerJoinedRoom += OnplayerJoindRoom;
        GlobalEvents.OnPlayerLeftRoom -= OnplayerJoindRoom;
    }


    private void OnDisable()
    {
        GlobalEvents.OnStateChane -= OnMenuStatusChaned;
        GlobalEvents.OnPlayerJoinedRoom -= OnplayerJoindRoom;
        GlobalEvents.OnPlayerLeftRoom -= OnplayerJoindRoom;
    }

    private void OnMenuStatusChaned(MenuSceneStates states)
    {
        if(states == MenuSceneStates.StartingScreen)
        {
            DisableCameras();
            Cameras[0].SetActive(true);
        }
        else if(states == MenuSceneStates.Connected)
        {
            DisableCameras();
            Cameras[1].SetActive(true);
        }
    }


    private void OnplayerJoindRoom(lobbyPlayerController controller)
    {
        if (controller.IsLocal) return;

        if(GameData.RemoteLobbyPlayerControllers.Count > 0)
        {
            DisableCameras();
            Cameras[GameData.RemoteLobbyPlayerControllers.Count+1].SetActive(true);
        }
    }


    void DisableCameras()
    {
        foreach (var cam in Cameras)
        {
            cam.SetActive(false);
        }
    }
}
