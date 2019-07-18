using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraRenderMode : MonoBehaviour
{
    private Canvas canvas;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public  void Onclick()
    {
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }
}
