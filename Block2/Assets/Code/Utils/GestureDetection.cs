using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;


[System.Serializable]
public struct Data
{
    public string name;
    public string description;
    public string hasDone;
    public int count;
    public Quaternion leftHandRotation;
    public Quaternion rightHandRotation;    
}

[System.Serializable]
public struct Gesture
{
    //DATA STRUCT NO LONGER IN USE.
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
    public float leftHandPinchStrength;
    public float rightHandPinchStrength;

}

public class GestureDetection : MonoBehaviour
{

    //CLASS NO LONGER BEING USED, RIP MY CODE.

    public List<float> pinchStrengthsLeft = new List<float>();
    public List<float> pinchStrengthsRight = new List<float>();
    public float threshold = 0.1f;
    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    private List<OVRBone> fingerBones;
    private Gesture previousGesture;

    // Start is called before the first frame update
    public void Start()
    {
       
        fingerBones = new List<OVRBone>(skeleton.Bones);
        previousGesture = new Gesture();
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveData();
        }

        


        Gesture currentGesture = Recognize();
        bool hasRecognized = !currentGesture.Equals(new Gesture());
        if (hasRecognized && !currentGesture.Equals(previousGesture))
        {

            GameManager.instance.SetCurrentGesture(currentGesture);
            previousGesture = currentGesture;

            currentGesture.onRecognized.Invoke();

        }
    }

    /// <summary>
    /// Saving data to a JSON File
    /// </summary>
    public void SaveData()
    {
        Gesture g = new Gesture();
        g.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();
        foreach (var bone in fingerBones)
        {
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }
        g.fingerDatas = data;
        gestures.Add(g);
    }

    /// <summary>
    /// Randomize the gesture
    /// </summary>
    /// <returns></returns>
    public Gesture GetRandomGesture()
    {
        Gesture[] gestureArray = gestures.ToArray();
        int randomGesture = Random.Range(0, gestureArray.Length);
        return gestureArray[randomGesture];
    }

    /// <summary>
    /// Recognizes the gesture by an offset, this is in the update loop
    /// </summary>
    /// <returns></returns>
    Gesture Recognize()
    {
        Gesture currentGesture = new Gesture();
        float currentMin = Mathf.Infinity;
        /* currentGesture.leftHandPinchStrength = skeleton.GetComponent<OVRHand>().GetFingerPinchStrength(OVRHand.HandFinger.Index);
         currentGesture.rightHandPinchStrength = skeleton.GetComponent<OVRHand>().GetFingerPinchStrength(OVRHand.HandFinger.Index);

         pinchStrengthsLeft.Add(currentGesture.leftHandPinchStrength);
         pinchStrengthsRight.Add(currentGesture.rightHandPinchStrength);*/
        foreach (var gesture in gestures)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]);
                if (distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }

                sumDistance += distance;
            }

            if (!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentGesture = gesture;
                SaveJSONData(currentGesture);

            }
        }
        return currentGesture;
    }

    /// <summary>
    /// Saving the data to a json file
    /// </summary>
    /// <param name="gesture"></param>
    public void SaveJSONData(Gesture gesture)
    {
        Data data = new Data();
        data.name = gesture.name;
    
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/DoctorsOnly.json", json);
    }



}
