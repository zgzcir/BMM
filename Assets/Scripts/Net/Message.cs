using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Common;

public class Message
{
    public int dynamicLength;
    private byte[] data = new byte[1024];

    public byte[] Data
    {
        get { return data; }
    }

    public int DynamicLength { get; } = 0;

    public int RemainSize
    {
        get { return data.Length - dynamicLength; }
    }


    public void ReadMessage(int newDataAmount, Action<RequestCode, string> processDataCallBack)
    {
        dynamicLength += newDataAmount;
        while (true)
        {
            if (newDataAmount < 4)
            {
                return;
            }

            int oneLength = BitConverter.ToInt32(data, 0);
            if (dynamicLength >= oneLength)
            {
                RequestCode requestCode = (RequestCode) BitConverter.ToInt32(data, 4);
                string s = Encoding.UTF8.GetString(data, 8, oneLength - 4);
                processDataCallBack(requestCode, s);
                Array.Copy(data, oneLength + 4, data, 0, dynamicLength - 4 - oneLength);
                dynamicLength -= oneLength + 4;
            }
            else
            {
                return;
            }
        }
    }


    public static byte[] PackData(ControllerCode controllerCode, RequestCode requestCode, string data)
    {
        byte[] controllerCodeBytes = BitConverter.GetBytes((int) controllerCode);
        byte[] requestCodeBytes = BitConverter.GetBytes((int) requestCode);

        byte[] dataBytes = Encoding.UTF8.GetBytes(data);

        int dataAmount = controllerCodeBytes.Length + requestCodeBytes.Length + dataBytes.Length;

        byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);

        byte[] finalBytes = dataAmountBytes.Concat(controllerCodeBytes).Concat(requestCodeBytes).Concat(dataBytes)
            .ToArray();

        return finalBytes;
    }
}