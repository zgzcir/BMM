using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class SetFirstLoginInformationRequest : BaseRequest
{
    private PreMenuPanel preMenuPanel;
    public override void Awake()
    {
        controllerCode = ControllerCode.User;
        requestCode = RequestCode.SetFirstLoginInformation;
        preMenuPanel = GetComponent<PreMenuPanel>();
        base.Awake();
    }

    public void SendRequest(string data)
    {
        base.SendRequest(data);
    }

    public override void OnResPonse(string data)
    {
        ReturnCode returnCode = (ReturnCode) int.Parse(data);
        preMenuPanel.OnSetFirstLoginInformationResponse(returnCode);
    }
}