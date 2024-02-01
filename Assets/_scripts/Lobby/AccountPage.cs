
using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountPage : MonoBehaviour
{
    [Header("Login")]
    [SerializeField] TMP_InputField LoginEmailInput;
    [SerializeField] TMP_InputField loginPasswordInput;
    [SerializeField] Toggle stayLogedInToggle;

    [Header("Login")]
    [SerializeField] TMP_InputField RegUsernameInput;
    [SerializeField] TMP_InputField RegEmailInput;
    [SerializeField] TMP_InputField RegPasswordInput;
    [SerializeField] TMP_InputField RegConfirmPasswordInput;

    private void Awake()
    {
        if(PlayerPrefs.HasKey(GameUtility.Keys.StayLoginToggle))
        {
            stayLogedInToggle.isOn = PlayerPrefs.GetInt(GameUtility.Keys.StayLoginToggle) == 1;
            if (stayLogedInToggle.isOn)
            {
                LoginEmailInput.text = PlayerPrefs.GetString(GameUtility.Keys.UserEmail);
                loginPasswordInput.text = PlayerPrefs.GetString(GameUtility.Keys.UserPassword);
                Login();
            }
        }
    }

    [ContextMenu("Delete Logins")]
    void deletePlayerprefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
    public void RegisterUser()
    {
        if (RegUsernameInput.text.ToLower().Contains("dev"))
        {
            RegEmailInput.text = RegUsernameInput.text + "@test.com";
            RegPasswordInput.text = "123456";

            Playfab.RegisterUserWithEmail(RegUsernameInput.text,
          RegEmailInput.text,
          RegPasswordInput.text,
          OnRegisterSucces,
          OnError
          );
            GlobalEvents.ChangedStat(MenuSceneStates.LoggingIn);
        }


        if (RegUsernameInput.text.Length < 3)
        {
            // short name
            return;
        }
        if (RegEmailInput.text.Length < 5)
        {
            //Short Email
            return;
        }

        if(RegPasswordInput.text.Length < 6)
        {
            //short password
            return;
        }
        if (RegPasswordInput.text != RegConfirmPasswordInput.text)
        {
            //password Does not Match
            return;
        }

        Playfab.RegisterUserWithEmail(RegUsernameInput.text,
           RegEmailInput.text,
           RegPasswordInput.text,
           OnRegisterSucces,
           OnError
           );
        GlobalEvents.ChangedStat(MenuSceneStates.LoggingIn);
    }
    public void Login()
    {
        if(LoginEmailInput.text == "delete")
        {
            deletePlayerprefs();
            return;
        }
        if(LoginEmailInput.text.ToLower().Contains("dev"))
        {
            loginPasswordInput.text = "123456";
        }
          Playfab.LoginUserWithEmail(LoginEmailInput.text.Contains("@"), LoginEmailInput.text, loginPasswordInput.text,
          onLoginSucces,
          OnError
          );
        GlobalEvents.ChangedStat(MenuSceneStates.LoggingIn);
    }
    public void LoginasGeust()
    {
        Playfab.LoginAsGuest(onLoginGesutSuccess, OnError);
        GlobalEvents.ChangedStat(MenuSceneStates.LoggingIn);
    }

    private void onLoginGesutSuccess(LoginResult result)
    {
        GameData.IsLogedinAsGuest = true;
        GameData.UserID = result.PlayFabId;
        GameData.UserName = GameUtility.OnCreatePlayerRandomName();
        GlobalEvents.ChangedStat(MenuSceneStates.Logedin);
    }

    private void onLoginSucces(LoginResult result)
    {
        PlayerPrefs.SetInt(GameUtility.Keys.StayLoginToggle,  stayLogedInToggle.isOn? 1 : 0);
        PlayerPrefs.SetString(GameUtility.Keys.UserEmail, stayLogedInToggle.isOn ? LoginEmailInput.text:"");
        PlayerPrefs.SetString(GameUtility.Keys.UserPassword, stayLogedInToggle.isOn ? loginPasswordInput.text : "");
        PlayerPrefs.Save();
        Playfab.GetAccountInfo();
        Playfab.GetUserData();
        GameData.UserID = result.PlayFabId;
        GameData.IsLogedin = true;
        GlobalEvents.ChangedStat(MenuSceneStates.Logedin);
    }
    private void OnRegisterSucces(RegisterPlayFabUserResult result)
    {
        PlayerPrefs.SetInt(GameUtility.Keys.StayLoginToggle, stayLogedInToggle.isOn ? 1 : 0);
        PlayerPrefs.SetString(GameUtility.Keys.UserEmail, stayLogedInToggle.isOn ? RegEmailInput.text : "");
        PlayerPrefs.SetString(GameUtility.Keys.UserPassword, stayLogedInToggle.isOn ? RegPasswordInput.text : "");
        PlayerPrefs.Save();
        GameUtility.InitializeUserData();
        GameData.UserID = result.PlayFabId;
        GameData.IsLogedin = true;
        GameData.UserName = RegUsernameInput.text;
        GlobalEvents.ChangedStat(MenuSceneStates.Logedin);
    }
    private void OnError(PlayFabError error)
    {
        SwitchableDebug.Log(error.ErrorMessage, debugType.PlayfabError);
        GlobalEvents.ChangedStat(MenuSceneStates.StartingScreen);
    }
}
