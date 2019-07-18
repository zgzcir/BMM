using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RollingAdaptation : MonoBehaviour
{
    public float itemHeight;
    public float spacing;
    public float viewHeight;

    private int maxClampItemCount;
    private int nowItemCount;

    public RectTransform contentTransform;

    private const string PREFIX = "friend";

    
    
    public void InitializationData()
    {
        viewHeight = gameObject.GetComponent<RectTransform>().sizeDelta.y;
        spacing = GetComponentInChildren<VerticalLayoutGroup>().spacing;
        itemHeight = GameObject.Find("ViewPort/Content/" + PREFIX + "item").GetComponent<RectTransform>().sizeDelta.y;
        maxClampItemCount = (int) (viewHeight / (itemHeight + spacing));
    }

    

    public void Adapt()
    {
        if (nowItemCount > maxClampItemCount)
        {
            contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x,
                contentTransform.sizeDelta.y + itemHeight + spacing);
        }
    }
}