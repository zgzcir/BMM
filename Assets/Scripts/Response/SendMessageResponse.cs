using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class SendMessageResponse : BaseRequest
{
    public override void Awake()
    {
        controllerCode = ControllerCode.Chat;
        requestCode = RequestCode.SendMessage;
        base.Awake();
    }

    public override void OnResPonse(string data)
    {
        print(data);
        string[] strs = data.Split(',');
        string m = strs[0];
        int id = int.Parse(strs[1]);
        _facade.AddMessage(id, m);
//Here 解析 消息+发送id 然后存入词典/返回request表示读取状况
    }
}