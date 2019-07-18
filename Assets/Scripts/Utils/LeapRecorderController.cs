using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class LeapRecorderController : MonoBehaviour
{

    public RecordedServiceProvider recordedProvider;

    public string recorderFilePath;
    public string playerFilePath;
    public string gestureSaveFilePath;

    public bool isPose;

    private Hand_Controller_gesture handCon;

    public KeyCode keyToRecord = KeyCode.R;
    public KeyCode keyToSave = KeyCode.S;
    public KeyCode keyToReset = KeyCode.E;
    public KeyCode keyToPause = KeyCode.Space;
    public KeyCode keyToPlay = KeyCode.Z;
    public KeyCode keyToLoad = KeyCode.L;

    public LeapRecorder recorder;

    public Text message;

    // Use this for initialization
    void Start()
    {
        if (recorder == null)
            this.recorder = recordedProvider.GetLeapRecorder();
        handCon = GetComponent<Hand_Controller_gesture>();
    }

    void Update()
    {
        if (Input.GetKeyDown(keyToRecord))
        {
            Debug.Log("Record");
            string s = recorderFilePath.Split('\\')[1].Split('.')[0];
            message.text = s+" Gesture Being Record";
            recorder.Reset();
            recorder.state = RecorderState.Recording;
        }
        else if (Input.GetKeyDown(keyToSave))
        {
            Debug.Log("Save");

            recorder.state = RecorderState.Stopped;
            string s = recorderFilePath.Split('\\')[1].Split('.')[0];
            message.text = s + " Gesture Being Save";
            Debug.Log(recorder.SaveToNewFile(recorderFilePath));
            string files = File.ReadAllText(gestureSaveFilePath);
            if (files != null)
                File.WriteAllText(gestureSaveFilePath, files + isPose.ToString() + '-'
                    + recorderFilePath + '-' + handCon.Rotation + '-'
                    + handCon.fingerCount + '-' + handCon.GetDis[0] + '-'
                    + handCon.GetDis[1] + '-' + handCon.GetDis[2] + '-'
                    + handCon.GetDis[3] + '-' + handCon.GetDis[4] + ',');
            //+ handCon.Angle[0] + '-'
            //+ handCon.Angle[1] + '-' + handCon.Angle[2] + '-'
            //+ handCon.Angle[3] + '-' + handCon.Angle[4] + ',');   //将手势的名称，是否是静态，受指数，手掌方向，手指距离存入文件
            else
                File.WriteAllText(gestureSaveFilePath, isPose.ToString() + '-'
                    + recorderFilePath + '-' + handCon.Rotation + '-'
                    + handCon.fingerCount + '-' + handCon.GetDis[0] + '-'
                    + handCon.GetDis[1] + '-' + handCon.GetDis[2] + '-'
                    + handCon.GetDis[3] + '-' + handCon.GetDis[4] + ',');
            //+ handCon.Angle[0] + '-'
            //+ handCon.Angle[1] + '-' + handCon.Angle[2] + '-'
            //+ handCon.Angle[3] + '-' + handCon.Angle[4] + ',');
            // Debug.Log(isPose.ToString() + '-' + recorderFilePath + ',');
        }
        else if (Input.GetKeyDown(keyToReset))
        {
            recorder.Reset();
            string s = recorderFilePath.Split('\\')[1].Split('.')[0];
            message.text = s + " Gesture Being Reset";
            Debug.Log(recorder.SaveToNewFile(recorderFilePath));
            recorder.state = RecorderState.Stopped;
        }
        else if (Input.GetKeyDown(keyToPlay))
        {
            string s = recorderFilePath.Split('\\')[1].Split('.')[0];
            message.text = s + "Gesture is Playing";
            recorder.Play();
        }
        else if (Input.GetKeyDown(keyToPause))
        {
            string s = recorderFilePath.Split('\\')[1].Split('.')[0];
            message.text = s + "Gesture is Pause";
            recorder.state = RecorderState.Paused;
        }
        else if (Input.GetKeyDown(keyToLoad))
        {
            string s = recorderFilePath.Split('\\')[1].Split('.')[0];
            message.text = s + "Gesture is Load";
            recorder.state = RecorderState.Paused;
            recorder.Reset();
            recorder.Load(playerFilePath);
            recorder.Play();
        }
    }
}
