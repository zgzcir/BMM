using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class AddFriendRequest : BaseRequest
{
    private void Awake()
    {
        controllerCode = ControllerCode.User;
        requestCode = RequestCode.AddFriend;
        base.Awake();
    }
    
}
