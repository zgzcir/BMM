using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class GetFriendListRequest : BaseRequest
{
    private MainPanel mainPanel;

    public override void Awake()
    {
        controllerCode = ControllerCode.User;
        requestCode = RequestCode.GetFriendList;
        mainPanel = GetComponent<MainPanel>();
        base.Awake();
    }

    public void SendRequest(int id)
    {
        base.SendRequest(id.ToString());
    }


    public override void OnResPonse(string data)
    {
        _facade.ClearFriendInformation();
        string[] strs1 = data.Split('#');
        if (!data.Equals("1"))
        {
            string[] strs2 = strs1[1].Split('|');
            foreach (string s in strs2)
            {
                string[] frs = s.Split(',');
                Friend friend = new Friend(int.Parse(frs[0]), frs[1]);
//                bool isOnLine = bool.Parse(frs[2]);
                //TODO 好友在线状态
                _facade.AddFriendInformation(friend);
            }

            mainPanel.OnGetFriendListResponse();
        }
        else
        {
            //nothing now
        }
    }
}