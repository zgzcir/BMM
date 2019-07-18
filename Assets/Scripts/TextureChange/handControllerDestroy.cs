using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class handControllerDestroy : MonoBehaviour
{

    private bool  IsNull=true;
    public GameObject qingKong;
    public GameObject allSend;
//
//    private Text  kuSelf;
//    private Text  kuSystem;

    // Start is called before the first frame update
    void Start()
    {
//        kuSelf = GameObject.FindWithTag("kuSelf").GetComponent<Text>();
//        kuSystem = GameObject.FindWithTag("kuSystem").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("LeapController(Clone)"))
            IsNull = false;
        else
            IsNull = true;
    }

    public void OnSendClick()
    {
        if (IsNull)
            return;
        if (TextCrete.instance.texts == null)
            return;
        TextCrete.instance.Width = 0;
        TextCrete.instance.Height = 0;
        foreach (var item in TextCrete.instance.texts)
        {
           //2  2 TextCrete.instance.texts.Remove(item);
            Destroy(item.gameObject);
        }

//        kuSelf.text = "预留";
//        kuSystem.text = "预留";

        TextCrete.instance.texts.Clear();
        Destroy(GameObject.Find("LeapController(Clone)"));
        qingKong.SetActive(false);
        allSend.SetActive(false);
    }
}
