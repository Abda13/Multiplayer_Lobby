using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class JsonAsyncUtility 
{
    public static async void FromJson<T>(string content, Action<T> OnDone)
    {
        T data = default;

        Task task = new Task(() =>
        {
            data = JsonUtility.FromJson<T>(content);
        });

        task.Start();
        await task;
        OnDone?.Invoke(data);
    }

    public static async void ToJson<T>( T gameData, Action<string> OnDone)
    {
        string content = "";
        Task task = new Task(() =>
        {
             content = JsonUtility.ToJson(gameData);
        });
        task.Start();
        await task;
        OnDone?.Invoke(content);
    }
}
