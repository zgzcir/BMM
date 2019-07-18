using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class RequestManager : BaseManager
{
    public RequestManager(Facade facade) : base(facade)
    {
        
        
    }
    
    private Dictionary<RequestCode, BaseRequest> requestDic = new Dictionary<RequestCode, BaseRequest>();

    public void AddRequest(RequestCode requestCode, BaseRequest request)
    {
        requestDic.Add(requestCode,request);
    }

    public BaseRequest GetRequest(RequestCode requestCode)
    {
     return   requestDic.TryGet(requestCode);
    }

    public void RemoveRequest(RequestCode requestCode)
    {
        requestDic.Remove(requestCode);
    }
     

    public void HandleResPonse(RequestCode requestCode ,string data)
    {
       
        BaseRequest request = requestDic.TryGet(requestCode);
        if (request == null)
        {
            Debug.LogWarning("无法得到ActionCode[" + requestCode + "]对应的Request类");return;
        }
        request.OnResPonse(data);
    }
}
