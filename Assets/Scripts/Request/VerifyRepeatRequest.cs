using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class VerifyRepeatRequest : BaseRequest
{
    private RegPanel regPanel;

    public override void Awake()
    {
        controllerCode = ControllerCode.User;
        requestCode = RequestCode.VerifyRepeat;
        regPanel = GetComponent<RegPanel>();
        base.Awake();
    }

    public override void SendRequest(string mail)
    {print(mail);
        base.SendRequest(mail);
    }

    public override void OnResPonse(string data)
    {
        ReturnCode returnCode = (ReturnCode) int.Parse(data);
        regPanel.OnVerifyRepateResponse(returnCode);
    }
}