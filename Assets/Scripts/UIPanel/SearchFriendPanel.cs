using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SearchFriendPanel : BasePanel
{
    [SerializeField] private InputField iF;
    [SerializeField] private GameObject outImage;
    private SearchFriendRequest searchFriendRequest;
    private Button backButton;

    [SerializeField] private GameObject sFItem;
    [SerializeField] private RectTransform content;
    private List<BaseItem> sfItems = new List<BaseItem>();


    public override void InjectPanelThings()
    {
        searchFriendRequest = GetComponent<SearchFriendRequest>();
        iF.onValueChanged.AddListener(value =>
        {
            searchFriendRequest.SendRequest(value);
            outImage.gameObject.SetActive(true);
        });
        iF.onValueChanged.AddListener((x) =>
            {
           
                for (int i = 0; i < sfItems.Count; i++)
                {
                      sfItems[i].DestroyMe();
                
                }
             sfItems.Clear();
              
            }
        );
        backButton = transform.Find("backButton").GetComponent<Button>();
        backButton.onClick.AddListener(() => { uiMng.PopPanel(); });
        base.InjectPanelThings();
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        base.OnEnter();
    }

    public void OnSearchFriendReseponse(string data)
    {
        if (data == "r")
        {
            return;
        }
        names.Clear();
        isNotTimeTODM = true;
        string[] strs1 = data.Split('|');
        string[] strs2 = strs1[0].Split(',');
        names.Add(strs1[1]);
        foreach (var VARIABLE in strs2)
        {
            names.Add(VARIABLE);
        }

        //TODO
    }

    private List<string> names = new List<string>();
    private bool isNotTimeTODM = true;

    private void Update()
    {
//        if (names.Count == 0&&isNotTimeTODM)
//        {
//            foreach (var VARIABLE in sfItems)
//            {
//                VARIABLE.DestroyMe();
//            }
//        }

        {
            for (int i = 1; i < names.Count; i++)
            {
                InsSerarchFriendItem(names[i], names[0]);
            }

            names.Clear();
            isNotTimeTODM = true;
        }
    }

//    private void Update()
//    {
//        if (isTimeToSyncInsSFItem)
//        {BroadcastMessage("DestoryMe");
//            foreach (var VARIABLE in userNames)
//            {
//                InsSFItem(VARIABLE,nickName);
//            }
//            isTimeToSyncInsSFItem = false;
//        }
//    }
//
//    private bool isTimeToSyncInsSFItem = false;
//    private List<string> userNames = new List<string>();
//    private string nickName;
//
    public void InsSerarchFriendItem(string username, string nickName)
    {
        GameObject g = Instantiate(sFItem);
        g.transform.SetParent(content);
        content.sizeDelta = new Vector2(content.sizeDelta.x, +content.sizeDelta.y + 101f);
        g.GetComponent<SearchFriendItem>().Structure(username, nickName,uiMng,facade);
        sfItems.Add(g.GetComponent<SearchFriendItem>());
    }
}