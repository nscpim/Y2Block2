using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI fishCaughtText;

    private static Manager[] managers;
    public Gesture gestureDone;

    public Image[] gestureImages;
    public Canvas gestureUI;
    public TextMeshProUGUI gestureText;
    public Image gestureImage;
    

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
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Update();
        }
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
