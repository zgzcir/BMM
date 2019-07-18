using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class Manager : MonoBehaviour
{

    private Animator animator;

    private GameObject iteator;

    public GameObject[] Touchs;

    private const string _path = "/GestureKeys.json";
    private const string _path1 = "Prefabs/";
    private bool isPlay = false;
    public  bool IsPlay { get { return isPlay; } }

    public Dictionary<string, string> dictGes;
    public Queue<GameObject> queue;

    private Canvas canvas;

    //public  int sum = 1;//控制动画播放数量

    // Use this for initialization
    void Start()
    {
        ReadJson();
        queue = new Queue<GameObject>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private void Update()//保证场景只有一个动画播放
    {
        //if (isPlay == false)
        //    this.gameObject.tag = "Button";
        //Touchs = GameObject.FindGameObjectsWithTag("Button");
        //GameObject onPlay = GameObject.FindGameObjectWithTag("OnPlay");
        //if (isPlay == true)
        //    foreach (GameObject item in Touchs)
        //    {
        //        item.SetActive(false);
        //    }
        //if (isPlay == false)
        //    foreach (GameObject item in Touchs)
        //    {
        //        item.SetActive(true);
        //    }
    }



    /// <summary>
    /// 队列处理
    /// </summary>
    /// <param name="strs"></param>
    public void ProcessQueue(char[] strs)
    {
        for (int i = 0; i < strs.Length; i++)
        {
            if (i < strs.Length - 2)
            {
                var s = strs[i].ToString() + strs[i + 1].ToString() + strs[i + 2].ToString();
                if (dictGes.ContainsKey(s))
                {
                    EnQueue(dictGes[s]);
                    i += 2;
                    continue;
                }
            }
            if (i < strs.Length - 1)
            {
                var s = strs[i].ToString() + strs[i + 1].ToString();
                if (strs[i].ToString() != strs[i + 1].ToString())  //叠词只读取一个字
                {
                    if (dictGes.ContainsKey(s))
                    {
                        i += 1;
                        EnQueue(dictGes[s]);
                        continue;
                    }
                }
                else
                {
                    if (dictGes.ContainsKey(strs[i].ToString()))
                    {
                        i += 1;
                        EnQueue(dictGes[strs[i].ToString()]);
                        continue;
                    }
                }

            }
            if (dictGes.ContainsKey(strs[i].ToString()))
            {
                EnQueue(dictGes[strs[i].ToString()]);
            }
        }
        StartCoroutine("PlayAnni");
    }

    /// <summary>
    /// 进队列
    /// </summary>
    /// <param name="str"></param>
    public void EnQueue(string str)
    {

        GameObject game = Resources.Load(_path1 + str) as GameObject;
        queue.Enqueue(game);
    }

    /// <summary>
    /// 出队列
    /// </summary>
    public void DeQueue()
    {
        iteator = Instantiate(queue.Dequeue()) as GameObject;
        animator = iteator.GetComponent<Animator>();
    }

    GameObject[] games = new GameObject[20];
    Animator[] animators = new Animator[20];

    /// <summary>
    /// 多线程进行协程
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAnni()
    {
       // Debug.Log(queue.Count);
        int i = 0,j=0;
        if(queue!=null)
        for (i = queue.Count-1; i >=0; i--)
        {
            GameObject game= Instantiate(queue.Dequeue()) as GameObject;
            game.transform.localScale = new Vector3(0, 0, 0);
            games[i] = game;
            game.GetComponent<Animator>().enabled=false;
            animators[i]= game.GetComponent<Animator>();
                j++;
        }
        //int j = -1;
        //j++;
        while (j!=0 || isPlay)
        {
            if (isPlay == false)
            {
                j--;
#if UNITY_ANDROID
                games[j].transform.localScale = new Vector3(50, 50, 50);
                games[j].transform.localPosition = new Vector3(-6.5f,0,0);
#endif

#if UNITY_IPHONE
                games[j].transform.localScale = new Vector3(50, 50, 50);
                games[j].transform.localPosition = new Vector3(-6.5f,0,0);
#endif

#if UNITY_STANDALONE_WIN
                games[j].transform.localScale = new Vector3(100, 100, 100);
#endif
                animators[j].enabled=true;
                //DeQueue();
                isPlay = true;
            }
            else
            {
                JudgeAnimator(games[j],animators[j],j);

            }
            yield return null;
        }
        //canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        yield return null;
    }

    /// <summary>
    /// 判断动画是否完毕
    /// </summary>
    /// <returns></returns>
    public void JudgeAnimator(GameObject iteator,Animator animator,int j)
    {
        var info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime > 0.5f)
        {
            if(j-1>=0)
            animators[j -1].enabled = true;
        }
        if (info.normalizedTime > 1f)
        {
            isPlay = false;
            Destroy(iteator);
        }
    }

    [System.Serializable]
    class GestureJson
    {
        public List<Gesture> infoList;
    }
    /// <summary>
    /// 读取Json文件
    /// </summary>
    public void ReadJson()
    {
        //string path = Application.dataPath + _path;
        //string str = File.ReadAllText(path, System.Text.Encoding.Default);
        dictGes = new Dictionary<string, string>();

        TextAsset ta = Resources.Load<TextAsset>("GestureKeys");
        //Debug.Log(ta);
        GestureJson jsonObject = JsonUtility.FromJson<GestureJson>(ta.text);

        //List<Gesture> list = JsonConvert.DeserializeObject<List<Gesture>>(str);
        foreach (var i in jsonObject.infoList )
        {
            //Debug.Log(i.key + i.value);
            dictGes.Add(i.key, i.value);
        }
    }

}
