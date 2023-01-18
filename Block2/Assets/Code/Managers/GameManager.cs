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
    public OVRSkeleton skeleton;
    public List<OVRBone> bones;
    public float strengthNumber;
    public Animator fishAnimator;
    public TextMeshProUGUI fishCaughtText;
    public int[] timesCompleted;
    private static Manager[] managers;
    public Gesture gestureDone;
    public Sprite[] gestureImages;
    public Canvas gestureUI;
    public TextMeshProUGUI gestureText;
    public Image gestureImage;
    public int gesture;
    private int fishCaught = 0;
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
        bones = new List<OVRBone>(skeleton.Bones);
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
    public enum GestureEnum
    {
        HoekVuist,
        HaakVuist,
        WristFlexion
    }

    public void RandomizeGesture()
    {
        gesture = UnityEngine.Random.Range(0, Enum.GetNames(typeof(GestureEnum)).Length);
        gestureImage.sprite = gestureImages[gesture];
    }


    public void DetectGesture(int gestureInt)
    {
        if (gestureInt == gesture)
        {
            timesCompleted[gestureInt]++;
            SaveJSONData((GestureEnum)gestureInt);
        }

    }

    public void GatherObject(ShapeRecognizerActiveState shapeObject)
    {
        var hand = shapeObject.Hand;
        strengthNumber = hand.GetFingerPinchStrength(Oculus.Interaction.Input.HandFinger.Index);
    }


    public void SaveJSONData(GestureEnum gesture)
    {
        Data data = new Data();
        data.name = gesture.ToString();
        data.count = timesCompleted[(int)gesture];
        data.description = string.Format("This is the {0} gesture and has been completed {1} time(s)!", gesture.ToString(), data.count);
        data.strength = strengthNumber;
        data.bones = new List<Vector3>();
        foreach (var bone in bones)
        {
            data.bones.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }
        data.hasDone = "true";
        string json = JsonUtility.ToJson(data, true);
        File.AppendAllText(Application.dataPath + "/Tasks.json", json);
        AddCount(1);
        fishAnimator.Play("play");
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
}
