using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllSend : MonoBehaviour
{

    private InputField inputText;

    private void Awake()
    {


#if  DEBUG
      inputText = GameObject.FindGameObjectWithTag("Input").GetComponent<InputField>();
#endif
    }

    public void OnClick()
    {
        string s = inputText.text;
        foreach (var item in TextCrete.instance.texts)
        {
            s += item.GetComponent<Text>().text;
        }
        inputText.text = s;
    }
}
