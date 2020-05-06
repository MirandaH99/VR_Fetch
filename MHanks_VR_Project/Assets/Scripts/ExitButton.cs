using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : InteractableObject
{
    public override void OnInteract()
    {
        base.OnInteract();

        Debug.Log("Quit Applicaiton");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

#if UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
#endif

        Application.Quit();
    }

}
