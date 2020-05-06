using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnArea : MonoBehaviour
{
    public bool spawnIsClear = false;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.GetComponentInParent<GrabbableObject>() is GrabbableObject)
        {
            spawnIsClear = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        spawnIsClear = true;
    }
   
}
