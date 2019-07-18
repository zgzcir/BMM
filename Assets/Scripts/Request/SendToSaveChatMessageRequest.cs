using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class SendToSaveChatMessageRequest : BaseRequest
{  private ChatPanel chatPanel;

    public override void Awake()
    {
        controllerCode = ControllerCode.Chat;
        requestCode = RequestCode.SendAndSaveChatMessage;
        chatPanel = GetComponent<ChatPanel>();
        base.Awake();
    }
   
    
    
}
