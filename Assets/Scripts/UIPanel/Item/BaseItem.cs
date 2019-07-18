using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    protected UIManager uiMng;
    protected Facade facade;


    protected bool isDestructible = true;


    public Facade Facade
    {
        set { facade = value; }
    }

    public UIManager UIMng
    {
        set { uiMng = value; }
    }

    public void DestroyMe()
    {if(isDestructible)
        Destroy(gameObject);
    }
}