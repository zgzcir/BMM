using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

[System.Serializable]
public class PlayAnima : MonoBehaviour {

    private Manager manager;

    private InputField Input;

    private Canvas canvas;

    private void Start()
    {
        manager = GameObject.Find("AnimManager").GetComponent<Manager>();
        Input = GetComponentInChildren<InputField>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        //this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void Update()
    {
        if(manager.IsPlay == false )
        {
            gameObject.tag = "Button";
        }
    }

    public void OnClick()
    {
        if (manager.IsPlay == false)
        {
            manager.ProcessQueue(Input.text.ToCharArray());
            this.gameObject.tag = "OnPlay";
        }
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
    }
}
