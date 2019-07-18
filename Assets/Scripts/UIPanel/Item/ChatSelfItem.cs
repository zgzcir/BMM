using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChatSelfItem : BaseItem
{
    public string Message { get; set; }
    public InputField iF;
    public Button voiceButton;
    public Button handButton;
    private Button mainButton;
    private Button backButton;
    private ChatPanel cPanel;


    public void Structure(string message, UIManager uiMng, Facade facade, SwitchManager.UserType ut)
    {
        ChangeButton(ut);
        ShowMessage(message);
        UIMng = uiMng;
        Facade = facade;
    }

    public void ChangeButton(SwitchManager.UserType ut)
    {
        if (ut == SwitchManager.UserType.Normal)
        {
            if (handButton || voiceButton)
            {
                handButton.gameObject.SetActive(false);
                voiceButton.gameObject.SetActive(true);
                mainButton = voiceButton;
                backButton = handButton;
            }
        }
        else
        {
            if (handButton || voiceButton)
            {
                voiceButton.gameObject.SetActive(false);
                handButton.gameObject.SetActive(true);
                mainButton = handButton;
                backButton = voiceButton;
            }
        }
    }

    private void Start()
    {
        cPanel = GetComponentInParent<ChatPanel>();
    }

    private void ButtonAdptation()
    {
        float iFSizeX;
        iFSizeX = iF.GetComponent<RectTransform>().sizeDelta.x;
        mainButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(
                mainButton.GetComponent<RectTransform>().anchoredPosition3D.x - iFSizeX - 3f,
                mainButton.GetComponent<RectTransform>().anchoredPosition3D.y)
                  ;
        backButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(
                backButton.GetComponent<RectTransform>().anchoredPosition3D.x - iFSizeX - 3f,
                backButton.GetComponent<RectTransform>().anchoredPosition3D.y)
            ;
    }

    public void ShowMessage(string message)
    {
        Message = message;
        iF.text = Message;
        Invoke("ButtonAdptation", 0.03f);
    }

    public void OnButtonClick()
    {
        //TODO
        cPanel.Display();
    }
}