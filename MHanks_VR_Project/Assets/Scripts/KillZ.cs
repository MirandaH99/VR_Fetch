using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZ : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Actor b = other.gameObject.GetComponentInParent<Actor>();
        BallScript bs = other.gameObject.GetComponentInParent<BallScript>();
        Player p = other.gameObject.GetComponentInParent<Player>();
        if (b)
        {
            Debug.Log(b.gameObject.name + " fell into Kill Z");
            b.DestroyActor(); 
        }
        if(p)
        {
            p.transform.position = Player.Instance.startPosition;
        }
        if(bs)
        {
            bs.transform.position = bs.startPosition;
        }
        Player.Instance.MinusScore();
    }
}
