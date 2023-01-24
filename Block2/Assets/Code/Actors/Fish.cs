using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fish : MonoBehaviour
{
    //Bool if the fish is moving
    public bool isMoving = false;
    //Speed of the fish
    public float speed = 5f;
    //Current Target Location
    private Transform newTarget;
    //Animator of the fish
    public Animation anim;

    // Start is called before the first frame update
    public void Start()
    {
        GameManager.instance.fish = gameObject;
        anim = gameObject.GetComponent<Animation>();       
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    /// <summary>
    /// Movement code for the fish
    /// </summary>
    public void Move()
    {
        if (!isMoving)
        {
            newTarget = GameManager.instance.targets[Random.Range(0, GameManager.instance.targets.Length)];
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
    /// <summary>
    /// Method for when a fish is caught
    /// </summary>
    public void Caught() 
    {
        anim.Play();
        Destroy(gameObject, 0.5f); 
        GameManager.instance.SpawnFish();
    }






}
