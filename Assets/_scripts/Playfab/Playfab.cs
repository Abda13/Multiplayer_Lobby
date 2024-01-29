using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Playfab
{
    #region Account
    public static void RegisterUserWithEmail(string userName, string email,string password, Action<RegisterPlayFabUserResult> OnSucces, Action<PlayFabError> OnError)
    {
        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = userName,
            Username = userName,
            Email = email, 
            Password = password
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnSucces, OnError);
    }

    public static void LoginUserWithEmail(bool isEmail, string userName, string password, Action<LoginResult> OnSucces, Action<PlayFabError> OnError)
    {
        if(isEmail)
        {
            var request = new LoginWithEmailAddressRequest
            {
                Email = userName,
                Password = password
            };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnSucces, OnError);
        }
        else
        {
            var request = new LoginWithPlayFabRequest 
            {
                Username = userName,
                Password = password 
            };
            PlayFabClientAPI.LoginWithPlayFab(request, OnSucces, OnError);
        }

    }


    public static void LoginAsGuest(Action<LoginResult> OnSucces, Action<PlayFabError> OnError)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSucces, OnError);

    }

    public static void OnUserForgotPassword(string email, Action<SendAccountRecoveryEmailResult> OnSucces, Action<PlayFabError> OnError)
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email,
            TitleId = "B384F"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnSucces, OnError);
    }
    #endregion

    #region User Data
    public static void UpdateUserData(Dictionary<string, string> data)
    {
        var request = new UpdateUserDataRequest
        {
            Data = data
        };
        PlayFabClientAPI.UpdateUserData(request, (reponce) =>
        {
            SwitchableDebug.Log("setting data success", debugType.Playfab);
        }
        , (error) =>
        {
            SwitchableDebug.Log("setting data Field : " + error.ErrorMessage, debugType.PlayfabError);
        });
    }
    public static void GetUserData()
    {
        GetFriendList();
        var request = new GetUserDataRequest
        {
            Keys = new List<string>
            {
                GameUtility.Keys.lastTeam,
                GameUtility.Keys.Settings,
                GameUtility.Keys.GameStats
            }
        };
        PlayFabClientAPI.GetUserData(request,
            (reponce) =>
            {
                GameUtility.ReadUserData(reponce.Data);
            },
            (error) =>
            {
                SwitchableDebug.Log("Getting data Field : " + error.ErrorMessage, debugType.PlayfabError);
            });
    }
    public static void GetAccountInfo()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
            (c) =>
            {
                GameData.UserName = c.AccountInfo.Username;
            },
            (error) =>
            {
                SwitchableDebug.Log("Getting name Field : " + error.ErrorMessage, debugType.PlayfabError);
            });
    }
    #endregion

    #region Friends
    public static void SendFriendRequest(User user)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "SendFriendRequest",
            FunctionParameter = new
            {
                FriendPlayFabId = user.id,
            }
        };
        PlayFabClientAPI.ExecuteCloudScript(request,
              (result) =>
              {
                  SwitchableDebug.Log($"Friend Request sent", debugType.PlayfabError);
              },
             (error) =>
             {
                 SwitchableDebug.Log("Friend Request Fiald : " + error.ErrorMessage, debugType.PlayfabError);
             });
    }
    public static void AcceptFriendRequest(User user)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "AcceptFriendRequest",
            FunctionParameter = new
            {
                FriendPlayFabId = user.id,
            }
        };
        PlayFabClientAPI.ExecuteCloudScript(request,
              (result) =>
              {
                  SwitchableDebug.Log($"Friend Request sent", debugType.PlayfabError);
              },
             (error) =>
             {
                 SwitchableDebug.Log("Friend Request Fiald : " + error.ErrorMessage, debugType.PlayfabError);
             });
    }    
    public static void DenyFriendRequest(User user)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "DenyFriendRequest",
            FunctionParameter = new
            {
                FriendPlayFabId = user.id,
            }
        };
        PlayFabClientAPI.ExecuteCloudScript(request,
              (result) =>
              {
                  SwitchableDebug.Log($"Friend Request sent", debugType.PlayfabError);
              },
             (error) =>
             {
                 SwitchableDebug.Log("Friend Request Fiald : " + error.ErrorMessage, debugType.PlayfabError);
             });
    }
    public static void OnFindOtherUsers(string userName, string email,bool isUsingEmail, Action<User> resultCallback, Action<PlayFabError> OnError)
    {
        var request = new GetAccountInfoRequest { };

        if(isUsingEmail)
        {
            request.Email = email;
        }
        else
        {
            request.Username = userName;
        }

        PlayFabClientAPI.GetAccountInfo(request,
            (result) =>
            {
               User user = new User();
                user.id = result.AccountInfo.PlayFabId;
                user.name = result.AccountInfo.Username;
                resultCallback.Invoke(user);
            }, OnError);
    }
    public static void GetFriendList()
    {
        var request = new GetFriendsListRequest{ };

        PlayFabClientAPI.GetFriendsList(request, 
            (result) =>
             {
                 foreach (var friend in result.Friends)
                 {
                     if (friend.Tags[0] == "confirmed")
                     {
                         GameData.FriendList.users.Add(new User()
                         {
                             name = friend.Username,
                             id = friend.FriendPlayFabId,
                         });
                     }

                     if (friend.Tags[0] == "requester")
                     {
                         GameData.ReceivedPendingFriendRequest.users.Add(new User()
                         {
                             name = friend.TitleDisplayName,
                             id = friend.FriendPlayFabId,
                         });
                     }
                 }
                 GlobalEvents.OnUserFriendsReceived?.Invoke();
                 GameData.IsFriendListReceived = true;
                 SwitchableDebug.Log("Friend List Received", debugType.PlayfabData);
             },
             (error) =>
             {
                 SwitchableDebug.Log("get friends Field : " + error.ErrorMessage, debugType.PlayfabError);
             });
    }
    public static void RemoveFriendList(string id, Action<RemoveFriendResult> resultCallback)
    {
        var request = new RemoveFriendRequest { FriendPlayFabId = id };
    
        PlayFabClientAPI.RemoveFriend(request, resultCallback,
            (error) =>
            {
                SwitchableDebug.Log("remove friends Field : " + error.ErrorMessage, debugType.PlayfabError);
            });
    }
    #endregion

    #region User Inventory
    public static void GetUserInventory()
    {
        var request = new GetUserInventoryRequest
        {
            
        };
        PlayFabClientAPI.GetUserInventory(request, (result) =>
        {
            int gasCans = result.VirtualCurrency["GC"];
            SwitchableDebug.Log("User Inventory : " + gasCans, debugType.PlayfabError);
            GlobalEvents.OnVirtualCurrencyReceived?.Invoke(gasCans);
        },
        (error) =>
        {
            SwitchableDebug.Log("Getting User Inventory Field : " + error.ErrorMessage, debugType.PlayfabError);
        });
    }
    public static void UpdateUserInventory(int amount)
    {
        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = "GC",
            Amount = amount
        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, (result) =>
        {
            SwitchableDebug.Log("Gas Has Been Updated : " + result.Balance, debugType.Playfab);
        },
        (error) =>
        {
            SwitchableDebug.Log("Gas Subtract User Virtual Currency Field : " + error.ErrorMessage, debugType.PlayfabError);
        });
    }

    public static void GetUserCoins()
    {
        var request = new GetUserInventoryRequest
        {

        };
        PlayFabClientAPI.GetUserInventory(request, (result) =>
        {
            int gasCans = result.VirtualCurrency["CO"];
            GlobalEvents.OnCoinsReceived?.Invoke(gasCans);
        },
        (error) =>
        {
            SwitchableDebug.Log("Getting User Coins Field : " + error.ErrorMessage, debugType.PlayfabError);
        });
    }
    public static void UpdateUserCoins(int amount)
    {
        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CO",
            Amount = amount
        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, (result) =>
        {
            SwitchableDebug.Log("Coins Has Been Updated : " + result.Balance, debugType.PlayfabError);
        },
        (error) =>
        {
            SwitchableDebug.Log("Coins Subtract User Virtual Currency Field : " + error.ErrorMessage, debugType.PlayfabError);
        });
    }
    #endregion
}
