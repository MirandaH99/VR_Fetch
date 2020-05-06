using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraitLineRender : MonoBehaviour
{
    public LineRenderer lr;
   

    public GameObject StartObject;
    public Vector3 EndObject;
    

    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        lr.widthMultiplier = 0.1f;
        lr.positionCount = 2;

    }

    // Update is called once per frame
    void Update()
    {
        //BuildArc();
    }
    public void BuildArc()
    {
        Vector3[] points = new Vector3[2];

        // Yes, this is brute forced. 
        // But if you want to do the math to make this better
        // go ahead. 

        points[0] = StartObject.transform.position;
        points[1] = EndObject;

     
        lr.SetPositions(points);
    }

}