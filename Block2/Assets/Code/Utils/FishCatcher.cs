using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCatcher : MonoBehaviour
{
    public Timer fishTimer = new Timer();
    public Timer fishCatch = new Timer();
    public float timeToCatchFish;
    public float randomIntervalMin;
    public float randomIntervalMax;
    public Gesture currentRandomGesture;
    public bool canCatch;
    public GestureDetection gestureDetection;
   

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (fishTimer.isActive && fishTimer.TimerDone())
        {
            fishTimer.StopTimer();
            fishCatch.SetTimer(timeToCatchFish);
        }

        if (fishCatch.isActive && fishCatch.TimerDone())
        {
            fishCatch.StopTimer();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Water")
        {
            canCatch = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Water")
        {
            if (canCatch)
            {
                float randomValue = Random.Range(randomIntervalMin, randomIntervalMax);
                fishTimer.SetTimer(randomValue);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Water")
        {
            canCatch = false; 
        }
    }

    public void CheckForSameGesture(Gesture gesture) 
    {

        

    }


}
