using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollider : MonoBehaviour
{
    BallScript bs;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        bs = other.gameObject.GetComponent<BallScript>();
        if (bs)
        {
            if (bs.currentState == BallScript.ToyStates.Thrown)
            {
                bs.currentState = BallScript.ToyStates.Retrievable;
            }
        }else
        {
            return;
        }
    }
}
