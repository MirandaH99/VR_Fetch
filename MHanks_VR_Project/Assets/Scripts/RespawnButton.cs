using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnButton : InteractableObject
{
    public GameObject boxPrefab;
    public GameObject RespawnPoint;
    RespawnArea spawn;
    private void Start()
    {
        spawn = GameObject.Find("ObjectSpawn").GetComponentInParent<RespawnArea>();
    }

    public override void OnInteract()
    {
        base.OnInteract();

        if (SpawnIsClear())
        {
            GameObject newBox = Instantiate(boxPrefab, RespawnPoint.transform.position, RespawnPoint.transform.rotation);
        }
    }

    public bool SpawnIsClear()
    {
        return spawn.spawnIsClear; 
    }
}
