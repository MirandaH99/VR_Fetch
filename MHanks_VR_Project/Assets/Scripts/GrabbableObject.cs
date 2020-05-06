using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : TargetableObject
{
    public bool isGrabbed = false;
    public GameObject holdingController = null;
}
