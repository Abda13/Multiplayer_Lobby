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

    [SerializeField] RectTransform IncomingInvite;
    [SerializeField] RectTransform IncomingChat;
    [SerializeField] RectTransform IncomingFriendRequest;


    private void OnEnable()
    {
        Buttons.LocalMove(new Vector3(-6f, 0,0), 1f);
    }


    public void OnOpenSettingsMenuPressed()
    {
        Buttons.LocalMove(new Vector3(120f, 0, 0), 1f);
        Settings.LocalMove(new Vector3(10, 0, 0), 1f); 
    }


    public void OnOpenFriendMenuPressed()
    {
        Buttons.LocalMove(new Vector3(120f, 0, 0), 1f); 
        FriendList.LocalMove(new Vector3(10, 0, 0), 1f);
    }






    public void OnMenuBackPressed()
    {
        Buttons.LocalMove(new Vector3(-6f, 0, 0), 1f);

        Settings.LocalMove(new Vector3(230f, 0, 0), 1f);
        FriendList.LocalMove(new Vector3(230f, 0, 0), 1f);
    }


    private void OnDisable()
    {
        Buttons.localPosition = new Vector3(120f, 0, 0); 
        Settings.localPosition = new Vector3(230f, 0, 0); 
        FriendList.localPosition = new Vector3(230f, 0, 0); 
    }
}
