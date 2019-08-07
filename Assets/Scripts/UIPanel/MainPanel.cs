using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    private Text name;
    private GetFriendListRequest getFriendListRequest;

    [SerializeField] private VerticalLayoutGroup parent;
    [SerializeField] private GameObject frItem;
    [SerializeField] private RectTransform content;

    private bool isTimeToInsFrItem;
    private bool isTimeToShowNotifications;

    [SerializeField] private Button sfButton;
    [SerializeField] private Button msgButton;

    private List<FriendItem> friendItems = new List<FriendItem>();
    private List<int> notificationsIds = new List<int>();
    public Transform ChatPart;
    [SerializeField] private GameObject aPItem;

    private Dropdown dpdSwitchLib;
    public void AddNotificationsIdAndShow(string data)
    {
        string[] strs = data.Split(',');

        foreach (string ids in strs)
        {
            print(ids);
            notificationsIds.Add(int.Parse(ids));
        }

        isTimeToShowNotifications = true;
    }

    private void ShowNotifications()
    {
        foreach (FriendItem fi in friendItems)
        {
            if (notificationsIds.Contains(fi.Id))
            {
                fi.ShowNotification();
                notificationsIds.Remove(fi.Id);
            }
        }

        notificationsIds.Clear();
    }

    private int SyncShowNotificationId = -1;

    public void SyncShowNotification(int id)
    {
        SyncShowNotificationId = id;
    }

    public void ShowNotification(int id)
    {
        foreach (FriendItem fi in friendItems)
        {
            if (fi.Id == id)
            {
                fi.ShowNotification();
            }
        }
    }

    public void OnChatPanelHandClick(bool isOpen)
    {
        dpdSwitchLib.gameObject.SetActive(isOpen);
    }
    public override void InjectPanelThings()
    {
        name = transform.Find("upPart/name").GetComponent<Text>();
        dpdSwitchLib = transform.Find("upPart/dpdSwitchLib").GetComponent<Dropdown>();
        name.text = facade.GetNickname();
        getFriendListRequest = GetComponent<GetFriendListRequest>();
//        msgButton = transform.Find("upPart/msgButton").GetComponent<Button>();
//        msgButton.onClick.AddListener(() => { uiMng.PushPanel(UIPanelType.ChatPanel); });
        sfButton.onClick.AddListener(() => { uiMng.PushPanel(UIPanelType.SearchFriendPanel); });
        transform.Find("upPart/quitButton").GetComponent<Button>().onClick.AddListener(() => {
            if (uiMng.panelStack.Count == 3)
            {
                uiMng.PopPanel();
                uiMng.PopPanel();
            }
            else
            {
                uiMng.PopPanel();
            }
        });
    }


    public void OnGetFriendListResponse()
    {
        isTimeToInsFrItem = true;
    }

    public void OnGetNotificationReponse(string data)
    {
        AddNotificationsIdAndShow(data);
    }

    public void OnSendApplyNoticeResponse(string data)
    {
        string[] strs = data.Split(',');
        syncNickName = strs[0];
        syncId = strs[1];
    }

    private string syncNickName = null;
    private string syncId = null;

    private void InsAPItem(string nickName, string id)
    {
        GameObject g = Instantiate(aPItem);
        g.transform.SetParent(parent.transform);
        content.sizeDelta = new Vector2(content.sizeDelta.x, +content.sizeDelta.y + 65);
        g.GetComponent<ApplyForItem>().Structure(id, nickName, uiMng, facade);
    }

    private void Update()
    {
        if (isTimeToInsFrItem)
        {
            InstantiateFriendItem();
            isTimeToInsFrItem = false;
        }

        if (isTimeToShowNotifications)
        {
            ShowNotifications();
            isTimeToShowNotifications = false;
        }

        if (SyncShowNotificationId != -1)
        {
            ShowNotification(SyncShowNotificationId);
            SyncShowNotificationId = -1;
        }

        if (syncNickName != null || syncId != null)
        {
            InsAPItem(syncNickName, syncId);
            syncNickName = null;
            syncId = null;
        }
    }
    public void UpdateLineDot()
    {
    }
    public void InstantiateFriendItem()
    {
        if (friendItems.Count > 0)
        {
            content.BroadcastMessage("DestroyMe");
            friendItems.Clear();
        }

        foreach (Friend fr in facade.GetFriends())
        {
            GameObject g = Instantiate(frItem);
            g.transform.SetParent(parent.transform);
            g.transform.localScale=Vector3.one;

#if UNITY_ANDROID||UNITY_STANDALONE_WIN||UNITY_EDITOR
//            content.sizeDelta = new Vector2(content.sizeDelta.x, +content.sizeDelta.y + 150);
#endif
#if MAYBENO
            content.sizeDelta = new Vector2(content.sizeDelta.x, +content.sizeDelta.y + 65);
#endif
            FriendItem fi = g.GetComponent<FriendItem>();

            fi.nickname = fr.Nickname;
            fi.ShowInformation();
            fi.Id = fr.Id;
            fi.UIMng = uiMng;
            fi.Facade = facade;
            friendItems.Add(fi);
        }
    }

    public override void OnEnter()
    {
//        Invoke("iii", 1f);
        getFriendListRequest.SendRequest(facade.GetUserID());
        base.OnEnter();
        gameObject.SetActive(true);

//        msgButton.onClick.Invoke();
    }


    public override void OnResume()
    {
//        gameObject.SetActive(true);
    }

    public override void OnPause()
    {
//        gameObject.SetActive(false);
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }
}