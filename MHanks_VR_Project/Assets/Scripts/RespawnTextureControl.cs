using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RespawnTextureControl : MonoBehaviour
{
    public Material normalTexture;
    public Material hoverTexture;
    public Material confirmTexture;

    void Start()
    {
        if (!normalTexture || !hoverTexture || !confirmTexture)
        {
            Debug.LogError("There are materials not set on the RespawnTextureControl script on Respawn Object in scene."); 
            return; 
        }
        OnHoverEnd(); 
    }
    private void SwitchMaterial(Material m)
    {
        MeshRenderer MR = gameObject.GetComponent<MeshRenderer>(); 
        Material[] mats = MR.materials;
        mats[1] = m;
        MR.materials = mats; 
    }

    public void OnHoverStart()
    {
        SwitchMaterial(hoverTexture);
    }

    public void OnHoverEnd()
    {
        SwitchMaterial(normalTexture);
    }

    public void OnConfirm()
    {
        SwitchMaterial(confirmTexture);
    }

    // Debug
    // Testing of Material Switching
    /*
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            OnHoverStart(); 
        }
        else
        {
            OnHoverEnd(); 
        }
    }
   */
}
