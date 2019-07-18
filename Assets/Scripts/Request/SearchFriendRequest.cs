using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class SearchFriendRequest : BaseRequest
{
    private SearchFriendPanel searchFriendPanel;

    public override void Awake()
    {
        controllerCode = ControllerCode.User;
        requestCode = RequestCode.SearchFriend;
        searchFriendPanel = GetComponent<SearchFriendPanel>();
        base.Awake();
    }

    public override void OnResPonse(string data)
    {
        searchFriendPanel.OnSearchFriendReseponse(data);
    }
}