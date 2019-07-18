using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class MessagesManager : BaseManager
{
    public MessagesManager(Facade facade) : base(facade)
    {
    }

//    private Dictionary<int,string> messagesDic=new Dictionary<int, string>();
    private RepeatableDictionary<int, string> messagesRdic = new RepeatableDictionary<int, string>();

    public void AddMessage(int id, string m)
    {
        messagesRdic.Add(id, m);
        ProcessMessage(id, m);
    }

    public List<string> GetMessage(int id)
    {
        return messagesRdic.GetAllValue(id);
    }

    private void ProcessMessage(int id, string m)
    {
        if (facade.NowChatID() == id)
        {
            ShowMessage(m);
        }
        else
        {
            ShowNotification(id);
        }
    }

    private void ShowMessage(string message)
    {
        facade.SyncInsFrChatItem(message);
    }

    private void ShowNotification(int id)
    {
        facade.SyncShowNotification(id);
    }
}