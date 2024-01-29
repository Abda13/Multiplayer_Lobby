using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SwitchableDebug
{
    public static void Log(string msg, debugType type)
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (SceneDeubg.inst)
        {
            if (SceneDeubg.inst.DisableAllDebug) return;

            if (shouldShow(type)) Debug.Log(msg);
        }
#endif
    }

    public static void Log(string msg, Object target ,debugType type)
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (SceneDeubg.inst)
        {
            if (SceneDeubg.inst.DisableAllDebug) return;

            if (shouldShow(type)) Debug.Log(msg, target);
        }
#endif
    }

    private static bool shouldShow(debugType type)
    {
        switch (type)
        {
            case debugType.Photon when SceneDeubg.inst.ShowPhotonDebug:
            case debugType.Playfab when SceneDeubg.inst.showPlayfabDebug:
            case debugType.PhotonServer when SceneDeubg.inst.showPhotonServerDebug:
            case debugType.PlayfabData when SceneDeubg.inst.showPlayfabDataDebug:
            case debugType.PlayfabError when SceneDeubg.inst.showPlayfabErrorDebug:
            case debugType.PhotonChat when SceneDeubg.inst.showPhotonChatDebug:
                return true;

            default:
                return false;
        }
    }
}

public enum debugType
{
    Photon,
    PhotonServer,
    PhotonChat,
    Playfab,
    PlayfabData,
    PlayfabError,
}