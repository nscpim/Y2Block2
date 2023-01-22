using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using Oculus.Interaction.PoseDetection;


public class GameManager : MonoBehaviour
{
    public Animator fishAnimator;
    public TextMeshProUGUI fishCaughtText, didCaughtText;
    public int[] timesCompleted;
    private static Manager[] managers;
    public Gesture gestureDone;
    public Sprite[] gestureImages;
    public Canvas gestureUI;
    public TextMeshProUGUI gestureText;
    public Image gestureImage;
    public int gesture;
    private int fishCaught = 0;
    public GameObject[] leftHandPoses;
    public GameObject[] rightHandPoses;
    public GameObject handCheckPanel;
    public OVRHand[] hands;
    public GameObject[] handSelect;
    public GameObject bait;
   


    public static GameManager instance { get; private set; }
    public static T GetManager<T>() where T : Manager
    {
        for (int i = 0; i < managers.Length; i++)
        {
            if (typeof(T) == managers[i].GetType())
            {
                return (T)managers[i];
            }
        }
        return default(T);
    }
    GameManager()
    {
        instance = this;
        managers = new Manager[]
        {
           new AudioManager(),
        };
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Awake();
        }
    }

    private void Start()
    {
        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Start();
        }
        RandomizeGesture();

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Update();
        }
    }
    /// <summary>
    ///  Enum of all the gestures, being used for randomizing and showing the certain gestures. NOTE: Do not Touch.
    /// </summary>
    public enum GestureEnum
    {
        HoekVuist_Neutral,
        HoekVuist_Closed,
        HaakVuist,
        Opposition_Index,
        Opposition_Middle,
        Opposition_Ring,
        Opposition_Pinky,
        PaperPose,
        Thumb_Extension
    }

    public void RandomizeGesture()
    {
        gesture = UnityEngine.Random.Range(0, Enum.GetNames(typeof(GestureEnum)).Length);
        gestureImage.sprite = gestureImages[gesture];
        GestureEnum enumValue = (GestureEnum)gesture;
        gestureText.text = enumValue.ToString();
    }


    public void DetectGesture(int gestureInt)
    {
        if (gestureInt == gesture)
        {
            Bait baitScript = bait.GetComponent<Bait>();
            if (baitScript.canCatch)
            {
                didCaughtText.text = "You caught a fish!";
                timesCompleted[gestureInt]++;
                SaveJSONData((GestureEnum)gestureInt);
                fishAnimator.Play("play");
            }
            else
            {
                didCaughtText.text = "Miss, you nearly caught a fish!";
            }
        }
    }
    
    public void SaveJSONData(GestureEnum gesture)
    {
        Data data = new Data();
        data.name = gesture.ToString();
        data.count = timesCompleted[(int)gesture];
        data.description = string.Format("This is the {0} gesture and has been completed {1} time(s)!", gesture.ToString(), data.count);
        data.hasDone = "true";
        data.leftHandRotation = hands[0].transform.rotation;
        data.rightHandRotation = hands[1].transform.rotation;
        string json = JsonUtility.ToJson(data, true);
        File.AppendAllText(Application.dataPath + "/Tasks.json", json);
        AddCount(1);
       
        RandomizeGesture();
    }

    public void AddCount(int amount)
    {
        fishCaught += amount;
        fishCaughtText.text = "Vis gevangen: " + fishCaught.ToString();
    }

    public void SetCurrentGesture(Gesture gesture)
    {
        gestureDone = gesture;
    }

    public Gesture GetGestureDone()
    {
        return gestureDone;
    }

    public void LeftHandSelect(bool isLeft)
    {
        if (isLeft)
        {
            for (int i = 0; i < leftHandPoses.Length; i++)
            {
                leftHandPoses[i].SetActive(true);
            }
            Debug.Log("Chose Left as hand.");
        }
        else
        {
            for (int i = 0; i < rightHandPoses.Length; i++)
            {
                rightHandPoses[i].SetActive(true);
            }
            Debug.Log("Chose right as hand.");
        }
        handCheckPanel.SetActive(false);
        for (int i = 0; i < handSelect.Length; i++)
        {
            handSelect[i].SetActive(false);
        }
    }
}
