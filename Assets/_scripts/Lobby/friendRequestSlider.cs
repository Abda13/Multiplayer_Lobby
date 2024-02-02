
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UITween;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class friendRequestSlider : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI body;
    [SerializeField] ChatType sliderType;

    Queue<ChatContent> chatContents = new Queue<ChatContent>();

    User user = new User();

    RectTransform rect;
    Vector3 localPos;
    bool isOn;
    float timer;
    //LayoutElement layoutElement;
    private void Awake()
    {
        rect = transform.GetChild(0) as RectTransform;
        localPos = rect.localPosition;
        //  layoutElement = GetComponent<LayoutElement>();
    }


    private void Start()
    {
       
        //layoutElement.ignoreLayout = true;
    }
    private void OnEnable()
    {
        GlobalEvents.OnChatReceived += chatRecived;
    }
    private void OnDisable()
    {
        GlobalEvents.OnChatReceived -= chatRecived;
    }

    private void chatRecived(ChatContent content)
    {
        if(content.chatType == sliderType)
        {
          
            if(isOn)
            {
                chatContents.Enqueue(content);
                return;
            }

            user.id = content.id;
            user.name = content.senderName;
            var userName = content.senderName;

            isOn = true;
            timer = Time.time + 5f;

            switch (sliderType)
            {
                case ChatType.Request:
                    body.text = $"{userName} sent you a friend request";
                    break;
                case ChatType.Invite:
                    body.text = $"{userName} invite you to join there lobby";
                    break;

                case ChatType.Massage:
                    if(GameData.IsChatWindowOpen)
                    {
                        if (!string.IsNullOrEmpty(GameData.CurrentChatWindowUser.id))
                        {
                            if (GameData.CurrentChatWindowUser.id.Equals(content.id, StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                        }
                    }
                    body.text = $"{userName} :{content.msg}";
                    break;
            }


            showSlider();
        }
    }

    private void Update()
    {
        if(isOn)
        {
            if(timer < Time.time)
            {
                HideSlider();
                timer = Time.time + 5f;
            }
        }
    }
    public void OnAccept()
    {
        if (sliderType == ChatType.Request)
        {
            GameUtility.OnAcceptFriendRequest(user);
            body.text = "Now you are friends";
            Invoke(nameof(HideSlider), 2f);
        }
        if (sliderType == ChatType.Massage)
        {
           GlobalEvents.OnOpenChat(user);
            HideSlider();
        }
        if (sliderType == ChatType.Invite)
        {
            GlobalEvents.OnAcceptInvite(user);
            body.text = "Now you are friends";
        }
   
    }
    [ContextMenu("showSlider")]
    public void showSlider()
    {
      //  layoutElement.ignoreLayout = false;
        rect.LocalMove(Vector3.zero, 0.3f);
    }
    public void HideSlider()
    {
        rect.LocalMove(localPos, 0.3f);

        Invoke(nameof(checkForOthers),.5f);
    }
    void checkForOthers()
    {
       // layoutElement.ignoreLayout = true;
        isOn = false;
        if (chatContents.Count != 0)
            chatRecived(chatContents.Dequeue());
    }


}
