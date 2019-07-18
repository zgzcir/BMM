using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class ApplyForAddFriendRequest : BaseRequest
{
    private SearchFriendItem _searchFriendItem;

    private void Awake()
    {
        controllerCode = ControllerCode.User;
        requestCode = RequestCode.ApplyForAddFriend;
        _searchFriendItem = GetComponent<SearchFriendItem>();
    }
    

}