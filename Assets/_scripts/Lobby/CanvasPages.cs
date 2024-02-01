using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasPages : MonoBehaviour
{
    [SerializeField] GameObject LoginCanvas;
    [SerializeField] GameObject LoadingCanvas;
    [SerializeField] GameObject CameMenu;
    [SerializeField] TextMeshProUGUI LoadingText;

    private void Start()
    {
        GlobalEvents.ChangedStat(MenuSceneStates.StartingScreen);
    }
    private void OnEnable()
    {
        GlobalEvents.OnStateChane += OnMenuStatusChaned;
    }
    private void OnDisable()
    {
        GlobalEvents.OnStateChane -= OnMenuStatusChaned;
    }

    private void OnMenuStatusChaned(MenuSceneStates states)
    {
        switch (states)
        {
            case MenuSceneStates.StartingScreen:
                LoginCanvas.SetActive(true);
                LoadingCanvas.SetActive(false);
                CameMenu.SetActive(false);
                break;
            case MenuSceneStates.LoggingIn:
                LoginCanvas.SetActive(false);
                LoadingCanvas.SetActive(true);
                LoadingText.text = "Logging in";
                break;
            case MenuSceneStates.Connecting:
                LoginCanvas.SetActive(false);
                LoadingCanvas.SetActive(true);
                LoadingText.text = "Connecting To Server : " + GameData.Server;
                break;
            case MenuSceneStates.Connected:
                LoginCanvas.SetActive(false);
                LoadingCanvas.SetActive(false);
                CameMenu.SetActive(true);
                break;
        }
    }
}
