using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchUserPanel : BasePanel
{
 private Button serachButton;


 public override void InjectPanelThings()
 {
  transform.Find("serachButton").GetComponent<Button>().onClick.AddListener(OnSerachButtonClick);
 }

 private void OnSerachButtonClick()
 {
   
 }
}
