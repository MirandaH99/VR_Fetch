// Generic Rotation Script 
// NHTI AGGP 
using UnityEngine;

public class Spin : MonoBehaviour
{
    public Vector3 RotationRate = Vector3.zero;
    public float DegreesPerSecond = 90f; 
    void Update()
    {
        gameObject.transform.Rotate(RotationRate * DegreesPerSecond * Time.deltaTime); 
    }
}
