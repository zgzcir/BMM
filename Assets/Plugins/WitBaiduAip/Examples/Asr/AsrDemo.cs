/*
 * 作者：关尔
 * 时间：2017年12月1日
 * 关注“洪流学堂”公众号，让你快人几步
 */

using UnityEngine;
using UnityEngine.UI;
using Wit.BaiduAip.Speech;

public class AsrDemo : MonoBehaviour
{
    public string APIKey = "";
    public string SecretKey = "";

    public Button StartButton;
    public Button StopButton;
    public Text DescriptionText;

    public InputField iF;

    private AudioClip _clipRecord;
    private Asr _asr;

    // Microphone is not supported in Webgl
#if !UNITY_WEBGL

    void Start()
    {
        _asr = new Asr(APIKey, SecretKey);
        StartCoroutine(_asr.GetAccessToken());
        StartButton.onClick.AddListener(OnClickStartButton);
        StopButton.onClick.AddListener(OnClickStopButton);
        StartButton.gameObject.SetActive(true);
        StopButton.gameObject.SetActive(false);
        DescriptionText.text = "";


    }

    private void OnClickStartButton()
    {
        StartButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(true);
        DescriptionText.text = "Listening...";

        _clipRecord = Microphone.Start(null, false, 30, 16000);
    }

    private void OnClickStopButton()
    {
        StartButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(false);
        DescriptionText.text = "Recognizing...";
        Microphone.End(null);
        Debug.Log("[WitBaiduAip demo]end record");
        var data = Asr.ConvertAudioClipToPCM16(_clipRecord);
        StartCoroutine(_asr.Recognize(data, s =>
        {
            if (s.result != null && s.result.Length > 0)
            {
                iF.text = s.result[0];
                DescriptionText.text = "";
            }
            else
            {
                DescriptionText.text = "未识别到声音";
            }

            StartButton.gameObject.SetActive(true);
        }));
    }
#endif
}