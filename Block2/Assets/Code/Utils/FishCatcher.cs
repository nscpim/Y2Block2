using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCatcher : MonoBehaviour
{
    public Timer fishTimer = new Timer();
    public float randomIntervalMin;
    public float randomIntervalMax;
    private bool canReset = false;

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

            


        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Water")
        {
            canReset = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Water")
        {
            if (canReset)
            {
                canReset = false;
                float randomValue = Random.Range(randomIntervalMin, randomIntervalMax);
                fishTimer.SetTimer(randomValue);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Water")
        {
            canReset = false; 
        }
    }

}
