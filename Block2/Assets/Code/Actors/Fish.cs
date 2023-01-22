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
    public Animator animator;
    // Start is called before the first frame update
    public void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
        transform.Rotate(new Vector3(-90,-90, 0));

        if (transform.position == newTarget.position)
        {
            isMoving = false;
        }
    }

    public void Caught() 
    {
    
    
    }




}
