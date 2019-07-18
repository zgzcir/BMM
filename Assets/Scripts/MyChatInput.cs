//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[System.Serializable]
//public class MyChatInput : MonoBehaviour {

//    public UIInput input;           
//    public UITextList textList ;

//    public UIScrollBar ScrollBar;

//    public GameObject Touch;
//    public List <Transform>  TouchBoxs=new List<Transform>() ;//改变位置
//    public List<Vector3 > Begins = new List<Vector3 >();  //存储最开始的位置

//    public Transform Sprite;

//    public string name;
//    private float  SumLabelLen = 300;


//	// Use this for initialization
//	void Start () {
//	}
	
//	// Update is called once per frame
//	void Update () {
//        SumLabelLen = Sprite.GetComponent <UISprite >().height   /ScrollBar.barSize;//计算Text的高度
//        //Debug.Log(SumLabelLen);
//        foreach (Transform item in TouchBoxs)//遍历，使得文字对应相应图标
//        {
//            if (item.transform.localPosition.y > 140|| item.transform.localPosition.y<-130)//在text范围外，使看不见
//            {
//                item.gameObject.SetActive(false);
//            }
//            else
//            {
//                item.gameObject.SetActive(true);
//            }
//        }
//        for (int i = 0; i < TouchBoxs.Count; i++)
//        {
//            int a = Mathf.CeilToInt ((1 - ScrollBar.value) * (SumLabelLen-300) ) ;
//            TouchBoxs[i].transform.localPosition =
//                Begins[i] - new Vector3(0, a, 0);
//        }
//	}
//    /// <summary>
//    ///输入回车进行提交
//    /// </summary>
//    public void OnChatSubmit()
//    {
//        string chatMessage = input.value;
//        textList.Add(name+" : "+chatMessage);
//        input.value = "";
//        ScrollBar.value = 1;
//        int len = chatMessage.Length;
//        if(TouchBoxs.Count!=0)
//        {
//            foreach (Transform  item in TouchBoxs )
//            {
//                if (len >= 0 && len < 14)
//                {
//                    item.localPosition += new Vector3(0, 30, 0);
//                }
//                else if (len < 20)
//                {
//                    item.localPosition += new Vector3(0, 60, 0);
//                }
//                else if (len < 39)
//                {
//                    item.localPosition += new Vector3(0, 90, 0);
//                }
//                else
//                {
//                    item.localPosition += new Vector3(0, 120, 0);
//                }
//            }
//        }     //移动图标位置
//        for (int i = 0; i < Begins.Count; i++)
//        {
//            if (len >= 0 && len < 14)
//            {
//                Begins[i] += new Vector3(0, 30, 0);
//            }
//            else if (len < 20)
//            {
//                Begins[i] += new Vector3(0, 60, 0);
//            }
//            else if (len < 39)
//            {
//                Begins[i] += new Vector3(0, 90, 0);
//            }
//            else
//            {
//                Begins[i] += new Vector3(0, 120, 0);
//            }
//        }  //给begins赋值
//            GameObject go= Instantiate(Touch, new Vector3(266, -130, 0), Quaternion.identity )as GameObject ;
//        go.GetComponentInChildren<UILabel>().text = chatMessage;
//        go.transform.parent = Sprite.transform;
//        go.transform.localPosition  = new Vector3(266, -130, 0);
//        go.transform.localScale  = new Vector3(1, 1, 1);
//        TouchBoxs.Add(go.transform);
//        Begins.Add(go.transform.localPosition);
//    }
//}
