using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCrete : MonoBehaviour
{
    public static TextCrete _instance;
    public static TextCrete instance{ get { return _instance; } }

    private  static int width = 0;
    public int Width { set { width = value; } }

    private static int height = 0;
    public int Height { set { height = value; } }

    private GameObject  text;
    private Transform  canvas;
    private LeapTrainer trainer;


    private string firstName="";

    public  List<GameObject> texts=new List<GameObject>();

    private void Awake()
    {
        _instance = this;
    }
    public List<string> txts = new List<string>(){"123","12312","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123","!2311123"};
    private int index = 0;
    private void Start()
    {
        canvas = GameObject.Find("Canvas").transform;
        trainer = GetComponent<LeapTrainer>();
        text = Resources.Load<GameObject>("Text");
        if (trainer)
        {
            trainer.OnGestureRecognized += Trainer_OnGestureRecognized;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && txts.Count>=index)
        {
            Trainer_OnGestureRecognized(null, 0, null);
        };
        if (Input.GetMouseButtonDown(1))
        {
            index = 0;
        };
    }
    void Trainer_OnGestureRecognized(string name, float value, Dictionary<string, float> allHits)
    {
        //bool isIdf = true;
        //string[] str = name.Split('\\');
        //string[] Name = str[1].Split('.');
        ////Debug.Log(count);
        //if (firstName != "")
        //{
        //    if (Name[0] == "可以")
        //    {
        //        Name[0] = "好";
        //    }
        //}
        //else if (Name[0] == "大家")
        //{
        //    Name[0] = "各位";
        //}

        //if (Name[0] == "大家")
        //{
        //    if (firstName == "他" || firstName == "你" || firstName == "我")
        //        Name[0] = "们";
        //}

        //firstName = Name[0];

        //foreach (var item in texts)
        //{
        //    if (item.GetComponent<Text>().text == Name[0])
        //        isIdf = false;
        //}

        //if (isIdf == false)
        //    return;

        width++;
        GameObject go = Instantiate(text);
        go.transform.SetParent(canvas, false);
        go.GetComponent<Text>().text = txts[index++];
        go.transform.localPosition = new Vector3(-320 + width * 130, 220 - height * 66, 0);
        if (-320 + width * 90 > 320)
        {
            width = 1;
            height++;
            go.transform.localPosition = new Vector3(-320 + width * 130, 220 - height * 66, 0);
        }
        //Debug.Log(-320 + count * 90);
        texts.Add(go);
    }
}
