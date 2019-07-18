using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net.Mail;
using System.Net;
using System;
public class Mail : MonoBehaviour {

    public void SendMail(string mailAddress,int guardCode)
    {
        try
        {print(guardCode);
            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("zgzzhenhao@163.com", "SRLS");
            mailMsg.To.Add(new MailAddress(mailAddress, "duifang"));
            mailMsg.Subject ="验证您的身份";
            mailMsg.Body = "Here is the SLRS Guard code you need:"+guardCode;
            SmtpClient client = new SmtpClient("smtp.163.com",25);  //发送服务器
            client.Credentials = (ICredentialsByHost)new NetworkCredential("zgzzhenhao@163.com", "978102863");
            client.Send(mailMsg);
        }
        catch (Exception ex)
        {
            Debug.Log("异常"+ex.StackTrace);
        } 
    }
}