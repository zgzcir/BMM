using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPanel : BasePanel
{

    private InputField inputText;

    private void Awake()
    {
        inputText = GameObject.FindGameObjectWithTag("Input").GetComponent<InputField>();
    }

    /// <summary>
    /// 关闭图片,并使按钮能用
    /// </summary>
    public void OnCloseClick()
    {
        
        foreach(var item in TextCrete.instance.texts)
        {
            item.GetComponent<Button>().enabled = true;
        }
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 关闭图片,使按钮能用并发送信息到输入面
    /// </summary>
    public void OnSendClick()
    {
        
        foreach (var item in TextCrete.instance.texts)
        {
            item.GetComponent<Button>().enabled = true;
        }
        string s = inputText.text;
        inputText.text = s+this.gameObject.name;
        Destroy(this.gameObject);
    }
}
