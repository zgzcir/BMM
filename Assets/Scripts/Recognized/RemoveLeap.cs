using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveLeap : MonoBehaviour
{
    public Text text;
    public handControllerDestroy hand;

    public void OnClick()
    {
        if (text.text != "聋哑人模式")
            hand.OnSendClick();

    }
}
