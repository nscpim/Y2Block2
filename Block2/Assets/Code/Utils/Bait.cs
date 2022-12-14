using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bait : MonoBehaviour
{

    public bool canCatch;

    // Start is called before the first frame update
    void Start()
    {
        
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
            //Show Which hand Gesture user must perform
        }
        if (collision.collider.transform.CompareTag("Fish") && canCatch)
        {
            collision.gameObject.GetComponent<Fish>().CatchFish();
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
