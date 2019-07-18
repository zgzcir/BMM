using UnityEngine;
using Leap;
using Leap.Unity;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * Now we get to defining the base LeapTrainer Controller.  This class contains the default implementations of all functions.
 * 
 * The constructor accepts an options parameter, which is then passed to the initialize in order to set up the object.
 * 
 */
public class LeapTrainer : MonoBehaviour
{

    public LeapServiceProvider serviceProvider;

    //public CreateText createText;

    /**
	 * Events Adapted to C#
	 */

    public delegate void StartedRecordingDelegate();
    public delegate void EndedRecordingDelegate();
    public delegate void GestureDetectedDelegate(List<Point> points, int frameCount);
    public delegate void GestureCreatedDelegate(string name, bool trainingSkipped);
    public delegate void GestureRecognizedDelegate(string name, float value, Dictionary<string, float> allHits);
    public delegate void GestureUnknownDelegate(Dictionary<string, float> allHits);
    public delegate void TrainingCountdownDelegate(int countdown);
    public delegate void TrainingStartedDelegate(string name);
    public delegate void TrainingCompleteDelegate(string name, List<List<Point>> gestures, bool isPose);
    public delegate void TrainingGestureSavedDelegate(string name, List<List<Point>> gestures);

    public event StartedRecordingDelegate OnStartedRecording;
    public event EndedRecordingDelegate OnEndedRecording;
    public event GestureDetectedDelegate OnGestureDetected;
    public event GestureCreatedDelegate OnGestureCreated;
    public event GestureRecognizedDelegate OnGestureRecognized;
    public event GestureUnknownDelegate OnGestureUnknown;
    public event TrainingCountdownDelegate OnTrainingCountdown;
    public event TrainingStartedDelegate OnTrainingStarted;
    public event TrainingCompleteDelegate OnTrainingComplete;
    public event TrainingGestureSavedDelegate OnTrainingGestureSaved;

    /**
	 * Attribs
	 */

    private TemplateMatcher templateMatcher;

    private Controller controller = null;   // An instance of Leap.Controller from the leap.js library.  This will be created if not passed as an option
    private TrainerListener listener = null;

    //bool pauseOnWindowBlur = false; // If this is TRUE, then recording and recognition are paused when the window loses the focus, and restarted when it's regained

    public float minRecordingVelocity = 300f; // The minimum velocity a frame needs to clock in at to trigger gesture recording, or below to stop gesture recording (by default)
    public float maxRecordingVelocity = 30f;  // The maximum velocity a frame can measure at and still trigger pose recording, or above which to stop pose recording (by default)

    public int minGestureFrames = 5;    // The minimum number of recorded frames considered as possibly containing a recognisable gesture 
    public int minPoseFrames = 75;      // The minimum number of frames that need to hit as recordable before pose recording is actually triggered
    public int frameCount = 0;      // The actual frame count	

    int recordedPoseFrames = 0; // A counter for recording how many pose frames have been recorded before triggering
    bool recordingPose = false; // A flag to indicate if a pose is currently being recorded
    bool recording = false;     // Variable to know if there is recording a gesture

    public float hitThreshold = 0.65f;  // The correlation output value above which a gesture is considered recognized. Raise this to make matching more strict

    int trainingCountdown = 3;  // The number of seconds after startTraining is called that training begins. This number of 'training-countdown' events will be emit.
    int trainingGestures = 1;   // The number of gestures samples that collected during training
    int convolutionFactor = 0;  // The factor by which training samples will be convolved over a gaussian distribution to expand the available training data

    public float downtime = 1000f; // The number of milliseconds after a gesture is identified before another gesture recording cycle can begin
    public float lastHit = 0;    // The timestamp at which the last gesture was identified (recognized or not), used when calculating downtime

    private Dictionary<string, List<List<Point>>> gestures = new Dictionary<string, List<List<Point>>>(); // The current set of recorded gestures - names mapped to convolved training data
    public Dictionary<string, List<List<Point>>> Gestures { get { return gestures; } set { gestures = value; } }
    private Dictionary<string, bool> poses = new Dictionary<string, bool>();      // Though all gesture data is stored in the gestures object, here we hold flags indicating which gestures were recorded as poses
    public Dictionary<string, bool> Poses { get { return poses; } set { poses = value; } }
    private List<Point> gesture = null;                           // Actual recording gesture

