using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    protected UIManager uiMng;
    protected Facade facade;
    protected Text showText;

    public Facade Facade
    {
        set { facade = value; }
    }

    public UIManager UIMng
    {
        set { uiMng = value; }
    }

    #region 面版生命周期

    public virtual void OnEnter()
    {
    }

    public virtual void OnPause()
    {
    }

    public virtual void OnResume()
    {
    }


    public virtual void OnExit()
    {
    }

    #endregion

    /// <summary>
    /// 初始化面板
    /// </summary>
    public virtual void InjectPanelThings()
    {
        try
        {
            showText = transform.Find("showText").GetComponent<Text>();
        }
        catch (Exception e)
        {
            Console.WriteLine("该panel未包含showText");
        }
    }

    public Text GetShowText()
    {
        if (showText != null)
        {
            return showText;
        }

        Debug.LogError("未找到showText");
        return null;
    }
}