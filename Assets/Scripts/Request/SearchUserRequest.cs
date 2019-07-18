using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class SearchUserRequest : BaseRequest
{
    private SearchUserPanel searchUserPanel;

    public override void Awake()
    {
        controllerCode = ControllerCode.User;
        requestCode = RequestCode.SearchUser;
        searchUserPanel = GetComponent<SearchUserPanel>();
        base.Awake();
        
    }

}