    private Dictionary<string, string> FingerRotDic = new Dictionary<string, string>();  //设置存储的手势与方向对应
    public Dictionary<string, string> fingerRotDic { get { return FingerRotDic; } set { FingerRotDic = value; } }
    private Dictionary<string, int> FingerCountDic = new Dictionary<string, int>();  //设置存储的手势与手指伸开数对应
    public Dictionary<string, int> fingerCountDic { get { return FingerCountDic; } set { FingerCountDic = value; } }
    private Dictionary<string, float[]> fingerDisDic = new Dictionary<string, float[]>();    //设置存储的手势到首长的距离
    public Dictionary<string, float[]> FingerDisDic { get { return fingerDisDic; } set { fingerDisDic = value; } }
    private Dictionary<string, float[]> fingerAngle = new Dictionary<string, float[]>();
    public Dictionary<string, float[]> FingerAngle { get { return fingerAngle; } set { fingerAngle = value; } }

    public Hand_Controller_gesture handController;

    string trainingGesture = ""; // The name of the gesture currently being trained, or null if training is not active
                                 //listeners				= {};	// Listeners registered to receive events emit from the trainer - event names mapped to arrays of listener functions

    public bool paused = false; // This variable is set by the pause() method and unset by the resume() method - when true it disables frame monitoring temporarily.

    void Awake()
    {
        this.templateMatcher = new GeometricalMatcher();
    }

    void Start()
    {
        this.controller = serviceProvider.GetLeapController();

        this.bindFrameListener();

    }

    private float time;
    void Update()
    {
        time = Time.time;
    }

    private class TrainerListener
    {

        private LeapTrainer lt;
        private Controller c;

        public TrainerListener(LeapTrainer lt, Controller c)
        {
            this.lt = lt;
            this.c = c;
            c.FrameReady += OnFrame;
        }

        public void Release()
        {
            c.FrameReady += OnFrame;
        }

        public void OnFrame(object sender, FrameEventArgs e)
        {
            lt.onFrame(e.frame);
        }

    }

