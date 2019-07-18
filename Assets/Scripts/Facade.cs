using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.UI;

public class Facade : MonoBehaviour
{
    private static Facade _instance;


    private UIManager uiMng;
    private RequestManager requestMng;
    private ClientManager clientMng;
    private UserManager userMng;
    private MessagesManager mesMng;
    private SwitchManager swiMng;
    public static Facade Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        _instance = this;
        if (_instance == null)
        {
            new GameObject().AddComponent<Facade>();
            _instance = this;
        }
    }
    private void Start()
    {
        InitManager();
    }
    private void Update()
    {
        UpdateManager();
    }

    private void OnDestroy()
    {
        DestroyManager();
    }

    private void InitManager()
    {
        uiMng = new UIManager(this);
        requestMng = new RequestManager(this);
        clientMng = new ClientManager(this);
        userMng = new UserManager(this);
        mesMng = new MessagesManager(this);
        swiMng = new SwitchManager(this);
        uiMng.OnInit();
        requestMng.OnInit();
        clientMng.OnInit();
    }

    private void UpdateManager()
    {
        uiMng.Update();
        requestMng.Update();
        clientMng.Update();
        userMng.Update();
        mesMng.Update();
        swiMng.Update();
    }

    private void DestroyManager()
    {
        uiMng.OnDestroy();
        requestMng.OnDestroy();
        clientMng.OnDestroy();
        userMng.OnDestroy();
        mesMng.OnDestroy();
        swiMng.OnDestroy();
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void RemoveRequest(RequestCode requestCode)
    {
        requestMng.RemoveRequest(requestCode);
    }

    public void HandleResponse(RequestCode requestCode, string data)
    {
        requestMng.HandleResPonse(requestCode, data);
    }

    public void AddRequest(RequestCode requestCode, BaseRequest request)
    {
        requestMng.AddRequest(requestCode, request);
    }

    public void SendRequest(ControllerCode controllerCode, RequestCode requestCode, string data)
    {
        clientMng.SendRequest(controllerCode, requestCode, data);
    }


    public void SetUserInformation(int id, string username, string nickname, bool ifl)
    {
        userMng.SetUserInformation(id, username, nickname, ifl);
    }

    public bool IsFirstTimeLogin()
    {
        return userMng.IsFirstTimeLogin();
    }

    public void Setlogined(string nickname)
    {
        userMng.Setlogined(nickname);
    }

    public int GetUserID()
    {
        return userMng.GetUserID();
    }

    public string GetNickname()
    {
        return userMng.GetNickname();
    }

    public void AddFriendInformation(Friend fr)
    {
        userMng.AddFriendInformation(fr);
    }

    public void ClearFriendInformation()
    {
        userMng.ClearFriendInformation();
    }

    public List<Friend> GetFriends()
    {
        return userMng.GetFriends();
    }

    public void AddMessage(int id, string m)
    {
        mesMng.AddMessage(id, m);
    }

    public List<string> GetMessage(int id)
    {
        return mesMng.GetMessage(id);
    }

    public void SyncInsFrChatItem(string message)
    {
        (uiMng.GetPanel(UIPanelType.ChatPanel) as ChatPanel).SyncInsFrChatItem(message);
    }

    public int NowChatID()
    {
        return (uiMng.GetPanel(UIPanelType.ChatPanel) as ChatPanel).NowChatId;
    }

    public void SyncShowNotification(int id)
    {
        (uiMng.GetPanel(UIPanelType.MainPanel) as MainPanel).SyncShowNotification(id);
    }

    public BaseRequest GetRequest(RequestCode requestCode)
    {
       return requestMng.GetRequest(requestCode);
    }

    public void ChangeType()
    {
        swiMng.ChangeType();
    }

    public SwitchManager.UserType GetType()
    {
        return swiMng.GetType();
    }
    
    
}