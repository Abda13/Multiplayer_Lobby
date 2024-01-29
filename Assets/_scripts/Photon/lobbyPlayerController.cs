using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine;

public class lobbyPlayerController : MonoBehaviour
{


    public string PlayerName => pv.Owner.NickName;
    public string Id => pv.Owner.UserId;
    public bool IsLocal => pv.IsMine;
    public bool IsSpeaking => pvv.IsSpeaking;
    public AudioSource MyPlayerSpeaker => myPlayerSpeaker;


    [SerializeField] private AudioSource myPlayerSpeaker;

    private PhotonVoiceView pvv;
    private PhotonView pv;



    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        pvv = GetComponent<PhotonVoiceView>();
    }
    private void OnEnable()
    {

        if (IsLocal) GameData.LocalLobbyPlayerController = this;
        else GameData.RemoteLobbyPlayerControllers.Add(this);

        GlobalEvents.OnPlayerJoinedRoom?.Invoke(this);
    }

    private void OnDisable()
    {
        if (IsLocal) GameData.LocalLobbyPlayerController = null;
        else GameData.RemoteLobbyPlayerControllers.Remove(this);

        GlobalEvents.OnPlayerLeftRoom?.Invoke(this);
    }
}