    private void onFrame(Frame frame)
    {
        if (this.paused) { return; }
        // Debug.Log(this.time - this.lastHit);
        if (this.time - this.lastHit < this.downtime) { return; }

        if (this.recordableFrame(frame, this.minRecordingVelocity, this.maxRecordingVelocity))
        {

            if (!recording)
            {

                recording = true;
                frameCount = 0;
                gesture = new List<Point>();
                this.recordedPoseFrames = 0;

                if (OnStartedRecording != null) OnStartedRecording();
            }

            frameCount++;

            this.recordFrame(frame, this.controller.Frame());

        }
        else if (recording)
        {

            recording = false;

            if (OnEndedRecording != null) OnEndedRecording();

            if (this.recordingPose || frameCount >= this.minGestureFrames)
            {
                if (OnGestureDetected != null) OnGestureDetected(gesture, frameCount);

                if (trainingGesture != "")
                {
                    //this.saveTrainingGesture(trainingGesture, gesture, this.recordingPose);
                }
                else
                {
                    try
                    {
                        this.recognize(gesture, frameCount, handController.Rotation, handController.fingerCount, handController.GetDis,handController.Angle);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message + " : " + e.StackTrace);
                    }
                }

                this.lastHit = this.time;
            }
        }
    }

    private void recordVector(Vector v)
    {
        this.gesture.Add(new Point(v.x, v.y, v.z, 0));
    }

    private void bindFrameListener()
    {
        listener = new TrainerListener(this, controller);
    }

    private bool recordableFrame(Frame frame, float min, float max)
    {

        float palmVelocity, tipVelocity;
        bool poseRecordable = false;

        foreach (var hand in frame.Hands)
        {

            palmVelocity = hand.PalmVelocity.Magnitude;

            if (palmVelocity >= min) { return true; }
            if (palmVelocity <= max) { poseRecordable = true; break; }

            foreach (var finger in hand.Fingers)
            {

                tipVelocity = finger.TipVelocity.Magnitude;

                if (tipVelocity >= min) { return true; }
                if (tipVelocity <= max) { poseRecordable = true; break; }
            }
        }

        if (poseRecordable)
        {

            this.recordedPoseFrames++;

            if (this.recordedPoseFrames >= this.minPoseFrames)
            {

                this.recordingPose = true;
                return true;
            }

        }
        else
        {
            this.recordedPoseFrames = 0;
        }

        return false;
    }

    private bool IsGoodFrame(Frame frame)
    {
        return frame.Hands.TrueForAll(h => IsGoodVector(h.StabilizedPalmPosition) && h.Fingers.TrueForAll(f => IsGoodVector(f.StabilizedTipPosition)));
    }

    private bool IsGoodVector(Leap.Vector v)
    {
        return !float.IsInfinity(Point.Distance(new Point(), new Point(v.x, v.y, v.z, 0)));
    }

    private void recordFrame(Frame frame, Frame lastFrame)
    {
        if (!IsGoodFrame(frame))
        {
            //Debug.Log("Frame discarded...");
            return;
        }

        foreach (var hand in frame.Hands)
        {

            recordVector(hand.PalmPosition);

            foreach (var finger in hand.Fingers)
            {
                recordVector(finger.TipPosition);
            }
        }
    }

    public void Create(string gestureName, bool skipTraining)
    {

        this.gestures.Add(gestureName, new List<List<Point>>());

        if (OnGestureCreated != null)
            OnGestureCreated(gestureName, skipTraining);

        if (!skipTraining)
        {
            StartCoroutine(StartTraining(gestureName, this.trainingCountdown));
        }
    }

    IEnumerator StartTraining(string gestureName, int countdown)
    {

        if (OnTrainingCountdown != null)
            OnTrainingCountdown(countdown);

        this.pause();

        yield return new WaitForSeconds(countdown);

        this.trainingGesture = gestureName;
        StartCoroutine(StartTraining(gestureName, countdown));

        this.resume();

        if (OnTrainingStarted != null)
            OnTrainingStarted(gestureName);

    }

    public bool Retrain(string gestureName)
    {

        if (this.gestures.ContainsKey(gestureName))
        {

            this.gestures[gestureName].Clear();
            StartCoroutine(StartTraining(gestureName, this.trainingCountdown));

            return true;
        }

        return false;
    }

    void trainAlgorithm(string gestureName, List<List<Point>> trainingGestures)
    {

        for (int i = 0, l = trainingGestures.Count; i < l; i++)
        {
            trainingGestures[i] = this.templateMatcher.process(trainingGestures[i]);
        }
    }

    public void loadFromFrames(string gestureName, List<Frame> frames, bool isPose, string rotation, int fingerCount, float[] fingerDis)// float [] angle)
    {
        List<Point> bcgesture = gesture;
        gesture = new List<Point>();

        Frame lf = null;
        foreach (var f in frames)
        {
            if (lf == null) lf = f;
            else this.recordFrame(f, lf);
        }

        saveTrainingGesture(gestureName, gesture, isPose, rotation, fingerCount, fingerDis);//,angle);

        gesture = bcgesture;
    }

    void saveTrainingGesture(string gestureName, List<Point> gesture, bool isPose, string rotation, int fingerCount, float[] fingerDis)//,float []angle)
    {

        List<List<Point>> trainingGestures = null;

        if (!this.gestures.TryGetValue(gestureName, out trainingGestures))
        {
            trainingGestures = new List<List<Point>>();
        }

        trainingGestures.Add(gesture);

        if (trainingGestures.Count == this.trainingGestures)
        {
            this.gestures[gestureName] = this.distribute(trainingGestures);

            this.FingerRotDic[gestureName] = rotation;    //将文件中的手势与方向对应起来
            //Debug.Log(rotation);
            this.FingerCountDic[gestureName] = fingerCount;//将文件中的手势与手指伸开数对应起来

            //this.fingerAngle[gestureName] = angle;

            this.fingerDisDic[gestureName] = fingerDis;   //将文件中的手势与距离对应
                                                          //Debug.Log(fingerDis[1]);
                                                          // Debug.Log(fingerDisDic[gestureName][1]);
            this.poses[gestureName] = isPose;

            this.trainingGesture = "";

            this.trainAlgorithm(gestureName, trainingGestures);

            if (OnTrainingComplete != null)
                OnTrainingComplete(gestureName, trainingGestures, isPose);

        }
        else
        {
            if (OnTrainingGestureSaved != null)
                OnTrainingGestureSaved(gestureName, trainingGestures);
        }
    }

    private float distributeAux(float f)
    {
        return Mathf.Round((UnityEngine.Random.Range(0, 1) * 2f - 1f) +
                            (UnityEngine.Random.Range(0, 1) * 2f - 1f) +
                            (UnityEngine.Random.Range(0, 1) * 2f - 1f) *
                           ((f * 10000f) / 50f) + (f * 10000f)) / 10000f;
    }

    List<List<Point>> distribute(List<List<Point>> trainingGestures)
    {

        var factor = this.convolutionFactor;

        if (factor == 0) { return trainingGestures; }

        List<Point> generatedGesture;

        for (int i = 0, p = factor; i < p; i++)
        {
            foreach (var gesture in trainingGestures)
            {

                generatedGesture = new List<Point>();

                foreach (var point in gesture)
                {
                    float x = distributeAux(point.x);
                    float y = distributeAux(point.y);
                    float z = distributeAux(point.z);

                    generatedGesture.Add(new Point(x, y, z, point.stroke));
                }

                trainingGestures.Add(generatedGesture);
            }
        }
        return trainingGestures;
    }

    void recognize(List<Point> gesture, int frameCount, string rotation, int fingers, float[] fingerDis,float []angle)
    {

        var threshold = this.hitThreshold;
        Dictionary<string, float> allHits = new Dictionary<string, float>();
        float hit = 0;
        float bestHit = 0;
        bool recognized = false;
        string closestGestureName = null;
        bool recognizingPose = (frameCount <= 5); //Single-frame recordings are idenfied as poses
        //Debug.Log(frameCount);
        foreach (var knownGesture in gestures)
        {
            if (this.poses[knownGesture.Key] != recognizingPose)
            {
                hit = 0.0f;
            }
            else
            {
                int a = 5;
                if (this.poses[knownGesture.Key])    //表示该手势为静态，要进行距离判断
                {
                    //Debug.Log(fingerDisDic[knownGesture.Key][2]);
                    a = 0;
                    for (int i = 0; i < 5; i++)
                        if (fingerDis[i] < fingerDisDic[knownGesture.Key][i] + 0.04f
                                && fingerDis[i] > fingerDisDic[knownGesture.Key][i] - 0.04f)
                                    //&& angle[i] < fingerAngle[knownGesture.Key][i] + 0.15f
                                    //     && angle[i] > fingerAngle[knownGesture.Key][i] - 0.15f)
                            a++;
                    //Debug.Log(a);
                    if (a >= 4 && this.FingerRotDic[knownGesture.Key] == rotation
                && this.FingerCountDic[knownGesture.Key] == fingers)
                        hit = 1;// (Mathf.Min(Mathf.Max(100f * Mathf.Max(a - 4f) / -4f, 0f), 100f) / 100f);
                    else
                        hit = 0;
                }
                else
                    hit = this.correlate(knownGesture.Key, knownGesture.Value, gesture, poses[knownGesture.Key]);
                //Debug.Log(knownGesture.Key+hit);
            }

            allHits.Add(knownGesture.Key, hit);

            if (hit >= threshold)
            {
                recognized = true;
                //if (this.poses[knownGesture.Key])
                //    if (OnGestureRecognized != null)
                //        OnGestureRecognized(knownGesture.Key, hit, allHits);
            }

            if (hit > bestHit)
            {
                bestHit = hit;
                closestGestureName = knownGesture.Key;
            }
        }

        if (recognized)
        {
            //if (!poses[closestGestureName])
                if (OnGestureRecognized != null)
                    OnGestureRecognized(closestGestureName, bestHit, allHits);
        }
        else
        {
            if (OnGestureUnknown != null)
                OnGestureUnknown(allHits);
        }
    }

    float correlate(string gestureName, List<List<Point>> trainingGestures, List<Point> gesture, bool isPose)
    {

        List<Point> parsedGesture = this.templateMatcher.process(gesture);

        float nearest = float.PositiveInfinity, distance;
        bool foundMatch = false;

        foreach (var toMatch in trainingGestures)
        {

            distance = this.templateMatcher.match(parsedGesture, toMatch);

            if (distance < nearest)
            {
                nearest = distance;
                foundMatch = true;
            }
        }
        // Values: 3-6 are accepted
        // Values above 6 are rejected		
        //Debug.Log(nearest);
        if (isPose)
            return (!foundMatch) ? 0f : (Mathf.Min(Mathf.Max(100f * Mathf.Max(nearest - 4f) / -4f, 0f), 100f) / 100f);
        else
            return (!foundMatch) ? 0f : (Mathf.Min(Mathf.Max(100f * Mathf.Max(nearest - 4f) / -4f, 0f), 100f) / 100f);
            //return (!foundMatch) ? 0f : Mathf.Max(3f - Mathf.Max(nearest - 3f, 0f), 0f) / 3f;
    }

    string getRecordingTriggerStrategy() { return "Frame velocity"; }

    string getFrameRecordingStrategy() { return "3D Geometric Positioning"; }

    string getRecognitionStrategy() { return "Geometric Template Matching"; }

    //string toJSON(string gestureName)
    //{

    //    var gestures = this.gestures[gestureName];


    //    SimpleJSON.JSONNode json = new SimpleJSON.JSONNode(),
    //                        name = new SimpleJSON.JSONNode(),
    //                        pose = new SimpleJSON.JSONNode(),
    //                        data = new SimpleJSON.JSONNode(),
    //                        x, y, z, stroke;

    //    foreach (var gesture in gestures)
    //    {

    //        SimpleJSON.JSONNode gesturePoints = new SimpleJSON.JSONNode();

    //        foreach (var point in gesture)
    //        {
    //            SimpleJSON.JSONNode pointComponents = new SimpleJSON.JSONNode();

    //            x = new SimpleJSON.JSONNode(); x.AsFloat = point.x;
    //            y = new SimpleJSON.JSONNode(); y.AsFloat = point.y;
    //            z = new SimpleJSON.JSONNode(); z.AsFloat = point.z;
    //            stroke = new SimpleJSON.JSONNode(); stroke.AsFloat = point.stroke;

    //            pointComponents.Add("x", x);
    //            pointComponents.Add("y", y);
    //            pointComponents.Add("z", z);
    //            pointComponents.Add("stroke", stroke);

    //            gesturePoints.Add(pointComponents);
    //        }

    //        data.Add(gesturePoints);
    //    }


    //    name.Value = gestureName;
    //    pose.AsBool = this.poses[gestureName];

    //    json.Add("name", name);
    //    json.Add("pose", pose);
    //    json.Add("data", data);

    //    return json.ToString();
    //}

    //Dictionary<string, object> fromJSON(string json)
    //{
    //    var jo = SimpleJSON.JSON.Parse(json);

    //    List<List<Point>> gesture = new List<List<Point>>();

    //    string gestureName = jo["name"].Value;
    //    foreach (var g in jo["data"].Childs)
    //    {
    //        List<Point> subGesture = new List<Point>();

    //        foreach (var p in g.Childs)
    //            subGesture.Add(new Point(p["x"].AsFloat, p["y"].AsFloat, p["z"].AsFloat, p["stroke"].AsInt));

    //        gesture.Add(subGesture);
    //    }

    //    this.Create(gestureName, true);
    //    this.gestures.Add(gestureName, gesture);
    //    this.poses.Add(gestureName, jo["pose"].AsBool);

    //    Dictionary<string, object> parsed = new Dictionary<string, object>();

    //    parsed["name"] = gestureName;
    //    parsed["pose"] = poses[gestureName];
    //    parsed["data"] = gesture;

    //    return parsed;
    //}

    LeapTrainer pause()
    {
        this.paused = true;
        return this;
    }

    LeapTrainer resume()
    {
        this.paused = false;
        return this;
    }

    void OnDestroy()
    {
        this.listener.Release();
    }

    public void Clean()
    {
        this.gestures.Clear();
        this.poses.Clear();
    }
}
