using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class SetReadedRequest : BaseRequest
{
    private void Awake()
    {
        controllerCode = ControllerCode.Chat;
        requestCode = RequestCode.SetReaded;
        base.Awake();
    }
    
    
}