using UnityEngine;
using System.Collections;

public class MouseMove : MonoBehaviour
{
    public Camera cam;

    void Update()
    {
        //Debug.Log("Move");

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

        if (Physics.Raycast(ray, out hit, 100)) 
        {
            gameObject.transform.position = hit.point;

            // Do something with the object that was hit by the raycast.
        }
    }
}