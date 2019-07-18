using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class SearchFriendItem : BaseItem
{
    [SerializeField] private Text usernameText;
    [SerializeField] private Text nicknameText;
    [SerializeField] private Text noticeText;
    [SerializeField] private Button sendButton;

    private ApplyForAddFriendRequest applyForAddFriendRequest;

    private void Awake()
    {
        applyForAddFriendRequest = GetComponent<ApplyForAddFriendRequest>();
        sendButton.onClick.AddListener(() =>
        {
            applyForAddFriendRequest.SendRequest(usernameText.text + ',' + facade.GetNickname()+','+facade.GetUserID());
            sendButton.gameObject.SetActive(false);
            noticeText.enabled = true;
        });
    }

    public void Structure(string username, string nickName, UIManager uiMng, Facade facade)
    {
        usernameText.text = username;
        nicknameText.text = nickName;
        UIMng = uiMng;
        Facade = facade;
    }
}