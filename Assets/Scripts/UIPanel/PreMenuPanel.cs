using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class PreMenuPanel : BasePanel
{
    private InputField nicknameIF;
    private SetFirstLoginInformationRequest setFirstLoginInformationRequest;

    public override void InjectPanelThings()
    {
        nicknameIF = transform.Find("nicknameIF").GetComponent<InputField>();
        transform.Find("confirmButton").GetComponent<Button>().onClick.AddListener(OnConfirmButtonClick);
        setFirstLoginInformationRequest = GetComponent<SetFirstLoginInformationRequest>();
    }

    private void OnConfirmButtonClick()
    {
        int id = facade.GetUserID();
        string data = id.ToString() + ',' + nicknameIF.text;
        setFirstLoginInformationRequest.SendRequest(data);
        facade.Setlogined(nicknameIF.text);
    }

    public void OnSetFirstLoginInformationResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMng.PushPanelSync(UIPanelType.MainPanel);
        }
        else
        {
            uiMng.SyncShowPanelMessage(UIPanelType.PreMenuPanel, "设置错误，请稍后再试");
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
    }

    public override void OnResume()
    {
        base.OnResume();
        gameObject.SetActive(true);
    }

    public override void OnPause()
    {
        gameObject.SetActive(false);
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }
}