using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardSelector : MonoBehaviour
{
    private List<InputField> inputFields;
    private int indexNowSelect = 0;
    private int indexMax;
    private bool isShiftDown;

    public Button mainButton;
    public InputField mainInput;

    private void Start()
    {
        inputFields = GetComponentsInChildren<InputField>().ToList();
        foreach (var iF in inputFields)
        {
            if (iF.readOnly)
            {
                inputFields.Remove(iF);
            }
        }
        inputFields[0].Select();
        indexMax = inputFields.Count;
    }


 
    void Update()
    {
     
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isShiftDown = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isShiftDown = false;
        }

        if (isShiftDown)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (indexNowSelect - 1 >= 0)
                {
                    indexNowSelect--;
                }
                else
                {
                    indexNowSelect = indexMax - 1;
                }

                inputFields[indexNowSelect].Select();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (indexNowSelect + 1 < indexMax)
                {
                    indexNowSelect++;
                }
                else
                {
                    indexNowSelect = 0;
                }

                inputFields[indexNowSelect].Select();
            }
        }

        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && mainButton != null)
        { 
            mainButton.onClick.Invoke();
//            indexNowSelect = 0;
//            inputFields[indexNowSelect].Select();
           
        }
    }
}