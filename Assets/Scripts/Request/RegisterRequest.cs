using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RegisterRequest : BaseRequest
{
 private RegPanel regPanel;

 public override void Awake()
 {
  controllerCode = ControllerCode.User;
  requestCode = RequestCode.Register;
   regPanel = GetComponent<RegPanel>();
  base.Awake();
 }

 public void SendRequest(string mailAddres,string password)
 {
  string data=mailAddres+','+password;
  base.SendRequest(data);
 }
 public override void OnResPonse(string data)
 {
       
  ReturnCode returnCode = (ReturnCode) int.Parse(data);
  regPanel.OnRegReponse(returnCode);
 }
}
