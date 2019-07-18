using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChatPanel : BasePanel
{
    public int NowChatId { get; set; }
    private Text frName;
    private InputField msIF;
    private SendToSaveChatMessageRequest scmRequest;
    private KeyboardSelector keyboardSelector;
    private List<ChatSelfItem> chatSelfItems = new List<ChatSelfItem>();
    private List<ChatFriendItem> chatFrItems = new List<ChatFriendItem>();

    public VerticalLayoutGroup parent;
    public GameObject frItem;
    public GameObject selfItem;
    public RectTransform content;
    public ChangeTypeButton ctButton;

    public GameObject LeapController;
#if UNITY_STANDALONE_WIN
    public GameObject qingKong;
    public GameObject allSend;

    public GameObject message;
#endif
    private Transform Camera;
    private Canvas canvas;

    public GameObject LeapMotionController;

    private MainPanel mainPanel;

//    private Text kuSelf;
//    private Text kuSystem;

    private void Awake()
    {
        Camera = GameObject.Find("Main Camera").transform;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
#if DEBUG
#if UNITY_STANDALONE_WIN
//        kuSelf = GameObject.FindWithTag("kuSelf").GetComponent<Text>();
//        kuSystem = GameObject.FindWithTag("kuSystem").GetComponent<Text>();
        qingKong.SetActive(false);
        allSend.SetActive(false);
        message.SetActive(false);
#endif
#endif
        //        msIF.onValueChanged.AddListener((data) =>
        //        {
        //            (transform as RectTransform).localPosition += new Vector3(0, 300, 0);
        //        });
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
    }
    //
    //    public override void OnPause()
    //    {
    //        gameObject.SetActive(false);
    //    }
    //
    //    public override void OnResume()
    //    {
    //        gameObject.SetActive(true);
    //    }

    public UIManager GetUiMng()
    {
        return uiMng;
    }

    public Facade GetFacade()
    {
        return facade;
    }

    private List<string> InsChatFrItemDatas = new List<string>();
    private bool isTimeToInsChatFrItem;

    private void Update()
    {
        if (isTimeToInsChatFrItem)
        {
            foreach (var message in InsChatFrItemDatas)
            {
                InsFrChatItem(message);
            }

            isTimeToInsChatFrItem = false;
        }

        if (!string.IsNullOrEmpty(syncInsFrChatItemMessage))
        {
            InsFrChatItem(syncInsFrChatItemMessage);
            syncInsFrChatItemMessage = null;
        }
    }

    public void DestroyChatItem()
    {
        if (chatSelfItems.Count == 0 || chatFrItems.Count == 0)
            return;
        BroadcastMessage("DestroyMe");
    }

    public void DestoryMe()
    {
    }

    public override void InjectPanelThings()
    {
        transform.Find("frName/back").GetComponent<Button>().onClick.AddListener(() => { uiMng.PopPanel(); });
        mainPanel = FindObjectOfType<MainPanel>();
        ctButton = transform.Find("ms/cb").GetComponent<ChangeTypeButton>();
        ctButton.GetComponent<Button>().onClick.AddListener(ChangeType);
        frName = transform.Find("frName/Text").GetComponent<Text>();
        msIF = transform.Find("ms/msIF").GetComponent<InputField>();
        transform.Find("ms/sendBtn").GetComponent<Button>().onClick.AddListener(OnSendButtonClick);
        scmRequest = GetComponent<SendToSaveChatMessageRequest>();
        keyboardSelector = GetComponent<KeyboardSelector>();
        transform.SetParent(mainPanel.ChatPart, false);

        base.InjectPanelThings();
    }

    public void SetFrName(string name)
    {
        frName.text = name;
    }

    private void OnSendButtonClick()
    {
        if (string.IsNullOrEmpty(msIF.text) || NowChatId == 0)
        {
            uiMng.ShowPanelMessage(UIPanelType.ChatPanel, "。。。");
            return;
        }

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        string data = msIF.text + ',' + NowChatId;
        InsSelfChatItem(msIF.text);
        scmRequest.SendRequest(data);
        GetComponentInChildren<ClearInputFiled>().ResetIF();
    }

    public void OnReciveChatMessageResponse(string data)
    {
        string[] strs = data.Split(',');
        foreach (var message in strs)
        {
            InsChatFrItemDatas.Add(message);
        }

        isTimeToInsChatFrItem = true;
    }

    private string syncInsFrChatItemMessage;

    public void SyncInsFrChatItem(string message)
    {
        syncInsFrChatItemMessage = message;
    }

    public void ChangeType()
    {
        facade.ChangeType();
        ctButton.ChangeButton(facade.GetType());
        ChangeButton(facade.GetType());
    }


    public SwitchManager.UserType GetType()
    {
        return facade.GetType();
    }

    public void Display()
    {
        uiMng.PushPanel(UIPanelType.DisplayPanel);
    }

    public void InsFrChatItem(string message)
    {
        GameObject g = Instantiate(frItem);
        g.transform.SetParent(parent.transform);
        content.sizeDelta = new Vector2(content.sizeDelta.x, +content.sizeDelta.y + 170);
        g.transform.localScale = Vector3.one;

        g.GetComponent<ChatFriendItem>().Structure(message, uiMng, facade, facade.GetType());
        chatFrItems.Add(g.GetComponent<ChatFriendItem>());
    }

    public void InsSelfChatItem(string message)
    {
        GameObject g = Instantiate(selfItem);
        g.transform.SetParent(parent.transform);
        content.sizeDelta = new Vector2(content.sizeDelta.x, +content.sizeDelta.y + 170);
        g.transform.localScale = Vector3.one;
        g.GetComponent<ChatSelfItem>().Structure(message, uiMng, facade, facade.GetType());
        chatSelfItems.Add(g.GetComponent<ChatSelfItem>());
    }

    public void ChangeButton(SwitchManager.UserType ut)
    {
        foreach (var sitem in chatSelfItems)
        {
            sitem.ChangeButton(ut);
        }

        foreach (var fitem in chatFrItems)
        {
            fitem.ChangeButton(ut);
        }
    }
#if UNITY_STANDALONE_WIN
    public void OnHandClick()
    {
        message.SetActive(true);
        Invoke("MesDestroy", 1);
        mainPanel.OnChatPanelHandClick(false);
        if (LeapMotionController)
            return;
        qingKong.SetActive(true);
        allSend.SetActive(true);
        LeapMotionController = Instantiate(LeapController);
        Transform tran = LeapMotionController.transform;
        LeapMotionController.transform.SetParent(Camera);
        LeapMotionController.transform.localPosition = tran.position;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        mainPanel.OnChatPanelHandClick(true);
//        kuSelf.text = "使用自制手势库";
//        kuSystem.text = "使用默认手势库";
    }

    public void OnCleanClick()
    {
        qingKong.SetActive(true);
        allSend.SetActive(true);
        LeapMotionController = Instantiate(LeapController);
        Transform tran = LeapMotionController.transform;
        LeapMotionController.transform.SetParent(Camera);
        LeapMotionController.transform.localPosition = tran.position;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
    }

    public void MesDestroy()
    {
        message.SetActive(false);
    }
#endif
}