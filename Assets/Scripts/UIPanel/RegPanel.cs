using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class RegPanel : BasePanel
{
    private InputField mailIF;
    private InputField codeIF;
    private InputField passwordIF;
    private InputField rePasswordIF;

    private Button sendCodeBtn;
    private Button regBtn;
    private Button cancelBtn;

    private string syncShowText;

    private Mail mail;

    private RegisterRequest registerRequest;
    private VerifyRepeatRequest verifyRepeatRequest;

    private bool isSyncRandomGuardCode;

    private int guardCode;


    public override void InjectPanelThings()
    {
        transform.Find("sendCodeBtn").GetComponent<Button>().onClick.AddListener(OnSendCodeBtnCLick);
        transform.Find("regBtn").GetComponent<Button>().onClick.AddListener(OnRegBtnClick);
        transform.Find("upPart/cancelBtn").GetComponent<Button>().onClick.AddListener(OnCancelBtnClick);


        mailIF = transform.Find("mailIF").GetComponent<InputField>();
        codeIF = transform.Find("codeIF").GetComponent<InputField>();
        passwordIF = transform.Find("passwordIF").GetComponent<InputField>();
        rePasswordIF = transform.Find("rePasswordIF").GetComponent<InputField>();

        mail = GetComponent<Mail>();
        registerRequest = GetComponent<RegisterRequest>();
        verifyRepeatRequest = GetComponent<VerifyRepeatRequest>();
        base.InjectPanelThings();
    }

    private void Update()
    {
        if (isSyncRandomGuardCode)
        {
            guardCode = Random.Range(10000, 99999);
            mail.SendMail(mailIF.text, guardCode);
            isSyncRandomGuardCode = false;
        }
    }

    private void OnSendCodeBtnCLick()
    { 
        if (string.IsNullOrEmpty(mailIF.text))
        {
            uiMng.ShowIFWarningMessage(mailIF, "电子邮件不能为空");
                return;
        }
        verifyRepeatRequest.SendRequest(mailIF.text);
    }

    private void SyncRandomGuardCode()
    {
        isSyncRandomGuardCode = true;
    }

 
    public void OnVerifyRepateResponse(ReturnCode  returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            SyncRandomGuardCode();

        }
        else
        {
            uiMng.SyncShowPanelMessage(UIPanelType.RegPanel,"该邮箱已注册，请更换其他邮箱");
        }
    }

    private void OnRegBtnClick()
    {
        bool isMeet = true;
        if (string.IsNullOrEmpty(mailIF.text))
        {
            uiMng.ShowIFWarningMessage(mailIF, "电子邮件不能为空");
            isMeet = false;
        }

        if (!Equals(codeIF.text, guardCode.ToString()))
        {
            uiMng.ShowIFWarningMessage(codeIF, "验证码错误");
            isMeet = false;
        }

        if (string.IsNullOrEmpty(passwordIF.text))
        {
            uiMng.ShowIFWarningMessage(passwordIF, "密码不能为空");
            isMeet = false;
        }

        if (string.IsNullOrEmpty(rePasswordIF.text) || !Equals(rePasswordIF.text, passwordIF.text))
        {
            uiMng.ShowIFWarningMessage(rePasswordIF, "密码输入不一致");
            isMeet = false;
        }

        if (!isMeet)
        {
            return;
        }

        registerRequest.SendRequest(mailIF.text, passwordIF.text);
    }

    public void OnRegReponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMng.SyncPopPanel();
        }
        else
        {
            uiMng.SyncShowPanelMessage(UIPanelType.RegPanel, "注册失败，请稍后再试");
        }
    }

    private void OnCancelBtnClick()
    {
        uiMng.PopPanel();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
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