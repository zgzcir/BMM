using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playSubmit : MonoBehaviour
{

    public  InputField playInputField;
    public LeapRecorderController PlayFilePath;


    private void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnSubmit()
    {
        if(!string.IsNullOrEmpty(playInputField.text))
        {
                    string s = "AnimiFile自制\\" + playInputField.text + ".json";
            PlayFilePath.playerFilePath = s;
            playInputField.text = "";
        }
    }
}
