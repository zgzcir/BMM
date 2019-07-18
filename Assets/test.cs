using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public VerticalLayoutGroup parent;
    public GameObject item;

    public RectTransform content;

    public void Ins()
    {
        GameObject g = Instantiate(item);
        g.transform.SetParent(parent.transform);
        content.sizeDelta=new Vector2( content.sizeDelta.x,+content.sizeDelta.y+65);
    }
}