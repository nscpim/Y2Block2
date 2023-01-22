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
   public void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().CompareTag("Fish"))
        {
            canCatch = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Fish"))
        {
            canCatch = false;
        }
    }
}
