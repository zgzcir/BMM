using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureCreate : MonoBehaviour
{

    public GameObject FrItem;
    private Transform canvas;


    private void Awake()
    {
        canvas = GameObject.Find("Canvas").transform;
    }

    public void OnClick()
    {
        GameObject go = Instantiate(FrItem);
        go.transform.SetParent(canvas,false);
        go.name = this.GetComponent<Text>().text;
        Image texture = go.GetComponentInChildren<Image>();
        texture.sprite = Resources.Load<Sprite>("Texture/" + this.GetComponent<Text>().text);
        foreach (var item in TextCrete.instance.texts)
        {
            item.GetComponent<Button>().enabled = false;
        }
    }
}
