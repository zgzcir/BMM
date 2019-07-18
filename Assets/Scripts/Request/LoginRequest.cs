using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class LoginRequest : BaseRequest
{
    private LoginPanel loginPanel;

    public override void Awake()
    {
        controllerCode = ControllerCode.User;
        requestCode = RequestCode.Login;
        loginPanel = GetComponent<LoginPanel>();
        base.Awake();
    }

//    private void Start()
//    {
//        controllerCode = ControllerCode.User;
//        requestCode = RequestCode.Login;
//        loginPanel = GetComponent<LoginPanel>();
//    }


    public void SendRequest(string username, string password)
    {
        string data = username + ',' + password;
        base.SendRequest(data);
    }

    public override void OnResPonse(string data)
    {
        ReturnCode returnCode;
        string nickname = "null";
        string[] strs = data.Split(',');
        if (strs.Length > 1)
        {
            int id = int.Parse(strs[0]);
            string username = strs[1];
            nickname = strs[2];
            bool ifl = Boolean.Parse(strs[3]);
            _facade.SetUserInformation(id, username, nickname, ifl);
            returnCode = (ReturnCode) int.Parse(strs[4]);
        }
        else
        {
            returnCode = ReturnCode.Fail;
        }

        loginPanel.OnLoginResponse(returnCode, nickname);
    }
}