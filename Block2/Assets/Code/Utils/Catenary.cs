using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catenary : MonoBehaviour
{

    public float wireRadius = 0.02f;
    public float wireCatenary = 10f;
    public float wireResolution = 0.1f;
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

    [ContextMenu("Regen")]
    public virtual void Regenerate() 
    {
        float distance = Vector3.Distance(rod.position, bait.position);
        int nPoints = (int)(distance / wireResolution + 1);

        Vector3[] wirePoints = new Vector3[nPoints];
        wirePoints[0] = rod.position;
        wirePoints[nPoints - 1] = bait.position;

        Vector3 dir = (bait.position - rod.position).normalized;
        float offset = CalculateCatenary(wireCatenary, -distance / 2);


        for (int i = 1; i < nPoints - 1; ++i)
        {
            Vector3 wirePoint = rod.position + i * wireResolution * dir;

            float x = i * wireResolution - distance / 2;
            wirePoint.y = wirePoint.y - (offset - CalculateCatenary(wireCatenary, x));

            wirePoints[i] = wirePoint;
        }
        GenerateWithLine(wirePoints);
    }

    private void GenerateWithLine(Vector3[] wirePoints) 
    {
        LineRenderer line = GetComponent<LineRenderer>();

        line.positionCount = wirePoints.Length;

        for (int i = 0; i < wirePoints.Length; ++i)
        {
            line.SetPosition(i, wirePoints[i]);
        }
    }

}

