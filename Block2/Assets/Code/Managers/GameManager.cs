using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;


public class GameManager : MonoBehaviour
{
    //public Animator fishAnimator;

    //Text Objects
    public TextMeshProUGUI fishCaughtText, didCaughtText, handSelectText;
    //Array of ints for every gesture, keeps track of how many times a gesture has been completed
    public int[] timesCompleted;
    //Holds all of the managers
    private static Manager[] managers;
    public Gesture gestureDone;
    //Array of all the images of the gestures
    public Sprite[] gestureImages;
    //Animator of the hand doing the gestures
    public Animator gestureHand;
    //UI World space canvas for the gesture part
    public Canvas gestureUI;
    //Text being used to show which gesture has to be done
    public TextMeshProUGUI gestureText;
    //Image thats being used to set the gesture images
    public Image gestureImage;
    //Gesture int for randomizing gestures
    public int gesture;
    //int for showing the amount of total fish caught
    private int fishCaught = 0;
    //Left Hand Gestures Array to enable if the user wants to use the left hand
    public GameObject[] leftHandPoses;
    //Right Hand Gestures Array to enable if the user wants to use the right hand
    public GameObject[] rightHandPoses;
    //Panel object for the selection of the hand
    public GameObject handCheckPanel;
    //OVRHand array for getting hand data
    public OVRHand[] hands;
    //Object for selecting the hand the user wants to use
    public GameObject[] handSelect;
    //Area in the water that gets checked for fish
    public GameObject bait;
    //Objects the fish can swim to
    public Transform[] targets;
    //Current fish object in the scene
    public GameObject fish;
    //Fish prefab for respawning the fish
    public GameObject fishPrefab;

    //singletone instance of the gamemanager
    public static GameManager instance { get; private set; }

    /// <summary>
    /// Method to get a certain Manager
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
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
    /// <summary>
    /// GameManager Constructor for initalizing
    /// </summary>
    GameManager()
    {
        instance = this;
        managers = new Manager[]
        {
           new AudioManager(),
        };
    }
    /// <summary>
    /// Awake, gets called before start
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Awake();
        }
    }

    /// <summary>
    /// Start gets called before update
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Start();
        }
        RandomizeGesture();

    }

    /// <summary>
    /// GameLoop
    /// </summary>
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

    /// <summary>
    /// Method for randomizing the gesture the user must do and playing the right animation.
    /// </summary>
    public void RandomizeGesture()
    {
        gesture = UnityEngine.Random.Range(0, Enum.GetNames(typeof(GestureEnum)).Length);
        gestureImage.sprite = gestureImages[gesture];
        switch (gesture)
        {
            case 0:
                gestureHand.Play("HoekVuist_Neutral");
                break;
            case 1:
                gestureHand.Play("HoekVuist_Closed");
                break;
            case 2:
                gestureHand.Play("HaakVuist");
                break;
            case 3:
                gestureHand.Play("Opposition_Index");
                break;
            case 4:
                gestureHand.Play("Opposition_All");
                break;
            case 5:
                gestureHand.Play("Opposition_All");
                break;
            case 6:
                gestureHand.Play("Opposition_Pinky");
                break;
            case 7:
                gestureHand.Play("PaperPose");
                break;
            case 8:
                gestureHand.Play("Thumb_Extension");
                break;
            default:
                break;
        }
        GestureEnum enumValue = (GestureEnum)gesture;
        gestureText.text = enumValue.ToString();
    }

    /// <summary>
    /// Method that gets called from the Event Wrapper when a gesture is detected
    /// Detects if its the right gesture and if it is the right one saves it to a json file
    /// </summary>
    /// <param name="gestureInt"></param>
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
                fish.GetComponent<Fish>().Caught();
            }
            else
            {
                didCaughtText.text = "Miss, you nearly caught a fish!";
            }
        }
    }

    /// <summary>
    /// JSON File saving, fills in the correct data based on the gesture and appends to the existing files ore creates a new file if it doesnt exist.
    /// </summary>
    /// <param name="gesture"></param>
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

    /// <summary>
    /// Adds a amount of fishcaught
    /// </summary>
    /// <param name="amount"></param>
    public void AddCount(int amount)
    {
        fishCaught += amount;
        fishCaughtText.text = "Vis gevangen: " + fishCaught.ToString();
    }

    /// <summary>
    /// Obescure code
    /// </summary>
    /// <param name="gesture"></param>
    public void SetCurrentGesture(Gesture gesture)
    {
        gestureDone = gesture;
    }

    /// <summary>
    /// Obsecure Code
    /// </summary>
    /// <returns></returns>
    public Gesture GetGestureDone()
    {
        return gestureDone;
    }

    /// <summary>
    /// Selection Method for the hands.
    /// </summary>
    /// <param name="isLeft"></param>
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
        handSelectText.gameObject.SetActive(false);
        for (int i = 0; i < handSelect.Length; i++)
        {
            handSelect[i].SetActive(false);
        }
    }

    /// <summary>
    /// Method for spawning a new fish
    /// </summary>
    public void SpawnFish()
    {
        GameObject.Instantiate(fishPrefab, fishPrefab.transform.position, fishPrefab.transform.rotation);
    }

}
