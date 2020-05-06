using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        DaBox b = other.gameObject.GetComponentInParent<DaBox>();
        if(b)
        {
            Player.Instance.AddScore();
        }
    }
}
