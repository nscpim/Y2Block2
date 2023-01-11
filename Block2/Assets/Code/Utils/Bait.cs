using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bait : MonoBehaviour
{
    public bool canCatch;
    private Timer fishCatchDuration;
        
    // Start is called before the first frame update
    void Start()
    {
        fishCatchDuration = new Timer();
    }

    // Update is called once per frame
    void Update()
    {
             
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.CompareTag("Water"))
        {
            canCatch = true;
           
        }
        if (collision.collider.transform.CompareTag("Fish") && canCatch)
        {
            fishCatchDuration.SetTimer(5f);
            //Show Which hand Gesture user must performs
            //Update the UI aswell

            if (fishCatchDuration.TimerDone() && fishCatchDuration.isActive)
            {
                collision.gameObject.GetComponent<Fish>().CatchFish();

            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.transform.CompareTag("Water"))
        {
            canCatch = false;
        }
    }
}
