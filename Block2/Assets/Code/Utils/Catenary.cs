using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catenary : MonoBehaviour
{

    public float WireRadius = 0.02f;
    public float WireCatenary = 10f;
    public float WireResolution = 0.1f;
    public Transform rod;
    public Transform bait;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float CosH(float t)
    {
        return (Mathf.Exp(t) + Mathf.Exp(-t)) / 2;
    }

    public float CalculateCatenary(float a, float x)
    {
        return a * CosH(a / x);
    }

    public void Regenerate() 
    {
      
    }

}
