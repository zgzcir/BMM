using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Leap;
using System;
using System.IO;
using System.Text;

public class RecordLoader : MonoBehaviour
{

    public string AnimiFile;
    //public string recorderFilePath;
    // public string recorderFilePath2;
    public LeapTrainer trainer;

    private LeapRecorder loader;

    // Use this for initialization
    void Start()
    {
        //loader = new LeapRecorder();
        //loader.Load(recorderFilePath);
        /////*var original = ;
        ////var clean = CleanFrames(original);
        ////Debug.Log("Original: " + original.Count + " Cleaned: " + clean.Count);*/

        //trainer.loadFromFrames(recorderFilePath, loader.GetFrames(), false);
        //loader = new LeapRecorder();
        //loader.Load(recorderFilePath2);
        //trainer.loadFromFrames(recorderFilePath2, loader.GetFrames(), false);

        string[] file = File.ReadAllText(AnimiFile).Split(',');   //读取手势名字.json

        foreach (string s in file)    //将已知手势全部存入字典  朝向手指伸开数
        {
            bool isPose;
            if (s.Split('-')[0] == "True")
            {
                isPose = true;
            }
            else
            {
                isPose = false;
            }
            if (s == "") continue;
            // Debug.Log(s);
            float[] dis = new float[5];
            //float[] angle = new float[5];
            for (int i = 0; i < 5; i++)
              //  if(i<5)
                    dis[i] = (float)Convert.ToDouble(s.Split('-')[i + 4]);
                    //else
                    //    angle[i-5] = (float)Convert.ToDouble(s.Split('-')[i + 4]);

            // Debug.Log(dis[2]);
            loader = new LeapRecorder();
            loader.Load(s.Split('-')[1]);
            trainer.loadFromFrames(s.Split('-')[1], loader.GetFrames(), isPose, s.Split('-')[2], Convert.ToInt32(s.Split('-')[3]), dis );// angle);  //false表示动作，true表示手势
        }
    }

    public void load()
    {
        trainer.FingerAngle.Clear();
        trainer.fingerCountDic.Clear();
        trainer.FingerDisDic.Clear();
        trainer.fingerRotDic.Clear();
        trainer.Gestures.Clear();
        trainer.Poses.Clear();

        string[] file = File.ReadAllText(AnimiFile).Split(',');   //读取手势名字.json

        foreach (string s in file)    //将已知手势全部存入字典  朝向手指伸开数
        {
            bool isPose;
            if (s.Split('-')[0] == "True")
            {
                isPose = true;
            }
            else
            {
                isPose = false;
            }
            if (s == "") continue;
            // Debug.Log(s);
            float[] dis = new float[5];
            //float[] angle = new float[5];
            for (int i = 0; i < 10; i++)
                //if (i < 5)
                    dis[i] = (float)Convert.ToDouble(s.Split('-')[i + 4]);
                //else
                //    angle[i - 5] = (float)Convert.ToDouble(s.Split('-')[i + 4]);

            // Debug.Log(dis[2]);
            loader = new LeapRecorder();
            loader.Load(s.Split('-')[1]);
            trainer.loadFromFrames(s.Split('-')[1], loader.GetFrames(), isPose, s.Split('-')[2], Convert.ToInt32(s.Split('-')[3]), dis);//, angle);  //false表示动作，true表示手势
        }
    }

    private List<Frame> CleanFrames(List<Frame> toClean)
    {
        var sum = toClean.Sum(f => FrameSum(f));
        Debug.Log("Sum: " + sum);
        return toClean.FindAll(f => IsGoodFrame(f));
    }

    private bool IsGoodFrame(Frame frame)
    {
        return frame.Hands.TrueForAll(h => IsGoodVector(h.StabilizedPalmPosition) && h.Fingers.TrueForAll(f => IsGoodVector(f.StabilizedTipPosition)));
    }

    private bool IsGoodVector(Leap.Vector v)
    {
        return !float.IsInfinity(Point.Distance(new Point(), new Point(v.x, v.y, v.z, 0)));
    }

    public float FrameAverage(Frame frame)
    {
        return frame.Hands.Average(h => HandAverage(h));
    }

    public float HandAverage(Hand hand)
    {
        return hand.Fingers.ConvertAll(f1 => f1.StabilizedTipPosition).Average(f => f.Magnitude);
    }

    public float FrameSum(Frame frame)
    {
        return frame.Hands.Sum(h => HandSum(h));
    }

    public float HandSum(Hand hand)
    {
        Point prev = new Point(hand.StabilizedPalmPosition.x, hand.StabilizedPalmPosition.y, hand.StabilizedPalmPosition.z, 0);
        //bool first = true;
        //Point prev = new Point();
        float distance = 0f;
        hand.Fingers.ConvertAll(f => f.StabilizedTipPosition).ForEach(f => {
            Point p = new Point(f.x, f.y, f.z, 0);
            // if (first) first = false;
            distance += Point.Distance(p, prev);
            if (float.IsInfinity(distance))
                return;
            prev = p;
        });
        return distance;
    }

}
