using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcRender : MonoBehaviour
{
    public LineRenderer lr;
    // https://docs.unity3d.com/ScriptReference/LineRenderer.html
    // https://docs.unity3d.com/ScriptReference/LineRenderer.SetPositions.html

    public GameObject StartObject;
    public Vector3 EndObject; 
    public float MidPointHeight = 2f;


    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        lr.widthMultiplier = 0.1f;
        lr.positionCount = 9; 

    }

    // Update is called once per frame
    void Update()
    {
        BuildArc();
    }
    public void BuildArc()
    {
        Vector3[] points = new Vector3[9];

        // Yes, this is brute forced. 
        // But if you want to do the math to make this better
        // go ahead. 

        points[0] = StartObject.transform.position;
        points[8] = EndObject;

        points[4] = MidPointHeightLocation(points[0], points[8], 4, MidPointHeight); 

        points[2] = MidPointHeightLocation(points[0], points[8], 2,  (MidPointHeight * .7f));
        points[6] = MidPointHeightLocation(points[0], points[8], 6, (MidPointHeight * .7f));

        points[1] = MidPointHeightLocation(points[0], points[8], 1, (MidPointHeight * .4f));
        points[3] = MidPointHeightLocation(points[0], points[8], 3, (MidPointHeight * .9f));
        points[5] = MidPointHeightLocation(points[0], points[8], 5, (MidPointHeight * .9f));
        points[7] = MidPointHeightLocation(points[0], points[8], 7, (MidPointHeight * .4f));

        lr.SetPositions(points);
    }

    public Vector3 MidPointHeightLocation (Vector3 a, Vector3 b, int p, float h)
    {
        Vector3 d = (b - a) / 8;
        d *= p;
        d.y += h;
        return (a + d); 
      
    }
}
