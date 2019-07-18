using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class recordSubmit : MonoBehaviour
{
    public InputField recordInputField;
    public LeapRecorderController recordFilePath;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnSubmit()
    {
        //Debug.Log(recordInputField.text);
        if (!string.IsNullOrEmpty(recordInputField.text))
        {
            string s = "AnimiFile自制\\" + recordInputField.text + ".json";
            Debug.Log(s);
            recordFilePath.recorderFilePath = s;
            recordInputField.text = "";
        }
    }
}
