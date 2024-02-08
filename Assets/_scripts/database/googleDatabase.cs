using Aba.Chat;
using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chat.Database
{
    public static class googleDatabase
    {
        static string url = "https://chathistory-6d932-default-rtdb.firebaseio.com";

        public static void PostToDatabase(string otherID, ChatUserInfo chatUserInfo)
        {
            RestClient.Post(url + "/" + otherID + GameData.UserID + "/.json", chatUserInfo).Then(x =>
            {
                SwitchableDebug.Log("Chat Posted To data Base", debugType.datadase);

            });
        }
    }
}
