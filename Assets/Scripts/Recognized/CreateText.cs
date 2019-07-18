using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateText : MonoBehaviour {

    public GameObject text;
    public LeapTrainer trainer;
    public  Transform Canvas;

    private List<GameObject> texts=new List<GameObject>();

    public static int height = 0;
    public static int width = 0;

    private string firstName = "";

    // Use this for initialization
    void Start () {
        text = Resources.Load<GameObject>("Text");
        Canvas = GameObject.Find("Canvas").transform;
        if (trainer)
        {
            trainer.OnGestureRecognized += Trainer_OnGestureRecognized;
        }
	}

    void Trainer_OnGestureRecognized(string name, float value, Dictionary<string, float> allHits)
    {
        bool isIdf = true;
        string[] str = name.Split('\\');
        string[] Name = str[1].Split('.');
        //Debug.Log(count);
        if (firstName != "")
        {
            if (Name[0] == "可以")
            {
                Name[0] = "好";
            }
        }
        else if (Name[0] == "大家")
        {
            Name[0] = "各位";
        }

        if (Name[0] == "大家")
        {
            if (firstName == "他" || firstName == "你" || firstName == "我")
                Name[0] = "们";
        }

        firstName = Name[0];

        foreach (var item in texts)
        {
            if (item.GetComponent<Text>().text == Name[0])
                isIdf = false;
        }

        if (isIdf == false)
            return;

        width++;
        GameObject go = Instantiate(text);
        go.transform.SetParent(Canvas, false);
        go.GetComponent<Text>().text = Name[0];
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

    // Update is called once per frame
    void Update () {
		
	}
}
