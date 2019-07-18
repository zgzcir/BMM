using Leap;
using Leap.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hand_Controller_gesture : MonoBehaviour
{

    public enum FingerRotation
    {
        left,
        right,
        forword,
        back,
        up,
        down
    }

    private FingerRotation fingerRotation;

    public float[] fingerDis = new float[5];
    public float[] GetDis { get { return fingerDis; } }

    public static bool Gesture_left = false;
    public static bool Gesture_right = false;
    public static bool Gesture_up = false;
    public static bool Gesture_down = false;
    public static bool Gesture_zoom = false;
    //
    public static bool Gesture_forword = false;
    public static bool IsClose = false;
    public static bool IsUp = false;

    public int fingerCount = 0;
    //
    public static float movePOs = 0.0f;

    private LeapProvider mProvider;
    private Frame mFrame;
    private Hand mHand;
    private Vector leftPosition;
    private Vector rightPosition;

    public static float zoom = 1.0f;

    public float[] angle = new float[5];
    public float[] Angle { get { return angle; } }

    public string Rotation
    {
        get
        {
            switch (fingerRotation)
            {
                case FingerRotation.left: return "left";
                case FingerRotation.right: return "right";
                case FingerRotation.back: return "back";
                case FingerRotation.forword: return "forword";
                case FingerRotation.up: return "up";
                case FingerRotation.down: return "down";
            }
            return "";
        }
    }

    private float pitch;
    private float yaw;
    private float roll;


    [Tooltip("Velocity (m/s) of Palm ")]
    public float smallestVelocity = 0.4f;//手掌移动的最小速度， 需要的只要手掌方向，所以需要手静止。

    [Tooltip("Velocity (m/s) of Single Direction ")]
    [Range(0, 1)]
    public float deltaVelocity = 0.7f;//单方向上手掌移动的速度 	

    [Tooltip("Delta degree to check 2 vectors same direction")]  //三角度检查2个向量的方向相同
    protected float handForwardDegree = 60;

    void Start()
    {
        mProvider = FindObjectOfType<LeapProvider>() as LeapProvider;
    }

    // Update is called once per frame	
    void Update()
    {
         mFrame = mProvider.CurrentFrame;//获取当前帧
                                        //获得手的个数		
                                        //print ("hand num are " + mFrame.Hands.Count); 
        if (mFrame.Hands.Count > 0)
        {
            if (mFrame.Hands.Count == 2)
                zoom = CalcuateDistance(mFrame);
            //if (mFrame.Hands.Count == 1)
            {
                LRUDGestures(mFrame, ref movePOs);
                fingerCount = GetFingers(mFrame);
                GetFingerDis(mFrame);
                //Debug.Log(GetFingers(mFrame));
            }
        }
    }

    float CalcuateDistance(Frame mFrame)
    {
        Gesture_zoom = true;
        Gesture_left = false;
        Gesture_right = false;
        float distance = 0f;
        //print ("Two hands");		
        foreach (var itemHands in mFrame.Hands)
        {
            if (itemHands.IsLeft)
            {
                leftPosition = itemHands.PalmPosition;
                //print ("leftPosition" + leftPosition);		
            }
            if (itemHands.IsRight)
            {
                rightPosition = itemHands.PalmPosition;
                //print ("rightPosition" + rightPosition);		
            }
        }
        if (leftPosition != Vector.Zero && rightPosition != Vector.Zero)
        {
            Vector3 leftPos = new Vector3(leftPosition.x, leftPosition.y, leftPosition.z);
            Vector3 rightPos = new Vector3(rightPosition.x, rightPosition.y, rightPosition.z);

            distance = 10 * Vector3.Distance(leftPos, rightPos);
            //print ("distance" + distance);
        }
        if (distance != 0)
            return distance;
        else
            return distance = 1;
    }

    void GetFingerDis(Frame mFrame)
    {
        foreach (var hand in mFrame.Hands)
        {
            fingerDis = new float[5];
            List<Finger> listOfFingers = hand.Fingers;
            fingerDis[0] = (listOfFingers[0].TipPosition - hand.PalmPosition).Magnitude;
            fingerDis[1] = (listOfFingers[1].TipPosition - hand.PalmPosition).Magnitude;
            fingerDis[2] = (listOfFingers[2].TipPosition - hand.PalmPosition).Magnitude;
            fingerDis[3] = (listOfFingers[3].TipPosition - hand.PalmPosition).Magnitude;
            fingerDis[4] = (listOfFingers[4].TipPosition - hand.PalmPosition).Magnitude;
            angle[0] = listOfFingers[0].TipPosition.Magnitude ;
            angle[1] = listOfFingers[1].TipPosition.Magnitude;
            angle[2] = listOfFingers[2].TipPosition.Magnitude;
            angle[3] = listOfFingers[3].TipPosition.Magnitude;
            angle[4] = listOfFingers[4].TipPosition.Magnitude;
        }
    }

    /// <summary>
    /// 的到有几根手指盛开
    /// </summary>
    int GetFingers(Frame mFrame)
    {
        float deltaCloseFinger = 0.08f;
        foreach (var hand in mFrame.Hands)
        {
            List<Finger> listOfFingers = hand.Fingers;
            int count = 0;
            for (int f = 0; f < listOfFingers.Count; f++)
            { //循环遍历所有的手~~
                Finger finger = listOfFingers[f];
                //Debug.Log(listOfFingers.Count);
                //fingerDis[f] =(finger.TipPosition - hand.PalmPosition).Magnitude;
               // Debug.Log((finger.TipPosition - hand.PalmPosition).Magnitude);
                if ((finger.TipPosition - hand.PalmPosition).Magnitude > deltaCloseFinger)    // Magnitude  向量的长度 。是(x*x+y*y+z*z)的平方根。                                                                                                //float deltaCloseFinger = 0.05f;
                {
                    count++;
                    //if (finger.Type == Finger.FingerType.TYPE_THUMB)
                    //Debug.Log ((finger.TipPosition - hand.PalmPosition).Magnitude);
                }
            }
            return count;
        }
        return 0;
    }

    void LRUDGestures(Frame mFrame, ref float movePOs)
    {
        Gesture_zoom = false;
        foreach (var item in mFrame.Hands)
        {
            int numFinger = item.Fingers.Count;
            //print ("item is  " + numFinger); 		

            //print("hand are " + isOpenFullHand (item));	
            // print ("isOpenFullHands is  " + isOpenFullHands(item));  	

            //if (item.GrabStrength == 1)
            //{
            //    print ("num is 0, gestures is woquan");//握拳
            //}
            //else if (item.GrabStrength == 0)
            {
                //由于摄像机原因，模型与unity方向不一致进行调整
                if (isPalmNormalSameDirectionWith(item, Vector3.left))
                {
                    Gesture_left = false;
                    Gesture_right = true;
                    Gesture_forword = false;
                    Gesture_up = false;
                    Gesture_down = false;
                    fingerRotation = FingerRotation.right;
                    // print(" Right");
                }
                else if (isPalmNormalSameDirectionWith(item, Vector3.right))
                {
                    Gesture_left = true;
                    Gesture_right = false;
                    Gesture_forword = false;
                    Gesture_up = false;
                    Gesture_down = false;
                    fingerRotation = FingerRotation.left;
                    // print("left");
                }
                else if (isPalmNormalSameDirectionWith(item, Vector3.up))
                {
                    Gesture_left = false;
                    Gesture_right = false;
                    Gesture_forword = true;
                    Gesture_up = false;
                    Gesture_down = false;
                    fingerRotation = FingerRotation.forword;
                    // print(" Forward");
                }
                else if (isPalmNormalSameDirectionWith(item, Vector3.down))
                {
                    Gesture_left = false;
                    Gesture_right = false;
                    Gesture_forword = false;
                    Gesture_up = false;
                    Gesture_down = false;
                    fingerRotation = FingerRotation.back;
                    //  print(" back");
                }
                else if (isPalmNormalSameDirectionWith(item, Vector3.forward))
                {
                    Gesture_left = false;
                    Gesture_right = false;
                    Gesture_forword = false;
                    Gesture_up = true;
                    Gesture_down = false;
                    fingerRotation = FingerRotation.up;
                    //   print(" Up");
                }
                else if (isPalmNormalSameDirectionWith(item, Vector3.back))
                {
                    Gesture_left = false;
                    Gesture_right = false;
                    Gesture_forword = false;
                    Gesture_up = false;
                    Gesture_down = true;
                    fingerRotation = FingerRotation.down;
                    // print(" Down");
                }
            }
        }
    }


    private bool isStone(Hand hand)
    {
        //print ("hand.GrabAngle" + hand.GrabAngle);	
        return hand.GrabAngle > 2.0f;
    }

    //是否抓取	
    public bool isGrabHand(Hand hand)
    {
        return hand.GrabStrength > 0.8f;
        //抓取力 	
    }

    //hand move four direction
    public bool isMoveRight(Hand hand)
    {
        return hand.PalmVelocity.x > deltaVelocity && !isStationary(hand);
    }

    // 手划向右边	
    public bool isMoveLeft(Hand hand)
    {
        //print (hand.PalmVelocity.x );		
        return hand.PalmVelocity.x < -deltaVelocity && !isStationary(hand);
    }

    //手向上 	
    public bool isMoveUp(Hand hand)
    {
        //print ("hand.PalmVelocity.y" + hand.PalmVelocity.y); 		
        return hand.PalmVelocity.y > deltaVelocity && !isStationary(hand);
    }

    //手向下  	
    public bool isMoveDown(Hand hand)
    {
        return hand.PalmVelocity.y < -deltaVelocity && !isStationary(hand);
    }

    //手向前	
    public bool isMoveForward(Hand hand)
    {
        //print (hand.PalmVelocity.z);	
        return hand.PalmVelocity.z > deltaVelocity && !isStationary(hand);
    }

    //手向后 
    public bool isMoveBack(Hand hand)
    {
        return hand.PalmVelocity.z < -deltaVelocity && !isStationary(hand);
    }   //固定不动的	

    public bool isStationary(Hand hand)  //判断是否静止
    {
        return hand.PalmVelocity.Magnitude < smallestVelocity;
        //Vector3.Magnitude返回向量的长度
    }

    protected bool isCloseHand(Hand hand)     //是否握拳 
    {
        float deltaCloseFinger = 0.07f;
        List<Finger> listOfFingers = hand.Fingers;
        int count = 0;
        for (int f = 0; f < listOfFingers.Count; f++)
        { //循环遍历所有的手~~
            Finger finger = listOfFingers[f];
            if ((finger.TipPosition - hand.PalmPosition).Magnitude < deltaCloseFinger)    // Magnitude  向量的长度 。是(x*x+y*y+z*z)的平方根。
                                                                                          //float deltaCloseFinger = 0.05f;
            {
                count++;
                //if (finger.Type == Finger.FingerType.TYPE_THUMB)
                //Debug.Log ((finger.TipPosition - hand.PalmPosition).Magnitude);
            }
        }
        return (count == 5);
    }

    protected float angle2LeapVectors(Leap.Vector a, Leap.Vector b)
    {
        //向量转化成 角度
        return Vector3.Angle(UnityVectorExtension.ToVector3(a), UnityVectorExtension.ToVector3(b));
    }

    protected bool isHandMoveForward(Hand hand)//向手掌方向移动
    {
        return isSameDirection(hand.PalmNormal, hand.PalmVelocity) && !isStationary(hand);
    }

    protected bool isSameDirection(Vector a, Vector b)
    {
        //判断两个向量是否 相同 方向
        //Debug.Log (angle2LeapVectors (a, b) + " " + b);
        return angle2LeapVectors(a, b) < handForwardDegree;
    }

    protected bool isPalmNormalSameDirectionWith(Hand hand, Vector3 dir)
    {
        //判断手的掌心方向于一个  向量   是否方向相同 
        return isSameDirection(hand.PalmNormal, UnityVectorExtension.ToVector(dir));
    }
}

