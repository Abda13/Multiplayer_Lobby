using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UITween;
using Unity.VisualScripting;

public class LeftSideMenuController : MonoBehaviour
{
    [SerializeField] RectTransform Buttons;
    [SerializeField] RectTransform Settings;
    [SerializeField] RectTransform FriendList;
    [SerializeField] RectTransform ChatWindow;
    [SerializeField] RectTransform ChatHistory;


    private void OnEnable()
    {
        Buttons.LocalMove(new Vector3(-6f, 0,0), 0.3f);
    }


    public void OnOpenSettingsMenuPressed()
    {
        Buttons.LocalMove(new Vector3(120f, 0, 0), 0.3f);
        Settings.LocalMove(new Vector3(10, 0, 0), 0.3f); 
    }


    public void OnOpenFriendMenuPressed()
    {
        Buttons.LocalMove(new Vector3(120f, 0, 0), 0.3f); 
        FriendList.LocalMove(new Vector3(10, 0, 0), 0.3f);
    }


    public void OnOpenChatHistory()
    {
        Buttons.LocalMove(new Vector3(120f, 0, 0), 0.3f);
        ChatHistory.LocalMove(new Vector3(10, 0, 0), 0.3f);
    }   
    
    public void OnOpenChatUserWindow()
    {
        Buttons.LocalMove(new Vector3(120f, 0, 0), 0.3f);
        ChatWindow.LocalMove(new Vector3(10, 0, 0), 0.3f);
    }



    public void OnMenuBackPressed()
    {
        Buttons.LocalMove(new Vector3(-6f, 0, 0), 0.3f);

        Settings.LocalMove(new Vector3(230f, 0, 0), 0.3f);
        FriendList.LocalMove(new Vector3(230f, 0, 0), 0.3f);
        ChatWindow.LocalMove(new Vector3(230f, 0, 0), 0.3f);
        ChatHistory.LocalMove(new Vector3(230f, 0, 0), 0.3f);
    }

    public void HideEverything()
    {
        Buttons.LocalMove(new Vector3(120f, 0, 0), 0.3f);

        Settings.LocalMove(new Vector3(230f, 0, 0), 0.3f);
        FriendList.LocalMove(new Vector3(230f, 0, 0), 0.3f);
        ChatWindow.LocalMove(new Vector3(230f, 0, 0), 0.3f);
        ChatHistory.LocalMove(new Vector3(230f, 0, 0), 0.3f);
    }


    private void OnDisable()
    {
        Buttons.localPosition = new Vector3(120f, 0, 0); 
        Settings.localPosition = new Vector3(230f, 0, 0); 
        FriendList.localPosition = new Vector3(230f, 0, 0);
        ChatWindow.localPosition = new Vector3(230f, 0, 0);
        ChatHistory.localPosition = new Vector3(230f, 0, 0); 
    }
}
