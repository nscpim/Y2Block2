using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fish : MonoBehaviour
{
    public Transform[] targets;
    public bool isMoving = false;
    public float speed = 5f;

   
  


    private Transform newTarget;
    private GestureDetection gestureDetection;
    private Gesture randomGesture;
    // Start is called before the first frame update
    public void Start()
    {
        gestureDetection = GameManager.instance.gameObject.GetComponent<GestureDetection>();
      
     
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    public void CatchFish()
    {
        if (GameManager.instance.GetGestureDone().name == randomGesture.name)
        {
            randomGesture = gestureDetection.gestures[Random.Range(0, gestureDetection.gestures.Count)];
            //Update which gesture the user must do
            Debug.Log("Caught Fish");
        }
    }
    public void Move()
    {
        if (!isMoving)
        {
            newTarget = targets[Random.Range(0, targets.Length)];
            isMoving = true;
        }
        transform.position = Vector3.MoveTowards(transform.position, newTarget.position, speed * Time.deltaTime);
        transform.LookAt(newTarget);

        if (transform.position == newTarget.position)
        {
            isMoving = false;
        }


    }

   


}
