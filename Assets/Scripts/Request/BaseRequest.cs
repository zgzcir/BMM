using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class BaseRequest : MonoBehaviour
{
    protected ControllerCode controllerCode = ControllerCode.None;
    protected RequestCode requestCode = RequestCode.None;
    protected Facade _facade;

    protected Facade Facade
    {
        get
        {
            if (_facade == null)
            {
                _facade = Facade.Instance;
            }
            return _facade;
        }
    }

    public virtual void Awake()
    {
        Facade.AddRequest(requestCode,this);
    }

//    public virtual void SendRequest( )
//    {
//
//    }

    public virtual void OnResPonse(string data)
    {
        
    }

    public virtual void SendRequest(string data)
    {
//        print(data);
        Facade.SendRequest(controllerCode, requestCode,data );

    }
    
}
