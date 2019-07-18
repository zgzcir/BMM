using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTypeButton : BaseItem
{
    public Text buttonText;
    public ChatPanel cPanel;

    public Image icon;
    public Sprite hand;
    public Sprite voice;

    public Button voiceButoon;
    public Button handButoon;

    private void Start()
    {
        buttonText.text = cPanel.GetType().ToString();
        isDestructible = false;
    }


    public void ChangeType()
    {
        cPanel.ChangeType();
        buttonText.text = cPanel.GetType().ToString();
    }

    public void ChangeButton(SwitchManager.UserType ut)
    {
        if (ut == SwitchManager.UserType.Deaf)
        {
            voiceButoon.gameObject.SetActive(false);
            handButoon.gameObject.SetActive(true);
        }
        else
        {
            handButoon.gameObject.SetActive(false);
            voiceButoon.gameObject.SetActive(true);
        }
    }
}

