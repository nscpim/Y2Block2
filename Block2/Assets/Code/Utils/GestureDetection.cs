using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Gesture
{

    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
    public float leftHandPinchStrength;
    public float rightHandPinchStrength;

}

public class GestureDetection : MonoBehaviour
{
    public List<float> pinchStrengthsLeft = new List<float>();
    public List<float> pinchStrengthsRight = new List<float>();
    public float threshold = 0.1f;
    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    public List<OVRBone> fingerBones;
    private Gesture previousGesture;

    // Start is called before the first frame update
    void Start()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);
        previousGesture = new Gesture();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveData();
        }

        Gesture currentGesture = Recognize();
        bool hasRecognized = !currentGesture.Equals(new Gesture());
        if (hasRecognized && !currentGesture.Equals(previousGesture))
        {
            previousGesture = currentGesture;
            currentGesture.onRecognized.Invoke();

        }
    }
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

    public Gesture GetRandomGesture()
    {
        Gesture[] gestureArray = gestures.ToArray();
        int randomGesture = Random.Range(0, gestureArray.Length);
        return gestureArray[randomGesture];
    }

    Gesture Recognize()
    {
        Gesture currentGesture = new Gesture();
        float currentMin = Mathf.Infinity;
        currentGesture.leftHandPinchStrength = skeleton.GetComponent<OVRHand>().GetFingerPinchStrength(OVRHand.HandFinger.Index);
        currentGesture.rightHandPinchStrength = skeleton.GetComponent<OVRHand>().GetFingerPinchStrength(OVRHand.HandFinger.Index);

        pinchStrengthsLeft.Add(currentGesture.leftHandPinchStrength);
        pinchStrengthsRight.Add(currentGesture.rightHandPinchStrength);
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
            }
        }
        return currentGesture;
    }

}
