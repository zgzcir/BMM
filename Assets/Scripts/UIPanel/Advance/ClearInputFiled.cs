using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearInputFiled : MonoBehaviour
{
  public void ResetIF()
  {
    GetComponent<InputField>().text = "";
    GetComponent<InputField>().Select();
  }
}
