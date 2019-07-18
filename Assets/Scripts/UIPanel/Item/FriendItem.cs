using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendItem : BaseItem
{
    public Text nicknameText;
    public Image lineDot;
    public Image notificationDot;
    public int Id { get; set; }
    public string nickname { get; set; }


    public void ShowNotification()
    {
        notificationDot.enabled = true;
    }
    public void CloseNotifaction()
    {
        notificationDot.enabled = false;
    }

    public void ShowInformation()
    {
        nicknameText.text = nickname;
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    public void SetLineDot()
    {
//TODO maybe
    }

    private void OnButtonClick()
    {
        uiMng.PushPanel(UIPanelType.ChatPanel);
        ChatPanel chatPanel = uiMng.GetPanel(UIPanelType.ChatPanel) as ChatPanel;
        chatPanel.NowChatId = Id;
        chatPanel.SetFrName(nickname);
        chatPanel.DestroyChatItem();
        CloseNotifaction();
        List<string> messages = facade.GetMessage(Id);
        foreach (string s in messages)
        {
            chatPanel.InsFrChatItem(s);
        }
    }
}