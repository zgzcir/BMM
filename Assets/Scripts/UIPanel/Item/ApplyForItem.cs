using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ApplyForItem : BaseItem
{
    [SerializeField] private Text nicknameText;
    [SerializeField] private Button applyButton;
    [SerializeField] private Button refusedButton;
  //  [SerializeField] private Animator ani;
    [SerializeField] private AddFriendRequest addFriendRequest;

    private int id;
    private string nickName;

    public void Structure(string id, string nickName, UIManager uiMng, Facade facade)
    {
        this.facade = facade;
        this.uiMng = uiMng;
        addFriendRequest = uiMng.GetPanel(UIPanelType.MainPanel).GetComponent<AddFriendRequest>();
        nicknameText.text = nickName;
        applyButton.onClick.AddListener(() =>
        {
            this.id = int.Parse(id);
            this.nickName = nickName;
            addFriendRequest.SendRequest(id + ',' + facade.GetUserID());
            DestroyMe();
        });
        refusedButton.onClick.AddListener(() =>
        {
            DestroyMe();
        });
    }
}